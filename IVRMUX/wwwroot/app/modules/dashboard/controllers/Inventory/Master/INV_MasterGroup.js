
(function () {
    'use strict';
    angular
        .module('app')
        .controller('INV_MasterGroupController', INV_MasterGroupController);
    INV_MasterGroupController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];
    function INV_MasterGroupController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {


        $scope.searchValue = "";
        $scope.searchValueU = "";
        $scope.searchValueI = "";
        $scope.productName = "";

        var paginationformasters;
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


        //-----------------------------------------------------------------------
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            $scope.currentPageU = 1;
            $scope.itemsPerPageU = paginationformasters;

            $scope.currentPageI = 1;
            $scope.itemsPerPageI = paginationformasters;
            $scope.search = "";
            $scope.presentCountgrid = "";
            $scope.presentCountgridU = "";
            $scope.presentCountgridI = "";
            var pageid = 2;
            apiService.getURI("INV_MasterGroup/getloaddata", pageid).
                then(function (promise) {
                    $scope.get_maingroup = promise.get_maingroup;
                    $scope.presentCountgrid = $scope.get_maingroup.length;
                    $scope.get_maingroupdd = promise.get_maingroupdd;
                    $scope.get_usergroup = promise.get_usergroup;
                    $scope.presentCountgridU = $scope.get_usergroup.length;

                    $scope.get_itemgroup = promise.get_itemgroup;
                    $scope.presentCountgridI = $scope.get_itemgroup.length;

                    $scope.get_usergp = promise.get_usergp;
                });
        };
        //---------------------------------Save--------------------------------------------
        //Main Group
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "INVMG_GroupName": $scope.invmG_GroupName,
                    "INVMG_AliasName": $scope.invmG_AliasName,
                    "INVMG_MGUGIGFlg": "MG",
                    "INVMG_Id": $scope.invmG_Id,
                    "INVMG_GroupPrefix": $scope.INVMG_GroupPrefix,
                    "INVMG_GroupSuffix": $scope.INVMG_GroupSuffix,
                    "INVMG_GroupStartingNo": $scope.INVMG_GroupStartingNo,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("INV_MasterGroup/savedetails", data).then(function (promise) {

                    if (promise.returnval == true) {
                        if (promise.invmG_Id == 0 || promise.invmG_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.invmG_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus == 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.invmG_Id == 0 || promise.invmG_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.invmG_Id > 0) {
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

        //User Group
        $scope.savedataUG = function () {
            $scope.submittedUG = true;
            if ($scope.myForm1.$valid) {
                var data = {
                    "INVMG_GroupName": $scope.invmG_GroupNameU,
                    "INVMG_AliasName": $scope.invmG_AliasNameU,
                    "INVMG_ParentId": $scope.invmG_Idu,
                    "INVMG_Id": $scope.invmG_Idug,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("INV_MasterGroup/savedetailsUG", data).then(function (promise) {

                    if (promise.returnval == true) {
                        if (promise.invmG_Id == 0 || promise.invmG_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.invmG_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus == 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.invmG_Id == 0 || promise.invmG_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.invmG_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }
                    $scope.cancelUG();
                    $scope.loaddata();
                    $scope.submittedUG = false;
                })
            }
            else {
                $scope.submittedUG = true;
            }
        };

        //Item Group
        $scope.groupChange = function () {
            var data = {
                "INVMG_ParentId": $scope.invmG_Idgi,
            }
            apiService.create("INV_MasterGroup/groupChange", data).
                then(function (promise) {
                    $scope.getusergroup = promise.getusergroup;
                })
        }
       
        $scope.savedataIG = function () {
            $scope.submittedIG = true;
            if ($scope.myForm2.$valid) {
                var data = {
                    "INVMG_GroupName": $scope.invmG_GroupNameI,
                    "INVMG_AliasName": $scope.invmG_AliasNameI,
                    "INVMG_ParentId": $scope.invmG_Idi,
                    "INVMG_Id": $scope.invmG_Idig,
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("INV_MasterGroup/savedetailsIG", data).then(function (promise) {

                    if (promise.returnval == true) {
                        if (promise.invmG_Id == 0 || promise.invmG_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.invmG_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus == 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.invmG_Id == 0 || promise.invmG_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.invmG_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }
                
                    $scope.cancelIG();
                    $scope.loaddata();
                    $scope.submittedIG = false;
                });
            }
            else {
                $scope.submittedIG = true;
            }
        };

        $scope.deactive = function (item, SweetAlert) {
            $scope.INVMG_Id = item.invmG_Id;
            var dystring = "";
            if (item.invmG_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (item.invmG_ActiveFlg == false) {
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
                        apiService.create("INV_MasterGroup/deactive", item).
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
                        swal("Record " + dystring + "d Cancelled!!!");
                    }
                });
        }
        $scope.deactiveU = function (item, SweetAlert) {
            $scope.INVMG_Id = item.invmG_Id;
            var dystring = "";
            if (item.invmG_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (item.invmG_ActiveFlg == false) {
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
                        apiService.create("INV_MasterGroup/deactive", item).
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
        $scope.deactiveI = function (item, SweetAlert) {
            $scope.INVMG_Id = item.invmG_Id;
            var dystring = "";
            if (item.invmG_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (item.invmG_ActiveFlg == false) {
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
                        apiService.create("INV_MasterGroup/deactive", item).
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

        $scope.edit = function (item) {
            $scope.invmG_Id = item.invmG_ParentId;
            $scope.invmG_GroupName = item.invmG_GroupName;
            $scope.invmG_AliasName = item.invmG_AliasName;
            $scope.INVMG_GroupPrefix = item.invmG_GroupPrefix;
            $scope.INVMG_GroupSuffix = item.invmG_GroupSuffix;
            $scope.INVMG_GroupStartingNo = item.invmG_GroupStartingNo;
            $scope.invmG_Idug = item.invmG_Id;
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.submittedUG = false;
            $scope.myForm1.$setPristine();
            $scope.myForm1.$setUntouched();
            $scope.submittedIG = false;
            $scope.myForm2.$setPristine();
            $scope.myForm2.$setUntouched();
        };

        $scope.editU = function (item) {
            $scope.invmG_Idu = item.invmG_ParentId;
            $scope.invmG_GroupNameU = item.invmG_GroupName;
            $scope.invmG_AliasNameU = item.invmG_AliasName;
            $scope.invmG_Idug = item.invmG_Id;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.submittedUG = false;
            $scope.myForm1.$setPristine();
            $scope.myForm1.$setUntouched();
            $scope.submittedIG = false;
            $scope.myForm2.$setPristine();
            $scope.myForm2.$setUntouched();
        };

        $scope.editI = function (item) {
            $scope.invmG_Idi = item.invmG_ParentId;
            $scope.invmG_GroupNameI = item.invmG_GroupName;
            $scope.invmG_AliasNameI = item.invmG_AliasName;
            $scope.invmG_Idig = item.invmG_Id;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.submittedUG = false;
            $scope.myForm1.$setPristine();
            $scope.myForm1.$setUntouched();
            $scope.submittedIG = false;
            $scope.myForm2.$setPristine();
            $scope.myForm2.$setUntouched();
        };


        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.interacted1 = function (field) {
            return $scope.submittedUG;
        };
        $scope.interacted2 = function (field) {
            return $scope.submittedIG;
        };
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';
        $scope.searchValueU = '';
        $scope.searchValueI = '';

        $scope.cancel = function () {
            $state.reload();
        }

        $scope.cancelUG = function () {
            $scope.invmG_Idu = "";
            $scope.invmG_GroupNameU = "";
            $scope.invmG_AliasNameU = "";
            $scope.submittedUG = false;
        }

        $scope.cancelIG = function () {
            $scope.invmG_Idi = "";
            $scope.invmG_GroupNameI = "";
            $scope.invmG_AliasNameI = "";
            $scope.submittedIG = false;
        }


        //======================Models===========================//
        $scope.onclick = function (id) {
            var data = {
                "INVMG_ParentId": id.invmG_ParentId,
                "INVMG_Id": id.invmG_Id
            }
            apiService.create("INV_MasterGroup/usergroup", data).
                then(function (promise) {
                    $scope.get_usergroup = promise.get_usergroup;
                    $scope.maingrp = promise.maingrp;
                    $scope.mainG = $scope.maingrp[0].invmG_GroupName
                })
        }
        $scope.onclickUser = function (id) {
            var data = {
                "INVMG_ParentId": id.invmG_ParentId,
                "INVMG_Id": id.invmG_Id
            }
            apiService.create("INV_MasterGroup/Itemgroup", data).
                then(function (promise) {
                    $scope.get_itemgroup = promise.get_itemgroup;
                    $scope.usergrp = promise.usergrp;
                    $scope.userG = $scope.usergrp[0].invmG_GroupName;
                })
        }

        //------------------------------
    }
})();