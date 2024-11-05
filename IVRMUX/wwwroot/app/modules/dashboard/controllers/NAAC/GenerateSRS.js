(function () {
    'use strict';
    angular.module('app').controller('NAAC_Master_CriteriaController', NAAC_Master_CriteriaController)
    NAAC_Master_CriteriaController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$filter', '$timeout', 'Excel', '$sce']
    function NAAC_Master_CriteriaController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $filter, $timeout, Excel, $sce) {

        $scope.submitted = false;


        $scope.savemsg1 = function () {
            if ($scope.myForm1.$valid) {

            }
            else {
                $scope.submitted = true;
            }
        }


        $scope.interacted1 = function (field) {
            return $scope.submitted;
        };
    }

})();