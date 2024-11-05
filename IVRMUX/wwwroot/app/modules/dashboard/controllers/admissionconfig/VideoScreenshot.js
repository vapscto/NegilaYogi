(function () {
    'use strict';
    angular.module('app').controller('VideoScreenshotController', VideoScreenshotController)

    VideoScreenshotController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function VideoScreenshotController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
        debugger;
        var video = document.querySelector('video');
        var canvas = document.querySelector('canvas');
        var context = canvas.getContext('2d');
        var w, h, ratio;

        //add loadedmetadata which will helps to identify video attributes

        video.addEventListener('loadedmetadata', function () {
            ratio = video.videoWidth / video.videoHeight;
            w = video.videoWidth - 100;
            h = parseInt(w / ratio, 10);
            canvas.width = w;
            canvas.height = h;
        }, false);

        //function snap() {
        //    debugger;
        //    context.fillRect(0, 0, w, h);
        //    context.drawImage(video, 0, 0, w, h);
        //    var img_data = canvas.toDataURL('image/jpg');

        //}

        $scope.snap = function () {

            debugger;
            context.fillRect(0, 0, w, h);
            context.drawImage(video, 0, 0, w, h);
           

        }
        
    }

})();