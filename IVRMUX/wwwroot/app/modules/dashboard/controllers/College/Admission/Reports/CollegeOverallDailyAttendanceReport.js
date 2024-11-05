(function () {
    'use strict';
    angular
        .module('app')
        .controller('CollegeOverallDailyAttendanceReportController', CollegeOverallDailyAttendanceReportController)

    CollegeOverallDailyAttendanceReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter']
    function CollegeOverallDailyAttendanceReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter) {


        $scope.catreport_btn = true;
        $scope.catreport = false;
        $scope.printdatatable = [];
        $scope.printdatatable_model = [];
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        //$scope.itemsPerPage_model = 10;
        $scope.submitted = false;

        $scope.objj = {};
        $scope.ddate = {};
        $scope.ddate = new Date();
  
        $scope.order_model = function (propertyName_model) {
            $scope.reverse = ($scope.propertyName_model === propertyName_model) ? !$scope.reverse : false;
            $scope.propertyName_model = propertyName_model;
        };
       

        $scope.propertyName = 'AMB_BranchName';
        $scope.order = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.fromdate = new Date();



        $scope.BindData = function () {
            apiService.get("CollegeDailyAttendance/getdetails").then(function (promise) {
                $scope.acdlist = promise.acdlist;
                $scope.allAcademicYear1 = promise.allyear1;
            
                $scope.allAcademicYear1 = promise.academicListdefault;
                for (var i = 0; i < $scope.acdlist.length; i++) {
                    name = $scope.acdlist[i].asmaY_Id;
                    for (var j = 0; j < $scope.allAcademicYear1.length; j++) {
                        if (parseInt(name) === parseInt($scope.allAcademicYear1[j].asmaY_Id)) {
                            $scope.acdlist[i].Selected = true;
                            $scope.asmaY_Id = $scope.allAcademicYear1[j].asmaY_Id;
                            //$scope.yearId = $scope.allAcademicYear1[j].asmaY_Id;
                        }
                    }
                }

                $scope.fromdate = new Date();
                $scope.fromdate = new Date();
                $scope.maxDatedof = new Date(
                    $scope.fromdate.getFullYear(),
                    $scope.fromdate.getMonth(),
                    $scope.fromdate.getDate());
                $scope.minDatedof = new Date(
                    $scope.fromdate.getFullYear(),
                    $scope.fromdate.getMonth(),
                    $scope.fromdate.getDate());
            });
        };


        $scope.submitted = false;
        $scope.showReport = function () {
            $scope.printdatatable = [];
            $scope.searchValue = "";

            if ($scope.myForm.$valid) {
                var fromdate = new Date($scope.fromdate).toDateString();
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "fromdate": fromdate,

                };
                apiService.create("CollegeDailyAttendance/GetAttendancedetails", data)
                    .then(function (promise) {
                        $scope.students = promise.collegestudentAttendanceList;
                        $scope.presentCountgrid = $scope.students.length;
                        if ($scope.students !== null && $scope.students.length > 0) {
                            $scope.TOTAL = 0;
                            $scope.PRESENT = 0;
                            $scope.ABSENT = 0;
                            $scope.catreport = true;
                            angular.forEach($scope.students, function (itm) {
                                $scope.TOTAL = $scope.TOTAL + itm.TOTAL;
                                $scope.PRESENT = $scope.PRESENT + itm.PRESENT;
                                $scope.ABSENT = $scope.ABSENT + itm.ABSENT;
                            });
                        }
                        else {
                            swal("No Records Found!");
                            $scope.catreport_btn = true;
                            $scope.catreport = false;
                        }
                    });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        $scope.toggleAll = function () {
            var toggleStatus = $scope.all2;
            angular.forEach($scope.filterValue1, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all2 === true) {
                    $scope.printdatatable.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });
        };

        $scope.optionToggled = function (SelectedStudentRecord, index) {

            $scope.all2 = $scope.students.every(function (itm) { return itm.selected; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }
        };


        $scope.teacher_stu_grid = false;
        $scope.rowid = function (user) {

            $scope.printdatatable_model = [];
            $scope.searchValue_model = "";

            var fromdate = new Date($scope.fromdate).toDateString();
            var data = {
                "AMB_Id": user.AMB_Id,
                "AMSE_Id": user.AMSE_Id,
                "ASMAY_Id": $scope.asmaY_Id,
                "fromdate": fromdate
            };

            apiService.create("CollegeDailyAttendance/getStudentAbsentDetails/", data)
                .then(function (promise) {
                    $scope.student_teacherList_view_new = promise.studentAbsent_teacherList



                    $scope.student_teacherList_view_new = [];
                    for (var i = 0; i < promise.studentAbsent_teacherList.length; i++) {
                        if (promise.studentAbsent_teacherList[i].ABSENT == 1) {
                            $scope.student_teacherList_view_new.push(promise.studentAbsent_teacherList[i]);
                        }
                    }
                    $scope.Branchsem_Name = promise.studentAbsent_teacherList[0].AMB_BranchName + '-' + promise.studentAbsent_teacherList[0].AMSE_SEMName;
                    $scope.classshow = true;
                    $scope.model_btn = true;

                    $scope.total_strength = user.TOTAL;
                    $scope.total_present = user.PRESENT;

                    if (user.ABSENT === 0) {
                        $scope.teacher_stu_grid = false;
                        $scope.name = "Absentees and Teacher's Names Are Not Available";
                        $scope.export_flag = false;
                    }
                    else {
                        $scope.teacher_stu_grid = true;
                        $scope.export_flag = true;
                        $scope.name = "Absentees and Teacher's Names Details:";
                    }

                    $scope.total_absent = user.ABSENT;

                    angular.forEach($scope.acdlist, function (itm) {
                        if (parseInt(itm.asmaY_Id) === parseInt($scope.asmaY_Id)) {
                            $scope.year_name = itm.asmaY_Year;
                        }
                    });
                    $scope.date_name = $scope.fromdate;
                });
        };

        $scope.toggleAll_model = function () {
            var toggleStatus_model = $scope.all_model;
            angular.forEach($scope.filterValue2, function (itm) {
                itm.selected_model = toggleStatus_model;
                if ($scope.all_model === true) {
                    $scope.printdatatable_model.push(itm);
                    $scope.count_model = $scope.printdatatable_model.length;
                }
                else {
                    $scope.printdatatable_model.splice(itm);
                }
            });
        };

        $scope.optionToggled_model = function (SelectedStudentRecord, index) {

            $scope.all_model = $scope.student_teacherList_view_new.every(function (itm) { return itm.selected_model; });
            if ($scope.printdatatable_model.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable_model.push(SelectedStudentRecord);
                $scope.count_model = $scope.printdatatable_model.length;
            }
            else {
                $scope.printdatatable_model.splice($scope.printdatatable_model.indexOf(SelectedStudentRecord), 1);
            }
        };

        $scope.exportToExcel_mod = function (tableId) {

            if ($scope.printdatatable_model !== null && $scope.printdatatable_model.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else {
                swal("Please Select Records to be Exported");
            }
        }

        $scope.exportToExcel = function (tableId) {

            if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
                var exportHref = Excel.tableToExcel(tableId, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else {
                swal("Please Select Records to be Exported");
            }
        };


    }
})();
