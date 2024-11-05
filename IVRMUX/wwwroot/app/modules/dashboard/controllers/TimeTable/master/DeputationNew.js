
(function () {
    'use strict';
    angular
        .module('app')
        .controller('DeputationNewController', DeputationNewController)
    DeputationNewController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter', 'Excel', '$timeout']
    function DeputationNewController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, Excel, $timeout) {
        $scope.editEmployee = {};
        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.grid_view = false;
        $scope.abs = false;

        $scope.options = {
            hstep: [1, 2, 3],
            mstep: [1, 5, 10, 15, 25, 30]
        };
        $scope.currentPage = 1;
        $scope.itemsPerPage = 15;
        $scope.itemsPerPage1 = 10;
        $scope.ismeridian = true;
        $scope.mainstafflist = [];
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };

        $scope.isOptionsRequired = function () {

            return !$scope.staff_list.some(function (options) {
                return options.stf;
            });
        }
        $scope.rpttyp = "Name";
        $scope.onclickloaddata = function () {
            $scope.loaddefault = false;
            $scope.loaddefault1 = false;
            $scope.HRME_Id = "";
            $scope.stf_dy_pds = false;
            $scope.stf_fr_pds = false;
        }

        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };

        $scope.changesbs = function () {
            $scope.HRME_Id = "";
            $scope.loaddefault = false;
            $scope.loaddefault1 = false;

            $scope.stf_dy_pds = false;
            $scope.stf_fr_pds = false;
            $scope.mainstafflist = [];
            $scope.staff_list = [];

            if ($scope.asmaY_Id == "" || $scope.myDate == "" || $scope.myDate == undefined || $scope.asmaY_Id == undefined || $scope.myDate == null || $scope.asmaY_Id == null) {
                swal("Please Select Academic Year And Date");
                $scope.HRME_Id = "";
                $scope.abs = false;
            }
            else {
                var TTSD_Date = new Date($scope.myDate).toDateString();
              
                var data = {

                    "ASMAY_Id": $scope.asmaY_Id,
                    "TTSD_Date": TTSD_Date,
                    "absflag": $scope.abs,
                }
                apiService.create("DeputationNew/getabsentstaff", data).
                    then(function (promise) {

                        $scope.staff_list = promise.stafflist;
                        //$scope.staff_list = [];
                        //for (var m = 0; m < promise.stafflist.length; m++) {
                        //    var stf_chg = { hrmE_Id: promise.stafflist[m].hrmE_Id, staffName: promise.stafflist[m].staffName, hrmE_EmployeeCode: promise.stafflist[m].hrmE_EmployeeCode, stf_name: promise.stafflist[m].staffName, stf_code: promise.stafflist[m].hrmE_EmployeeCode, stf_code_name: promise.stafflist[m].hrmE_EmployeeCode + ':' + promise.stafflist[m].staffName };
                        //    $scope.staff_list.push(stf_chg);
                        //}
                    })
            }
        }



        var abst_asmcL_Id = "";
        var abst_asmS_Id = "";
        var abst_ttmD_Id = "";
        var abst_ttmP_Id = "";
        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function (all) {
            $scope.submitted = true;
            $scope.classwisesubstituted = [];
            if ($scope.myForm.$valid) {
            



                var TTSD_Date = new Date($scope.myDate).toDateString();


                angular.forEach($scope.staff_day_periods, function (user) {


                    $scope.classwisesubstituted.push({

                        ttmD_Id: user.ttmD_Id,
                        ttmP_Id: user.ttmP_Id,
                       
                        asmaY_Id: user.asmaY_Id,
                        asmcL_Id: user.asmcL_Id,
                        asmS_Id: user.asmS_Id,
                       
                        TTSD_DeputedStaff: user.Sub_Id

                    })
          
                })



        
                    var data = {
                        "TTSD_Id": $scope.TTSD_Id,
                        "ASMAY_Id": $scope.asmaY_Id,
                        "TTSD_Date": TTSD_Date,
                        //"ASMCL_Id": abst_asmcL_Id,
                        //"ASMS_Id": abst_asmS_Id,
                        //"TTMD_Id": abst_ttmD_Id,
                        //"TTMP_Id": abst_ttmP_Id,
                        "TTSD_AbsentStaff": $scope.HRME_Id.HRME_Id,
                        //"TTSD_DeputedStaff": deput_hrmE_Id,
                        "TTSD_Remarks": $scope.remarks,
                        "smsflag": $scope.SMS_Flag,
                        "mailflag": $scope.EMAIL_Flag,
                        "NOT_Flag": $scope.NOT_Flag,
                       
                        subarray:$scope.classwisesubstituted

                    }
                    apiService.create("DeputationNew/savedetails", data).
                        then(function (promise) {
                            if (promise.returnval === true) {
                                swal('Data successfully Saved');
                            }
                            else {
                                swal('Data Not Saved !');
                            }
                            $scope.BindData();
                        })
                    $scope.clearid();
               
            }
            else {
                $scope.submitted = true;

            }

        };
        //TO  GEt The Values iN Grid
        $scope.BindData = function () {
            $scope.HRME_Id = "";
            $scope.abs = false;
            $scope.staff_list = [];

            $scope.rpttyp = "Name";
            apiService.getDATA("DeputationNew/getdetails").
                then(function (promise) {
                    $scope.year_list = promise.acayear;
                    $scope.category_list = promise.categorylist;
                    $scope.class_list = promise.classlist;
                    $scope.temp_classlist = promise.classlist;
                    $scope.staff_list = promise.stafflist;
                    $scope.day_list = promise.daylist;
                    $scope.myDate = new Date();

                    $scope.get($scope.myDate);
                    //for (var m = 0; m < promise.stafflist.length; m++) {
                    //    var stf_chg = { hrmE_Id: promise.stafflist[m].hrmE_Id, staffName: promise.stafflist[m].staffName, hrmE_EmployeeCode: promise.stafflist[m].hrmE_EmployeeCode, stf_name: promise.stafflist[m].staffName, stf_code: promise.stafflist[m].hrmE_EmployeeCode, stf_code_name: promise.stafflist[m].hrmE_EmployeeCode + ':' + promise.stafflist[m].staffName };
                    //    $scope.staff_list.push(stf_chg);
                    //}

                })
        };
        $scope.all_check = function () {
            if ($scope.usercheck == "1") {
                angular.forEach($scope.staff_list, function (role) {
                    role.stf = true;
                })
                $scope.stf_flag = true;
            }
            else if ($scope.usercheck == "0") {
                angular.forEach($scope.staff_list, function (role) {
                    role.stf = false;
                })
                $scope.stf_flag = false;
            }
        }
        $scope.stf_fr_pds = false;

        $scope.cell_click = function (dayid, periodid, day, period, stfid, yrid, clsid, secid, TTFGDC_Id, user) {

            //angular.forEach($scope.staff_day_periods, function (ff) {
            //    if (ff.ttfgD_Id == TTFGDC_Id) {
            //        ff.clr = 'green'
            //    }
            //    else {
            //        ff.clr = ''
            //    }

            //})


            $scope.loaddefault1 = false;
            $scope.dpcount = [];
            abst_asmcL_Id = clsid;
            abst_asmS_Id = secid;
            abst_ttmD_Id = dayid;
            abst_ttmP_Id = periodid;
            var TTSD_Date = new Date($scope.myDate).toDateString();
            var data = {
                "ASMAY_Id": yrid,
                "TTMD_Id": dayid,
                "TTMP_Id": periodid,
                "HRME_Id": stfid,
                "TTSD_Date": TTSD_Date,
            }
            apiService.create("DeputationNew/get_free_stfdets", data).
                then(function (promise) {
                    var freeperiodweekly = 0;
                    var freeperioddaily = 0;
                    if (promise.time_Table_substitute != "" && promise.time_Table_substitute != null) {
                        $scope.stf_fr_pds = true;
                        $scope.loaddefault1 = true;
                    }

                   freeperiodweekly = promise.freeperiodweekly.length;
                     freeperioddaily = promise.freeperioddaily.length;


                    $scope.mainstafflist = [];

                    angular.forEach(promise.time_Table_substitute, function (eps) {
                        if ($scope.mainstafflist.length == 0) {
                            $scope.mainstafflist.push({ HRME_Id: eps.hrmE_Id, STAFF: eps.staffName, EMPCODE: eps.hrmE_EmployeeCode, DCNT: eps.DCNT, WCNT: eps.WCNT, MCNT: eps.MCNT, YCNT: eps.YCNT, asmaY_Id: eps.asmaY_Id, deviceid: eps.deviceid, freeperioddaily: freeperioddaily, freeperiodweekly: freeperiodweekly });
                        }
                        else if ($scope.mainstafflist.length > 0) {
                            var al_exm_cnt = 0;
                            angular.forEach($scope.mainstafflist, function (exm) {
                                if (exm.HRME_Id == eps.hrmE_Id) {
                                    al_exm_cnt += 1;
                                }
                            })
                            if (al_exm_cnt == 0) {
                                $scope.mainstafflist.push({ HRME_Id: eps.hrmE_Id, STAFF: eps.staffName, EMPCODE: eps.hrmE_EmployeeCode, DCNT: eps.DCNT, WCNT: eps.WCNT, MCNT: eps.MCNT, YCNT: eps.YCNT, asmaY_Id: eps.asmaY_Id, deviceid: eps.deviceid });
                            }
                        }
                    })



                    $scope.dpcount = promise.dpcount;
                    $scope.absentstfcnt = promise.absentstfcnt;
                    $scope.weeklycntlist = promise.weeklycntlist;
                    angular.forEach($scope.mainstafflist, function (dd) {
                        //depute count
                        angular.forEach($scope.dpcount, function (xx) {
                            if (dd.HRME_Id == xx.HRME_Id) {

                                dd.weekcnt = xx.WeekStaffcount;
                                dd.mntcnt = xx.MonthStaffcount;
                                dd.totalcnt = xx.YearStaffcount;
                                dd.daycnt = xx.TodayStaffcount;

                            }

                        })

                        //absent count
                        angular.forEach($scope.absentstfcnt, function (xx) {
                            if (dd.HRME_Id == xx.HRME_Id) {
                                dd.weekcnt1 = xx.WeekStaffcount;
                                dd.mntcnt1 = xx.MonthStaffcount;
                                dd.totalcnt1 = xx.YearStaffcount;
                                dd.daycnt1 = xx.TodayStaffcount;

                            }

                        })


                        angular.forEach($scope.weeklycntlist, function (xx) {
                            if (dd.HRME_Id == xx.HRME_Id) {
                                dd.WWCNT = xx.TPCOUNT;
                            }

                        })

                    })



                    $scope.periodslst = promise.periodslst;
                    $scope.ttlistdata = promise.time_Table_substitute;

                    angular.forEach($scope.mainstafflist, function (zz) {

                        var templist = [];

                        angular.forEach($scope.periodslst, function (pp) {
                            var clsdetails = '';
                            var subdetails = '';

                            angular.forEach($scope.ttlistdata, function (tt) {
                                if (tt.hrmE_Id == zz.HRME_Id && tt.ttmP_Id == pp.ttmP_Id) {

                                    if (clsdetails == '') {
                                        clsdetails = tt.asmcL_ClassName + '&' + tt.asmC_SectionName;
                                    }
                                    else {
                                        clsdetails = clsdetails + ' ' + 'AND' + ' ' + tt.asmcL_ClassName + '&' + tt.asmC_SectionName;
                                    }

                                    if (subdetails == '') {
                                        subdetails = tt.ismS_SubjectName;
                                    }
                                    else {
                                        if (subdetails != tt.ismS_SubjectName) {
                                            subdetails = subdetails + ' ' + 'AND' + ' ' + tt.ismS_SubjectName;
                                        }

                                    }

                                }

                            })

                            if (clsdetails != '' && subdetails != '') {
                                templist.push({ TTMP_Id: pp.ttmP_Id, PR: 'P--' + pp.ttmP_PeriodName, CLS: clsdetails, SUB: subdetails });
                            }


                        })

                        zz.list = templist;
                    })

                    console.log('eeeee');
                    console.log($scope.mainstafflist);

                    angular.forEach($scope.mainstafflist, function (dd) {
                        dd.DAYTL = dd.list.length;
                        var gg = 0;
                        var newlist = [];
                        angular.forEach(dd.list, function (xx) {
                            gg += 1;
                            if (gg == 1) {
                                dd.PR = xx.PR;
                                dd.CLS = xx.CLS;
                                dd.SUB = xx.SUB;
                            }
                            else {
                                newlist.push(xx);
                            }

                        })
                        if (newlist.length > 0) {
                            dd.list = newlist;
                        }


                    })



                    console.log($scope.staff_free_periods);
                    if (promise.time_Table_substitute == "" || promise.time_Table_substitute == null) {
                        swal("No Staffs Are Free   For Selected Day !!!");
                        $scope.stf_fr_pds = false;
                        $scope.loaddefault1 = false;
                    }

                    //kavita

                    $scope.staff_day_periods.push({
                        ttmD_Id: user.ttmD_Id,
                        ttmP_Id: user.ttmP_Id,
                        ttmD_DayNam: user.ttmD_DayName,
                        ttmP_PeriodName: user.ttmP_PeriodName,
                        hrmE_Id: user.hrmE_Id,
                        asmaY_Id: user.asmaY_Id,
                        asmcL_Id: user.asmcL_Id,
                        asmS_Id: user.asmS_Id,
                        ttfgD_Id: user.ttfgD_Id,
                        ttmP_PeriodName: user.ttmP_PeriodName,
                        ismS_SubjectName: user.ismS_SubjectName,
                        asmC_SectionName: user.asmC_SectionName,
                        asmcL_ClassName: user.asmcL_ClassName,
                        substitutestafflist: $scope.mainstafflist




                    })


                })
        }

        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            angular.forEach($scope.students, function (itm) {
                itm.checkedvalue = toggleStatus;

            });
        }
        $scope.optionToggled = function (obj) {

            angular.forEach($scope.mainstafflist, function (itm) {
                if (obj.HRME_Id == itm.HRME_Id) {
                    debugger;
                    var d = 0;
                    var w = 0;
                    var m = 0;
                    var y = 0;


                    if (itm.DCNT != null && itm.DCNT != undefined && itm.DCNT != '' && itm.DCNT != 0) {

                        if (parseInt(obj.daycnt) >= parseInt(itm.DCNT)) {
                            swal('Daily DeputationNew is Exceeds');
                            d += 1;
                        }
                    }
                    if (itm.WCNT != null && itm.WCNT != undefined && itm.WCNT != '' && itm.WCNT != 0) {

                        if (parseInt(obj.weekcnt) >= parseInt(itm.WCNT)) {
                            swal('Weekly DeputationNew is Exceeds');
                            w += 1;
                        }
                    }
                    if (itm.MCNT != null && itm.MCNT != undefined && itm.MCNT != '' && itm.MCNT != 0) {

                        if (parseInt(obj.mntcnt) >= parseInt(itm.MCNT)) {
                            swal('Monthly DeputationNew is Exceeds');
                            m += 1;
                        }
                    }

                    if (itm.YCNT != null && itm.YCNT != undefined && itm.YCNT != '' && itm.YCNT != 0) {

                        if (parseInt(obj.totalcnt) >= parseInt(itm.YCNT)) {
                            swal('Yearly DeputationNew is Exceeds');
                            y += 1;
                        }
                    }
                    if (d == 0 && w == 0 && m == 0 && y == 0) {
                        itm.checkedvalue = true;
                    }
                    else {
                        itm.checkedvalue = false;
                    }





                }
                else {
                    itm.checkedvalue = false;
                }
            });
        }
        $scope.dptdetails = [];
        $scope.viewrecordspopup3 = function (stfid, yrid, flg, stname) {
            debugger;
            $scope.staff_Name1 = stname;
            $scope.dptdetails = [];
            var TTSD_Date = new Date($scope.myDate).toDateString();
            var data = {
                "ASMAY_Id": yrid,
                "HRME_Id": stfid,
                "FLAG": flg,
                "TTSD_Date": TTSD_Date,
            }
            apiService.create("DeputationNew/viewdeputation", data).
                then(function (promise) {

                    $scope.dptdetails = promise.dptdetails;
                })

        }

        $scope.dptdetails = [];
        $scope.viewrecordspopup4 = function (stfid, flg, stname) {
            debugger;
            $scope.staff_Name2 = stname;
            $scope.dptdetails1 = [];
            var TTSD_Date = new Date($scope.myDate).toDateString();
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "HRME_Id": stfid,
                "FLAG": flg,
                "TTSD_Date": TTSD_Date,
            }
            apiService.create("DeputationNew/viewabsent", data).
                then(function (promise) {

                    $scope.dptdetails1 = promise.dptdetails;
                })

        }



        $scope.dptdetails = [];
        $scope.viewrecordspopup5 = function (stfid, yrid, flg, stname) {
            debugger;
            $scope.staff_Name2 = stname;
            $scope.dptdetails1 = [];
            var TTSD_Date = new Date($scope.myDate).toDateString();
            var data = {
                "ASMAY_Id": yrid,
                "HRME_Id": stfid,
                "FLAG": flg,
                "TTSD_Date": TTSD_Date,
            }
            apiService.create("DeputationNew/viewabsent", data).
                then(function (promise) {

                    $scope.dptdetails1 = promise.dptdetails;
                })

        }

        $scope.viewrecordspopup9 = function (stfid, yrid) {

            var data = {
                "ASMAY_Id": yrid,
                "HRME_Id": stfid,
                "TTMD_Id": $scope.ttmD_Id,

            }
            apiService.create("DeputationNew/viewrecordspopup9", data).
                then(function (promise) {
                    if (promise.periodslst != null && promise.periodslst != "" && promise.gridweeks != null && promise.gridweeks != "" && promise.time_Table != null && promise.time_Table != "") {
                        // $scope.grid_view = true;
                        $scope.period_list = promise.periodslst;
                        $scope.day_list1 = promise.gridweeks;
                        $scope.tt_list = promise.time_Table;
                        var temp_array = [];
                        $scope.table_list = [];
                        for (var j = 0; j < $scope.day_list1.length; j++) {
                            temp_array = [];
                            for (var i = 0; i < $scope.period_list.length; i++) {

                                var count = 0;
                                var newCol = "";
                                for (var k = 0; k < $scope.tt_list.length; k++) {

                                    if ($scope.tt_list[k].ttmP_Id == $scope.period_list[i].ttmP_Id && $scope.tt_list[k].ttmD_Id == $scope.day_list1[j].ttmD_Id) {

                                        if (count == 0) {
                                            newCol = { pedid: $scope.period_list[i].ttmP_Id, dayid: $scope.day_list1[j].ttmD_Id, value: $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].ismS_SubjectName, pedname: $scope.period_list[i].ttmP_PeriodName, dayname: $scope.day_list1[j].ttmD_DayName, color: 'black', value1: $scope.tt_list[k].staffName + ' :' + $scope.tt_list[k].ismS_SubjectName }
                                            count += 1;
                                        }
                                        else if (count > 0) {
                                            newCol.value = newCol.value + ' && ' + $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].ismS_SubjectName;
                                            newCol.value1 = newCol.value1 + ' && ' + $scope.tt_list[k].staffName + ' :' + $scope.tt_list[k].ismS_SubjectName;
                                            count += 1;
                                        }
                                    }
                                    $scope.staff_Name = $scope.tt_list[k].staffName;
                                }
                                if (newCol == "") {
                                    newCol = { pedid: $scope.period_list[i].ttmP_Id, dayid: $scope.day_list1[j].ttmD_Id, value: ' ', pedname: $scope.period_list[i].ttmP_PeriodName, dayname: $scope.day_list1[j].ttmD_DayName, color: 'black' }
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
                        // swal("No,TimeTable is Not Generated !!!!");
                        swal("TimeTable is Not Generated For Selected Details !!!!");
                        $scope.grid_view = false;
                        $scope.datareport = false;
                    }
                })

        };


        $scope.viewrecordspopup2 = function (stfid, yrid) {

            var data = {
                "ASMAY_Id": yrid,
                "HRME_Id": stfid,

            }
            apiService.create("DeputationNew/getalldetailsviewrecords2", data).
                then(function (promise) {
                    if (promise.periodslst != null && promise.periodslst != "" && promise.gridweeks != null && promise.gridweeks != "" && promise.time_Table != null && promise.time_Table != "") {
                        // $scope.grid_view = true;
                        $scope.period_list = promise.periodslst;
                        $scope.day_list1 = promise.gridweeks;
                        $scope.tt_list = promise.time_Table;
                        var temp_array = [];
                        $scope.table_list = [];
                        for (var j = 0; j < $scope.day_list1.length; j++) {
                            temp_array = [];
                            for (var i = 0; i < $scope.period_list.length; i++) {

                                var count = 0;
                                var newCol = "";
                                for (var k = 0; k < $scope.tt_list.length; k++) {

                                    if ($scope.tt_list[k].ttmP_Id == $scope.period_list[i].ttmP_Id && $scope.tt_list[k].ttmD_Id == $scope.day_list1[j].ttmD_Id) {

                                        if (count == 0) {
                                            newCol = { pedid: $scope.period_list[i].ttmP_Id, dayid: $scope.day_list1[j].ttmD_Id, value: $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].ismS_SubjectName, pedname: $scope.period_list[i].ttmP_PeriodName, dayname: $scope.day_list1[j].ttmD_DayName, color: 'black', value1: $scope.tt_list[k].staffName + ' :' + $scope.tt_list[k].ismS_SubjectName }
                                            count += 1;
                                        }
                                        else if (count > 0) {
                                            newCol.value = newCol.value + ' && ' + $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].ismS_SubjectName;
                                            newCol.value1 = newCol.value1 + ' && ' + $scope.tt_list[k].staffName + ' :' + $scope.tt_list[k].ismS_SubjectName;
                                            count += 1;
                                        }
                                    }
                                    $scope.staff_Name = $scope.tt_list[k].staffName;
                                }
                                if (newCol == "") {
                                    newCol = { pedid: $scope.period_list[i].ttmP_Id, dayid: $scope.day_list1[j].ttmD_Id, value: ' ', pedname: $scope.period_list[i].ttmP_PeriodName, dayname: $scope.day_list1[j].ttmD_DayName, color: 'black' }
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
                        // swal("No,TimeTable is Not Generated !!!!");
                        swal("TimeTable is Not Generated For Selected Details !!!!");
                        $scope.grid_view = false;
                        $scope.datareport = false;
                    }
                })

        };
        $scope.clearpopupgrid2 = function () {
            $scope.table_list = "";
        };

        // $scope.remarks = "";
        //TO clear  data
        $scope.clearid = function () {
            $scope.asmaY_Id = "";
            $scope.ttmC_Id = "";
            $scope.HRME_Id = "";
            $scope.ttmD_Id = "";
            $scope.myDate = "";
            $scope.SMS_Flag = false;
            $scope.EMAIL_Flag = false;
            $scope.remarks = "";
            $scope.rpttyp = "Name";
            $scope.submitted = false;
            $scope.loaddefault = false;
            $scope.loaddefault1 = false;
            $scope.stf_dy_pds = false;
            $scope.stf_fr_pds = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
            $scope.abs = false;
        };
        $scope.search = "";
        $scope.asmaY_Id = "";
        $scope.myDate = "";
        $scope.ttmD_Id = "";
        $scope.hrmE_Id = "";
        $scope.stf_dy_pds = false;
        $scope.loaddefault = false;
        $scope.loaddefault1 = false;
        $scope.absentdpcount = [];
        $scope.get_period_alloted = function () {
           
            $scope.staff_day_periods = [];
            $scope.absentdpcount = [];
            $scope.stf_fr_pds = false;
            $scope.mainstafflist = [];
            if ($scope.asmaY_Id == "" || $scope.myDate == "") {
                swal("Please Select Academic Year And Date");
                $scope.HRME_Id = "";
            }
            else if ($scope.asmaY_Id != "" && $scope.ttmD_Id != "" && $scope.HRME_Id != "" && $scope.HRME_Id.HRME_Id != "") {
                var TTSD_Date = new Date($scope.myDate).toDateString();
                var data = {
                    // "TTMC_Id": $scope.ttmC_Id1,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "TTMD_Id": $scope.ttmD_Id,
                    "HRME_Id": $scope.HRME_Id.HRME_Id,
                    "TTSD_Date": TTSD_Date,
                }
                apiService.create("DeputationNew/get_period_alloted", data).
                    then(function (promise) {

                        if (promise.time_Table != "" && promise.time_Table != null) {
                            $scope.stf_dy_pds = true;
                            $scope.loaddefault = true;
                            $scope.loaddefault1 = false;
                            $scope.absentdpcount = promise.absentdpcount;

                        }
                     //   $scope.staff_day_periods = promise.time_Table;

                        angular.forEach(promise.time_Table, function (user) {
                            
                            $scope.cell_click(user.ttmD_Id, user.ttmP_Id, user.ttmD_DayName, user.ttmP_PeriodName, user.hrmE_Id,
                                user.asmaY_Id, user.asmcL_Id, user.asmS_Id, user.ttfgD_Id, user);

                     

                        })



                        if (promise.time_Table == "" || promise.time_Table == null) {
                            swal("No periods Are Allocated To Selected Staff For Selected Day !!!");
                            $scope.stf_dy_pds = false;
                            $scope.loaddefault = false;
                            $scope.loaddefault1 = false;
                        }

                    })
            }
        }



        $scope.get = function (D) {

            var date12 = new Date(D);
            var day23 = $filter('date')(date12, 'EEE');
            var day123 = $filter('uppercase')(day23);
            var flag = 0;
            for (var i = 0; i < $scope.day_list.length; i++) {

                var day456 = $filter('uppercase')($scope.day_list[i].ttmD_DayName).substring(0, 3);
                // var day786=day456.substring(0, 1);
                if (day123 == day456) {

                    $scope.ttmD_Id = $scope.day_list[i].ttmD_Id;
                    flag = 1;

                }
            }
            if (flag == 0) {
                swal("Please Select Working Days of Week !!!");
                $scope.ttmD_Id = "";
                $scope.myDate = "";
            }
            if ($scope.asmaY_Id != "" && $scope.ttmD_Id != "" && $scope.HRME_Id != "" && $scope.HRME_Id.HRME_Id != undefined && $scope.HRME_Id.HRME_Id != "") {
                $scope.get_period_alloted();
            }
        }

        $scope.getStudentBYYear = function (iddata) {

            for (var k = 0; k < $scope.year_list.length; k++) {

                if ($scope.year_list[k].asmaY_Id == iddata) {

                    var data = $scope.year_list[k].asmaY_Year;

                }

            }

            if (data != null) {

                console.log(data);
                var name, name1;
                for (var i = 0; i < data.length; i++) {
                    if (i < 4) {
                        if (i == 0) {
                            name = data[i];
                        } else {
                            name += data[i];
                        }
                    }
                    if (i > 4) {
                        if (i == 5) {
                            name1 = data[5];
                        } else {
                            name1 += data[i];
                        }
                    }
                }
                $scope.fromDate = name;
                $scope.toDate = name1;
                $scope.frommon = "";
                $scope.tomon = "";
                $scope.fromDay = "";
                $scope.toDay = "";
                // For Academic From Date


                $scope.minDatemf = new Date(
                    $scope.fromDate,
                    $scope.frommon,
                    $scope.fromDay + 1);

                $scope.maxDatemf = new Date(
                    $scope.toDate,
                    $scope.tomon,
                    $scope.toDay + 365);

            }


        }

        $scope.onsubstaffchange = function (staffid, staffdata,ttmpid) {

            //console.log(staffid);
            //console.log(staffdata);

            for (var i = 0; i < staffdata.length; i++) {
                if (staffdata[i].HRME_Id == Number(staffid)) {
                    for (var j = 0; j < $scope.staff_day_periods.length; j++) {
                        if ($scope.staff_day_periods[j].ttmP_Id == ttmpid) {
                            $scope.staff_day_periods[j].daycnt = staffdata[i].DAYTL;
                            $scope.staff_day_periods[j].weekcnt = staffdata[i].WWCNT;
                            $scope.staff_day_periods[j].mntcnt = staffdata[i].mntcnt;
                            $scope.staff_day_periods[j].currdaydeputed = staffdata[i].daycnt;
                            $scope.staff_day_periods[j].weekcntdeputed = staffdata[i].weekcnt;
                            $scope.staff_day_periods[j].mntcntdeputed = staffdata[i].mntcnt;
                            $scope.staff_day_periods[j].totaldeputedcount = staffdata[i].totalcnt;
                            $scope.staff_day_periods[j].weekcnt1 = staffdata[i].weekcnt1;
                            $scope.staff_day_periods[j].mntcnt1 = staffdata[i].mntcnt1;
                            $scope.staff_day_periods[j].totalcnt1 = staffdata[i].totalcnt1;
                            $scope.staff_day_periods[j].HRMEnew_Id = Number(staffid);
                            $scope.staff_day_periods[j].freeperiodweekly = staffdata[i].freeperiodweekly;
                            $scope.staff_day_periods[j].freeperioddaily = staffdata[i].freeperioddaily;
                           
                        }
                    }
                }
            }

        }
        $scope.viewrecordspopup2 = function (stfid, yrid) {

            var data = {
                "ASMAY_Id": yrid,
                "HRME_Id": stfid,

            }
            apiService.create("Deputation/getalldetailsviewrecords2", data).
                then(function (promise) {
                    if (promise.periodslst != null && promise.periodslst != "" && promise.gridweeks != null && promise.gridweeks != "" && promise.time_Table != null && promise.time_Table != "") {
                        // $scope.grid_view = true;
                        $scope.period_list = promise.periodslst;
                        $scope.day_list1 = promise.gridweeks;
                        $scope.tt_list = promise.time_Table;
                        var temp_array = [];
                        $scope.table_list = [];
                        for (var j = 0; j < $scope.day_list1.length; j++) {
                            temp_array = [];
                            for (var i = 0; i < $scope.period_list.length; i++) {

                                var count = 0;
                                var newCol = "";
                                for (var k = 0; k < $scope.tt_list.length; k++) {

                                    if ($scope.tt_list[k].ttmP_Id == $scope.period_list[i].ttmP_Id && $scope.tt_list[k].ttmD_Id == $scope.day_list1[j].ttmD_Id) {

                                        if (count == 0) {
                                            newCol = { pedid: $scope.period_list[i].ttmP_Id, dayid: $scope.day_list1[j].ttmD_Id, value: $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].ismS_SubjectName, pedname: $scope.period_list[i].ttmP_PeriodName, dayname: $scope.day_list1[j].ttmD_DayName, color: 'black', value1: $scope.tt_list[k].staffName + ' :' + $scope.tt_list[k].ismS_SubjectName }
                                            count += 1;
                                        }
                                        else if (count > 0) {
                                            newCol.value = newCol.value + ' && ' + $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].ismS_SubjectName;
                                            newCol.value1 = newCol.value1 + ' && ' + $scope.tt_list[k].staffName + ' :' + $scope.tt_list[k].ismS_SubjectName;
                                            count += 1;
                                        }
                                    }
                                    $scope.staff_Name = $scope.tt_list[k].staffName;
                                }
                                if (newCol == "") {
                                    newCol = { pedid: $scope.period_list[i].ttmP_Id, dayid: $scope.day_list1[j].ttmD_Id, value: ' ', pedname: $scope.period_list[i].ttmP_PeriodName, dayname: $scope.day_list1[j].ttmD_DayName, color: 'black' }
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
                        // swal("No,TimeTable is Not Generated !!!!");
                        swal("TimeTable is Not Generated For Selected Details !!!!");
                        $scope.grid_view = false;
                        $scope.datareport = false;
                    }
                })

        };
    }

})();