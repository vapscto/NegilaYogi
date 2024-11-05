
(function () {
    'use strict';
    angular
.module('app')
        .controller('EsatregreportController', EsatregreportController)

    EsatregreportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', '$compile', '$sce']
    function EsatregreportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, $compile, $sce) {




        $scope.onLoadGetData = function () {
            var data = 30;
            apiService.getURI("MasterTemplate/getSaletypes", data).
                then(function (promise) {
                    $scope.get_Saletypes = promise.get_Saletypes;
                });
        };
    }
})();















