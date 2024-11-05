(function () {
    'use strict';

    angular
        .module('app')
        .controller('ProgramMasterController', ProgramMasterController);

    ProgramMasterController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];

    function ProgramMasterController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
        //   $scope.searchValue = "";
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.loadgrid = function () {
            apiService.getURI("ProgramMaster/loadgrid/", 1).then(function (promise) {
                $scope.programList = promise.programList;
                $scope.presentCountgrid = $scope.programList.length;
            });
            $scope.cancel();

        }

        $scope.submitted = false;
        $scope.saveRecord = function () {
            if ($scope.myForm.$valid) {
                var obj = {
                    "SPCCPM_Id": $scope.SPCCPM_Id,
                    "SPCCPM_Name": $scope.SPCCPM_Name,
                    "SPCCPM_Description": $scope.SPCCPM_Description
                }
                apiService.create("ProgramMaster/saveRecord", obj).then(function (promise) {
                    if (promise.returnVal == 'saved') {
                        swal("Record Saved Successfully");
                        $scope.loadgrid();
                    }
                    else if (promise.returnVal == 'updated') {
                        swal("Record Updated Successfully");
                        $scope.loadgrid();
                    }
                    else if (promise.returnVal == 'duplicate') {
                        swal("Record already exist");
                    }
                    else if (promise.returnVal == "savingFailed") {
                        swal("Failed to save record");
                    }
                    else if (promise.returnVal == "updateFailed") {
                        swal("Failed to update record");
                    }
                    else {
                        swal("Sorry...something went wrong");
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        }
        $scope.edit = function (data) {
            $scope.SPCCPM_Id = data;
            apiService.getURI("ProgramMaster/Edit/", $scope.SPCCPM_Id).then(function (promise) {
                $scope.editDetails = promise.editDetails;
                $scope.SPCCPM_Name = promise.editDetails[0].spccpM_Name
                $scope.SPCCPM_Description = promise.editDetails[0].spccpM_Description;
            });
        }
        $scope.deactive = function (data) {

            if (data.spccpM_ActiveFlag == false) {
                var data1 = {
                    "SPCCPM_Id": data.spccpM_Id,
                    "SPCCPM_ActiveFlag": true
                }
            }
            else if (data.spccpM_ActiveFlag == true) {
                var data1 = {
                    "SPCCPM_Id": data.spccpM_Id,
                    "SPCCPM_ActiveFlag": false
                }
            }
            apiService.create("ProgramMaster/deactivate/", data1).then(function (promise) {
                if (promise.returnVal != '' && promise != null) {
                    swal(promise.returnVal);
                    $scope.loadgrid();
                }
                else {
                    swal("Something went wrong");
                }
            });

        }
        $scope.cancel = function () {
            $scope.SPCCPM_Id = 0;
            $scope.SPCCPM_Name = "";
            $scope.SPCCPM_Description = "";
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.searchValue = "";
        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.spccpM_Description)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.spccpM_Name)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }
    }
})();
