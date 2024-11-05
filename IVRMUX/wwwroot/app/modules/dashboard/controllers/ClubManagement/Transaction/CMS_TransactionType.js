(function () {
    'use strict';
    angular
        .module('app')
        .controller('CMS_TransactionTypeController', CMS_TransactionTypeController)
    CMS_TransactionTypeController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function CMS_TransactionTypeController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.editS = false;
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = 10;
            $scope.itemsPerPage = 10;
            $scope.famcomP_BookBeginingDate = new Date();         
            var id = 1;
            apiService.getURI("CMS_TransactionType/Getdetails", id).
                then(function (promise) {
                    $scope.details = promise.loadDetails;
                });   
        };
        $scope.delete = function (det, SweetAlert) {
            var data = {
                "CMSTRANSTY_Id": det.cmstranstY_Id
            }
            var dystring = "";
            if (det.cmstranstY_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (det.cmstranstY_ActiveFlag == false) {
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
                        apiService.create("CMS_TransactionType/deleteDetails", data).
                            then(function (promise) {
                                if (promise.returnval == "active") {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else if (promise.returnval == "notactive") {
                                    swal("Record Not Active / Deactive !!!");
                                }
                                else if (promise.returnval == "admin") {
                                    swal('Please Contact Administrator !');
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record " + dystring + " Cancelled!!!");
                    }
                });
        }
        $scope.edit = function (det) {
           // $scope.editS = true;
         
          
            var data = {
                "CMSTRANSTY_Id": det.cmstranstY_Id
            }

            apiService.create("CMS_TransactionType/editDetails/", data).
                then(function (promise) {
                    
                    $scope.cmstranstY_TransactionsName = promise.cmsdetails[0].cmstranstY_TransactionsName;
                    $scope.cmstranstY_AliasName = promise.cmsdetails[0].cmstranstY_AliasName;
                    $scope.cmstranstY_Amount = promise.cmsdetails[0].cmstranstY_Amount;
                   
                    $scope.cmstranstY_Id = promise.cmsdetails[0].cmstranstY_Id;
                    if (promise.cmsdetails[0].cmstranstY_AllowCreditTransFlg != null) {
                        if (promise.cmsdetails[0].cmstranstY_AllowCreditTransFlg == 0) {
                            $scope.cmstranstY_AllowCreditTransFlg = false;
                        }
                        else {
                            $scope.cmstranstY_AllowCreditTransFlg = true;
                        }
                    }
                    else {
                        $scope.cmstranstY_AllowCreditTransFlg = false;
                    }

                    if (promise.cmsdetails[0].cmstranstY_ConsiderForMinTransFlg != null) {
                        if (promise.cmsdetails[0].cmstranstY_ConsiderForMinTransFlg == 0) {
                            $scope.cmstranstY_ConsiderForMinTransFlg = false;
                        }
                        else {
                            $scope.cmstranstY_ConsiderForMinTransFlg = true;
                        }
                    }
                    else {
                        $scope.cmstranstY_ConsiderForMinTransFlg = false;
                    }


                    if (promise.cmsdetails[0].cmstranstY_CompulsoryFlg != null) {
                        if (promise.cmsdetails[0].cmstranstY_CompulsoryFlg == 0) {
                            $scope.cmstranstY_CompulsoryFlg = false;
                        }
                        else {
                            $scope.cmstranstY_CompulsoryFlg = true;
                        }
                    }
                    else {
                        $scope.cmstranstY_CompulsoryFlg = false;
                    }


                    if (promise.cmsdetails[0].cmstranstY_MemberCanChooseFlg != null) {
                        if (promise.cmsdetails[0].cmstranstY_MemberCanChooseFlg == 0) {
                            $scope.cmstranstY_MemberCanChooseFlg = false;
                        }
                        else {
                            $scope.cmstranstY_MemberCanChooseFlg = true;
                        }
                    }
                    else {
                        $scope.famcomP_SetDispFlg = false;
                    }

                    if (promise.cmsdetails[0].cmstranstY_CoverChargeFlg != null) {
                        if (promise.cmsdetails[0].cmstranstY_CoverChargeFlg == 0) {
                            $scope.cmstranstY_CoverChargeFlg = false;
                        }
                        else {
                            $scope.cmstranstY_CoverChargeFlg = true;
                        }
                    }
                    else {
                        $scope.cmstranstY_CoverChargeFlg = false;
                    }

                    if (promise.cmsdetails[0].cmstranstY_BarTransactionFlg != null) {
                        if (promise.cmsdetails[0].cmstranstY_BarTransactionFlg == 0) {
                            $scope.cmstranstY_BarTransactionFlg = false;
                        }
                        else {
                            $scope.cmstranstY_BarTransactionFlg = true;
                        }
                    }
                    else {
                        $scope.cmstranstY_BarTransactionFlg = false;
                    }

                    if (promise.cmsdetails[0].cmstranstY_FoodTransactionFlg != null) {
                        if (promise.cmsdetails[0].cmstranstY_FoodTransactionFlg == 0) {
                            $scope.cmstranstY_FoodTransactionFlg = false;
                        }
                        else {
                            $scope.cmstranstY_FoodTransactionFlg = true;
                        }
                    }
                    else {
                        $scope.cmstranstY_FoodTransactionFlg = false;
                    }

                    if (promise.cmsdetails[0].cmstranstY_CardTransactionFlg != null) {
                        if (promise.cmsdetails[0].cmstranstY_CardTransactionFlg == 0) {
                            $scope.cmstranstY_CardTransactionFlg = false;
                        }
                        else {
                            $scope.cmstranstY_CardTransactionFlg = true;
                        }
                    }
                    else {
                        $scope.cmstranstY_CardTransactionFlg = false;
                    }

                   
                    $scope.scroll();

                })
        }
        $scope.submitted = false;
        $scope.saveDetails = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {              
                var cmstranstY_Id = 0;
                if ($scope.cmstranstY_Id > 0) {
                    cmstranstY_Id = $scope.cmstranstY_Id;
                }               
                var cmstransty_AllowCreditTransFlg = 0;
                if ($scope.cmstranstY_AllowCreditTransFlg == true) {
                    cmstransty_AllowCreditTransFlg = 1;
                }
                var cmstransty_ConsiderForMinTransFlg = 0;
                if ($scope.cmstranstY_ConsiderForMinTransFlg == true) {
                    cmstransty_ConsiderForMinTransFlg = 1;
                }
                var cmstransty_CompulsoryFlg = 0;
                if ($scope.cmstranstY_CompulsoryFlg == true) {
                    cmstransty_CompulsoryFlg = 1;
                }
                var cmstransty_MemberCanChooseFlg = 0;
                if ($scope.cmstranstY_MemberCanChooseFlg == true) {
                    cmstransty_MemberCanChooseFlg = 1;
                }
                var cmstransty_CoverChargeFlg = 0;
                if ($scope.cmstranstY_CoverChargeFlg == true) {
                    cmstransty_CoverChargeFlg = 1;
                }
                var cmstransty_BarTransactionFlg = 0;
                if ($scope.cmstranstY_BarTransactionFlg == true) {
                    cmstransty_BarTransactionFlg = 1;
                }
                var cmstransty_FoodTransactionFlg = 0;
                if ($scope.cmstranstY_FoodTransactionFlg == true) {
                    cmstransty_FoodTransactionFlg = 1;
                }
                var cmstransty_CardTransactionFlg = 0;
                if ($scope.cmstranstY_CardTransactionFlg == true) {
                    cmstransty_CardTransactionFlg = 1;
                }                         
                var data = {
                    "CMSTRANSTY_Id": cmstranstY_Id,
                    "CMSTRANSTY_TransactionsName": $scope.cmstranstY_TransactionsName,
                    "CMSTRANSTY_AliasName": $scope.cmstranstY_AliasName,
                    "CMSTRANSTY_Amount": $scope.cmstranstY_Amount,                                  
                    "CMSTRANSTY_AllowCreditTransFlg": cmstransty_AllowCreditTransFlg,
                    "CMSTRANSTY_ConsiderForMinTransFlg": cmstransty_ConsiderForMinTransFlg,
                    "CMSTRANSTY_CompulsoryFlg": cmstransty_CompulsoryFlg,
                    "CMSTRANSTY_MemberCanChooseFlg": cmstransty_MemberCanChooseFlg,
                    "CMSTRANSTY_CoverChargeFlg": cmstransty_CoverChargeFlg,
                    "CMSTRANSTY_BarTransactionFlg": cmstransty_BarTransactionFlg,
                    "CMSTRANSTY_FoodTransactionFlg": cmstransty_FoodTransactionFlg,
                    "CMSTRANSTY_CardTransactionFlg": cmstransty_CardTransactionFlg,
                                  }
                apiService.create("CMS_TransactionType/saveDetails", data)
                    .then(function (promise) {
                        if (promise.returnval == "save") {
                            swal('Record Saved Successfully !');
                        }
                        else if (promise.returnval == "Notsave") {
                            swal('Record Not Saved !');
                        }
                        else if (promise.returnval == "update") {
                            swal('Record Updated Successfully !');
                        }
                        else if (promise.returnval == "Notupdate") {
                            swal('Record Not Updated !');
                        }
                        else if (promise.returnval == "Duplicate") {
                            swal('Record Already Exist !');
                        }
                        else if (promise.returnval == "admin") {
                            swal('Please Contact Administrator !');
                        }
                        $state.reload();
                    })
            }
        };
        $scope.clearid = function () {
            $state.reload();
        }
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
       
        $scope.scroll = function () {
            $("html, body").animate({ scrollTop: 0 }, 1000);
        };
    }
})();
