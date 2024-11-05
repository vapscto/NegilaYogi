
(function () {
    'use strict';
    angular
.module('app')
.controller('AdmissionfeeController', AdmissionfeeController)

AdmissionfeeController.$inject = ['$scope', 'preadmission', '$location', '$routeParams'];
function AdmissionfeeController($rootScope, $scope, $state, $location, Flash) {
    $scope.preadmission = preadmission.query();
}
})();
