PreadmissionController.$inject = ['$scope', 'preadmission', '$location', '$routeParams'];
function PreadmissionController($rootScope, $scope, $state, $location, Flash) {
        $scope.preadmission = preadmission.query();
    }]);
