(function () {
    'use strict';
    angular.module('app').controller('htmltemplatesController', htmltemplatesController)
    htmltemplatesController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', 'superCache', '$stateParams']
    function htmltemplatesController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, superCache, $stateParams) {
   
        $scope.moduleflg = 1;

     

        $scope.moduleflg = "ADMISSION";

        $scope.clickFunction = function (ppp) {
            $scope.YYYY = "";   
            $scope.YYYY = ppp;
            $('#myModal').modal('show');
        }
        $(document).ready(function () {
            $("img").click(function () {
                var t = $(this).attr("src");
                $(".modal-body").html("<img src='" + t + "' class='modal-img'>");
                $("#myModal").modal();
            });
        });
   




        
        






    }
})();