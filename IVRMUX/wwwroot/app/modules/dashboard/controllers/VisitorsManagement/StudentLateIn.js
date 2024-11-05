(function () {
    'use strict';
    angular
        .module('app')
        .controller('GatePassController', GatePassController)

    GatePassController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$filter', 'superCache']
    function GatePassController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $filter, superCache) {

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.todaysdate = new Date();

        $scope.loadgrid = function () {
            apiService.getURI("GatePass/getDetails/", 1).then(function (promise) {
                $scope.studentlist = promise.studentlist;
            });
            //   $scope.cancel();
        };

        // TO Show The Data
        $scope.submitted = false;
        $scope.saveData = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var data = {
                    "AMST_Id": $scope.AMST_Id,
                }

                apiService.create("GatePass/getStudentDetails", data).
                    then(function (promise) {
                        if (promise.studDetails.length > 0) {
                            $scope.student = promise.studDetails;
                            $scope.sttud = true;
                            $scope.screport = true;
                        }
                        else {
                            swal('Student Late In Record Not Available')
                            $scope.sttud = false;
                            $scope.screport = false;
                        }
                    })
            }
        };

        //for print
        $scope.sendmail = function () {
            var data = {
                "AMST_Id": $scope.AMST_Id,
            }

            apiService.create("GatePass/sendmail", data).
                then(function (promise) {

                    if (promise.returnVal == 'saved') {
                        swal("SMS and Email sent successfully");
                    }
                    else {
                        swal("Something went wrong!!!");
                        $scope.loadgrid();
                    }
                })
        }

        // end for print

        $scope.cancel = function () {
            $scope.AGPH_Id = 0;
            $scope.AMST_Id = "";
            $scope.AGPH_Remark = "";
            $scope.HRME_Id = "";
            $scope.radiotype = "";
            $scope.Cumureport = false;
            $scope.Cumureport1 = false;
            $scope.sttud = false;
            $scope.screport = false;
            $scope.empp = false;
            $scope.export = false;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
        };
    }

})();