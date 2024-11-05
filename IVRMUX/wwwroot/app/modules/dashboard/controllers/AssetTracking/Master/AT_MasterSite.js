
(function () {
    'use strict';
    angular
        .module('app')
        .controller('AT_MasterSiteController', AT_MasterSiteController);
    AT_MasterSiteController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function AT_MasterSiteController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {



        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0
        }

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        //=====================================PAGE LOAD
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("AT_MasterSite/getloaddata", pageid).
                then(function (promise) {
                    $scope.get_sites = promise.get_sites;
                    $scope.presentCountgrid = $scope.get_sites.length;
                })
        };

           //=====================================SAVE EDIT DEACTIVE
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "INVMSI_SiteBuildingName": $scope.invmsI_SiteBuildingName,
                    "INVMSI_SiteRemarks": $scope.invmsI_SiteRemarks,                   
                    "INVMSI_Id": $scope.invmsI_Id,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("AT_MasterSite/savedetails", data).then(function (promise) {
                    if (promise.returnval == true) {
                        if (promise.invmsI_Id == 0 || promise.invmsI_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.invmsI_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus == 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.invmsI_Id == 0 || promise.invmsI_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.invmsI_Id > 0) {
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

        $scope.deactive = function (item, SweetAlert) {
            $scope.INVMSI_Id = item.invmsI_Id;
            var dystring = "";
            if (item.invmsI_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (item.invmsI_ActiveFlg == false) {
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
                        apiService.create("AT_MasterSite/deactive", item).
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
                        swal("Record " + dystring + " Cancelled!!!");
                    }
                });
        }

        $scope.edit = function (item) {
            $scope.invmsI_SiteBuildingName = item.invmsI_SiteBuildingName;
            $scope.invmsI_SiteRemarks = item.invmsI_SiteRemarks;         
            $scope.invmsI_Id = item.invmsI_Id;
        }
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';
      
    }
})();