CnfController.$inject = ['$scope', 'preadmission', '$location', '$routeParams'];
function CnfController($rootScope, $scope, $state, $location, Flash) {
        $scope.preadmission = preadmission.query();
    }]);
