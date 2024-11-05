(function () {
    'use strict';
    angular
        .module('app')
        .controller('CMS_Master_InstallmentTypeController', CMS_Master_InstallmentTypeController)
    CMS_Master_InstallmentTypeController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$stateParams', '$filter', 'superCache', '$q']
    function CMS_Master_InstallmentTypeController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $stateParams, $filter, superCache, $q) {
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("CMS_Master_InstallmentType/loaddata", pageid).then(function (promise) {
                $scope.getreport = promise.pages;
                // $scope.MonthArray = promise.monthArray;
            })
        };
        $scope.submitted = false;
        $scope.savepages = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var CMSMINSTTY_Id = 0;
                if ($scope.CMSMINSTTY_Id > 0) {
                    CMSMINSTTY_Id = $scope.CMSMINSTTY_Id;
                }
                var data = {
                    "CMSMINSTTY_Id": CMSMINSTTY_Id,
                    "CMSMINSTTY_InstallmentTypeFlg": $scope.CMSMINSTTY_InstallmentTypeFlg,
                    "CMSMINSTTY_InstallmentType": $scope.CMSMINSTTY_InstallmentType,
                    "CMSMINSTTY_Duration": $scope.CMSMINSTTY_Duration,
                    "CMSMINSTTY_DurationFlg": $scope.CMSMINSTTY_DurationFlg

                }
                apiService.create("CMS_Master_InstallmentType/savedata", data).
                    then(function (promise) {

                        if (promise.returnval == "save") {
                            swal('Record Saved Successfully !');

                        }
                        else if (promise.returnval == "Notsave") {
                            swal('Records Not Saved !');
                        }
                        else if (promise.returnval == "update") {
                            swal('Records Updated Successfully !');

                        }
                        else if (promise.returnval == "Notupdate") {
                            swal('Records Not Updated  !');

                        }
                        else if (promise.returnval == "exit") {
                            swal('Records Already Exist !');
                        }
                        else if (promise.returnval == "admin") {
                            swal('Please Contact  Administrator  !');
                        }
                        $state.reload();
                    })
            }
        };
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        //
        //Deactive
        $scope.Deletedata = function (item, SweetAlert) {
            var data = {
                "CMSMINSTTY_Id": item.cmsminsttY_Id
            }
            var dystring = "";
            if (item.cmsminsttY_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.cmsminsttY_ActiveFlag == false) {
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
                        apiService.create("CMS_Master_InstallmentType/deactive", data).
                            then(function (promise) {
                                if (promise.returnval == "active") {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else if (promise.returnval == "notactive") {
                                    swal("Record Not  Active / Deactive  !!!");
                                }
                                else if (promise.returnval == "admin") {
                                    swal('Please Contact  Administrator  !');
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        }

        $scope.edit = function (user) {
            $scope.CMSMINSTTY_Id = user.cmsminsttY_Id;
            $scope.CMSMINSTTY_InstallmentTypeFlg = user.cmsminsttY_InstallmentTypeFlg;
            $scope.CMSMINSTTY_InstallmentType = user.cmsminsttY_InstallmentType;
            $scope.CMSMINSTTY_Duration = user.cmsminsttY_Duration;
            $scope.CMSMINSTTY_DurationFlg = user.cmsminsttY_DurationFlg;
        };
        $scope.clear = function () {
            $state.reload();
        }
    }
})();