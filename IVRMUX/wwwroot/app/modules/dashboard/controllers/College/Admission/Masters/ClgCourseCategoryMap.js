(function () {
    'use strict';

    angular
        .module('app')
        .controller('ClgMasterCourseCategoryMapController', ClgMasterCourseCategoryMapController);

    ClgMasterCourseCategoryMapController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout'];

    function ClgMasterCourseCategoryMapController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        $scope.BindData = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";

            var id = 2;
            apiService.getURI("ClgMasterCourseCategoryMap/getalldetails", id).
                then(function (promise) {

                    $scope.MasterCourseList = promise.masterCourseList;
                    $scope.MasterCategoryList = promise.mastercategory;
                    $scope.griddetails = promise.grid;
                    $scope.presentCountgrid = $scope.griddetails.length;
                })
        };

        $scope.submitted = false;

        $scope.savetmpldata = function () {

            if ($scope.myForm.$valid) {
                var data = {
                    "AMCO_Id": $scope.amcO_Id,
                    "AMCOC_Id": $scope.amcoC_Id,
                    "AMCOCM_Id": $scope.AMCOCM_Id
                }
                apiService.create("ClgMasterCourseCategoryMap/Savedetails", data).then(function (promise) {


                    if (promise.message == "Exists") {
                        swal("You Can Not Map Same Course With Multiple Category");
                    } else {
                        if (promise.message == "Add") {
                            if (promise.returnval == true) {
                                swal("Record Saved Successfully")
                            } else {
                                swal("Failed To Save Record")
                            }
                        } else if (promise.message == "Update") {
                            if (promise.returnval == true) {
                                swal("Record Update Successfully")
                            } else {
                                swal("Failed To Update Record")
                            }
                        } else {
                            swal("Kindly Contact Administrator!!!");
                        }
                    }
                    $state.reload();
                })
            }
            else {
                $scope.submitted = true;
            }
        }


        $scope.Clearid = function () {
            $state.reload();
        }


        //For deactive data record
        $scope.deactive = function (item, SweetAlert) {

            $scope.AMCOCM_Id = item.amcocM_Id;

            var dystring = "";
            if (item.amcocM_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (item.amcocM_ActiveFlg == false) {
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
                        apiService.create("ClgMasterCourseCategoryMap/deactive", item).
                            then(function (promise) {
                                if (promise.returnval == true) {


                                    swal("Record " + dystring + "d Successfully!!!");

                                }
                                else {


                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }
                });
        }

        //For Cancel data record 
        $scope.Cancel = function () {
            $state.reload();
        }


        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };

        $scope.resultClick = function () {
            $scope.AMCOCM = item.amcocM_Id;
        }

        $scope.edit = function (item) {
            $scope.amcO_Id = item.amcO_Id;
            $scope.amcoC_Id = item.amcoC_Id;
            $scope.AMCOCM_Id = item.amcocM_Id;
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.searchValue = '';

        $scope.filterValue = function (obj) {

            return (angular.lowercase(obj.amcO_CourseName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.amcoC_Name)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }
    }
}
)();
