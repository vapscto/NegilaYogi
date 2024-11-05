(function () {
    'use strict';
    angular.module('app').controller('ChangeOfBranchReport', ChangeOfBranchReport)

    ChangeOfBranchReport.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout']
    function ChangeOfBranchReport($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {

        $scope.print_flag = true;
        $scope.submitted = false;

        var paginationformasters = '';
        var ivrmcofigsettings = [];
        var copty;

        ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = [];
        $scope.coptyright = copty;
        $scope.ddate = new Date();

        admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("ChangeOfBranchReport/loaddata", pageid).then(function (promise) {
                $scope.academiclist = promise.academiclist;
            });
        };      

        $scope.getcourse = function () {
            $scope.alldata = [];
            $scope.amB_Id = '';
            $scope.branchlist = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("ChangeOfBranchReport/getcourse", data).then(function (promise) {
                $scope.courselist = promise.courselist;
            });
        };       

        $scope.getbranch = function () {
            $scope.alldata = [];
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id
            };
            apiService.create("ChangeOfBranchReport/getbranch", data).then(function (promise) {
                $scope.branchlist = promise.branchlist;
            });
        };

        $scope.print_flag = true;

        $scope.Report = function () {
            if ($scope.myForm.$valid) {
                $scope.alldata = [];
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": 0
                };

                apiService.create("ChangeOfBranchReport/Report", data).then(function (promise) {
                    if (promise.reportdata !== null && promise.reportdata.length !== 0) {
                        $scope.print_flag = false;
                        $scope.alldata = promise.reportdata;

                        angular.forEach($scope.academiclist, function (dd) {
                            if (dd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                                $scope.yearname = dd.asmaY_Year;
                            }
                        });

                        angular.forEach($scope.courselist, function (dd) {
                            if (dd.amcO_Id === parseInt($scope.AMCO_Id)) {
                                $scope.coursename = dd.amcO_CourseName;
                            }
                        });
                    }
                    else {
                        swal("Record Not Found");
                        $state.reload();
                    }
                });
            }
        };

        $scope.Clearid = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.printData = function () {
            var innerContents = '';
            innerContents = document.getElementById("printareaId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/preadmission/RegReportPdf.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); },900);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();
        };

        $scope.exportToExcel = function (tableId) {
            var exportHref = Excel.tableToExcel(tableId, 'Change Branch');
            var excelname = "Change Branch Report.xls";
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

