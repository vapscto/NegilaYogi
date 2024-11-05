(function () {
    'use strict';
    angular.module('app').controller('Hm_master_clenessController', Hm_master_clenessController)
    Hm_master_clenessController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', 'superCache', '$stateParams', '$filter']
    function Hm_master_clenessController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, superCache, $stateParams, $filter) {





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
            apiService.getURI("HM_Master/load_CL", data).then(function (promise) {
                if (promise.clennesslist !== null || promise.clennesslist > 0) {
                    $scope.clennesslist = promise.clennesslist;

                }
            });
        };

        $scope.savedata = function () {
            $scope.submitted = false;
            if ($scope.myForm.$valid) {
                var data = {
                    "HMMCLN_Id": $scope.HMMCLN_Id,
                    "HMMCLN_CleannessName": $scope.HMMCLN_CleannessName,
                    "HMMCLN_CleannessDesc": $scope.HMMCLN_CleannessDesc

                };
                apiService.create("HM_Master/Save_CL", data).then(function (promise) {
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
                "HMMCLN_Id": obj.hmmclN_Id
            };
            apiService.create("HM_Master/Edit_CL", data).then(function (promise) {
                if (promise.clenness_edit !== null && promise.clenness_edit.length > 0) {
                    $scope.HMMCLN_Id = promise.clenness_edit[0].hmmclN_Id;
                    $scope.HMMCLN_CleannessName = promise.clenness_edit[0].hmmclN_CleannessName;
                    $scope.HMMCLN_CleannessDesc = promise.clenness_edit[0].hmmclN_CleannessDesc;
                }
            });
        };

        $scope.ActiveDeactive = function (user, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";

            if (user.hmmclN_ActiveFlg === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";
            }

            var data = {
                "HMMCLN_Id": user.hmmclN_Id,
                "HMMCLN_ActiveFlg": user.hmmclN_ActiveFlg
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
                        apiService.create("HM_Master/ActiveDeactive_CL", data).then(function (promise) {
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


