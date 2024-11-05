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

            //  $scope.facfyM_BBDate = new Date();
            // $scope.sortKey = 'facfyM_BBDate';

            var id = 1;
            apiService.getURI("CMS_TransactionType/GetTaxInitialData", id).
                then(function (promise) {
                
                    $scope.transaction = promise.fill_TaxTransaction;
                    $scope.details = promise.fill_Taxdetails;
                    


                });

            $scope.fromdate = new Date();
            $scope.fromdate = new Date();

            //$scope.plMaxdate = new Date();
            //$scope.plMaxdate.setDate($scope.plMaxdate.getDate());
            $scope.minDatedof = new Date();
        };




        $scope.delete = function (det, SweetAlert) {
            var data = {
                "CMSTRANSTYTAX_Id": det.cmstranstytaX_Id
            }
            var dystring = "";
            if (det.cmstranstytaX_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (det.cmstranstytaX_ActiveFlag == false) {
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
                        apiService.create("CMS_TransactionType/deleteTaxDetails", data).
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
            //  $scope.famcomP_Id = det.famcomP_Id;
            $scope.editS = true;
            //var cmstranstyinT_Id = 0;
            //if (det.cmstranstyinT_Id > 0) {
            //    cmstranstyinT_Id = det.cmstranstyinT_Id;
            //}

            var data = {
                "CMSTRANSTYTAX_Id": det.cmstranstytaX_Id
            }
            //  var id = det.facfyM_Id;
            apiService.create("CMS_TransactionType/editTaxDetails/", data).
                then(function (promise) {

                    $scope.cmstranstY_Id = promise.fill_Taxdetails[0].cmstranstY_Id;
                    $scope.cmstranstY_TaxNo = promise.fill_Taxdetails[0].cmstranstY_TaxNo;
                    $scope.cmstranstytaX_TaxPercent = promise.fill_Taxdetails[0].cmstranstytaX_TaxPercent;
                    $scope.cmstranstytaX_Id = promise.fill_Taxdetails[0].cmstranstytaX_Id;
                    $scope.scroll();

                })
        }
        $scope.submitted = false;
        $scope.saveDetails = function () {
            if ($scope.myForm.$valid) {
                var cmstranstytaX_Id = 0;
                if ($scope.cmstranstytaX_Id > 0) {
                    cmstranstytaX_Id = $scope.cmstranstytaX_Id;
                }

                //var facfyM_BBDate = ;

                var data = {
                    "CMSTRANSTYTAX_Id": cmstranstytaX_Id,

                    "CMSTRANSTY_Id": $scope.cmstranstY_Id,

                    "CMSTRANSTY_TaxNo": $scope.cmstranstY_TaxNo,
                    "CMSTRANSTYTAX_TaxPercent": $scope.cmstranstytaX_TaxPercent,




                }
                apiService.create("CMS_TransactionType/saveTaxDetails", data)
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
