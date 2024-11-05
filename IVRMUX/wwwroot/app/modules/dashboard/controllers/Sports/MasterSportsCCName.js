(function () {
    'use strict';

    angular
        .module('app')
        .controller('MasterSportsCCName', MasterSportsCCName);

    MasterSportsCCName.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];

    function MasterSportsCCName($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.searchValue = "";
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.usercheckhouss23 = false;
        $scope.SPCCMSCC_MultiAttemptFlg = false;
        $scope.obj = {};
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.loadgrid = function () {
            apiService.getURI("MasterSportsCCName/loadgrid/", 1).then(function (promise) {
                $scope.gropList = promise.gropList;
                $scope.gropListtwo = promise.gropList;
                if (promise.count > 0) {
                    $scope.sportsCCNameList = promise.sportsCCNameList;
                    $scope.presentCountgrid = $scope.sportsCCNameList.length;
                }
                //$scope.cancel();
            });
        }
        $scope.submitted = false;
        $scope.saveRecord = function () {
            if ($scope.myForm.$valid) {
                $scope.temp = [];
                if ($scope.Budgetupload != null && $scope.Budgetupload.length > 0) {
                    angular.forEach($scope.Budgetupload, function (opq) {
                        if (opq.SPCCMSCCG_Under != null && opq.SPCCMSCCG_Under != "") {
                            if ($scope.SPCCMSCC_Id > 0) {
                                $scope.temp.push({
                                    SPCCMSCCG_UnderEvent: opq.SPCCMSCCG_Under,
                                    SPCCMSCCG_Id: opq.spccmsccG_Id,

                                })
                            }
                            else {
                                $scope.temp.push({
                                    SPCCMSCCG_UnderEvent: opq.SPCCMSCCG_Under
                                })
                            }
                            
                        }
                    });
                    
                }

                //if ($scope.gropListtwo != null && $scope.gropListtwo.length > 0) {
                //    if ($scope.SPCCMSCC_SGFlag === "Group") {
                //        angular.forEach($scope.gropListtwo, function (opq) {
                //            if (opq.housselectt23 === true) {
                //                $scope.temp.push({
                //                    SPCCMSCCG_Under: opq.spccmsccG_Id
                //                })
                //            }
                //        });
                //    }
                //    else if ($scope.SPCCMSCC_SGFlag === "Single") {
                //        if ($scope.obj.SPCCMSCCG_Under > 0) {
                //            $scope.temp.push({
                //                SPCCMSCCG_Under: $scope.obj.SPCCMSCCG_Under,
                //                SPCCMSCCG_Level:1
                //            })
                //        }
                //    }

                //}
                if ($scope.SPCCMSCC_MultiAttemptFlg == false) {
                    $scope.obj.spccmscC_NoOfAttempts = null;
                }
                var obj = {
                    "SPCCMSCC_Id": $scope.SPCCMSCC_Id,
                    "SPCCMSCCG_Id": $scope.SPCCMSCCG_Id,
                    "SPCCMSCC_SportsCCName": $scope.SPCCMSCC_SportsCCName,
                    "SPCCMSCC_SportsCCDesc": $scope.SPCCMSCC_SportsCCDesc,
                    "SPCCMSCC_SGFlag": $scope.SPCCMSCC_SGFlag,
                    "SPCCMSCC_NoOfMembers": $scope.SPCCMSCC_NoOfMembers,
                    "SPCCMSCC_RecHighLowFlag": $scope.SPCCMSCC_RecHighLowFlag,
                    "SPCCMSCC_RecInfo": $scope.SPCCMSCC_RecInfo,
                    "SPCCMSCC_MultiAttemptFlg": $scope.SPCCMSCC_MultiAttemptFlg,
                    "SPCCMSCC_NoOfAttempts": $scope.obj.spccmscC_NoOfAttempts,
                    "tempDatas": $scope.temp,
                }
                apiService.create("MasterSportsCCName/saveRecord", obj).then(function (promise) {
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
                    $state.reload();
                });
            }
            else {
                $scope.submitted = true;
            }
        }
        $scope.edit = function (data) {
            $scope.SPCCMSCC_Id = data;
            $scope.Budgetupload = [];
            apiService.getURI("MasterSportsCCName/Edit/", $scope.SPCCMSCC_Id).then(function (promise) {
                $scope.editDetails = promise.editDetails;
                $scope.SPCCMSCCG_Id = promise.editDetails[0].spccmsccG_Id
                $scope.SPCCMSCC_SportsCCName = promise.editDetails[0].spccmscC_SportsCCName;
                $scope.SPCCMSCC_SportsCCDesc = promise.editDetails[0].spccmscC_SportsCCDesc

                $scope.SPCCMSCC_SGFlag = promise.editDetails[0].spccmscC_SGFlag;
                $scope.SPCCMSCC_NoOfMembers = promise.editDetails[0].spccmscC_NoOfMembers
                $scope.SPCCMSCC_RecHighLowFlag = promise.editDetails[0].spccmscC_RecHighLowFlag;
                $scope.SPCCMSCC_RecInfo = promise.editDetails[0].spccmscC_RecInfo;
                if (promise.editsubevent != null && promise.editsubevent.length > 0) {
                    angular.forEach(promise.editsubevent, function (dddd) {
                        dddd.SPCCMSCCG_Under = dddd.spccmsccG_SportsCCGroupName;
                        dddd.SPCCMSCCG_Id = dddd.SPCCMSCCG_Id;
                    });
                    $scope.Budgetupload = promise.editsubevent;
                }
                $scope.SPCCMSCC_MultiAttemptFlg = promise.editDetails[0].spccmscC_MultiAttemptFlg;
                $scope.obj.spccmscC_NoOfAttempts = promise.editDetails[0].spccmscC_NoOfAttempts;
                //gropList
                //if (promise.gropList != null && promise.gropList.length > 0) {
                //    if ($scope.SPCCMSCC_SGFlag === "Group") {
                //        angular.forEach($scope.gropListtwo, function (opq) {
                //            angular.forEach(promise.gropList, function (pp) {
                //                if (pp.SPCCMSCCG_Under === opq.SPCCMSCCG_Under) {
                //                    opq.housselectt23 = true;
                //                }
                //            })
                //        });
                //    }
                //    else if ($scope.SPCCMSCC_SGFlag === "Single") {
                //        $scope.obj.SPCCMSCCG_Under = promise.gropList[0].spccmsccG_Under; 
                //    }

                //}
            });
        }
        $scope.all_checkhouss23 = function () {
            var checkStatusallhous = $scope.obj.usercheckhouss23;
            angular.forEach($scope.gropList, function (hos) {
                hos.housselectt23 = checkStatusallhous;
            });


        }
        $scope.togchkbxhouses23 = function () {
            $scope.usercheckhouss23 = $scope.gropList.every(function (options) {
                return options.housselectt23;
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
            if (newuser1.spccmscC_ActiveFlag == false) {

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
                        apiService.create("MasterSportsCCName/deactivate", newuser1).
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


        $scope.materaldocuupload = [{ id: 'materal' }];
        $scope.Budgetupload = [{ id: 'materal' }];

        $scope.addBudget = function () {
            var newItemNo = $scope.Budgetupload.length + 1;
            if (newItemNo <= 50) {
                $scope.Budgetupload.push({ 'id': 'Materal' + newItemNo });
            }
        };
        $scope.removeBudget = function (index) {
            var newItemNo = $scope.Budgetupload.length - 1;
            $scope.Budgetupload.splice(index, 1);


        };
        $scope.cancel = function () {
            $scope.SPCCMSCC_Id = 0;
            $scope.SPCCMSCCG_Id = "";
            $scope.SPCCMSCC_SportsCCName = "";
            $scope.SPCCMSCC_SportsCCDesc = "";
            $scope.SPCCMSCC_SGFlag = "";
            $scope.SPCCMSCC_NoOfMembers = "";
            $scope.SPCCMSCC_RecHighLowFlag = "";
            $scope.SPCCMSCC_RecInfo = "";
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.SPCCMSCCG_Under = "";
            $scope.Budgetupload = [];
            $state.reload();
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.get_value = function () {
            debugger;
            if ($scope.SPCCMSCC_SGFlag == 'Single') {
                $scope.SPCCMSCC_NoOfMembers = 1;
            }
        }

    }
})();
