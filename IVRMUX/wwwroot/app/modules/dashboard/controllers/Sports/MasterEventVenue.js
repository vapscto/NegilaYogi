(function () {
    'use strict';

    angular
        .module('app')
        .controller('MasterEventVenue', MasterEventVenue);

    MasterEventVenue.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];

    function MasterEventVenue($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
        $scope.searchValue = "";
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.loadgrid = function () {
            apiService.getURI("MasterEventVenue/loadgrid/", 1).then(function (promise) {
                if (promise.count > 0) {
                    $scope.eventVenueList = promise.eventVenueList;
                    $scope.presentCountgrid = $scope.eventVenueList.length;
                }
                $scope.cancel();
            });
        }
        $scope.submitted = false;
        $scope.saveRecord = function () {
            if ($scope.myForm.$valid) {
                var obj = {
                    "SPCCMEV_Id": $scope.SPCCMEV_Id,
                    "SPCCMEV_EventVenue": $scope.SPCCMEV_EventVenue,
                    "SPCCMEV_EventVenueDesc": $scope.SPCCMEV_EventVenueDesc
                    
                }
                apiService.create("MasterEventVenue/saveRecord", obj).then(function (promise) {
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
            $scope.SPCCMEV_Id = data;
            apiService.getURI("MasterEventVenue/Edit/", $scope.SPCCMEV_Id).then(function (promise) {
                $scope.editDetails = promise.editDetails;
                $scope.SPCCMEV_EventVenue = promise.editDetails[0].spccmeV_EventVenue
                $scope.SPCCMEV_EventVenueDesc = promise.editDetails[0].spccmeV_EventVenueDesc;
            });
        }
        $scope.deactive = function (newuser1, SweetAlertt) {

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            var mgs = "";
            if (newuser1.spccmeV_ActiveFlag == false) {

                mgs = "Activate";

            }
            else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " the Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("MasterEventVenue/deactivate/", newuser1).
                            then(function (promise) {

                                if (promise.returnVal != '' && promise != null) {
                                    if (promise.returnVal != null) {
                                        swal(promise.returnVal);
                                        $scope.loadgrid();
                                    }
                                }
                                else {
                                    swal('Failed to Activate/Deactivate the Record');
                                }
                            })
                    } else {
                        swal("Cancelled");
                    }
                })
        }
        $scope.cancel = function () {
            $scope.SPCCMEV_Id = 0;
            $scope.SPCCMEV_EventVenue = "";
            $scope.SPCCMEV_EventVenueDesc = "";
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
    }
})();
