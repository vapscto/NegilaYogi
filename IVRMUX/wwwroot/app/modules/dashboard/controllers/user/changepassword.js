
(function () {
    'use strict';
    angular
.module('app')
.controller('changepswdController', ChangeController)
    ChangeController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$window', 'FormSubmitter'];
    function ChangeController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $window, FormSubmitter) {

        var HostName = location.host;

    $scope.submitted = false;
    $scope.SubmitForm = function () {
        $scope.submitted = true;
        if ($scope.myForm.$valid) {

            if ($scope.newpwd != $scope.cnfpwd) {
                swal('Confirm Password is Not Match with New Password');
                return;
            }

            var data = {
                "password": $scope.curpwd,
                "new_password": $scope.newpwd,
               
            }
            apiService.create("Changepswd/", data).
                  then(function (promise) {
                      if (promise.returnMsg === "Success") {
                          swal('Password changed Successfully', 'success');
                          $state.reload();
                      }
                      else if (promise.returnMsg === "fail") {
                          swal('Kindly enter valid Current password', 'Password is incorrect');
                          return;
                      }
                      else if (promise.returnMsg != "") {
                          swal(promise.returnMsg);
                          return;
                      }
                      else if (promise.returnMsg == "Error") {
                          swal('Password Not Changed', 'Failed');
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