﻿(function () {
    'use strict';
    angular
        .module('app')
        .controller('NAACMC717DisabledFriendlyEnvironmentReportController', NAACMC717DisabledFriendlyEnvironmentReportController)

    NAACMC717DisabledFriendlyEnvironmentReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', '$q', 'Excel', '$timeout']
    function NAACMC717DisabledFriendlyEnvironmentReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, $q, Excel, $timeout) {

        $scope.sort = function (keyname) {
            $scope.propertyName = keyname;
            $scope.reverse = !$scope.reverse;
        };
        $scope.submitted = false;

        $scope.Clearid = function () {
            $state.reload();
        };

        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("NAAC711GenderEquityReport/loaddata", pageid).then(function (promise) {
                $scope.getinstitutioncycle = promise.getinstitutioncycle;
                $scope.cycleid = promise.getinstitutioncycle[0].cycleid;
                $scope.getparentidzero = promise.getinstitution;
                $scope.naacsL_InstitutionTypeFlg = promise.naacsL_InstitutionTypeFlg;
                angular.forEach($scope.getparentidzero, function (options) {
                    options.select = true;
                });
            });
        };

        $scope.get_selcetYear = function (data) {
            var nofyear = Number($scope.noofyear);
            angular.forEach($scope.allacademicyear, function (tt) {
                tt.select = false;
            });
            var s = 0;
            angular.forEach($scope.allacademicyear, function (pp) {
                if (s < nofyear) {
                    pp.select = true;
                }
                s += 1;
            });
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.isOptionsRequired = function () {
            return !$scope.getparentidzero.some(function (options) {
                return options.select;
            });
        };

        $scope.togchkbx = function () {
            $scope.noofyear = 0;
            angular.forEach($scope.getparentidzero, function (ff) {
                if (ff.select == true) {
                    $scope.noofyear += 1;
                }
            });
        };

        $scope.printflag = false;
        $scope.Report = function () {
            $scope.selected_Inst = [];
            angular.forEach($scope.getparentidzero, function (mm) {
                if (mm.select) {
                    $scope.selected_Inst.push(mm);
                }
            });

            if ($scope.myForm.$valid) {
                var data = {
                    "cycleid": $scope.cycleid,
                    selected_Inst: $scope.selected_Inst,
                    Type: "717DisabledFriendlyEnvironment"
                };
                apiService.create("NAAC711GenderEquityReport/Report", data).then(function (promise) {
                    if (promise.alldata.length > 0) {
                        $scope.alldata6 = promise.alldata[0];
                        $scope.printflag = true;
                    }
                    else {
                        swal('No Records Found!');
                    }
                });
            }
            else {
                $scope.submitted = true;
            }  
            
        };
        $scope.exportToExcel = function (table) {
            var exportHref = Excel.tableToExcel(table, 'sheet name');
            $timeout(function () { location.href = exportHref; }, 100);
        };

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
        };
    }
})();