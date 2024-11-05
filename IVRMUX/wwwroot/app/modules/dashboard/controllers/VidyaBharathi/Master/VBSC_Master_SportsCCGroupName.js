
(function () {
    'use strict';
    angular
        .module('app')
        .controller('VBSC_Master_SportsCCGroupNameController', VBSC_Master_SportsCCGroupNameController);
    VBSC_Master_SportsCCGroupNameController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function VBSC_Master_SportsCCGroupNameController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {


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

        //---------------------------OnloadData-----------------------------------------------------

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("VBSC_Master_SportsCCGroupName/getloaddata", pageid).
                then(function (promise) {
                    // MT_Id
                   
                    $scope.master_trust = promise.master_trust;
                    $scope.mT_Id = promise.mT_Id;

                    $scope.get_customer = promise.get_customer;
                    $scope.presentCountgrid = $scope.get_customer.length;
                })
        };

        //----------------------------Savedata----------------------------------------------------

        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "VBSCMSCCG_SportsCCGroupName": $scope.vbscmsccG_SportsCCGroupName,
                    "VBSCMSCCG_SportsCCGroupDesc": $scope.vbscmsccG_SportsCCGroupDesc,
                    "VBSCMSCCG_Id": $scope.vbscmsccG_Id,
                    "VBSCMSCCG_SCCFlag": $scope.vbscmsccG_SCCFlag,
                    "MT_Id": $scope.mT_Id
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("VBSC_Master_SportsCCGroupName/savedetails", data).then(function (promise) {

                    if (promise.returnval == true) {
                        if (promise.vbscmsccG_Id == 0 || promise.vbscmsccG_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.vbscmsccG_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus == 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.vbscmsccG_Id == 0 || promise.vbscmsccG_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.vbscmsccG_Id > 0) {
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

        //---------------------------Active & Deactive-----------------------------------------------------

        $scope.deactive = function (item, SweetAlert) {
            $scope.VBSCMSCCG_Id = item.vbscmsccG_Id;
            var dystring = "";
            if (item.vbscmsccG_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.vbscmsccG_ActiveFlag == false) {
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
                        apiService.create("VBSC_Master_SportsCCGroupName/deactive", item).
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
        
        //--------------------------------------------------------------------------------

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $state.reload();
        };

        $scope.cancel = function () {
            $state.reload();
        };
        
        $scope.edit = function (item) {
            $scope.mT_Id = item.mT_Id;
            $scope.vbscmsccG_SportsCCGroupName = item.vbscmsccG_SportsCCGroupName;
            $scope.vbscmsccG_SportsCCGroupDesc = item.vbscmsccG_SportsCCGroupDesc;
            $scope.vbscmsccG_SCCFlag = item.vbscmsccG_SCCFlag;
            $scope.vbscmsccG_Id = item.vbscmsccG_Id;
        }
        
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';

    }
})();