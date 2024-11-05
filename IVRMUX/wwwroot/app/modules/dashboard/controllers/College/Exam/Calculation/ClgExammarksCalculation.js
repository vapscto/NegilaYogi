
(function () {
    'use strict';
    angular
        .module('app')
        .controller('ClgExammarksCalculationController', ClgExammarksCalculationController)

    ClgExammarksCalculationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function ClgExammarksCalculationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {

        $scope.BindData = function () {
            apiService.getDATA("ClgExammarksCalculation/Getdetails").
                then(function (promise) {
                    $scope.yearlt = promise.yearlist;
                    $scope.course_list = promise.courseslist;
                    $scope.semisters_list = promise.semesters;
                    $scope.branch_list = promise.branchlist;
                    $scope.exsplt = promise.examlist;
                    $scope.seclist = promise.sectionlist;
                })
        };



        $scope.submitted = false;
        $scope.saveddata = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "ACMS_Id": $scope.ACMS_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "EME_Id": $scope.EME_Id
                }
                apiService.create("ClgExammarksCalculation/Calculation", data).
                    then(function (promise) {
                        if (promise.returnval === true) {
                            swal('Marks Calculated Successfully');
                        }
                        else {
                            swal('Marks Not Calculated !!!');
                        }
                    })

            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.cancel = function () {
            $scope.ASMAY_Id = ""
            $scope.AMCO_Id = ""
            $scope.AMB_Id = ""
            $scope.ACMS_Id = ""
            $scope.AMSE_Id = ""
            $scope.EME_Id = ""
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        }


        $scope.interacted = function (field) {

            return $scope.submitted;
        };

    }

})();