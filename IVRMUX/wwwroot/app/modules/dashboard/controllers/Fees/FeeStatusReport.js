(function () {
    'use strict';
    angular
.module('app')
.controller('FeeStatusReportController', FeeStatusReportController)
    FeeStatusReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http','superCache']
    function FeeStatusReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5
            var pageid = 1;

            apiService.getURI("FeeStatusReport/getalldetails", pageid).
            then(function (promise) {

                $scope.arrlist6 = promise.adcyear;

            })
        };

        $scope.ShowReport = function (asmaY_Id, fromDate, todate, result) {

            var data = {

                "ASMAY_Id": asmaY_Id,
                "From_Date": fromDate,
                "To_Date": todate,
                "type": result,
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("FeeStatusReport/radiobtndata", data).
         then(function (promise) {


             if (promise.type == "group") {

                 $scope.groups = promise.groupalldata;
                 $scope.grp = true;
                 $scope.hrd = false;
             }

             else if (promise.type == "head") {

                 $scope.heads = promise.headalldata;
                 $scope.grp = false;
                 $scope.hrd = true;
             }

         })

        };
    }
})();
