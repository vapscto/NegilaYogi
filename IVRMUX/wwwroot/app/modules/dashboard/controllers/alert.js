app.controller('alertController', function ($scope, $window) {
    $scope.ShowConfirm = function () {
        if ($window.confirm("Please confirm?")) {
            $scope.Message = "You clicked YES.";
        } else {
            $scope.Message = "You clicked NO.";
        }
    }
});