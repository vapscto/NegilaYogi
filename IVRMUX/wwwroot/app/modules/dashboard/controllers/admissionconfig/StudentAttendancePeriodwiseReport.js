
(function () {
    'use strict';
    angular.module('app').controller('StudentAttendancePeriodwiseReportController', StudentAttendancePeriodwiseReportController)

    StudentAttendancePeriodwiseReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache']
    function StudentAttendancePeriodwiseReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache) {
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
            apiService.getURI("StudentAttendanceReport/LoadData", pageid).then(function (promise) {
                $scope.yearDropdown = promise.academicList;
                $scope.classDropdown = promise.classlist;
                $scope.ASMAY_Id = promise.asmaY_Id;
                $scope.monthList = promise.monthList;

                $scope.yearlist = [];

                angular.forEach($scope.yearDropdown, function (dd) {
                    if (dd.asmaY_Id === promise.asmaY_Id) {
                        $scope.minDatef = new Date(dd.asmaY_From_Date);
                        $scope.maxDatef = new Date(dd.asmaY_To_Date);
                        $scope.maxDatet = new Date(dd.asmaY_To_Date);

                        var yearlist = dd.asmaY_Year.split('-');

                        $scope.yearlist.push({ id: yearlist[0], value: yearlist[0] })
                        $scope.yearlist.push({ id: yearlist[1], value: yearlist[1] })
                    }
                });
            });
        };

        $scope.OnChangeFlag = function () {
            $scope.obj.fromdate = null;
            $scope.obj.todate = null;
            $scope.excel_flag = true;
            $scope.newarray = [];
        };

        $scope.OnChangeYear = function () {
            $scope.classDropdown = [];
            $scope.sectionDropdown = [];
            $scope.subjectDropdown = [];

            $scope.obj.fromdate = null;
            $scope.obj.todate = null;
            $scope.excel_flag = true;

            $scope.ASMCL_Id = "";
            $scope.ASMS_Id = "";
            $scope.obj.ISMS_Id = "";
            $scope.obj.YearId = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };

            apiService.create("StudentAttendanceReport/OnChangeAcademicYear", data).then(function (promise) {
                $scope.classDropdown = promise.classlist;

                $scope.yearlist = [];

                angular.forEach($scope.yearDropdown, function (dd) {
                    if (dd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                        $scope.minDatef = new Date(dd.asmaY_From_Date);
                        $scope.maxDatef = new Date(dd.asmaY_To_Date);
                        $scope.maxDatet = new Date(dd.asmaY_To_Date);
                        var yearlist = dd.asmaY_Year.split('-');
                        $scope.yearlist.push({ id: yearlist[0], value: yearlist[0] })
                        $scope.yearlist.push({ id: yearlist[1], value: yearlist[1] })
                    }
                });
            });
        };

        $scope.OnChangeClass = function () {
            $scope.sectionDropdown = [];
            $scope.subjectDropdown = [];
            $scope.excel_flag = true;
            $scope.obj.fromdate = null;
            $scope.obj.todate = null;
            $scope.ASMS_Id = "";
            $scope.obj.ISMS_Id = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id
            };

            apiService.create("StudentAttendanceReport/OnChangeClass", data).then(function (promise) {
                $scope.sectionDropdown = promise.sectionList;
            });
        };

        $scope.OnChangeSection = function () {
            $scope.excel_flag = true;
            $scope.obj.fromdate = null;
            $scope.obj.todate = null;
            $scope.subjectDropdown = [];
            $scope.obj.ISMS_Id = "";

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id
            };

            apiService.create("StudentAttendanceReport/OnChangeSection", data).then(function (promise) {
                $scope.subjectDropdown = promise.subjectlist;

                $scope.getstudentlist = promise.getstudentlist;
            });
        };

        $scope.OnReport = function () {
            if ($scope.myForm.$valid) {
                $scope.excel_flag = true;
                $scope.newarray = [];

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "reportflag": $scope.obj.type
                };

                if ($scope.obj.type === "Date") {
                    data.ISMS_Id = $scope.obj.ISMS_Id;
                    data.fromdate = new Date($scope.obj.fromdate).toDateString();
                    data.todate = new Date($scope.obj.todate).toDateString();
                }

                else if ($scope.obj.type === "Year") {
                    data.ISMS_Id = $scope.obj.ISMS_Id;
                }

                else if ($scope.obj.type === "StudentWise") {
                    data.AMST_Id = $scope.obj.AMST_Id;
                }

                else if ($scope.obj.type === "MonthDateWise") {
                    data.monthid = $scope.obj.MonthId;
                    data.yearid = $scope.obj.YearId;
                }

                apiService.create("StudentAttendanceReport/OnReport", data).then(function (promise) {
                    if (promise.newarray !== null && promise.newarray.length > 0) {
                        $scope.excel_flag = false;
                        if ($scope.obj.type === "Date") {
                            $scope.newarray = promise.newarray;
                            $scope.newarray_total = promise.newarray_total;

                            $scope.columnlist = [];

                            angular.forEach($scope.newarray, function (dd) {
                                var count = 0;
                                if ($scope.columnlist.length === 0) {
                                    $scope.columnlist.push({ columname: dd.AMonthName });
                                } else if ($scope.columnlist.length > 0) {
                                    count = 0;
                                    angular.forEach($scope.columnlist, function (d) {
                                        if (d.columname === dd.AMonthName) {
                                            count += 1;
                                        }
                                    });
                                    if (count === 0) {
                                        $scope.columnlist.push({ columname: dd.AMonthName });
                                    }
                                }
                            });

                            $scope.colspan = $scope.columnlist.length;

                            $scope.studentwiseatt = [];
                            angular.forEach($scope.newarray_total, function (stu) {
                                $scope.studentwiseatt = [];
                                angular.forEach($scope.newarray, function (stu_att) {
                                    if (stu.AMST_Id === stu_att.AMST_Id) {
                                        $scope.studentwiseatt.push(stu_att);
                                    }
                                });
                                stu.attendance = $scope.studentwiseatt;
                            });
                        }

                        else if ($scope.obj.type === "Year") {
                            $scope.newarray = promise.newarray;
                            $scope.getbtwn_monthsname = promise.getbtwn_monthsname;
                        }

                        else if ($scope.obj.type === "StudentWise") {
                            $scope.newarray = promise.newarray;
                            $scope.getsubjectlist = promise.getsubjectlist;
                        }

                        else if ($scope.obj.type === "MonthDateWise") {
                            $scope.daysOfMonth = promise.daysOfMonth;
                            $scope.newarray = promise.newarray;
                            $scope.newarray_date = promise.newarray_date;

                            $scope.daysOfMonth_pre_abs = [];

                            angular.forEach($scope.daysOfMonth, function (dd) {
                                angular.forEach($scope.newarray_date, function (ddd) {
                                    if (dd.columnname === ddd.ASA_FromDate) {
                                        dd.Total_ClassHeld = '(Class Held : ' + ddd.ASA_ClassHeld+')';
                                    }
                                });

                                $scope.daysOfMonth_pre_abs.push({ date: dd.columnname, sub_columnname: 'Present' });
                                $scope.daysOfMonth_pre_abs.push({ date: dd.columnname, sub_columnname: 'Absent' });
                            });

                            $scope.StudentList = [];

                            angular.forEach($scope.newarray, function (dd) {

                                if ($scope.StudentList.length === 0) {
                                    $scope.StudentList.push({ Studentname: dd.StudentName, Admno: dd.amst_admno, class: dd.ClassSection, AMST_Id: dd.AMST_Id });
                                } else if ($scope.StudentList.length > 0) {
                                    var count = 0;

                                    angular.forEach($scope.StudentList, function (d) {
                                        if (d.AMST_Id === dd.AMST_Id) {
                                            count += 1;
                                        }
                                    });
                                    if (count === 0) {
                                        $scope.StudentList.push({ Studentname: dd.StudentName, Admno: dd.amst_admno, class: dd.ClassSection, AMST_Id: dd.AMST_Id });
                                    }
                                }
                            });

                            $scope.stu_att_details = [];

                            angular.forEach($scope.StudentList, function (stu) {
                                $scope.stu_att_details = [];
                                angular.forEach($scope.newarray, function (stu_att) {
                                    if (stu.AMST_Id === stu_att.AMST_Id) {
                                        $scope.stu_att_details.push({
                                            AMST_Id: stu_att.AMST_Id, date: stu_att.ASA_FromDate, PresentCount: stu_att.PresentCount,
                                            AbsentCount: stu_att.AbsentCount, ASA_ClassHeld: stu_att.ASA_ClassHeld
                                        });
                                    }
                                });

                                stu.stu_att_details = $scope.stu_att_details;
                            });
                        }

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

                        angular.forEach($scope.subjectDropdown, function (dd) {
                            if (dd.ismS_Id === parseInt($scope.obj.ISMS_Id)) {
                                $scope.subjectname = dd.ismS_SubjectName;
                            }
                        });

                        angular.forEach($scope.getstudentlist, function (dd) {
                            if (dd.amsT_Id === parseInt($scope.obj.AMST_Id)) {
                                $scope.studentname = dd.amsT_FirstName;
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
            $scope.maxDatet = new Date($scope.minDatetd.getFullYear(),
                $scope.minDatetd.getMonth(),
                $scope.minDatetd.getDate() + 30);

        };

        $scope.exportToExcel = function () {
            var exportHref = "";
            var excelname = "";
            if ($scope.obj.type === 'Date') {
                exportHref = Excel.tableToExcel('#excelid', 'Subject Wise Attendance Report');
                excelname = "Subject Wise Attendance Report.xlsx";
            } else if ($scope.obj.type === 'Year') {
                exportHref = Excel.tableToExcel('#excelidd', 'Subject Wise Attendance Report');
                excelname = "Subject Wise Attendance Report.xlsx";
            } else if ($scope.obj.type === 'StudentWise') {
                exportHref = Excel.tableToExcel('#exceliddd', 'Subject Wise Attendance Report');
                excelname = "Subject Wise Attendance Report.xlsx";
            } else if ($scope.obj.type === 'MonthDateWise') {
                exportHref = Excel.tableToExcel('#exceliddd_month', 'Month Date Wise Attendance Report');
                excelname = "Month Date Wise Attendance Report.xlsx";
            }

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

            if ($scope.obj.type === 'Date') {
                innerContents = document.getElementById("printsection").innerHTML;
            } else if ($scope.obj.type === 'Year') {
                innerContents = document.getElementById("printsectiond").innerHTML;
            } else if ($scope.obj.type === 'StudentWise') {
                innerContents = document.getElementById("printsectiondd").innerHTML;
            } else if ($scope.obj.type === 'MonthDateWise') {
                innerContents = document.getElementById("printsectiondd_month").innerHTML;
            }
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