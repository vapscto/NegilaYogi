

var login = angular.module('login', ['ui.router', 'ngResource', 'ngAnimate']);


login.config(["$stateProvider", function ($stateProvider) {

    //login page state
    $stateProvider.state('login', {
        url: '/login/:virtualId',
        templateUrl: 'app/modules/login/index.html',
        controller: 'loginCtrl',
        controllerAs: 'vm',
        data: {
            pageTitle: 'Login'
        }
    });

    //login page state
    $stateProvider.state('adminlogin', {
        url: '/adminlogin',
        templateUrl: 'app/modules/login/adminLogin.html',
        controller: 'loginCtrl',
        controllerAs: 'vm',
        data: {
            pageTitle: 'Login'
        }
    });

    $stateProvider.state('lock', {
        url: '/lock',
        templateUrl: 'app/modules/login/lock.html',
        controller: 'loginCtrl',
        controllerAs: 'vm',
        data: {
            pageTitle: 'lock'
        }
    });

}]);

