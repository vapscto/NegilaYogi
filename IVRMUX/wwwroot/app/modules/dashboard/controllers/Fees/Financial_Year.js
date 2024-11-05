
(function () {
    'use strict';
    angular
        .module('app')
        .controller('Financial_YearController', Financial_YearController)

    Financial_YearController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$q', '$filter', '$sce'];
    function Financial_YearController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $q, $filter, $sce) {

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;

        $scope.page1 = "page1";
        $scope.search = " ";

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.submitted = false;
        $scope.save = function () {


            if ($scope.myForm.$valid) {
                var data = {
                    "IMFY_Id": $scope.IMFY_Id,
                    "IMFY_FromDate": $scope.IMFY_FromDate,
                    "IMFY_ToDate": $scope.IMFY_ToDate,
                    "asmaY_Id": $scope.IMFY_FinancialYear,
                    "year2": $scope.IMFY_AssessmentYear
                  
                   

                };
                apiService.create("Financial_Year/save", data).then(function (promise) {

                    if (promise.msg === 'Saved') {
                        swal("Data Saved Successfully.....!!!!!");
                        $state.reload();
                    }
                    else if (promise.msg === 'Failed') {
                        swal("Data Not Saved Successfully.....!!!!!");
                        $state.reload();
                    }
                    else if (promise.msg === 'updated') {
                        swal("Data Updated.....!!!!!");
                        $state.reload();
                    }
                    else if (promise.msg === 'failed') {
                        swal("Data Not Updated Successfully.....!!!!!");
                        $state.reload();
                    }
                    else if (promise.duplicate === true) {
                        swal("Data already Exists.....!!!!!");
                    }
                    else {
                        swal("Something is Wrong...");
                    }
                });

            }
            else {
                $scope.submitted = true;
            }

        };
        $scope.search = '';
        $scope.filtervalue1 = function (user) {
           
        };
        $scope.loaddata = function () {
            
            var data = {
                // pageid = 2
            };
            apiService.create("Financial_Year/loaddata", data).then(function (promise) {
              
                $scope.alldata1 = promise.alldata1;
                $scope.allacademicyear = promise.yeardata;
               
            });
        };
       

        $scope.Clearid = function () {
            $state.reload();
        };
    }
})();