

(function () {
    'use strict';
    angular
.module('app')
.controller('FeeCurrencyFactoReportController', FeeCurrencyFactoReport123)

    FeeCurrencyFactoReport123.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http','superCache']
    function FeeCurrencyFactoReport123($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
        $scope.onclickloaddata = function () {
            if ($scope.rndind == "Indi") {
                $scope.rndind123 = true;
            }
            else {
                $scope.rndind123 = false;
            }
          
        };
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;
            var pageid = 2;
            apiService.getURI("FeeCurrencyFactoReport/getalldetails123", pageid).
        then(function (promise) {
            $scope.acayyearbind = promise.acayear;
            $scope.currencylist = promise.factorcurrency
            $scope.onclickloaddata();
        })
        }


        $scope.ShowReportdata = function () {

            var data = {
                "typeofrpt": $scope.rndind,
                "asmyid": $scope.academicyr,
                "FMCU_Id": $scope.currencymodel,
            }
            apiService.create("FeeCurrencyFactoReport/getreport", data).
        then(function (promise) {
            $scope.students = promise.reportlist;
           // $scope.currentPage = 1;
          //  $scope.itemsPerPage = 5;
        })
        }
    }
})();

