DocumentsController.$inject = ['$scope', 'preadmission', '$location', '$routeParams'];
function DocumentsController($rootScope, $scope, $state, $location, Flash) {
        $scope.preadmission = preadmission.query();
    }]);
