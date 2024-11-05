(function () {
    'use strict';
    angular.module('app').controller('InstitutionUserMappingController', InstitutionUserMappingController)
    InstitutionUserMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function InstitutionUserMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {

        $scope.loaddata = function () {
            var pageid = 2;
            $scope.itemsPerPage = 10;
            $scope.currentPage = 1;

            apiService.getURI("InstitutionUserMapping/loaddata", pageid).then(function (promise) {
                $scope.getinstitution = promise.getinstitution;
                $scope.getinstitutionloaddata = promise.getinstitutionloaddata;
            });
        };

        $scope.onchangeinstitution = function () {
            $scope.getemployeedetails = [];
            $scope.getsavedemployee = [];
            var data = {
                "MI_Id": $scope.MI_Id
            };
            apiService.create("InstitutionUserMapping/onchangeinst", data).then(function (promise) {
                $scope.getemployeedetails = promise.getemployeedetails;
                $scope.getsavedemployee = promise.getsavedemployee;
                angular.forEach($scope.getemployeedetails, function (dd) {
                    angular.forEach($scope.getsavedemployee, function (d) {
                        if (dd.hrmE_Id === d.hrmE_Id) {
                            dd.checkedsub = true;
                        }
                    });
                });
            });
        };

        $scope.savedata = function () {
            $scope.submitted = false;
            if ($scope.myForm.$valid) {

                $scope.tempdetails = [];
                angular.forEach($scope.getemployeedetails, function (d) {
                    if (d.checkedsub) {
                        $scope.tempdetails.push({ HRME_Id: d.hrmE_Id });
                    }
                });

                var data = {
                    "MI_Id": $scope.MI_Id,
                    "saveselectedlist": $scope.tempdetails
                };

                apiService.create("InstitutionUserMapping/savedetails", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.returnval === true) {
                            swal("Record Saved/Updated Successfully");
                        } else {
                            swal("Failed To Save/Update Record");
                        }

                        $state.reload();
                    }
                });

            } else {
                $scope.submitted = true;
            }
        };

        $scope.viewdetails = function (dd) {
            $scope.getviewemployeedetails = [];

            var data = {
                "MI_Id": dd.mI_Id
            };
            apiService.create("InstitutionUserMapping/viewdetails", data).then(function (promise) {
                $scope.getviewemployeedetails = promise.getviewemployeedetails;
            });
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.clearData = function () {
            $state.reload();
        };

        $scope.searchchkbx = "";
        $scope.search = "";
        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.employeename).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        };

        $scope.isOptionsRequired1 = function () {
            return !$scope.getemployeedetails.some(function (options) {
                return options.checkedsub;
            });
        };

        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.mI_Name)).indexOf(angular.lowercase($scope.search)) >= 0;                
        };
    }
})();