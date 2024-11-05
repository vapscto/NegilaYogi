(function () {
    'use strict';
    angular
        .module('app')
        .controller('FACompanyFYMappingController', FACompanyFYMappingController)
    FACompanyFYMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function FACompanyFYMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.editS = false;
        $scope.loaddata = function () {

            $scope.currentPage = 1;
            $scope.currentPage1 = 1;
            $scope.itemsPerPage1 = 10;
            $scope.itemsPerPage = 10;

            $scope.facfyM_BBDate = new Date();
           // $scope.sortKey = 'facfyM_BBDate';

            var id = 1;
            apiService.getURI("FAMasterCompany/GetInitialData", id).
                then(function (promise) {
                    $scope.companies = promise.fillcompany;
                    $scope.year = promise.fillfinacialyear;
                    $scope.FYDetails = promise.fyDetails;


                });

            $scope.fromdate = new Date();
            $scope.fromdate = new Date();

            //$scope.plMaxdate = new Date();
            //$scope.plMaxdate.setDate($scope.plMaxdate.getDate());
            $scope.minDatedof = new Date();
        };


  

        $scope.delete = function (det, SweetAlert) {
            var data = {
                "FACFYM_Id": det.facfyM_Id
            }
            var dystring = "";
            if (det.facfyM_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (det.facfyM_ActiveFlg == false) {
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
                        apiService.create("FAMasterCompany/deleteFYDetails", data).
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
            var facfym = 0;
            if (det.facfyM_Id > 0) {
                facfym = det.facfyM_Id;
            }
          //  var id = det.facfyM_Id;
            apiService.getURI("FAMasterCompany/editFYDetails/", facfym).
                then(function (promise) {

                    $scope.famcomP_Id = promise.fyDetails[0].famcomP_Id;
                    $scope.facfyM_FinancialYearCloseFlg = promise.fyDetails[0].facfyM_FinancialYearCloseFlg;
                    $scope.facfyM_RefNo = promise.fyDetails[0].facfyM_RefNo;
                    $scope.facfyM_Id = promise.fyDetails[0].facfyM_Id;
                    $scope.imfY_Id = promise.fyDetails[0].imfY_Id;
                    $scope.facfyM_Budget = promise.fyDetails[0].facfyM_Budget;
                   
                    $scope.facfyM_BBDate = new Date(promise.fyDetails[0].facfyM_BBDate);
                    $scope.scroll();
                    
                })
        }
        $scope.submitted = false;
        $scope.saveDetails = function () {
            if ($scope.myForm.$valid) {
                var facyM_Id = 0;
                if ($scope.facfyM_Id > 0) {
                    facyM_Id = $scope.facfyM_Id;
                }
                var facfyM_FinancialYearCloseFlg = 0;
                if ($scope.facfyM_FinancialYearCloseFlg == true) {
                    facfyM_FinancialYearCloseFlg = 1;
                }
                //var facfyM_BBDate = ;

                var data = {
                    "FACFYM_Id": $scope.facfyM_Id,

                    "IMFY_Id": $scope.imfY_Id,

                    "FAMCOMP_Id": $scope.famcomP_Id,
                    "FACFYM_RefNo": $scope.facfyM_RefNo,
                    "FACFYM_BBDate": new Date($scope.facfyM_BBDate).toDateString(),
                    "FACFYM_Budget": $scope.facfyM_Budget,
                    "FACFYM_FinancialYearCloseFlg": facfyM_FinancialYearCloseFlg



                }
                apiService.create("FAMasterCompany/saveFYDetails", data)
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
