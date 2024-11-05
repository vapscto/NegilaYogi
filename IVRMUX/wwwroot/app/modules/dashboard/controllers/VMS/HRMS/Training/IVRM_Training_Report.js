(function () {
    'use strict';
    angular.module('app').controller('IVRM_Training_ReportController', IVRM_Training_ReportController)
    IVRM_Training_ReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function IVRM_Training_ReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.submitted = false;
        $scope.obj = {};
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.FromDate = new Date();
        $scope.ToDate = $scope.FromDate;


        $scope.Loaddata = function () {
          
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("IVRTM_Training/onloaddatareport", pageid).then(function (promise) {

                $scope.emp_list = promise.emp_deatils;
                $scope.programname = promise.trainingdetails;
                $scope.FromDate = new Date();

                $scope.ToDate = $scope.FromDate;
                $scope.minDateTo = new Date(
                    $scope.ToDate.getFullYear(),
                    $scope.ToDate.getMonth(),
                    $scope.ToDate.getDate());
            });
        };

        $scope.submitted = false;
        $scope.report = function () {

            $scope.submitted = true;

            if ($scope.myForm.$valid) {

                //var fromdate1 =  $filter('date')($scope.startfromdate, "yyyy-MM-dd");
                //var todate1 =  $filter('date')($scope.startenddate, "yyyy-MM-dd");

                var data = {
                    "HRME_Id": $scope.obj.hrmE_Id.hrmE_Id,
                    "IVRMTT_TrainingMode": $scope.obj.ivrmtT_Id.ivrmtT_TrainingMode,
                    "status": $scope.obj.ivrmtT_Id.ivrmtT_Status,                   
                    "startdate": $scope.FromDate,
                    "enddate": $scope.ToDate,
                    
                };

                apiService.create("IVRTM_Training/getreport", data).then(function (promise) {
                    if (promise.trainingdetails.length > 0) {
                        $scope.sumary = promise.trainingdetails;                       
                    }
                    else {
                        swal("No Records Found");
                        $scope.count = 0;
                    }

                });
            }
            else {
                $scope.submitted1 = true;
            }
        };


        //setToDate
        $scope.setToDate = function (FromDate) {

            $scope.ToDate = FromDate;
            $scope.minDateTo = new Date(
                $scope.ToDate.getFullYear(),
                $scope.ToDate.getMonth(),
                $scope.ToDate.getDate());


            // $scope.Employee.ToDate = ;
        }
        


        $scope.printToCart = function (Baldwin) {
            var innerContents = document.getElementById("Training").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/pfchallan/PFChallanPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.exportToExcel = function () {
            var divToPrint = document.getElementById("Training");
            var blob = new Blob([divToPrint.outerHTML], {
                type: "application/vnd.ms-excel;charset=utf-8"
            });
            saveAs(blob, "Report.xls");
        };




        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Clearid = function () {
            $state.reload();
        };



        $scope.printData = function () {
            var innerContents = "";
            innerContents = document.getElementById("printDeviation").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +

                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/NDSReportPdf.css" />' +

                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };




    }
})();

