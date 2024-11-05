 (function () {
    'use strict';
    angular.module('app').controller('ChatgptController', ChatgptController)
    ChatgptController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$window', 'FormSubmitter'];
    function ChatgptController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $window, FormSubmitter) {

     

        $scope.submitted = false;
        $scope.savemsg = function () {
            $scope.submitted = true;
            //$scope.obj = {};
            if ($scope.myForm.$valid) {
                var data = {
                    "message": $scope.chatgptmessage,
                    //"ChatCompletion": $scope.chatCompletion,
                }
                apiService.create("Chatgpt/chatgpt", data).then(function (promise) {
                    //$scope.chatCompletion = promise.chatCompletion;
                    $scope.chatcompletion = promise.chatCompletion


                  })
            }
        };



        var HostName = location.host;
        $scope.cleardata = function () {
            $window.location.href = 'http://' + HostName + '/#/app/homepage';
        }


        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
    }
})();