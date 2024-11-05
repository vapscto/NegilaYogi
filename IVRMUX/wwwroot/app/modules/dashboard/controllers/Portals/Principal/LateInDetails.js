
     (function () {
         'use strict';
         angular
     .module('app')
     .controller('LateInDetailsController', LateInDetailsController)

         LateInDetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window']
         function LateInDetailsController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {                                  
             $scope.Todaydate = new Date();
             $scope.Binddata = function () {
                 // $scope.fields();
                 $scope.fromdate = new Date();
                 var data = {
                     "fromdate": $scope.fromdate,

                 }
                 var config = {
                     headers: {
                         'Content-Type': 'application/json;'
                     }
                 }

                 
                 apiService.create("LateInDetails/getalldetails", data).then(function (promise) {
                     

                     $scope.filldepartment = promise.filldepartment;

                 })

             }


             $scope.ondatechange = function (fromdate) {

                 var data = {
                     "fromdate": fromdate,

                 }
                 var config = {
                     headers: {
                         'Content-Type': 'application/json;'
                     }
                 }

                 
                 apiService.create("LateInDetails/ondatechange", data).then(function (promise) {
                     
                     var lateT = [];
                     var latetime;
                     $scope.filldepartment = promise.filldepartment;
                     //for (var i = 0; i < $scope.filldepartment.length; i++)
                     //{
                     //    if ($scope.filldepartment[i].foepD_PunchTime > $scope.filldepartment[i].foesT_IHalfLoginTime) {
                     //       // latetime = $scope.filldepartment[i].foepD_PunchTime - $scope.filldepartment[i].foesT_IHalfLoginTime

                     //    } else {
                     //        latetime = '--:--';
                     //    }
                     //    lateT.push(latetime)
                     //}
                    
                 })
             }
    }
           
    


     })();