(function () {
    'use strict';
    angular
        .module('app')
        .controller('ClassWorkDownloadController', ClassWorkDownloadController)

    ClassWorkDownloadController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window']
    function ClassWorkDownloadController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {

        $scope.work_grid = false;

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;

            apiService.getDATA("ClassWorkDownload/getloaddata").
                then(function (promise) {

                    $scope.subjectlist = promise.subjectlist;
                })
        };

        //-----------Subject Selection
        $scope.onsubjchange = function () {

            var data = {
                "ISMS_Id": $scope.ismS_Id,
            }
            apiService.create("ClassWorkDownload/getwork", data).
                then(function (promise) {

                    $scope.worklist = promise.worklist;
                    $scope.work_grid = true;
                    var enddate = "";

                    var curDate = new Date();

                    angular.forEach($scope.worklist, function (wrk) {
                        enddate = new Date(wrk.icW_ToDate);
                        enddate.setDate(enddate.getDate() + 1)
                        if (enddate < curDate) {
                            wrk.finalDate = true;
                        }
                        else {
                            wrk.finalDate = false;
                        }

                    })

                    if ($scope.worklist.length == "0") {

                        swal("No Work found....")
                        $scope.work_grid = false;
                    }

                })
        }

    };
})();