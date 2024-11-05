(function () {
    'use strict';
    angular
        .module('app')
        .controller('FAMasterCompanyController', FAMasterCompanyController)
    FAMasterCompanyController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function FAMasterCompanyController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.editS = false;
        $scope.loaddata = function () {
           
            $scope.currentPage = 1;
            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = 10;
            $scope.itemsPerPage = 10;

            $scope.famcomP_BookBeginingDate = new Date();
         //   $scope.sortKey = 'famcomP_BookBeginingDate';
            var id = 1;
            apiService.getURI("FAMasterCompany/Getdetails", id).
                then(function (promise) {
                    $scope.masterCompanyDetails = promise.masterCompanyDetails; 
                  
                });
            $scope.fromdate = new Date();
            $scope.fromdate = new Date();
            //disable date
            
          //  $scope.minDatedof = new Date();

            //$scope.plMaxdate = new Date();
            //$scope.plMaxdate.setDate($scope.plMaxdate.getDate());
            //
        };



        $scope.delete = function (det, SweetAlert) {
            var data = {
                "FAMCOMP_Id": det.famcomP_Id
            }
            var dystring = "";
            if (det.famcomP_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (det.famcomP_ActiveFlg == false) {
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
                        apiService.create("FAMasterCompany/deleteDetails", data).
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
            $scope.editS = true;
            //  $scope.famcomP_Id = det.famcomP_Id;
            var id = det.famcomP_Id;

            apiService.getURI("FAMasterCompany/editDetails/", id).
                then(function (promise) {
                  
                    $scope.famcomP_CompanyName = promise.masterCompanyDetails[0].famcomP_CompanyName;
                    $scope.famcomP_CompanyAddress = promise.masterCompanyDetails[0].famcomP_CompanyAddress;
                    $scope.famcomP_Description = promise.masterCompanyDetails[0].famcomP_Description;
                    $scope.famcomP_EMailId = promise.masterCompanyDetails[0].famcomP_EMailId;
                    $scope.famcomP_Password = promise.masterCompanyDetails[0].famcomP_Password;
                    $scope.famcomP_PhoneNo = promise.masterCompanyDetails[0].famcomP_PhoneNo;
                   
                    $scope.famcomP_IncomeTaxNo = promise.masterCompanyDetails[0].famcomP_IncomeTaxNo;
                    $scope.famcomP_SalesTaxNo = promise.masterCompanyDetails[0].famcomP_SalesTaxNo;
                    $scope.famcomP_BookBeginingDate = new Date(promise.masterCompanyDetails[0].famcomP_BookBeginingDate);
                    $scope.famcomP_Id = promise.masterCompanyDetails[0].famcomP_Id;
                    if (promise.masterCompanyDetails[0].famcomP_CMPTypeFlg != null) {
                        if (promise.masterCompanyDetails[0].famcomP_CMPTypeFlg == 0) {
                            $scope.famcomP_CMPTypeFlg = false;
                        }
                        else {
                            $scope.famcomP_CMPTypeFlg = true;
                        }
                    }
                    else {
                        $scope.famcomp_cmptypeflg = false;
                    }

                    if (promise.masterCompanyDetails[0].famcomP_DuplicateVoucherFlg != null) {
                        if (promise.masterCompanyDetails[0].famcomP_DuplicateVoucherFlg == 0) {
                            $scope.famcomP_DuplicateVoucherFlg = false;
                        }
                        else {
                            $scope.famcomP_DuplicateVoucherFlg = true;
                        }
                    }
                    else {
                        $scope.famcomP_DuplicateVoucherFlg = false;
                    }


                    if (promise.masterCompanyDetails[0].famcomP_PrintReceiptFlg != null) {
                        if (promise.masterCompanyDetails[0].famcomP_PrintReceiptFlg == 0) {
                            $scope.famcomP_PrintReceiptFlg = false;
                        }
                        else {
                            $scope.famcomP_PrintReceiptFlg = true;
                        }
                    }
                    else {
                        $scope.famcomP_PrintReceiptFlg = false;
                    }


                    if (promise.masterCompanyDetails[0].famcomP_SetDispFlg != null) {
                        if (promise.masterCompanyDetails[0].famcomP_SetDispFlg == 0) {
                            $scope.famcomP_SetDispFlg = false;
                        }
                        else {
                            $scope.famcomP_SetDispFlg = true;
                        }
                    }
                    else {
                        $scope.famcomP_SetDispFlg = false;
                    }

                    if (promise.masterCompanyDetails[0].famcomP_SetLedgerBalanceFlg != null) {
                        if (promise.masterCompanyDetails[0].famcomP_SetLedgerBalanceFlg == 0) {
                            $scope.famcomP_SetLedgerBalanceFlg = false;
                        }
                        else {
                            $scope.famcomP_SetLedgerBalanceFlg = true;
                        }
                    }
                    else {
                        $scope.famcomP_SetLedgerBalanceFlg = false;
                    }

                    if (promise.masterCompanyDetails[0].famcomP_SetNegBalanceFlg != null) {
                        if (promise.masterCompanyDetails[0].famcomP_SetNegBalanceFlg == 0) {
                            $scope.famcomP_SetNegBalanceFlg = false;
                        }
                        else {
                            $scope.famcomP_SetNegBalanceFlg = true;
                        }
                    }
                    else {
                        $scope.famcomP_SetNegBalanceFlg = false;
                    }

                    if (promise.masterCompanyDetails[0].famcomP_SetTypeFlg != null) {
                        if (promise.masterCompanyDetails[0].famcomP_SetTypeFlg == 0) {
                            $scope.famcomP_SetTypeFlg = false;
                        }
                        else {
                            $scope.famcomP_SetTypeFlg = true;
                        }
                    }
                    else {
                        $scope.famcomP_SetTypeFlg = false;
                    }

                    if (promise.masterCompanyDetails[0].famcomP_StatusFlg != null) {
                        if (promise.masterCompanyDetails[0].famcomP_StatusFlg == 0) {
                            $scope.famcomP_StatusFlg = false;
                        }
                        else {
                            $scope.famcomP_StatusFlg = true;
                        }
                    }
                    else {
                        $scope.famcomP_StatusFlg = false;
                    }

                    if (promise.masterCompanyDetails[0].famcomP_UseBillWiseDetailsFlg != null) {
                        if (promise.masterCompanyDetails[0].famcomP_UseBillWiseDetailsFlg == 0) {
                            $scope.famcomP_UseBillWiseDetailsFlg = false;
                        }
                        else {
                            $scope.famcomP_UseBillWiseDetailsFlg = true;
                        }
                    }
                    else {
                        $scope.famcomP_UseBillWiseDetailsFlg = false;
                    }


                    if (promise.masterCompanyDetails[0].famcomP_UseDebitCreditFlg != null) {
                        if (promise.masterCompanyDetails[0].famcomP_UseDebitCreditFlg == 0) {
                            $scope.famcomP_UseDebitCreditFlg = false;
                        }
                        else {
                            $scope.famcomP_UseDebitCreditFlg = true;
                        }
                    }
                    else {
                        $scope.famcomP_UseDebitCreditFlg = false;
                    }
                    $scope.scroll();

                })
        }
        $scope.submitted = false;
        $scope.saveDetails = function () {
           $scope.submitted = true;
          if ($scope.myForm.$valid ) {
                var famcomP_Id = 0;
                if ($scope.famcomP_Id > 0) {
                    famcomP_Id = $scope.famcomP_Id;
                }
                var famcomp_PrintReceiptFlg = 0;
                if ($scope.famcomP_PrintReceiptFlg == true) {
                    famcomp_PrintReceiptFlg = 1;
                }
                var famcomp_duplicatevoucherflg = 0;
                if ($scope.famcomP_DuplicateVoucherFlg == true) {
                    famcomp_duplicatevoucherflg = 1;
                }
                var famcomp_usebillwisedetailsflg = 0;
                if ($scope.famcomP_UseBillWiseDetailsFlg == true) {
                    famcomp_usebillwisedetailsflg = 1;
                }
                var famcomp_cmptypeflg = 0;
                if ($scope.famcomP_CMPTypeFlg == true) {
                    famcomp_cmptypeflg = 1;
                }
                var famcomp_usedebitcreditflg = 0;
                if ($scope.famcomP_UseDebitCreditFlg == true) {
                    famcomp_usedebitcreditflg = 1;
                }

                var famcomp_settypeflg = 0;
                if ($scope.famcomP_SetTypeFlg == true) {
                    famcomp_settypeflg = 1;
                }

                var famcomp_setledgerbalanceflg = 0;
                if ($scope.famcomP_SetLedgerBalanceFlg == true) {
                    famcomp_setledgerbalanceflg = 1;
                }

                var famcomp_setdispflg = 0;
                if ($scope.famcomP_SetDispFlg == true) {
                    famcomp_setdispflg = 1;
                }
                var famcomp_usedebitcreditflg = 0;
                if ($scope.famcomP_UseDebitCreditFlg == true) {
                    famcomp_usedebitcreditflg = 1;
                }

                var famcomP_SetNegBalanceFlg = 0;
                if ($scope.famcomP_SetNegBalanceFlg == true) {
                    famcomP_SetNegBalanceFlg = 1;
                }
                //var famcomP_BookBeginingDate = 

                var data = {
                    "FAMCOMP_Id":famcomP_Id,
                  

                    "FAMCOMP_CompanyName": $scope.famcomP_CompanyName,
                    "FAMCOMP_CompanyAddress": $scope.famcomP_CompanyAddress,
                    "FAMCOMP_Description": $scope.famcomP_Description,
                    "FAMCOMP_PhoneNo": $scope.famcomP_PhoneNo,
                    "FAMCOMP_EMailId": $scope.famcomP_EMailId,
                    "FAMCOMP_IncomeTaxNo": $scope.famcomP_IncomeTaxNo,
                    "FAMCOMP_SalesTaxNo": $scope.famcomP_SalesTaxNo,
                    "FAMCOMP_Password": $scope.famcomP_Password,
                    "FAMCOMP_BookBeginingDate": new Date($scope.famcomP_BookBeginingDate).toDateString(),
                    "FAMCOMP_PrintReceiptFlg": famcomp_PrintReceiptFlg,
                    "FAMCOMP_DuplicateVoucherFlg": famcomp_duplicatevoucherflg,
                    "FAMCOMP_UseBillWiseDetailsFlg": famcomp_usebillwisedetailsflg,
                    "FAMCOMP_CMPTypeFlg": famcomp_cmptypeflg,
                    "FAMCOMP_UseDebitCreditFlg": famcomp_usedebitcreditflg,
                    "FAMCOMP_SetTypeFlg":famcomp_settypeflg,
                    "FAMCOMP_SetLedgerBalanceFlg": famcomp_setledgerbalanceflg,
                    "FAMCOMP_SetDispFlg": famcomp_setdispflg,
                    "FAMCOMP_SetNegBalanceFlg": famcomP_SetNegBalanceFlg,
            
                    
                }
                apiService.create("FAMasterCompany/saveDetails", data)
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

       // };
        //$scope.interacted = function (field) {
        //    return $scope.submitted || field.$dirty;
      //  };
        $scope.scroll = function () {
            $("html, body").animate({ scrollTop: 0 }, 1000);
        };
    }
})();
