(function () {
    'use strict';
    angular
.module('app')
        .controller('BusBookingDetailsReportController', BusBookingDetailsReportController)

    BusBookingDetailsReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter']
    function BusBookingDetailsReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {
        $scope.sortKey = "astA_Id";
        $scope.columnsTest = [];
        $scope.obj = {};
        $scope.tadprint = false;
        $scope.exp_excel_flag = true;
        $scope.print_flag = true;
        $scope.printstudents = [];
        $scope.count12 = false;
        $scope.datecheck = false;
        $scope.usrname = localStorage.getItem('username');


        //var id = 1;
        //$scope.itemsPerPage = 10;
        //$scope.currentPage = 1;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.usrname = localStorage.getItem('username');
        $scope.itemsPerPage = paginationformasters;
        if ($scope.itemsPerPage == undefined || $scope.itemsPerPage == null) {
            $scope.itemsPerPage = 5;
        }
        $scope.currentPage = 1;
        $scope.coptyright = copty;
        $scope.ddate = new Date();
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
            apiService.getDATA("BusBookingDetailsReport/getdata").then(function (promise) {
                if (promise != null) {
                    $scope.hiregrouplist = promise.hiregrouplist;
                    $scope.fillvahicletype = promise.fillvahicletype;

                }
                else {
                    swal("No Records Found")
                }



                //Set From date and To date
                $scope.frmdate = new Date();

                $scope.todate = $scope.Employee.frmdate;
                $scope.minDateTo = new Date(
                    $scope.todate.getFullYear(),
                    $scope.todate.getMonth(),
                    $scope.todate.getDate());

            })
        }

        $scope.FormatType = "Format1";

        //setToDate
        $scope.setToDate = function (frmdate) {

            $scope.todate = frmdate;
            $scope.minDateTo = new Date(
                $scope.todate.getFullYear(),
                $scope.todate.getMonth(),
                $scope.todate.getDate());
            $scope.todate = "";
            if ($scope.griddeatails) {
                $scope.griddeatails = false;
            }
        }

        $scope.OnchageToDate = function () {

            if ($scope.griddeatails) {
                $scope.griddeatails = false;
            }
        }



        $scope.cancel = function () {
            //$scope.searchValue = '';
            //$scope.frmdate = '';
            //$scope.todate = '';
            //$scope.griddata = [];
            //$scope.griddeatails = false;
            //$scope.griddata.length = 0;
            $state.reload();
            
        }

        $scope.searchValue = '';
        $scope.griddeatails = false;


        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.griddata = [];
       
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

       
       


        $scope.cdeposit = [];
        $scope.getreport = function (obj) {

            $scope.all = "";
            $scope.searchValue = '';
            if ($scope.myForm.$valid) {


                var fromdate1 = $scope.frmdate == null ? "" : $filter('date')($scope.frmdate, "yyyy-MM-dd");
                var todate1 = $scope.todate == null ? "" : $filter('date')($scope.todate, "yyyy-MM-dd");
               // var data = {};
                //if ($scope.trmvT_Id=='0') {
                //     data = {
                //        "frmdate": fromdate1,
                //        "todate": todate1,
                //        "TRHG_Id": $scope.trhG_Id,
                //}
                   
                //} else {
                  var data = {
                        "frmdate": fromdate1,
                        "todate": todate1,
                        "TRHG_Id": $scope.trhG_Id,
                         "TRMVT_Id": $scope.trmvT_Id,
                    }

                //}

               
                apiService.create("BusBookingDetailsReport/Getreportdetails", data).
                 then(function (promise) {
                     debugger;
                     if (promise.griddata.length >0) {
                         $scope.griddata = promise.griddata;
                         $scope.griddeatails = true;

                         console.log($scope.griddata);
                        
                     }
                     else {
                         $scope.reporsmart = false;
                         swal("Record Not Found !!");
                         $state.reload();
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
       '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); },900);">' + innerContents + '</html>'
       );
            popupWinindow.document.close();

        }

        $scope.exportToExcel = function (tabel1) {

            var exportHref = Excel.tableToExcel(tabel1, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 900);
        }

        $scope.validreport = function () {

            $scope.students = [];

        }
    };

})();