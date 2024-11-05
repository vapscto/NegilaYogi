(function () {
    'use strict';
    angular
.module('app')
.controller('FeeHeadWisecollectionReportController', FeeHeadWisecollectionReportController)
    FeeHeadWisecollectionReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http','superCache']
    function FeeHeadWisecollectionReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
        $scope.loaddata = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = 5
            var pageid = 1;
            apiService.getURI("FeeHeadWisecollectionReport/getalldetails", pageid).
        then(function (promise) {

            $scope.yearlst = promise.fillyear;
            $scope.classlist = promise.fillclass;
            $scope.seclist = promise.fillsec;
            $scope.headlist = promise.fillfeehead;
            $scope.arrlistchkgroup = promise.fillfeegroup;                  

        })
        }
        $scope.btweenordate = true;
        $scope.allorindiorcon1 = true;
        $scope.allorindiorcons123 = true;

        $scope.allorconsorall = function () {
            if ($scope.allorindiorcon == 'all') {
                $scope.allorindiorcon1 = true;
                $scope.allorindi12 = false;
                $scope.allorindiorcons123 = true;
            }
            else if ($scope.allorindiorcon == 'indi') {
                $scope.allorindiorcon1 = false;
                $scope.allorindi12 = false;
                $scope.allorindiorcons123 = true;
            }
            else if ($scope.allorindiorcon == 'consolidate') {
                $scope.allorindiorcon1 = false;
                $scope.allorindi12 = true;
                $scope.allorindiorcons123 = false;


            }
        }

        $scope.dateorbetween = function () {
            if ($scope.detweenordates == 'btndates') {
                $scope.btweenordate1 = false;
                $scope.btweenordate = true;
            }
            else if ($scope.detweenordates == 'date') {
                $scope.btweenordate1 = true;
                $scope.btweenordate = false;
            }
        }

        $scope.ShowReport = function () {

            $scope.albumNameArraygroupids = [];
            angular.forEach($scope.arrlistchkgroup, function (role) {
                if (!!role.selected) $scope.albumNameArraygroupids.push({ columnID: role.fmG_Id, columnName: role.fmG_GroupName });
            })

          
            if ($scope.detweenordates == 'btndates') {
                $scope.ondate = new Date();
               
            }
            else if ($scope.detweenordates == 'date') {
                $scope.ondate = $scope.ondate;
                $scope.datefrom = new Date();
                $scope.ondate = new Date();
            }
            else {
                $scope.datefrom = new Date();
                $scope.ondate = new Date();
                $scope.ondate = new Date();
            }

            if ($scope.allorindiorcon == 'consolidate') {
                $scope.woffconbal1 = $scope.woffconbal1;
            }
            else{
                $scope.woffconbal1 = '';
            }

            if ($scope.activeleft == 'L') {
                $scope.activeleft = $scope.activeleft;
            }
            else if ($scope.activeleft == 'S') {
                $scope.activeleft = $scope.activeleft;
            }
            else {
                $scope.activeleft = '';
            }

            if ($scope.woffconbal == 'waivedoff' || $scope.woffconbal == 'concession' || $scope.woffconbal == 'balance') {
                $scope.woffconbal = $scope.woffconbal;
            }
            else {
                $scope.woffconbal = '';
            }
          

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "FMH_Id": $scope.FMH_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMC_Id": $scope.ASMC_Id,
                "fromdate": $scope.datefrom,
                "todate": $scope.dateto,
                "ondate": $scope.ondate,
                "tempgroupids": $scope.albumNameArraygroupids,
                "allorindiorcons": $scope.allorindiorcon,
                "dateorbteween": $scope.detweenordates,
                "consolidateflag": $scope.woffconbal1,
                "activeleft": $scope.activeleft,
                "nonconsolidateflag": $scope.woffconbal,
            }

            apiService.create("FeeHeadWisecollectionReport/getreport", data).
        then(function (promise) {
         
           $scope.students = promise.alldatagridreport;
           $scope.left = "";
           $scope.active = "";

            if ($scope.allorindiorcon == 'all') {
                $scope.alloriniditabel = true;
                $scope.consolidate = false;
            }
            else if ($scope.allorindiorcon == 'indi') {
                $scope.alloriniditabel = true;
                $scope.consolidate = false;
            }
            else {
                $scope.alloriniditabel = false;
                $scope.consolidate = true;
            }

        })
        }

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
    }
})();