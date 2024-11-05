(function () {
    'use strict';
    angular
.module('app')
.controller('TTExistingToNewController', TTExistingToNewController)

    TTExistingToNewController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function TTExistingToNewController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {

        $scope.BindData = function () {
            apiService.getDATA("TTExistingToNew/getdetails").
            then(function (promise) {
                $scope.acdlist = promise.acdlist;
                $scope.acdlist1 = promise.acdlist;
            })
        };

        $scope.onselectAcdYear = function (ASMAY_Id) {
            apiService.getDATA("TTExistingToNew/getdetails").
            then(function (promise) {
                $scope.arraylist = [];
                for (var j = 0; j < promise.acdlist.length; j++) {
                    if (ASMAY_Id != promise.acdlist[j].asmaY_Id) {
                        $scope.arraylist.push(promise.acdlist[j]);
                    }
                }
                $scope.acdlist1 = $scope.arraylist;
            })
        };
    }

})();