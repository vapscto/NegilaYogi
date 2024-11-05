(function () {
    'use strict';
    angular.module('app').controller('PC_Indent_Approval_ReportController', PC_Indent_Approval_ReportController)

    PC_Indent_Approval_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$timeout','Excel']
    function PC_Indent_Approval_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $timeout, Excel) {

        $scope.submitted = false;
        $scope.indentapproveddetais = []; 
        $scope.maxdate = new Date();        

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.Loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            $scope.search = "";

            var pageid = 2;
            apiService.getURI("PC_Report/onloaddata", pageid).then(function (promise) {
                $scope.getuserinstitution = promise.getuserinstitution;
                $scope.MI_Id = promise.mI_Id;
            });
        };

        $scope.OnChangeExpenditureInstitution = function () {
            $scope.indentapproveddetais = [];
            $scope.getindenapprovedreportdetails = [];
        };

        $scope.onchangefromdate = function () {
            $scope.PCINDENT_Date_To = null;
            $scope.indentapproveddetais = []; 
            $scope.getindenapprovedreportdetails = [];
        };

        $scope.onchangedate = function () {
            $scope.indentapproveddetais = []; 
            $scope.getindenapprovedreportdetails = [];                     
        };

        $scope.getreport = function (PCINDENT_Idss) {
            if ($scope.myForm.$valid) {
                $scope.indentapproveddetais = [];
                $scope.getindenapprovedreportdetails = [];

                var data = {
                    "MI_Id": $scope.MI_Id,
                    "Fromdate": new Date($scope.PCINDENT_Date_From).toDateString(),
                    "Todate": new Date($scope.PCINDENT_Date_To).toDateString()
                };
                apiService.create("PC_Report/getindentapprovedreport", data).then(function (promise) {
                    if (promise !== null) {
                        $scope.getindenapprovedreportdetails = promise.getindenapprovedreportdetails;
                        if ($scope.getindenapprovedreportdetails !== null && $scope.getindenapprovedreportdetails.length > 0) {

                            $scope.indentapproveddetais = $scope.getindenapprovedreportdetails;
                            $scope.indentapprovedparticulardetais = [];
                            $scope.fromdated = new Date($scope.PCINDENT_Date_From).toDateString();
                            $scope.todated = new Date($scope.PCINDENT_Date_To).toDateString();
                            $scope.institutiondetails = promise.institutiondetails;
                            $scope.imagenew = $scope.institutiondetails[0].mI_Logo;
                            $scope.institutioname = $scope.institutiondetails[0].mI_Name;

                            console.log($scope.indentapproveddetais);
                        }
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.exportToExceldetails = function () {

            var excelname = 'Cash Approved Details Report';
            excelname = excelname.toUpperCase() + '.xls';           
            var exportHref = Excel.tableToExcel('#exceltopd', 'Cash Approved Details Report');
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

        $scope.Clearid = function () {
            $state.reload();
        };
    }
})();
