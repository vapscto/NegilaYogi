(function () {
    'use strict';
    angular
        .module('app')
        .controller('UploadPageController', UploadPageController)

    UploadPageController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout']
    function UploadPageController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout) {


        $scope.ListofItems = [];
        $scope.ListofItems = [{ name: "BC0031", program: "Program-E", certificateName: "Automation Certificate", year: "2018-2019" },
        { name: "BC0032", program: "Program-D", certificateName: " Certificate-D", year: "2017-2018" },
        { name: "BC0033", program: "Program-C", certificateName: " Certificate-C", year: "2016-2017" },
        { name: "BC0034", program: "Program-B", certificateName: " Certificate-B", year: "2015-2016" },
        { name: "BC0035", program: "Program-A", certificateName: " Certificate-A", year: "2014-2015" },
        ]


        $scope.loaddata = function () {

            var pageid = 2;
            apiService.getURI("CurricularAspects/getdata", pageid).then(function (promise) {
                $scope.noofyear = 4;
                $scope.yearlist = promise.yearlist;

                //$scope.yearlist = [
                //    { asmaY_Year: "2018-2019", asmaY_Id: 5 },
                //    { asmaY_Year: "2017-2018", asmaY_Id: 4 },
                //    { asmaY_Year: "2016-2017", asmaY_Id: 3 },
                //    { asmaY_Year: "2015-2016", asmaY_Id: 2 },
                //    { asmaY_Year: "2014-2015", asmaY_Id: 1 },
                //]

                var s = 0;
                angular.forEach($scope.yearlist, function (pp) {
                    if (s < $scope.noofyear) {
                        pp.select = true;
                    }
                    s += 1;
                })


            })
        }


        $scope.printData = function (printmodal) {
            var innerContents = document.getElementById("printmodal").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print.css" />' +
                '<link href="plugins/bootstrap/css/bootstrap.css" />' +
                '<link href="css/style.css" rel="stylesheet" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }
      

    }
})();

