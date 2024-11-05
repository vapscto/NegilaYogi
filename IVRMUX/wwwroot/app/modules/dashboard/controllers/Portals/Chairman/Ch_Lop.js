(function () {
    'use strict';
    angular
.module('app')
.controller('Ch_LopController', Ch_LopController)

    Ch_LopController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache']
    function Ch_LopController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache) {

        $scope.salary_list = [];
        $scope.yearL = [];
        var sal_list = [];
        $scope.searchValue = "";
        $scope.submitted = false;
        $scope.LoadData = function () {
         
            $scope.showweekly = false;
            $scope.showday_d = true;
            apiService.getDATA("Ch_Lop/getalldetails")
                .then(function (promise) {
                    
                    $scope.yearL = promise.yearlist;
                 //   $scope.hrmlY_LeaveYear = promise.hrmlY_LeaveYear;
                    $scope.hrmlY_Id = promise.hrmlY_Id
                    $scope.salary_list = promise.salarylist;
                    $scope.fillmonths = promise.fillmonths;
                    $scope.ivrM_Month_Name = promise.monthName;

                    if ($scope.salary_list != null && $scope.salary_list != 0) {
                        $scope.salaryD = true;
                        angular.forEach($scope.salary_list, function (st) {
                            st.frmdate = $filter('date')(new Date(st.frmdate), 'MM/dd/yyyy');
                            st.todate = $filter('date')(new Date(st.todate), 'MM/dd/yyyy');

                        })

                    } else {
                        $scope.salaryD = false;


                        swal('No LOP details ')
                    }

                })

          
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }


            $scope.onselectgroup = function (hrmlY_Id) {
                
                $scope.submitted = true;
                if ($scope.myForm.$valid) {
                var data = {
                    "HRMLY_Id": hrmlY_Id,
                    "monthName": $scope.ivrM_Month_Name
                }
               
               
                apiService.create("Ch_Lop/onyearchange", data).
           then(function (promise) {
               $scope.hrmlY_Id = promise.hrmlY_Id
               $scope.salary_list = promise.salarylist;
               $scope.fillmonths = promise.fillmonths;
               $scope.ivrM_Month_Name = promise.monthName;

               if ($scope.salary_list != null && $scope.salary_list != 0) {
                   $scope.salaryD = true;

                   angular.forEach($scope.salary_list, function (st) {
                       st.frmdate = $filter('date')(new Date(st.frmdate), 'MM/dd/yyyy');
                       st.todate = $filter('date')(new Date(st.todate), 'MM/dd/yyyy');

                   })


               } else {
                   $scope.salaryD = false;

                   swal('No LOP details ')
               }
           

           })
            }
        else {
                    $scope.submitted = true;
        }
            };

          
            $scope.interacted = function (field) {
                return $scope.submitted;
            };

            $scope.onmonth = function (ivrM_Month_Name) {
                $scope.submitted = true;
                if ($scope.myForm.$valid) {
                    var data = {
                        "HRMLY_Id": $scope.hrmlY_Id,
                        "monthName": ivrM_Month_Name
                    }


                    apiService.create("Ch_Lop/onmonth/", data)
                   .then(function (promise) {

                       $scope.hrmlY_Id = promise.hrmlY_Id
                       $scope.salary_list = promise.salarylist;
                       $scope.fillmonths = promise.fillmonths;
                       $scope.ivrM_Month_Name = promise.monthName;

                       if ($scope.salary_list != null && $scope.salary_list != 0) {
                           $scope.salaryD = true;
                           angular.forEach($scope.salary_list, function (st) {
                               st.frmdate = $filter('date')(new Date(st.frmdate), 'MM/dd/yyyy');
                               st.todate = $filter('date')(new Date(st.todate), 'MM/dd/yyyy');

                           })

                       } else {
                           $scope.salaryD = false;

                           swal('No LOP details ')
                       }



                   })
                }
                else {
                    $scope.submitted = true;
                }
            }

           
        };

        

    };
})();