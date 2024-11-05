(function () {
    'use strict';
    angular
.module('app')
.controller('Ch_feedbackController', Ch_feedbackController)

    Ch_feedbackController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache']
    function Ch_feedbackController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache) {

        $scope.feedbacklist = [];
        $scope.searchValue = "";
        $scope.submitted = false;
        $scope.LoadData = function () {
            $scope.feedbacklist = [];
            $scope.showweekly = false;
            $scope.showday_d = true;
            apiService.getDATA("Ch_feedback/getalldetails")
                .then(function (promise) {
                    
                    $scope.academicyearlst = promise.academicyearlst;
                    $scope.asmaY_Id = promise.asmaY_Id;


                    $scope.feedbacklist = promise.feedbackdetails;
                

                    if ($scope.feedbacklist != null && $scope.feedbacklist.length > 0) {
                        $scope.salaryD = true;
                        angular.forEach($scope.feedbacklist, function (st) {
                            st.asgfE_FeedbackDate = $filter('date')(new Date(st.asgfE_FeedbackDate), 'MM/dd/yyyy');


                        })

                    } else {
                        $scope.salaryD = false;


                        swal('No Data')
                    }

                })


            $scope.OnAcdyear = function (id) {


                apiService.getURI("Ch_feedback/OnAcdyear",id)
                    .then(function (promise) {

                        $scope.academicyearlst = promise.academicyearlst;
                        $scope.asmaY_Id = promise.asmaY_Id;


                        $scope.feedbacklist = promise.feedbackdetails;


                        if ($scope.feedbacklist != null && $scope.feedbacklist.length > 0) {
                            $scope.salaryD = true;
                            angular.forEach($scope.feedbacklist, function (st) {
                                st.asgfE_FeedbackDate = $filter('date')(new Date(st.asgfE_FeedbackDate), 'MM/dd/yyyy');


                            })

                        } else {
                            $scope.salaryD = false;


                            swal('No Data')
                        }

                    })
            }
          
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }


          

          
            $scope.interacted = function (field) {
                return $scope.submitted;
            };

       

           
        };

        

    };
})();