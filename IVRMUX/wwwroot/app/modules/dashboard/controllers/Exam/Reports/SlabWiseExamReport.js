(function () {
    'use strict';
    angular.module('app', ['ngSanitize']).controller('SlabWiseExamReportController', SlabWiseExamReportController)

    SlabWiseExamReportController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', 'Excel', '$timeout',]
    function SlabWiseExamReportController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, Excel, $timeout, ) {

        $scope.getslabreport = [];
        $scope.subjectlist = [];
        $scope.obj = {}
        $scope.BindData = function () {

            apiService.getDATA("CumulativeReport/Getdetails").then(function (promise) {
                $scope.yearlt = promise.yearlist;
                $scope.clslist = promise.classlist;
                $scope.seclist = promise.seclist;
                $scope.amstlt = promise.amstlist;
                $scope.exsplt = promise.exmstdlist;
            });
        };
        $scope.getsubject = function () {
            var data = {
                "ASMAY_ID": $scope.obj.asmaY_Id,
                "ASMCL_ID": $scope.obj.asmcL_Id,
                "ASMS_ID": $scope.obj.asmS_Id,
                "EME_ID": $scope.obj.emE_Id
            }
            apiService.create("SlabWiseExamReport/getsubjects",data).then(function (promise) {
                $scope.subjectlist = promise.subjectlist;

            });
        };

        $scope.reporttype = function () {
            var x='adarsh'
            $scope.getslabreport = [];
        }
       

        $scope.getreport = function () {
            if ($scope.myForm.$valid) {
                var item = {
                    "ASMAY_ID": $scope.obj.asmaY_Id,
                    "ASMCL_ID": $scope.obj.asmcL_Id,
                    "ASMS_ID": $scope.obj.asmS_Id,
                    "EME_ID": $scope.obj.emE_Id,
                    "ISMS_ID": $scope.obj.ISMS_ID,
                    "FROMMARKS": $scope.obj.frommarks,
                    "TOMARKS": $scope.obj.tomarks,
                    "reporttype": $scope.reporttype
                }
                apiService.create("SlabWiseExamReport/getslabreport", item).then(function (reponse) {
                    if (reponse.getslabreport != null && reponse.getslabreport.length > 0) {
                        $scope.getslabreport = reponse.getslabreport;
                        $scope.x = $scope.getslabreport[0];
                    }
                    else {
                        swal("No Data Found For The Selected Marks Range !", "", "error");
                        $scope.subjectlist = [];
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
           
        }
    }
})();