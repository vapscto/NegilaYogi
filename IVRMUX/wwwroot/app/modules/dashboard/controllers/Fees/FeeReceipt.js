

(function () {
    'use strict';
    angular
.module('app')
.controller('FeeReceiptReportController', FeeReceiptReportController123)

    FeeReceiptReportController123.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http','superCache']
    function FeeReceiptReportController123($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;         
            var pageid = 2;
            apiService.getURI("FeeReceiptReport/getalldetails123", pageid).
        then(function (promise) {
            $scope.categoryarray = promise.categoryarray;
            $scope.receiptlistarray = promise.newreplist;
            $scope.dis = false; $scope.dis123 = false;
            $scope.onclickloaddata();
        })
        }
        $scope.onclickloaddata = function () {
            if ($scope.rndind == "ReceiptNoWise") {
                $scope.recpdrp = false;
            }
            else {
                $scope.recpdrp = true;
            }
            if ($scope.Header == "1") {
                $scope.recatogorydrp =true ;
            }
            else {
                $scope.recatogorydrp = false;
            }
        };
        $scope.ShowReportdata = function () {         
          
            var data = {
                "recpno": $scope.rcp_model,
            }
            apiService.create("FeeReceiptReport/getreport", data).
        then(function (promise) {
            $scope.students = promise.reportdatelist;
            $scope.stunameM = $scope.naem(promise.reportdatelist);
            $scope.admnoM = $scope.stuadmno(promise.reportdatelist);
            $scope.clsM = $scope.classnaem(promise.reportdatelist);
            $scope.rcpnoDM = $scope.repno(promise.reportdatelist);
            $scope.acyrDM = $scope.acayyername(promise.reportdatelist);
            $scope.cheqdateDM = $scope.dateofcheck(promise.reportdatelist);
            $scope.feerecvdM = $scope.paidAmt(promise.reportdatelist);
            $scope.payflgM = $scope.typeonmode(promise.reportdatelist);
            $scope.rmksM = "";
            $scope.totpaidM = $scope.getTotal(promise.reportdatelist);
            $scope.totconsessionM = $scope.getTotal1(promise.reportdatelist);
            $scope.totfineM = $scope.getTotal2(promise.reportdatelist);
            $scope.totnetM = $scope.getTotal(promise.reportdatelist);
            $scope.totbalM = $scope.getTotal(promise.reportdatelist);
            $scope.TextBoxChanged();
            $scope.dis = true;
        })
        }
        $scope.naem = function (int) {var total;
            angular.forEach($scope.students, function (e) { total = e.stuname; });
            return total;
        };
        $scope.stuadmno = function (int) {
            var total;
            angular.forEach($scope.students, function (e) { total = e.stuadmno; });
            return total;
        };
        $scope.classnaem = function (int) {
            var total;
            angular.forEach($scope.students, function (e) { total = e.classnaem; });
            return total;
        };
        $scope.repno = function (int) {
            var total;
            angular.forEach($scope.students, function (e) { total = e.repno; });
            return total;
        };
        $scope.acayyername = function (int) {
            var total;
            angular.forEach($scope.students, function (e) { total = e.acayyername; });
            return total;
        };
        $scope.dateofcheck = function (int) {
            var total;
            angular.forEach($scope.students, function (e) { total = e.dateofcheck; });
            return total;
        };
        $scope.paidAmt = function (int) {
            var total;
            angular.forEach($scope.students, function (e) { total = e.paidAmt; });
            return total;
        };
        $scope.typeonmode = function (int) {
            var total;
            angular.forEach($scope.students, function (e) { total = e.typeonmode; });
            return total;
        };       
        $scope.getTotal = function (int) {
            var total = 0;
            angular.forEach($scope.students, function (e) {
                total += e.paidAmt;
            });
            return total;
        };
        $scope.getTotal1 = function (int) {
            var total = 0;
            angular.forEach($scope.students, function (e) {
                total += e.concessionAmt;
            });
            return total;
        };
        $scope.getTotal2 = function (int) {
            var total = 0;
            angular.forEach($scope.students, function (e) {
                total += e.fineAmt;
            });
            return total;
        };
      

        $scope.printData = function () {
            
            var divToPrint = document.getElementById("printrcp123");
            var newWin = window.open("");
            newWin.document.write(divToPrint.outerHTML);
            newWin.print();
            newWin.close();
          
        }
        $scope.TextBoxChanged = function () {
            var valu = $scope.ins_model;
            var pageid = 2;
            apiService.getURI("FeeReceiptReport/getinsdetils", pageid).
        then(function (promise) {
            //$scope.categoryarray = promise.insdata;
            angular.forEach(promise.insdata, function (e) {
                $scope.insnamebind = e.insname;
                $scope.insaddrebind = e.insaddress
            });
        })
        }
       
    }
})();

