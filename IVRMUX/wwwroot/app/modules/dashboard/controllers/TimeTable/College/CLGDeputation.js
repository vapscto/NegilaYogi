
(function () {
    'use strict';
    angular
        .module('app')
        .controller('CLGDeputationController', CLGDeputationController)

    CLGDeputationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter', 'Excel', '$timeout']
    function CLGDeputationController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, Excel, $timeout) {
        $scope.editEmployee = {};
        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.grid_view = false;
        $scope.itemsPerPage1 = 10;
        $scope.options = {
            hstep: [1, 2, 3],
            mstep: [1, 5, 10, 15, 25, 30]
        };
        $scope.currentPage = 1;
        $scope.itemsPerPage = 50;
        $scope.ismeridian = true;
        $scope.abs = false;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };


        $scope.changesbs = function () {
            $scope.HRME_Id = "";
            $scope.loaddefault = false;
            $scope.loaddefault1 = false;

            $scope.stf_dy_pds = false;
            $scope.stf_fr_pds = false;
            $scope.staff_free_periods = [];
            $scope.staff_list = [];

            if ($scope.asmaY_Id == "" || $scope.myDate == "" || $scope.myDate == undefined || $scope.asmaY_Id == undefined || $scope.myDate == null || $scope.asmaY_Id == null) {
                swal("Please Select Academic Year And Date");
                $scope.HRME_Id = "";
                $scope.abs = false;
            }
            else {
                var TTSD_Date = new Date($scope.myDate).toDateString();
                debugger;
                var data = {

                    "ASMAY_Id": $scope.asmaY_Id,
                    "TTSD_Date": TTSD_Date,
                    "absflag": $scope.abs,
                }
                apiService.create("CLGDeputation/getabsentstaff", data).
                    then(function (promise) {

                        //$scope.staff_list = promise.stafflist;
                        $scope.staff_list = [];
                        for (var m = 0; m < promise.stafflist.length; m++) {
                            var stf_chg = { hrmE_Id: promise.stafflist[m].hrmE_Id, staffName: promise.stafflist[m].staffName, hrmE_EmployeeCode: promise.stafflist[m].hrmE_EmployeeCode, stf_name: promise.stafflist[m].staffName, stf_code: promise.stafflist[m].hrmE_EmployeeCode, stf_code_name: promise.stafflist[m].hrmE_EmployeeCode + ':' + promise.stafflist[m].staffName };
                            $scope.staff_list.push(stf_chg);
                        }
                    })
            }
        }

        $scope.isOptionsRequired = function () {

            return !$scope.staff_list.some(function (options) {
                return options.stf;
            });
        }
        $scope.rpttyp = "Name";
        $scope.onclickloaddata = function () {
            $scope.loaddefault = false;
            $scope.loaddefault1 = false;
            $scope.hrmE_Id = "";
            $scope.stf_dy_pds = false;
            $scope.stf_fr_pds = false;
        }

        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };

        var abst_amcO_Id = "";
        var abst_amB_Id = "";
        var abst_amsE_Id = "";
        var abst_acmS_Id = "";
        var abst_ttmD_Id = "";
        var abst_ttmP_Id = "";
        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function (all) {
            $scope.submitted = true;
            var dvcid = '';
            if ($scope.myForm.$valid) {
                var deput_hrmE_Id = "";
                angular.forEach($scope.mainstafflist, function (role) {
                    if (role.checkedvalue) {
                        deput_hrmE_Id = role.HRME_Id;
                        dvcid = role.deviceid;
                    } 
                })
                angular.forEach(all, function (role) {
                    if (role.Selected) abst_ttmP_Id = role.TTMP_PeriodName;
                })

                var TTSD_Date = new Date($scope.myDate).toDateString();
                if (deput_hrmE_Id != "" && deput_hrmE_Id != null) {
                    var data = {
                        "TTSD_Id": $scope.TTSD_Id,
                        "ASMAY_Id": $scope.asmaY_Id,
                        "TTSD_Date": TTSD_Date,
                        "AMCO_Id": abst_amcO_Id,
                        "AMB_Id": abst_amB_Id,
                        "AMSE_Id": abst_amsE_Id,
                        "ACMS_Id": abst_acmS_Id,
                        "TTMD_Id": abst_ttmD_Id,
                        "TTMP_Id": abst_ttmP_Id,
                        "TTSD_AbsentStaff": $scope.HRME_Id.hrmE_Id,
                        "TTSD_DeputedStaff": deput_hrmE_Id,
                        "TTSD_Remarks": $scope.remarks,
                        "smsflag": $scope.SMS_Flag,
                        "mailflag": $scope.EMAIL_Flag,
                        "NOT_Flag": $scope.NOT_Flag,
                        "deviceid": dvcid

                    }
                    apiService.create("CLGDeputation/savedetails", data).
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
                    swal("Please Select Deputed Staff !!!!")
                }
            }
            else {
                $scope.submitted = true;

            }

        };
        //TO  GEt The Values iN Grid
        $scope.BindData = function () {

            $scope.abs = false;
            $scope.currentPage = 1;
            $scope.itemsPerPage = 50;
            $scope.rpttyp = "Name";
            apiService.getDATA("CLGDeputation/getdetails").
                then(function (promise) {
                    $scope.year_list = promise.acayear;
                    $scope.category_list = promise.categorylist;
                    $scope.class_list = promise.classlist;
                    $scope.temp_classlist = promise.classlist;
                    //$scope.staff_list = promise.stafflist;         
                    $scope.day_list = promise.daylist;
                    $scope.staff_list = [];
                    for (var m = 0; m < promise.stafflist.length; m++) {
                        var stf_chg = { hrmE_Id: promise.stafflist[m].hrmE_Id, staffName: promise.stafflist[m].staffName, hrmE_EmployeeCode: promise.stafflist[m].hrmE_EmployeeCode, stf_name: promise.stafflist[m].staffName, stf_code: promise.stafflist[m].hrmE_EmployeeCode, stf_code_name: promise.stafflist[m].hrmE_EmployeeCode + ':' + promise.stafflist[m].staffName };
                        $scope.staff_list.push(stf_chg);
                    }

                })
        };

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
            apiService.create("CLGDeputation/viewdeputation", data).
                then(function (promise) {

                    $scope.dptdetails = promise.dptdetails;
                })

        }


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
        $scope.cell_click = function (dayid, periodid, day, period, stfid, yrid, crsid, brid, semid, secid, TTFGDC_Id) {


            angular.forEach($scope.staff_day_periods, function (ff) {
                if (ff.TTFGDC_Id == TTFGDC_Id) {
                    ff.clr ='green'
                }
                else {
                    ff.clr = ''
                }

            })

            $scope.loaddefault1 = false;
            abst_amcO_Id = crsid;
            abst_amB_Id = brid;
            abst_amsE_Id = semid;
            abst_acmS_Id = secid;
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
            apiService.create("CLGDeputation/get_free_stfdets", data).
                then(function (promise) {

                    if (promise.time_Table != "" && promise.time_Table != null) {
                        $scope.stf_fr_pds = true;
                        $scope.loaddefault1 = true;
                    }


                    $scope.mainstafflist = [];

                    angular.forEach(promise.time_Table, function (eps) {
                        if ($scope.mainstafflist.length == 0) {
                            $scope.mainstafflist.push({ HRME_Id: eps.HRME_Id, STAFF: eps.EName, EMPCODE: eps.HRME_EmployeeCode, DCNT: eps.DCNT, WCNT: eps.WCNT, MCNT: eps.MCNT, YCNT: eps.YCNT, asmaY_Id: eps.ASMAY_Id, deviceid: eps.deviceid });
                        }
                        else if ($scope.mainstafflist.length > 0) {
                            var al_exm_cnt = 0;
                            angular.forEach($scope.mainstafflist, function (exm) {
                                if (exm.HRME_Id == eps.HRME_Id) {
                                    al_exm_cnt += 1;
                                }
                            })
                            if (al_exm_cnt == 0) {
                                $scope.mainstafflist.push({ HRME_Id: eps.HRME_Id, STAFF: eps.EName, EMPCODE: eps.HRME_EmployeeCode, DCNT: eps.DCNT, WCNT: eps.WCNT, MCNT: eps.MCNT, YCNT: eps.YCNT, asmaY_Id: eps.ASMAY_Id, deviceid: eps.deviceid });
                            }
                        }
                    })

                    $scope.dpcount = promise.dpcount;
                    $scope.absentdpcountall = promise.absentdpcountall;
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
                        angular.forEach($scope.absentdpcountall, function (xx) {
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
                    $scope.ttlistdata = promise.time_Table;

                    angular.forEach($scope.mainstafflist, function (zz) {

                        var templist = [];

                        angular.forEach($scope.periodslst, function (pp) {
                            var clsdetails = '';
                            var subdetails = '';

                            angular.forEach($scope.ttlistdata, function (tt) {
                                if (tt.HRME_Id == zz.HRME_Id && tt.TTMP_Id == pp.ttmP_Id) {

                                    if (clsdetails == '') {
                                        clsdetails = tt.AMCO_CourseName + '&' + tt.AMB_BranchName + '&' + tt.AMSE_SEMName + '&' + tt.ACMS_SectionName;
                                    }
                                    else {
                                        clsdetails = clsdetails + ' ' + 'AND' + ' ' + tt.AMCO_CourseName + '&' + tt.AMB_BranchName + '&' + tt.AMSE_SEMName + '&' + tt.ACMS_SectionName;
                                    }

                                    if (subdetails == '') {
                                        subdetails = tt.ISMS_SubjectName;
                                    }
                                    else {
                                        if (subdetails != tt.ISMS_SubjectName) {
                                            subdetails = subdetails + ' ' + 'AND' + ' ' + tt.ISMS_SubjectName;
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


                    console.log($scope.mainstafflist);



                    //var temp = [];
                    //var name = "";
                    //var code = "";
                    //for (var n = 0; n < promise.time_Table.length; n++) {


                    //    if (n == 0) {

                    //        temp.push({ HRME_Id: promise.time_Table[n].HRME_Id, HRME_EmployeeCode: promise.time_Table[n].HRME_EmployeeCode, EName: promise.time_Table[n].EName, AMCO_CourseName: promise.time_Table[n].AMCO_CourseName, AMB_BranchName: promise.time_Table[n].AMB_BranchName, AMSE_SEMName: promise.time_Table[n].AMSE_SEMName, ACMS_SectionName: promise.time_Table[n].ACMS_SectionName, ISMS_SubjectName: promise.time_Table[n].ISMS_SubjectName, TTMP_PeriodName: promise.time_Table[n].TTMP_PeriodName, ASMAY_Id: promise.time_Table[n].ASMAY_Id });
                    //        var name = promise.time_Table[n].EName;
                    //        var code = promise.time_Table[n].HRME_EmployeeCode;
                    //    }
                    //    else if (n > 0) {
                    //        if (name == promise.time_Table[n].EName && code == promise.time_Table[n].HRME_EmployeeCode) {

                    //            temp.push({ HRME_Id: promise.time_Table[n].HRME_Id, HRME_EmployeeCode: ' ', EName: ' ', AMCO_CourseName: promise.time_Table[n].AMCO_CourseName, AMB_BranchName: promise.time_Table[n].AMB_BranchName, AMSE_SEMName: promise.time_Table[n].AMSE_SEMName, ACMS_SectionName: promise.time_Table[n].ACMS_SectionName, ISMS_SubjectName: promise.time_Table[n].ISMS_SubjectName, TTMP_PeriodName: promise.time_Table[n].TTMP_PeriodName, ASMAY_Id: promise.time_Table[n].ASMAY_Id });

                               
                    //        }
                    //        else {

                    //            temp.push({ HRME_Id: promise.time_Table[n].HRME_Id, HRME_EmployeeCode: promise.time_Table[n].HRME_EmployeeCode, EName: promise.time_Table[n].EName, AMCO_CourseName: promise.time_Table[n].AMCO_CourseName, AMB_BranchName: promise.time_Table[n].AMB_BranchName, AMSE_SEMName: promise.time_Table[n].AMSE_SEMName, ACMS_SectionName: promise.time_Table[n].ACMS_SectionName, ISMS_SubjectName: promise.time_Table[n].ISMS_SubjectName, TTMP_PeriodName: promise.time_Table[n].TTMP_PeriodName, ASMAY_Id: promise.time_Table[n].ASMAY_Id });

                    //        }
                    //        var name = promise.time_Table[n].EName;
                    //        var code = promise.time_Table[n].HRME_EmployeeCode;
                    //    }
                    //}

                    //$scope.staff_free_periods = temp;

                    //$scope.dpcount = promise.dpcount;
                    //$scope.absentdpcountall = promise.absentdpcountall;
                    //angular.forEach($scope.staff_free_periods, function (dd) {
                    //    angular.forEach($scope.dpcount, function (xx) {
                    //        if (dd.HRME_Id == xx.HRME_Id) {
                    //            if (dd.EName != '') {
                    //                dd.weekcnt = xx.WeekStaffcount;
                    //                dd.mntcnt = xx.MonthStaffcount;
                    //                dd.totalcnt = xx.YearStaffcount;
                    //                dd.daycnt = xx.TodayStaffcount;
                    //            }
                    //        }

                    //    })


                    //    angular.forEach($scope.absentdpcountall, function (xx) {
                    //        if (dd.HRME_Id == xx.HRME_Id) {
                    //            if (dd.EName != '') {
                    //                dd.weekcnt1 = xx.WeekStaffcount;
                    //                dd.mntcnt1 = xx.MonthStaffcount;
                    //                dd.totalcnt1 = xx.YearStaffcount;
                    //                dd.daycnt1 = xx.TodayStaffcount;
                    //            }
                    //        }

                    //    })

                    //})

                    //console.log($scope.staff_free_periods);

                    if (promise.time_Table == "" || promise.time_Table == null) {
                        swal("No Staffs Are Free   For Selected Day !!!");
                        $scope.stf_fr_pds = false;
                        $scope.loaddefault1 = false;
                    }
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
                            swal('Daily Deputation is Exceeds');
                            d += 1;
                        }
                    }
                    if (itm.WCNT != null && itm.WCNT != undefined && itm.WCNT != '' && itm.WCNT != 0) {

                        if (parseInt(obj.weekcnt) >= parseInt(itm.WCNT)) {
                            swal('Weekly Deputation is Exceeds');
                            w += 1;
                        }
                    }
                    if (itm.MCNT != null && itm.MCNT != undefined && itm.MCNT != '' && itm.MCNT != 0) {

                        if (parseInt(obj.mntcnt) >= parseInt(itm.MCNT)) {
                            swal('Monthly Deputation is Exceeds');
                            m += 1;
                        }
                    }

                    if (itm.YCNT != null && itm.YCNT != undefined && itm.YCNT != '' && itm.YCNT != 0) {

                        if (parseInt(obj.totalcnt) >= parseInt(itm.YCNT)) {
                            swal('Yearly Deputation is Exceeds');
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
        //$scope.optionToggled = function (obj) {

        //    angular.forEach($scope.staff_free_periods, function (itm) {
        //        if (obj.HRME_Id == itm.HRME_Id) {

        //            itm.checkedvalue = true;
        //        }
        //        else {
        //            itm.checkedvalue = false;
        //        }
        //    });
        //}

        $scope.viewrecordspopup2 = function (stfid, yrid) {

            var data = {
                "ASMAY_Id": yrid,
                "HRME_Id": stfid,
                "ttftype": 'F'

            }
            apiService.create("CLGDeputation/getalldetailsviewrecords2", data).
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
                                            newCol = { pedid: $scope.period_list[i].ttmP_Id, dayid: $scope.day_list1[j].ttmD_Id, value: $scope.tt_list[k].AMCO_CourseName + ' :' + $scope.tt_list[k].AMB_BranchName + ' :' + $scope.tt_list[k].AMSE_SEMName + ' :' + $scope.tt_list[k].ACMS_SectionName + ' :' + $scope.tt_list[k].ismS_SubjectName, pedname: $scope.period_list[i].ttmP_PeriodName, dayname: $scope.day_list1[j].ttmD_DayName, color: 'black', value1: $scope.tt_list[k].staffName + ' :' + $scope.tt_list[k].ismS_SubjectName }
                                            count += 1;
                                        }
                                        else if (count > 0) {
                                            newCol.value = newCol.value + ' && ' + $scope.tt_list[k].AMCO_CourseName + ' :' + $scope.tt_list[k].AMB_BranchName + ' :' + $scope.tt_list[k].AMSE_SEMName + ' :' + $scope.tt_list[k].ACMS_SectionName+':'+ $scope.tt_list[k].ismS_SubjectName;
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



        $scope.viewrecordspopup9 = function (stfid, yrid) {

            var data = {
                "ASMAY_Id": yrid,
                "HRME_Id": stfid,
                "TTMD_Id": $scope.ttmD_Id,
                "ttftype": 'D'

            }
            apiService.create("CLGDeputation/getalldetailsviewrecords2", data).
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
                                            newCol = { pedid: $scope.period_list[i].ttmP_Id, dayid: $scope.day_list1[j].ttmD_Id, value: $scope.tt_list[k].AMCO_CourseName + ' :' + $scope.tt_list[k].AMB_BranchName + ' :' + $scope.tt_list[k].AMSE_SEMName + ' :' + $scope.tt_list[k].ACMS_SectionName + ' :' + $scope.tt_list[k].ismS_SubjectName, pedname: $scope.period_list[i].ttmP_PeriodName, dayname: $scope.day_list1[j].ttmD_DayName, color: 'black', value1: $scope.tt_list[k].staffName + ' :' + $scope.tt_list[k].ismS_SubjectName }
                                            count += 1;
                                        }
                                        else if (count > 0) {
                                            newCol.value = newCol.value + ' && ' + $scope.tt_list[k].AMCO_CourseName + ' :' + $scope.tt_list[k].AMB_BranchName + ' :' + $scope.tt_list[k].AMSE_SEMName + ' :' + $scope.tt_list[k].ACMS_SectionName + ':' + $scope.tt_list[k].ismS_SubjectName;
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
            apiService.create("CLGDeputation/viewabsent", data).
                then(function (promise) {

                    $scope.dptdetails1 = promise.dptdetails;
                })

        }

        $scope.viewrecordspopup5 = function (stfid,yearid ,flg, stname) {
            debugger;
            $scope.staff_Name2 = stname;
            $scope.dptdetails1 = [];
            var TTSD_Date = new Date($scope.myDate).toDateString();
            var data = {
                "ASMAY_Id": yearid,
                "HRME_Id": stfid,
                "FLAG": flg,
                "TTSD_Date": TTSD_Date,
            }
            apiService.create("CLGDeputation/viewabsent", data).
                then(function (promise) {

                    $scope.dptdetails1 = promise.dptdetails;
                })

        }
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
        };

        $scope.asmaY_Id = "";
        $scope.myDate = "";
        $scope.ttmD_Id = "";
        $scope.hrmE_Id = "";
        $scope.stf_dy_pds = false;
        $scope.loaddefault = false;
        $scope.loaddefault1 = false;
        $scope.get_period_alloted = function () {
            $scope.stf_fr_pds = false;

            if ($scope.asmaY_Id == "" || $scope.myDate == "") {
                swal("Please Select Academic Year And Date");
                $scope.HRME_Id = "";
            }
            else if ($scope.asmaY_Id != "" && $scope.ttmD_Id != "" && $scope.HRME_Id.hrmE_Id != "") {
                var TTSD_Date = new Date($scope.myDate).toDateString();
                var data = {
                    // "TTMC_Id": $scope.ttmC_Id1,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "TTMD_Id": $scope.ttmD_Id,
                    "HRME_Id": $scope.HRME_Id.hrmE_Id,
                    "TTSD_Date": TTSD_Date,
                }
                apiService.create("CLGDeputation/get_period_alloted", data).
                    then(function (promise) {

                        if (promise.time_Table != "" && promise.time_Table != null) {
                            $scope.stf_dy_pds = true;
                            $scope.loaddefault = true;
                            $scope.loaddefault1 = false;
                        }
                        $scope.staff_day_periods = promise.time_Table;
                        $scope.absentdpcount = promise.absentdpcount;

                        console.log($scope.staff_day_periods);
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
            if ($scope.asmaY_Id != "" && $scope.ttmD_Id != "" && $scope.HRME_Id.hrmE_Id != "" && $scope.HRME_Id != "" && $scope.HRME_Id.hrmE_Id != undefined) {
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

    }

})();