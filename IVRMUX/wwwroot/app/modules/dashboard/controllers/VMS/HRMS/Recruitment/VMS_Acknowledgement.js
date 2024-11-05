(function () {
    'use strict';
    angular.module('app').controller('VMS_Acknowledgement', VMS_Acknowledgement)

    VMS_Acknowledgement.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$q']
    function VMS_Acknowledgement($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q) {






        //dynamic table for Document

        $scope.documentList = [{ id: 'document' }];
        $scope.addNewDocument = function () {
            var newItemNo = $scope.documentList.length + 1;

            if (newItemNo <= 50) {
                $scope.documentList.push({ 'id': 'document' + newItemNo });
            }
        };

        $scope.removeNewDocument = function (index, data) {
            var newItemNo = $scope.documentList.length - 1;
            $scope.documentList.splice(index, 1);
            if (data.hrmedS_Id > 0) {
                $scope.DeleteDocumentData(data);
            }


            //if ($scope.documentList.length === 0) {
            //}


        };


        

        $scope.loaddata = function () {
            
            var id = 2;
            apiService.getURI("VMS_Acknowledgement/loaddata", id).then(function (promise) {
                debugger;
                $scope.yearDropdown = promise.yearlist;
               
            })
        }


    }
})();