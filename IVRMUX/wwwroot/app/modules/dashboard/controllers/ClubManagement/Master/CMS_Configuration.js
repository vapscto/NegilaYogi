(function () {
    'use strict';
    angular
        .module('app')
        .controller('CMS_ConfigurationController', CMS_ConfigurationController)
    CMS_ConfigurationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$stateParams', '$filter', 'superCache', '$q']
    function CMS_ConfigurationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $stateParams, $filter, superCache, $q) {
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        
        $scope.coptyright = copty;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.search = "";
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("CMS_MasterDepartment/loaddataconfigure", pageid).then(function (promise) {
                $scope.getreport = promise.getreport;
                $scope.presentCountgrid = $scope.getreport.length;
              

            })
        };
        $scope.submitted = false;
        $scope.savepages = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var CMSCON_Id = 0;
                if ($scope.CMSCON_Id > 0) {
                    CMSCON_Id = $scope.CMSCON_Id;
                }
                var data = {
                    "CMSCON_Id": CMSCON_Id,
                    "CMSCON_ApplicationApplFlg": $scope.CMSCON_ApplicationApplFlg,
                    "CMSCON_DiscountApplFlg": $scope.CMSCON_DiscountApplFlg,
                    "CMSCON_BOMFlg": $scope.CMSCON_BOMFlg,
                    "CMSCON_CategorywiseFlg": $scope.CMSCON_CategorywiseFlg,
                    "CMSCON_CreditFlg": $scope.CMSCON_CreditFlg,
                    "CMSCON_IncentiveApplFlg": $scope.CMSCON_IncentiveApplFlg,
                    "CMSCON_TaxApplFlg": $scope.CMSCON_TaxApplFlg,
                    "CMSCON_PayLateFeeInterestFlg": $scope.CMSCON_PayLateFeeInterestFlg,
                    "CMSCON_InterestPercent": $scope.CMSCON_InterestPercent,
                    "CMSCON_MaxNoDependent": $scope.CMSCON_MaxNoDependent,
                    "CMSCON_NoOfProposal": $scope.CMSCON_NoOfProposal,
                    "CMSCON_AllowNonMemberCreditTransFlg": $scope.CMSCON_AllowNonMemberCreditTransFlg,
                    "CMSCON_CoverChargeAmtFlg": $scope.CMSCON_CoverChargeAmtFlg

                }
                apiService.create("CMS_MasterDepartment/saveconfigure", data).
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
                        else if (promise.returnval == "configure") {
                            swal('Already Configure !');
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

                "CMSCON_Id": item.cmscoN_Id
            }
            var dystring = "";
            if (item.cmscoN_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.cmscoN_ActiveFlag == false) {
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
                        apiService.create("CMS_MasterDepartment/confdeactive", data).
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
            $scope.CMSCON_Id = user.cmscoN_Id;
            $scope.CMSCON_ApplicationApplFlg = user.cmscoN_ApplicationApplFlg;
            $scope.CMSCON_DiscountApplFlg = user.cmscoN_DiscountApplFlg;
            $scope.CMSCON_BOMFlg = user.cmscoN_BOMFlg;
            $scope.CMSCON_BOMFlg = user.cmscoN_BOMFlg;
            $scope.CMSCON_CategorywiseFlg = user.cmscoN_CategorywiseFlg;
            $scope.CMSCON_CategorywiseFlg = user.cmscoN_CategorywiseFlg;
            $scope.CMSCON_CreditFlg = user.cmscoN_CreditFlg;
            $scope.CMSCON_IncentiveApplFlg = user.cmscoN_IncentiveApplFlg;
            $scope.CMSCON_TaxApplFlg = user.cmscoN_TaxApplFlg;
            $scope.CMSCON_PayLateFeeInterestFlg = user.cmscoN_PayLateFeeInterestFlg;
            $scope.CMSCON_InterestPercent = user.cmscoN_InterestPercent;
            $scope.CMSCON_MaxNoDependent = user.cmscoN_MaxNoDependent;
            $scope.CMSCON_NoOfProposal = user.cmscoN_NoOfProposal;
            $scope.CMSCON_AllowNonMemberCreditTransFlg = user.cmscoN_AllowNonMemberCreditTransFlg;
            $scope.CMSCON_CoverChargeAmtFlg = user.cmscoN_CoverChargeAmtFlg;
        };
        $scope.clear = function () {
            $state.reload();
        }
    }

})();