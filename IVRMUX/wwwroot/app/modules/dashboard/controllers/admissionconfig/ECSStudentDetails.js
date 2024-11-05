//By prashant latest file
(function () {
    'use strict';
    angular
        .module('app')
        .controller('ECSStudentDetialsReportController', ECSStudentDetialsReportController)

    ECSStudentDetialsReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', '$stateParams', 'Excel', '$timeout']
    function ECSStudentDetialsReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, $stateParams, Excel, $timeout) {

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
        } else {
            paginationformasters = 10;
            copty = "";
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        var logopath = "";

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length > 0) {
            logopath = admfigsettings[0].asC_Logo_Path;
        } else {
            logopath = "";
        }

        $scope.FromDate = new Date();
        $scope.imgname = logopath;
        $scope.IsHiddenup = true;
        $scope.IsHiddendown = false;
        $scope.pagination = false;
        $scope.currentPage = 1;
        $scope.export_flag = true;
        $scope.Print_flag = false;
        $scope.printdatatable = [];


        $scope.getloaddata = function () {
            var pageid = 2;
            apiService.getURI("ECSReport/getloaddata", pageid).then(function (promise) {
                if (promise !== null) {
                    $scope.yearlist = promise.getyearlist;
                }
            });
        };


        $scope.getclass = function () {
            $scope.getreportdatalist = [];
            $scope.report = false;
            $scope.export_flag = true;
            $scope.ASMS_Id = "";
            $scope.ASMCL_Id = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("ECSReport/getclass", data).then(function (promise) {
                $scope.classlist = promise.getclasslist;

                //angular.forEach($scope.yearlist, function (dd) {

                //    if (dd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                //        $scope.FromDateD = new Date(dd.asmaY_From_Date);
                //        $scope.todate = new Date(dd.asmaY_To_Date);

                //        $scope.minDatef = $scope.FromDateD;
                //        $scope.maxDatef = $scope.todate;

                //    }

                //});
            });
        };

        $scope.getsection = function () {
            $scope.getreportdatalist = [];
            $scope.report = false;
            $scope.export_flag = true;
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id
            };
            apiService.create("ECSReport/getsection", data).then(function (promise) {
                if (promise !== null) {
                    $scope.sectionlist = promise.getsectionlist;
                }
            });
        };

        $scope.showecsdetails = function () {

            if ($scope.myform.$valid) {
                $scope.getreportdatalist = [];
                $scope.searchValue = "";
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_id": $scope.ASMS_Id,
                    "reportdate": new Date($scope.FromDate).toDateString()
                };
                apiService.create("ECSReport/showecsdetails", data).then(function (promise) {
                    if (promise !== null) {
                        $scope.getreportdatalist = promise.getreportdata;
                        if ($scope.getreportdatalist.length !== null && $scope.getreportdatalist.length > 0) {
                            $scope.reportdetails = $scope.getreportdatalist;
                            $scope.report = true;
                            $scope.export_flag = false;

                            angular.forEach($scope.yearlist, function (dd) {

                                if (dd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                                    $scope.year = dd.asmaY_Year;
                                }

                            });

                        } else {
                            swal("No Records Found");
                            $scope.report = false;
                            $scope.searchValue = "";
                            $scope.export_flag = true;
                        }
                    } else{
                        swal("No Records Found");
                        $scope.report = false;
                        $scope.export_flag = true;
                        $scope.searchValue = "";
                    }
                });
            } else {
                $scope.submitted = true;
            }          
        };

        $scope.exportToExcel = function (tableId) {
            var exportHref = Excel.tableToExcel(tableId, 'ECS Report');
            $timeout(function () { location.href = exportHref; }, 100);
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

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.clear = function () {
            $state.reload();
        };
        $scope.searchValue = "";
        $scope.searchByColumn = function (searchValue, searchColumn, searchyear) {
            if (searchValue !== "" && searchValue !== null && searchValue !== undefined) {
                var data = {
                    "EnteredData": searchValue,
                    "SearchColumn": searchColumn,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "ASMS_id": $scope.ASMS_Id
                };

                apiService.create("ECSReport/searchByColumn", data).then(function (promise) {

                    if (promise !== null) {
                        $scope.getreportdatalist = [];
                        $scope.getreportdatalist = promise.getreportdata;
                        if ($scope.getreportdatalist.length !== null && $scope.getreportdatalist.length > 0) {
                            $scope.reportdetails = $scope.getreportdatalist;
                            $scope.report = true;
                            $scope.export_flag = false;

                            angular.forEach($scope.yearlist, function (dd) {

                                if (dd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                                    $scope.year = dd.asmaY_Year;
                                }

                            });

                        } else {
                            swal("No Records Found");
                            $scope.report = false;
                            $scope.export_flag = true;
                            $scope.searchValue = "";
                        }


                    } else {
                        swal("No Records Found");
                        $scope.report = false;
                        $scope.export_flag = true;
                        $scope.searchValue = "";
                    }

                });

            } else {
                swal("Please Enter Value To Be Searched In Search here.....Text Box  And Then Click On Search Icon");
            }            
        };
    }

})();