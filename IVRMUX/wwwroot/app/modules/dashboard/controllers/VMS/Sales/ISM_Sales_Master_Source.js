(function () {
    'use strict';
    angular
        .module('app')
        .controller('Sales_Source_MasterController', sales_Source_MasterController)

    sales_Source_MasterController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function sales_Source_MasterController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {


        $scope.sortKey = 'ISMSMSO_Id';
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
            apiService.getURI("Sales_Lead/get_load_src", pageid)
                .then(function (promise) {
                    $scope.source_list = promise.source_list;
                    $scope.presentCountgrid = $scope.source_list.length;

                })
        }





        // edit
        $scope.edibl = {}
        $scope.edit = function (bil) {
          
            var data = {
                "ISMSMSO_Id":bil.ismsmsO_Id
            }
            apiService.create("Sales_Lead/Edit_details_src", data).
                then(function (promise) {
                    $scope.Id = promise.edit_source_list[0].ismsmsO_Id;
                    $scope.source = promise.edit_source_list[0].ismsmsO_SourceName;
                    $scope.Remark = promise.edit_source_list[0].ismsmsO_Remarks;

                })
        };


        //save
        $scope.submitted = false;
        $scope.savetmpldata = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "ISMSMSO_SourceName": $scope.source,
                    "ISMSMSO_Remarks": $scope.Remark,
                    "ISMSMSO_Id": $scope.Id
                }
                apiService.create("Sales_Lead/SaveEdit_src", data).
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
            if (bL.ismsmsO_ActiveFlag === false) {
                mgs = "Activate";
            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " the Source?",
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
                        apiService.create("Sales_Lead/deactivate_src", bL).
                            then(function (promise) {

                                if (promise.retbool === false) {
                                    swal('Master Source Deactivated Successfully');
                                }
                                else if (promise.retbool === true) {
                                    swal('Master Source Activated Successfully');
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



