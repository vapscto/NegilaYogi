(function () {
    'use strict';
    angular
        .module('app')
        .controller('CollegeStudentBirthdayReportController', CollegeStudentBirthdayReportController)

    CollegeStudentBirthdayReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout']
    function CollegeStudentBirthdayReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout) {

        $scope.report = false;
        $scope.catreport = false;
        $scope.statestudentlist = [];

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length !== 0 && ivrmcofigsettings.length !== null && ivrmcofigsettings.length !== undefined) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;

        var logopath = "";
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length !== 0 && admfigsettings.length !== null && admfigsettings.length !== undefined) {
            logopath = admfigsettings[0].asC_Logo_Path;
        } else {
            logopath = "";
        }

        $scope.imgname = logopath;
        $scope.obj = {};


        $scope.BindData = function () {
            apiService.getDATA("StatewiseStudentAdmission/getdetails").
                then(function (promise) {

                    $scope.acdlist = promise.acdlist;
                    $scope.semlist = promise.semlist;
                    $scope.countrylist = promise.countrylist;

                });
        };

        $scope.onselectAcdYear = function (ASMAY_Id) {
            $scope.catreport = false;
            $scope.statestudentlist = [];
            var data = {
                "ASMAY_Id": ASMAY_Id
            };
            apiService.create("StatewiseStudentAdmission/onselectAcdYear", data).
                then(function (promise) {
                    $scope.courselist = promise.courselist;

                });
        };

        $scope.onselectCourse = function (ASMAY_Id, AMCO_Id) {
            $scope.catreport = false;
            $scope.statestudentlist = [];
            var data = {
                "AMCO_Id": AMCO_Id,
                "ASMAY_Id": ASMAY_Id
            };
            apiService.create("StatewiseStudentAdmission/onselectCourse", data).
                then(function (promise) {
                    $scope.branchlist = promise.branchlist;
                });
        };

        $scope.onselectBranch = function (ASMAY_Id, AMCO_Id, AMB_Id) {
            $scope.catreport = false;
            $scope.statestudentlist = [];
            var data = {

                "ASMAY_Id": ASMAY_Id,
                "AMCO_Id": AMCO_Id,
                "AMB_Id": AMB_Id
            };

            apiService.create("StatewiseStudentAdmission/onselectBranch", data).
                then(function (promise) {
                    $scope.semlist = promise.semlist;
                });
        };


        $scope.all_check = function () {

            var toggleStatus = $scope.detail_checked;
            angular.forEach($scope.checklist, function (itm) {
                itm.ivrm_id = toggleStatus;
            });
        };

        $scope.albumNameArraycolumn = [];
        $scope.columnsTest = [];

        $scope.addColumn2 = function (role1) {
            $scope.detail_checked = $scope.checklist.every(function (itm) { return itm.selected; });

            if (role1.selected === true) {

                $scope.albumNameArraycolumn.push(role1);

                var newCol = { id: role1.ivrM_CLG_NAME, checked: true, value: role1.ivrM_CLG_PAR }

                $scope.columnsTest.push(newCol);
            }
            else {
                var som = $scope.albumNameArraycolumn.indexOf(role1);
                $scope.columnsTest.splice($scope.albumNameArraycolumn.indexOf(role1), 1);
                $scope.albumNameArraycolumn.splice($scope.albumNameArraycolumn.indexOf(role1), 1);

            }
        };


        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        $scope.searchValue = '';

        $scope.students = [];
        $scope.catreport = false;
        $scope.submitted = false;

        $scope.ShowReport = function (obj) {
            $scope.all = false;
            $scope.catreport = false;
            $scope.statestudentlist = [];

            if ($scope.myForm.$valid) {

                var fromdate1 = "";
                var todate1 = "";
                var monthid = 0;
                if ($scope.all1 == 1) {
                    monthid = 0;
                    fromdate1 = new Date(obj.fromdate).toDateString();
                    todate1 = new Date(obj.todate).toDateString();
                }
                else {
                    monthid = $scope.obj.month;
                    fromdate1 = new Date().toDateString();
                    todate1 = new Date().toDateString();
                }

                var data = {
                    "AMSE_Id": $scope.AMSE_Id,
                    "months": monthid,                 
                    "flag": $scope.all1,
                    "Fromdate": fromdate1,
                    "Todate": todate1
                };

                apiService.create("StatewiseStudentAdmission/onreportbirthday", data).
                    then(function (promise) {

                        if (promise !== null) {
                            $scope.students = promise.statestudentlist;
                            if ($scope.students.length > 0) {
                                $scope.count = $scope.students.length;
                                $scope.catreport = true;
                                $scope.printdatatabledate = promise.statestudentlist;
                                console.log($scope.statestudentlist);
                            } else {
                                swal("No Records  Found");
                            }

                        } else {
                            swal("No Records  Found");
                        }

                    });
            }
            else {
                $scope.submitted = true;
            }
        };


        $scope.cancel = function () {
            $state.reload();
        };


        $scope.printData = function () {

            var innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/CumulativeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.exportToExcel = function (tableId) {

            var exportHref = Excel.tableToExcel(tableId, 'Student Birthday Report');
            $timeout(function () { location.href = exportHref; }, 100);

        };


        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.isOptionsRequired = function () {
            return !$scope.checklist.some(function (options) {
                return options.ivrm_id;
            });
        };

        $scope.printstudents = [];
        $scope.toggleAll = function () {

            var toggleStatus = $scope.all;
            angular.forEach($scope.students, function (itm) {
                itm.selected = toggleStatus;
                if ($scope.all === true) {
                    if ($scope.printstudents.indexOf(itm) === -1) {
                        $scope.printstudents.push(itm);
                    }
                }
                else {
                    $scope.printstudents.splice(itm);
                }
            });
        };

        $scope.optionToggled = function (SelectedStudentRecord, index) {
            $scope.all = $scope.students.every(function (itm) { return itm.selected; });
            if ($scope.printstudents.indexOf(SelectedStudentRecord) === -1) {
                $scope.printstudents.push(SelectedStudentRecord);
            }
            else {
                $scope.printstudents.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);
            }
        };

    }

})();