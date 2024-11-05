

var app = angular.module('app', ['ui.router', 'ui.calendar', 'ui.bootstrap', 'flash',
    //main modules
    'login', 'dashboard', 'ngCookies', 'angular-hmac-sha512']);

app.config(['$crypthmacProvider', function ($crypthmacProvider) {
    $crypthmacProvider.setCryptoSecret('jfoiwjfwoifje83');
}]);

app.config(['$stateProvider', '$locationProvider', '$urlRouterProvider', function ($stateProvider, $locationProvider, $urlRouterProvider, $modalInstance) {

    //IdleScreenList
    $stateProvider
       .state('app', {
           url: '/app',
           templateUrl: 'app/common/app.html',
           controller: 'appCtrl',
           controllerAs: 'vm',
           data: {
               pageTitle: 'Login'
           }
       });
    $urlRouterProvider.otherwise('login/');
    //$urlRouterProvider.otherwise('app/dashboard');
    //$urlRouterProvider.otherwise('/app/dashboard');
}]);

// set global configuration of application and it can be accessed by injecting appSettings in any modules
app.constant('appSettings', appConfig);