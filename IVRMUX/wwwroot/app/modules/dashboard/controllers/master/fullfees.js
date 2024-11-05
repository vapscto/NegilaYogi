FullfeesController.$inject = ['$scope', 'preadmission', '$location', '$routeParams'];
function FullfeesController($rootScope, $scope, $state, $location, Flash) {
        $scope.preadmission = preadmission.query();
    }]);
