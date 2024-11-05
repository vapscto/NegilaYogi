

(function () {
    'use strict';
    angular
.module('app')
.controller('ClassSecessionWiseFeeCollectionReportController', ClassSecessionWiseFeeCollectionReportController123)

    ClassSecessionWiseFeeCollectionReportController123.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http','superCache']
    function ClassSecessionWiseFeeCollectionReportController123($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
      
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;
            var pageid = 2;
            apiService.getURI("ClassSecessionWiseFeeCollectionReport/getalldetails123", pageid).
        then(function (promise) {
            $scope.acayyearbind = promise.acayear;
            $scope.groupsnames = promise.groupslist;
            $scope.userlistarray = promise.userlist;
         
        })
        }


        $scope.ShowReportdata = function () {

            var data = {
                "typeofrpt": $scope.rndind,
                "asmyid": $scope.academicyr,
                "FMCU_Id": $scope.currencymodel,
            }
            apiService.create("ClassSecessionWiseFeeCollectionReport/getreport", data).
        then(function (promise) {
            $scope.students = promise.reportlist;
        
        })
        }
    }
})();

