
(function () {
    'use strict';
    angular
.module('app')
.controller('SubjectwiseController', SubjectwiseController)

    SubjectwiseController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', 'Excel', '$timeout', '$http', '$q', '$stateParams', '$filter']
    function SubjectwiseController($rootScope, $scope, $state, $location, apiService, Flash, Excel, $timeout, $http, $q, $stateParams, $filter) {

        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.asmcL_ClassName).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
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

        $scope.isOptionsRequired1 = function () {

            return !$scope.subject.some(function (options) {
                return options.subje;
            });
        }



        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };

        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function () {
            
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                $scope.albumNameArray1 = [];
                $scope.albumNameArray2 = [];

                angular.forEach($scope.class, function (role) {
                    if (role.cls) $scope.albumNameArray1.push(role);
                })
                angular.forEach($scope.subject, function (role) {
                    if (role.subje) $scope.albumNameArray2.push(role);
                })
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    classarray: $scope.albumNameArray1,
                    subarray: $scope.albumNameArray2,
                }
                apiService.create("Subjectwise/savedetail", data).
                    then(function (promise) {
                        if (promise.tt != null && promise.tt != "" && promise.periodslst != null && promise.periodslst != "" && promise.gridweeks != null && promise.gridweeks != "") {
                            $scope.table_list_sub_wise = [];
                            $scope.subject_Name = "";
                            for (var a = 0; a < $scope.albumNameArray2.length; a++) {
                                $scope.grid_view = true;
                                $scope.period_list = promise.periodslst;
                                $scope.day_list = promise.gridweeks;
                                $scope.tt_list = promise.tt;
                                var temp_array = [];
                                $scope.table_list = [];

                                $scope.subject_Name = $scope.albumNameArray2[a].ismS_SubjectName;

                                for (var j = 0; j < $scope.day_list.length; j++) {
                                    temp_array = [];
                                    for (var i = 0; i < $scope.period_list.length; i++) {
                                        var count = 0;
                                        var newCol = "";
                                        for (var k = 0; k < $scope.tt_list.length; k++) {

                                            if ($scope.tt_list[k].ttmP_Id == $scope.period_list[i].ttmP_Id && $scope.tt_list[k].ttmD_Id == $scope.day_list[j].ttmD_Id && $scope.tt_list[k].ismS_Id == $scope.albumNameArray2[a].ismS_Id) {

                                                if (count == 0) {
                                                    newCol = { pedid: $scope.period_list[i].ttmP_Id, dayid: $scope.day_list[j].ttmD_Id, value: $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].ismS_SubjectName, pedname: $scope.period_list[i].ttmP_PeriodName, dayname: $scope.day_list[j].ttmD_DayName, color: 'black', value1: $scope.tt_list[k].staffName + ' :' + $scope.tt_list[k].ismS_SubjectName, value2: $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].staffName + ' :' + $scope.tt_list[k].ismS_SubjectName }
                                                    count += 1;
                                                }
                                                else if (count > 0) {
                                                    newCol.value = newCol.value + ' ' + $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].ismS_SubjectName;
                                                    newCol.value1 = newCol.value1 + ' ' + $scope.tt_list[k].staffName + ' :' + $scope.tt_list[k].ismS_SubjectName;
                                                    newCol.value2 = newCol.value2 + ' ' + $scope.tt_list[k].asmcL_ClassName + ' :' + $scope.tt_list[k].asmC_SectionName + ' :' + $scope.tt_list[k].staffName;
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
                                $scope.table_list_sub_wise.push({ array: $scope.table_list, header: $scope.subject_Name });
                            }
                        }
                        else {
                            swal('Data Not Found !');
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
            
            apiService.getDATA("Subjectwise/getdetails").
       then(function (promise) {
           $scope.subject = promise.subdrp;
           $scope.class = promise.clsdrp;
           $scope.Academic = promise.year;

       })
        };


        //TO clear  data
        $scope.clearid = function () {

            $state.reload();
        };

        $scope.all_check1 = function () {
            var toggleStatus = $scope.usercheck1;
            angular.forEach($scope.class, function (itm) {
                itm.cls = toggleStatus;
            });
        };
        $scope.togchkbx1 = function () {
            $scope.usercheck1 = $scope.class.every(function (options) {
                return options.cls;
            });
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