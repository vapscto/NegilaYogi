
(function () {
    'use strict';
    angular
.module('app')
        .controller('CLGTTCourseReportBWMCController', CLGTTCourseReportBWMCController)

    CLGTTCourseReportBWMCController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', 'Excel', '$timeout', '$http', '$q', '$stateParams', '$filter']
    function CLGTTCourseReportBWMCController($rootScope, $scope, $state, $location, apiService, Flash, Excel, $timeout, $http, $q, $stateParams, $filter) {

        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.amsE_SEMName).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        }
        $scope.searchchkbxvv = "";
        $scope.filterchkbxvv = function (obj) {
            return angular.lowercase(obj.amcO_CourseName).indexOf(angular.lowercase($scope.searchchkbxvv)) >= 0;
        }

        $scope.searchchkbx1 = "";
        $scope.filterchkbx1 = function (obj) {
            return angular.lowercase(obj.acmS_SectionName).indexOf(angular.lowercase($scope.searchchkbx1)) >= 0;
        }

        $scope.searchchkbx15 = "";
        $scope.filterchkbx15 = function (obj) {
            return angular.lowercase(obj.amB_BranchName).indexOf(angular.lowercase($scope.searchchkbx15)) >= 0;
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
        $scope.togchkbxvv = function () {
            $scope.usercheckvv = $scope.courselist.every(function (options) {
                return options.class;
            });

            $scope.getbranch_catg();
        }
        $scope.togchkbx1 = function () {
            $scope.usercheck1 = $scope.section_list.every(function (options) {
                return options.sec;
            });
        }
        $scope.togchkbx15 = function () {
            $scope.usercheck15 = $scope.branchlist.every(function (options) {
                return options.sec;
            });

            $scope.get_semister();
        }


        $scope.all_check = function () {
            
            var toggleStatus = $scope.usercheck;
            angular.forEach($scope.semisterlist, function (itm) {
                itm.class = toggleStatus;
            });
        }
        $scope.all_checkvv = function () {
            
            var toggleStatus = $scope.usercheckvv;
            angular.forEach($scope.courselist, function (itm) {
                itm.class = toggleStatus;
            });

            $scope.getbranch_catg();
        }



        $scope.all_check1 = function () {
            var toggleStatus = $scope.usercheck1;
            angular.forEach($scope.section_list, function (itm) {
                itm.sec = toggleStatus;
            });
        }
        $scope.all_check15 = function () {
            var toggleStatus = $scope.usercheck15;
            angular.forEach($scope.branchlist, function (itm) {
                itm.sec = toggleStatus;
            });
            $scope.get_semister();
        }
        $scope.isOptionsRequired = function () {
            return !$scope.semisterlist.some(function (options) {
                return options.class;
            });

        }
        $scope.isOptionsRequiredvv = function () {
            return !$scope.courselist.some(function (options) {
                return options.class;
            });

        }

        $scope.isOptionsRequired1 = function () {
            return !$scope.section_list.some(function (options) {
                return options.sec;
            });

        }
        $scope.isOptionsRequired15 = function () {
            return !$scope.branchlist.some(function (options) {
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
            $scope.usercheck15 = false;
            $scope.courids = [];
            angular.forEach($scope.courselist, function (ff) {
                if (ff.class == true) {
                    $scope.courids.push(ff);
                }
            })

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
                    crids: $scope.courids,
                }
                apiService.create("CLGTTCommon/multplegetbranch_catg", data).
                    then(function (promise) {

                        $scope.branchlist = promise.branchlist;

                        if (promise.branchlist == "" || promise.branchlist == null) {
                            swal("No Branch Are Mapped To Selected Category/Course");
                        }
                    })
            }
        };

        $scope.get_semister = function () {
            $scope.usercheck = false;
            $scope.semisterlist = [];
            $scope.courids = [];
            angular.forEach($scope.courselist, function (ff) {
                if (ff.class == true) {
                    $scope.courids.push(ff);
                }
            })
            $scope.brids = [];
            angular.forEach($scope.branchlist, function (ff) {
                if (ff.sec == true) {
                    $scope.brids.push(ff);
                }
            })


            if ($scope.ASMAY_Id == "" && $scope.ASMAY_Id == undefined) {
                swal("Please Select The Academic Year !");
            }
            else if ($scope.ASMAY_Id != "" && $scope.ASMAY_Id != undefined && $scope.TTMC_Id != "") {
                var data = {
                    "TTMC_Id": $scope.TTMC_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    crids: $scope.courids,
                    brnchds: $scope.brids,
                }
                apiService.create("CLGTTCommon/multget_semister", data).
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
        $scope.cname = '';
        $scope.bname = '';
        $scope.scname = '';
        $scope.smname = '';
        $scope.submitted = false;
        $scope.GetReport = function () {
            $scope.cname = '';
            $scope.bname = '';
            $scope.scname = '';
            $scope.smname = '';
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                
                $scope.albumNameArray1 = [];
                $scope.albumNameArray2 = [];

                angular.forEach($scope.semisterlist, function (role) {
                    if (role.class) {
                        $scope.albumNameArray1.push(role);
                        if ($scope.smname == '') {
                            $scope.smname = role.amsE_SEMName;
                        }
                        else {
                            $scope.smname = $scope.smname + ' , ' + role.amsE_SEMName;
                        }
                    }
                        
                })
                angular.forEach($scope.section_list, function (role) {
                    if (role.sec) {
                        $scope.albumNameArray2.push(role);

                        if ($scope.scname == '') {
                            $scope.scname = role.acmS_SectionName;
                        }
                        else {
                            $scope.scname = $scope.scname + ' , ' + role.acmS_SectionName;
                        }
                    }

                        
                })
                $scope.courids = [];
                angular.forEach($scope.courselist, function (ff) {
                    if (ff.class == true) {
                        $scope.courids.push(ff);
                        if ($scope.cname=='') {
                            $scope.cname = ff.amcO_CourseName;
                        }
                        else {
                            $scope.cname = $scope.cname + ' , ' + ff.amcO_CourseName;
                        }
                        
                    }
                })
                $scope.brids = [];
                angular.forEach($scope.branchlist, function (ff) {
                    if (ff.sec == true) {
                        $scope.brids.push(ff);
                        if ($scope.bname == '') {
                            $scope.bname = ff.amB_BranchName;
                        }
                        else {
                            $scope.bname = $scope.bname + ' , ' + ff.amB_BranchName;
                        }

                    }
                    
                })
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "TTMC_Id": $scope.TTMC_Id,
                    classarray: $scope.albumNameArray1,
                    sectionarray: $scope.albumNameArray2,
                    crids: $scope.courids,
                    brnchds: $scope.brids,

                }
                apiService.create("CLGTTCourseReportBWMC/getrpt", data).
                    then(function (promise) {
                      
                        $scope.time_Table = promise.time_Table;
                        $scope.TT_Break_list_all = promise.tT_Break_list_all;
                        $scope.allday = promise.allday;
                        $scope.periodtimelist = promise.periodtimelist;
                        $scope.periodtimelist_distinct = promise.periodtimelist_distinct;
                      


                        
                        if ($scope.time_Table.length > 0 && $scope.allday.length > 0) {
                            $scope.grid_view = true;

                            $scope.header1 = [];
                            $scope.header2 = [];
                            $scope.mainheader = [];
                            $scope.mainheader1 = [];
                            var cnt = 0;
                            angular.forEach($scope.allday, function (ee) {
                                cnt += 1;
                                var dayperiodtm = [];
                                angular.forEach($scope.periodtimelist, function (xx) {

                                    if (ee.ttmD_Id == xx.TTMD_Id) {
                                        if (cnt == 1) {
                                            angular.forEach($scope.periodslst, function (ww) {

                                                debugger;
                                                if (ww.ttmP_Id == xx.TTMP_Id && angular.lowercase(ee.ttmD_DayName) != 'saturday') {
                                                    $scope.mainheader.push({ TTMP_Id: xx.TTMP_Id, TTMP_PeriodName: ww.ttmP_PeriodName, START: xx.TTMDPT_StartTime, END: xx.TTMDPT_EndTime })
                                                }
                                                debugger;
                                                if (ww.ttmP_Id == xx.TTMP_Id && angular.lowercase(ee.ttmD_DayName) == 'saturday') {
                                                    $scope.mainheader1.push({ TTMP_Id: xx.TTMP_Id, TTMP_PeriodName: ww.ttmP_PeriodName, START: xx.TTMDPT_StartTime, END: xx.TTMDPT_EndTime })
                                                }


                                            })



                                        }


                                        angular.forEach($scope.periodslst, function (ww) {
                                            if (ww.ttmP_Id == xx.TTMP_Id && angular.lowercase(ee.ttmD_DayName) == 'saturday') {
                                                $scope.mainheader1.push({ TTMP_Id: xx.TTMP_Id, TTMP_PeriodName: ww.ttmP_PeriodName, START: xx.TTMDPT_StartTime, END: xx.TTMDPT_EndTime })
                                            }


                                        })
                                        dayperiodtm.push(xx);
                                    }


                                })

                                ee.timlist = dayperiodtm;

                            })




                            $scope.mainperiodlist = [];
                            $scope.mainperiodlist1 = [];
                            var ctww = 0;
                            angular.forEach($scope.allday, function (yy) {
                                ctww += 1;
                                if (ctww == 1) {


                                    var ct = 0;
                                    angular.forEach($scope.mainheader, function (xx) {
                                        if ($scope.TT_Break_list_all.length > 0) {
                                            var cnt = 0;
                                            angular.forEach($scope.TT_Break_list_all, function (zz) {
                                                if (yy.ttmD_Id == zz.ttmD_Id && angular.lowercase(yy.ttmD_DayName) != 'saturday') {
                                                    ct += 1;
                                                    debugger;
                                                    if (xx.TTMP_PeriodName == zz.ttmbC_AfterPeriod) {
                                                        cnt += 1;
                                                        $scope.mainperiodlist.push({ TTMP_Id: xx.TTMP_Id, TTMP_PeriodName: 'PERIOD' + ':' + xx.TTMP_PeriodName, START: xx.START, END: xx.END })
                                                        $scope.mainperiodlist.push({ TTMP_Id: zz.ttmbC_BreakName, TTMP_PeriodName: 'BREAK', START: zz.ttmbC_BreakStartTime, END: zz.ttmbC_BreakEndTime })
                                                    }
                                                    else {

                                                    }
                                                }
                                                //else {
                                                //    $scope.mainperiodlist.push({ TTMP_Id: xx.ttmP_Id, TTMP_PeriodName: xx.ttmP_PeriodName })
                                                //}

                                            })
                                            if (cnt == 0 && ct > 0) {
                                                $scope.mainperiodlist.push({ TTMP_Id: xx.TTMP_Id, TTMP_PeriodName: 'PERIOD' + ':' + xx.TTMP_PeriodName, START: xx.START, END: xx.END })
                                            }
                                            if (ct == 0) {
                                                $scope.mainperiodlist.push({ TTMP_Id: xx.TTMP_Id, TTMP_PeriodName: 'PERIOD' + ':' + xx.TTMP_PeriodName, START: xx.START, END: xx.END })
                                            }

                                        }
                                        else {
                                            $scope.mainperiodlist.push({ TTMP_Id: xx.TTMP_Id, TTMP_PeriodName: 'PERIOD' + ':' + xx.TTMP_PeriodName, START: xx.START, END: xx.END })
                                        }


                                    })
                                }


                                if (angular.lowercase(yy.ttmD_DayName) == 'saturday') {


                                    var ct = 0;
                                    angular.forEach($scope.mainheader1, function (xx) {
                                        if ($scope.TT_Break_list_all.length > 0) {
                                            var cnt = 0;
                                            angular.forEach($scope.TT_Break_list_all, function (zz) {
                                                if (yy.ttmD_Id == zz.ttmD_Id && angular.lowercase(yy.ttmD_DayName) == 'saturday') {
                                                    ct += 1;
                                                    debugger;
                                                    if (xx.TTMP_PeriodName == zz.ttmbC_AfterPeriod) {
                                                        cnt += 1;
                                                        $scope.mainperiodlist1.push({ TTMP_Id: xx.TTMP_Id, TTMP_PeriodName: 'PERIOD' + ':' + xx.TTMP_PeriodName, START: xx.START, END: xx.END })
                                                        $scope.mainperiodlist1.push({ TTMP_Id: zz.ttmbC_BreakName, TTMP_PeriodName: 'BREAK', START: zz.ttmbC_BreakStartTime, END: zz.ttmbC_BreakEndTime })
                                                    }
                                                    else {

                                                    }
                                                }


                                            })
                                            if (cnt == 0 && ct > 0) {
                                                $scope.mainperiodlist1.push({ TTMP_Id: xx.TTMP_Id, TTMP_PeriodName: 'PERIOD' + ':' + xx.TTMP_PeriodName, START: xx.START, END: xx.END })
                                            }
                                            if (ct == 0) {
                                                $scope.mainperiodlist1.push({ TTMP_Id: xx.TTMP_Id, TTMP_PeriodName: 'PERIOD' + ':' + xx.TTMP_PeriodName, START: xx.START, END: xx.END })
                                            }

                                        }
                                        else {
                                            $scope.mainperiodlist1.push({ TTMP_Id: xx.TTMP_Id, TTMP_PeriodName: 'PERIOD' + ':' + xx.TTMP_PeriodName, START: xx.START, END: xx.END })
                                        }


                                    })
                                }


                                var ttlist = [];
                                angular.forEach($scope.time_Table, function (nn) {
                                    if (nn.TTMD_Id == yy.ttmD_Id) {
                                        ttlist.push(nn);
                                    }


                                })



                                $scope.distcourselist = [];

                                angular.forEach($scope.TT_Break_list_all, function (st) {
                                    if (st.ttmD_Id == yy.ttmD_Id) {


                                        if ($scope.distcourselist.length == 0) {


                                            $scope.distcourselist.push({ TTMP_Id: st.ttmbC_BreakName, TTMP_PeriodName: st.ttmbC_BreakName, AF: st.ttmbC_AfterPeriod });
                                        }
                                        else if ($scope.distcourselist.length > 0) {
                                            var cntt = 0;
                                            angular.forEach($scope.distcourselist, function (exm) {
                                                if (exm.AF == st.ttmbC_AfterPeriod) {
                                                    cntt += 1;
                                                }
                                            })
                                            if (cntt == 0) {

                                                $scope.distcourselist.push({ TTMP_Id: st.ttmbC_BreakName, TTMP_PeriodName: st.ttmbC_BreakName, AF: st.ttmbC_AfterPeriod });
                                            }
                                        }
                                    }
                                })

                                angular.forEach($scope.distcourselist, function (ddd) {
                                    ttlist.push({ TTMP_Id: ddd.TTMP_Id })
                                })


                                yy.timetable = ttlist;

                            })




                           
                        }

 else {
                            swal("TimeTable is Not Generated For Selected Details !!!!");
                            $scope.grid_view = false;
                        }

                       
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
            apiService.getDATA("CLGTTCourseReportBWMC/getdetails").
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