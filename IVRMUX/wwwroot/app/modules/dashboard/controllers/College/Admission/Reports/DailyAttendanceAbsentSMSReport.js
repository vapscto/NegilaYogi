(function () {
    'use strict';
    angular
        .module('app')
        .controller('DailyAttendanceAbsentSMSReportController', DailyAttendanceAbsentSMSReportController)

    DailyAttendanceAbsentSMSReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter']
    function DailyAttendanceAbsentSMSReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter) {


        $scope.catreport_btn = true;
        $scope.catreport = false;
        $scope.printdatatable = [];
        $scope.printdatatable_model = [];
        $scope.currentPage = 1;
        $scope.catreport1_btn = true;
        //$scope.itemsPerPage = 10;
        //$scope.itemsPerPage_model = 10;
        $scope.submitted = false;

        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null) {
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
                copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
            } else {
                paginationformasters = 10;
            }
        } else {
            paginationformasters = 10;
        }


        $scope.itemsPerPage = paginationformasters;
        $scope.itemsPerPage_model = paginationformasters;
        $scope.coptyright = copty;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != null) {
            if (admfigsettings.length > 0) {
                var logopath = admfigsettings[0].asC_Logo_Path;
            }
        }


        $scope.imgname = logopath;

        $scope.propertyName = 'ASMCL_ClassName';
        $scope.order = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        //load
        $scope.StuAttRptDropdownList = function () {
            apiService.get("CollegeDailyAttendance/getdetails/").then(function (promise) {
                $scope.yearDropdown = promise.acdlist;
            });
        }
        $scope.fromdate = new Date();
        //Report
        $scope.submitted = false;
        $scope.showReport = function () {
            $scope.printdatatable = [];
            $scope.searchValue = "";
            $scope.searchValue1 = "";
            if ($scope.myForm.$valid) {

                var fromdate = new Date($scope.fromdate).toDateString();
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "FromDate": fromdate,
                }
                apiService.create("CollegeDailyAttendance/getAttendetails", data)
                    .then(function (promise) {

                        $scope.student_teacherList = promise.studentreport;
                        $scope.presentCountgrid = $scope.student_teacherList.length;


                        if ($scope.student_teacherList != null && $scope.student_teacherList.length > 0) {
                            $scope.catreport1_btn = false;
                            $scope.catreport = true;
                        }

                        else {
                            swal("No Records Found!");
                            $scope.catreport1_btn = true;
                            $scope.catreport1_btn = true;
                            $scope.catreport = false;
                        }
                    });
            } else {
                $scope.submitted = true;
            }

        }


        $scope.toggleAll = function () {
            var toggleStatus = $scope.all2;
            angular.forEach($scope.filterValue11, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all2 == true) {
                    $scope.printdatatable.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });
        }

        $scope.optionToggled = function (SelectedStudentRecord, index) {

            $scope.all2 = $scope.student_teacherList.every(function (itm) { return itm.selected; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }
        }

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {

            return ($filter('date')(obj.AMST_Date, 'dd-MM-yyyy').indexOf($scope.searchValue) >= 0) || ($filter('date')(obj.AMST_DOB, 'dd-MM-yyyy').indexOf($scope.searchValue) >= 0) || (obj.AMST_FirstName).indexOf($scope.searchValue) >= 0 || JSON.stringify(obj.AMST_AdmNo).indexOf($scope.searchValue) >= 0 || JSON.stringify(obj.AMAY_RollNo).indexOf($scope.searchValue) >= 0 || (obj.AMST_DOB_Words).indexOf($scope.searchValue) >= 0 || (obj.AMST_FatherName).indexOf($scope.searchValue) >= 0 || (obj.AMST_MotherName).indexOf($scope.searchValue) >= 0 || JSON.stringify(obj.AMST_FatherMobleNo).indexOf($scope.searchValue) >= 0 || JSON.stringify(obj.AMST_MobileNo).indexOf($scope.searchValue) >= 0 || (obj.AMST_PerAdd3).indexOf($scope.searchValue) >= 0 || (obj.AMST_emailId).indexOf($scope.searchValue) >= 0 || (obj.AMST_BloodGroup).indexOf($scope.searchValue) >= 0 || (obj.AMST_PerAdd3).indexOf($scope.searchValue) >= 0;
        }

        $scope.searchValue1 = '';
        $scope.filterValue11 = function (obj) {

            return (obj.classsection).indexOf($scope.searchValue1) >= 0 || (obj.studentname).indexOf($scope.searchValue1) >= 0;
        }

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
            $scope.searchValue1 = '';
        }

        //-Sending sms-//
        $scope.sendsms = function () {

            if ($scope.printdatatable.length > 0) {

                var fromdate = new Date($scope.fromdate).toDateString();
                var data = {
                    "absentlist": $scope.printdatatable,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "FromDate": fromdate
                }
                apiService.create("CollegeDailyAttendance/absentsendsms", data).then(function (promise) {
                    if (promise != null) {
                        swal(promise.message);
                        $state.reload();
                    }
                })
            } else {
                swal("Select The Student List To Send The SMS");
            }
        }

        //-Sending Email-//
        $scope.sendemail = function () {
            var fromdate = new Date($scope.fromdate).toDateString();
            var data = {
                "absentlist": $scope.printdatatable,
                "ASMAY_Id": $scope.asmaY_Id,
                "fromdate": fromdate
            }
            apiService.create("CollegeDailyAttendance/sendemail", data).then(function (promise) {
                if (promise != null) {
                    swal(promise.message);
                    $state.reload();
                }
            })
        }

        //--Smart Card Attendance Transfer--//
        $scope.smartcardatt = function () {
            var fromdate = new Date($scope.fromdate).toDateString();
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "fromdate": fromdate
            }
            apiService.create("CollegeDailyAttendance/smartcardatt", data).then(function (promise) {
                if (promise != null) {
                    swal(promise.message);
                    $state.reload();
                }
            })
        }

        $scope.createuser = function () {
            var fromdate = new Date($scope.fromdate).toDateString();
            var data = {
                "fromdate": fromdate,
                "ASMAY_Id": $scope.asmaY_Id,

            }
            apiService.create("CollegeDailyAttendance/createuser", data).then(function (promise) {
                if (promise != null) {
                    swal(promise.message);
                }
            })
        }

    }
})();
