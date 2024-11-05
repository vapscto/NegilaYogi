
(function () {
    'use strict';
    angular
        .module('app')
        .controller('AdmissionReportController', AdmissionReportController);
    AdmissionReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', 'superCache', '$timeout', '$filter'];
    function AdmissionReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, superCache, $timeout, $filter) {

        $scope.obj = {};
        $scope.IsHiddendown = true;
        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.mI_Name).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        }

        $scope.asmaY_Date = new Date();

        $scope.fromDate = new Date();
        $scope.toDate = new Date();
        $scope.asmaY_FromDate = new Date();
        $scope.maxDatefromt = new Date();

        $scope.asmaY_ToDate = new Date();
        $scope.maxDatetot = new Date();

        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };

        //======================
        $scope.togchkbx = function () {
            $scope.usercheck = $scope.institutlist.every(function (options) {
                return options.miid;
            });
        }
        $scope.all_check = function () {

            var toggleStatus = $scope.usercheck;
            angular.forEach($scope.institutlist, function (itm) {
                itm.miid = toggleStatus;
            });
        }
        $scope.isOptionsRequired = function () {
            return !$scope.institutlist.some(function (options) {
                return options.miid;
            });

        }

        
        //========load data
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            var pageid = 2;
            $scope.all_check();

            apiService.getURI("AdmissionReport/getloaddata", pageid).then(function (promise) {
                $scope.institutlist = promise.institutlistnew;

            });
        };

        //==========report
        $scope.submitted = false;
        $scope.onreport = function () {
            $scope.submitted = true;
            
            if ($scope.myForm.$valid) {
                var fromDate = $scope.asmaY_FromDate == null ? "" : $filter('date')($scope.asmaY_FromDate, "yyyy-MM-dd");
                var toDate = $scope.asmaY_ToDate == null ? "" : $filter('date')($scope.asmaY_ToDate, "yyyy-MM-dd");
                $scope.institutlists = [];                angular.forEach($scope.institutlist, function (aca) {                    if (aca.miid === true) {                        $scope.institutlists.push({ mI_Id: aca.mI_Id });                    }                });
                var data = {
                    //"MI_Id": $scope.mI_Id,
                    "ActiveCount": $scope.Active,
                    "LeftCount": $scope.TCtaken,
                    "DeactiveCount": $scope.Deactive,
                    "Fromdatee":fromDate,
                    "ToDate": toDate,
                    "institutlist": $scope.institutlists
                }
                apiService.create("AdmissionReport/onreport", data).
                    then(function (promise) {
                        $scope.get_Report = promise.get_Report;
                        $scope.count = $scope.get_Report.length;
                    });
            }
            else {
                $scope.submitted = true;
            }


        }

        $scope.ShowHidedown = function () {
            $scope.IsHiddendown = $scope.IsHiddendown ? false : true;
        }
        $scope.onrdochange = function (artype) {
            $scope.institutlist = "";
        }
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        //============cancel
        $scope.cancel = function () {
            $scope.usercheck = false;
            $scope.all_check();
            $state.reload();
        };


        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';

        $scope.searchString = "";
    }
})();