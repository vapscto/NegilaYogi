(function () {
    'use strict';

    angular
        .module('app')
        .controller('MasterHirer', MasterHirer);

    MasterHirer.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];

    function MasterHirer($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.loaddata = function () {
            apiService.getURI("MasterHirer_Group_Rate/loadHirerData/", 1).then(function (promise) {
                $scope.hirerGroupList = promise.hirerGroupList;
                if (promise.count > 0) {
                    $scope.hirerList = promise.hirerList;
                    $scope.presentCountgrid = $scope.hirerList.length;
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
                    "TRHG_Id": $scope.TRHG_Id,
                    "TRMH_HirerName": $scope.TRMH_HirerName,
                    "TRMH_ConatctPerName": $scope.TRMH_ConatctPerName,
                    "TRMH_ContactPersonDesg": $scope.TRMH_ContactPersonDesg,
                    "TRMH_ContactNo": $scope.TRMH_ContactNo,
                    "TRMH_MobileNo": $scope.TRMH_MobileNo,
                    "TRMH_EmailId": $scope.TRMH_EmailId,
                    "TRMH_Address": $scope.TRMH_Address,
                    "TRMH_Id": $scope.TRMH_Id
                }
                apiService.create("MasterHirer_Group_Rate/saveHirer", obj).then(function (promise) {
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
            $scope.TRMH_Id = data.trmH_Id;
            apiService.getURI("MasterHirer_Group_Rate/EditHirer/", $scope.TRMH_Id).then(function (promise) {
                $scope.editDataList = promise.editDataList;
                $scope.TRHG_Id = promise.editDataList[0].trhG_Id;
                $scope.TRMH_HirerName = promise.editDataList[0].trmH_HirerName
                $scope.TRMH_ConatctPerName = promise.editDataList[0].trmH_ConatctPerName;
                $scope.TRMH_ContactPersonDesg = promise.editDataList[0].trmH_ContactPersonDesg;
                $scope.TRMH_ContactNo = promise.editDataList[0].trmH_ContactNo;
                $scope.TRMH_MobileNo = promise.editDataList[0].trmH_MobileNo;
                $scope.TRMH_EmailId = promise.editDataList[0].trmH_EmailId;
                $scope.TRMH_Address = promise.editDataList[0].trmH_Address;
            });
        }
        $scope.deactive = function (data) {
            
            if (data.trmH_ActiveFlg == false) {
                var obj = {
                    "TRMH_Id": data.trmH_Id,
                    "TRMH_ActiveFlg": true
                }
            }
            else if (data.trmH_ActiveFlg == true) {
                var obj = {
                    "TRMH_Id": data.trmH_Id,
                    "TRMH_ActiveFlg": false
                }
            }
            apiService.create("MasterHirer_Group_Rate/deactivateHirer/", obj).then(function (promise) {
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
