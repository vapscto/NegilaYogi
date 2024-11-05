(function () {
    'use strict';
    angular
        .module('app')
        .controller('Induction_Program_ReportController', Induction_Program_ReportController)

    Induction_Program_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'apiService', 'Flash', '$http', '$q', '$stateParams', '$filter', 'Excel', '$timeout']
    function Induction_Program_ReportController($rootScope, $scope, $state, $location, apiService, Flash, $http, $q, $stateParams, $filter, Excel, $timeout) {

        $scope.usrname = localStorage.getItem('username');
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_ReportPagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        else {
            paginationformasters = 10;
            copty = "";
        }

        $scope.submitted = false;
        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;

        $scope.ShowReport = function () {
            $scope.printstudents = [];
            $scope.searchValue = "";
            
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
                "ALL": $scope.All,
                "OPEN": $scope.Open,
                "RUNNING": $scope.Running,
                "COMPLETE": $scope.Complete
            };

            apiService.create("Induction_Training/GetInductionReport", data).
                then(function (promise) {
                    $scope.imgname = promise.instlogo;
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
            return $scope.submitted;
        };

        // =================== Print option
        $scope.empid = {};
        //$scope.print = [];

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

        $scope.selectall = function () {

            if ($scope.All = true) {
                $scope.Open = true;
                $scope.Running = true;
                $scope.Complete = true;
            }
            else {
                $scope.Open = false;
                $scope.Running = false;
                $scope.Complete = false;
            }
        }
           
    }
})();