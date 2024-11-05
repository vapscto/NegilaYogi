(function () {
    'use strict';
    angular
        .module('app')
        .controller('MasterServiceStationController', MasterServiceStationController);
    MasterServiceStationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];

    function MasterServiceStationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
        debugger;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.servnamegrid = [];
        $scope.loaddata = function () {
            debugger;
            apiService.getURI("MasterServiceStation/loadservicestation/", 1).then(function (promise) {
                $scope.servnamegrid = promise.servnamegrid;
                if ($scope.servnamegrid.length > 0) {
                  
                    $scope.presentCountgrid = $scope.servnamegrid.length;
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
                    "TRMSST_Id": $scope.TRMSES_Id,
                    "TRMSST_ServiceStationName": $scope.TRMSES_Name,
                    "TRMSST_EmailId": $scope.TRMSES_EmailId,
                    "TRMSST_ContactNo": $scope.TRMSES_MobileNo,
                    "TRMSST_Address": $scope.TRMSES_Address,
                }
                apiService.create("MasterServiceStation/savestation", obj).then(function (promise) {
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

        $scope.searchValue = '';
        $scope.edit = function (data) {
            $scope.TRMSES_Id = data.trmssT_Id;
            apiService.getURI("MasterServiceStation/Editstation/", $scope.TRMSES_Id).then(function (promise) {
                $scope.editDataList = promise.editDataList;
                $scope.TRMSES_Id = promise.editDataList[0].trmssT_Id
                $scope.TRMSES_Name = promise.editDataList[0].trmssT_ServiceStationName;
                $scope.TRMSES_EmailId = promise.editDataList[0].trmssT_EmailId;
                $scope.TRMSES_MobileNo = promise.editDataList[0].trmssT_ContactNo;
                $scope.TRMSES_Address = promise.editDataList[0].trmssT_Address;
              
            });
        }

        $scope.deactive = function (user, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (user.trmssT_ActiveFlag === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";

            }
            else {

                mgs = "Active";
                confirmmgs = "Activated";

            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("MasterServiceStation/deactivatestation/", user).
                            then(function (promise) {
                                debugger;
                                if (promise.returnVal =='exist') {
                                    swal('You Can Not Deactivate this Record It Already Mapped');
                                }
                                else {
                                    if (promise.retval == true) {
                                        
                                        swal(confirmmgs + " " + " Successfully");
                                        $state.reload();
                                    }
                                    else {
                                        swal(confirmmgs +"");
                                        $state.reload();
                                    }
                                }
                            })
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                    $state.reload();
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
