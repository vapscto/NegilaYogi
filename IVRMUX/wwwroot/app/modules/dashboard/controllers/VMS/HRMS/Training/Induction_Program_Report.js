(function () {
    'use strict';
    angular
        .module('app')
        .controller('Induction_Program_ReportController', induction_Program_ReportController)

    induction_Program_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter', 'Excel', '$timeout']
    function induction_Program_ReportController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, Excel, $timeout) {

        $scope.submitted = false;
        $scope.imgname = ""; //logopath
       // $scope.itemsPerPage = $scope.paginationformasters;
        $scope.currentPage = 1;

        $scope.ShowReport = function () {
            $scope.printstudents = [];
            $scope.searchValue = "";
            if ($scope.All == true) {
                $scope.allCheck1 = 4;
            }
            else {
                $scope.allCheck1 = 7;
            }

            if ($scope.Open == true) {
                $scope.openCheck1 = 1;
            }
            else {
                $scope.openCheck1 = 8;
            }
            if ($scope.Running == true) {
                $scope.runningCheck1 = 2;
            }
            else {
                $scope.runningCheck1 = 9;
            }
            if ($scope.Complete == true) {
                $scope.completeCheck1 = 3;
            }
            else {
                $scope.completeCheck1 = 10;
            }
            if ($scope.StartDate == null || $scope.StartDate == '') {
                $scope.StartDate1 = $filter('date')(new Date(), "yyyy-MM-dd");
            }
            else {
                $scope.StartDate1 = $filter('date')($scope.StartDate, "yyyy-MM-dd");
            }
            if ($scope.EndDate == null || $scope.EndDate == '') {
                $scope.EndDate1 = $filter('date')(new Date(), "yyyy-MM-dd");
            }
            else {
                $scope.EndDate1 = $filter('date')($scope.EndDate, "yyyy-MM-dd");
            }
            //$scope.StartDate1 = $filter('date')($scope.StartDate, "yyyy-MM-dd");
            //$scope.EndDate1 = $filter('date')($scope.EndDate, "yyyy-MM-dd");

            if ($scope.myForm.$valid) {
                var data = {
                    "START_DATE": $scope.StartDate1,
                    "END_DATE": $scope.EndDate1,
                    "ALL": $scope.allCheck1,
                    "OPEN": $scope.openCheck1,
                    "RUNNING": $scope.runningCheck1,
                    "COMPLETE": $scope.completeCheck1
                };

                apiService.create("Induction_Training/GetInductionReport", data).
                    then(function (promise) {
                        if (promise.induction_training_report.length > 0) {
                            $scope.screport = true;
                            $scope.searchResult = promise.induction_training_report;
                            $scope.presentCountgrid = $scope.searchResult.length;
                        }
                        else {
                            $scope.searchResult = {};
                            swal("Records Not Found");
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.cleardata = function () {
            $state.reload();
        };

        $scope.eve = {};
        $scope.printviews = function (ev) {
            $scope.evetrainer_list = [];
            $scope.eve = ev.HRTCR_Id;
            var pageid = $scope.eve;
            apiService.getURI("Induction_Training/print_trainer_list", pageid).then(function (promise) {
                $scope.printdatatable = promise.print_trainer_list;
                $scope.printdatatable1 = promise.print_trainer_list;
            });
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        //// =================== Print option
        $scope.empid = {};
        ///$scope.print = [];

        $scope.print = function () {
            if ($scope.printdatatable1 != null && $scope.printdatatable1.length > 0) {
                var innerContents = document.getElementById('printSectionId').innerHTML;
                var popupWinindow = window.open('');
                popupWinindow.document.open();
                popupWinindow.document.write('<html><head>' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet"  href="css/print/SRKVSStudentAddressBook/SRKVSStudentAddressBookPdf.css" />' +
                    '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                    '</head><body onload="window.print()" onfocus="window.setTimeout(function() { window.close(); },100);">' + innerContents + '</html>');
                popupWinindow.document.close();
            }
            else {
                swal("No Data Found");
            }
        };           
    }
})();