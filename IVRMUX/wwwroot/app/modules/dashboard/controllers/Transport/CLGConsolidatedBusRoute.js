(function () {
    'use strict';
    angular
.module('app')
        .controller('CLGConsolidatedBusRouteController', CLGConsolidatedBusRouteController)

    CLGConsolidatedBusRouteController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter']
    function CLGConsolidatedBusRouteController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {
        $scope.sortKey = "routeid";
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

        $scope.cancel = function () {
            $scope.asmaY_Id = '';
            $scope.griddata = [];
            $scope.grid = false;

        }

        $scope.tadprint = false;

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

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        $scope.BindData = function () {
            apiService.getDATA("CLGConsolidatedBusRoute/getdata").then(function (promise) {
                if (promise != null) {
                    $scope.YearList = promise.yearList;
                }
                else {
                    swal("No Records Found")
                }
            })
        }      

        $scope.searchValue = '';

        $scope.filterValue = function (obj) {
            return angular.lowercase(obj.asmcL_ClassName).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                 angular.lowercase(obj.trmR_RouteName).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.grid = false;
        $scope.griddata = [];
        $scope.getreport = function (obj) {
            $scope.grid = false;
            $scope.all = "";
            $scope.searchValue = '';          
            if ($scope.myForm.$valid) {
                var data =   {                       
                        "asmaY_Id": $scope.asmaY_Id                           

                    }
                apiService.create("CLGConsolidatedBusRoute/Getreportdetails", data).
                 then(function (promise) {
                   
                     $scope.griddata = promise.griddata;
                     console.log($scope.griddata)

                     angular.forEach($scope.YearList, function (fff) {

                         if (fff.asmaY_Id == $scope.asmaY_Id) {
                             $scope.yearname = fff.asmaY_Year;
                         }

                     })
                     $scope.TwoWayNewCounttotal = 0;
                     $scope.TwoWayregCounttotal = 0;
                     $scope.TwoWaytotalCounttotal = 0;

                     $scope.onePWayNewCounttotal = 0;
                     $scope.onePWayregCounttotal = 0;
                     $scope.onePWaytotalCounttotal = 0;

                     $scope.oneDWayNewCounttotal = 0;
                     $scope.oneDWayregCounttotal = 0;
                     $scope.oneDWaytotalCounttotal = 0;
                     $scope.totalstudent = $scope.griddata[0].totaltransport;
                     angular.forEach($scope.griddata, function (dd) {
                         $scope.TwoWayNewCounttotal += dd.twonewcount;
                         $scope.TwoWayregCounttotal += dd.tworegcount;
                         $scope.TwoWaytotalCounttotal += dd.twototalcount;

                         $scope.onePWayNewCounttotal += dd.Onepicknewcount;
                         $scope.onePWayregCounttotal += dd.Onepickregcount;
                         $scope.onePWaytotalCounttotal += dd.onepicktotalcount;

                         $scope.oneDWayNewCounttotal += dd.Onedropnewcount;
                         $scope.oneDWayregCounttotal += dd.Onedropregcount;
                         $scope.oneDWaytotalCounttotal += dd.onedroptotalcount;

                     })
                     if ($scope.griddata.length>0) {
                         $scope.grid = true;
                     }
                     else {
                         swal('No Record Found');
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
           '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 1000);">' + innerContents + '</html>'
           );
                popupWinindow.document.close();  
         
        }
        
        $scope.exportToExcel = function (export_id) {
            var exportHref = Excel.tableToExcel(export_id, 'sheet name');
                $timeout(function () { location.href = exportHref; }, 1000);
            }

        $scope.validreport = function () {

            $scope.students = [];

        }
   };

})();