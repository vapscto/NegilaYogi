(function () {
    'use strict';
    angular
        .module('app')
        .controller('OverallDailyAttendanceReportController', OverallDailyAttendanceReportController)

    OverallDailyAttendanceReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter']
    function OverallDailyAttendanceReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter) {


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

        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.itemsPerPage_model = paginationformasters;
        $scope.coptyright = copty;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.propertyName = 'ASMCL_ClassName';
        $scope.order = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };


        $scope.order_model = function (propertyName_model) {
            $scope.reverse = ($scope.propertyName_model === propertyName_model) ? !$scope.reverse : false;
            $scope.propertyName_model = propertyName_model;
        };
        // $scope.albumNameArraycolumn = [];
        $scope.teacher_stu_grid = false;
        $scope.rowid = function (user) {

            $scope.printdatatable_model = [];
            $scope.searchValue_model = "";

            var fromdate = new Date($scope.fromdate).toDateString();
            var classSection = {
                "ASMCL_className": user.ASMCL_Id,
                "ASMC_SectionName": user.ASMS_ID,
                "ASMAY_Id": $scope.asmaY_Id,
                "fromdate": fromdate
            };

            apiService.create("OverallDailyAttendanceReport/getStudentDetails/", classSection)
                .then(function (promise) {

                    $scope.student_teacherList_view_new = [];
                    for (var i = 0; i < promise.student_teacherList.length; i++) {
                        if (promise.student_teacherList[i].ABSENT == 1) {
                            $scope.student_teacherList_view_new.push(promise.student_teacherList[i]);
                        }
                    }
                    $scope.cls_sec_name = promise.student_teacherList[0].ASMCL_ClassName + '-' + promise.student_teacherList[0].ASMC_SectionName;
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

                    $scope.half_day = user.HALFDAY;
                    angular.forEach($scope.yearDropdown, function (itm) {
                        if (parseInt(itm.asmaY_Id) === parseInt($scope.asmaY_Id)) {
                            $scope.year_name = itm.asmaY_Year;
                        }
                    });
                    $scope.date_name = $scope.fromdate;
                });
        };


        //
        $scope.viewalldetails = function () {

            $scope.printdatatable_model = [];
            $scope.searchValue_model = "";

            var fromdate = new Date($scope.fromdate).toDateString();
            var classSection = {
                "ASMAY_Id": $scope.asmaY_Id,
                "fromdate": fromdate
            };

            apiService.create("OverallDailyAttendanceReport/getStudentAllDetails/", classSection)
                .then(function (promise) {

                    $scope.student_teacherList_view_new = [];
                    for (var i = 0; i < promise.student_teacherList.length; i++) {
                        if (promise.student_teacherList[i].ABSENT == 1) {
                            $scope.student_teacherList_view_new.push(promise.student_teacherList[i]);
                        }
                    }
                    //$scope.cls_sec_name = promise.student_teacherList[0].ASMCL_ClassName + '-' + promise.student_teacherList[0].ASMC_SectionName;
                    $scope.classshow = false;
                    $scope.model_btn = true;

                    $scope.total_strength = $scope.TOTAL;
                    $scope.total_present = $scope.PRESENT;

                    if ($scope.ABSENT === 0) {
                        $scope.teacher_stu_grid = false;
                        $scope.name = "Absentees and Teacher's Names Are Not Available";
                        $scope.export_flag = false;
                    }
                    else {
                        $scope.teacher_stu_grid = true;
                        $scope.export_flag = true;
                        $scope.name = "Absentees and Teacher's Names Details:";
                    }

                    $scope.total_absent = $scope.ABSENT;

                    $scope.half_day = $scope.HALFDAY;
                    angular.forEach($scope.yearDropdown, function (itm) {
                        if (parseInt(itm.asmaY_Id) === parseInt($scope.asmaY_Id)) {
                            $scope.year_name = itm.asmaY_Year;
                        }
                    });
                    $scope.date_name = $scope.fromdate;
                });
        };



        //



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

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {

            return ($filter('date')(obj.AMST_Date, 'dd-MM-yyyy').indexOf($scope.searchValue) >= 0) ||
                ($filter('date')(obj.AMST_DOB, 'dd-MM-yyyy').indexOf($scope.searchValue) >= 0) ||
                (obj.AMST_FirstName).indexOf($scope.searchValue) >= 0 || JSON.stringify(obj.AMST_AdmNo).indexOf($scope.searchValue) >= 0 ||
                JSON.stringify(obj.AMAY_RollNo).indexOf($scope.searchValue) >= 0 ||
                (obj.AMST_DOB_Words).indexOf($scope.searchValue) >= 0 ||
                (obj.AMST_FatherName).indexOf($scope.searchValue) >= 0 ||
                (obj.AMST_MotherName).indexOf($scope.searchValue) >= 0 ||
                JSON.stringify(obj.AMST_FatherMobleNo).indexOf($scope.searchValue) >= 0 ||
                JSON.stringify(obj.AMST_MobileNo).indexOf($scope.searchValue) >= 0 ||
                (obj.AMST_PerAdd3).indexOf($scope.searchValue) >= 0 ||
                (obj.AMST_emailId).indexOf($scope.searchValue) >= 0 || (obj.AMST_BloodGroup).indexOf($scope.searchValue) >= 0 ||
                (obj.AMST_PerAdd3).indexOf($scope.searchValue) >= 0;
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

        //$scope.printData = function () {
        //    
        //    if ($scope.printdatatable !== null && $scope.printdatatable.length > 0) {
        //        var divToPrint = document.getElementById("table");
        //        var newWin = window.open();
        //        newWin.document.write(divToPrint.outerHTML);
        //        newWin.print();
        //        newWin.close();
        //    }
        //    else {
        //        swal("Please select records to be printed");
        //    }
        //}

        $scope.StuAttRptDropdownList = function () {
            apiService.get("OverallDailyAttendanceReport/getdetails/").then(function (promise) {
                $scope.yearDropdown = promise.academicList;
                $scope.allAcademicYear1 = promise.academicListdefault;
                for (var i = 0; i < $scope.yearDropdown.length; i++) {
                    name = $scope.yearDropdown[i].asmaY_Id;
                    for (var j = 0; j < $scope.allAcademicYear1.length; j++) {
                        if (parseInt(name) === parseInt($scope.allAcademicYear1[j].asmaY_Id)) {
                            $scope.yearDropdown[i].Selected = true;
                            $scope.asmaY_Id = $scope.allAcademicYear1[j].asmaY_Id;
                            //$scope.yearId = $scope.allAcademicYear1[j].asmaY_Id;
                        }
                    }
                }
                $scope.categoryDropdown = promise.category_list;
                $scope.categoryflag = promise.categoryflag;
                $scope.fromdate = new Date();
                $scope.fromdate = new Date();
                $scope.maxDatedof = new Date(
                    $scope.fromdate.getFullYear(),
                    $scope.fromdate.getMonth(),
                    $scope.fromdate.getDate());
            });
        };

        $scope.submitted = false;
        $scope.showReport = function () {
            $scope.printdatatable = [];
            $scope.searchValue = "";
            var AMC_Id = 0
            if ($scope.objj.amC_Id != 'All') {
                AMC_Id = $scope.objj.amC_Id
            }
            if ($scope.myForm.$valid) {
                var fromdate = new Date($scope.fromdate).toDateString();
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "fromdate": fromdate,
                    "AMC_Id": AMC_Id
                };
                apiService.create("OverallDailyAttendanceReport/getAttendetails", data)
                    .then(function (promise) {
                        $scope.students = promise.studentAttendanceList;
                        $scope.presentCountgrid = $scope.students.length;
                        if ($scope.students !== null && $scope.students.length > 0) {
                            $scope.catreport_btn = false;
                            $scope.catreport = true;

                            $scope.TOTAL = 0;
                            $scope.PRESENT = 0;
                            $scope.ABSENT = 0;
                            $scope.HALFDAY = 0;



                            angular.forEach($scope.students, function (itm) {
                                $scope.TOTAL = $scope.TOTAL + itm.TOTAL;
                                $scope.PRESENT = $scope.PRESENT + itm.PRESENT;
                                $scope.ABSENT = $scope.ABSENT + itm.ABSENT;
                                $scope.HALFDAY = $scope.HALFDAY + itm.HALFDAY;

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

        $scope.clear = function () {
            $scope.asmaY_Id = "";
            $scope.fromdate = "";
            $scope.submitted = false;
            $scope.catreport = false;
            $scope.catreport_btn = true;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.searchValue = '';
        };
    }
})();
