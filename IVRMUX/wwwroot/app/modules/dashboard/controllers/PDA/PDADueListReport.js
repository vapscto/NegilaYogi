(function () {
    'use strict';
    angular
.module('app')
.controller('PDADueListReportController', PDADueListReportController)
    PDADueListReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', 'superCache']
    function PDADueListReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, superCache) {

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("PDADueListReport/getalldetails", pageid).
        then(function (promise) {
            $scope.acdyr = promise.fillyear;
            $scope.classcount = promise.classlist;
        })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }

        }


        $scope.onselectclass = function (asmcL_Id) {
            
            $scope.asmS_Id = "";
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            
            apiService.create("PDADueListReport/getsection", data).
               then(function (promise) {
                   $scope.sectioncount = promise.fillsection;
                   //for (var i = 0; i < $scope.classcount.length; i++) {
                   //    if (asmcL_Id == $scope.classcount[i].asmcL_Id) {
                   //        $scope.seledcls = $scope.classcount[i].asmcL_ClassName;
                   //    }
                   //}
                   //  $scope.arrlstinst1 = promise.fillinstallment;
               })
        }

        $scope.onselectsection = function (amsC_Id) {
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ASMCL_Id": $scope.asmcL_Id,
                "AMSC_Id": $scope.asmS_Id,
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            
            apiService.create("PDADueListReport/getstudent", data).
               then(function (promise) {
                   $scope.studentlst = promise.fillstudent;
                   //for (var i = 0; i < $scope.sectioncount.length; i++) {
                   //    if (amsC_Id == $scope.sectioncount[i].amsC_Id) {
                   //        $scope.seledsect = $scope.sectioncount[i].asmc_sectionname;
                   //    }
                   //}
               })
        }




        $scope.submitted = false;
        $scope.ShowReport = function () {
            if ($scope.myForm.$valid) {

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "PDAMH_ID": $scope.PDAMH_ID,
                    "From_Date": new Date($scope.fromDate).toDateString(),
                    "To_Date": new Date($scope.todate).toDateString(),
                }

                apiService.create("PDADueListReport/radiobtndata", data).
               then(function (promise) {
                   if (promise.headwise != null && promise.headwise != "") {
                       $scope.reportdetails = promise.headwise;
                       $scope.Grid_view = true;
                       $scope.print_flag = false;

                   }
                   else {
                       swal("No Record Found");
                       $scope.Grid_view = false;
                       $scope.print_flag = true;
                   }

               })
            }
            else {
                $scope.submitted = true;

            }
        };





     

    }
})();
