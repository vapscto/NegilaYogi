StudentprofileController.$inject = ['$scope', 'preadmission', '$location', '$routeParams'];
function StudentprofileController($rootScope, $scope, $state, $location, Flash) {
        $scope.preadmission = preadmission.query();
    }]);
