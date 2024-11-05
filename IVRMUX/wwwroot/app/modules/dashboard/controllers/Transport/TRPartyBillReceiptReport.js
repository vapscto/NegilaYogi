(function () {
    'use strict';
    angular
.module('app')
        .controller('TRPartyBillReceiptReportController', TRPartyBillReceiptReportController)

    TRPartyBillReceiptReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter']
    function TRPartyBillReceiptReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {
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
        $scope.ddate = new Date();
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
            apiService.getDATA("TRPartyBillReceiptReport/getdata").then(function (promise) {
                if (promise != null) {
                    $scope.hiregrouplist = promise.hiregrouplist;
                      
                }
                else {
                    swal("No Records Found")
                }
            })
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

        $scope.gpname = '';
        $scope.cdeposit = [];
        $scope.getreport = function () {
            $scope.gpname = '';
            $scope.all = "";
            $scope.searchValue = '';
            if ($scope.myForm.$valid) {
              
                    var data = {
                        "TRHG_Id": $scope.trhG_Id,
                    }
              
                
                apiService.create("TRPartyBillReceiptReport/Getreportdetails", data).
                 then(function (promise) {
                     debugger;
                     if (promise.griddata != null) {
                         $scope.griddata = promise.griddata;

                         angular.forEach($scope.hiregrouplist, function (ll) {
                             if (ll.trhG_Id == $scope.trhG_Id) {
                                 $scope.gpname = ll.trhG_HirerGroup;
                             }
                         })




                         $scope.griddeatails = true;
                         $scope.chargett = 0;
                         $scope.paidtt = 0;
                         $scope.disctt = 0;
                         $scope.baltt = 0;


                     



                         angular.forEach($scope.griddata, function (tt) {

                             $scope.chargett += tt.TRTP_BillAmount;
                             $scope.paidtt += tt.TRTPP_PaidAmount;
                             $scope.disctt += tt.TRTP_DiscountAmount ;
                             if (tt.TRTPP_ReceiptNo == null || tt.TRTPP_ReceiptNo == undefined || tt.TRTPP_ReceiptNo == '') {
                                 $scope.baltt += tt.TRTP_BillAmount;
                             }
                             else {
                                 $scope.baltt += tt.RunningBalAmount;
                             }

                            
                         })
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
       '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 1000);">' + innerContents + '</html>'
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