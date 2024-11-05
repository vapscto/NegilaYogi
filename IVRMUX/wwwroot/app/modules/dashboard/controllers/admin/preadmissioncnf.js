PreadmissioncnfController.$inject = ['$scope', 'preadmission', '$location', '$routeParams'];
function PreadmissioncnfController($rootScope, $scope, $state, $location, Flash) {
        $scope.preadmission = preadmission.query();
    }]);
