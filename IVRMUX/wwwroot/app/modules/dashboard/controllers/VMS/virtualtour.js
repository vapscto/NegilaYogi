(function () {
    'use strict';
    angular
        .module('app')
        .controller('virtualtourController', virtualtourController)

    virtualtourController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function virtualtourController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
        // form Object
        $scope.Bank = {};

        // instantiate script on your container
        $('.valiant').Valiant360();

        // play
        $('.valiant').Valiant360('play');

        // pause
        $('.valiant').Valiant360('pause');

        // load new video file
        $('.valiant').Valiant360('loadVideo', 'https://yanwsh.github.io/videojs-panorama/assets/shark.mp4');

        // Get form Details at onload 
        $scope.onLoadGetData = function () {
            $scope.mediatype = "video";
        }


        //$scope.changeproperty = function () {

        //    if ($scope.mediatype =="video") {
        //        $('#campusvedio').modal('show');
        //    }
        //    else if ($scope.mediatype == "image") {
        //        $('#mymodalviewdetailsfirsttab').modal('show');
        //    }
        //    else {
        //        $('#mymodalviewdetailsfirsttab').modal('show');
        //    }
            
        //}


    }

})();