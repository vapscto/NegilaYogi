(function () {
    'use strict';

    angular
        .module('app')
        .controller('MasterHirerRate', MasterHirerRate);

    MasterHirerRate.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];

    function MasterHirerRate($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.loaddata = function () {
            apiService.getURI("MasterHirer_Group_Rate/loadRateData/", 1).then(function (promise) {
                $scope.hirerGroupList = promise.hirerGroupList;
                $scope.vhcleTypeList = promise.vhcleTypeList;
                if (promise.count > 0) {
                    $scope.hirerRateList = promise.hirerRateList;
                    $scope.presentCountgrid = $scope.hirerRateList.length;
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
                    "TRMVT_Id": $scope.TRMVT_Id,
                    "TRHG_Id": $scope.TRHG_Id,
                    "TRHR_RatePerKM": $scope.TRHR_RatePerKM,
                    "TRHR_Id": $scope.TRHR_Id
                }
                apiService.create("MasterHirer_Group_Rate/saveRate", obj).then(function (promise) {
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
            $scope.TRHR_Id = data.trhR_Id;
            apiService.getURI("MasterHirer_Group_Rate/EditRate/", $scope.TRHR_Id).then(function (promise) {
                $scope.editDataList = promise.editDataList;
                $scope.TRMVT_Id = promise.editDataList[0].trmvT_Id;
                $scope.TRHG_Id = promise.editDataList[0].trhG_Id;
                $scope.TRHR_RatePerKM = promise.editDataList[0].trhR_RatePerKM;
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
