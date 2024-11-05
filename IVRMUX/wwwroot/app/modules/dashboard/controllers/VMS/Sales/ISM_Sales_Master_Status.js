(function () {
    'use strict';
    angular
        .module('app')
        .controller('Sales_Status_MasterController', sales_Status_MasterController)

    sales_Status_MasterController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function sales_Status_MasterController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {


        $scope.sortKey = 'ISMSMST_Id';
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
            apiService.getURI("Sales_Lead/get_load_sts", pageid)
                .then(function (promise) {
                    $scope.status_list = promise.status_list;
                    $scope.presentCountgrid = $scope.status_list.length;

                })
        }





        // edit
        $scope.edibl = {}
        $scope.edit = function (bil) {
         
            var data = {
                "ISMSMST_Id": bil.ismsmsT_Id
            }
            apiService.create("Sales_Lead/Edit_details_sts", data).
                then(function (promise) {
                    $scope.Id = promise.edit_status_list[0].ismsmsT_Id;
                    $scope.ISMSMST_StatusName = promise.edit_status_list[0].ismsmsT_StatusName;
                    $scope.ISMSMST_Remarks = promise.edit_status_list[0].ismsmsT_Remarks;

                })
        };


        //save
        $scope.submitted = false;
        $scope.savetmpldata = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "ISMSMST_StatusName": $scope.ISMSMST_StatusName,
                    "ISMSMST_Remarks": $scope.ISMSMST_Remarks,
                    "ISMSMST_Id": $scope.Id
                }
                apiService.create("Sales_Lead/SaveEdit_sts", data).
                    then(function (promise) {
                        if (promise.returnvales !== "") {

                            if (promise.returnvalue === "Update") {
                                swal('Record Updated Successfully');
                                $state.reload();
                            }
                            else if (promise.returnvalue === "Add") {
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
            if (bL.ismsmsT_ActiveFlag === false) {
                mgs = "Activate";
            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " the Status?",
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
                        apiService.create("Sales_Lead/deactivate_sts", bL).
                            then(function (promise) {

                                if (promise.retbool === false) {
                                    swal('Master Status  Deactivated Successfully');
                                }
                                else if (promise.retbool === true) {
                                    swal('Master Status Activated Successfully');
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



