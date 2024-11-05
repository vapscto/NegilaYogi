

dashboard.controller("regController", ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash',
function ($rootScope, $scope, $state, $location, dashboardService, Flash) {
    $scope.personalDetails = [
         
          {
              'sname': "hai",
              'class': "2",
              'parcentage': "80%",
              'grade': "1",
              'pyear': "2016",
              'tmarks': "456",
              'board': "svu",
          }];

    $scope.addNew = function (personalDetail) {
        $scope.personalDetails.push({
            'sname': "",
            'class': "",
            'parcentage': "",
            'grade': "",
            'pyear': "",
            'tmarks': "",
            'board': "",
        });
    };

    $scope.remove = function () {
        var newDataList = [];
        $scope.selectedAll = false;
        angular.forEach($scope.personalDetails, function (selected) {
            if (!selected.selected) {
                newDataList.push(selected);
            }
        });
        $scope.personalDetails = newDataList;
    };

    $scope.checkAll = function () {
        if (!$scope.selectedAll) {
            $scope.selectedAll = true;
        } else {
            $scope.selectedAll = false;
        }
        angular.forEach($scope.personalDetails, function (personalDetail) {
            personalDetail.selected = $scope.selectedAll;
        });
    };
    console.log("coming to Portfolio controller");


}]);

