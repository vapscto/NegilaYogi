
(function () {
    'use strict';
    angular
        .module('app')
        .controller('AT_MasterLocationController', AT_MasterLocationController);
    AT_MasterLocationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function AT_MasterLocationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {



        var paginationformasters;
        $scope.obj = {};
        $scope.editS = false;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        else {
            paginationformasters = 0;
        }

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        //-------------------------------------------------------------------

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("AT_MasterLocation/getloaddata", pageid).
                then(function (promise) {
                    $scope.get_sites = promise.get_sites;
                    $scope.get_employee = promise.get_employee;
                    $scope.get_locations = promise.get_locations;
                    $scope.presentCountgrid = $scope.get_locations.length;
                });
        };
        //============================================ SAVE EDIT

        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {};
                if ($scope.contactpersonflag === "E") {
                    $scope.empid = $scope.obj.hrmE_Id.hrmE_Id;
                    $scope.invmlO_InchargeName = $scope.obj.hrmE_Id.employeename;
                    data = {
                        "INVMSI_Id": $scope.invmsI_Id,
                        "INVMLO_LocationRoomName": $scope.invmlO_LocationRoomName,
                        "INVMLO_LocationRemarks": $scope.invmlO_LocationRemarks,
                        "HRME_Id": $scope.empid,
                        "INVMLO_InchargeName": $scope.invmlO_InchargeName,
                        "INVMLO_Id": $scope.invmlO_Id
                    };
                }
                else {
                    data = {
                        "INVMSI_Id": $scope.invmsI_Id,
                        "INVMLO_LocationRoomName": $scope.invmlO_LocationRoomName,
                        "INVMLO_LocationRemarks": $scope.invmlO_LocationRemarks,
                        "HRME_Id": "",
                        "INVMLO_InchargeName": $scope.invmlO_InchargeName,
                        "INVMLO_Id": $scope.invmlO_Id
                    };
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("AT_MasterLocation/savedetails", data).then(function (promise) {

                    if (promise.returnval === true) {
                        if (promise.invmlO_Id === 0 || promise.invmlO_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.invmlO_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus === 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.invmlO_Id === 0 || promise.invmlO_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.invmlO_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }
                    $state.reload();
                });
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
        };



        $scope.deactive = function (item, SweetAlert) {
            $scope.INVMLO_Id = item.invmlO_Id;
            var dystring = "";
            if (item.invmlO_ActiveFlg === true) {
                dystring = "Deactivate";
            }
            else if (item.invmlO_ActiveFlg === false) {
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
                        apiService.create("AT_MasterLocation/deactive", item).
                            then(function (promise) {
                                if (promise.returnval === true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            });
                    }
                    else {
                        swal("Record " + dystring + " Cancelled!!!");
                    }
                });
        };

        $scope.edit = function (item) {
            $scope.editS = true;
            $scope.invmsI_Id = item.invmsI_Id;
            $scope.invmsI_SiteBuildingName = item.invmsI_SiteBuildingName;
            $scope.invmlO_LocationRoomName = item.invmlO_LocationRoomName;
            $scope.invmlO_LocationRemarks = item.invmlO_LocationRemarks;
            $scope.obj.hrmE_Id = item;
            if (item.hrmE_Id > 0 || item.hrmE_Id !== null) {
                $scope.contactpersonflag = 'E';
                // $scope.obj.hrmE_Id.employeename = item;

                $scope.obj.hrmE_Id.employeename = $scope.obj.hrmE_Id.invmlO_InchargeName;
            }
            else {
                $scope.contactpersonflag = 'O';
                $scope.obj.hrmE_Id.invmlO_InchargeName = item;
            }

            //  $scope.obj.invmlO_InchargeName = item;
            $scope.invmlO_Id = item.invmlO_Id;

            var data = {
                "INVMLO_Id": item.invmlO_Id
            };
            apiService.create("AT_MasterLocation/getcontactperson", data).
                then(function (promise) {
                    $scope.get_contactperson = promise.get_contactperson;
                    $scope.contactpersonflag = promise.contactflag;
                    $scope.invmlO_Id = $scope.get_contactperson[0].invmlO_Id;
                    //  $scope.obj.hrmE_Id = $scope.get_contactperson[0].hrmE_Id;
                    $scope.invmlO_InchargeName = $scope.get_contactperson[0].invmlO_InchargeName;
                });
        };

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';
    }
})();