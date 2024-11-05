(function () {
    'use strict';
    angular.module('app').controller('PromotionReportDetailsController', PromotionReportDetailsController)

    PromotionReportDetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout']
    function PromotionReportDetailsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        var paginationformasters = '';
        var ivrmcofigsettings = [];

        var copty;
        ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !==null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = [];

        $scope.coptyright = copty;
        $scope.ddate = new Date();
        var logopath = "";
        admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !==null && admfigsettings.length > 0) {
            logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        $scope.onpageload = function () {
            var pageid = 1;
            apiService.getURI("PromotionReportDetails/getdata", pageid).then(function (promise) {
                $scope.yearlist = promise.allAcademicYear;
            });
        };

        $scope.print_flag = true;

        $scope.onchangeyear = function () {
            $scope.print_flag = true;
            $scope.alldata = [];
            $scope.alldata1 = [];
            var data = {
                "ASMAY_Id": $scope.asmaY_Id
            };

            apiService.create("PromotionReportDetails/onchangeyear", data).then(function (Promise) {
                $scope.classlist = Promise.allclasslist;
            });

        };

        $scope.onchangeclass = function () {
            $scope.print_flag = true;
            $scope.alldata = [];
            $scope.alldata1 = [];
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id
            };
            apiService.create("PromotionReportDetails/onchangeclass", data).then(function (promise) {
                $scope.sectionlist = promise.allsectionlist;
            });
        };

        $scope.onchangesection = function () {
            $scope.print_flag = true;
            $scope.alldata = [];
            $scope.alldata1 = [];
        };

        $scope.onclickdates = function () {
            $scope.alldata = [];
            $scope.alldata1 = [];
            $scope.print_flag = true;
        };


        $scope.fillallorindi = function () {
            $scope.alldata = [];
            $scope.alldata1 = [];
            if ($scope.ts.allorindii === "A") {
                $scope.Flag = "all";
            }
            else {
                $scope.Flag = "";
            }
            $scope.print_flag = true;
            $scope.asmcL_Id = '';
            $scope.asmS_Id = '';

        };
        $scope.Flag = "all";

        $scope.submitted = false;
        $scope.Report = function () {
            $scope.alldata = [];
            $scope.alldata1 = [];
            $scope.print_flag = true;
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "ASMS_Id": $scope.asmS_Id,
                "Flag": $scope.Flag,
                "reporttype": $scope.dailybtedates
            };
            apiService.create("PromotionReportDetails/Report", data).then(function (promise) {
                $scope.print_flag = false;
                if (promise.reportdata.length > 0) {
                    $scope.alldata = promise.reportdata;
                    $scope.alldata1 = promise.reportdata1;

                    angular.forEach($scope.yearlist, function (dd) {
                        if (dd.asmaY_Id === parseInt($scope.asmaY_Id)) {
                            $scope.yearname = dd.asmaY_Year;
                        }
                    });

                    angular.forEach($scope.classlist, function (dd) {
                        if (dd.asmcL_Id === parseInt($scope.asmcL_Id)) {
                            $scope.classname = dd.asmcL_ClassName;
                        }
                    });

                    angular.forEach($scope.sectionlist, function (dd) {
                        if (dd.asmS_Id === parseInt($scope.asmS_Id)) {
                            $scope.sectionname = dd.asmC_SectionName;
                        }
                    });
                }
                else {
                    swal("No Record Found");
                    $state.reload();
                }
            });
        };


        $scope.printData = function () {
            var innerContents = '';
            if ($scope.dailybtedates === "overall"){
                innerContents = document.getElementById("printareaId").innerHTML;
            } else {
                innerContents = document.getElementById("printareaId55").innerHTML;
            }
           
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

        $scope.exportToExcel = function (table) {
            var data = "";
            if ($scope.dailybtedates === "overall"){
                data = "#A";
            } else {
                data = "#A1";
            }

            var exportHref = Excel.tableToExcel(data, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        };

        $scope.Clearid = function () {
            $state.reload();
        };
    }
})();