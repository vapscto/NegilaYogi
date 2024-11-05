(function () {
    'use strict';
    angular
        .module('app')
        .controller('ExchangeableCourseController', ExchangeableCourseController)

    ExchangeableCourseController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout']
    function ExchangeableCourseController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {


        $scope.ListofItems = [];
        $scope.ListofItems = [

        {name: "Program-E",program: "BC0031",  certificateName: "Automation Certificate", year: "2018-2019" },
        {name: "Program-D",program: "BC0032",  certificateName: " Certificate-D", year: "2017-2018" },
        {name: "Program-C",program: "BC0033",  certificateName: " Certificate-C", year: "2016-2017" },
        {name: "Program-B",program: "BC0034",  certificateName: " Certificate-B", year: "2015-2016" },
        { name: "Program-A",program: "BC0035",  certificateName: " Certificate-A", year: "2014-2015" },
        ]


        $scope.loaddata = function () {

            var pageid = 2;
            apiService.getURI("CurricularAspects/getdata", pageid).then(function (promise) {
                $scope.getinstitutioncycle = promise.getinstitutioncycle;
                $scope.cycleid = promise.getinstitutioncycle[0].cycleid;
                $scope.getparentidzero = promise.getinstitution;

                $scope.naacsL_InstitutionTypeFlg = promise.naacsL_InstitutionTypeFlg;

                angular.forEach($scope.getparentidzero, function (options) {
                    options.select = true;
                })


            })
        }


        $scope.get_selcetYear = function (data) {

            var nofyear = Number($scope.noofyear);
            angular.forEach($scope.yearlist, function (tt) {
                tt.select = false;
            })
            var s = 0;
            angular.forEach($scope.yearlist, function (pp) {
                if (s < nofyear) {
                    pp.select = true;
                }
                s += 1;
            })

        }

        $scope.togchkbx = function () {  
            $scope.noofyear = 0;
            angular.forEach($scope.yearlist, function (ff) {
                if (ff.select == true) {
                    $scope.noofyear += 1;
                }

            })
        }


        $scope.exportToExcel = function (table) {
            var excelname = 'Cat 1.3.xls';
            var exportHref = Excel.tableToExcel(table, '1.3.3');
            $timeout(function () {
                var a = document.createElement('a');
                a.href = exportHref;
                a.download = excelname;
                document.body.appendChild(a);
                a.click();
                a.remove();
            }, 100);
        }

        //$scope.exportToExcel = function (table) {            
        //    var exportHref = Excel.tableToExcel(table, 'sheet name');
        //        $timeout(function () { location.href = exportHref; }, 100);           
        //}


        $scope.cancel = function () {
            $state.reload();
        }
        $scope.printData = function () {

            var innerContents = document.getElementById("printSectionId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/ExchangableCoursePdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length !== 0 && ivrmcofigsettings.length !== undefined) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;
        var logopath = "";
        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null && admfigsettings.length !== 0 && admfigsettings.length !== undefined) {
            logopath = admfigsettings[0].asC_Logo_Path;
        } else {
            logopath = "";
        }
        $scope.imgname = logopath;

        $scope.table_flag = false;

    }
})();

