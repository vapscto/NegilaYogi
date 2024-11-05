
(function () {
    'use strict';
    angular
.module('app')
.controller('StaffReplacementUnalocatedPeriodController', StaffReplacementUnalocatedPeriodController)

    StaffReplacementUnalocatedPeriodController = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', 'superCache']
    function StaffReplacementUnalocatedPeriodController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, superCache) {

        $scope.gridOptions = {};
        $scope.datareport = false;
        $scope.submitted = false;
        $scope.GetReport = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var data = {
                    "HRME_Id": $scope.hrmE_Id
                }
                apiService.create("StaffReplacementUnalocatedPeriod/getrpt", data).
                    then(function (promise) {
                        if (promise.datalst.length > 0)
                        {
                            $scope.datareport = true;
                            $scope.gridOptions.columnDefs.push({
                                name: 'Days',
                                displayName: "Days",
                                enableSorting: false,
                                enableFiltering: false,
                            });

                            for (var i = 0; i < promise.periodslst.length; i++) {
                                $scope.gridOptions.columnDefs.push({
                                    name: promise.periodslst[i].ttmP_PeriodName,
                                    displayName: promise.periodslst[i].ttmP_PeriodName + " " + "Period",
                                    enableSorting: false,
                                    enableFiltering: false,
                                });
                            }
                            $scope.gridOptions.data = promise.datalst
                        }
                        else
                        {
                            swal("Their Is No Records for Selected staff !")
                        }

                    })
            }
        };



        //TO  GEt The Values iN Grid
        $scope.BindData = function () {
            apiService.getDATA("StaffReplacementUnalocatedPeriod/getalldetails").
       then(function (promise) {

           $scope.clearid();
           $scope.academic = promise.academiclist;
           $scope.categorylst = promise.catelist;
           $scope.stafflst = promise.staffDrpDwn;               
       })
        };
        //TO clear  data
        $scope.clearid = function () {

            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
        };

        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };

        $scope.get_category = function () {

            if ($scope.asmaY_Id === "") {
                swal("Please Select the Academic Year !");
            }
            else {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                }
                apiService.create("StaffReplacementUnalocatedPeriod/get_catg", data).
         then(function (promise) {

             $scope.categorylst = promise.catelist;
             if (promise.catelist === "" || promise.catelist === null) {
                 swal("No Category Are Mapped To Selected Academic Year");
             }
         })
            }
        };


    }
})();