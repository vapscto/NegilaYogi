

(function () {
    'use strict';
    angular
.module('app')
.controller('FeeSummaryReportController', FeeSummaryReport123)

    FeeSummaryReport123.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http','superCache']
    function FeeSummaryReport123($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
        $scope.onclickloaddata = function () {
            if ($scope.rndind == "CollectionSummary") {
                $scope.rndind123 = true;
                $scope.table = CollectionSummaryTable();
            }
            else {
                $scope.rndind123 = false;
                $scope.table = ReceiptSummaryTable();
            }
           
            if ($scope.betdates == "betdatesdisable") {
                $scope.betdatesdisable = false;
                $scope.betdatesdisable1 = false;
                $scope.acyeardisable = true;
            }
            else {
                $scope.betdatesdisable = true;
                $scope.betdatesdisable1 = true;
                $scope.acyeardisable = false;
            }
        };
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;
            var pageid = 2;
            apiService.getURI("FeeSummaryReport/getalldetails123", pageid).
        then(function (promise) {
            $scope.acayyearbind = promise.acayear;
            $scope.arrlistchk = promise.fgrp
            $scope.onclickloaddata();
        })
        }
        function CollectionSummaryTable() {
            return {
                columns: [{
                    value: 'Sl.No'
                }, {
                    value: 'Details'
                }, {
                    value: 'Amount'
                },
               {
                   value: 'No.Of Students'
               }],
            }
        }
        function ReceiptSummaryTable() {
            return {
                columns: [{
                    value: 'Sl.No'
                }, {
                    value: 'Details'
                }, {
                    value: 'Amount'
                },
               {
                   value: 'No.Of Receipts'
               }],
            }
        }


        $scope.getTotal = function (int) {
            var total = 0;
            angular.forEach($scope.students, function (e) {
                total += e.Amount;
            });
            return total;
        };

        $scope.getTotalcnt = function (int) {
            var totalcnt = 0;
            angular.forEach($scope.students, function (e) {
                totalcnt += e.studcount;
            });
            return totalcnt;
        };
        $scope.ShowReportdata = function (arrlistchk) {
            $scope.albumNameArray = [];
            angular.forEach($scope.arrlistchk, function (role) {
                if (!!role.selected) $scope.albumNameArray.push({FMG_Id:role.fmG_Id});
            })
            var data = {
                "yerid": $scope.academicyr,
                "fromdate": $scope.fromdateM,
                "todate": $scope.todateM,
                "FalgIn": $scope.rndind,
                "Falgout": $scope.betdates,
                "TempararyArrayListnew": $scope.albumNameArray,
            }
            apiService.create("FeeSummaryReport/getreport", data).
        then(function (promise) {
            $scope.students = promise.reportdatelist;
            $scope.tot=$scope.getTotal(promise.reportdatelist);          
            $scope.totcnt = $scope.getTotalcnt(promise.reportdatelist);
        })
        }
    }
})();

