(function () {
    'use strict';
    angular
.module('app')
        .controller('DriverChartReportController', DriverChartReportController)

    DriverChartReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter']
    function DriverChartReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {
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

        $scope.ddate = new Date();
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

        $scope.allstudentcheck = function () {

            angular.forEach($scope.fillvahicleno, function (ff) {
                if ($scope.allstdcheck == true) {
                    ff.checkedgrplst1 = true;

                }
                else {
                    ff.checkedgrplst1 = false;
                }


            })


        }

        $scope.firstfnc1 = function (aa) {

            $scope.allstdcheck = $scope.fillvahicleno.every(function (itm) { return itm.checkedgrplst1; });

        }

        $scope.BindData = function () {
            var pageid = 1;
            apiService.getURI("DriverChartReport/getdata", pageid).then(function (promise) {
                if (promise != null) {
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
        $scope.getloaddata = [];

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.griddata = [];
       
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };



        $scope.vehicletypechange = function () {
            $scope.allstdcheck = false;
            $scope.getloaddata = [];
            $scope.griddeatails = false;
            $scope.opentt = 0;
            $scope.closett = 0;
            $scope.totaltt = 0;
            $scope.literstt = 0;
            $scope.millagett = 0;
            $scope.ratett = 0;
            $scope.totalamttt = 0;
            $scope.emstt = 0;
            $scope.extratt = 0;
            $scope.grsstt = 0;
            var data = {
                "TRMVT_Id": $scope.trmvT_Id,
            } 
            apiService.create("DriverChartReport/vehicletypechange", data).
                then(function (promise) {

                    $scope.fillvahicleno = promise.fillvahicleno;


                });
        }
        $scope.valsstd = [];

        $scope.cdeposit = [];
        $scope.getreport = function () {
            $scope.getloaddata = [];

            $scope.griddeatails = false;
            $scope.opentt = 0;
            $scope.closett = 0;
            $scope.totaltt = 0;
            $scope.literstt = 0;
            $scope.millagett = 0;
            $scope.ratett = 0;
            $scope.totalamttt = 0;
            $scope.emstt = 0;
            $scope.extratt = 0;
            $scope.grsstt = 0;

            $scope.valsstd = [];
            $scope.all = "";
            $scope.searchValue = '';
            if ($scope.myForm.$valid) {
                for (var u = 0; u < $scope.fillvahicleno.length; u++) {
                    if ($scope.fillvahicleno[u].checkedgrplst1 == true) {
                        $scope.valsstd.push($scope.fillvahicleno[u]);
                    }
                }
                if ($scope.valsstd.length  > 0) {
                    var fromdate1 = $scope.frmdate == null ? "" : $filter('date')($scope.frmdate, "yyyy-MM-dd");
                    var todate1 = $scope.todate == null ? "" : $filter('date')($scope.todate, "yyyy-MM-dd");
                    if ($scope.trmvT_Id == '0') {
                        var data = {
                            "FRMDATE": fromdate1,
                            "TODATE": todate1,
                            "vhlid": $scope.valsstd,

                        }
                    }
                    else {
                        var data = {
                            "TRMVT_Id": $scope.trmvT_Id,
                            "FRMDATE": fromdate1,
                            "TODATE": todate1,
                            "vhlid": $scope.valsstd,

                        }
                    }



                    apiService.create("DriverChartReport/Getreportdetails", data).
                        then(function (promise) {
       
                            if (promise.getloaddata.length>0) {
                                $scope.getloaddata = promise.getloaddata;
                                console.log($scope.getloaddata);
                                $scope.griddeatails = true;
                                $scope.opentt = 0;
                                $scope.closett = 0;
                                $scope.totaltt = 0;
                                $scope.literstt = 0;
                                $scope.millagett = 0;
                                $scope.ratett = 0;
                                $scope.totalamttt = 0;
                                $scope.emstt = 0;
                                $scope.extratt = 0;
                                $scope.grsstt = 0;

                                angular.forEach($scope.getloaddata, function (tt) {


                                    $scope.closett += tt.trdC_ToKM;
                                    $scope.opentt += tt.trdC_FromKM;
                                    $scope.totaltt += tt.trdC_TotalKM;
                                    $scope.literstt += tt.trdC_NoofLtr;
                                    $scope.millagett += tt.trdC_TotalMileage;
                                    $scope.ratett += tt.trdC_RateKm;
                                    $scope.totalamttt += tt.trdC_TotalAmount;
                                    $scope.emstt += tt.trdC_Emission;
                                    $scope.extratt += tt.trdC_AddtAmt,
                                        $scope.grsstt += tt.trdC_GrossAmount;
                                })
                            }
                            else {
                                $scope.reporsmart = false;
                                $scope.griddeatails = false;
                                swal("Record Not Found !!");
                                $state.reload();
                            }

                        })
                }
                else {
                    swal('Select Vehicle Number')
                }
                
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