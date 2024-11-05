(function () {
    'use strict';
    angular.module('app').controller('ExamSubjectWiseRemarksReportController', ExamSubjectWiseRemarksReportController)

    ExamSubjectWiseRemarksReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter']
    function ExamSubjectWiseRemarksReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter) {

        $scope.gridflag = false;
        $scope.print_flag = true;
        $scope.excel_flag = true;
        $scope.currentPage = 1;
        $scope.obj = {};
        $scope.obj.all = false;
        $scope.submitted = false;
        $scope.userPrivileges = "";
        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.LoadData = function () {
            var pageid = 2;
            apiService.getURI("ExamWiseRemarksReport/LoadData", pageid).then(function (promise) {
                $scope.yearDropdown = promise.yearlist;
            });
        };

        $scope.All_Individual = function () {
            $scope.students = [];
            $scope.gridflag = false;
            $scope.print_flag = true;
        };

        $scope.OnChangeYear = function () {
            $scope.asmcL_Id = "";
            $scope.asmS_Id = "";
            $scope.EME_Id = "";

            $scope.classDropdown = [];
            $scope.sectionDropdown = [];
            $scope.getexamlist = [];
            $scope.getsubjectlist = [];

            $scope.students = [];
            $scope.gridflag = false;
            $scope.print_flag = true;
            $scope.excel_flag = true;
            $scope.obj.all = false;
            var data = {
                "ASMAY_Id": $scope.asmaY_Id
            };

            apiService.create("ExamWiseRemarksReport/OnChangeYear", data).then(function (promise) {
                if (promise !== null) {
                    $scope.classDropdown = promise.classname;
                }
            });
        };

        $scope.OnChangeClass = function () {
            $scope.asmS_Id = "";
            $scope.EME_Id = "";
            $scope.students = [];
            $scope.getsubjectlist = [];
            $scope.sectionDropdown = [];
            $scope.getexamlist = [];
            $scope.gridflag = false;
            $scope.print_flag = true;
            $scope.excel_flag = true;
            $scope.obj.all = false;
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id
            };
            apiService.create("ExamWiseRemarksReport/OnChangeClass", data).then(function (promise) {
                if (promise !== null) {
                    $scope.sectionDropdown = promise.secname;
                }
            });
        };

        $scope.OnChangeSection = function () {
            $scope.EME_Id = "";
            $scope.students = [];
            $scope.getsubjectlist = [];
            $scope.getexamlist = [];
            $scope.gridflag = false;
            $scope.print_flag = true;
            $scope.excel_flag = true;
            $scope.obj.all = false;
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMS_Id": $scope.asmS_Id,
                "Report_Type": 'Exam'
            };
            apiService.create("ExamWiseRemarksReport/OnChangeSection", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getexamlist = promise.examname;
                }
            });
        };

        $scope.OnChangeExam = function () {
            $scope.students = [];
            $scope.getsubjectlist = [];
            $scope.gridflag = false;
            $scope.print_flag = true;
            $scope.excel_flag = true;
            $scope.obj.all = false;
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMS_Id": $scope.asmS_Id,
                "EME_Id": $scope.EME_Id,
                "Report_Type": 'Exam'
            };
            apiService.create("ExamWiseRemarksReport/OnChangeExam", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getsubjectlist = promise.getsubjectlist;
                    $scope.obj.all = true;

                    $timeout(function () { $scope.OnClickAll(); }, 1000);
                }
            });
        };

        $scope.GetExamSubjectWiseRemarksReport = function () {
            $scope.searchValue = "";
            $scope.tempsubjectlist = [];

            if ($scope.myForm.$valid) {
                angular.forEach($scope.getsubjectlist, function (dd) {
                    if (dd.ISMS_Id) {
                        $scope.tempsubjectlist.push({ ISMS_Id: dd.ismS_Id, ISMS_SubjectName: dd.ismS_SubjectName });
                    }
                });

                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMS_Id": $scope.asmS_Id,
                    "EME_Id": $scope.EME_Id,
                    "Subject_PT_TypeReport": $scope.Admnoallind,
                    "Selected_Subject_List": $scope.tempsubjectlist
                };

                apiService.create("ExamWiseRemarksReport/GetExamSubjectWiseRemarks_PTReport", data).then(function (promise) {
                    $scope.date = new Date();
                    if (promise.savedata !== null && promise.savedata.length > 0) {
                        $scope.savedata = promise.savedata;
                        $scope.students = [];
                        angular.forEach($scope.savedata, function (dd) {
                            if ($scope.students.length === 0) {
                                $scope.students.push({
                                    AMST_Id: dd.amsT_Id, studentname: dd.studentname, AMST_AdmNo: dd.amsT_AdmNo,
                                    AMST_RegistrationNo: dd.amsT_RegistrationNo, AMAY_RollNo: dd.amaY_RollNo
                                });
                            } else if ($scope.students.length > 0) {
                                var count = 0;
                                angular.forEach($scope.students, function (d) {
                                    if (d.AMST_Id === dd.amsT_Id) {
                                        count += 1;
                                    }
                                });
                                if (count === 0) {
                                    $scope.students.push({
                                        AMST_Id: dd.amsT_Id, studentname: dd.studentname, AMST_AdmNo: dd.amsT_AdmNo,
                                        AMST_RegistrationNo: dd.amsT_RegistrationNo, AMAY_RollNo: dd.amaY_RollNo
                                    });
                                }
                            }
                        });

                        $scope.temp_Temarksdetails = [];
                        angular.forEach($scope.students, function (dd) {
                            $scope.temp_Temarksdetails = [];
                            angular.forEach($scope.savedata, function (d) {
                                if (d.amsT_Id === dd.AMST_Id) {
                                    var color = "black";
                                    if (d.empatY_Color !== undefined && d.empatY_Color !== null && d.empatY_Color !== "") {
                                        color = d.empatY_Color;
                                    }
                                    $scope.temp_Temarksdetails.push({
                                        EME_Id: d.emE_Id, EMER_Remarks: d.emeR_Remarks, AMST_Id: d.amsT_Id,
                                        ISMS_Id: d.ismS_Id, fontcolor: color
                                    });
                                }
                            });

                            dd.Remarks_Details = $scope.temp_Temarksdetails;
                        });

                        console.log($scope.students);
                        $scope.gridflag = true;
                        $scope.print_flag = false;
                        $scope.excel_flag = false;

                        angular.forEach($scope.yearDropdown, function (y) {
                            if (y.asmaY_Id === parseInt($scope.asmaY_Id)) {
                                $scope.yearnamed = y.asmaY_Year;
                            }
                        });

                        angular.forEach($scope.classDropdown, function (y) {
                            if (y.asmcL_Id === parseInt($scope.asmcL_Id)) {
                                $scope.classnamed = y.asmcL_ClassName;
                            }
                        });

                        angular.forEach($scope.sectionDropdown, function (y) {
                            if (y.asmS_Id === parseInt($scope.asmS_Id)) {
                                $scope.sectionamed = y.asmC_SectionName;
                            }
                        });

                        angular.forEach($scope.getexamlist, function (y) {
                            if (y.emE_Id === parseInt($scope.EME_Id)) {
                                $scope.examnamed = y.emE_ExamName;
                            }
                        });

                        if ($scope.Admnoallind === "StudetSubjectWiseRemarks") {
                            $scope.reportname = "Student Exam Subject Wise Remarks"
                        } else if ($scope.Admnoallind === "StudetSubjectWisePaperType") {
                            $scope.reportname = "Student Exam Subject Wise Paper Type"
                        }

                        $scope.colspan = 3 + $scope.tempsubjectlist.length;
                        $scope.institutename = promise.instituelist[0].mI_Name;
                    }
                    else {
                        swal("No record Found !");
                        $scope.students = [];
                        $scope.gridflag = false;
                        $scope.print_flag = true;
                        $scope.excel_flag = true;
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.isOptionsRequired1 = function () {
            return !$scope.getsubjectlist.some(function (options) {
                return options.ISMS_Id;
            });
        };

        $scope.OnClickAll = function () {
            angular.forEach($scope.getsubjectlist, function (dd) {
                dd.ISMS_Id = $scope.obj.all;
            });
        };

        $scope.individual = function () {
            $scope.obj.all = $scope.getsubjectlist.every(function (itm) { return itm.ISMS_Id; });
        };

        $scope.printData = function () {
            var innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();
        };

        $scope.exportToExcel = function (tableId) {
            var exportHref = Excel.tableToExcel(tableId, $scope.reportname);
            var excelname = $scope.reportname + ".xls";
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $state.reload();
        };
    }
})();
