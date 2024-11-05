
(function () {
    'use strict';
    angular
.module('app')
.controller('ConSettingWizardCompleteController', ConSettingWizardCompleteController);

    ConSettingWizardCompleteController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window','superCache'];
    function ConSettingWizardCompleteController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, superCache) {

        $scope.start = function () {
            var HostName = location.host;
            $window.location.href = 'http://' + HostName + '/#/app/homepage';
        };

        var bar = new ProgressBar.Circle(container, {
            color: '#15b76d',
            // This has to be the same size as the maximum width to
            // prevent clipping
            strokeWidth: 4,
            trailWidth: 1,
            easing: 'easeInOut',
            duration: 1400,
            text: {
                autoStyleContainer: false
            },
            from: { color: '#aaa', width: 1 },
            to: { color: '#15b76d', width: 2 },
            // Set default step function for all animate calls
            step: function (state, circle) {
                circle.path.setAttribute('stroke', state.color);
                circle.path.setAttribute('stroke-width', state.width);

                var value = Math.round(circle.value() * 100);
                if (value === 0) {
                    circle.setText('');
                } else {
                    circle.setText(value + '%');
                }

            }
        });
        bar.text.style.fontFamily = '"Raleway", Helvetica, sans-serif';
        bar.text.style.fontSize = '2rem';

        bar.animate(1.0);
      
    }

})();