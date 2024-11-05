(function () {
    'use strict';
    angular
.module('app')
.controller('Stu_FeedbackController', Stu_FeedbackController)

    Stu_FeedbackController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window']
    function Stu_FeedbackController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {

  

     
        $scope.hide_examg = false;
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;
            
            apiService.getDATA("Stu_Feedback/getloaddata").
                then(function (promise) {
                    
                    $scope.instname = promise.instname;

                    $scope.institutename = $scope.instname[0].mI_Name;
                    $scope.studetiallist = promise.studetiallist;
                    $scope.name = $scope.studetiallist[0].amsT_FirstName;
                    $scope.phone = $scope.studetiallist[0].amsT_MobileNo;
                    $scope.email = $scope.studetiallist[0].amsT_emailId;

                    
                   
                })
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        //-----------savecomment 

        $scope.cancel = function () {
            $scope.comment = '';
            $scope.submitted = false;
        }

        $scope.savecomment = function () {

            if ($scope.myForm.$valid) {
           
            
            var data = {
                "adm_name": $scope.name,
                "adm_emailid": $scope.email,
                "adm_contactno": $scope.phone,
                "adm_comment": $scope.comment,
            }
            apiService.create("Stu_Feedback/savecomment", data).
               then(function (promise) {
                   
                   if (promise.submsg != null) {
                      
                       if (promise.submsg==true) {
                           swal("Comment Submitted Successfully")
                           $scope.cancel();
                       }

                       if (promise.submsg == false) {
                           swal("Error")
                       }
                   }
                   else {
                       swal("Error")
                   }

               })
            }
            else {
                $scope.submitted = true;
            }
        }
     
       
    };
})();