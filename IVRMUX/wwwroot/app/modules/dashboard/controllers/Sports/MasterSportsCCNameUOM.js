(function () {
    'use strict';

    angular
        .module('app')
        .controller('MasterSportsCCNameUOM', MasterSportsCCNameUOM);

    MasterSportsCCNameUOM.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];

    function MasterSportsCCNameUOM($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
        $scope.searchValue = "";
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.loadgrid = function () {
            apiService.getURI("MasterSportsCCNameUOM/loadgrid/", 1).then(function (promise) {
                $scope.sportsCCNameList = promise.sportsCCNameList;
                $scope.uomList = promise.uomList;
                if (promise.count > 0) {
                    $scope.sportsCCNameUOMList = promise.sportsCCNameUOMList;
                    $scope.presentCountgrid = $scope.sportsCCNameUOMList.length;
                }
                $scope.cancel();
            });
        }
        $scope.submitted = false;
        $scope.saveRecord = function () {
            if ($scope.myForm.$valid) {
                var obj = {
                    "SPCCMSCCUOM_Id": $scope.SPCCMSCCUOM_Id,
                    "SPCCMSCC_Id": $scope.SPCCMSCC_Id,
                    "SPCCMUOM_Id": $scope.SPCCMUOM_Id,
                }
                apiService.create("MasterSportsCCNameUOM/saveRecord", obj).then(function (promise) {
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
            $scope.SPCCMSCCUOM_Id = data;
            apiService.getURI("MasterSportsCCNameUOM/Edit/", $scope.SPCCMSCCUOM_Id).then(function (promise) {
                $scope.editDetails = promise.editDetails;
                $scope.SPCCMSCC_Id = promise.editDetails[0].spccmscC_Id
                $scope.SPCCMUOM_Id = promise.editDetails[0].spccmuoM_Id;
            });
        }
  

      

        //==============================================Deactivate
        $scope.deactive = function (newuser1, SweetAlertt) {
            debugger;
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            var mgs = "";
            if (newuser1.spccmsccuoM_ActiveFlag == false) {

                mgs = "Activate";

            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " Record?",
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
                        apiService.create("MasterSportsCCNameUOM/deactivate", newuser1).
                            then(function (promise) {
                                debugger;
                                if (promise.retval == true) {
                                    swal("Record " + mgs + "d Successfully!!!");
                                    $scope.loadgrid();
                                }
                                else {
                                    swal("Record Not " + mgs + "d Successfully!!!");
                                    $scope.loadgrid();
                                }

                            })
                    }
                    else {
                        swal("Cancelled");
                    }
                })
        }




        $scope.cancel = function () {
            $scope.SPCCMSCCUOM_Id = 0;
            $scope.SPCCMSCC_Id = "";
            $scope.SPCCMUOM_Id = "";
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
    }
})();
