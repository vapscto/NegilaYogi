SeatController.$inject = ['$scope', 'preadmission', '$location', '$routeParams'];
function SeatController($rootScope, $scope, $state, $location, Flash) {
        $scope.preadmission = preadmission.query();
    }]);
