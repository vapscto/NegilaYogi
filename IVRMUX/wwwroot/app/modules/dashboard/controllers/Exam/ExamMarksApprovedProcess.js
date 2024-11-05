(function () {
    'use strict';
    angular
        .module('app')
        .controller('ExamMarksApprovedProcessController', ExamMarksApprovedProcessController)

    ExamMarksApprovedProcessController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function ExamMarksApprovedProcessController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {

        $scope.BindData = function () {
            var pageid = 2;
            apiService.getDATA("ExamCalculation/Getdetails", pageid).
                then(function (promise) {
                    $scope.yearlt = promise.yearlist;
                });
        };

        $scope.getexam = function () {
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("ExamCalculation/getexam", data).
                then(function (promise) {
                    $scope.exmstdlist = promise.exmstdlist;
                });
        };

        $scope.getclass = function () {
            var data = {
                "EME_Id": $scope.EME_Id,
                "ASMAY_Id": $scope.ASMAY_Id
            };

            apiService.create("ExamCalculation/getclass", data).
                then(function (promise) {
                    $scope.classlist = promise.classlist;
                });
        };

        $scope.submitted = false;

        $scope.savedetail = function () {
            $scope.submitted = true;


            if ($scope.myForm.$valid) {
                $scope.checkseclist = [];
                angular.forEach($scope.seclist, function (option3) {
                    if (!!option3.section) $scope.checkseclist.push(option3);
                });

                var data = {

                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "section_Array": $scope.checkseclist,
                };

                apiService.create("ExamCalculation/saveapprove", data).
                    then(function (promise) {
                        if (promise.returnval === true) {

                            swal('Data Saved successfully');
                            $scope.cancel();
                            $scope.BindData();
                        }
                        else if (promise.returnval === false && promise.returnduplicatestatus === 'Duplicate') {
                            swal('Records Already Exist !');
                        }
                        else {
                            swal('Data Not Saved !');
                        }
                    });
            }

        };


        $scope.isOptionsRequired1 = function () {

            return !$scope.seclist.some(function (option3) {
                return option3.asmS_Id;
            });
        };

        $scope.cancel = function () {
            $scope.ASMAY_Id = '';
            $scope.ASMCL_Id = '';
            $scope.ASMS_Id = '';
            $scope.EME_Id = '';
            $scope.prefix = '';
            $scope.startno = '';
            $scope.increment = '';
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

    }

})();