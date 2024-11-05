(function () {
    'use strict';
    angular
        .module('app')
        .controller('masterLeavingReason', masterLeavingReason)
    masterLeavingReason.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache']
    function masterLeavingReason($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache) {
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.search = "";
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        $scope.Clearid = function () {
            $scope.HRMLREA_LeavingReason = '';
            $state.reload();
        };
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.all_check = function () {
            
        }
        //============load
        $scope.submitted = false;
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("masterLeavingReason/loaddata", pageid).then(function (promise) {
                $scope.alldata = promise.alldata;
                $scope.get_master = $scope.alldata.length;
            });
        }
        //================save
        $scope.savedata = function () {           
            if ($scope.myForm.$valid) {
                var data = {
                    "HRMLREA_Id": $scope.HRMLREA_Id,
                    "MI_Id": $scope.MI_Id,
                    "HRMLREA_LeavingReason": $scope.HRMLREA_LeavingReason,
                    "HRMLREA_TransferredFlg": $scope.HRMLREA_TransferredFlg	,
                }
                apiService.create("masterLeavingReason/savedata", data).then(function (promise) {                 
                    if (promise.duplicate == true) {
                        swal("Record already existing");
                    }
                    else if (promise.msg == 'Saved') {
                        swal("Record saved succussfully");
                        $state.reload();
                    }
                    else if (promise.msg == 'Failed') {
                        swal("Record not saved succussfully");
                    }
                    else if (promise.msg == 'Updated') {
                        swal("Record updated succussfully")
                        $state.reload();
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        }
        //==============Edit
        $scope.EditData = function (user) {
            var data = {
                "HRMLREA_Id": user.hrmlreA_Id	
            }
            apiService.create("masterLeavingReason/EditData", data).then(function (promise) {
                if (promise.editlist !== null && promise.editlist.length > 0) {
                    $scope.editlist = promise.editlist;
                    $scope.HRMLREA_Id = promise.editlist[0].hrmlreA_Id;
                    $scope.MI_Id = promise.editlist[0].mI_Id;
                    $scope.HRMLREA_LeavingReason = promise.editlist[0].hrmlreA_LeavingReason;
                    if (promise.editlist[0].hrmlreA_TransferredFlg != null) {
                        if (promise.editlist[0].hrmlreA_TransferredFlg == 0) {
                            $scope.HRMLREA_TransferredFlg = false;
                        }
                        else {
                            $scope.HRMLREA_TransferredFlg = true;
                        }
                    }
                    else {
                        $scope.HRMLREA_TransferredFlg = false;
                    }
                }
            });
        }
        //=============masterDecative
        $scope.masterDecative = function (usersem, SweetAlert) {
            var dystring = "";
            if (usersem.hrmlreA_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (usersem.hrmlreA_ActiveFlg == false) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("masterLeavingReason/masterDecative", usersem).then(function (promise) {
                            if (promise.returnval == true) {
                                swal("Record " + dystring + "d" + " Successfully!!!");
                                $state.reload();
                            }
                            else {
                                swal("Record Not " + dystring + "d" + " Successfull!!!");
                                $state.reload();
                            }
                        })
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }
                });
        }
    }
})();