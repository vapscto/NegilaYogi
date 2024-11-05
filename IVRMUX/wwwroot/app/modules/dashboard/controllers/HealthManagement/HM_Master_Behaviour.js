(function () {
    'use strict';
    angular.module('app').controller('Hm_master_behaviousController', Hm_master_behaviousController)
    Hm_master_behaviousController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', 'superCache', '$stateParams', '$filter']
    function Hm_master_behaviousController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, superCache, $stateParams, $filter) {





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
            apiService.getURI("HM_Master/load_MB", data).then(function (promise) {
                if (promise.behaviourlist !== null || promise.behaviourlist>0) {
                    $scope.behaviourlist = promise.behaviourlist;
                    
                }
            });
        };

        $scope.savedata = function () {
            $scope.submitted = false;
            if ($scope.myForm.$valid) {
                   var data = {
                       "HMMBEH_Id": $scope.HMMBEH_Id,
                       "HMMBEH_BehaviourName": $scope.HMMBEH_BehaviourName,
                       "HMMBEH_BehaviourDesc": $scope.HMMBEH_BehaviourDesc
                   
                };
                apiService.create("HM_Master/Save_MB", data).then(function (promise) {
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
                "HMMBEH_Id": obj.hmmbeH_Id
            };
            apiService.create("HM_Master/Edit_MB", data).then(function (promise) {
                if (promise.behaviour_edit !== null && promise.behaviour_edit.length > 0) {
                    $scope.HMMBEH_Id = promise.behaviour_edit[0].hmmbeH_Id;
                    $scope.HMMBEH_BehaviourName = promise.behaviour_edit[0].hmmbeH_BehaviourName;
                    $scope.HMMBEH_BehaviourDesc = promise.behaviour_edit[0].hmmbeH_BehaviourDesc;
                                 }
            });
        };

        $scope.ActiveDeactive = function (user, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";

            if (user.hmmbeH_ActiveFlg === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";
            }

            var data = {
                "HMMBEH_Id": user.hmmbeH_Id,
                "HMMBEH_ActiveFlg": user.hmmbeH_ActiveFlg
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
                        apiService.create("HM_Master/ActiveDeactive_MB", data).then(function (promise) {
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


