
(function () {
    'use strict';
    angular.module('app').controller('StudentAttendancePerdiodWiseAbsentReportController', StudentAttendancePerdiodWiseAbsentReportController)

    StudentAttendancePerdiodWiseAbsentReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache']
    function StudentAttendancePerdiodWiseAbsentReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache) {
        $scope.currentPage = 1;
        $scope.printdatatable = [];
        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.obj = {};
        $scope.usrname = localStorage.getItem('username');

        $scope.sortKey = 'AMAY_RollNo';
        $scope.obj.type = 'StudentAbsentPeriodWise';
        $scope.sortReverse = false;
        $scope.excel_flag = true;
        $scope.obj.searchchkbx = "";
        $scope.obj.searchchkbx1 = "";

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
                        $scope.getdate = new Date();
                        if (new Date(dd.asmaY_To_Date) > $scope.getdate) {
                            $scope.maxDatet = new Date();
                            $scope.maxDatef = new Date();
                        } else {
                            $scope.maxDatet = new Date(dd.asmaY_To_Date);
                            $scope.maxDatef = new Date(dd.asmaY_To_Date);
                        }
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
            $scope.obj.searchchkbx = "";
            $scope.obj.searchchkbx1 = "";
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

            $scope.obj.searchchkbx = "";
            $scope.obj.searchchkbx1 = "";
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

                        $scope.getdate = new Date();

                        if (new Date(dd.asmaY_To_Date) > $scope.getdate) {
                            $scope.maxDatet = new Date();
                            $scope.maxDatef = new Date();
                        } else {
                            $scope.maxDatet = new Date(dd.asmaY_To_Date);
                            $scope.maxDatef = new Date(dd.asmaY_To_Date);
                        }
                        
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
            $scope.obj.searchchkbx = "";
            $scope.obj.searchchkbx1 = "";
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
            $scope.obj.searchchkbx = "";
            $scope.obj.searchchkbx1 = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id
            };

            apiService.create("StudentAttendanceReport/OnChangeSection", data).then(function (promise) {
                $scope.subjectDropdown = promise.subjectlist;
                $scope.getstudentlist = promise.getstudentlist;
                $scope.obj.all = true;
                $scope.obj.all1 = true;

                $timeout(function () { $scope.OnClickAll(); }, 500);
                $timeout(function () { $scope.OnClickAll1(); }, 500);
            });
        };

        $scope.OnClickAll = function () {
            angular.forEach($scope.getstudentlist, function (dd) {
                dd.checkedsub = $scope.obj.all;
            });
        };
        $scope.individual = function () {
            $scope.obj.all = $scope.getstudentlist.every(function (itm) { return itm.checkedsub; });
        };
        $scope.isOptionsRequired3 = function () {
            return !$scope.getstudentlist.some(function (options) {
                return options.checkedsub;
            });
        };
        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.amsT_FirstName).indexOf(angular.lowercase($scope.obj.searchchkbx)) >= 0;
        };

        $scope.OnClickAll1 = function () {
            angular.forEach($scope.subjectDropdown, function (dd) {
                dd.checkedsubj = $scope.obj.all1;
            });
        };

        $scope.individual1 = function () {
            $scope.obj.all1 = $scope.subjectDropdown.every(function (itm) { return itm.checkedsubk; });
        };

        $scope.isOptionsRequired4 = function () {
            return !$scope.subjectDropdown.some(function (options) {
                return options.checkedsubj;
            });
        };

        $scope.filterchkbx1 = function (obj) {
            return angular.lowercase(obj.ismS_SubjectName).indexOf(angular.lowercase($scope.obj.searchchkbx1)) >= 0;
        };

        $scope.OnReport = function () {
            if ($scope.myForm.$valid) {
                $scope.excel_flag = true;
                $scope.newarray = [];
                $scope.Get_Selected_Student_List = [];
                $scope.Get_Selected_Subject_List = [];

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_Id": $scope.ASMS_Id,
                    "reportflag": $scope.obj.type,
                    "fromdate": new Date($scope.obj.fromdate).toDateString(),
                    "todate": new Date($scope.obj.todate).toDateString()
                };

                if ($scope.obj.type === "StudentAbsentPeriodWise") {
                    angular.forEach($scope.getstudentlist, function (dd) {
                        if (dd.checkedsub === true) {
                            $scope.Get_Selected_Student_List.push({ AMST_Id: dd.amsT_Id });
                        }
                    });
                    data.Get_Selected_Student_List = $scope.Get_Selected_Student_List;
                } else if ($scope.obj.type === "SubjectWiseAttEntry") {
                    angular.forEach($scope.subjectDropdown, function (dd) {
                        if (dd.checkedsubj === true) {
                            $scope.Get_Selected_Subject_List.push({ ISMS_Id: dd.ismS_Id });
                        }
                    });

                    data.Get_Selected_Subject_List = $scope.Get_Selected_Subject_List;
                }

                apiService.create("StudentAttendanceReport/OnReport", data).then(function (promise) {
                    if (promise.newarray !== null && promise.newarray.length > 0) {
                        $scope.excel_flag = false;
                        $scope.newarray = promise.newarray;

                        if ($scope.obj.type === "StudentAbsentPeriodWise") {
                            $scope.studentlist = [];
                            angular.forEach($scope.newarray, function (stu) {
                                if ($scope.studentlist.length === 0) {
                                    $scope.studentlist.push({
                                        AMST_Id: stu.AMST_Id,
                                        STUDENTNAME: stu.STUDENTNAME,
                                        AMST_RegistrationNo: stu.AMST_RegistrationNo,
                                        AMST_AdmNo: stu.AMST_AdmNo,
                                        AMAY_RollNo: stu.AMAY_RollNo,
                                        ASMCL_ClassName: stu.ASMCL_ClassName,
                                        ASMC_SectionName: stu.ASMC_SectionName,
                                        ASMAY_Year: stu.ASMAY_Year,
                                    });
                                }

                                else if ($scope.studentlist.length > 0) {
                                    var count = 0;

                                    angular.forEach($scope.studentlist, function (dd) {
                                        if (dd.AMST_Id === stu.AMST_Id) {
                                            count += 1;
                                        }
                                    });

                                    if (count === 0) {
                                        $scope.studentlist.push({
                                            AMST_Id: stu.AMST_Id,
                                            STUDENTNAME: stu.STUDENTNAME,
                                            AMST_RegistrationNo: stu.AMST_RegistrationNo,
                                            AMST_AdmNo: stu.AMST_AdmNo,
                                            AMAY_RollNo: stu.AMAY_RollNo,
                                            ASMCL_ClassName: stu.ASMCL_ClassName,
                                            ASMC_SectionName: stu.ASMC_SectionName,
                                        });
                                    }
                                }
                            });

                            angular.forEach($scope.studentlist, function (stu) {
                                $scope.studentwise_attendance = [];
                                angular.forEach($scope.newarray, function (stu_att) {
                                    if (stu.AMST_Id === stu_att.AMST_Id) {
                                        $scope.studentwise_attendance.push({
                                            AMST_Id: stu_att.AMST_Id,
                                            ASA_FROMDATE: stu_att.ASA_FROMDATE,
                                            ASA_Entry_DateTime: stu_att.ASA_Entry_DateTime,
                                            EMPLOYEENAME: stu_att.EMPLOYEENAME,
                                            ISMS_SUBJECTNAME: stu_att.ISMS_SUBJECTNAME,
                                            TTMP_PeriodName: stu_att.TTMP_PeriodName,
                                            TotalCount: stu_att.TotalCount,
                                            PresentCount: stu_att.PresentCount,
                                            AbsentCount: stu_att.AbsentCount
                                        });
                                    }
                                });
                                stu.attendancedetails = $scope.studentwise_attendance;
                            });
                        }

                        else if ($scope.obj.type === "SubjectWiseAttEntry") {
                            $scope.subjectlistdetails = [];
                            $scope.daysOfMonth = promise.daysOfMonth;

                            angular.forEach($scope.newarray, function (stu) {
                                if ($scope.subjectlistdetails.length === 0) {
                                    $scope.subjectlistdetails.push({
                                        ISMS_Id: stu.ISMS_Id,
                                        ISMS_SubjectName: stu.ISMS_SubjectName
                                    });
                                }

                                else if ($scope.subjectlistdetails.length > 0) {
                                    var count = 0;

                                    angular.forEach($scope.subjectlistdetails, function (dd) {
                                        if (dd.ISMS_Id === stu.ISMS_Id) {
                                            count += 1;
                                        }
                                    });

                                    if (count === 0) {
                                        $scope.subjectlistdetails.push({
                                            ISMS_Id: stu.ISMS_Id,
                                            ISMS_SubjectName: stu.ISMS_SubjectName
                                        });
                                    }
                                }
                            });

                            angular.forEach($scope.subjectlistdetails, function (stu) {
                                $scope.subjectwise_attendance = [];
                                angular.forEach($scope.newarray, function (stu_att) {
                                    if (stu.ISMS_Id === stu_att.ISMS_Id) {
                                        $scope.subjectwise_attendance.push({
                                            ISMS_Id: stu_att.ISMS_Id,                                            
                                            ISMS_SUBJECTNAME: stu_att.ISMS_SUBJECTNAME,
                                            ASA_FROMDATE: stu_att.ASA_FROMDATE,
                                            Periods: stu_att.Periods
                                        });
                                    }
                                });
                                stu.attendancedetails = $scope.subjectwise_attendance;
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
            //$scope.maxDatet = new Date($scope.minDatetd.getFullYear(),
            //    $scope.minDatetd.getMonth(),
            //    $scope.minDatetd.getDate() + 30);
        };

        $scope.exportToExcel = function () {
            var exportHref = "";
            var excelname = "";
            if ($scope.obj.type === "StudentAbsentPeriodWise") {
                exportHref = Excel.tableToExcel('#exceldd', 'Student Subject Wise Attednace Report');
                excelname = "Student Subject Wise Attednace Report.xlsx";
            } else if ($scope.obj.type === "SubjectWiseAttEntry") {
                exportHref = Excel.tableToExcel('#exceldd1', 'Subject Staff Date Wise Attednace Entry Report');
                excelname = "Subject Staff Date Wise Attednace Entry Report.xlsx";
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

            if ($scope.obj.type === "StudentAbsentPeriodWise") {
                innerContents = document.getElementById("printidd").innerHTML;
            } else if ($scope.obj.type === "SubjectWiseAttEntry") {
                innerContents = document.getElementById("printidd1").innerHTML;
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
        };

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };
    }
})();