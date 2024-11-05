
(function () {
    'use strict';
    angular
.module('app')
        .controller('AccessDeniedController', AccessDeniedController)

    AccessDeniedController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function AccessDeniedController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
    };
})();


