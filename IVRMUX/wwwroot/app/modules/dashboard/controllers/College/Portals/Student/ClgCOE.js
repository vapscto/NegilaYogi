(function () {
    'use strict';
    angular
        .module('app')
        .controller('ClgCOEController', ClgCOEController)

    ClgCOEController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window']
    function ClgCOEController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {

        $scope.coe_grid = false;

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;

            apiService.getDATA("ClgCOE/getloaddata").
                then(function (promise) {
                    $scope.yearlist = promise.yearlist;
                    $scope.currentyear = promise.currentyear;
                    $scope.calenderlist = promise.calenderlist;
                    angular.forEach($scope.yearlist, function (y) {
                        angular.forEach($scope.currentyear, function (c) {
                            if (y.asmaY_Id == c.asmaY_Id) {
                                $scope.asmaY_Year = c.asmaY_Year;
                            }
                        })
                    })

                    $scope.defVal = ("0" + (new Date().getMonth() + 1)).slice(-2);
                    $scope.month = $scope.defVal.toString();

                    $scope.onmonthchange();
                })
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;
            $scope.reverse = !$scope.reverse;
        }
        //============================ Academic Year Selection
        $scope.onmonthchange = function (asmaY_Id) {
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "month": $scope.month
            }
            apiService.create("ClgCOE/getcoedata", data).
                then(function (promise) {                    
                    $scope.coereportlst = promise.coereportlist;
                    if ($scope.coereportlst.length == "0") {
                        swal("No Record found....!!")
                        $scope.coe_grid = false;
                    }
                    else {
                        $scope.coe_grid = true;
                    }
                    $scope.asmaY_Id = promise.coereportlist[0].asmaY_Id;
                })
        }
    };
})();