(function () {
    'use strict';

    angular
        .module('app')
        .controller('VBSC_Master_SportsCCName_UOMController', VBSC_Master_SportsCCName_UOMController);

    VBSC_Master_SportsCCName_UOMController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function VBSC_Master_SportsCCName_UOMController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
        $scope.searchValue = "";
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.loadgrid = function () {
            apiService.getURI("VBSC_Master_SportsCCName_UOM/getDetails/", 1).then(function (promise) {

                $scope.sportsCCNameList = promise.sportsCCNameList;
                $scope.uomList = promise.uomList;
                $scope.savedata = promise.save;
                if (promise.savedata > 0) {
                    $scope.sportsCCNameUOMList = promise.sportsCCNameUOMList;     
                }
                $scope.cancel();
            });
        }

        
        $scope.submitted = false;
        $scope.saveRecord = function () {
            if ($scope.myForm.$valid) {
                var obj = {
                    "VBSCMSCCUOM_Id": $scope.VBSCMSCCUOM_Id,
                    "VBSCMSCC_Id": $scope.VBSCMSCC_Id,
                    "VBCCMUOM_Id": $scope.VBCCMUOM_Id,
                    
                }
                apiService.create("VBSC_Master_SportsCCName_UOM/saveRecord", obj).then(function (promise) {
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
            $scope.VBSCMSCCUOM_Id = data;
            apiService.getURI("VBSC_Master_SportsCCName_UOM/Edit/", $scope.VBSCMSCCUOM_Id).then(function (promise) {
                $scope.editDetails = promise.editDetails;
                $scope.VBSCMSCC_Id = promise.editDetails[0].vbscmscC_Id
                $scope.VBCCMUOM_Id = promise.editDetails[0].vbccmuoM_Id;
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
            if (newuser1.vbscmsccuoM_ActiveFlag == false) {

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
                        apiService.create("VBSC_Master_SportsCCName_UOM/deactivate", newuser1).
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
            $scope.VBSCMSCCUOM_Id = 0;
            $scope.VBSCMSCC_Id = "";
            $scope.VBCCMUOM_Id = "";
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();

        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
    }
})();
