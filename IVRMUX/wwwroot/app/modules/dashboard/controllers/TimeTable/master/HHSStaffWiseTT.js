
(function () {
    'use strict';
    angular
        .module('app')
        .controller('HHSStaffWiseTTController', HHSStaffWiseTTController)

    HHSStaffWiseTTController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter', 'Excel', '$timeout']
    function HHSStaffWiseTTController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, Excel, $timeout) {
        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.staffName).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
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

            return !$scope.staff_list.some(function (options) {
                return options.stf;
            });
        }


        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };

        // TO Save The Data
        $scope.submitted = false;
        $scope.GetReport = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                $scope.albumNameArray1 = [];

                angular.forEach($scope.staff_list, function (role) {
                    if (role.stf) $scope.albumNameArray1.push(role);
                })
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    staffarray: $scope.albumNameArray1,
                }
                apiService.create("StaffWiseTT/getrpt", data).
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
                                for (var j = 0; j < $scope.day_list.length; j++) {
                                    temp_array = [];
                                    for (var i = 0; i < $scope.period_list.length; i++) {
                                        var count = 0;
                                        var newCol = "";
                                        for (var k = 0; k < $scope.tt_list.length; k++) {

                                            if ($scope.tt_list[k].ttmP_Id == $scope.period_list[i].ttmP_Id && $scope.tt_list[k].ttmD_Id == $scope.day_list[j].ttmD_Id && $scope.tt_list[k].hrmE_Id == $scope.albumNameArray1[a].hrmE_Id) {
                                                if (count == 0) {
                                                    newCol = { pedid: $scope.period_list[i].ttmP_Id, dayid: $scope.day_list[j].ttmD_Id, value: $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName, pedname: $scope.period_list[i].ttmP_PeriodName, dayname: $scope.day_list[j].ttmD_DayCode, color: 'black', value1: $scope.tt_list[k].staffName + ' :' + $scope.tt_list[k].ismS_SubjectName, value2: $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].staffName + ' :' + $scope.tt_list[k].ismS_SubjectName, value_: $scope.tt_list[k].ismS_SubjectName }
                                                    count += 1;
                                                }
                                                else if (count > 0) {
                                                    newCol.value = newCol.value + ' && ' + $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName;
                                                    newCol.value1 = newCol.value1 + ' && ' + $scope.tt_list[k].staffName + ' :' + $scope.tt_list[k].ismS_SubjectName;
                                                    newCol.value2 = newCol.value2 + ' && ' + $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].staffName;
                                                    count += 1;
                                                }
                                            }
                                        }
                                        if (newCol == "") {
                                            newCol = { pedid: $scope.period_list[i].ttmP_Id, dayid: $scope.day_list[j].ttmD_Id, value: ' ', pedname: $scope.period_list[i].ttmP_PeriodName, dayname: $scope.day_list[j].ttmD_DayCode, color: 'black' }
                                        }
                                        temp_array.push(newCol);
                                        newCol = "";
                                        count = 0;
                                    }
                                    $scope.table_list.push(temp_array);
                                }
                                //  
                                var _array = [];
                                $scope._array = []; $scope.staffnameswithclas_ = []; var newdata = "";
                                $scope.staffnameswithclas_ = promise.staffnameswithclas;
                                for (var h = 0; h < $scope.staffnameswithclas_.length; h++) {
                                    var count1 = 0;
                                    if ($scope.albumNameArray1[a].hrmE_Id === $scope.staffnameswithclas_[h].hrmE_Id) {
                                        newdata = $scope.staffnameswithclas_[h].names
                                        break;
                                    }
                                }
                                var _array1 = [];
                                $scope._array1 = []; $scope.nameswithclas_ = []; var newdata1 = "";
                                $scope.nameswithclas_ = promise.nameswithclas;
                                for (var h = 0; h < $scope.nameswithclas_.length; h++) {
                                    var count11 = 0; 
                                    if ($scope.albumNameArray1[a].hrmE_Id === $scope.nameswithclas_[h].hrmE_Id) {
                                        if (count11 === 0) {
                                            newdata1 = $scope.nameswithclas_[h].names 
                                            count11++;
                                        }
                                        else {
                                            newdata1 = newdata1 + ',' + names
                                        }
                                    }
                                }
                                //
                                $scope.table_list_sub_wise.push({
                                    array: $scope.table_list, header: $scope.albumNameArray1[a].staffName + '-' + newdata1 + '-' + newdata
                                    , id: $scope.albumNameArray1[a].hrmE_Id
                                });
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

            $scope.rpttyp = "SAWC";
            $scope.all_check();

            apiService.getDATA("StaffWiseTT/getdetails").
                then(function (promise) {
                    $scope.year_list = promise.acayear;
                    $scope.category_list = promise.categorylist;
                    $scope.class_list = promise.classlist;
                    $scope.temp_classlist = promise.classlist;
                    $scope.staff_list = promise.stafflist;

                })
        };

        $scope.togchkbx = function () {
            $scope.usercheck = $scope.staff_list.every(function (options) {
                return options.stf;
            });
        }
        $scope.all_check = function () {
            var toggleStatus = $scope.usercheck;
            angular.forEach($scope.staff_list, function (itm) {
                itm.stf = toggleStatus;
            });
        }

        //TO clear  data
        $scope.clearid = function () {
            $scope.asmaY_Id = "";
            $scope.ttmC_Id = "";
            $scope.hrmE_Id = "";
            $scope.usercheck = false;
            // $scope.stf = false;
            $scope.all_check();
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