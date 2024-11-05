(function () {
    'use strict';
    angular
        .module('app')
        .controller('COEController', COEController)

    COEController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window']
    function COEController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {

        $scope.coe_grid = false;

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;

            apiService.getDATA("COE/getloaddata").
                then(function (promise) {
                    $scope.coeyearlst = promise.coeyearlist;
                    $scope.currentyar = promise.currentyear;
                    for (var t = 0; t < $scope.coeyearlst.length; t++) {
                        if ($scope.coeyearlst[t].asmaY_Id === $scope.currentyar[0].asmaY_Id) {
                            $scope.asmaY_Id = $scope.currentyar[t].asmaY_Id;
                        }
                    }
                    $scope.defVal = ("0" + (new Date().getMonth() + 1)).slice(-2);
                    $scope.month = $scope.defVal.toString();
                    $scope.onmonthchange();
                });
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   
            $scope.reverse = !$scope.reverse; 
        };
        //-----------academicyear Selection
        $scope.onmonthchange = function (asmaY_Id) {
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "month": $scope.month
            };
            apiService.create("COE/getcoedata", data).
                then(function (promise) {
                    $scope.coereportlst = promise.coereportlist;
                    if ($scope.coereportlst.length === "0") {
                        swal("No Record found....");
                        $scope.coe_grid = false;
                    }
                    else {
                        $scope.coe_grid = true;
                    }
                   // $scope.asmaY_Id = promise.coereportlist[0].asmaY_Id;
                });
        };
    }
})();