
(function () {
    'use strict';
    angular
.module('app')
.controller('ConfigurationController', ConfigurationController)

    ConfigurationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', 'superCache']
    function ConfigurationController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, superCache) {
     
        // TO Save The Data
        $scope.submitted = false;
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.TTC_Id !== 0 || $scope.TTC_Id === undefined)
            {
                apiService.getDATA("Configuration/getalldetails").
                   then(function (promise) {
                       $scope.TTC_Id = promise.detailslist[0].ttC_Id;
                })
            }

            if ($scope.myForm.$valid) {

                var data = {
                    "TTC_StaffwiseContiniousPeriods":Number($scope.ttC_StaffwiseContiniousPeriods),
                    "TTC_MaxMinPeriodCheckingFlg": Number($scope.ttC_MaxMinPeriodCheckingFlg),
                    "TTC_CTConstraintFlg": Number($scope.ttC_CTConstraintFlg),
                    "TTC_CTConstraintNoOfDays":Number($scope.ttC_CTConstraintNoOfDays),
                    "TTC_Id": $scope.TTC_Id
                }
                apiService.create("Configuration/savedetail", data).
                    then(function (promise) {


                        if (promise.returnduplicatestatus === "Save") {
                            swal('Record Saved Successfully');
                            cleardata();
                            $scope.BindData();

                        }
                        else if (promise.returnduplicatestatus === "NotSave") {
                            swal('Record Not Saved');
                            cleardata();
                            $scope.BindData();

                        }
                        else if (promise.returnduplicatestatus === "Update") {
                            swal('Record Updated Successfully');
                            $scope.BindData();

                        }
                        else if (promise.returnduplicatestatus === "NotUpdate") {
                            swal('Record Not Updated');
                            $scope.BindData();

                        }

                    })
            }

        };

      

        $scope.BindData = function () {
            apiService.getDATA("Configuration/getalldetails").
       then(function (promise) {
           
           $scope.TTC_Id = promise.detailslist[0].ttC_Id;
           $scope.list1 = promise.detailslist;
           $scope.ttC_CTConstraintFlg = promise.detailslist[0].ttC_CTConstraintFlg;
           if (promise.detailslist[0].ttC_StaffwiseContiniousPeriods === true) {
                $scope.ttC_StaffwiseContiniousPeriods = 1;
           }
           else
           {
               $scope.ttC_StaffwiseContiniousPeriods = 0;
           }

           if (promise.detailslist[0].ttC_MaxMinPeriodCheckingFlg === true) {
               $scope.ttC_MaxMinPeriodCheckingFlg = 1;
           }
           else {
               $scope.ttC_MaxMinPeriodCheckingFlg = 0;
           }
           $scope.ttC_CTConstraintNoOfDays = promise.detailslist[0].ttC_CTConstraintNoOfDays;
       })
     };
    }

})();