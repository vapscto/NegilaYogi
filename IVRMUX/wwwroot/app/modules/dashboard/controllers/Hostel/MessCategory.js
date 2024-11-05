(function () {
    'use strict';
    angular
        .module('app')
        .controller('MessCategoryController', MessCategoryController)

    MessCategoryController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function MessCategoryController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {


        $scope.sortReverse = true;
        $scope.itemsPerPage = 10;

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {

            return (angular.lowercase(obj.mob)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }

        //get data
        $scope.getAllDetail = function () {

            $scope.currentPage = 1;
            var pageid = 2;
            apiService.getURI("HS_Master/get_messCategorydata", pageid)
                .then(function (promise) {
                    $scope.get_messCategorylist = promise.get_messCategorylist;
                    
                })
        }

        //save
        $scope.submitted = false;
        $scope.savetmpldata = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "HLMMC_Id": $scope.hlmmC_Id,
                    "HLMMC_Name": $scope.hlmmC_Name,                   
                }
                apiService.create("HS_Master/save_messCategorydata", data).
                    then(function (promise) {
                        if (promise.returnval !== null && promise.duplicate !== null) {
                            if (promise.duplicate === false) {
                                if (promise.returnval === true) {
                                    if ($scope.hlmmC_Id > 0) {
                                        swal('Record Updated Successfully!!!');
                                        $state.reload();
                                    }
                                    else {
                                        swal('Record Saved Successfully!!!');
                                        $state.reload();
                                    }
                                }
                                else {
                                    if (promise.returnval === false) {
                                        if ($scope.hlmmC_Id > 0) {
                                            swal('Record Not Updated Successfully!!!');
                                        }
                                        else {
                                            swal('Record Not Saved Successfully!!!');
                                        }
                                    }
                                }
                            }
                            else {
                                swal("Record already exist");
                            }
                        }
                        else {
                            swal("Kindly Contact Administrator!!!");
                        }
                    })

            }
            else {

                $scope.submitted = true;
            }

        };
        
        //================edit

        $scope.edit_data = function (user) {

            $scope.hlmmC_Id = user.hlmmC_Id;
            $scope.hlmmC_Name = user.hlmmC_Name;
        };


        $scope.cancel = function () {
            $state.reload();
        }

        //================================deactive

        $scope.deactive = function (user, SweetAlert) {
            debugger;
            $scope.hlmmC_Id = user.hlmmC_Id;

            var dystring = "";
            if (user.hlmmC_ActiveFlag === true) {
                dystring = "Deactivate";
            }
            else if (user.hlmmC_ActiveFlag === false) {
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
                        apiService.create("HS_Master/deactiveY_messCategorydata", user).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal("Record " + dystring + "d Successfully!!!");

                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");

                                }
                                $state.reload();
                            })

                    }
                    else {
                        swal("Record " + dystring + " Cancelled");
                    }

                });
        }


        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };


    }

})();



