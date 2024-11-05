(function () {
    'use strict';

    angular
        .module('app')
        .controller('MasterHirerGroup', MasterHirerGroup);

    MasterHirerGroup.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];

    function MasterHirerGroup($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.loaddata = function () {
            apiService.getURI("MasterHirer_Group_Rate/loadData/", 1).then(function (promise) {
                if (promise.count > 0) {
                    $scope.hirerGroupList = promise.hirerGroupList;
                    $scope.presentCountgrid = $scope.hirerGroupList.length;
                }
                else {
                    swal("No Records Found");
                }
            });
        }
        $scope.submitted = false;
        $scope.savedata = function () {
            if ($scope.myForm.$valid) {
                var obj = {
                    "TRHG_HirerGroup": $scope.TRHG_HirerGroup,
                    "TRHG_HirerDec": $scope.TRHG_HirerDec,
                    "TRHG_Id": $scope.TRHG_Id
                }
                apiService.create("MasterHirer_Group_Rate/save", obj).then(function (promise) {
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
                    else if (promise.returnVal == "failedUpdate") {
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
            $scope.TRHG_Id = data.trhG_Id;
            apiService.getURI("MasterHirer_Group_Rate/Edit/", $scope.TRHG_Id).then(function (promise) {
                $scope.editDataList = promise.editDataList;
                $scope.TRHG_HirerGroup = promise.editDataList[0].trhG_HirerGroup;
                $scope.TRHG_HirerDec = promise.editDataList[0].trhG_HirerDec;
            });
        }
        $scope.deactive = function (data) {
            
            if (data.trhG_ActiveFlg == false) {
                var obj = {
                    "TRHG_Id": data.trhG_Id,
                    "TRHG_ActiveFlg":true
                }
            }
            else if (data.trhG_ActiveFlg == true) {
                var obj = {
                    "TRHG_Id": data.trhG_Id,
                    "TRHG_ActiveFlg": false
                }
            }
            apiService.create("MasterHirer_Group_Rate/deactivate/", obj).then(function (promise) {
                if (promise.returnVal != '' && promise != null) {
                    swal(promise.returnVal);
                    $state.reload();
                }
                else {
                    swal("Something went wrong");
                }
            });

        }
        $scope.cancel = function () {
            $state.reload();
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
    }
})();
