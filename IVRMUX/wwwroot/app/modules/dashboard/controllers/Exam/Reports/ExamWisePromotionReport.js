(function () {
    'use strict';
    angular.module('app').controller('ExamwisepromotioreportController', ExamwisepromotioreportController)

    ExamwisepromotioreportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter']
    function ExamwisepromotioreportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter) {
        $scope.sortKey = "astA_Id";
        $scope.gridflag = false;
        $scope.print_flag = true;
        $scope.excel_flag = true;
        $scope.currentPage = 1;
        //  $scope.itemsPerPage = 10;
        $scope.printdatatable = [];
        $scope.studentAttendanceList = {};

        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;

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
        $scope.StuAttRptDropdownList = function () {

            apiService.get("MeritList/getdetails/").then(function (promise) {
                $scope.yearDropdown = promise.yearlist;

            });
        };

        $scope.onchangeyear = function () {
            $scope.asmcL_Id = "";
            $scope.asmS_Id = "";
            $scope.EME_Id = "";
            $scope.students = [];
            $scope.gridflag = false;
            $scope.print_flag = true;
            $scope.excel_flag = true;
            var data = {
                "ASMAY_Id": $scope.asmaY_Id
            };

            apiService.create("MeritList/onchangeyear", data).then(function (promise) {
                if (promise !== null) {
                    $scope.classDropdown = promise.classname;
                }

            });
        };

        $scope.onchangeclass = function () {
            $scope.asmS_Id = "";
            $scope.EME_Id = "";
            $scope.students = [];
            $scope.gridflag = false;
            $scope.print_flag = true;
            $scope.excel_flag = true;
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id
            };
            apiService.create("MeritList/onchangeclass", data).then(function (promise) {
                if (promise !== null) {
                    $scope.sectionDropdown = promise.secname;
                }
            });
        };

        $scope.onchangesection = function () {
            $scope.EME_Id = "";
            $scope.students = [];
            $scope.gridflag = false;
            $scope.print_flag = true;
            $scope.excel_flag = true;
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMS_Id": $scope.asmS_Id
            };
            apiService.create("MeritList/onchangesection", data).then(function (promise) {
                if (promise !== null) {
                    $scope.examdropdown = promise.examname;
                }

            });
        };

        $scope.onchangeexam = function () {
            $scope.students = [];
            $scope.gridflag = false;
            $scope.print_flag = true;
            $scope.excel_flag = true;
        };


        $scope.submitted = false;

        $scope.savetmpldata = function () {
            $scope.printdatatable = [];
            $scope.searchValue = "";
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMS_Id": $scope.asmS_Id,
                "EME_Id": $scope.EME_Id
            };

            if ($scope.myForm.$valid) {
                if ($scope.asmaY_Id === undefined || $scope.asmaY_Id === null) {
                    swal("Select The Fields !");
                }
                else {
                    apiService.create("MeritList/getreport", data).then(function (promise) {
                        $scope.date = new Date();
                        if (promise.studentAttendanceList !== null && promise.studentAttendanceList.length > 0) {
                            $scope.students = promise.studentAttendanceList;

                            $scope.getsubjecttopers = promise.getsubjecttopers;
                            $scope.getgradewisetotal = promise.getgradewisetotal;
                        
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

                            angular.forEach($scope.examdropdown, function (y) {
                                if (y.emE_Id === parseInt($scope.EME_Id)) {
                                    $scope.examnamed = y.emE_ExamName;
                                }
                            });

                            $scope.masterinstitution = promise.masterinstitution;
                            $scope.institutename = $scope.masterinstitution[0].mI_Name;
                            $scope.instituteaddress = $scope.masterinstitution[0].mI_Address1;

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
            var excelname = "Merit List Report.xls";
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
