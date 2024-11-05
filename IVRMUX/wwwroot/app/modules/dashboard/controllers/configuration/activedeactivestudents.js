(function () {
    'use strict';
    angular
.module('app')
        .controller('ActivateDeactivateStudentController', ActivateDeactivateStudentController)

    ActivateDeactivateStudentController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function ActivateDeactivateStudentController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.pagesrecord = {};

        $scope.getpagesname = function () {
            var pageid = 1;
            apiService.getURI("ActivateDeactivateStudent/getdata", pageid).
        then(function (promise) {
            $scope.yearlist = promise.yearfilllist;
            $scope.classlist = promise.classfilllist;
            $scope.sectionlist = promise.sectionfilllist;
        })
        }

        $scope.onacademicyearchange = function (yearlist) {
            var yearid = $scope.ASMAY_Id;

            var data = {
                "yearid": $scope.ASMAY_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.getURI("ActivateDeactivateStudent/getACS", yearid).

        then(function (promise) {
          
            $scope.classlist = promise.classfilllist;
           
        })
        }

        $scope.fillstudentlist = function () {

            var data = {
                "yearid": $scope.ASMAY_Id,
                "asmcL_Id": $scope.ASMCL_Id,
                "sectionid": $scope.ASMC_Id,
                "AMST_SOL": $scope.AMST_SOL
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("ActivateDeactivateStudent/getS/", data).

        then(function (promise) {

            $scope.students = promise.studentlist;

        })
        }

        var studclear = [];
        $scope.Clearid = function () {
            $scope.ASMAY_Id = "";
            $scope.ASMCL_Id = "";
            $scope.ASMC_Id = "";
            $scope.AMST_SOL_activate = "";
            $scope.AMST_SOL_deactivate = "";
            $scope.students = studclear;
            $scope.AMST_SOL = "";
        }
  
        $scope.savedata = function (pagesrecord) {

            var acti = $scope.AMST_SOL_activate
            var deacti = $scope.AMST_SOL_deactivate
            if (acti==true)
            {
                $scope.AMST_SOL_activate = 'S';
            }
            if (deacti == true) {
                $scope.AMST_SOL_activate = 'D';
            }

            var data = {
                "yearid": $scope.ASMAY_Id,
                "asmcL_Id": $scope.ASMCL_Id,
                "sectionid": $scope.ASMC_Id,
                "AMST_SOL": $scope.AMST_SOL,
                "AMST_SOL_activate": $scope.AMST_SOL_activate,
                savetmpdata: pagesrecord

            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("ActivateDeactivateStudent/savedata/", data).

        then(function (promise) {

            if (promise.returnval == true) {
                swal('Record Activated/Deactivated Successfully', 'success');
            }
            else {
                swal('Record Not Activated/Deactivated Successfully', 'Failed');
            }

            $scope.ASMAY_Id = "";
            $scope.ASMCL_Id = "";
            $scope.ASMC_Id = "";
            $scope.students = studclear;
            $scope.AMST_SOL_activate = "";
            $scope.AMST_SOL_deactivate = "";
            $scope.AMST_SOL = "";

        })
        }

    }
})();

