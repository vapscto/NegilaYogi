
(function () {
    'use strict';
    angular
        .module('app')
        .controller('TimeTableGenerationController', TimeTableGenerationController)

    TimeTableGenerationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', 'superCache', 'Excel', '$timeout']
    function TimeTableGenerationController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, superCache, Excel, $timeout) {

        $scope.Workload_table = false;
        $scope.Workload_table1 = false;
        $scope.hidecons = true;
        $scope.editEmployee = {};
        $scope.temparray = [];
        $scope.printdatatable = [];
        $scope.printdatatable1 = [];

        $scope.FXPRD = false;
        $scope.CLSFXPRD = false;
        $scope.BFPRD = false;
        $scope.CNSPRD = false;
        $scope.THREESDC = false;
        $scope.THREESDCREP = false;
        $scope.TWOSDC = false;
        $scope.TWOSDCREP = false;
        $scope.ONESDC = false;
        $scope.ONESDCREP = false;
        $scope.NONSDC = false;
        $scope.AVLPRD = false;

        // TO Generate TT
        $scope.submitted = false;
        $scope.generate = function () {
          
                $scope.submitted = true;
                var ttmcids = '';
                $scope.albumNameArray = [];
                if ($scope.ttmC_Id === 'all') {
                    angular.forEach($scope.categorylst, function (item) {
                        $scope.albumNameArray.push(item);
                    })
                }
                else if ($scope.ttmC_Id !== '') {
                    angular.forEach($scope.categorylst, function (item) {
                        if (item.ttmC_Id === Number($scope.ttmC_Id)) {
                            $scope.albumNameArray.push(item);
                        }
                    })
                }
                if ($scope.albumNameArray.length > 0) {
                    if ($scope.myForm.$valid) {
                        debugger;
                            var data = {
                                "TTMC_CategoryName": $scope.TTMC_CategoryName,
                                "ASMAY_Id": $scope.asmaY_Id,
                                "STAFFSDK": $scope.staffSDK,
                                "CLSSDK": $scope.clsSDK,
                                "STAFF_CONSDK": $scope.conSDK,
                                "STAFFSDKP": $scope.staffSDKP,
                                "CLSSDKP": $scope.clsSDKP,
                                "STAFF_CONSDKP": $scope.conSDKP,
                                "ttmc_idslist": $scope.albumNameArray,
                                 "FXPRD": $scope.FXPRD,
                                "CLSFXPRD": $scope.CLSFXPRD,
                                "BFPRD": $scope.BFPRD,
                                "STAFFSDKP": $scope.staffSDKP,
                                "CLSSDKP": $scope.clsSDKP,
                                "CNSPRD": $scope.CNSPRD,
                                "THREESDC": $scope.THREESDC,
                                "THREESDCREP": $scope.THREESDCREP,
                                "THREESDCREP": $scope.TWOSDC,
                                "TWOSDCREP": $scope.ONESDC,
                                "ONESDCREP": $scope.ONESDCREP,
                                "NONSDC": $scope.NONSDC,
                                "ONESDCREP": $scope.AVLPRD,

                                
                            }
                            apiService.create("TimeTableGeneration/generate", data).
                                then(function (promise) {
                                    if (promise.returnval === true) {
                                        $scope.get_count();
                                        if ($scope.totalnotalloted !== 0) {
                                            swal('TT Generated successfully');
                                        }
                                        else {
                                            swal('TT Generated successfully', 'Some of the Possibilities Not possible fully not generated,please check constraints!');
                                        }
                                    }
                                    else {
                                        swal('Data Not Saved !');
                                    }
                                })
                        }
                        else {
                            swal('Please Select Academic Year and Category !');
                        }                   
                }
                else {
                    swal('Please Select Academic Year and Category !');
                }
         
        };

        //TO  GEt The Values iN Grid
        $scope.BindData = function () {
            apiService.getDATA("TimeTableGeneration/getalldetails").
                then(function (promise) {
                    $scope.printdatatable = promise.workloadpdf;
                    $scope.printdatatable1 = promise.workloadpdf1;
                    $scope.tempTTdata_list = promise.time_Table_new;
                    $scope.temparray = promise.time_Table_new;
                    $scope.currentPage = 1;
                    $scope.itemsPerPage = 5;
                    $scope.categorylst = promise.catelist;
                    $scope.academic = promise.academiclist;
                    $scope.totalperiods = 0;
                    $scope.totalalloted = 0;
                    $scope.totalpending = 0;
                    $scope.totalnotalloted = 0;
                    if ($scope.totalalloted !== 0) {
                        $scope.replacementbuttons = false;
                        $scope.udcrflag = true;
                        $scope.rflag = true;
                    }
                    else {
                        $scope.replacementbuttons = true;
                        $scope.udcrflag = true;
                        $scope.rflag = true;
                    }
                    $scope.staffSDK = 1;
                    $scope.clsSDK = 1;
                    $scope.conSDK = 1;
                    $scope.clsteacherCon = 1;
                    $scope.UDCflag = 1;
                    $scope.SDCflag = 1;
                    $scope.clsteacherCon = 1;
                    $scope.clsteacherCon = 1;
                    $scope.staffSDKP = 0;
                    $scope.clsSDKP = 0;
                    $scope.conSDKP = 0;
                    $scope.gridOptions.data =promise.versionlist;
                })
        };

        $scope.exportToExcel = function (asmay, ttc) {
            if (asmay != "" && asmay != undefined) {
                if (ttc != "" && ttc != undefined) {
                    var d1 = Table_Excel;
                    $scope.tableId1 = d1.id;
                    var blob1 = new Blob([document.getElementById($scope.tableId1).outerHTML],
                        {
                           // type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet;charset=utf-8"
                            type: "application/vnd.ms-excel"
                        });
                    saveAs(blob1, "Report of Workload" + ".xls");
                }
                else {
                    swal("Please Select  Category !!");
                }
            }
            else {
                swal("Please Select Academicyear !!");
            }
        }

        $scope.getTotal1 = function (int) {
            var total1 = 0;
            angular.forEach($scope.temparray, function (e) {
                total1 += e.totalallotedcount;
            });
            return total1;
        };

        //TO clear  data
        $scope.clearid = function () {
            $scope.asmaY_Id = "";
            $scope.ttmC_Id = "";
            $scope.clsteacherCon = 0;
            $scope.staffSDK = 1;
            $scope.clsSDK = 1;
            $scope.conSDK = 1;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
            $scope.totalperiods = 0;
            $scope.totalalloted = 0;
            $scope.totalnotalloted = 0;
            $scope.totalpending = 0;

        };
        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };
        $scope.get_category = function () {

            if ($scope.asmaY_Id === "") {
                swal("Please Select the Academic Year !");
            }
            else {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                }
                apiService.create("TimeTableGeneration/get_catg", data).
                    then(function (promise) {

                        $scope.categorylst = promise.catelist;
                        if (promise.catelist === "" || promise.catelist === null) {
                            swal("No Category Are Mapped To Selected Academic Year");
                        }
                    })

            }
        };
        $scope.bifflg = true;
        $scope.allasdcflg = true;
        $scope.get_count = function () {
            $scope.bifflg = true;
            $scope.allasdcflg = true;

            $scope.albumNameArray = [];
            if ($scope.ttmC_Id === 'all') {
                angular.forEach($scope.categorylst, function (item) {
                    $scope.albumNameArray.push(item);
                })
            }
            else if ($scope.ttmC_Id !== '') {
                angular.forEach($scope.categorylst, function (item) {
                    if (item.ttmC_Id === Number($scope.ttmC_Id)) {
                        $scope.albumNameArray.push(item);
                    }
                })
            }

            if ($scope.albumNameArray.length > 0) {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ttmc_idslist": $scope.albumNameArray
                }
                apiService.create("TimeTableGeneration/get_count", data).
                    then(function (promise) {
                        $scope.totalperiods = promise.countArray[0].totalpriodscount;
                        $scope.totalalloted = promise.countArray[0].totalallotedcount;
                        $scope.totalpending = promise.countArray[0].totalnotallotedcount;
                        $scope.totalnotalloted = Number($scope.totalperiods) - Number($scope.totalalloted);
                        $scope.printdatatable = promise.workloadpdf;
                        $scope.printdatatable1 = promise.workloadpdf1;
                        $scope.tempTTdata_list = promise.time_Table_new;
                        $scope.temparray = promise.time_Table_new;

                        if (promise.fixingperiodcnt == 0 && promise.fixingperiodstaffcnt== 0) {
                            $scope.bifflg = false;
                        }

                        if (promise.fixingperiodcnt == 0 && promise.fixingperiodstaffcnt == 0 && promise.bifurcationcnt == 0 && promise.consecutivecnt == 0) {
                            $scope.allasdcflg = false;
                        }
                       



                    })
            }
            else {
                swal('Please Select Academic Year and Category !');
            }
        };

        $scope.viewPending_entry = true;
        $scope.viewtemprecords = function () {

            var data = {
                "STAFFSDKP": $scope.staffSDKP,
                "CLSSDKP": $scope.clsSDKP,
                "STAFF_CONSDKP": $scope.conSDKP

            }
            apiService.create("TimeTableGeneration/Get_temp_data", data).
                then(function (promise) {
                    if (promise.time_Table_new != null) {
                        $scope.tempTTdata_list = promise.time_Table_new;
                        $scope.viewPending_entry = true;
                        $scope.grid_viewPending = true;
                    }
                    else {
                        swal("Time Table Is Not Available For Your Selection !");
                    }

                })
        };
        $scope.popupclre = function () {
            //  $scope.BindData();
            $scope.viewPending_entry = true;
            $scope.grid_viewPending = true;
        };
        $scope.clear1 = function () {
            $scope.BindData();
            $scope.viewPending_entry = true;
            $scope.grid_viewPending = false;
        };
        $scope.clearidpendding = function () {
            $scope.viewPending_entry = true;
            $scope.grid_viewPending = false;
            $scope.staffSDKP = 0;
            $scope.clsSDKP = 0;
            $scope.conSDKP = 0;


        };
        $scope.toggleAll1 = function () {
            var toggleStatus = $scope.all1;
            angular.forEach($scope.tempTTdata_list, function (itm) {
                itm.checkedvalue = toggleStatus;

            });
        };
        $scope.optionToggled1 = function () {
            $scope.all1 = $scope.tempTTdata_list.every(function (itm) { return itm.checkedvalue; })
        };
        $scope.viewrecordspopup = function (employee) {

            $scope.albumNameArray1 = [];

            $scope.albumNameArray1.push(employee);

            $scope.editEmployee = employee.hrmE_Id;
            var data = {
                "StaffID": $scope.editEmployee,
            }
            apiService.create("TimeTableGeneration/getalldetailsviewrecords", data).
                then(function (promise) {

                    if (promise.tt != null && promise.tt != "" && promise.periodslst != null && promise.periodslst != "" && promise.gridweeks != null && promise.gridweeks != "") {
                        $scope.table_list_sub_wise = [];
                        for (var a = 0; a < $scope.albumNameArray1.length; a++) {
                            $scope.grid_view = true;
                            $scope.period_list = promise.periodslst;
                            $scope.day_list = promise.gridweeks;
                            $scope.tt_list = promise.tt;
                            var temp_array = [];
                            $scope.table_list = [];

                            //  $scope.subject_Name = $scope.albumNameArray2[a].ismS_SubjectName;

                            for (var j = 0; j < $scope.day_list.length; j++) {
                                temp_array = [];
                                for (var i = 0; i < $scope.period_list.length; i++) {
                                    var count = 0;
                                    var newCol = "";
                                    for (var k = 0; k < $scope.tt_list.length; k++) {

                                        if ($scope.tt_list[k].ttmP_Id == $scope.period_list[i].ttmP_Id && $scope.tt_list[k].ttmD_Id == $scope.day_list[j].ttmD_Id && $scope.tt_list[k].hrmE_Id == $scope.albumNameArray1[a].hrmE_Id) {

                                            if (count == 0) {
                                                newCol = { pedid: $scope.period_list[i].ttmP_Id, dayid: $scope.day_list[j].ttmD_Id, value: $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].ismS_SubjectName, pedname: $scope.period_list[i].ttmP_PeriodName, dayname: $scope.day_list[j].ttmD_DayName, color: 'black', value1: $scope.tt_list[k].staffName + ' :' + $scope.tt_list[k].ismS_SubjectName, value2: $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].staffName + ' :' + $scope.tt_list[k].ismS_SubjectName, value_: $scope.tt_list[k].ismS_SubjectName, cls_id: $scope.albumNameArray1[a].asmcL_Id, sec_id: $scope.albumNameArray1[a].asmS_Id }
                                                count += 1;
                                            }
                                            else if (count > 0) {
                                                newCol.value = newCol.value + ' && ' + $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].ismS_SubjectName;
                                                newCol.value1 = newCol.value1 + ' && ' + $scope.tt_list[k].staffName + ' :' + $scope.tt_list[k].ismS_SubjectName;
                                                newCol.value2 = newCol.value2 + ' && ' + $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].staffName;
                                                count += 1;
                                            }
                                        }

                                    }
                                    if (newCol == "") {
                                        newCol = { pedid: $scope.period_list[i].ttmP_Id, dayid: $scope.day_list[j].ttmD_Id, value: ' ', pedname: $scope.period_list[i].ttmP_PeriodName, dayname: $scope.day_list[j].ttmD_DayName, color: 'black' }
                                    }
                                    temp_array.push(newCol);
                                    newCol = "";
                                    count = 0;
                                }

                                $scope.table_list.push(temp_array);
                            }
                            $scope.table_list_sub_wise.push({ array: $scope.table_list, header: $scope.albumNameArray1[a].staffName, id: $scope.albumNameArray1[a].hrmE_Id });
                            // $scope.table_headers.push($scope.table_list);
                        }
                        $scope.secondview = $scope.table_list_sub_wise;
                    }
                    else {
                        swal("TimeTable is Not Generated For Selected Details !!!!");
                        $scope.grid_view = false;
                        $scope.table_list_sub_wise = "";
                        $('#Dialogpopup').modal('hide');

                    }


                })

        };
        $scope._Selected_list = [];
        $scope.SaveTempTTdata = function (tempTTdata_list) {
            $scope.albumNameArray = [];
            angular.forEach($scope.tempTTdata_list, function (user) {
                if (!!user.checkedvalue) $scope.albumNameArray.push(user);
            })
            if ($scope.albumNameArray.length > 0) {
                var data = {
                    "TempararyArrayList": $scope.albumNameArray,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("TimeTableGeneration/saveTemptomain", data).
                    then(function (promise) {
                        if (promise.returnval === false) {
                            swal('Record Not Saved');
                        }
                        else {
                            swal('Record Saved Successfully');
                        }

                    })

            }
            else {
                swal('Select  Atleast One Or Close ');
            }

            $('#pendingReplacementDialog').modal('hide');
            $scope.tempTTdata_list = [];

        };
        $scope.Replacementfunction = function (employee) {
            $scope.class = employee.asmcL_Id;
            $scope.section = employee.asmS_Id;

            $scope.albumNameArray1 = [];

            $scope.albumNameArray1.push(employee);

            $scope.editEmployee = employee.hrmE_Id;
            var data = {
                "StaffID": $scope.editEmployee,
                "ASMCL_Id": $scope.class,
                "ASMS_Id": $scope.section,

            }
            apiService.create("TimeTableGeneration/getreplacemntdetailsviewrecords", data).
                then(function (promise) {

                    if (promise.tT1 != null && promise.tT1 != "" && promise.periodslst1 != null && promise.periodslst1 != "" && promise.gridweeks1 != null && promise.gridweeks1 != "") {
                        $scope.table_list_sub_wise = [];
                        // $scope.table_headers = [];

                        for (var a = 0; a < $scope.albumNameArray1.length; a++) {
                            $scope.grid_view = true;
                            $scope.period_list = promise.periodslst1;
                            $scope.day_list = promise.gridweeks1;
                            $scope.tt_list = promise.tT1;
                            var temp_array = [];
                            $scope.table_list = [];

                            //  $scope.subject_Name = $scope.albumNameArray2[a].ismS_SubjectName;

                            for (var j = 0; j < $scope.day_list.length; j++) {
                                temp_array = [];
                                for (var i = 0; i < $scope.period_list.length; i++) {
                                    var count = 0;
                                    var newCol = "";
                                    for (var k = 0; k < $scope.tt_list.length; k++) {

                                        if ($scope.tt_list[k].ttmP_Id == $scope.period_list[i].ttmP_Id && $scope.tt_list[k].ttmD_Id == $scope.day_list[j].ttmD_Id && $scope.tt_list[k].hrmE_Id == $scope.albumNameArray1[a].hrmE_Id) {

                                            if (count == 0) {
                                                newCol = { pedid: $scope.period_list[i].ttmP_Id, dayid: $scope.day_list[j].ttmD_Id, value: $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].ismS_SubjectName, pedname: $scope.period_list[i].ttmP_PeriodName, dayname: $scope.day_list[j].ttmD_DayName, color: 'black', value1: $scope.tt_list[k].staffName + ' :' + $scope.tt_list[k].ismS_SubjectName, value2: $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].staffName + ' :' + $scope.tt_list[k].ismS_SubjectName, value_: $scope.tt_list[k].ismS_SubjectName }
                                                count += 1;
                                            }
                                            else if (count > 0) {
                                                newCol.value = newCol.value + ' && ' + $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].ismS_SubjectName;
                                                newCol.value1 = newCol.value1 + ' && ' + $scope.tt_list[k].staffName + ' :' + $scope.tt_list[k].ismS_SubjectName;
                                                newCol.value2 = newCol.value2 + ' && ' + $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].staffName;
                                                count += 1;
                                            }
                                        }

                                    }
                                    if (newCol == "") {
                                        newCol = { pedid: $scope.period_list[i].ttmP_Id, dayid: $scope.day_list[j].ttmD_Id, value: ' ', pedname: $scope.period_list[i].ttmP_PeriodName, dayname: $scope.day_list[j].ttmD_DayName, color: 'black' }
                                    }
                                    temp_array.push(newCol);
                                    newCol = "";
                                    count = 0;
                                }

                                $scope.table_list.push(temp_array);
                            }
                            $scope.table_list_sub_wise.push({ array: $scope.table_list, header: $scope.albumNameArray1[a].staffName, id: $scope.albumNameArray1[a].hrmE_Id });
                            // $scope.table_headers.push($scope.table_list);
                        }
                        $scope.secondview = $scope.table_list_sub_wise;
                        var temp_table_list = [];
                        temp_table_list = $scope.table_list;
                        $scope.temp_grid = temp_table_list;
                    }
                    else {
                        swal("TimeTable is Not Generated For Selected Details !!!!");
                        $scope.grid_view = false;
                        $scope.table_list_sub_wise = "";
                        $('#ReplacementDialog').modal('hide');

                    }


                })
        };
        $scope.cell_click_2 = function (dayid, periodid, day, period) {

            if ($scope.from != null && $scope.from != "") {
                $scope.to = day + "Day & " + period + " period";
                $scope.day_to_Id = dayid;
                $scope.period_to_Id = periodid;
            }
            else {
                $scope.from = day + "Day & " + period + " period";
                $scope.day_from_Id = dayid;
                $scope.period_from_Id = periodid;
                //  swal("class id :" + $scope.class + " and section id :" + $scope.section);

                for (var i = 0; i < $scope.table_list.length; i++) {
                    for (var j = 0; j < $scope.table_list[i].length; j++) {
                        $scope.temp_color = $scope.table_list[i][j].color;
                        if ($scope.table_list[i][j].pedid == periodid && $scope.table_list[i][j].dayid == dayid) {
                            $scope.table_list[i][j].color = "orange";
                            $scope.table_list[i][j].background = "Red";

                        }
                        else {
                            $scope.table_list[i][j].color = $scope.temp_color;
                        }
                    }
                }


                var data = {
                    "TTMD_Id": dayid,
                    "TTMP_Id": periodid,
                    "ASMCL_Id": $scope.class,
                    "ASMS_Id": $scope.section,
                    "staffSDK": $scope.staffSDKP_Replace,
                    "subSDK": $scope.clsSDKP_Replace,
                    "conSDK": $scope.conSDKP_Replace
                }
                apiService.create("StaffReplacementForClassSection/getpossiblePeriod", data).
                    then(function (promise) {
                        $scope.table_list = [];
                        $scope.table_list123 = [];
                        $scope.table_list = $scope.temp_grid;

                        //for (var i = 0; i < $scope.table_list.length; i++) {
                        //    for (var j = 0; j < $scope.table_list[i].length; j++) {
                        //        $scope.temp_color = $scope.table_list[i][j].color;
                        //        if ($scope.table_list[i][j].pedid == $scope.table_list123[i][j].ttmP_Id && $scope.table_list[i][j].dayid == $scope.table_list123[i][j].ttmD_Id) {
                        //            $scope.table_list[i][j].color = "white";
                        //            $scope.table_list[i][j].background = "Green";
                        //        }
                        //        else {
                        //            $scope.table_list[i][j].color = $scope.temp_color;
                        //        }
                        //    }
                        //}
                        for (var i = 0; i < $scope.table_list.length; i++) {
                            for (var j = 0; j < $scope.table_list[i].length; j++) {
                                $scope.temp_color = $scope.table_list[i][j].color;
                                if ($scope.table_list[i][j].pedid == $scope.temp_grid[i][j].pedid && $scope.table_list[i][j].dayid == $scope.temp_grid[i][j].dayid) {
                                    if ($scope.table_list[i][j].pedid == periodid && $scope.table_list[i][j].dayid == dayid) {
                                        $scope.table_list[i][j].color = "white";
                                        $scope.table_list[i][j].background = "Red";
                                    }
                                    else {
                                        if ($scope.table_list[i][j].value == " ") {
                                            $scope.table_list[i][j].color = "white";
                                            $scope.table_list[i][j].background = "white";
                                        }
                                        else {

                                            $scope.table_list[i][j].color = "white";
                                            $scope.table_list[i][j].background = "Green";
                                        }
                                    }

                                }
                                else {
                                    $scope.table_list[i][j].color = $scope.temp_color;
                                }
                            }
                        }
                    })



            }

        }
        $scope.clearid_Replace = function () {
            $scope.staffSDKP_Replace = 0;
            $scope.clsSDKP_Replace = 0;
            $scope.conSDKP_Replace = 0;
            $scope.grid_view_replace = false;
            $scope.from = "";
            $scope.to = "";

        };
        $scope.replacement_to_save = function () {

            if ($scope.from != null && $scope.from != "" && $scope.to != null && $scope.to != "") {
                var data = {
                    "ASMCL_Id": $scope.class,
                    "ASMS_Id": $scope.section,
                    "TTMD_ID_from": Number($scope.day_from_Id),
                    "TTMP_ID_from": Number($scope.period_from_Id),
                    "TTMD_ID_to": Number($scope.day_to_Id),
                    "TTMP_ID_to": Number($scope.period_to_Id)
                }
                apiService.create("StaffReplacementForClassSection/savedetail", data).
                    then(function (promise) {
                        if (promise.returnval == true) {
                            swal("Replacement saved sucessfully !");
                            $scope.clearid_Replace();
                            $('#ReplacementDialog').modal('hide');
                        }
                        else {
                            swal("Replacement Not Saved");
                            $scope.clearid_Replace();
                            $('#ReplacementDialog').modal('hide');
                        }
                    })
            }
            else {
                swal("Please select Replacement day & Periods !!!");
            }
        };
        $scope.resetflag = true;
        $scope.resetTT = function () {
            debugger;
            $scope.resetflag = true;
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if ($scope.resetflag === true) {
                mgs = "Reset";
                confirmmgs = "Deactivated";

            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " TimeTable??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {

                        $scope.albumNameArray = [];
                        if ($scope.ttmC_Id === 'all') {
                            angular.forEach($scope.categorylst, function (item) {
                                $scope.albumNameArray.push(item);
                            })
                        }
                        if ($scope.ttmC_Id === 'all') {
                            var data = {
                                "ASMAY_Id": $scope.asmaY_Id,
                                //   "ttmc_idslist": $scope.albumNameArray
                            }
                            apiService.create("TimeTableGeneration/resetTT", data).
                                then(function (promise) {
                                    if (promise.returnval === true) {
                                        $scope.get_count();
                                        swal('Time Table Sucessfully Reseted !');
                                    }
                                    else {
                                        swal('Please Contact Administration!');
                                    }
                                })
                            $scope.BindData();
                        }
                        else {
                            swal('Please Select Academic Year and Category !', 'Please Select Category as All Because some of the staffs will take both category means will get allocated possiblities very less !');
                        }

                    }
                    else {
                        swal("TimeTable " + mgs + " Cancelled");
                    }
                });





        };
        $scope.gridOptions = {
            enableFiltering: true,
            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,
            columnDefs: [
                { name: 'SlNo', field: 'name', enableFiltering: false, enableSorting: false, cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'insname', displayName: 'Institution Name' },
                { name: 'asmayname', displayName: 'Academic Year' },
               { name: 'categoryname', displayName: 'Category Name' },              
                {
                    field: 'id', name: '',
                    displayName: 'Actions', enableFiltering: false, enableSorting: false, cellTemplate:
                        '<div class="grid-action-cell">' +
                        '<a ng-if="row.entity.ttfG_ActiveFlag === false" href="javascript:void(0)" style="color:green;" ng-click="grid.appScope.deactive(row.entity);"> <md-tooltip md-direction="down">Activate Now</md-tooltip> <i class="fa fa-toggle-off text-red" aria-hidden="true"></i></a>' +
                        '<span ng-if="row.entity.ttfG_ActiveFlag === true"><a href="javascript:void(0)"  style="color:red;" ng-click="grid.appScope.deactive(row.entity);">  <md-tooltip md-direction="down">Deactivate Now</md-tooltip> <i class="fa fa-toggle-on text-green"  aria-hidden="true"></i></a><span>' +
                        '</div>'
                }
            ]
        };
        $scope.deactive = function (employee, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.ttfG_ActiveFlag === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";

            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";

            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("TimeTableGeneration/deactivate", employee).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal(confirmmgs + " " + "successfully.");
                                }
                                else {
                                    swal(confirmmgs + " " + " successfully");
                                }
                                $scope.BindData();
                            })

                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        }


    }
})();