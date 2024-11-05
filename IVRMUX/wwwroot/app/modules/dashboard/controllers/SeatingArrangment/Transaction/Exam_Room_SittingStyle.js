(function () {
    'use strict';
    angular.module('app').controller('SA_ExamDate_RoomDetailsController', SA_ExamDate_RoomDetailsController)

    SA_ExamDate_RoomDetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'uiCalendarConfig', 'superCache', '$window', '$http']

    function SA_ExamDate_RoomDetailsController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, uiCalendarConfig, superCache, $window, $http) {

        $scope.ESARSITSTY_SameBranchSameSemFlg = false;
        $scope.ESARSITSTY_DiffBranchSameSemFlg = false;
        $scope.ESARSITSTY_SameBranchDiffSemFlg = false;
        $scope.ESARSITSTY_DiffBranchDiffSemFlg = false;
        $scope.ESARSITSTY_AnyBranchAnySemFlg = false;
        $scope.ESARSITSTY_Id = 0;

        $scope.GetRoomSittingStyleloaddata = function () {
            var id = 1;
            apiService.getURI("Exam_Room_Date/GetRoomSittingStyleloaddata", id).then(function (promise) {
                $scope.GetSavedDetails = promise.getRoomSittingStyleDetails;
                if ($scope.GetSavedDetails !== null && $scope.GetSavedDetails.length > 0) {
                    $scope.ESARSITSTY_SameBranchSameSemFlg = $scope.GetSavedDetails[0].esarsitstY_SameBranchSameSemFlg;
                    $scope.ESARSITSTY_DiffBranchSameSemFlg = $scope.GetSavedDetails[0].esarsitstY_DiffBranchSameSemFlg;
                    $scope.ESARSITSTY_SameBranchDiffSemFlg = $scope.GetSavedDetails[0].esarsitstY_SameBranchDiffSemFlg;
                    $scope.ESARSITSTY_DiffBranchDiffSemFlg = $scope.GetSavedDetails[0].esarsitstY_DiffBranchDiffSemFlg;
                    $scope.ESARSITSTY_AnyBranchAnySemFlg = $scope.GetSavedDetails[0].esarsitstY_AnyBranchAnySemFlg;
                    $scope.ESARSITSTY_Id = $scope.GetSavedDetails[0].esarsitstY_Id;
                }
            });
        };

        $scope.SaveRoomSittingStyle = function () {

            var count = 0;
            if ($scope.ESARSITSTY_SameBranchSameSemFlg === false && $scope.ESARSITSTY_DiffBranchSameSemFlg === false
                && $scope.ESARSITSTY_SameBranchDiffSemFlg === false && $scope.ESARSITSTY_DiffBranchDiffSemFlg === false
                && $scope.ESARSITSTY_AnyBranchAnySemFlg === false) {
                count = 1;
            }

            if (count === 0) {
                var data = {
                    "ESARSITSTY_Id": $scope.ESARSITSTY_Id,
                    "ESARSITSTY_SameBranchSameSemFlg": $scope.ESARSITSTY_SameBranchSameSemFlg,
                    "ESARSITSTY_DiffBranchSameSemFlg": $scope.ESARSITSTY_DiffBranchSameSemFlg,
                    "ESARSITSTY_SameBranchDiffSemFlg": $scope.ESARSITSTY_SameBranchDiffSemFlg,
                    "ESARSITSTY_DiffBranchDiffSemFlg": $scope.ESARSITSTY_DiffBranchDiffSemFlg,
                    "ESARSITSTY_AnyBranchAnySemFlg": $scope.ESARSITSTY_AnyBranchAnySemFlg
                };

                apiService.create("Exam_Room_Date/SaveRoomSittingStyle", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.message === "Add") {
                            if (promise.returnval === true) {
                                swal("Record Saved Successfully");
                            } else {
                                swal("Failed To Save Record");
                            }
                        } else if (promise.message === "Update") {
                            if (promise.returnval === true) {
                                swal("Record Updated Successfully");
                            } else {
                                swal("Failed To Update Record");
                            }
                        } else {
                            swal("Failed To Save/Update Record");
                        }
                        $scope.cleardata();
                    }
                });

            } else {
                swal("Select Atleast One Check Box To Save The Details");
            }
        };

        $scope.cleardata = function () {
            $scope.ESARSITSTY_SameBranchSameSemFlg = false;
            $scope.ESARSITSTY_DiffBranchSameSemFlg = false;
            $scope.ESARSITSTY_SameBranchDiffSemFlg = false;
            $scope.ESARSITSTY_DiffBranchDiffSemFlg = false;
            $scope.ESARSITSTY_AnyBranchAnySemFlg = false;
            $scope.ESARSITSTY_Id = 0;
            $scope.GetRoomSittingStyleloaddata();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.isOptionsRequired1 = function () {
            return !$scope.Getroomdetails.some(function (options) {
                return options.checked;
            });
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };
    }
})();