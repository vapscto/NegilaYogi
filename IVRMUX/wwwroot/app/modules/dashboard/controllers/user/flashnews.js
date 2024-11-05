
(function () {
    'use strict';
    angular
.module('app')
.controller('flashnewsController', flashnewsController)
    flashnewsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$window', 'FormSubmitter'];
    function flashnewsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $window, FormSubmitter) {

        var HostName = location.host;

    $scope.submitted = false;
    $scope.SubmitForm = function () {
        $scope.submitted = true;
        if ($scope.myForm.$valid) {
            var data = {
                "password": $scope.curpwd,
              
               
            }
            apiService.create("flashnews/", data).
                  then(function (promise) {
                      if (promise.returnMsg === "Success") {
                          swal('Flashnews Saved Successfully', 'success');
                          $state.reload();
                      }
                      else {
                          swal('Flashnews Not Saved', 'Failed');
                          $state.reload();
                          }
                  })
        }
    };

    $scope.cleardata = function () {
        $window.location.href = 'http://' + HostName + '/#/app/homepage';
    }


    $scope.interacted = function (field) {
        return $scope.submitted || field.$dirty;
    };
}
})();