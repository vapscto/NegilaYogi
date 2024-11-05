(function () {
    'use strict';
    angular.module('app').controller('SwimmingAttendanceReportController', SwimmingAttendanceReportController);
    SwimmingAttendanceReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', 'superCache', '$stateParams'];

    function SwimmingAttendanceReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, superCache, $stateParams) {

        $scope.chckedIndexs = [];
        $scope.unchckedIndexs = [];
        $scope.albumNameArraycolumn = [];
        $scope.columnsTest = [];

        $scope.SaveDis = false;
        $scope.IsHiddenup = true;
        $scope.IsHiddendown = false;

        $scope.currentPage = 1;
        //$scope.itemsPerPage = 10;

        // Load Initial Datas

        $scope.gridswimming = false;
        $scope.gridslunch = false;
        $scope.gridlibrary = false;
        $scope.grid_flag = false;
        $scope.excel_flag = false;

        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.userPrivileges = privlgs[i];
            }
        }

        //disable the sundays in calender
        $scope.onlyWeekendsPredicate = function (date) {
            var day = date.getDay();
            return day === 1 || day === 2 || day === 3 || day === 4 || day === 5 || day === 6;
        };

        $scope.att = {};

        $scope.AttendenceCheckDis = true;

        $scope.loaddata = function () {
            var pageid = 1;
            apiService.getURI("SwimmingAttendanceReport/loaddata", pageid).then(function (promise) {

                if (promise !== null) {
                    $scope.yearDropdown = promise.getyear;
                } else {
                    swal("Something Went Wrong Contact Administrator");
                }
            });
        };

        $scope.onchnageyear = function () {
            $scope.grid_flag = false;
            $scope.gridswimming = false;
            $scope.gridslunch = false;
            $scope.gridlibrary = false;
            $scope.excel_flag = false;
            $scope.ASMCL_Id = "";
            $scope.ASMS_Id = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("SwimmingAttendanceReport/onchnageyear", data).then(function (promise) {
                if (promise !== null) {
                    $scope.classDropdown = promise.getclass;

                    angular.forEach($scope.yearDropdown, function (y) {

                        if (y.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                            var dataf, datat;
                            dataf = new Date(y.asmaY_From_Date);
                            datat = new Date(y.asmaY_To_Date);

                            $scope.fDate = dataf;
                            $scope.tDate = datat;
                            $scope.dd = new Date();

                            $scope.minDatedof = new Date(
                                $scope.fDate.getFullYear(),
                                $scope.fDate.getMonth(),
                                $scope.fDate.getDate());

                            $scope.maxDatedof = new Date(
                                $scope.dd.getFullYear(),
                                $scope.dd.getMonth(),
                                $scope.dd.getDate());
                        }
                    });
                } else {
                    swal("Something Went Wrong Contact Administrator");
                }
            });
        };

        $scope.onchangeclass = function () {
            $scope.grid_flag = false;
            $scope.gridswimming = false;
            $scope.gridslunch = false;
            $scope.gridlibrary = false;
            $scope.excel_flag = false;
            $scope.ASMS_Id = "";
            var data = {
                "ASMAY_Id": $scope.ASMaY_Id,
                "ASMCL_Id": $scope.ASMCL_Id
            };
            apiService.create("SwimmingAttendanceReport/onchangeclass", data).then(function (promise) {
                if (promise !== null) {
                    $scope.sectionDropdown = promise.getsection;
                } else {
                    swal("Something Went Wrong Contact Administrator");
                }
            });
        };

        $scope.getreporttype = function () {
            $scope.grid_flag = false;
            $scope.gridswimming = false;
            $scope.gridslunch = false;
            $scope.gridlibrary = false;
            $scope.printdatatable = [];
            $scope.excel_flag = false;
        };

        $scope.getDataByType = function () {
            $scope.grid_flag = false;
            $scope.gridswimming = false;
            $scope.gridslunch = false;
            $scope.gridlibrary = false;
            $scope.printdatatable = [];
            $scope.excel_flag = false;
        };

        $scope.search = function (att) {

            if ($scope.myForm.$valid) {
                $scope.report1 = [];
                $scope.printdatatable = [];
                $scope.grid_flag = false;
                var datetime = "";
                if ($scope.type === "1") {
                    datetime = new Date().toDateString();
                } else {
                    datetime = new Date($scope.fromdaily).toDateString();
                }

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "date": datetime,
                    "reporttype": 'Swimming'
                };

                apiService.create("SwimmingAttendanceReport/search", data).then(function (promise) {

                    if (promise !== null) {
                        $scope.report1 = promise.getreport;
                        if ($scope.report1 === null || $scope.report1.length === 0) {
                            $scope.grid_flag = false;
                            $scope.excel_flag = false;
                            swal("No Records Found");
                        } else {
                            $scope.students = promise.getreport;
                            $scope.printdatatable = $scope.students;
                            $scope.grid_flag = true;
                            $scope.excel_flag = true;
                        }
                    }
                });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.save = function (att) {
            if ($scope.newform.$valid) {
                var data = {
                    "ASMAY_Id": att.asmaY_Id,
                    "ASMCL_Id": att.asmcL_Id,
                    "ASMS_Id": att.asmS_Id,
                    "ASSC_AttendanceDate": new Date(att.ASSC_AttendanceDate).toDateString(),
                    "ASSC_EntryForFlg": 'Swimming',
                    "Tempstudent": $scope.studentlist
                };
                apiService.create("SwimmingAttendance/save", data).then(function (promise) {
                    if (promise.returnval === true) {
                        swal("Record Saved / Updated Successfully");
                    } else {
                        swal("Failed To Save /Update Record");
                    }
                    $state.reload();
                });


            } else {
                $scope.submitted1 = true;
            }

        };

        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };

        $scope.submitted = false;
        $scope.submitted1 = false;

        $scope.sortBy = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //search
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return JSON.stringify(obj.amaY_RollNo).indexOf($scope.searchValue) >= 0 ||
                angular.lowercase(obj.studentname).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (obj.amsT_RegistrationNo).indexOf($scope.searchValue) >= 0 ||
                (obj.amsT_AdmNo).indexOf($scope.searchValue) >= 0;
        };

        //clear data
        $scope.clearData = function () {
            $state.reload();
        };

    }
})();
