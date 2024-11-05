(function () {
    'use strict';

    angular
        .module('app')
        .controller('MasterEvents', MasterEvents);

    MasterEvents.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];

    function MasterEvents($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
     //   $scope.searchValue = "";
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        //===============================loaddata
        $scope.loaddata = function () { 
            var pageid = 2;
            apiService.getURI("MasterEvent/getDetails", pageid).then(function (promise) {
                    $scope.eventList = promise.eventList;
                $scope.presentCountgrid = $scope.eventList.length;
            });
                $scope.cancel();
            
        }

        //$scope.qualification_type = 'others';

        //$scope.onselectradio = function () {
        //    $scope.loaddata();
        //};


        $scope.submitted = false;
        $scope.saveRecord = function () {
            if ($scope.myForm.$valid) {
                var obj = {
                    "SPCCME_Id": $scope.spccmE_Id,
                    "SPCCME_EventName": $scope.spccmE_EventName,
                    "SPCCME_EventNameDesc": $scope.spccmE_EventNameDesc,
                    //"SPCCME_Flag":$scope.qualification_type
                }
                apiService.create("MasterEvent/saveRecord", obj).then(function (promise) {
                    if (promise.returnVal == 'saved') {
                        swal("Record Saved Successfully");
                        $state.reload();
                    }
                    else if (promise.returnVal == 'updated') {
                        swal("Record Updated Successfully");
                        $state.reload();
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
            var data={
                "SPCCME_Id":data,
            }
            //$scope.SPCCME_Id = data;
            apiService.create("MasterEvent/EditDetails", data).then(function (promise) {
                $scope.editDetails = promise.editDetails;
                $scope.spccmE_EventName = promise.editDetails[0].spccmE_EventName
                $scope.spccmE_EventNameDesc = promise.editDetails[0].spccmE_EventNameDesc;
                $scope.spccmE_Id = promise.editDetails[0].spccmE_Id;
            });
        }
        $scope.deactive = function (newuser1, SweetAlertt) {
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            var mgs = "";
            if (newuser1.spccmE_ActiveFlag == false) {

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
                        apiService.create("MasterEvent/deactivate/", newuser1).
                            then(function (promise) {

                                if (promise.returnVal != '' && promise != null) {
                                    if (promise.returnVal != null) {
                                        swal(promise.returnVal);
                                        $scope.loaddata();
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
            $scope.spccmE_Id = 0;
            $scope.spccmE_EventName = "";
            $scope.spccmE_EventNameDesc = "";
            $scope.submitted = false;

            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.searchValue = "";
        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.spccmE_EventNameDesc)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
             (angular.lowercase(obj.spccmE_EventName)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }
    }
})();
