(function () {
    'use strict';
    angular.module('app').controller('AdmissionCancelReportController', AdmissionCancelReportController)

    AdmissionCancelReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$stateParams', '$filter', 'Excel','$timeout']
    function AdmissionCancelReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $stateParams, $filter, Excel, $timeout) {

        $scope.edit = false;
        $scope.excel_flag = true;
        $scope.excel_flag = true;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        var logopath = "";
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        if (privlgs !== null && privlgs.length > 0) {
            for (var i = 0; i < privlgs.length; i++) {
                if (privlgs[i].pageId == pageid) {
                    $scope.userPrivileges = privlgs[i];
                    console.log($scope.userPrivileges);
                }
            }
        }
        $scope.obj = {};
        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;

        $scope.OnLoadAdmissionCancelReport = function () {
            var pageid = 2;
            apiService.getURI("StudentAdmission/OnLoadAdmissionCancelReport", pageid).then(function (promise) {
                if (promise !== null) {
                    $scope.allAcademicYear = promise.allAcademicYear;
                    $scope.academicYearOnLoad = promise.academicYearOnLoad;
                   // $scope.ASMAY_Id = promise.asmaY_Id;

                    angular.forEach($scope.allAcademicYear, function (dd) {
                        if (dd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                            $scope.yearname = dd.asmaY_Year;
                        }
                    });

                    if (promise.getwdstudentdetails !== undefined && promise.getwdstudentdetails !== null && promise.getwdstudentdetails.length > 0) {
                        $scope.getwdstudentdetails = promise.getwdstudentdetails;
                        $scope.excel_flag = false;
                    } else {
                        swal("No Records Found");
                    }
                }
            });
        };

        $scope.OnChangeAdmissionCancelReportYear = function () {
            $scope.getstudentdetailslist = [];
            $scope.getstudentdetails = [];
            $scope.AMST_Id = "";
            $scope.edit = false;
            $scope.submitted = false;
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };

            apiService.create("StudentAdmission/OnChangeAdmissionCancelReportYear", data).then(function (promise) {
                if (promise !== null) {

                    if (promise.getwdstudentdetails !== undefined && promise.getwdstudentdetails !== null && promise.getwdstudentdetails.length > 0) {
                        angular.forEach($scope.allAcademicYear, function (dd) {
                            if (dd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                                $scope.yearname = dd.asmaY_Year;
                            }
                        });
                        $scope.getwdstudentdetails = promise.getwdstudentdetails;
                        $scope.excel_flag = false;
                    } else {
                        swal("No Records Found");
                        $scope.getwdstudentdetails = [];
                    }
                }
            });
        };


        $scope.cleardata = function () {
            $scope.getwdstudentdetails = [];
            $scope.allAcademicYear = [];
            $scope.ASMAY_Id = "";
            $scope.excel_flag = true;
            $scope.OnLoadAdmissionCancelReport();          
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted;
        };

        $scope.filterValue1 = function (obj) {
            return ($filter('aacA_ACDate')(obj.aacA_ACDate, 'dd-MM-yyyy').indexOf($scope.search) >= 0) ||
                (angular.lowercase(obj.studentname)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.amsT_AdmNo)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.asmaY_Year)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.asmcL_ClassName)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.asmC_SectionName)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.amsT_AdmNo)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.aacA_ACReason)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (JSON.stringify(obj.aacA_CancellationFee)).indexOf($scope.search) >= 0 ||
                (JSON.stringify(obj.aacA_ToRefundAmount)).indexOf($scope.search) >= 0;
        };


        $scope.printData = function () {
            var innerContents = document.getElementById("printreport").innerHTML;
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
            var exportHref = Excel.tableToExcel(tableId, 'Admission Cancelled Report');
            var excelname = "Admission Cancelled Report.xls";
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };
    }
})();