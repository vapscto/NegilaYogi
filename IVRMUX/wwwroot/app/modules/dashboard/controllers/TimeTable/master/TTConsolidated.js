
(function () {
    'use strict';
    angular
        .module('app')
        .controller('TTConsolidatedController', TTConsolidatedController)

    TTConsolidatedController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter', 'Excel', '$timeout']
    function TTConsolidatedController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, Excel, $timeout) {
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        $scope.tadprint = false;
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            // paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.usrname = localStorage.getItem('username');

        //if ($scope.itemsPerPage == undefined || $scope.itemsPerPage == null) {
        //    $scope.itemsPerPage = 5;
        //}
        $scope.labelvalue = 1;
        $scope.coptyright = copty;
        $scope.ddate = new Date();
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.rpttyp = 'SAWC';
        $scope.changeradio = function () {

            $scope.timetablelist = [];
            $scope.stdmainarray = [];
        }
        $scope.imgname = logopath;
        $scope.editEmployee = {};
        $scope.TYPE = 'ST';
        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.rpttypai = false;
        $scope.stdsearchValue = '';
        $scope.csubsearchValue = '';
        $scope.sftpsearchValue = '';
        $scope.freesearchValue = '';
        $scope.spcsearchValue = '';
        $scope.cspcsearchValue = '';
        $scope.itemsPerPage = 10;
        $scope.itemsPerPage1 = 10;
        $scope.itemsPerPage2 = 10;
        $scope.itemsPerPage3 = 10;
        $scope.itemsPerPage4 = 10;
        $scope.itemsPerPagespc = 10;
        $scope.itemsPerPagecspc = 10;
        $scope.currentPage = 1;
        $scope.currentPage1 = 1;
        $scope.currentPage2 = 1;
        $scope.currentPage3 = 1;
        $scope.currentPage4 = 1;
        $scope.currentPagespc = 1;
        $scope.currentPagecspc = 1;
        $scope.ldstafflst = [];
        $scope.ldperiodslst = [];
        $scope.claslist = [];
        $scope.seclist = [];

        $scope.options = {
            hstep: [1, 2, 3],
            mstep: [1, 5, 10, 15, 25, 30]
        };
        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        $scope.dayslst_fix = [];
        //TO get data on load
        $scope.LoadData = function () {

            apiService.getDATA("TTConsolidated/getalldetails").then(function (promise) {
                $scope.year_list = promise.acayear;
                $scope.category_list = promise.categorylist;
                //$scope.class_list = promise.classlist;
                //$scope.temp_classlist = promise.classlists;
                $scope.cls_List = promise.gridcls;
                $scope.dayslst_fix = promise.dayslst_fix;
                $scope.ldstafflst = promise.stafflst;
                $scope.ldperiodslst = promise.periodslst;
                $scope.cateoglst = promise.cateoglst;
                $scope.claslist = promise.classlists;
                $scope.seclist = promise.sectionlists;
            });
        };

        $scope.all_check = function () {
            var toggleStatus = $scope.usercheck;
            angular.forEach($scope.ldstafflst, function (itm) {
                itm.stf = toggleStatus;
            });

        }
        $scope.togchkbx = function () {
            $scope.usercheck = $scope.ldstafflst.every(function (options) {
                return options.stf;
            });
        }

        $scope.isOptionsRequired = function () {
            if ($scope.TYPE == 'CSUB') {
                return !$scope.ldstafflst.some(function (options) {
                    return options.stf;
                });
            }

        }
        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.empName).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        }
        ///day check box
        $scope.searchchkbx1 = "";
        $scope.filterchkbx1 = function (obj) {
            return angular.lowercase(obj.ttmD_DayName).indexOf(angular.lowercase($scope.searchchkbx1)) >= 0;
        }


        $scope.all_check1 = function () {
            var toggleStatus = $scope.usercheck1;
            angular.forEach($scope.dayslst_fix, function (itm) {
                itm.sayy = toggleStatus;
            });

        }
        $scope.togchkbx1 = function () {
            $scope.usercheck1 = $scope.dayslst_fix.every(function (options) {
                return options.sayy;
            });
        }

        $scope.isOptionsRequired1 = function () {
            if ($scope.TYPE == 'CSUB' || $scope.TYPE == 'STSUB') {
                return !$scope.dayslst_fix.some(function (options) {
                    return options.sayy;
                });
            }

        }

        $scope.searchclasschkbx = "";
        $scope.filterclasschkbx = function (obj) {
            return angular.lowercase(obj.asmcL_ClassName).indexOf(angular.lowercase($scope.searchclasschkbx)) >= 0;
        }

        $scope.searchsectionchkbx = "";
        $scope.filtersectionchkbx = function (obj) {
            return angular.lowercase(obj.asmC_SectionName).indexOf(angular.lowercase($scope.searchsectionchkbx)) >= 0;
        }

        $scope.all_checkclass = function (userclasscheck) {
            var toggleStatus = userclasscheck;
            angular.forEach($scope.claslist, function (itm) {
                itm.claslis = toggleStatus;
               
               
            });
        }

        $scope.togchkbx4 = function () {
            $scope.userclasscheck = $scope.claslist.every(function (options) {
                return options.claslis;
            });
        }
        $scope.togchkbx5 = function () {
            $scope.usersectioncheck = $scope.seclist.every(function (options) {
                return options.secs;
            });
        }
        $scope.all_checksection = function () {
            var toggleStatus = $scope.usersectioncheck;
            angular.forEach($scope.seclist, function (itm) {
                itm.secs = toggleStatus;
            });
        }

        $scope.isOptionsRequired5 = function () {
            if ($scope.TYPE == 'ST') {

                return !$scope.seclist.some(function (options) {
                    return options.secs;
                });
            }
        }
        $scope.isOptionsRequired4 = function () {
            if ($scope.TYPE == 'ST') {
                return !$scope.claslist.some(function (options) {
                    return options.claslis;
                });
            }
        }


        ///Period check box
        $scope.searchchkbx2 = "";
        $scope.filterchkbx2 = function (obj) {
            return angular.lowercase(obj.ttmP_PeriodName).indexOf(angular.lowercase($scope.searchchkbx2)) >= 0;
        }


        $scope.all_check2 = function () {
            var toggleStatus = $scope.usercheck2;
            angular.forEach($scope.ldperiodslst, function (itm) {
                itm.ppr = toggleStatus;
            });

        }
        $scope.togchkbx2 = function () {
            $scope.usercheck2 = $scope.ldperiodslst.every(function (options) {
                return options.ppr;
            });
        }

        $scope.isOptionsRequired2 = function () {
            if ($scope.TYPE == 'CSUB' || $scope.TYPE == 'STSUB') {
                return !$scope.ldperiodslst.some(function (options) {
                    return options.ppr;
                });
            }

        }



        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        //----------TO Get Repot
        $scope.submitted = false;

        //$scope.timetablelist = [];
        $scope.GetReport = function () {
            $scope.submitted = true;
            //$scope.timetablelist = [];
            $scope.stdmainarray = [];
            //if ($scope.myForm.$valid) {
            var stfidss = []
            var dayidss = []
            var periodidss = []
            var catedss = []
            var classdss = []
            var sectiondss = []

            if ($scope.TYPE == 'CSUB' || $scope.TYPE == 'STSUB' || $scope.TYPE == 'SPW') {

                angular.forEach($scope.ldstafflst, function (gg) {
                    if (gg.stf == true) {
                        stfidss.push(gg);
                    }

                })
                angular.forEach($scope.dayslst_fix, function (ff) {
                    if (ff.sayy == true) {
                        dayidss.push(ff);
                    }

                })
                angular.forEach($scope.ldperiodslst, function (pp) {
                    if (pp.ppr == true) {
                        periodidss.push(pp);
                    }

                })
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "TYPE": $scope.TYPE,
                    stfidss: stfidss,
                    dayidss: dayidss,
                    periodidss: periodidss,
                }
            }
            else if ($scope.TYPE == 'PSTF') {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "TTMD_Id": $scope.TTMD_Id,
                    "TTMP_Id": $scope.TTMP_Id,
                    "rpttypairods": $scope.rpttypai,
                    "TYPE": $scope.TYPE,
                    stfidss: stfidss,
                    dayidss: dayidss,
                    periodidss: periodidss,
                }
            }
            else if ($scope.TYPE == 'STFP') {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "TTMD_Id": $scope.TTMD_Id,
                    "rpttypairods": $scope.rpttypai,
                    "TYPE": $scope.TYPE,
                    stfidss: stfidss,
                    dayidss: dayidss,
                    periodidss: periodidss,
                }
            }
            else if ($scope.TYPE == 'ATG') {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "TYPE": $scope.TYPE,
                }
            }
            else if ($scope.TYPE == 'STD') {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "TYPE": $scope.TYPE
                }
            }
            //else if ($scope.TYPE == 'SPW') {
            //    var data = {
            //        "ASMAY_Id": $scope.asmaY_Id,
            //        "TYPE": $scope.TYPE,
            //        stfidss: stfidss,
            //        dayidss: dayidss,
            //        periodidss: periodidss,
            //    }
            //}
            else if ($scope.TYPE == 'SPC') {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "TYPE": $scope.TYPE
                }
            }
            else if ($scope.TYPE == 'CSPC') {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "TYPE": $scope.TYPE
                }
            }
            else if ($scope.TYPE == 'RMR') {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "TYPE": $scope.TYPE,
                    "rpttyp": $scope.rpttyp
                }
            }
            else {
                angular.forEach($scope.claslist, function (mm) {
                    if (mm.claslis == true) {

                        classdss.push(mm);

                    }
                })
                angular.forEach($scope.seclist, function (nn) {
                    if (nn.secs == true) {

                        sectiondss.push(nn);

                    }
                })
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "rpttypairods": $scope.rpttypai,
                    "TYPE": $scope.TYPE,
                    "rpttyp": $scope.rpttyp,

                    "TTMC_Id": $scope.ttmC_Id,

                    classdss: classdss,
                    sectiondss: sectiondss,

                    stfidss: stfidss,
                    dayidss: dayidss,
                    periodidss: periodidss,
                }
            }

            apiService.create("TTConsolidated/getrpt", data).
                then(function (promise) {
                    debugger;
                    $scope.yearrname = '';
                    angular.forEach($scope.year_list, function (ff) {
                        if ($scope.asmaY_Id == ff.asmaY_Id) {
                            $scope.yearrname = ff.asmaY_Year;
                        }
                    })

                    $scope.categoryName = '';
                    angular.forEach($scope.cateoglst, function (cc) {
                        if ($scope.ttmC_Id == cc.ttmC_Id) {
                            $scope.categoryName = cc.ttmC_Id;
                        }
                    })

                    $scope.stafflist = promise.stafflst;

                    $scope.period_list = promise.periodslst;

                    $scope.daysList = promise.dayslst;
                    $scope.timetablelist = promise.finaltable;

                    if ($scope.timetablelist.length > 0 && $scope.timetablelist != null) {

                        if ($scope.TYPE == 'ST') {


                            $scope.mainarray = [];
                            var prlist = [];
                            angular.forEach($scope.daysList, function (dd) {

                                angular.forEach($scope.period_list, function (pp) {
                                    var ttlist = [];

                                    angular.forEach($scope.timetablelist, function (tt) {
                                        if (dd.ttmD_Id == tt.TTMD_Id && pp.ttmP_Id == tt.TTMP_Id) {
                                            ttlist.push(tt);
                                        }

                                    })
                                    prlist.push({ dayperiod: dd.ttmD_DayName + '/' + pp.ttmP_PeriodName, TTMD_Id: dd.ttmD_Id, TTMD_DayName: dd.ttmD_DayName, TTMP_Id: pp.ttmP_Id, TTMP_PeriodName: pp.ttmP_PeriodName, TTLIST: ttlist })

                                })

                                // $scope.mainarray.push({ TTMD_Id: dd.ttmD_Id, TTMD_DayName: dd.ttmD_DayName, PPLIST: prlist})
                                $scope.mainarray = prlist

                            })
                            console.log($scope.mainarray);

                            angular.forEach($scope.mainarray, function (rr) {

                                angular.forEach($scope.stafflist, function (ss) {
                                    var cnnnt = 0;
                                    angular.forEach(rr.TTLIST, function (tt) {

                                        if (ss.hrmE_Id == tt.HRME_Id) {
                                            cnnnt += 1;
                                            //  tt['id' + tt.HRME_Id] = tt.StaffSubject;
                                            if (rr['id' + tt.HRME_Id] == undefined || rr['id' + tt.HRME_Id] == '') {
                                                rr['id' + tt.HRME_Id] = tt.StaffSubject;
                                            }
                                            else {
                                                rr['id' + tt.HRME_Id] = rr['id' + tt.HRME_Id] + '&&' + tt.StaffSubject
                                            }

                                        }

                                    })

                                    //if (cnnnt == 0) {
                                    //    rr.TTLIST.push({ HRME_Id: ss.hrmE_Id, EmployeeName: '', TTMSAB_Abbreviation: ss.ttmsaB_Abbreviation, StaffSubject: '', TTMP_Id: rr.TTMP_Id, TTMD_Id: rr.TTMD_Id, ['id'+ss.hrmE_Id]: '' })
                                    //}


                                })

                            })

                            $scope.mainarray1 = $scope.mainarray
                            $scope.details = $scope.mainarray1;

                            //angular.forEach($scope.mainarray1, function (mm) {

                            //    $scope.details = mm.TTLIST;

                            //})


                            //console.log($scope.mainarray1);
                            $scope.colarrayall = [];
                            $scope.colarrayall1 = [];
                            $scope.colarrayall2 = [];
                            $scope.colarrayall.push({
                                title: 'DAY/PERIOD', name: 'dayperiod', field: 'dayperiod', width: 150
                            }),
                                angular.forEach($scope.stafflist, function (mm) {
                                    if ($scope.rpttypai == true) {
                                        $scope.colarrayall.push({ title: mm.empName, name: 'id' + mm.hrmE_Id, field: 'id' + mm.hrmE_Id, width: 150 })
                                    }
                                    else {
                                        $scope.colarrayall.push({ title: mm.ttmsaB_Abbreviation, name: 'id' + mm.hrmE_Id, field: 'id' + mm.hrmE_Id, width: 150 })
                                    }


                                })


                            console.log($scope.colarrayall);
                            console.log($scope.stafflist);
                            console.log($scope.details);
                            $(document).ready(function () {
                                $('#gridhhs').empty();
                                $("#gridhhs").kendoGrid({
                                    toolbar: ["excel"],


                                    excel: {
                                        fileName: "ConsolidatedReport.xls",
                                        //allPages: true,
                                        proxyURL: "",
                                        filterable: true,
                                        allPages: true
                                    },

                                    excelExport: function (e) {

                                        var sheet = e.workbook.sheets[0];
                                        sheet.frozenRows = 2;
                                        sheet.mergedCells = ["A1:BK1"];
                                        sheet.name = "ConsolidatedReport";

                                        var myHeaders = [{
                                            value: "TIMETABLE" + ' -- ' + $scope.yearrname,
                                            fontSize: 20,
                                            textAlign: "center",
                                            background: "#ffffff",
                                            color: "black"
                                        }];

                                        sheet.rows.splice(0, 0, { cells: myHeaders, type: "header", height: 70 });
                                    },

                                    dataSource: {
                                        data: $scope.details,
                                        pageSize: 100
                                    },

                                    sortable: true,
                                    // pageable: true,
                                    groupable: false,
                                    filterable: true,
                                    columnMenu: true,
                                    reorderable: true,
                                    resizable: true,

                                    columns: $scope.colarrayall,
                                    dataBound: function () {
                                        var pagenum = this.dataSource.page();
                                        var pageitms = this.dataSource.pageSize();
                                        var rows = this.items();
                                        $(rows).each(function () {
                                            var index = (pagenum - 1) * pageitms + $(this).index() + 1;
                                            var rowLabel = $(this).find(".row-number");
                                            $(rowLabel).html(index);
                                        });
                                    }
                                });
                            });
                        }
                        else if ($scope.TYPE == 'STD') {
                            $scope.stafflist = promise.stafflst;

                            $scope.stdmainarray = [];
                            angular.forEach($scope.stafflist, function (ss) {
                                var stafttlit = [];
                                var cnt = 0;
                                angular.forEach($scope.timetablelist, function (tt) {
                                    if (ss.hrmE_Id == tt.HRME_Id) {
                                        cnt += tt.PCOUNT;
                                        stafttlit.push(tt)
                                    }
                                })
                                $scope.stdmainarray.push({ HRME_Id: ss.hrmE_Id, NAME: ss.empName, ABR: ss.ttmsaB_Abbreviation, ttlist: stafttlit, TOTAL: cnt })
                            })

                        }

                        else if ($scope.TYPE == 'CSUB') {
                            $scope.subjectlist = promise.subjectlist;
                            $scope.class_sectons = promise.class_sectons;

                            $scope.stdmainarray = [];
                            angular.forEach($scope.subjectlist, function (ss) {
                                var periodlst = [];
                                var cnt = 0;
                                angular.forEach($scope.timetablelist, function (tt) {
                                    if (ss.ismS_Id == tt.ISMS_Id) {
                                        cnt += tt.PCOUNT;
                                        periodlst.push(tt)
                                    }
                                })
                                $scope.stdmainarray.push({ ISMS_Id: ss.ismS_Id, SUBJECT: ss.ismS_SubjectName, ttlist: periodlst, TOTAL: cnt })
                            })

                        }
                        else if ($scope.TYPE == 'STSUB') {
                            $scope.subjectlist = promise.subjectlist;
                            $scope.class_sectons = promise.class_sectons;

                            $scope.stdmainarray = [];
                            angular.forEach($scope.subjectlist, function (ss) {
                                var periodlst = [];
                                var cnt = 0;
                                angular.forEach($scope.timetablelist, function (tt) {
                                    if (ss.ismS_Id == tt.ISMS_Id) {
                                        cnt += 1;
                                        periodlst.push(tt)
                                        //if (cnt==1) {
                                        //    periodlst.push(tt)
                                        //}
                                        //else {
                                        //    tt.ENAME = ' ' + '&&' + ' ' + tt.ENAME;
                                        //    periodlst.push(tt)
                                        //}

                                    }
                                })
                                $scope.stdmainarray.push({ ISMS_Id: ss.ismS_Id, SUBJECT: ss.ismS_SubjectName, ttlist: periodlst })
                            })

                        }
                        else if ($scope.TYPE == 'SSW') {
                            $scope.stafflist = promise.stafflst;

                            angular.forEach($scope.stafflist, function (ss) {
                                var cont = 0;
                                angular.forEach($scope.timetablelist, function (tt) {
                                    if (ss.hrmE_Id == tt.HRME_Id) {
                                        cont += 1;
                                    }


                                })
                                ss.clssecnt = cont;
                            })


                        }
                        else if ($scope.TYPE == 'SPW') {
                            $scope.stafflist = promise.stafflst;

                            $scope.stdmainarray = $scope.period_list;
                            angular.forEach($scope.stdmainarray, function (pp) {
                                angular.forEach($scope.stafflist, function (ss) {

                                    angular.forEach($scope.timetablelist, function (tt) {
                                        if (pp.ttmP_Id == tt.TTMP_Id && ss.hrmE_Id == tt.HRME_Id) {

                                            pp['id' + tt.HRME_Id] = tt.PCOUNT;
                                        }

                                    })


                                })

                            })

                            $scope.colarrayall = [];
                            $scope.colarrayall.push({
                                title: 'PERIOD', name: 'ttmP_PeriodName', field: 'ttmP_PeriodName', width: 150
                            }),
                                angular.forEach($scope.stafflist, function (mm) {

                                    $scope.colarrayall.push({ title: mm.empName, name: 'id' + mm.hrmE_Id, field: 'id' + mm.hrmE_Id, width: 150 })

                                })

                            console.log($scope.stdmainarray);
                            console.log($scope.colarrayall);


                            $(document).ready(function () {
                                $('#SPWGRID').empty();
                                $("#SPWGRID").kendoGrid({
                                    toolbar: ["excel"],


                                    excel: {
                                        fileName: "ConsolidatedReport.xls",
                                        //allPages: true,
                                        proxyURL: "",
                                        filterable: true,
                                        allPages: true
                                    },

                                    excelExport: function (e) {

                                        var sheet = e.workbook.sheets[0];
                                        sheet.frozenRows = 2;
                                        sheet.mergedCells = ["A1:BK1"];
                                        sheet.name = "ConsolidatedReport";

                                        var myHeaders = [{
                                            value: "STAFF PERIOD WISE WORKLOAD" + ' -- ' + $scope.yearrname,
                                            fontSize: 20,
                                            textAlign: "center",
                                            background: "#ffffff",
                                            color: "black"
                                        }];

                                        sheet.rows.splice(0, 0, { cells: myHeaders, type: "header", height: 70 });
                                    },

                                    dataSource: {
                                        data: $scope.stdmainarray,
                                        pageSize: 100
                                    },

                                    sortable: true,
                                    // pageable: true,
                                    groupable: false,
                                    filterable: true,
                                    columnMenu: true,
                                    reorderable: true,
                                    resizable: true,

                                    columns: $scope.colarrayall,
                                    dataBound: function () {
                                        var pagenum = this.dataSource.page();
                                        var pageitms = this.dataSource.pageSize();
                                        var rows = this.items();
                                        $(rows).each(function () {
                                            var index = (pagenum - 1) * pageitms + $(this).index() + 1;
                                            var rowLabel = $(this).find(".row-number");
                                            $(rowLabel).html(index);
                                        });
                                    }
                                });
                            });

                        }
                        else if ($scope.TYPE == 'STFP') {
                            $scope.stafflist = promise.stafflst;
                            angular.forEach($scope.stafflist, function (ss) {
                                var prdlist = [];
                                angular.forEach($scope.timetablelist, function (tt) {
                                    if (tt.HRME_Id == ss.hrmE_Id) {
                                        prdlist.push(tt);
                                    }

                                })
                                if (prdlist.length > 0) {
                                    $scope.stdmainarray.push({ HRME_Id: ss.hrmE_Id, ENAME: ss.empName, VLIST: prdlist })
                                }

                            })


                            console.log($scope.stdmainarray);
                        }
                        else if ($scope.TYPE == 'PSTF') {
                            $scope.stdmainarray = $scope.timetablelist;
                        }
                        else if ($scope.TYPE == 'SPC') {
                            $scope.stdtt = promise.finaltable;
                            $scope.prdlst = promise.periodslst;
                            $scope.stffflst = promise.stafflst;

                            $scope.stdmainarray = [];
                            //angular.forEach($scope.stffflst, function (ss) {
                            //    var periodlst = [];
                            //        angular.forEach($scope.stdtt, function (tt) {

                            //            if (ss.hrmE_Id == tt.HRME_Id) {
                            //                periodlst.push(tt);
                            //            }

                            //        })

                            //    $scope.stdmainarray.push({ HRME_Id: ss.hrmE_Id, ENAME: ss.empName, ABR: ss.ttmsaB_Abbreviation, plist: periodlst})

                            //})
                            $scope.stdmainarray = [];
                            angular.forEach($scope.stffflst, function (ss) {
                                $scope.periodlsss = [];
                                angular.forEach($scope.prdlst, function (pp) {
                                    var newdata = [];
                                    var count = 0;
                                    var newCol = "";
                                    angular.forEach($scope.stdtt, function (tt) {
                                        if (ss.hrmE_Id == tt.HRME_Id && pp.ttmP_Id == tt.TTMP_Id) {


                                            if (count == 0) {


                                                if (tt.MINID == tt.MAXID) {
                                                    var prd = 1;
                                                    newCol = tt.ASMCL_ClassName + '::' + tt.ASMC_SectionName + '::' + tt.ISMS_SubjectName + '::' + tt.MinDay + '::' + prd
                                                }
                                                else {
                                                    var prd1 = tt.MAXID - tt.MINID + 1;
                                                    newCol = tt.ASMCL_ClassName + '::' + tt.ASMC_SectionName + '::' + tt.ISMS_SubjectName + '::' + tt.MinDay + '-' + tt.MaxDay + '::' + prd1
                                                }


                                            }

                                            if (count > 0) {
                                                var prd = 1;

                                                if (tt.MINID == tt.MAXID) {
                                                    newCol = newCol + ' ' + '&&' + ' ' + tt.ASMCL_ClassName + '::' + tt.ASMC_SectionName + '::' + tt.ISMS_SubjectName + '::' + tt.MinDay + '::' + prd;
                                                }
                                                else {
                                                    var prd1 = tt.MAXID - tt.MINID + 1;
                                                    newCol = newCol + ' ' + '&&' + ' ' + tt.ASMCL_ClassName + '::' + tt.ASMC_SectionName + '::' + tt.ISMS_SubjectName + '::' + tt.MinDay + '-' + tt.MaxDay + '::' + prd1;
                                                }
                                            }


                                            count += 1;

                                        }


                                    })


                                    $scope.periodlsss.push({ TTMP_Id: pp.ttmP_Id, Details: newCol })

                                })

                                $scope.stdmainarray.push({ HRME_Id: ss.hrmE_Id, ENAME: ss.empName, ABR: ss.ttmsaB_Abbreviation, plist: $scope.periodlsss })

                            })




                            console.log($scope.stdmainarray);

                        }
                        else if ($scope.TYPE == 'CSPC') {
                            $scope.stdtt = promise.finaltable;
                            $scope.prdlst = promise.periodslst;
                            $scope.class_sectons = promise.class_sectons;

                            //$scope.stdmainarray = [];
                            //angular.forEach($scope.class_sectons, function (ss) {
                            //    var periodlst = [];
                            //        angular.forEach($scope.stdtt, function (tt) {

                            //            if (ss.asmcL_Id == tt.ASMCL_Id && ss.asmS_Id == tt.ASMS_Id) {
                            //                periodlst.push(tt);
                            //            }

                            //        })

                            //    $scope.stdmainarray.push({ ASMCL_ClassName: ss.asmcL_ClassName, plist: periodlst})

                            //})


                            $scope.stdmainarray = [];
                            angular.forEach($scope.class_sectons, function (ss) {
                                $scope.periodlsss = [];
                                angular.forEach($scope.prdlst, function (pp) {
                                    var newdata = [];
                                    var count = 0;
                                    var newCol = "";
                                    debugger;
                                    angular.forEach($scope.stdtt, function (tt) {
                                        if (ss.asmcL_Id == tt.ASMCL_Id && ss.asmS_Id == tt.ASMS_Id && pp.ttmP_Id == tt.TTMP_Id) {


                                            if (count == 0) {


                                                if (tt.MINID == tt.MAXID) {
                                                    var prd = 1;
                                                    newCol = tt.EmpName + '::' + tt.ISMS_SubjectName + '::' + tt.MinDay + '::' + prd
                                                }
                                                else {
                                                    var prd1 = tt.MAXID - tt.MINID + 1;
                                                    newCol = tt.EmpName + '::' + tt.ISMS_SubjectName + '::' + tt.MinDay + '-' + tt.MaxDay + '::' + prd1
                                                }


                                            }

                                            if (count > 0) {
                                                var prd = 1;

                                                if (tt.MINID == tt.MAXID) {
                                                    newCol = newCol + ' ' + '&&' + ' ' + tt.EmpName + '::' + tt.ISMS_SubjectName + '::' + tt.MinDay + '::' + prd;
                                                }
                                                else {
                                                    var prd1 = tt.MAXID - tt.MINID + 1;
                                                    newCol = newCol + ' ' + '&&' + ' ' + tt.EmpName + '::' + tt.ISMS_SubjectName + '::' + tt.MinDay + '-' + tt.MaxDay + '::' + prd1;
                                                }
                                            }


                                            count += 1;

                                        }


                                    })


                                    $scope.periodlsss.push({ TTMP_Id: pp.ttmP_Id, Details: newCol })

                                })

                                $scope.stdmainarray.push({ ASMCL_ClassName: ss.asmcL_ClassName, plist: $scope.periodlsss })

                            })


                            console.log($scope.stdmainarray);

                        }

                        else if ($scope.TYPE == 'RMR') {
                            debugger;
                            $scope.brklist = promise.tT_Break_list_all;
                            $scope.roomlst = promise.roomlst;

                            $scope.mainarrayclm = [];

                            angular.forEach($scope.daysList, function (dd) {
                                var prlist = [];
                                angular.forEach($scope.period_list, function (pp) {
                                    var cnt1 = 0;
                                    var cnt = 0;
                                    var ppid = '';
                                    var ppname = '';
                                    angular.forEach($scope.brklist, function (tt) {

                                        cnt1 += 1;


                                        if (parseInt(pp.ttmP_PeriodName) == tt.ttmB_AfterPeriod && tt.ttmD_Id == dd.ttmD_Id) {/*&& */
                                            debugger;
                                            cnt += 1;
                                            ppid = 'id' + pp.ttmP_Id + dd.ttmD_Id + 'id';
                                            ppname = 'BREAK';
                                        }

                                    })
                                    if (cnt == 0) {
                                        prlist.push({ pid: pp.ttmP_Id, pname: pp.ttmP_PeriodName, title: 'P-' + pp.ttmP_PeriodName, field: 'id' + pp.ttmP_Id + dd.ttmD_Id, name: 'id' + pp.ttmP_Id + dd.ttmD_Id, width: 150 })
                                    }
                                    if (cnt > 0) {
                                        debugger;
                                        prlist.push({ pid: pp.ttmP_Id, pname: pp.ttmP_PeriodName, title: 'P-' + pp.ttmP_PeriodName, field: 'id' + pp.ttmP_Id + dd.ttmD_Id, name: 'id' + pp.ttmP_Id + dd.ttmD_Id, width: 150 })
                                        prlist.push({ pid: ppid, pname: ppname, title: ppname, field: ppid, name: ppid, width: 150 })
                                    }

                                })


                                $scope.mainarrayclm.push({ did: dd.ttmD_Id, dname: dd.ttmD_DayName, plistm: prlist })

                            })

                            var cc = $scope.mainarrayclm.length;

                            $scope.colarrayall = [];

                            $scope.colarrayall.push({ title: 'Room Name', field: 'ttmrM_RoomName', name: 'ttmrM_RoomName', width: 200 })
                            var ddc = 0;
                            angular.forEach($scope.mainarrayclm, function (ee) {
                                ddc += 1;
                                $scope.colarrayall.push({ title: ee.dname, field: 'idss' + ee.did, name: 'idss' + ee.did, columns: ee.plistm })

                                if (ddc <= cc) {
                                    $scope.colarrayall.push({ title: '--', field: 'a', name: '', width: 50 })
                                }

                            })


                            console.log($scope.colarrayall);

                            //angular.forEach($scope.mainarray, function (rr) {

                            //    angular.forEach($scope.stafflist, function (ss) {
                            //        var cnnnt = 0;
                            //        angular.forEach(rr.TTLIST, function (tt) {

                            //            if (ss.hrmE_Id == tt.HRME_Id) {
                            //                cnnnt += 1;

                            //                if (rr['id' + tt.HRME_Id] == undefined || rr['id' + tt.HRME_Id] == '') {
                            //                    rr['id' + tt.HRME_Id] = tt.StaffSubject;
                            //                }
                            //                else {
                            //                    rr['id' + tt.HRME_Id] = rr['id' + tt.HRME_Id] + '&&' + tt.StaffSubject
                            //                }

                            //            }

                            //        })



                            //    })

                            //})


                            angular.forEach($scope.roomlst, function (xx) {
                                xx.a = ' ';
                                angular.forEach($scope.daysList, function (dd) {
                                    var prlist = [];
                                    angular.forEach($scope.period_list, function (pp) {
                                        var cnt1 = 0;
                                        var cnt = 0;
                                        var ppid = '';
                                        var ppname = '';
                                        angular.forEach($scope.brklist, function (tt) {

                                            cnt1 += 1;


                                            if (parseInt(pp.ttmP_PeriodName) == tt.ttmB_AfterPeriod && tt.ttmD_Id == dd.ttmD_Id) {/*&& */
                                                debugger;
                                                cnt += 1;
                                                ppid = 'id' + pp.ttmP_Id + dd.ttmD_Id + 'id';
                                                ppname = 'BREAK';

                                                xx[ppid] = tt.ttmB_BreakName
                                            }

                                        })


                                        angular.forEach($scope.timetablelist, function (zz) {
                                            if (zz.TTMP_Id == pp.ttmP_Id && zz.TTMD_Id == dd.ttmD_Id && zz.TTMRM_Id == xx.ttmrM_Id) {
                                                xx['id' + pp.ttmP_Id + dd.ttmD_Id] = zz.StaffSubject;
                                            }


                                        })


                                        //if (cnt == 0) {
                                        //    prlist.push({ pid: pp.ttmP_Id, pname: pp.ttmP_PeriodName, title: 'P-' + pp.ttmP_PeriodName, field: 'id' + pp.ttmP_Id + dd.ttmD_Id, name: 'id' + pp.ttmP_Id + dd.ttmD_Id, width: 150 })
                                        //}
                                        //if (cnt > 0) {
                                        //    debugger;
                                        //    prlist.push({ pid: pp.ttmP_Id, pname: pp.ttmP_PeriodName, title: 'P-' + pp.ttmP_PeriodName, field: 'id' + pp.ttmP_Id + dd.ttmD_Id, name: 'id' + pp.ttmP_Id + dd.ttmD_Id, width: 150 })
                                        //    prlist.push({ pid: ppid, pname: ppname, title: 'P-' + ppname, field: ppid, name: ppid, width: 150 })
                                        //}

                                    })


                                    //$scope.mainarrayclm.push({ did: dd.ttmD_Id, dname: dd.ttmD_DayName, plistm: prlist })

                                })


                            })




                            $scope.mainarray1 = $scope.mainarray
                            $scope.details = $scope.mainarray1;


                            console.log($scope.colarrayall);

                            $(document).ready(function () {
                                $('#gridhhs1').empty();
                                $("#gridhhs1").kendoGrid({
                                    toolbar: ["excel"],


                                    excel: {
                                        fileName: "ConsolidatedReport.xls",

                                        proxyURL: "",
                                        filterable: true,
                                        allPages: true
                                    },

                                    excelExport: function (e) {

                                        var sheet = e.workbook.sheets[0];
                                        sheet.frozenRows = 2;
                                        sheet.mergedCells = ["A1:BK1"];
                                        sheet.name = "ConsolidatedReport";

                                        var myHeaders = [{
                                            value: " ROOM  WISE TIMETABLE REPORT" + ' -- ' + $scope.yearrname,
                                            fontSize: 20,
                                            textAlign: "center",
                                            background: "#ffffff",
                                            color: "black"
                                        }];

                                        sheet.rows.splice(0, 0, { cells: myHeaders, type: "header", height: 70 });
                                    },

                                    dataSource: {
                                        data: $scope.roomlst,
                                        pageSize: 100
                                    },

                                    sortable: true,

                                    groupable: false,
                                    filterable: true,
                                    columnMenu: true,
                                    reorderable: true,
                                    resizable: true,

                                    columns: $scope.colarrayall,
                                    dataBound: function () {
                                        var pagenum = this.dataSource.page();
                                        var pageitms = this.dataSource.pageSize();
                                        var rows = this.items();
                                        $(rows).each(function () {
                                            var index = (pagenum - 1) * pageitms + $(this).index() + 1;
                                            var rowLabel = $(this).find(".row-number");
                                            $(rowLabel).html(index);
                                        });
                                    }
                                });
                            });


                        }
                        else if ($scope.TYPE == 'ATG') {
                            $scope.stdtt = promise.finaltable;

                            $scope.colarrayall = [];

                            $scope.colarrayall.push({ title: 'STAFF', field: 'ENAME', name: 'ENAME', width: 250 });
                            $scope.colarrayall.push({ title: 'CLASS', field: 'ASMCL_ClassName', name: 'ASMCL_ClassName', width: 100 });
                            $scope.colarrayall.push({ title: 'SECTION', field: 'ASMC_SectionName', name: 'ASMC_SectionName', width: 100 });
                            $scope.colarrayall.push({ title: 'SUBJECT', field: 'ISMS_SubjectName', name: 'ISMS_SubjectName', width: 200 });
                            $scope.colarrayall.push({
                                title: 'PERIODS COUNT', field: 'PCOUNT', name: 'PCOUNT', width: 200, aggregates: ["sum"], footerTemplate: "TOTAL: #=sum#",
                                groupFooterTemplate: "TOTAL: #=sum#"
                            });
                            $scope.tempaggary = [];

                            $scope.tempaggary.push({ field: 'PCOUNT', name: 'PCOUNT', aggregate: "sum" });

                            $(document).ready(function () {
                                $('#gridhhs12').empty();
                                $("#gridhhs12").kendoGrid({
                                    toolbar: ["excel"],


                                    excel: {
                                        fileName: "ConsolidatedReport.xls",

                                        proxyURL: "",
                                        filterable: true,
                                        allPages: true
                                    },

                                    excelExport: function (e) {

                                        var sheet = e.workbook.sheets[0];
                                        sheet.frozenRows = 2;
                                        sheet.mergedCells = ["A1:F1"];
                                        sheet.name = "ConsolidatedReport";

                                        var myHeaders = [{
                                            value: " STAFF WORKLOAD AFTER TIMETABLE GENERATION" + ' -- ' + $scope.yearrname,
                                            fontSize: 20,
                                            textAlign: "center",
                                            background: "#ffffff",
                                            color: "black"
                                        }];

                                        sheet.rows.splice(0, 0, { cells: myHeaders, type: "header", height: 70 });
                                    },

                                    dataSource: {
                                        data: $scope.stdtt,
                                        pageSize: 100,
                                        aggregate: $scope.tempaggary
                                    },

                                    sortable: true,

                                    groupable: false,
                                    filterable: true,
                                    columnMenu: true,
                                    reorderable: true,
                                    resizable: true,

                                    columns: $scope.colarrayall,
                                    dataBound: function () {
                                        var pagenum = this.dataSource.page();
                                        var pageitms = this.dataSource.pageSize();
                                        var rows = this.items();
                                        $(rows).each(function () {
                                            var index = (pagenum - 1) * pageitms + $(this).index() + 1;
                                            var rowLabel = $(this).find(".row-number");
                                            $(rowLabel).html(index);
                                        });
                                    }
                                });
                            });

                        }
                    }
                    else {
                        swal("NO RECORD FOUND");
                    }



                })

        }


        $scope.printData = function () {

            var innerContents = '';
            if ($scope.TYPE == 'STD') {
                innerContents = document.getElementById("STDPRNT").innerHTML;
            }
            else if ($scope.TYPE == 'CSUB') {
                innerContents = document.getElementById("CSUBPRNT").innerHTML;
            }
            else if ($scope.TYPE == 'STSUB') {
                innerContents = document.getElementById("STSUBPRNT").innerHTML;
            }
            else if ($scope.TYPE == 'STFP') {
                innerContents = document.getElementById("STFPPRNT").innerHTML;
            }
            else if ($scope.TYPE == 'PSTF') {
                innerContents = document.getElementById("PSTFPRNT").innerHTML;
            }
            else if ($scope.TYPE == 'SPC') {
                innerContents = document.getElementById("SPCPRNT").innerHTML;
            }
            else if ($scope.TYPE == 'CSPC') {
                innerContents = document.getElementById("CSPCPRNT").innerHTML;
            }


            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/preadmission/RegReportPdf.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); },900);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();

        }

        $scope.exportToExcel = function () {
            var tabel1 = '';
            if ($scope.TYPE == 'STD') {
                tabel1 = '#STDEXCEL';
            }
            else if ($scope.TYPE == 'CSUB') {
                tabel1 = '#CSUBEXCEL';
            }
            else if ($scope.TYPE == 'STSUB') {
                tabel1 = '#STSUBEXCEL';
            }
            else if ($scope.TYPE == 'STFP') {
                tabel1 = '#STFPEXCEL';
            }
            else if ($scope.TYPE == 'PSTF') {
                tabel1 = '#PSTFEXCEL';
            }
            else if ($scope.TYPE == 'SPC') {
                tabel1 = '#SPCEXCEL';
            }
            else if ($scope.TYPE == 'CSPC') {
                tabel1 = '#CSPCEXCEL';
            }
            var exportHref = Excel.tableToExcel(tabel1, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 900);
        }
        //------TO clear  data
        $scope.clearid = function () {
            $state.reload();
        };

        //to get class by category      
        $scope.ttmC_Id = "";
        $scope.asmaY_Id = "";
        $scope.get_class = function () {

            if ($scope.asmaY_Id === "") {
                swal("Please Select The Academic Year !");
                $scope.ttmC_Id = "";
            }
            else if ($scope.ttmC_Id != "" && $scope.asmaY_Id != "") {
                var data = {
                    "TTMC_Id": $scope.ttmC_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                }
                apiService.create("TTConsolidated/getclass_catg", data).
                    then(function (promise) {

                        $scope.claslist = promise.classlists;
                        $scope.asmcL_Id = "";
                        $scope.userclasscheck = 0;
                        $scope.cls_flag = false;
                        if (promise.classlists == "" || promise.classlists == null) {
                            swal("No classes Are Mapped To Selected Category");
                        }
                    })
            }
        };

        //------TO clear  data
        $scope.clearid = function () {
            $state.reload();
        };

        //$scope.interacted1 = function (field) {

        //    return $scope.submitted || field.$dirty;
        //};


        //$scope.interacted = function (field) {

        //    return $scope.submitted || field.$dirty;
        //};
    }

})();