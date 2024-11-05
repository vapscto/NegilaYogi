(function () {
    'use strict';
    angular.module('app').controller('Hm_MasterDdoctorController', Hm_MasterDdoctorController)
    Hm_MasterDdoctorController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', 'superCache', '$stateParams', '$filter']
    function Hm_MasterDdoctorController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, superCache, $stateParams, $filter) {





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
            apiService.getURI("HM_Master/load_DC", data).then(function (promise) {
                if (promise.doctorlist !== null || promise.doctorlist > 0) {
                    $scope.doctorlist = promise.doctorlist;

                }
            });
        };

        $scope.savedata = function () {
            $scope.submitted = false;
            if ($scope.myForm.$valid) {
                var data = {
                    "HMMDOC_Id": $scope.HMMDOC_Id,
                    "HMMDOC_DoctorName": $scope.HMMDOC_DoctorName,
                    "HMMDOC_DoctorQualification": $scope.HMMDOC_DoctorQualification,
                    "HMMDOC_Specialisation": $scope.HMMDOC_Specialisation,
                    "HMMDOC_Address": $scope.HMMDOC_Address,
                    "HMMDOC_Phoneno": $scope.HMMDOC_Phoneno,
                    "HMMDOC_EmailId": $scope.HMMDOC_EmailId,
                    "HMMDOC_BloodGroup": $scope.HMMDOC_BloodGroup,

                };
                apiService.create("HM_Master/Save_DC", data).then(function (promise) {
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
                "HMMDOC_Id": obj.hmmdoC_Id
            };
            apiService.create("HM_Master/Edit_DC", data).then(function (promise) {
                if (promise.doctor_edit !== null && promise.doctor_edit.length > 0) {
                    $scope.HMMDOC_Id = promise.doctor_edit[0].hmmdoC_Id;
                    $scope.HMMDOC_DoctorName = promise.doctor_edit[0].hmmdoC_DoctorName;
                    $scope.HMMDOC_DoctorQualification = promise.doctor_edit[0].hmmdoC_DoctorQualification;
                    $scope.HMMDOC_Specialisation = promise.doctor_edit[0].hmmdoC_Specialisation;
                    $scope.HMMDOC_Address = promise.doctor_edit[0].hmmdoC_Address;
                    $scope.HMMDOC_Phoneno = promise.doctor_edit[0].hmmdoC_Phoneno;
                    $scope.HMMDOC_EmailId = promise.doctor_edit[0].hmmdoC_EmailId;
                    $scope.HMMDOC_BloodGroup = promise.doctor_edit[0].hmmdoC_BloodGroup;
                }
            });
        };

        $scope.ActiveDeactive = function (user, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";

            if (user.hmmdoC_ActiveFlg === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";
            }

            var data = {
                "HMMDOC_Id": user.hmmdoC_Id,
                "HMMDOC_ActiveFlg": user.hmmdoC_ActiveFlg
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
                        apiService.create("HM_Master/ActiveDeactive_DC", data).then(function (promise) {
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


