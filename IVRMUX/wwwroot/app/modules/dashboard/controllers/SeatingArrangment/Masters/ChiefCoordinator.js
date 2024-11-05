(function () {
    'use strict';
    angular
        .module('app')
        .controller('ChiefCoordinatorController', ChiefCoordinatorController)

    ChiefCoordinatorController.$inject = ['$rootScope','$http', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'uiCalendarConfig', 'superCache', '$window']

    function ChiefCoordinatorController($rootScope, $http, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, uiCalendarConfig, superCache, $window) {

        $scope.tempcldrlst = [];



        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.search = "";

        $scope.loaddata = function () {
            var id = 1;
            apiService.getURI("SAMasterSuperintendent/load_CC", id).then(function (promise) {
                $scope.yearlst = promise.yearlst;
                $scope.examlist = promise.examlist;
                $scope.university_examlist = promise.university_examlist;
                $scope.chiefcoordinatorlist = promise.chiefcoordinatorlist;

            })

        };

        //==============save data==========
        $scope.saveData = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "ESACHCRD_Id": $scope.ESACHCRD_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "EME_Id": $scope.EME_Id,
                    "ESAUE_Id": $scope.ESAUE_Id,
                    "ESACHCRD_ChiefCordName": $scope.ESACHCRD_ChiefCordName,
                    "ESACHCRD_Add1": $scope.ESACHCRD_Add1,
                    "ESACHCRD_Add2": $scope.ESACHCRD_Add2,
                    "ESACHCRD_Add3": $scope.ESACHCRD_Add3,
                    "ESACHCRD_Add4": $scope.ESACHCRD_Add4,
                    "ESACHCRD_Add5": $scope.ESACHCRD_Add5,
                    "ESACHCRD_Add6": $scope.ESACHCRD_Add6,
                }
                apiService.create("SAMasterSuperintendent/Save_CC", data).then(function (promise) {
                    if (promise.message == "Add") {
                        swal('Record Save Successfully.');
                    }
                    else if (promise.message == "Update") {
                        swal('Record Update Successfully.');
                    }
                    if (promise.message == "Error") {
                        swal('Record Save/Update Failed..!!.');
                    }
                    $state.reload();
                })
            }
            else {
                $scope.submitted = true;
            }
        }
        $scope.cleardata = function () {
            $state.reload();
        }
        //==============edit data========
        $scope.Edit = function (obj) {

            var data = {
                "ESACHCRD_Id": obj.ESACHCRD_Id,
                "ASMAY_Id": obj.ASMAY_Id
            };
            apiService.create("SAMasterSuperintendent/Edit_CC", data).then(function (promise) {
                if (promise.edit_cclist !== null && promise.edit_cclist.length > 0) {
                    $scope.ESACHCRD_Id = promise.edit_cclist[0].esachcrD_Id;
                    $scope.ASMAY_Id = promise.edit_cclist[0].asmaY_Id;
                    $scope.EME_Id = promise.edit_cclist[0].emE_Id;
                    $scope.ESAUE_Id = promise.edit_cclist[0].esauE_Id;
                    $scope.ESACHCRD_ChiefCordName = promise.edit_cclist[0].esachcrD_ChiefCordName;
                    $scope.ESACHCRD_Add1 = promise.edit_cclist[0].esachcrD_Add1;
                    $scope.ESACHCRD_Add2 = promise.edit_cclist[0].esachcrD_Add2;
                    $scope.ESACHCRD_Add3 = promise.edit_cclist[0].esachcrD_Add3;
                    $scope.ESACHCRD_Add4 = promise.edit_cclist[0].esachcrD_Add4;
                    $scope.ESACHCRD_Add5 = promise.edit_cclist[0].esachcrD_Add5;
                    $scope.ESACHCRD_Add6 = promise.edit_cclist[0].esachcrD_Add6;

                    $scope.yearlst = promise.yearlst;
                    $scope.examlist = promise.examlist;
                    $scope.university_examlist = promise.university_examlist;

                }
            });
        };

        //===============active/deactive data============
        $scope.deactive = function (user, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";

            if (user.ESACHCRD_ActiveFlg === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";
            }

            var data = {
                "ESACHCRD_Id": user.ESACHCRD_Id,
                "ESACHCRD_ActiveFlg": user.ESACHCRD_ActiveFlg
            };

            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("SAMasterSuperintendent/ActiveDeactive_CC", data).then(function (promise) {
                            if (promise.message === 'true') {
                                swal("Record Activated Successfully");
                            }
                            else if (promise.message === 'false') {
                                swal("Record De-Activated Successfully");
                            } else if (promise.message === 'error') {
                                swal("Operation Failed..!!");
                            }
                            $state.reload();
                        });

                    }

                });
        };
        $scope.interacted = function () {
            return $scope.submitted;
        }







    };
})();