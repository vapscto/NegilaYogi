(function () {
    'use strict';
    angular
        .module('app')
        .controller('FooditemtaxController', FooditemtaxController)
    FooditemtaxController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'superCache']
    function FooditemtaxController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, superCache) {
        $scope.EditRecord = {};
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
        $scope.obj = {};
        $scope.CMMFI_OutofStockFlg = false;

        $scope.loaddata = function () {

            var pageid = 2;
            apiService.getURI("Fooditemtax/loaddata", pageid).
                then(function (promise) {
                    $scope.foodtax = promise.foodtax;
                    $scope.fooditeam = promise.fooditeam;
                    $scope.invmaster = promise.invmaster;

                    if ($scope.foodtax != null && $scope.foodtax.length > 0) {
                        $scope.presentCountgrid = $scope.foodtax.length;
                    }
                  
                })
        }
        $scope.submitted = false;
        $scope.submit = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var CMMFIT_Id = 0;
                if ($scope.obj.CMMFIT_Id > 0) {
                    CMMFIT_Id = $scope.obj.CMMFIT_Id;
                }
               
                var data = {

                    "CMMFIT_TaxPercent": $scope.CMMFIT_TaxPercent,
                    "CMMFI_Id": $scope.CMMFI_Id,
                    "INVMT_Id": $scope.Inv_tax,
                    "CMMFIT_Id": CMMFIT_Id,
                }
                apiService.create("Fooditemtax/savedata", data).
                    then(function (promise) {


                        if (promise.returnval == "save") {
                            swal('Record Saved Successfully !', "", "success");

                        }
                        else if (promise.returnval == "Notsave") {
                            swal('Records Not Saved !');
                        }
                        else if (promise.returnval == "update") {
                            swal('Records Updated Successfully !', "", "success");

                        }
                        else if (promise.returnval == "Notupdate") {
                            swal('Records Not Updated  !', "", "error");

                        }
                        else if (promise.returnval == "exit") {
                            swal('Records Already Exist !', "", "error");
                        }
                        else if (promise.returnval == "admin") {
                            swal('Please Contact  Administrator  !', "", "warning");
                        }


                        $state.reload();
                    })

            }


        }
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.cmmfI_FoodItemName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.taxpercent)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        };
   
        $scope.deactive = function (tax) {
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };

            var mgs = "";
            if (tax.cmmfiT_ActiveFlg == false) {

                mgs = "Activate";

            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " Category?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("Fooditemtax/deactivate", tax).then(function (promise) {
                            swal(promise.returnval);
                            $state.reload();
                        });
                    }
                    else {
                        swal("Cancelled");
                    }
                });

        };
        $scope.cancel = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

        $scope.clear = function () {
            $state.reload();
        }
    }

})();