
(function () {
    'use strict';
    angular
.module('app')
        .controller('AbbreviationReportController', AbbreviationReportController)

    AbbreviationReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', 'Excel','$timeout']
    function AbbreviationReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, Excel, $timeout) {
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.masterlist = false;
    
        $scope.sortReverse = true;
       
     
        $scope.feeorder = false;
        $scope.feetext = true;
        $scope.searchValue = "";
        $scope.ddate = new Date();
        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        $scope.tadprint = false;

        $scope.loaddata = function () {
            var pageid = 2;
            //apiService.getURI("KMLogBook/getreportdata/", pageid).then(function (promise) {
            //    $scope.fillvehicletype = promise.fillvehicletype;
            //    $scope.monthlist = promise.monthlist;
            //})
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

        $scope.headname = '';
        $scope.issuertype1 = 'STF';
     
        $scope.masterdistancerate = [];
        $scope.getreportdata = [];
        $scope.submitted = false;
        $scope.savedata = function () {
            debugger;
            $scope.getreportdata = [];
            

                    var data = {
                        "tsallorindii": $scope.issuertype1,
                      
                    }
                    apiService.create("ClasswiseConsolidatedReport/getabreport/", data).then(function (promise) {
                        $scope.getreportdata = promise.getreportdata;
                        if ($scope.getreportdata.length >0) {
                            $scope.getreportdata = promise.getreportdata;
                            
                        }
                        else {
                            swal('No Record Found')
                        }
                        
                    })
               
           
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.rdchange = function () {
            $scope.getreportdata = [];
        };

      

        $scope.checkvalid1 = function () {
            $scope.TRDC_ToKM = parseInt($scope.TRDC_FromKM) + 1;
        }

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }


        $scope.searchValue = '';
     

        $scope.clear = function () {
            $state.reload();
        }


        $scope.printData = function () {

            var innerContents = '';
            if ($scope.issuertype1==='STF') {
                innerContents = document.getElementById("printareaId1").innerHTML;
            }
            else {
                innerContents = document.getElementById("printareaId").innerHTML;
            }

      
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/preadmission/RegReportPdf.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 22100);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();

        }

        $scope.exportToExcel = function () {
            debugger;
            var tabel1 = '';
            if ($scope.issuertype1 === 'STF') {
                tabel1 = '#table12';
            }
            else {
                tabel1 = '#table1';
            }
            var exportHref = Excel.tableToExcel(tabel1, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        }
    };
})();


