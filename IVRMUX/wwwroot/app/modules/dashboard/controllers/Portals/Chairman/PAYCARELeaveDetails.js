
   
     (function () {
         'use strict';
         angular
     .module('app')
     .controller('PAYCARELeaveDetailsController', PAYCARELeaveDetailsController)

         PAYCARELeaveDetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window', 'appSettings']
         function PAYCARELeaveDetailsController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window, appSettings) {

             var paginationformasters;
             var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
             if (ivrmcofigsettings.length > 0) {
                 paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
             }
             $scope.masterlist = false;
             $scope.currentPage = 1;
             $scope.itemsPerPage = paginationformasters;
             $scope.searchValue = "";
             if ($scope.itemsPerPage == undefined)
             {
                 $scope.itemsPerPage =15
             }
             
             $scope.leavedetails1 =false;
             $scope.loadbasicdata = function () {
                
                 apiService.getDATA("PAYCARELeaveDetails/Getdetails").
      then(function (promise) {
          
         
         
          $scope.departmentdropdown = promise.department;
         
          
        //  $scope.hrmD_Id = promise.hrmd_id;
          $scope.fillyear = promise.fillyear;
          $scope.filldesiganation = promise.filldesiganation;
          
        
          
         

      })

    }

  
             $scope.Ondepartment = function (hrmD_Id) {
                 
                  $scope.hrmdeS_Id = "";
                 var data = {
                     "hrmd_id": hrmD_Id,

                 }
                 apiService.create("PAYCARELeaveDetails/Getdesignation", data).
               then(function (promise) {

                   

                   $scope.designation = promise.designation;





               })


             }
             $scope.interacted = function (field) {
                 return $scope.submitted;
             };

             $scope.showreport = function () {
                 $scope.leavedetails = [];
                 $scope.leavedetails1 = false;
                 $scope.submitted = true;
                 if ($scope.myForm.$valid) {
                     var data = {
                         "hrmd_id": $scope.hrmD_Id,
                         "HRMDES_Id": $scope.hrmdeS_Id,
                         "ASMAY_Id": $scope.hrmlY_Id,
                     }
                     var config = {
                         headers: {
                             'Content-Type': 'application/json;'
                         }
                     }
                     apiService.create("PAYCARELeaveDetails/showreport", data).
                   then(function (promise) {

                       
                       $scope.masterleave = promise.masterleave;
                       $scope.leavedetails = promise.leavedetails;
                       if ($scope.leavedetails.length > 0) {
                           $scope.leavedetails1 = true;
                           $scope.lt = [];
                           for (var i = 0; i < $scope.masterleave.length; i++) {
                               $scope.lt.push({
                                   label: 'TL'
                               })
                               $scope.lt.push({
                                   label: 'LT'
                               })
                               $scope.lt.push({
                                   label: 'BL'
                               })
                           }
                          // console.log($scope.lt);
                       }
                       else {
                           swal("NO Record found");
                       }

                   })
                 }
                 else {
                     $scope.submitted = true;
                 }
             }



             $scope.sort = function (keyname) {
                 $scope.sortKey = keyname;   //set the sortKey to the param passed
                 $scope.reverse = !$scope.reverse; //if true make it false and vice versa
             }
    $scope.showempGrid = function (hrmdeS_Id) {
        $scope.emppop = [];
        var data = {
            "hrmd_id": $scope.hrmD_Id,
            "HRMDES_Id": hrmdeS_Id,

        }
        var config = {
            headers: {
                'Content-Type': 'application/json;'
            }
        }
        apiService.create("PAYCARELeaveDetails/Getemppop", data).
      then(function (promise) {

          

          $scope.emppop = promise.employeeDetails;





      })
    
}

    
   
   

         };
     })();