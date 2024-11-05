(function () {
    'use strict';

    angular
        .module('app')
        .controller('StudentConsolidatedCertificateReportController', StudentConsolidatedCertificateReportController);

    StudentConsolidatedCertificateReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout'];

    function StudentConsolidatedCertificateReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {
        $scope.search = "";
        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));
        $scope.getcertificateDetlist = [];
        $scope.allc = false;
        $scope.alls = false;
        $scope.excel_flag = true;
        $scope.print_flag = true;
        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;
        $scope.printdatatable = [];
        $scope.BindData = function () {
            $scope.ASMAY_Id = "";
            apiService.getDATA("StudentConsolidatedCertificateReport/GetAcademicYear").
                then(function (promise) {
                    $scope.yearlt = promise.academicYear;
                    $scope.Certificate = promise.masterCertificate;

                    //$scope.newuser1 = promise.getyear;
                    $scope.allAcademicYear1 = promise.getyear1;

                    for (var i = 0; i < $scope.yearlt.length; i++) {
                        name = $scope.yearlt[i].asmaY_Id;
                        for (var j = 0; j < $scope.allAcademicYear1.length; j++) {
                            if (name == $scope.allAcademicYear1[j].asmaY_Id) {
                                $scope.yearlt[i].Selected = true;
                                $scope.ASMAY_Id = $scope.allAcademicYear1[j].asmaY_Id;
                                $scope.RepeatDta2();
                            }
                        }
                    }

                    //if (academicyrlst != null && academicyrlst.length > 0) {
                    //    $scope.ASMAY_Id = academicyrlst[0].asmaY_Id;
                    //    $scope.RepeatDta2();
                    //}

                })
        };

        $scope.RepeatDta2 = function () {
            $scope.chckedIndexs = [];
            $scope.getclasslist = [];

            $scope.getsectionlist = [];
            $scope.allc = false;
            $scope.alls = false;
            $scope.getcertificateDetlist = [];
            $scope.excel_flag = true;
            $scope.print_flag = true;
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            }
            apiService.create("StudentConsolidatedCertificateReport/GetClassDetails", data).then(function (promise) {
                if (promise != null) {
                    $scope.getclasslist = promise.classDetails;

                }
                else {
                    swal("No Class Are Allotted For Academic Year");
                    $scope.getclasslist = "";
                    $scope.excel_flag = true;
                    $scope.print_flag = true;
                }
            });
        };

        $scope.OnClassClickAll = function () {
            $scope.getsectionlist = [];
            $scope.getcertificateDetlist = [];
            $scope.excel_flag = true;
            $scope.print_flag = true;
            angular.forEach($scope.getclasslist, function (dd) {
                dd.ASMC_Ids = $scope.allc;
            });

            if ($scope.allc == true) {
                $scope.OnClassClick();
            }
        };

        $scope.OnSectionClick = function () {
            $scope.getcertificateDetlist = [];
            $scope.excel_flag = true;
            $scope.print_flag = true;
        };

        $scope.OnClassClick = function () {

            $scope.Temp_ASMCLIds = [];
            $scope.getsectionlist = [];
            $scope.getcertificateDetlist = [];
            $scope.excel_flag = true;
            $scope.print_flag = true;
            angular.forEach($scope.getclasslist, function (dd) {
                if (dd.ASMC_Ids) {

                    $scope.Temp_ASMCLIds.push({ ASMC_Id: dd.asmcL_Id });
                }
            });
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "Temp_ASMCLIds": $scope.Temp_ASMCLIds
            }
            apiService.create("StudentConsolidatedCertificateReport/GetSectionDetails", data).then(function (promise) {
                if (promise != null) {
                    $scope.getsectionlist = promise.getsectionlist;

                }
                else {
                    swal("No Section Are Allotted For Academic Year");
                    $scope.getsectionlist = "";
                    $scope.excel_flag = true;
                    $scope.print_flag = true;
                }
            });
        };

        $scope.searchclasschkbx = "";
        $scope.filterclasschkbx = function (obj) {
            return angular.lowercase(obj.asmcL_ClassName).indexOf(angular.lowercase($scope.searchclasschkbx)) >= 0;
        };

        $scope.OnSectionClickAll = function () {

            $scope.getcertificateDetlist = [];

            angular.forEach($scope.getsectionlist, function (dd) {
                dd.ASMS_Ids = $scope.alls;
            });
            $scope.excel_flag = true;
            $scope.print_flag = true;

        };
        //OnSectionClickAll

        $scope.searchsectionchkbx = "";
        $scope.filtersectionchkbx = function (obj) {
            return angular.lowercase(obj.ASMC_SectionName).indexOf(angular.lowercase($scope.searchsectionchkbx)) >= 0;
        };


        $scope.submitted = false;
        $scope.saveddata = function (obj) {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.Temp_ASMCLIds = [];
                $scope.Temp_ASMS_Ids = [];
                $scope.getcertificateDetlist = [];

                angular.forEach($scope.getclasslist, function (dd) {
                    if (dd.ASMC_Ids) {

                        $scope.Temp_ASMCLIds.push({ ASMC_Id: dd.asmcL_Id });
                    }
                });
                angular.forEach($scope.getsectionlist, function (dd) {
                    if (dd.ASMS_Ids) {

                        $scope.Temp_ASMS_Ids.push({ ASMS_Id: dd.ASMS_Id });
                    }
                });
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASC_ReportType": $scope.CERT_Id,
                    "Temp_ASMCLIds": $scope.Temp_ASMCLIds,
                    "Temp_ASMS_Ids": $scope.Temp_ASMS_Ids
                }
                apiService.create("StudentConsolidatedCertificateReport/GetCertificateDetails", data).then(function (promise) {
                    if (promise.getCertificateDet != null && promise.getCertificateDet.length > 0) {

                        $scope.getcertificateDetlist = promise.getCertificateDet;
                        $scope.excel_flag = false;
                        $scope.print_flag = false;
                    }
                    else {
                        swal("No students have applied for a certificate.");
                        $scope.excel_flag = true;
                        $scope.print_flag = true;
                    }
                });
            }
            else {
                $scope.excel_flag = true;
                $scope.print_flag = true;
            }

        };

        $scope.filterValue = function (obj) {
            angular.lowercase(obj.ASC_ReportType).indexOf(angular.lowercase($scope.search)) >= 0 ||
                angular.lowercase(obj.CertificateCount).indexOf(angular.lowercase($scope.search)) >= 0;
        };

        $scope.viewData = function (option) {
            $scope.Studentlist1 = [];
            var data = {
                "ASMAY_Id": option.ASMAY_Id,
                "ASC_ReportType": option.ASC_ReportType,
                "MI_Id": option.MI_Id,
                "ASMCL_Ids": option.ASMCL_Ids,
                "ASMS_Ids": option.ASMS_Ids
            }
            apiService.create("StudentConsolidatedCertificateReport/GetStudentDetails", data).then(function (promise) {
                if (promise != null) {
                    $scope.ReportType = option.ASC_ReportType;
                    $scope.Studentlist = promise.getstudentDet;
                    $('#myModalCoverview').modal('show');
                }
                else {
                    swal("No students have applied for a certificate.");
                    $scope.Studentlist = "";
                }
            });

        };

        $scope.exportToExcel = function () {
            if ($scope.getcertificateDetlist !== null && $scope.getcertificateDetlist.length > 0) {
                var exportHref = Excel.tableToExcel('#table1', 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else {
                swal("Please Select Records to be Exported");
            }
        }
        $scope.printData = function () {
            $scope.date = new Date();
            if ($scope.getcertificateDetlist !== null && $scope.getcertificateDetlist.length > 0) {
                var innerContents = document.getElementById("printSectionId1").innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" href="css/print/preadmission/AdmissionReports.css" rel="stylesheet" />' +
                    '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
                );
                popupWinindow.document.close();
            }
            else {
                swal("Please Select Records to be Printed");
            }


        };

        $scope.exportToExcel1 = function () {
            if ($scope.Studentlist !== null && $scope.Studentlist.length > 0) {
                var exportHref = Excel.tableToExcel('#table', 'sheet name');
                $timeout(function () { location.href = exportHref; }, 100);
            }
            else {
                swal("Please Select Records to be Exported");
            }
        }
        $scope.printData1 = function () {
            $scope.date = new Date();
            if ($scope.Studentlist !== null && $scope.Studentlist.length > 0) {
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
            }
            else {
                swal("Please Select Records to be Printed");
            }


        };

        $scope.cancel = function () {
            $state.reload();
        };
        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        //var x = document.getElementById("demo");

        //$scope.getLocation = function () {
        //    if (navigator.geolocation) {
        //        navigator.geolocation.getCurrentPosition(showPosition);
        //    } else {
        //        $scope.demo = "Geolocation is not supported by this browser.";
        //    }
        //};

        //function showPosition(position) {
        //    $scope.demo = "Latitude: " + position.coords.latitude +
        //        "<br>Longitude: " + position.coords.longitude;
        //}
    };





})();