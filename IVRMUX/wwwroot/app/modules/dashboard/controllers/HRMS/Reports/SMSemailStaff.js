(function () {
    'use strict';
    angular
        .module('app')
        .controller('SMSemailStaffController', SMSemailStaffController)
    SMSemailStaffController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$stateParams', '$filter', 'superCache', '$q']
    function SMSemailStaffController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $stateParams, $filter, superCache, $q) {
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("SMSemailStaffReport/getalldetails", pageid).then(function (promise) {
                $scope.depatmentlist = promise.getDepartment;
              
                $scope.getdesination = promise.getdesination;

            })
        };
        $scope.onDepartment = function () {
     
           
                for (var i = 0; i < $scope.depatmentlist.length; i++) {
                    if ($scope.depatmentlist[i].hrmD_Id === $scope.HRMD_Id) {
                        $scope.moduleynew.push({
                            HRMD_Id: $scope.depatmentlist[i].hrmD_Id
                        })
                    }
                }
               
                var data = {
                    "departmentOne": $scope.departmentOne,
                    
                }
            apiService.create("SMSemailStaffReport/getreport", data).then(function (promise) {
                
                
                   

                })
           // }
           
        };
        $scope.submitted = false;
        $scope.getreportpage = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {                          
                var data = {
                  
                }
                apiService.create("SMSemailStaffReport/getreport", data).
                    then(function (promise) {
                        
                    })
            }

        };
        $scope.sendEmail = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {

                }
                apiService.create("SMSemailStaffReport/smsemail", data).
                    then(function (promise) {

                    })
            }

        };
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };     
        $scope.clear = function () {
            $state.reload();
        }
    }

})();