(function () {
    'use strict';
    angular
.module('app')
        .controller('CLGRouteSessionStrengthController', CLGRouteSessionStrengthController)

    CLGRouteSessionStrengthController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter']
    function CLGRouteSessionStrengthController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {
        $scope.sortKey = "astA_Id";
        $scope.columnsTest = [];
        $scope.obj = {};
        $scope.tadprint = false;
        $scope.exp_excel_flag = true;
        $scope.print_flag = true;
        $scope.printstudents = [];
        $scope.count12 = false;

        $scope.usrname = localStorage.getItem('username');

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }

        $scope.itemsPerPage = paginationformasters;
        if ($scope.itemsPerPage == undefined || $scope.itemsPerPage == null) {
            $scope.itemsPerPage = 5;
        }
        $scope.currentPage = 1;
        $scope.coptyright = copty;

        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        $scope.reporsmart = false;
        $scope.totalstudent = 0;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        $scope.BindData = function () {
            apiService.getDATA("CLGRouteSessionStrength/getdata").then(function (promise) {
                if (promise != null) {
                    $scope.YearList = promise.yearList;
                }
                else {
                    swal("No Records Found")
                }
            })
        }


        $scope.ddate = new Date();
        $scope.searchValue = '';

        $scope.filterValue = function (obj) {
            return angular.lowercase(obj.asmcL_ClassName).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                 angular.lowercase(obj.trmR_RouteName).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.griddata = [];
        $scope.getreport = function (obj) {
            $scope.totalstudent = 0;
            $scope.griddata = [];
            $scope.all = "";
            $scope.searchValue = '';
            if ($scope.myForm.$valid) {
                var data = {
                    "asmaY_Id": $scope.asmaY_Id

                }
                apiService.create("CLGRouteSessionStrength/Getreportdetails", data).
                 then(function (promise) {

                     if (promise.griddata.length == 0) {
                         $scope.reporsmart = false;
                         swal("Record Not Found !!");
                         $state.reload();
                     }
                     else {
                         $scope.totalstudent = promise.totaltrcount;
                         $scope.students = promise.messagelist;
                         $scope.classarray = promise.schedultime;
                         $scope.griddata = promise.griddata;
                         $scope.presentCountgrid = $scope.students.length;
                         $scope.exp_excel_flag = false;
                         $scope.print_flag = false;
                         $scope.temparray = [];
                         $scope.temparray2 = [];

                         angular.forEach($scope.YearList, function (fff) {

                             if (fff.asmaY_Id == $scope.asmaY_Id) {
                                 $scope.yearname = fff.asmaY_Year;
                             }

                         })
                         angular.forEach($scope.students, function (t1) {
                             var rtcnt = 0;
                             angular.forEach($scope.griddata, function (t2) {

                                 if (t1.trmR_Id == t2.trmR_Id) {
                                     rtcnt += t2.stdCount
                                 }

                             })
                             $scope.temparray.push({ trmR_Id: t1.trmR_Id, total: rtcnt })
                         })



                         angular.forEach($scope.students, function (yy) {

                             angular.forEach($scope.temparray, function (gg) {
                                 if (gg.trmR_Id == yy.trmR_Id) {
                                     yy.total = gg.total;
                                 }
                             })
                         })


                         angular.forEach($scope.classarray, function (t1) {
                             var rtcnt1 = 0;
                             angular.forEach($scope.griddata, function (t2) {

                                 if (t1.trmS_Id == t2.trmS_Id) {
                                     rtcnt1 += t2.stdCount
                                 }

                             })
                             $scope.temparray2.push({ trmS_Id: t1.trmS_Id, total: rtcnt1 })

                         })
                         $scope.total12 = 0;
                         angular.forEach($scope.temparray2, function (t2) {
                             $scope.total12 += t2.total
                         })
                     }
                 })
            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.printData = function () {

            var innerContents = document.getElementById("printareaId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
           '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
       '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
        '<link type="text/css" media="print" href="css/print/preadmission/RegReportPdf.css" rel="stylesheet" />' +
       '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
       );
            popupWinindow.document.close();

        }

        $scope.exportToExcel = function (tabel1) {

            var exportHref = Excel.tableToExcel(tabel1, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        }

        $scope.validreport = function () {

            $scope.students = [];

        }
    };

})();