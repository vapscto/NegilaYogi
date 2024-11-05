

//(function () {
//    'use strict';
//    angular
//.module('app')
//.controller('FeeCategoryWiseReportController', FeeCategorywiseController123)

//    FeeCategorywiseController123.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http','superCache']
//    function FeeCategorywiseController123($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
       
//        $scope.loaddata = function () {
//            $scope.report = false;
//        }
//        $scope.interacted = function (field) {
//            return $scope.submitted || field.$dirty;
//        };
//        $scope.submitted = false;
//        $scope.ShowReportdata = function () {
//            if ($scope.myForm.$valid) {
//                var data = {
//                    "frmdate": $scope.frmdateM,
//                    "todate": $scope.todaetM,
//                }
//                apiService.create("FeeCategoryWiseReport/getreport", data).
//                    then(function (promise) {
//                        if (promise.reportdatelist != null && promise.reportdatelist != "") {
//                            $scope.students = promise.reportdatelist;
//                            $scope.report = true;
//                        }
//                    })
//            }
//            else {
//                swal("No Records Found");
//                $scope.submitted = true;
//            }
//        }

//        $scope.Clear_Details = function () {
//            $state.reload();
//        }
//    }
//})();






(function () {
    'use strict';
    angular
.module('app')
.controller('FeeCategoryWiseReportController', FeeCategoryWiseReportController)

    FeeCategoryWiseReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter']
    function FeeCategoryWiseReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter) {

        $scope.loaddata = function () {
            
            $scope.report = false;
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;
            var pageid = 2;
            
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clear_Details = function () {
            $state.reload();
            $scope.loaddata();
        }




        $scope.ShowReportdata = function () {


            if ($scope.myForm.$valid) {
                $scope.report = false;
                $scope.from_date = new Date($scope.FMCB_fromDATE).toDateString();
                $scope.to_date = new Date($scope.FMCB_toDATE).toDateString();


                var data = {
                    "frmdate": $scope.from_date,
                    "todate": $scope.to_date,
                }
                apiService.create("FeeCategoryWiseReport/getreport", data).
        then(function (promise) {
            
            //$scope.acayyearbind = promise.acayear;
            if (promise.reportdatelist.length > 0 && promise.reportdatelist != null) {

            }
            else {
                swal("No Records Found");
            }

        })
                

               

            }
            else {
                //swal("No Records Found");
                $scope.submitted = true;
            }
        }
    }
})();














