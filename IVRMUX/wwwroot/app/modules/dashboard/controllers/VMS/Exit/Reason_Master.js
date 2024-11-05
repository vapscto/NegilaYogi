(function () {
    'use strict';
    angular
        .module('app')
        .controller('Reason_MasterController', reason_MasterController)
    reason_MasterController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', '$stateParams', 'Excel', '$timeout']
    function reason_MasterController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, $stateParams, Excel, $timeout) {
        //====================================================================================
        $scope.sortKey = 'ismresgmrE_Id';
        $scope.sortReverse = true;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }



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

        //===================Load data=======================
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("Exit_Employee/Get_Reason", pageid).then(function (promise) {
                $scope.get_reason_list = promise.get_reason_list;
                $scope.presentCountgrid = $scope.get_reason_list.length;
            });
        }
        $scope.Clearid = function () {
            $state.reload();
        }
        //=============Save/Edit========================
        $scope.submitted = false;
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "ISMRESGMRE_Id": $scope.id,
                    "ISMRESGMRE_ResignationReasons": $scope.ismresgmrE_ResignationReasons
                };
                apiService.create("Exit_Employee/Save_Edit_Reason", data).then(function (promise) {
                    if (promise.returnval === "Add") {
                        swal('Record Saved Successfully');
                    }
                    else if (promise.returnval === "Update") {
                        swal('Record Updated Successfully');
                    }
                    else {
                        swal('Operation Failed');
                    }
                    $state.reload();
                });
            }
            else {

                $scope.submitted = true;
            }
        }

        //============================getdetailsReasonmaster================================
        $scope.edit = {};
        $scope.Edit = function (ss) {
            var pageid = ss.ismresgmrE_Id
            apiService.getURI("Exit_Employee/get_details_reason", pageid).then(function (promise) {
                $scope.ismresgmrE_ResignationReasons = promise.get_details_reason_list[0].ismresgmrE_ResignationReasons;
                $scope.id = promise.get_details_reason_list[0].ismresgmrE_Id
            })


        }

        $scope.deactive = function (flr, SweetAlertt) {

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            var mgs = "";
            if (flr.ismresgmrE_ActiveFlag === false) {
                mgs = "Activate";
            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " the Academic Year?",
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
                        apiService.create("Exit_Employee/active_deactive_reason", flr).
                            then(function (promise) {

                                if (promise.returnval === "false") {
                                    swal('Master Reason Deactivated Successfully');
                                }

                                else if (promise.returnval === "true") {
                                    swal('Master Reason Activated Successfully');
                                }
                                else {
                                    swal("Cancelled");
                                }
                                $state.reload();
                            });
                    }
                    else {
                        swal("Cancelled");
                        $state.reload();
                    }
                });
        }
     


        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };

    }
})();
