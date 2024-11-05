
(function () {
    'use strict';
    angular.module('app').controller('StudentAttendanceDeletedReportController', StudentAttendanceDeletedReportController)

    StudentAttendanceDeletedReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache']
    function StudentAttendanceDeletedReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache) {
        $scope.currentPage = 1;
        $scope.printdatatable = [];
        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.obj = {};
        $scope.usrname = localStorage.getItem('username');

        $scope.sortKey = 'AMAY_RollNo';
        $scope.obj.type = 'Date';
        $scope.sortReverse = false;
        $scope.excel_flag = true;

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        } else {
            paginationformasters = 10;
            copty = "";
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;
        $scope.coptyright = copty;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }       

        $scope.imgname = logopath;

        $scope.LoadData = function () {
            var pageid = 2;
            apiService.getURI("StudentAttendanceReport/OnAttendanceLoadData", pageid).then(function (promise) {
                $scope.yearDropdown = promise.academicList;
                $scope.classDropdown = promise.classlist;
                $scope.ASMAY_Id = promise.asmaY_Id;

                angular.forEach($scope.yearDropdown, function (dd) {
                    if (dd.asmaY_Id === promise.asmaY_Id) {
                        $scope.minDatef = new Date(dd.asmaY_From_Date);
                        $scope.maxDatef = new Date(dd.asmaY_To_Date);
                        $scope.maxDatet = new Date(dd.asmaY_To_Date);
                    }
                });
            });
        };

        $scope.OnChangeFlag = function () {
            //$scope.obj.fromdate = null;
            //$scope.obj.todate = null;
            $scope.excel_flag = true;
            $scope.newarray = [];
        };

        $scope.OnAttendanceChangeYear = function () {
            $scope.classDropdown = [];
            $scope.sectionDropdown = [];
            $scope.subjectDropdown = [];

            $scope.obj.fromdate = null;
            $scope.obj.todate = null;
            $scope.excel_flag = true;

            $scope.ASMCL_Id = "";
            $scope.ASMS_Id = "";
            $scope.obj.ISMS_Id = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };

            apiService.create("StudentAttendanceReport/OnAttendanceChangeYear", data).then(function (promise) {
                $scope.classDropdown = promise.classlist;

                angular.forEach($scope.yearDropdown, function (dd) {
                    if (dd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                        $scope.minDatef = new Date(dd.asmaY_From_Date);
                        $scope.maxDatef = new Date(dd.asmaY_To_Date);
                        $scope.maxDatet = new Date(dd.asmaY_To_Date);
                    }
                });
            });
        };

        $scope.OnAttendanceChangeClass = function () {
            $scope.sectionDropdown = [];
            $scope.subjectDropdown = [];
            $scope.excel_flag = true;
            //$scope.obj.fromdate = null;
            //$scope.obj.todate = null;
            $scope.ASMS_Id = "";
            $scope.obj.ISMS_Id = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id
            };

            apiService.create("StudentAttendanceReport/OnAttendanceChangeClass", data).then(function (promise) {
                $scope.sectionDropdown = promise.sectionList;
            });
        };

        $scope.OnAttendanceChangeSection = function () {
            $scope.excel_flag = true;
            //$scope.obj.fromdate = null;
            //$scope.obj.todate = null;
            $scope.subjectDropdown = [];
            $scope.obj.ISMS_Id = "";

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id
            };

            apiService.create("StudentAttendanceReport/OnAttendanceChangeSection", data).then(function (promise) {
                $scope.subjectDropdown = promise.subjectlist;

                $scope.getstudentlist = promise.getstudentlist;
            });
        };

        $scope.GetAttendanceDeletedReport = function () {
            if ($scope.myForm.$valid) {
                $scope.excel_flag = true;
                $scope.newarray = [];
                var classid = 0;
                var sectionid = 0;
                var sectionids = "0";
                if ($scope.obj.type !== 'All') {
                    classid = $scope.ASMCL_Id;
                    sectionid = $scope.ASMS_Id;
                    if (sectionid === "0") {
                        angular.forEach($scope.sectionDropdown, function (dd) {
                            sectionids = sectionids + "," + dd.asmS_Id;
                        });
                    } else {
                        sectionids = sectionids + "," + $scope.ASMS_Id;
                    }
                }

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": classid,
                    "sectionids": sectionids,
                    "fromdate": new Date($scope.obj.fromdate).toDateString(),
                    "todate": new Date($scope.obj.todate).toDateString(),
                    "reportflag": $scope.obj.type
                };

                apiService.create("StudentAttendanceReport/GetAttendanceDeletedReport", data).then(function (promise) {
                    if (promise.newarray !== null && promise.newarray.length > 0) {
                        $scope.excel_flag = false;

                        $scope.newarray = promise.newarray;

                        angular.forEach($scope.yearDropdown, function (dd) {
                            if (dd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                                $scope.yearname = dd.asmaY_Year;
                            }
                        });

                        angular.forEach($scope.classDropdown, function (dd) {
                            if (dd.asmcL_Id === parseInt($scope.ASMCL_Id)) {
                                $scope.classname = dd.asmcL_ClassName;
                            }
                        });

                        angular.forEach($scope.sectionDropdown, function (dd) {
                            if (dd.asmS_Id === parseInt($scope.ASMS_Id)) {
                                $scope.sectionname = dd.asmC_SectionName;
                            }
                        });

                        console.log($scope.newarray_total);
                    } else {
                        swal("No Records Found");
                    }
                });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.setTodate = function () {
            $scope.obj.todate = null;
            $scope.minDatet = new Date($scope.obj.fromdate);
            $scope.minDatetd = new Date($scope.obj.fromdate);
            if (new Date() <= $scope.maxDatet) {
                $scope.maxDatet = new Date();
            }
            $scope.excel_flag = true;
            $scope.newarray = [];
        };        

        $scope.setTodate1 = function () {            
            $scope.excel_flag = true;
            $scope.newarray = [];
        };

        $scope.exportToExcel = function () {
            var exportHref = "";
            var excelname = "";
            exportHref = Excel.tableToExcel('#excelid', 'Attendance Deleted Report');
            excelname = "Attendance Deleted Report.xlsx";

            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };

        $scope.printData = function () {
            var innerContents = "";
            var popupWinindow = "";

            innerContents = document.getElementById("printsection").innerHTML;

            popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.submitted = false;

        $scope.Clearid = function () {
            $state.reload();
            $scope.asmaY_Id = "";
            $scope.asmcL_Id = "";
            $scope.asmC_Id = "";
            $scope.fromdate = "";
            $scope.todate = "";
            // $scope.amsT_Id = "";
            $scope.amM_Id = "";
        };

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };

    }
})();