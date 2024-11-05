(function () {
    'use strict';
    angular
        .module('app')
        .controller('Sales_Product_MasterController', sales_Product_MasterController)

    sales_Product_MasterController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function sales_Product_MasterController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {


        $scope.sortKey = 'ISMSMPR_Id';
        $scope.sortReverse = true;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.ddate = {};
        $scope.ddate = new Date();

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));



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
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("Sales_Lead/get_load_pro", pageid)
                .then(function (promise) {
                    $scope.product_list = promise.product_list;
                    $scope.presentCountgrid = $scope.product_list.length;

                })
        }





        // edit
        $scope.edibl = {}
        $scope.edit = function (bil) {
          
            var data = {
                "ISMSMPR_Id":bil.ismsmpR_Id
            }
            apiService.create("Sales_Lead/Edit_details_pro", data).
                then(function (promise) {
                    $scope.Id = promise.edit_product_list[0].ismsmpR_Id;
                    $scope.product = promise.edit_product_list[0].ismsmpR_ProductName;
                    $scope.Remark = promise.edit_product_list[0].ismsmpR_Remarks;

                })
        };


        //save
        $scope.submitted = false;
        $scope.savetmpldata = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "ISMSMPR_ProductName": $scope.product,
                    "ISMSMPR_Remarks": $scope.Remark,
                    "ISMSMPR_Id": $scope.Id
                }
                apiService.create("Sales_Lead/SaveEdit_pro", data).
                    then(function (promise) {
                        if (promise.returnvales !== "") {

                            if (promise.returnvales === "Update") {
                                swal('Record Updated Successfully');
                                $state.reload();
                            }
                            else if (promise.returnvales === "Add") {
                                swal('Record Saved Successfully');
                                $state.reload();
                            }


                        }

                        else {
                            swal('Operation Failed');
                            $state.reload();
                        }
                    })

            }
            else {

                $scope.submitted = true;
            }

        };
        $scope.Clearid = function () {
            $state.reload();
        }
        //deactive
        $scope.deactive = function (bL, SweetAlertt) {

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            var mgs = "";
            if (bL.ismsmpR_ActiveFlag === false) {
                mgs = "Activate";
            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " the Product?",
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
                        apiService.create("Sales_Lead/deactivate_pro", bL).
                            then(function (promise) {

                                if (promise.retbool === false) {
                                    swal('Master Product Deactivated Successfully');
                                }
                                else if (promise.retbool === true) {
                                    swal('Master Product Activated Successfully');
                                }


                                $state.reload();
                            });
                    } else {
                        swal("Cancelled");
                       
                    }

                });
        };



        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };


    }

})();



