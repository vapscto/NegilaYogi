
(function () {
    'use strict';
    angular
.module('app')
        .controller('EsatregreportController', EsatregreportController)

    EsatregreportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', '$compile', '$sce']
    function EsatregreportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, $compile, $sce) {



        $scope.searchValueI = "";        var paginationformasters;        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));        if (ivrmcofigsettings.length > 0) {            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;        }        else {            paginationformasters = 10;        }        $scope.usrname = localStorage.getItem('username');        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));        if (admfigsettings.length > 0) {            var logopath = admfigsettings[0].asC_Logo_Path;        }        $scope.imgname = logopath;        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));        //-------------------------------------------------------------------     
        $scope.onLoadGetData = function () {
            var data = 30;
            apiService.getURI("MasterTemplate/getSaletypes", data).
                then(function (promise) {
                    $scope.get_Saletypes = promise.get_Saletypes;
                });
        };
    }
})();
















