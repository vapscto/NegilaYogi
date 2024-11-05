

(function () {
    'use strict';
    angular
        .module('app')
        .controller('CandidateDashboardController', CandidateDashboardController);

    CandidateDashboardController.$inject = ['$scope','$state'];
    function CandidateDashboardController($scope, $state) {

        $scope.loaddata = function () {
            $scope.search = "";
        };

        $scope.redirectpage = function () {
            $state.go('app.addCandidate');
        };
    }
})();