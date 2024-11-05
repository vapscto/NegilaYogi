(function () {
    'use strict';
    angular
        .module('app')
        .controller('PL_Master_CompanyController', PL_Master_CompanyController)
    PL_Master_CompanyController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$stateParams', '$filter', 'superCache', '$q']
    function PL_Master_CompanyController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $stateParams, $filter, superCache, $q) {
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


        //$scope.loaddata = function () {
        //    var pageid = 2;
        //    apiService.getURI("PL_Master_Company/loaddata", pageid).then(function (promise) {
        //        $scope.get_Placement = promise.pages;
        //        if ($scope.get_Placement != null && $scope.get_Placement.length > 0) {
        //            $scope.presentCountgrid = $scope.get_Placement.length;
        //        }

        //    })
        //};


        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("PL_Master_Company/loaddata", pageid).
                then(function (promise) {
                    $scope.get_Placement = promise.get_Placement;
                    $scope.presentCountgrid = $scope.get_Placement.length;
                })
        };


        $scope.submitted = false;

        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "PLMCOMP_CompanyName": $scope.plmcomP_CompanyName,
                    "PLMCOMP_FacilityFilePath": $scope.plmcomP_FacilityFilePath,
                    "PLMCOMP_Website": $scope.plmcomP_Website,
                    "PLMCOMP_CompanyAddress": $scope.plmcomP_CompanyAddress,
                    "PLMCOMP_Id": $scope.plmcomP_Id,               
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("PL_Master_Company/savedata", data).then(function (promise) {

                    if (promise.returnval == true) {
                        if (promise.plmcomP_Id == 0 || promise.plmcomP_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.plmcomP_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus == 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.plmcomP_Id == 0 || promise.plmcomP_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.plmcomP_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }
                    $state.reload();
                })
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $state.reload();
        }
        //
        //Deactive
        $scope.deactive = function (item, SweetAlert) {

            var data = {

                "PLMCOMP_Id": item.plmcomP_Id
            }
            var dystring = "";
            if (item.plmcomP_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.plmcomP_ActiveFlag == false) {
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
                        apiService.create("PL_Master_Company/deactive", data).
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
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        }

        $scope.editdata = function (user) {
            $scope.plmcomP_CompanyName = user.plmcomP_CompanyName;
            $scope.plmcomP_FacilityFilePath = user.plmcomP_FacilityFilePath;
            $scope.plmcomP_Website = user.plmcomP_Website;
            $scope.plmcomP_CompanyAddress = user.plmcomP_CompanyAddress;
            $scope.plmcomP_Id = user.plmcomP_Id;
        };
        $scope.clear = function () {
            $state.reload();
        }
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';

    }

})();