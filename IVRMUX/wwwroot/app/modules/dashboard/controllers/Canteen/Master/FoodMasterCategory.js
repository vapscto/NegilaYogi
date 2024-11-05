(function () {
    'use strict';
    angular
        .module('app')
        .controller('FoodMasterCategoryController', FoodMasterCategoryController)
    FoodMasterCategoryController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$stateParams', '$filter', 'superCache', '$q']
   
    function FoodMasterCategoryController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $stateParams, $filter, superCache, $q) {

        $scope.EditRecord = {};
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.obj = {};

        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.loaddata = function () {

            var pageid = 2;
            apiService.getURI("FoodMasterCategory/loaddata", pageid).
                then(function (promise) {
                    $scope.foodcategeory = promise.foodcategeory;

                    if ($scope.foodcategeory != null && $scope.foodcategeory.length > 0) {
                        $scope.presentCountgrid = $scope.foodcategeory.length;
                    }

                })
        }


           $scope.submitted = false;
           $scope.submit = function () {
               $scope.submitted = true;
               if ($scope.myForm.$valid) {
                   var CMMCA_Id = 0;
                   if ($scope.obj.CMMCA_Id > 0) {
                       CMMCA_Id = $scope.obj.CMMCA_Id;
                   }
                   var data = {

                       "CMMCA_CategoryName": $scope.CMMCA_Categeory,
                       "CMMCA_Remarks": $scope.CMMCA_Remarks,
                       "CMMCA_Id": CMMCA_Id
                   }
                   apiService.create("FoodMasterCategory/savedata", data).
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
             
            
        };
        //edit data 
        $scope.Editdata = function (EditRecord) {
            var data = {
                "CMMCA_Id": EditRecord.cmmcA_Id,
            }
            apiService.create("FoodMasterCategory/GetEditdata/", data).then(function (promise) {

                $scope.CMMCA_Categeory = promise.gridviewDetails[0].cmmcA_CategoryName;
                $scope.CMMCA_Remarks = promise.gridviewDetails[0].cmmcA_Remarks;
                $scope.obj.CMMCA_Id = promise.gridviewDetails[0].cmmcA_Id;

            })
        };

        
    

        $scope.deactive = function (category) {
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };

            var mgs = "";
            if (category.cmmcA_ActiveFlag == false) {

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
                        apiService.create("FoodMasterCategory/deactivate", category).then(function (promise) {
                            swal(promise.returnval);
                            $state.reload();
                        });
                    }
                    else {
                        swal("Cancelled");
                    }
                });

        };
        //search
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return (angular.lowercase(obj.cmmcA_CategoryName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.cmmcA_Remarks)).indexOf(angular.lowercase($scope.searchValue)) >= 0
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