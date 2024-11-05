(function () {
    'use strict';
    angular.module('app').controller('HM_M_ObservationController', HM_M_ObservationController)
    HM_M_ObservationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', 'superCache', '$stateParams', '$filter']
    function HM_M_ObservationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, superCache, $stateParams, $filter) {





        var paginationformasters = 0;
        var copty = "";
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        } else {
            paginationformasters = 10;
        }
        $scope.search = "";


        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            var data = 2;
            apiService.getURI("HM_Master/load_OB", data).then(function (promise) {
                if (promise.observationlist !== null || promise.observationlist > 0) {
                    $scope.observationlist = promise.observationlist;

                }
            });
        };

        $scope.savedata = function () {
            $scope.submitted = false;
            if ($scope.myForm.$valid) {
                var data = {
                    "HMMOBS_Id": $scope.HMMOBS_Id,
                    "HMMOBS_Observation": $scope.HMMOBS_Observation,
                    "HMMOBS_ObservationDesc": $scope.HMMOBS_ObservationDesc

                };
                apiService.create("HM_Master/Save_OB", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.message == "Add") {
                            swal("Record Saved Successfully");
                        }
                        else if (promise.message == "Update") {
                            swal("Record Update Successfully");
                        }
                        else if (promise.message === "Error") {
                            swal("Failed To Save/Update");
                        }
                        $state.reload();
                    }
                });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.editdata = function (obj) {

            var data = {
                "HMMOBS_Id": obj.hmmobS_Id
            };
            apiService.create("HM_Master/Edit_OB", data).then(function (promise) {
                if (promise.observation_edit !== null && promise.observation_edit.length > 0) {
                    $scope.HMMOBS_Id = promise.observation_edit[0].hmmobS_Id;
                    $scope.HMMOBS_Observation = promise.observation_edit[0].hmmobS_Observation;
                    $scope.HMMOBS_ObservationDesc = promise.observation_edit[0].hmmobS_ObservationDesc;
                }
            });
        };

        $scope.ActiveDeactive = function (user, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";

            if (user.hmmobS_ActiveFlg === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";
            }

            var data = {
                "HMMOBS_Id": user.hmmobS_Id,
                "HMMOBS_ActiveFlg": user.hmmobS_ActiveFlg
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
                        apiService.create("HM_Master/ActiveDeactive_OB", data).then(function (promise) {
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

        $scope.cleardata = function () {
            $state.reload();
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };


    }
})();


