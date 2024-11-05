
(function () {
    'use strict';
    angular
        .module('app')
        .controller('BookTypeReportController', BookTypeReportController)

    BookTypeReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$q','$filter']
    function BookTypeReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $q, $filter) {

        $scope.submitted = false;
        $scope.tablediv = false;
        $scope.printd = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.search = "";

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

       //---------------Get-Report
        $scope.get_report = function () {
            if ($scope.myForm.$valid) {
                var fromdate1 = $scope.Issue_Date == null ? "" : $filter('date')($scope.Issue_Date, "yyyy-MM-dd");
                var todate1 = $scope.IssueToDate == null ? "" : $filter('date')($scope.IssueToDate, "yyyy-MM-dd");
                var data = {
                    "Type": $scope.Type,
                    "Issue_Date": fromdate1,
                    "IssueToDate": todate1,
                }

                apiService.create("BookTypeReport/get_report", data).then(function (promise) {
                    if (promise.getdata.length > 0) {

                        $scope.getdata = promise.getdata;
                      //  console.log($scope.getdata)
                        $scope.tablediv = true;;
                        $scope.printd = true;
                    }
                    else {
                        swal('Record Not Available!!!');
                        $state.reload();
                    }
                })

            }
            else {
                $scope.submitted = true;
            }

        }
        //-------------End--Get-Report



        //===========print===========//
        $scope.printData = function () {
            var innerContents = document.getElementById("printtable").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Library/BookTypeReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }
        //==============End==============//

        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        //-----------clear-field
        $scope.Clearid = function () {
            $state.reload();
        }



    }
})();

