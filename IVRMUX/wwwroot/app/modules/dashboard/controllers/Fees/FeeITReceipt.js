

(function () {
    'use strict';
    angular
.module('app')
.controller('FeeITReceiptReportController', FeeITReceiptReportController123)

    FeeITReceiptReportController123.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http','superCache']
    function FeeITReceiptReportController123($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5;         
            var pageid = 2;
            apiService.getURI("FeeITReceiptReport/getalldetails123", pageid).
        then(function (promise) {            
            $scope.studentlst = promise.admsudentslist;
            $scope.acayyearbind = promise.academicyr;
            
        })
        }
        $scope.onclickloaddata = function () {
            if ($scope.usercheck == "1") {
                $scope.checked = false;
            }
            else {
                $scope.checked = true;
            }
            if ($scope.studentcheck == "1") {
                $scope.studentcheckdrp = false;
            }
            else {
                $scope.studentcheckdrp = true;
            }
            if ($scope.recp == "1") {
                $scope.recpdrp = false;
                
            }
            else
            {
                $scope.recpdrp = true;
            }
            if ($scope.betdates == "1") {
               
                $scope.frmdated = false;
                $scope.todated = false;
            }
            else {
                $scope.frmdated = true;
                $scope.todated = true;
            }
        };
        $scope.ShowReportdata = function () {         
          
            var data = {
                "datedisplay": $scope.DateM,
                "Amst_Id": $scope.Amst_Id,
                "asmyid": $scope.asmaY_Id,               
            }
            apiService.create("FeeITReceiptReport/getreport", data).
        then(function (promise) {
            $scope.getclientsnames = promise.studentsnames;
            angular.forEach($scope.getclientsnames, function (e) { $scope.ClientName = e.clientname; });
            angular.forEach($scope.getclientsnames, function (e) { $scope.ClientAddress = e.insaddress; });
            angular.forEach($scope.getclientsnames, function (e) { $scope.stuname = e.stuname; });
            angular.forEach($scope.getclientsnames, function (e) { $scope.stufather = e.fathername; });
            angular.forEach($scope.getclientsnames, function (e) { $scope.stumother = e.mothername; });
            angular.forEach($scope.getclientsnames, function (e) { $scope.stucls = e.classname; });
            angular.forEach($scope.getclientsnames, function (e) { $scope.stusec = e.sectionname; });
              
            $scope.students = promise.reportdatelist;

            $scope.TotalDisplay = $scope.getTotal(promise.reportdatelist);
        })
        }
        $scope.onselectmodeof = function () {
            var VALU;
            if ($scope.BRcheck == "1") {
                VALU = $scope.CMR_Id;
            }
            else {
                VALU = 'Uncheck';
            }
            var data = {
                "filterinitialdata": $scope.filterdata,
                "fillbusroutestudents": VALU,
                "fillseccls": $scope.sectiondrp,
                "fillclasflg": $scope.clsdrp,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("FeeITReceiptReport/getgroupmappedheads", data).
       then(function (promise) {
           $scope.studentlst = promise.admsudentslist;


       })
        };

        $scope.getTotal = function (int) {
            var total = 0;
            angular.forEach($scope.students, function (e) {
                total += e.tot;
            });
            return total;
        };
    }
})();

