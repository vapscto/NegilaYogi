(function () {
    'use strict';
    angular.module('app').controller('HM_Illness_StudentEntry_ReportController', HM_Illness_StudentEntry_ReportController)
    HM_Illness_StudentEntry_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', 'superCache', '$stateParams', '$filter', 'Excel', '$timeout']
    function HM_Illness_StudentEntry_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, superCache, $stateParams, $filter, Excel, $timeout) {

        var paginationformasters = 0;
        var copty = "";
        $scope.obj = {};
        $scope.obj.search = "";
        $scope.obj.smschecked = false;
        $scope.obj.emailchecked = false;
        $scope.obj.whatsappchecked = false;

        $scope.GetReportStudentList = [];
        $scope.GetReportDataList = [];

        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        } else {
            paginationformasters = 10;
        }

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;

        $scope.maxdate = new Date();
        $scope.HMTILL_Date = new Date();
        $scope.editflag = false;

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var data = 2;
            apiService.getURI("HM_Illness_StudentEntry/LoadStudentIllnessReportData", data).then(function (promise) {

                $scope.GetReportAcademicYearList = promise.getReportAcademicYearList;
                $scope.ASMAY_Id = promise.asmaY_Id;                 

                if (promise.getReportStudentList !== null && promise.getReportStudentList.length > 0) {
                    $scope.GetReportStudentList = promise.getReportStudentList;
                } else {
                    swal("Student Details Not Found");
                }
            });
        };

        $scope.OnChangeRadio = function () {
            $scope.GetReportDataList = [];
        };
        

        $scope.onstudentnamechange = function () {
            $scope.GetReportDataList = [];
        };

        $scope.OnChangeYear = function () {
            $scope.AMST_Id = "";         
            $scope.GetReportStudentList = [];
            $scope.GetReportDataList = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id              
            };
            apiService.create("HM_Illness_StudentEntry/ReportOnChangeYearData", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.getReportStudentList !== null && promise.getReportStudentList.length > 0) {
                        $scope.GetReportStudentList = promise.getReportStudentList;
                    } else {
                        swal("Student Details Not Found");
                    }
                }
            });
        };

        $scope.savedata = function () {
            $scope.submitted = false;
            $scope.GetReportDataList = [];
            if ($scope.myForm.$valid) {
                var amst_id = 0;
                if ($scope.ReportType !== 'Yearwise') {
                    amst_id = $scope.AMST_Id.amsT_Id;
                }
                var data = {                    
                    "ASMAY_Id": $scope.ASMAY_Id,                   
                    "AMST_Id": amst_id,
                    "ReportType": $scope.ReportType
                };
                apiService.create("HM_Illness_StudentEntry/ReportStudentIllnessData", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.getReportDataList !== null && promise.getReportDataList.length > 0) {
                            $scope.GetReportDataList = promise.getReportDataList;
                            $scope.GetMasterInstitutionDetails = promise.getMasterInstitutionDetails;
                            $scope.instname = $scope.GetMasterInstitutionDetails[0].mI_Name;
                        } else {
                            swal("No Records Found");
                        }
                    }
                });
            } else {
                $scope.submitted = true;
            }
        };        

        $scope.cleardata = function () {
            $scope.ASMAY_Id = "";
            $scope.AMST_Id = "";           
            $scope.obj.search = "";
            $scope.AMST_Id = "";
            $scope.GetReportStudentList = [];
            $scope.GetReportDataList = [];                  
            $scope.loaddata();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.filterValue = function (obj) {
            return ($filter('date')(obj.hmtilL_Date, 'dd/MM/yyyy').indexOf($scope.obj.search) >= 0) ||                 
                (angular.lowercase(obj.studentName)).indexOf(angular.lowercase($scope.obj.search)) >= 0 ||
                (angular.lowercase(obj.admissionNo)).indexOf(angular.lowercase($scope.obj.search)) >= 0 ||
                (angular.lowercase(obj.yearName)).indexOf(angular.lowercase($scope.obj.search)) >= 0 ||
                (angular.lowercase(obj.className)).indexOf(angular.lowercase($scope.obj.search)) >= 0 ||
                (angular.lowercase(obj.sectionName)).indexOf(angular.lowercase($scope.obj.search)) >= 0 ||
                (angular.lowercase(obj.hmmilL_IllnessName)).indexOf(angular.lowercase($scope.obj.search)) >= 0;
        }; 

        $scope.ExportToExcel = function () {
            var exportHref = Excel.tableToExcel('#tableId', 'sheet name');
            var excelname = "Student Illness Report.xlsx";
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        };

        $scope.Print = function () {
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
    }
})();