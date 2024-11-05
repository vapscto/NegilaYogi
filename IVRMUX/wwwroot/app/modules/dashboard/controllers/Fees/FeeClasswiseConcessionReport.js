(function () {
    'use strict';
    angular
.module('app')
.controller('FeeClasswiseConcessionReportController', FeeClasswiseConcessionReportController)
    FeeClasswiseConcessionReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http','superCache']
    function FeeClasswiseConcessionReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5
            var pageid = 1;

            apiService.getURI("FeeClasswiseConcessionReport/getalldetails", pageid).
            then(function (promise) {

                $scope.arrlist6 = promise.adcyear;
                $scope.groupcount = promise.fillmastergroup;
                 $scope.headcount = promise.fillmasterhead;
                $scope.classcount = promise.fillclass;
                $scope.sectioncount = promise.fillsection;
               
            })
        }

        $scope.ShowReport = function () {

            var data = {
                "FMG_Id": $scope.fmG_Id,
                "ASMAY_Id": $scope.asmaY_Id,
                "FMH_Id": $scope.fmH_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "AMSC_Id": $scope.asmC_Id,
              
            }


            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("FeeClasswiseConcessionReport/radiobtndata", data).
          then(function (promise) {

              $scope.students = promise.groupalldata;



          })

        }

    }
})();
