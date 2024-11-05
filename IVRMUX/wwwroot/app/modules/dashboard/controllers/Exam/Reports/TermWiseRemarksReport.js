(function () {
    'use strict';
    angular.module('app').controller('TermWiseRemarksReportController', TermWiseRemarksReportController)

    TermWiseRemarksReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter']
    function TermWiseRemarksReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter) {

        $scope.gridflag = false;
        $scope.print_flag = true;
        $scope.excel_flag = true;
        $scope.currentPage = 1;

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
            apiService.getURI("ExamWiseRemarksReport/LoadData",pageid).then(function (promise) {
                $scope.yearDropdown = promise.yearlist;
            });
        };

        $scope.getlist = function () {            
            $scope.students = [];
            $scope.gridflag = false;
            $scope.print_flag = true;
            $scope.excel_flag = true;
        };

        $scope.OnChangeYear = function () {
            $scope.asmcL_Id = "";
            $scope.asmS_Id = "";
            $scope.EME_Id = "";

            $scope.classDropdown =[];
            $scope.sectionDropdown =[];
            $scope.gettermlist = [];

            $scope.students = [];
            $scope.gridflag = false;
            $scope.print_flag = true;
            $scope.excel_flag = true;
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
            $scope.sectionDropdown = [];
            $scope.gettermlist = [];
            $scope.gridflag = false;
            $scope.print_flag = true;
            $scope.excel_flag = true;
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
            $scope.gettermlist = [];
            $scope.gridflag = false;
            $scope.print_flag = true;
            $scope.excel_flag = true;
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMS_Id": $scope.asmS_Id,
                "Report_Type": 'Term'
            };
            apiService.create("ExamWiseRemarksReport/OnChangeSection", data).then(function (promise) {
                if (promise !== null) {
                    $scope.gettermlist = promise.termlist;
                }
            });
        };

        $scope.onchangeexam = function () {
            $scope.students = [];
            $scope.gridflag = false;
            $scope.print_flag = true;
            $scope.excel_flag = true;
        };

        $scope.isOptionsRequired1 = function () {
            return !$scope.gettermlist.some(function (options) {
                return options.ECT_Id;
            });
        };

        $scope.submitted = false;

        $scope.GetTermWiseRemarksReport = function () {           
            $scope.searchValue = "";
            $scope.tempexamlist = [];

            if ($scope.myForm.$valid) {

                if ($scope.examtype === 'IE') {
                    angular.forEach($scope.gettermlist, function (dd) {
                        if (dd.ECT_Id) {
                            $scope.tempexamlist.push({ ECT_Id: dd.ecT_Id, ECT_TerrmName: dd.ecT_TermName });
                        }
                    });
                }               

                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMS_Id": $scope.asmS_Id,
                    "Term_Type": $scope.examtype
                };

                if ($scope.examtype === 'IE') {
                    data.Selected_Term_List = $scope.tempexamlist;
                }

                apiService.create("ExamWiseRemarksReport/GetTermWiseRemarksReport", data).then(function (promise) {
                    $scope.date = new Date();
                    if (promise.savedata !== null && promise.savedata.length > 0) {
                        $scope.savedata = promise.savedata;

                        $scope.students = [];
                        angular.forEach($scope.savedata, function (dd) {
                            if ($scope.students.length === 0) {
                                $scope.students.push({
                                    AMST_Id: dd.amsT_Id, studentname: dd.studentname, AMST_AdmNo: dd.amsT_AdmNo,
                                    AMST_RegistrationNo: dd.amsT_RegistrationNo, AMAY_RollNo: dd.amaY_RollNo, ECTERE_Remarks: dd.ecterE_Remarks
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
                                        AMST_RegistrationNo: dd.amsT_RegistrationNo, AMAY_RollNo: dd.amaY_RollNo, ECTERE_Remarks: dd.ecterE_Remarks
                                    });
                                }
                            }
                        });

                        if ($scope.examtype === "IE") {
                            $scope.temp_Temarksdetails = [];
                            angular.forEach($scope.students, function (dd) {
                                $scope.temp_Temarksdetails = [];
                                angular.forEach($scope.savedata, function (d) {
                                    if (d.amsT_Id === dd.AMST_Id) {
                                        $scope.temp_Temarksdetails.push({ ECT_Id: d.ecT_Id, ECTERE_Remarks: d.ecterE_Remarks, AMST_Id: d.amsT_Id });
                                    }
                                });

                                dd.Remarks_Details = $scope.temp_Temarksdetails;
                            });

                            $scope.colspan = 3 + $scope.tempexamlist.length;
                        }                        

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



        $scope.toggleAll = function () {
            var toggleStatus = $scope.all2;
            $scope.printdatatable = [];
            angular.forEach($scope.students, function (itm) {
                itm.selected = toggleStatus;
                if (itm.selected === true) {
                    $scope.printdatatable.push(itm);
                }
            });
        };

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.amaY_RollNo)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.amsT_FirstName)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.estmP_TotalMaxMarks)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.estmP_TotalObtMarks)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.estmP_Percentage)).indexOf($scope.searchValue) >= 0 ||
                (angular.lowercase(obj.estmP_SectionRank)).indexOf($scope.searchValue) >= 0 ||
                (JSON.stringify(obj.classes)).indexOf($scope.searchValue) >= 0
        };

        $scope.optionToggled = function (SelectedStudentRecord, index) {
            $scope.all2 = $scope.students.every(function (itm) { return itm.selected; });
            $scope.printdatatable = [];
            angular.forEach($scope.students, function (itm) {
                if (itm.selected === true) {
                    $scope.printdatatable.push(itm);
                }
            });
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
            var exportHref = Excel.tableToExcel(tableId, 'sheet name');
            var excelname = "Term Wise Remarks Report.xls";
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
