(function () {
    'use strict';
    angular.module('app').controller('CollegeGradeSlabReportController', CollegeGradeSlabReportController)

    CollegeGradeSlabReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', '$filter']
    function CollegeGradeSlabReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, $filter) {

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

        var paginationformasters = 10;
        var copty = "";
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;

        var logopath = "";
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.LoadData = function () {
            var pageid = 4;
            apiService.getURI("CollegeExamGeneralReport/MasterGradeReportLoadData", pageid).then(function (promise) {
                $scope.MasterGradeList = promise.masterGradeList;
            });
        };

        $scope.getDataByType = function (type) {
            $scope.print_flag = true;
            $scope.excel_flag = true;
            $scope.gridflag = false;
        };

        $scope.onchangegrade = function () {
            $scope.gridflag = false;
            $scope.print_flag = true;
            $scope.excel_flag = true;
        };

        $scope.submitted = false;

        $scope.savetmpldata = function () {
            $scope.printdatatable = [];
            $scope.searchValue = "";
            if ($scope.myForm.$valid) {

                var data = {
                    "reporttype": $scope.type,
                    "EMGR_Id": $scope.type === 2 ? $scope.EMGR_Id : 0
                };                

                apiService.create("CollegeExamGeneralReport/MasterGradeReportDetails", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.gradeListDetails !== null && promise.gradeListDetails.length > 0) {
                            $scope.gridflag = true;
                            $scope.print_flag = false;
                            $scope.excel_flag = false;
                            $scope.GradeListDetails = promise.gradeListDetails;
                            $scope.GradeList = promise.gradeList;
                            $scope.MasterInstitution = promise.masterInstitution;

                            $scope.gradelist_temp = [];
                            angular.forEach($scope.GradeList, function (dd) {
                                $scope.gradedetailslist_temp = [];
                                angular.forEach($scope.GradeListDetails, function (d) {
                                    if (d.emgR_Id === dd.emgR_Id) {
                                        $scope.gradedetailslist_temp.push({
                                            EMGR_Id: d.emgR_Id, EMGD_Id: d.emgD_Id, EMGD_Name: d.emgD_Name,
                                            EMGD_From: d.emgD_From, EMGD_To: d.emgD_To
                                        })
                                    }
                                });
                                $scope.gradelist_temp.push({
                                    EMGR_Id: dd.emgR_Id, EMGR_GradeName: dd.emgR_GradeName,
                                    EMGR_MarksPerFlag: dd.emgR_MarksPerFlag === "P" ? "Percentage" : "Marks",
                                    GradeDetails: $scope.gradedetailslist_temp
                                });
                            });
                        }
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
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
            var excelname = "Master Grade Slab.xls";
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
            return $scope.submitted || field.$dirty;
        };

        $scope.cancel = function () {
            $state.reload();
        };
    }
})();
