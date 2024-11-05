(function () {
    'use strict';
    angular
.module('app')
.controller('FeeClassWiseReportController', FeeClassWiseReportController)
    FeeClassWiseReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http','superCache']
    function FeeClassWiseReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 5
            var pageid = 1;

            apiService.getURI("FeeClassWiseReport/getalldetails", pageid).
            then(function (promise) {

                $scope.arrlist6 = promise.adcyear;
                $scope.groupcount = promise.fillmastergroup;
               // $scope.headcount = promise.fillmasterhead;
                //$scope.installcount = promise.fillinstallment;
                $scope.classcount = promise.fillclass;
                $scope.sectioncount = promise.fillsection;
                $scope.categorycount = promise.fillcategory;
                $scope.ctg = false;
            })
        }

        $scope.onselectgroup = function (groupcount) {

            var data = {
                FMG_Id: $scope.fmG_Id,
                ASMAY_Id: $scope.asmaY_Id,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            
            apiService.create("FeeClassWiseReport/getheadisbygrpid", data).
               then(function (promise) {
                   $scope.headcount = promise.fillmasterhead;
               })
        }

        $scope.onselecthead = function (headcount) {

            var data = {
                FMH_Id: $scope.fmH_Id,
                ASMAY_Id: $scope.asmaY_Id,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            
            apiService.create("FeeClassWiseReport/getinstallmentid", data).
               then(function (promise) {
                   $scope.installcount = promise.fillinstallment;
               })
        }

        $scope.onclickloaddata = function () {

            if ($scope.ctg == true) {

                $scope.checked = true;

                $scope.frmdt = true;

             

            }
            else if ($scope.ctg == false) {
                $scope.checked = false;

                $scope.frmdt = false;
            }

        }





        $scope.ShowReport = function () {


            var data = {
                "FMG_Id": $scope.fmG_Id,
                "ASMAY_Id": $scope.asmaY_Id,
                "FMH_Id": $scope.fmH_Id,
                "FTI_Id": $scope.ftI_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "AMSC_Id": $scope.asmC_Id,
                "AMC_id": $scope.amC_Id,
                "category": $scope.ctg,
               
            }


            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("FeeClassWiseReport/radiobtndata", data).
          then(function (promise) {

              $scope.students = promise.classwisedata;


          })

        }
    }
})();
