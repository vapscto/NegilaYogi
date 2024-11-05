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
            var id = 1;
            apiService.getURI("CMS_TransactionType/GetInitialData", id).
                then(function (promise) {
                    $scope.installment = promise.fill_Installment;
                    $scope.transaction = promise.fill_Transaction;
                    $scope.details = promise.fill_details;

                });
            $scope.fromdate = new Date();
            $scope.fromdate = new Date();           
            $scope.minDatedof = new Date();
        };
        $scope.delete = function (det, SweetAlert) {
            var data = {
                "CMSTRANSTYINT_Id": det.cmstranstyinT_Id
            }
            var dystring = "";
            if (det.cmstranstyinT_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (det.cmstranstyinT_ActiveFlag == false) {
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
                        apiService.create("CMS_TransactionType/deleteInsDetails", data).
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
            var data = {
                "CMSTRANSTYINT_Id": det.cmstranstyinT_Id
            }          
            apiService.create("CMS_TransactionType/editInsDetails/", data).
                then(function (promise) {
                    $scope.cmstranstY_Id = promise.fill_details[0].cmstranstY_Id;
                    $scope.cmsminsT_Id = promise.fill_details[0].cmsminsT_Id;
                    $scope.cmstranstyinT_Amount = promise.fill_details[0].cmstranstyinT_Amount;
                    $scope.cmstranstyinT_Id = promise.fill_details[0].cmstranstyinT_Id;
                    $scope.scroll();
                })
        }
        $scope.submitted = false;
        $scope.saveDetails = function () {
            if ($scope.myForm.$valid) {
                var cmstranstyinT_Id = 0;
                if ($scope.cmstranstyinT_Id > 0) {
                    cmstranstyinT_Id = $scope.cmstranstyinT_Id;
                }                            
                var data = {
                    "CMSTRANSTYINT_Id": $scope.cmstranstyinT_Id,
                    "CMSTRANSTY_Id": $scope.cmstranstY_Id,
                    "CMSMINST_Id": $scope.cmsminsT_Id,
                    "CMSTRANSTYINT_Amount": $scope.cmstranstyinT_Amount,           
                }
                apiService.create("CMS_TransactionType/saveInsDetails", data)
                    .then(function (promise) {
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
                            swal('Records Not Updated !');
                        }
                        else if (promise.returnval == "Duplicate") {
                            swal('Records Already Exist !');
                        }
                        else if (promise.returnval == "admin") {
                            swal('Please Contact Administrator !');
                        }
                        $state.reload();
                    })
            }
            else {
                $scope.submitted = true;
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
