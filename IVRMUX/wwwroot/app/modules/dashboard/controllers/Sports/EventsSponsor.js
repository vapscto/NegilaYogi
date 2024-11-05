(function () {
    'use strict';

    angular
        .module('app')
        .controller('EventsSponsor', EventsSponsor);

    EventsSponsor.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];

    function EventsSponsor($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
        $scope.searchValue = "";
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.loadgrid = function () {
            apiService.getURI("EventsSponsor/loadgrid/", 1).then(function (promise) {
                $scope.sponsorList = promise.sponsorList;
                $scope.events = promise.events;
                if (promise.count > 0) {
                    $scope.sponsormappingList = promise.sponsormappingList;
                    $scope.presentCountgrid = $scope.sponsormappingList.length;
                }
                $scope.cancel();
            });
        }
        $scope.submitted = false;
        $scope.saveRecord = function () {
            if ($scope.myForm.$valid) {
                var obj = {
                    "SPCCESP_Id": $scope.SPCCESP_Id,
                    "SPCCE_Id": $scope.SPCCE_Id,
                    "SPCCMSP_Id": $scope.SPCCMSP_Id
                }
                apiService.create("EventsSponsor/saveRecord", obj).then(function (promise) {
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
            $scope.SPCCESP_Id = data;
            apiService.getURI("EventsSponsor/Edit/", $scope.SPCCESP_Id).then(function (promise) {
                $scope.editDetails = promise.editDetails;
                $scope.SPCCE_Id = promise.editDetails[0].spccE_Id
                $scope.SPCCMSP_Id = promise.editDetails[0].spccmsP_Id;
            });
        }
        $scope.deactive = function (data) {
            
            if (data.spccesP_ActiveFlag == false) {
                var obj = {
                    "SPCCESP_Id": data.spccesP_Id,
                    "SPCCESP_ActiveFlag": true
                }
            }
            else if (data.spccesP_ActiveFlag == true) {
                var obj = {
                    "SPCCESP_Id": data.spccesP_Id,
                    "SPCCESP_ActiveFlag": false
                }
            }
            apiService.create("EventsSponsor/deactivate/", obj).then(function (promise) {
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
            $scope.SPCCESP_Id = 0;
            $scope.SPCCE_Id = "";
            $scope.SPCCMSP_Id = "";
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
    }
})();
