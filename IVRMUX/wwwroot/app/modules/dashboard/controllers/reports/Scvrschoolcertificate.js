(function () {
    'use strict';
    angular
        .module('app')
        .controller('ScvrschoolcertificateController', ScvrschoolcertificateController)

    ScvrschoolcertificateController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function ScvrschoolcertificateController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
        $scope.today = new Date();
        $scope.onpageload = function () {
            //$scope.study = true;
            var pageid = 2;

            apiService.get("Scvrschoolcertificate/getdata/2").then(function (promise) {

            })
        }
        $scope.printToCart = function () {
            var innerContents = document.getElementById('BBKVTC').innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BGStudentTcPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close(); 
        };
    }
})();