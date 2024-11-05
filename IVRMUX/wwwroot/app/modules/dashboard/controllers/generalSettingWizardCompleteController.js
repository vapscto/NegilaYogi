

//dashboard.controller("loginController", ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash',
//function ($rootScope, $scope, $state, $location, dashboardService, Flash) {
//    $scope.predicate = 'sno';
//    $scope.reverse = false;
//    $scope.currentPage = 1;
//    $scope.order = function (predicate) {
//        $scope.reverse = ($scope.predicate === predicate) ? !$scope.reverse : false;
//        $scope.predicate = predicate;
//    };
//    $scope.students = [
//      { sno: '1', name: 'Kevin', age: 25, gender: 'boy' },
//      { sno: '2', name: 'John', age: 30, gender: 'girl' },
//      { sno: '3', name: 'Laura', age: 28, gender: 'girl' },
//      { sno: '4', name: 'Joy', age: 15, gender: 'girl' },
//      { sno: '5', name: 'Mary', age: 28, gender: 'girl' },
//      { sno: '6', name: 'Peter', age: 95, gender: 'boy' },
//      { sno: '7', name: 'Bob', age: 50, gender: 'boy' },
//      { sno: '8', name: 'Erika', age: 27, gender: 'girl' },
//      { sno: '9', name: 'Patrick', age: 40, gender: 'boy' },
//      { sno: '10', name: 'Tery', age: 60, gender: 'girl' }
//    ];
//    $scope.totalItems = $scope.students.length;
//    $scope.numPerPage = 5;
//    $scope.paginate = function (value) {
//        var begin, end, index;
//        begin = ($scope.currentPage - 1) * $scope.numPerPage;
//        end = begin + $scope.numPerPage;
//        index = $scope.students.indexOf(value);
//        return (begin <= index && index < end);
//    };


//}]);


(function () {
    'use strict';
    angular
.module('app')
.controller('generalSettingWizardCompleteController', generalSettingWizardCompleteController);

    generalSettingWizardCompleteController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window','superCache'];
    function generalSettingWizardCompleteController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, superCache) {

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