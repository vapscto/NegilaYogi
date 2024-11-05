
(function () {
    'use strict';
    angular
.module('app')
        .controller('ExpirySettingsController', ExpirySettingsController)

    ExpirySettingsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function ExpirySettingsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        $scope.sortKey = 'trmA_Id';
        $scope.sortReverse = true;
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.searchValue = "";
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("ExpirySettings/getdata", pageid).then(function (promise) {
                if (promise != null) {
                    if (promise.getdatadetails.length > 0) {
                        $scope.getdatadetails = promise.getdatadetails;
                        $scope.TRC_Id = $scope.getdatadetails[0].trC_Id
                        $scope.TRC_DLExpReminderDays = $scope.getdatadetails[0].trC_DLExpReminderDays
                        $scope.TRC_EmmisionExpDays = $scope.getdatadetails[0].trC_EmmisionExpDays
                        $scope.TRC_TaxExpDays = $scope.getdatadetails[0].trC_TaxExpDays
                        $scope.TRC_FitnessExpDays = $scope.getdatadetails[0].trC_FitnessExpDays
                        $scope.TRC_SpeedControlDays = $scope.getdatadetails[0].trC_SpeedControlDays
                        $scope.TRC_CeaseFireDays = $scope.getdatadetails[0].trC_CeaseFireDays
                        $scope.TRC_InsuranceDays = $scope.getdatadetails[0].trC_InsuranceDays
                        $scope.TRC_GreenTaxDays = $scope.getdatadetails[0].trC_GreenTaxDays
                        $scope.TRC_PermitDays = $scope.getdatadetails[0].trC_PermitDays



                        if ($scope.TRC_DLExpReminderDays == 0) {
                            $scope.TRC_DLExpReminderDays = '';
                        }

                        if ($scope.TRC_EmmisionExpDays == 0) {
                            $scope.TRC_EmmisionExpDays = '';
                        }
                        if ($scope.TRC_TaxExpDays == 0) {
                            $scope.TRC_TaxExpDays = '';
                        }
                        if ($scope.TRC_FitnessExpDays == 0) {
                            $scope.TRC_FitnessExpDays = '';

                        }
                        if ($scope.TRC_SpeedControlDays == 0) {
                            $scope.TRC_SpeedControlDays = '';
                        }
                        if ($scope.TRC_CeaseFireDays == 0) {
                            $scope.TRC_CeaseFireDays = '';

                        }
                        if ($scope.TRC_InsuranceDays == 0) {
                            $scope.TRC_InsuranceDays = '';
                        }
                        if ($scope.TRC_GreenTaxDays == 0) {
                            $scope.TRC_GreenTaxDays = '';

                        }
                        if ($scope.TRC_PermitDays == 0) {
                            $scope.TRC_PermitDays = '';
                        }
                    }
                    else {
                        swal("Not Saved any Parameter")
                    }
                }
                else {
                    swal("Error")
                }
            })

        }

        $scope.submitted = false;
      

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
  
        //-------Save Data-------//
        $scope.savedata = function () {

            if ($scope.TRC_DLExpReminderDays =='') {
                $scope.TRC_DLExpReminderDays = 0;
            }

            if ($scope.TRC_EmmisionExpDays == '') {
                $scope.TRC_EmmisionExpDays = 0;
            }
            if ($scope.TRC_TaxExpDays == '') {
                $scope.TRC_TaxExpDays = 0;
            }
            if ($scope.TRC_FitnessExpDays == '') {
                $scope.TRC_FitnessExpDays = 0;

            }
            if ($scope.TRC_SpeedControlDays == '') {
                $scope.TRC_SpeedControlDays = 0;
            }
            if ($scope.TRC_CeaseFireDays == '') {
                $scope.TRC_CeaseFireDays = 0;

            }
            if ($scope.TRC_InsuranceDays == '') {
                $scope.TRC_InsuranceDays = 0;
            }
            if ($scope.TRC_GreenTaxDays == '') {
                $scope.TRC_GreenTaxDays = 0;

            }
            if ($scope.TRC_PermitDays == '') {
                $scope.TRC_PermitDays = 0;
            }
            

                var data = {
                    "TRC_Id": $scope.TRC_Id,
                    "TRC_DLExpReminderDays": $scope.TRC_DLExpReminderDays,
                    "TRC_EmmisionExpDays": $scope.TRC_EmmisionExpDays,
                    "TRC_TaxExpDays": $scope.TRC_TaxExpDays,
                    "TRC_FitnessExpDays": $scope.TRC_FitnessExpDays,
                    "TRC_SpeedControlDays": $scope.TRC_SpeedControlDays,
                    "TRC_CeaseFireDays": $scope.TRC_CeaseFireDays,
                    "TRC_InsuranceDays": $scope.TRC_InsuranceDays,
                    "TRC_GreenTaxDays": $scope.TRC_GreenTaxDays,
                    "TRC_PermitDays": $scope.TRC_PermitDays,
                }
                apiService.create("ExpirySettings/savedata", data).then(function (promise) {
                    if (promise != null) {
                        if (promise.message == "Add") {
                            if (promise.retrval == true) {
                                swal("Record Saved Successfully");
                            }
                            else {
                                swal("Failed To Save Record");
                            }
                        }
                        else if (promise.message == "Update") {
                            if (promise.retrval == true) {
                                swal("Record Update Successfully");
                            }
                            else {
                                swal("Failed To Update Record");
                            }
                        }
                        else if (promise.message == "Duplicate") {
                            swal("Record Already Exists");
                        }
                    }
                    else {

                    }
                    $state.reload();
                })
                     
        }
       
        //--Cancel--//
        $scope.cancle = function () {
            $scope.TRMA_Id = 0;
            $state.reload();
        }

        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            
            return (angular.lowercase(obj.trmA_AreaName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.trmA_AliasName)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }

        //---Edit Data--//
        $scope.edit = function (user) {
            var data = {
                "TRMA_Id":user.trmA_Id
            }
            apiService.create("ExpirySettings/geteditdata", data).then(function (Promise) {
                if (Promise != null) {
                    $scope.aliasname = Promise.geteditdataarea[0].trmA_AliasName;
                    $scope.Zoneareaname = Promise.geteditdataarea[0].trmA_AreaName;
                    $scope.TRMA_Id = Promise.geteditdataarea[0].trmA_Id;
                }
            })
        }

        //--Active Deactive---//
        $scope.deactive = function (user, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (user.trmA_ActiveFlg === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";

            }
            else {

                mgs = "Active";
                confirmmgs = "Activated";

            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
        function (isConfirm) {
            if (isConfirm) {
                apiService.create("ExpirySettings/activedeactive/", user).
                then(function (promise) {
                    if (promise.message != null) {
                        swal(promise.message);
                    }
                    else {
                        if (promise.returnval == true) {
                            swal(confirmmgs + " " + "Successfully.");
                            $state.reload();
                        }
                        else {
                            swal(confirmmgs + " " + " Successfully");
                            $state.reload();
                        }
                    }
                })
            }
            else {
                swal("Record " + mgs + " Cancelled");
            }
            $state.reload();
        });
        }
 
        //--Sorting--//     
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        
    };
})();


