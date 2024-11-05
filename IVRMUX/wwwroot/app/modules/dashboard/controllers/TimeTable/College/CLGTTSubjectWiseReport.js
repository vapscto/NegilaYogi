
(function () {
    'use strict';
    angular
.module('app')
        .controller('CLGTTSubjectWiseReport', CLGTTSubjectWiseReport)

    CLGTTSubjectWiseReport.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', 'Excel', '$timeout', '$http', '$q', '$stateParams', '$filter']
    function CLGTTSubjectWiseReport($rootScope, $scope, $state, $location, apiService, Flash, Excel, $timeout, $http, $q, $stateParams, $filter) {

        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.amcO_CourseName).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        }

        $scope.searchchkbx3 = "";
        $scope.filterchkbx3 = function (obj) {
            return angular.lowercase(obj.amB_BranchName).indexOf(angular.lowercase($scope.searchchkbx3)) >= 0;
        }

        $scope.searchchkbx4 = "";
        $scope.filterchkbx4 = function (obj) {
            return angular.lowercase(obj.amsE_SEMName).indexOf(angular.lowercase($scope.searchchkbx4)) >= 0;
        }
        $scope.searchchkbx5 = "";
        $scope.filterchkbx5 = function (obj) {
            return angular.lowercase(obj.acmS_SectionName).indexOf(angular.lowercase($scope.searchchkbx5)) >= 0;
        }
            
        $scope.searchchkbx1 = "";
        $scope.filterchkbx1 = function (obj) {
            return angular.lowercase(obj.ismS_SubjectName).indexOf(angular.lowercase($scope.searchchkbx1)) >= 0;
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

        $scope.isOptionsRequired = function () {

            return !$scope.class.some(function (options) {
                return options.cls;
            });
        }

        $scope.isOptionsRequired3 = function () {
            return !$scope.branch.some(function (options) {

                return options.cls;
            })
        }
        $scope.isOptionsRequired4 = function () {
            return !$scope.semister.some(function (options){
                return options.cls;
            })
        }
        $scope.isOptionsRequired5 = function () {
            return !$scope.section.some(function (options) {
                return options.cls;
            })
        }


        $scope.isOptionsRequired1 = function () {

            return !$scope.subject.some(function (options) {
                return options.subje;
            });
        }



        $scope.interacted = function (field) {

            return $scope.submitted;
        };

        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function () {
            
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                $scope.albumNameArray1 = [];
                $scope.albumNameArray2 = [];
                $scope.albumNameArray3 = [];
                $scope.albumNameArray4 = [];
                $scope.albumNameArray5 = [];
                //course
                angular.forEach($scope.class, function (role) {
                    if (role.cls) $scope.albumNameArray1.push(role);
                })
                //subject
                angular.forEach($scope.subject, function (role) {
                    if (role.subje) $scope.albumNameArray2.push(role);
                })
                //branch
                angular.forEach($scope.branch, function (role) {
                    if (role.cls) $scope.albumNameArray3.push(role);
                })
                //semister
                angular.forEach($scope.semister, function (role) {
                    if (role.cls) $scope.albumNameArray4.push(role);
                })
                //section
                angular.forEach($scope.section, function (role) {
                    if (role.cls) $scope.albumNameArray5.push(role);
                })
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    classarray: $scope.albumNameArray1,
                    subarray: $scope.albumNameArray2,
                    brharray: $scope.albumNameArray3,
                    semarray: $scope.albumNameArray4,
                    secarray: $scope.albumNameArray5,
                }
                apiService.create("CLGTTSubjectWiseReport/savedetail", data).
                    then(function (promise) {



                        $scope.getreportdata = promise.getreportdata;

                        if ($scope.getreportdata.length > 0) {
                            $scope.grid_view = true;
                            $scope.mainlist = [];
                            angular.forEach($scope.albumNameArray2, function (ss) {
                                $scope.daymainlist = [];
                                angular.forEach($scope.day_list, function (dd) {
                                    $scope.periodsmainlist = [];
                                    angular.forEach($scope.period_list, function (pp) {

                                        angular.forEach($scope.getreportdata, function (tt) {
                                          
                                            if (ss.ismS_Id == tt.ISMS_Id && dd.ttmD_Id == tt.TTMD_Id && pp.ttmP_Id == tt.TTMP_Id) {
                                                $scope.periodsmainlist.push(tt);

                                            }

                                        })



                                    })


                                    $scope.daymainlist.push({ TTMD_Id: dd.ttmD_Id, TTMD_DayName: dd.ttmD_DayName, periodlst: $scope.periodsmainlist })


                                })

                                $scope.mainlist.push({ ISMS_Id: ss.ismS_Id, EMPNAME: ss.ismS_SubjectName, daywiselist: $scope.daymainlist });




                            })

                            console.log($scope.mainlist);

                        }
                        else {
                            swal("TimeTable is Not Generated For Selected Details !!!!");
                            $scope.grid_view = false;
                        }






//2,3 html





                        //if (promise.tt != null && promise.tt != "" && promise.periodslst != null && promise.periodslst != "" && promise.gridweeks != null && promise.gridweeks != "") {
                        //    $scope.table_list_sub_wise = [];
                        //    $scope.subject_Name = "";
                        //    for (var a = 0; a < $scope.albumNameArray2.length; a++) {
                        //        $scope.grid_view = true;
                        //        $scope.period_list = promise.periodslst;
                        //        $scope.day_list = promise.gridweeks;
                        //        $scope.tt_list = promise.tt;
                        //        var temp_array = [];
                        //        $scope.table_list = [];

                        //        $scope.subject_Name = $scope.albumNameArray2[a].ismS_SubjectName;

                        //        for (var j = 0; j < $scope.day_list.length; j++) {
                        //            temp_array = [];
                        //            for (var i = 0; i < $scope.period_list.length; i++) {
                        //                var count = 0;
                        //                var newCol = "";
                        //                for (var k = 0; k < $scope.tt_list.length; k++) {

                        //                    if ($scope.tt_list[k].ttmP_Id == $scope.period_list[i].ttmP_Id && $scope.tt_list[k].ttmD_Id == $scope.day_list[j].ttmD_Id && $scope.tt_list[k].ismS_Id == $scope.albumNameArray2[a].ismS_Id) {

                        //                        if (count == 0) {
                        //                            newCol = { pedid: $scope.period_list[i].ttmP_Id, dayid: $scope.day_list[j].ttmD_Id, value: $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].ismS_SubjectName, pedname: $scope.period_list[i].ttmP_PeriodName, dayname: $scope.day_list[j].ttmD_DayName, color: 'black', value1: $scope.tt_list[k].staffName + ' :' + $scope.tt_list[k].ismS_SubjectName, value2: $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].staffName + ' :' + $scope.tt_list[k].ismS_SubjectName }
                        //                            count += 1;
                        //                        }
                        //                        else if (count > 0) {
                        //                            newCol.value = newCol.value + ' ' + $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].ismS_SubjectName;
                        //                            newCol.value1 = newCol.value1 + ' ' + $scope.tt_list[k].staffName + ' :' + $scope.tt_list[k].ismS_SubjectName;
                        //                            newCol.value2 = newCol.value2 + ' ' + $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].staffName;
                        //                            count += 1;
                        //                        }
                        //                    }

                        //                }
                        //                if (newCol == "") {
                        //                    newCol = { pedid: $scope.period_list[i].ttmP_Id, dayid: $scope.day_list[j].ttmD_Id, value: ' ', pedname: $scope.period_list[i].ttmP_PeriodName, dayname: $scope.day_list[j].ttmD_DayName, color: 'black' }
                        //                }
                        //                temp_array.push(newCol);
                        //                newCol = "";
                        //                count = 0;
                        //            }

                        //            $scope.table_list.push(temp_array);
                        //        }
                        //        $scope.table_list_sub_wise.push({ array: $scope.table_list, header: $scope.subject_Name });
                        //    }
                        //}
                        //else {
                        //    swal('Data Not Found !');
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
       // $scope.BindData = function () {
            
       //     apiService.getDATA("CLGTTSubjectWiseReport/getdetails").
       //then(function (promise) {
       //    $scope.subject = promise.subdrp;
       //    $scope.class = promise.clsdrp;
       //    $scope.Academic = promise.year;

       //})
        // };
        $scope.BindData = function () {
           
            var pageid = 2;
            apiService.getURI("CLGTTSubjectWiseReport/getdetails", pageid).then(function (promise) {

                $scope.Academic = promise.year;
                $scope.class = promise.courselist;
                $scope.section = promise.sectionlist;
                $scope.subject = promise.subjectlist;
                $scope.period_list = promise.periodslst;
                $scope.day_list = promise.gridweeks;

            })
        }
        $scope.acade = function () {
 $scope.branch = [];
            $scope.semister = [];
        }


        $scope.getbranch = function () {    
            $scope.crsarray = [];
            angular.forEach($scope.class, function (dd) {
                if (dd.cls == true) {
                    $scope.crsarray.push(dd);
                }
            })
            if ($scope.crsarray.length > 0) {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    coursels: $scope.crsarray,
                }
                apiService.create("CLGTTSubjectWiseReport/getbranch", data).then(function (promise) {
                    $scope.branch = promise.branchlist;
                })
            }
            else {
                $scope.branch = [];
                $scope.semister = [];
            }           
        }
        $scope.getsemister = function () {
            $scope.crsarray = [];
            angular.forEach($scope.class, function (dd) {
                if (dd.cls == true) {
                    $scope.crsarray.push(dd);
                }
            })
            $scope.brcarray = [];
            angular.forEach($scope.branch, function (gg) {
                if (gg.cls == true) {
                    $scope.brcarray.push(gg);
                }
            })
            if ($scope.brcarray.length > 0) {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    coursels: $scope.crsarray,
                    branchls: $scope.brcarray,
                }
                apiService.create("CLGTTSubjectWiseReport/getsemister", data).then(function (promise) {
                    $scope.semister = promise.semisterlist;

                })
            }
            else {
                $scope.semister = [];
            }
        }
        



        //TO clear  data
        $scope.clearid = function () {

            $state.reload();
        };

        $scope.all_check1 = function () {
            var toggleStatus = $scope.usercheck1;
            angular.forEach($scope.class, function (itm) {
                itm.cls = toggleStatus;
            });
            $scope.getbranch();
        };
        $scope.all_check3 = function () {
            var toggleStatus = $scope.usercheck3;
            angular.forEach($scope.branch, function (itm) {
                itm.cls = toggleStatus;
            });
            $scope.getsemister();
        };
        $scope.all_check4 = function () {
            var toggleStatus = $scope.usercheck4;
            angular.forEach($scope.semister, function (itm) {
                itm.cls = toggleStatus;
            });
            //$scope.getsection;
        }
        $scope.all_check5 = function () {
            var toggleStatus = $scope.usercheck5;
            angular.forEach($scope.section, function (itm) {
                itm.cls = toggleStatus;
            });
        }
        $scope.togchkbx1 = function () {
            $scope.usercheck1 = $scope.class.every(function (options) {
                return options.cls;
            });
            $scope.getbranch();
        }

        $scope.togchkbx5 = function () {
            $scope.usercheck1 = $scope.section.every(function (options) {
                return options.cls;
            });
           
        }
        $scope.togchkbx3 = function () {
            $scope.usercheck3 = $scope.branch.every(function (options) {
                return options.cls;
            });
            $scope.getsemister();
        };
        $scope.togchkbx4 = function () {
            $scope.usercheck4 = $scope.semister.every(function (options) {
                return options.cls;
            });
            $scope.getsubject();
        }
        $scope.all_check2 = function () {
            var toggleStatus = $scope.usercheck2;
            angular.forEach($scope.subject, function (itm) {
                itm.subje = toggleStatus;
            });
        };
        $scope.togchkbx2 = function () {
            $scope.usercheck2 = $scope.subject.every(function (options) {
                return options.subje;
            });
        }
    }

})();