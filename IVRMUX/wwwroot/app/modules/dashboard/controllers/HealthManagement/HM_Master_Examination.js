(function () {
    'use strict';
    angular.module('app').controller('Hm_master_examinationController', Hm_master_examinationController)
    Hm_master_examinationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', 'superCache', '$stateParams', '$filter']
    function Hm_master_examinationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, superCache, $stateParams, $filter) {





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
            apiService.getURI("HM_Master/load_EX", data).then(function (promise) {
                if (promise.examinationlist !== null || promise.examinationlist > 0) {
                    $scope.examinationlist = promise.examinationlist;

                }
            });
        };

        $scope.savedata = function () {
            $scope.submitted = false;
            if ($scope.myForm.$valid) {
                var data = {
                    "HMMEXM_Id": $scope.HMMEXM_Id,
                    "HMMEXM_ExaminationName": $scope.HMMEXM_ExaminationName,
                    "HMMEXM_ExamDesc": $scope.HMMEXM_ExamDesc

                };
                apiService.create("HM_Master/Save_EX", data).then(function (promise) {
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
                "HMMEXM_Id": obj.hmmexM_Id
            };
            apiService.create("HM_Master/Edit_EX", data).then(function (promise) {
                if (promise.examination_edit !== null && promise.examination_edit.length > 0) {
                    $scope.HMMEXM_Id = promise.examination_edit[0].hmmexM_Id;
                    $scope.HMMEXM_ExaminationName = promise.examination_edit[0].hmmexM_ExaminationName;
                    $scope.HMMEXM_ExamDesc = promise.examination_edit[0].hmmexM_ExamDesc;
                }
            });
        };

        $scope.ActiveDeactive = function (user, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";

            if (user.hmmexM_ActiveFlg === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";
            }

            var data = {
                "HMMEXM_Id": user.hmmexM_Id,
                "HMMEXM_ActiveFlg": user.hmmexM_ActiveFlg
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
                        apiService.create("HM_Master/ActiveDeactive_EX", data).then(function (promise) {
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


