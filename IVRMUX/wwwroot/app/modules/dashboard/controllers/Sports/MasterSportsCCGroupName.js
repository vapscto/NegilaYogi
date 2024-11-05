(function () {
    'use strict';

    angular
        .module('app')
        .controller('MasterSportsCCGroupName', MasterSportsCCGroupName);

    MasterSportsCCGroupName.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];

    function MasterSportsCCGroupName($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.searchValue = "";
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.loadgrid = function () {
            apiService.getURI("MasterSportsCCGroup/loadgrid/", 1).then(function (promise) {
                if (promise.count > 0) {
                    $scope.groupNameList = promise.groupNameList;
                    $scope.presentCountgrid = $scope.groupNameList.length;
                }
                $scope.cancel();
            });
        }
        $scope.submitted = false;
        $scope.saveRecord = function () {
            if ($scope.myForm.$valid) {
                $scope.temp = [];
                if ($scope.materaldocuupload != null && $scope.materaldocuupload.length > 0) {
                    angular.forEach($scope.materaldocuupload, function (opq) {
                        if (opq.SPCCMSCCG_Under != null && opq.SPCCMSCCG_Under !="") {
                            $scope.temp.push({
                                SPCCMSCCG_SportsCCGroupName: opq.SPCCMSCCG_Under,
                                SPCCMSCCG_Level: opq.SPCCMSCCG_Level,
                                SPCCMSCCG_SportsCCGroupDesc: opq.SportsCCGroupDesc
                            })
                        }
                       
                    });

                }
                var obj = {
                    "SPCCMSCCG_Id": $scope.SPCCMSCCG_Id,
                    "SPCCMSCCG_SportsCCGroupName": $scope.SPCCMSCCG_SportsCCGroupName,
                    "SPCCMSCCG_SportsCCGroupDesc": $scope.SPCCMSCCG_SportsCCGroupDesc,
                    "SPCCMSCCG_SCCFlag": $scope.SPCCMSCCG_SCCFlag,
                    "tempDatas": $scope.temp,
                }
                apiService.create("MasterSportsCCGroup/saveRecord", obj).then(function (promise) {
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
            $scope.SPCCMSCCG_Id = data;
            apiService.getURI("MasterSportsCCGroup/Edit/", $scope.SPCCMSCCG_Id).then(function (promise) {
                $scope.editDetails = promise.editDetails;
                $scope.SPCCMSCCG_SportsCCGroupName = promise.editDetails[0].spccmsccG_SportsCCGroupName
                $scope.SPCCMSCCG_SportsCCGroupDesc = promise.editDetails[0].spccmsccG_SportsCCGroupDesc;
                $scope.SPCCMSCCG_SCCFlag = promise.editDetails[0].spccmsccG_SCCFlag
            });
        }


        /////===============================================Deactive
        //$scope.deactive = function (data) {

        //    if (data.spccmsccG_ActiveFlag == false) {
        //        var obj = {
        //            "SPCCMSCCG_Id": data.spccmsccG_Id,
        //            "SPCCMSCCG_ActiveFlag": true
        //        }
        //    }
        //    else if (data.spccmsccG_ActiveFlag == true) {
        //        var obj = {
        //            "SPCCMSCCG_Id": data.spccmsccG_Id,
        //            "SPCCMSCCG_ActiveFlag": false
        //        }
        //    }
        //    apiService.create("MasterSportsCCGroup/deactivate/", obj).then(function (promise) {
        //        if (promise.returnVal != '' && promise != null) {
        //            swal(promise.returnVal);
        //            $scope.loadgrid();
        //        }
        //        else {
        //            swal("Something went wrong");
        //        }
        //    });

        //}

        //=================Activation/Deactivation--Record.........
        $scope.deactive = function (user, SweetAlert) {
            debugger;
            $scope.SPCCMSCCG_Id = user.SPCCMSCCG_Id;

            var dystring = "";
            if (user.spccmsccG_ActiveFlag == 1) {
                dystring = "Deactivate";
            }
            else if (user.spccmsccG_ActiveFlag == 0) {
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
                        apiService.create("MasterSportsCCGroup/deactivate", user).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d Successfully!!!");

                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");

                                }
                                $state.reload();
                            })

                    }
                    else {
                        swal("Record " + dystring + " Cancelled");
                    }

                });
        }
        //================End----Activation/Deactivation--Record........
        $scope.cancel = function () {
            $scope.SPCCMSCCG_Id = 0;
            $scope.SPCCMSCCG_SportsCCGroupName = "";
            $scope.SPCCMSCCG_SportsCCGroupDesc = "";
            $scope.SPCCMSCCG_SCCFlag = "";
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.materaldocuupload = [{ id: 'materal' }];
        $scope.addgrnrows = function () {
            var newItemNo = $scope.materaldocuupload.length + 1;
            if (newItemNo <= 10) {
                $scope.materaldocuupload.push({ 'id': 'Materal' + newItemNo });
            }
        };
        $scope.removegrnrows = function (index) {
            var newItemNo = $scope.materaldocuupload.length - 1;
            $scope.materaldocuupload.splice(index, 1);


        };

    }
})();
