(function () {
    'use strict';
    angular
.module('app')
        .controller('ClgMasterCategoryController', ClgMasterCategoryController)

    ClgMasterCategoryController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function ClgMasterCategoryController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        // $scope.sortKey = 'amC_Id';
        $scope.sortReverse = true;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null) {
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            }
            else {
                paginationformasters = 10;
            }
        }
        else {
            paginationformasters = 10;
        }

        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.getAllDetails = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.searchValue = '';
            var pageid = 2;
            apiService.getDATA("ClgMasterCategory/getalldetails", pageid).
            then(function (promise) {
                $scope.CategoryList = promise.masterCategoryList;                               
                $scope.typelist = [
               { id: '1', name: "School" },
               { id: '2', name: "College" }
                ];
            })

        }

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        $scope.submitted = false;
        $scope.savetmpldata = function () {
            

            if ($scope.myForm.$valid) {
                var data = {
                    "AMCOC_Name": $scope.AMCOC_Name,
                    "AMCOC_Address": $scope.AMCOC_Address,
                    "AMCOC_Details": $scope.AMCOC_Details,
                    "AMCOC_Type": $scope.AMCOC_Type,
                    "ACMC_RegNoPrefix": $scope.ACMC_RegNoPrefix,
                    "AMCOC_Id": $scope.AMCOC_Id,
                }
                apiService.create("ClgMasterCategory/Savedetails", data)

                .then(function (promise) {
                    if (promise.message == "Add") {
                        swal('Record Saved Successfully');                        
                    }
                    else if (promise.message == "Update") {
                        swal('Record Updated Successfully');                        
                    }
                    else if (promise.message == "false") {
                        swal('Record Not Saved/Updated successfully');
                    }
                    else if (promise.message == "Duplicate") {
                        swal('Category Already Exist');
                    }
                    else {
                        swal('Operation Failed');
                    }
                    $state.reload();
                })

            }
            else {
                $scope.submitted = true;
            }
            

        };


        $scope.edit = function (item) {
            $scope.AMCOC_Id = item.amcoC_Id;
            $scope.AMCOC_Name = item.amcoC_Name;
            $scope.AMCOC_Address = item.amcoC_Address,
            $scope.ACMC_RegNoPrefix = item.acmC_RegNoPrefix,
            $scope.AMCOC_Details = item.amcoC_Details,
            $scope.AMCOC_Type = item.amcoC_Type
        }



        $scope.deactive = function (category) {
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            
            var mgs = "";
            if (category.acmC_ActiveFlag == false) {
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
                     apiService.create("ClgMasterCategory/deactivate", category).
                     then(function (promise) {
                         
                         if (promise.message == "Mapped") {
                             swal("You Can Not Deactive This Record Its Already Mapped.");

                         } else {
                             if (promise.returnval == true) {
                                 swal("Record Successfully " + mgs + "");
                             }
                             else {
                                 swal("Failed To " + mgs + " Redcord");
                             }
                             
                         }
                         
                         $state.reload();
                     })
                 }
                 else {
                     swal("Cancelled");
                     $state.reload();
                 }
             })
        }

        $scope.Clearid = function () {
            $state.reload();
        }

        $scope.searchValue = "";
        $scope.searchfilter = function (obj) {

            return (angular.lowercase(obj.amcoC_Name)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.amcoC_Address)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.amcoC_Details)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.amcoC_Type)).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        }




    }

})();

