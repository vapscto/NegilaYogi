(function () {
    'use strict';
    angular
        .module('app')
        .controller('CMS_Master_InstallmentsController', CMS_Master_InstallmentsController)
    CMS_Master_InstallmentsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$stateParams', '$filter', 'superCache', '$q']
    function CMS_Master_InstallmentsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $stateParams, $filter, superCache, $q) {
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
        $scope.CMSMINST_ApplicableDate = new Date();
        $scope.CMSMINST_FromDate = new Date();
        $scope.CMSMINST_ToDate = new Date();

        $scope.currentPage = 1;
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("CMS_Master_Installments/loaddata", pageid).then(function (promise) {
                $scope.getreport = promise.pages;
                $scope.getinstall = promise.installArray;
                $scope.MonthArray = promise.monthArray;
            })
        };
        $scope.clearfields = function () {

            $state.reload();
        }
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        $scope.edit = function (user) {
            $scope.CMSMINST_Id = user.cmsminsT_Id;
            $scope.CMSMINSTTY_Id = user.cmsminsttY_Id;
            $scope.CMSMINST_InstallmentName = user.cmsminsT_InstallmentName;
            $scope.CMSMINST_FromDate = new Date(user.cmsminsT_FromDate);
            $scope.CMSMINST_FromMonth = user.cmsminsT_FromMonth;
            $scope.CMSMINST_ToMonth = user.cmsminsT_ToMonth;            
            $scope.CMSMINST_ToDate = new Date(user.cmsminsT_ToDate);
            $scope.CMSMINST_ApplicableDate = new Date(user.cmsminsT_ApplicableDate);
            $scope.CMSMINST_ApplMonth = user.cmsminsT_ApplMonth;                                                                   
        };
        $scope.submitted = false;
        $scope.savepages = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var CMSMINST_Id = 0;
                if ($scope.CMSMINST_Id > 0) {
                    CMSMINST_Id = $scope.CMSMINST_Id;
                }
                var data = {
                    "CMSMINST_Id": CMSMINST_Id,
                    "CMSMINSTTY_Id": $scope.CMSMINSTTY_Id,
                    "CMSMINST_InstallmentName": $scope.CMSMINST_InstallmentName,
                    "CMSMINST_FromDate": new Date($scope.CMSMINST_FromDate).toDateString(),
                    "CMSMINST_FromMonth": $scope.CMSMINST_FromMonth,
                    "CMSMINST_ToDate": new Date($scope.CMSMINST_ToDate).toDateString(),
                    "CMSMINST_ToMonth": $scope.CMSMINST_ToMonth,
                    "CMSMINST_ApplicableDate": new Date($scope.CMSMINST_ApplicableDate).toDateString(),
                    "CMSMINST_ApplMonth": $scope.CMSMINST_ApplMonth
                }
                apiService.create("CMS_Master_Installments/savedata", data).
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
        //Deactive
        $scope.Deletedata = function (item, SweetAlert) {
            var data = {
                "CMSMINST_Id": item.cmsminsT_Id
            }
            var dystring = "";
            if (item.cmsminsT_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.cmsminsT_ActiveFlag == false) {
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
                        apiService.create("CMS_Master_Installments/deactive", data).
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

    }

})();