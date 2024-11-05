
(function () {
    'use strict';
    angular
.module('app')
.controller('ClassWiseTTController', ClassWiseTTController)

        ClassWiseTTController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', 'Excel', '$timeout', '$http', '$q', '$stateParams', '$filter']
        function ClassWiseTTController($rootScope, $scope, $state, $location, apiService, Flash, Excel, $timeout, $http, $q, $stateParams, $filter) {

        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.asmcL_ClassName).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        }

        $scope.searchchkbx1 = "";
        $scope.filterchkbx1 = function (obj) {
            return angular.lowercase(obj.asmC_SectionName).indexOf(angular.lowercase($scope.searchchkbx1)) >= 0;
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
            $scope.usercheck = $scope.class_list.every(function (options) {
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
            angular.forEach($scope.class_list, function (itm) {
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
            return !$scope.class_list.some(function (options) {
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

        // TO Save The Data
        $scope.submitted = false;
        $scope.GetReport = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                $scope.albumNameArray1 = [];
                $scope.albumNameArray2 = [];

                angular.forEach($scope.class_list, function (role) {
                    if (role.class) $scope.albumNameArray1.push(role);
                })
                angular.forEach($scope.section_list, function (role) {
                    if (role.sec) $scope.albumNameArray2.push(role);
                })
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "TTMC_Id": $scope.ttmC_Id,
                    classarray: $scope.albumNameArray1,
                    sectionarray: $scope.albumNameArray2,
                }
                apiService.create("ClassWiseTT/getrpt", data).
                    then(function (promise) {
                        if (promise.tt != null && promise.tt != "" && promise.periodslst != null && promise.periodslst != "" && promise.gridweeks != null && promise.gridweeks != "") {
                            $scope.table_list_cls_sec_wise = [];
                            for (var a = 0; a < $scope.albumNameArray1.length; a++) {
                                $scope.period_break_list = [];
                                $scope.period_list = promise.periodslst;
                                for (var p = 0; p < $scope.period_list.length; p++) {
                                    var break_flag = false;
                                    for (var c = 0 ; c < promise.tT_Break_list.length; c++) {
                                        if ($scope.albumNameArray1[a].asmcL_Id == promise.tT_Break_list[c].asmcL_Id && parseFloat($scope.period_list[p].ttmP_PeriodName) == promise.tT_Break_list[c].ttmB_AfterPeriod) {
                                            $scope.period_break_list.push({ ped_id: $scope.period_list[p].ttmP_Id, ped_name: $scope.period_list[p].ttmP_PeriodName })
                                            $scope.period_break_list.push({ ped_id: 0, ped_name: 'Break', brk_name: promise.tT_Break_list[c].ttmB_BreakName })
                                            break_flag = true;
                                        }
                                    }
                                    if (break_flag == false) {
                                        $scope.period_break_list.push({ ped_id: $scope.period_list[p].ttmP_Id, ped_name: $scope.period_list[p].ttmP_PeriodName, brk_name: ' ' })
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

                                                if ($scope.tt_list[k].ttmP_Id == $scope.period_break_list[i].ped_id && $scope.tt_list[k].ttmD_Id == $scope.day_list[j].ttmD_Id && $scope.tt_list[k].asmcL_Id == $scope.albumNameArray1[a].asmcL_Id && $scope.tt_list[k].asmS_Id == $scope.albumNameArray2[b].asmS_Id) {
                                                    if (count == 0) {
                                                        newCol = { pedid: $scope.period_break_list[i].ped_id, dayid: $scope.day_list[j].ttmD_Id, value: $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].ismS_SubjectName, pedname: $scope.period_break_list[i].ped_name, dayname: $scope.day_list[j].ttmD_DayName, color: 'black', value1: $scope.tt_list[k].staffName + ' :' + $scope.tt_list[k].ismS_SubjectName, value2: $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].staffName }
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
                                            if (newCol == "") {
                                                if ($scope.period_break_list[i].ped_id == 0) {
                                                    for (var x = 0; x < promise.tT_Break_list_all.length; x++) {
                                                        if ($scope.day_list[j].ttmD_Id == promise.tT_Break_list_all[x].ttmD_Id && promise.tT_Break_list_all[x].asmcL_Id == $scope.albumNameArray1[a].asmcL_Id && $scope.period_break_list[(i - 1)].ped_id == promise.tT_Break_list_all[x].ttmB_AfterPeriod) {
                                                            newCol = { pedid: $scope.period_break_list[i].ped_id, dayid: $scope.day_list[j].ttmD_Id, value1: promise.tT_Break_list_all[x].ttmB_BreakName, pedname: $scope.period_break_list[i].ped_name, dayname: $scope.day_list[j].ttmD_DayName, color: 'black' }
                                                        }
                                                        if ($scope.day_list[j].ttmD_Id == promise.tT_Break_list_all[x].ttmD_Id && promise.tT_Break_list_all[x].asmcL_Id == $scope.albumNameArray1[a].asmcL_Id && $scope.period_break_list[(i - 1)].ped_id != promise.tT_Break_list_all[x].ttmB_AfterPeriod) {
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
                                        $scope.table_list_cls_sec_wise.push({ array: $scope.table_list, ped_list: $scope.period_break_list, header: "Class : " + $scope.albumNameArray1[a].asmcL_ClassName + " & Section : " + $scope.albumNameArray2[b].asmC_SectionName, id: $scope.albumNameArray1[a].asmcL_Id + ':' + $scope.albumNameArray2[b].asmS_Id });
                                 
                                }
                            }
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
            apiService.getDATA("ClassWiseTT/getdetails").
       then(function (promise) {
           $scope.year_list = promise.acayear;
           $scope.categorylst = promise.categorylist;
           $scope.class_list = promise.classlist;
           $scope.temp_classlist = promise.classlist;
           $scope.section_list = promise.sectionlist;

       })
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
                apiService.create("ClassWiseTT/getclass_catg", data).
        then(function (promise) {

            $scope.class_list = promise.classlist;
            $scope.asmcL_Id = "";
            $scope.usercheck = 0;
            $scope.cls_flag = false;
            if (promise.classlist == "" || promise.classlist == null) {
                swal("No classes Are Mapped To Selected Category");
            }
        })
            }
        };


        //TO clear  data
        $scope.clearid = function () {
            
            $scope.asmaY_Id = "";
            $scope.ttmC_Id = "";
            $scope.usercheck = false;
            $scope.usercheck1 = false;
            $scope.all_check();
            $scope.all_check1();
            $scope.class_list = $scope.temp_classlist;
            $scope.grid_view = false;
            $scope.submitted = false;
            $scope.datareport = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
        };


    }

})();