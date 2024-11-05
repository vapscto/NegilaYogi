RegistrationController.$inject = ['$scope', 'preadmission', '$location', '$routeParams'];
function RegistrationController($rootScope, $scope, $state, $location, Flash) {
        $scope.preadmission = preadmission.query();
    }]);
