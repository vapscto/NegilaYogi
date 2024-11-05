dashboard.controller("VikasainterController", ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'superCache',
function ($rootScope, $scope, $state, $location, dashboardService, Flash, superCache) {

    $scope.choices = [{ id: 'choice1' }];

    $scope.addNewChoice = function () {
        var newItemNo = $scope.choices.length + 1;
        $scope.choices.push({ 'id': 'choice' + newItemNo });
    };

    $scope.removeChoice = function () {
        var lastItem = $scope.choices.length - 1;
        $scope.choices.splice(lastItem);
    };

}]);