(function () {
    'use strict';
    angular
        .module('app')
        .controller('NaacICT413Report', NaacICT413Report)

    NaacICT413Report.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q', 'Excel', '$timeout']
    function NaacICT413Report($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q, Excel, $timeout) {

        $scope.sort = function (keyname) {
            $scope.propertyName = keyname;   //set the propertyName to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        $scope.submitted = false;

        $scope.Clearid = function () {
            $state.reload();
        };
        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("NaacCriteria4Report/loaddata", pageid).then(function (promise) {
                $scope.getinstitutioncycle = promise.getinstitutioncycle;
                $scope.cycleid = promise.getinstitutioncycle[0].cycleid;
                $scope.getparentidzero = promise.getinstitution;

                angular.forEach($scope.getparentidzero, function (options) {
                    options.select = true;
                })

            });
        };
    
        $scope.Report = function () {
            var data = {
                "ASMAY_Id": 2
            };
            apiService.create("NaacCriteria4Report/ReportCriteria4", data).then(function (promise) {
           
                $scope.alldata7 = promise.alldata7;
                $scope.alldata72 = promise.alldata72;
                if ($scope.alldata7.length > 0) {
                    angular.forEach($scope.alldata7, function (tt) {
                        $scope.mainArray = [];
                        angular.forEach($scope.alldata72, function (ss) {
                            if (tt.ncaC413ICT_Id == ss.ncaC413ICT_Id) {
                                $scope.mainArray.push(ss);
                            }
                        })
                        tt.listdata = $scope.mainArray;
                    })
                    console.log($scope.alldata7);

                    $scope.printflag = true;
                }
                else {
                    swal('No Records Found!')
                }
            });
        };
        $scope.exportToExcel = function (table) {
            var exportHref = Excel.tableToExcel(table, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        }

        $scope.printData = function () {
            var innerContents = '';
            innerContents = document.getElementById("printareaId1").innerHTML;
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
    }
})();