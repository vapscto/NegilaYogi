(function () {
    'use strict';
    angular
        .module('app')
        .controller('FAUserGroupController', FAUserGroupController)

    FAUserGroupController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$stateParams', '$filter', 'superCache', '$q']
    function FAUserGroupController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $stateParams, $filter, superCache, $q) {
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.itemsPerPagetwo = paginationformasters;
        $scope.coptyright = copty;
        $scope.currentPage = 1;
        $scope.currentPagetwo = 1;
        $scope.itemsPerPage = 10;
        $scope.itemsPerPagetwo = 10;
        $scope.searchvalue = "";
        $scope.search = "";
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("FAUser_Group/getalldetails", pageid).then(function (promise) {
                $scope.getreport = promise.getreport;
                $scope.presentCountgrid = promise.getreport.length;
                $scope.getgroupname = promise.getgroupname;
                $scope.fyear = promise.fyear;
                $scope.companyname = promise.companyname;
                $scope.usergroupname = promise.usergroupname;

            })
        };
        $scope.submittedtwo = false;
        $scope.savepagestwo = function () {
            $scope.submittedtwo = true;
            if ($scope.myFormtwo.$valid) {
                var FAUGRP_IdTwo = 0;
                if ($scope.FAUGRP_IdTwo > 0) {
                    FAUGRP_IdTwo = $scope.FAUGRP_IdTwo;
                }
                var data = {
                    "FAUGRP_IdTwo": FAUGRP_IdTwo,
                    "FAUGRP_UserGroupName": $scope.FAUGRP_UserGroupNametwo,
                    "FAUGRP_AliasName": $scope.FAUGRP_AliasNametwo,
                    "FAUGRP_Description": $scope.FAUGRP_Descriptiontwo,
                    "FAUGRP_Position": $scope.FAUGRP_Position,
                    "FAUGRP_Id": $scope.FAUGRP_IdThree,
                    //"FAMCOMP_Id": $scope.FAMCOMP_Id,
                    //"IMFY_Id": $scope.IMFY_Id,
                    //"FAMGRP_Id": $scope.FAMGRP_Id
                }
                apiService.create("FAUser_Group/savedatatwo", data).
                    then(function (promise) {
                        if (promise.returnval == "save") {
                            swal('Record Saved Successfully !');
                        }
                        else if (promise.returnval == "Notsave") {
                            swal('Records Not Saved !');
                        }
                        else if (promise.returnval == "update") {
                            swal('Records Updated Successfully !');
                        }
                        else if (promise.returnval == "Notupdate") {
                            swal('Records Not Updated  !');
                        }
                        else if (promise.returnval == "exit") {
                            swal('Records Already Exist !');
                        }
                        else if (promise.returnval == "admin") {
                            swal('Please Contact  Administrator  !');
                        }
                        $scope.cleartwo();
                       

                    })
            }

        };
        //FAMasterGroup
        $scope.submitted = false;
        $scope.savepages = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var FAUGRP_Idone = 0;
                if ($scope.FAUGRP_IdOne > 0) {
                    FAUGRP_Idone = $scope.FAUGRP_IdOne;
                }

                var data = {
                    "FAUGRP_Idone": FAUGRP_Idone,
                    "FAUGRP_UserGroupName": $scope.FAUGRP_UserGroupName,
                    "FAUGRP_AliasName": $scope.FAUGRP_AliasName,
                    "FAUGRP_Description": $scope.FAUGRP_Description,
                    "FAMCOMP_Id": $scope.FAMCOMP_Id,
                    "IMFY_Id": $scope.IMFY_Id,
                    "FAMGRP_Id": $scope.FAMGRP_Id
                }
                apiService.create("FAUser_Group/savedata", data).
                    then(function (promise) {
                        if (promise.returnval == "save") {
                            swal('Record Saved Successfully !');
                        }
                        else if (promise.returnval == "Notsave") {
                            swal('Records Not Saved !');
                        }
                        else if (promise.returnval == "update") {
                            swal('Records Updated Successfully !');
                        }
                        else if (promise.returnval == "Notupdate") {
                            swal('Records Not Updated  !');
                        }
                        else if (promise.returnval == "exit") {
                            swal('Records Already Exist !');
                        }
                        else if (promise.returnval == "admin") {
                            swal('Please Contact  Administrator  !');
                        }
                        $state.reload();
                    })
            }
        };
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        //myFormtwo
        $scope.interactedtwo = function (field) {
            return $scope.submittedtwo || field.$dirty;
        };        //

        //Deactive
        $scope.Deletedata = function (item, SweetAlert) {

            var mgs = "";
            var confirmmgs = "";

            var data = {
                "FAUGRP_Id": item.faugrP_Id
            }
            var dystring = "";
            if (item.faugrP_ActiveFlg == true) {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";
            }
            else if (item.faugrP_ActiveFlg == false) {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + mgs + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("FAUser_Group/Deletedetails", data).
                            then(function (promise) {
                                if (promise.returnval == "active") {
                                    swal("Record " + confirmmgs + " Successfully!!!");
                                }
                                else if (promise.returnval == "notactive") {
                                    swal("Record Not  Active / Deactive  !!!");
                                }
                                else if (promise.returnval == "admin") {
                                    swal('Please Contact  Administrator  !');
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record  " + confirmmgs + " Cancelled!!!");
                    }
                });
        }
        $scope.edit = function (user) {
            $scope.FAUGRP_UserGroupName = user.faugrP_UserGroupName;
            $scope.FAUGRP_AliasName = user.faugrP_AliasName;
            $scope.FAUGRP_IdOne = user.faugrP_Id;
            $scope.FAMGRP_Id = user.famgrP_Id;
            $scope.FAUGRP_Description = user.faugrP_Description;
            $scope.FAMCOMP_Id = user.famcomP_Id;
            $scope.IMFY_Id = user.imfY_Id;
        };
        $scope.clear = function () {
            $state.reload();
        }
        $scope.cleartwo = function () {
            $scope.FAUGRP_IdTwo = ""; $scope.FAUGRP_UserGroupNametwo = "";
            $scope.FAUGRP_AliasNametwo = ""; $scope.FAUGRP_Descriptiontwo = "";
            $scope.FAUGRP_Position = "";
            $scope.submittedtwo = false;
        };

        //deactibetwo
        $scope.Deletedatatwo = function (itemtwo, SweetAlert) {
            var data = {
                "FAUGRP_IdTwo": itemtwo.faugrP_Id,
            } 
            var dystring = "";
            if (itemtwo.faugrP_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (itemtwo.faugrP_ActiveFlg == false) {
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
                        apiService.create("FAUser_Group/Deletedetails", data).
                            then(function (promise) {
                                if (promise.returnval == "active") {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else if (promise.returnval == "notactive") {
                                    swal("Record Not  Active / Deactive  !!!");
                                }
                                else if (promise.returnval == "admin") {
                                    swal('Please Contact  Administrator  !');
                                }
                               // $scope.FAUGRPChange();
                            })
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        }
        $scope.editwo = function (usertwo) {
            $scope.FAUGRP_IdTwo = usertwo.faugrP_Id;
            $scope.FAUGRP_IdThree = usertwo.faugrP_ParentId;
            $scope.FAUGRP_UserGroupNametwo = usertwo.faugrP_UserGroupName;
            $scope.FAUGRP_AliasNametwo = usertwo.faugrP_AliasName;
            $scope.FAUGRP_Position = usertwo.faugrP_Position;
            $scope.FAUGRP_Descriptiontwo = usertwo.faugrP_Description;
           // $scope.FAMCOMP_Id = usertwo.famcomP_Id;
           // $scope.IMFY_Id = usertwo.imfY_Id;
           
           // $scope.FAUGRP_IdOne = usertwo.faugrP_Id;

        };
        $scope.FAUGRPChange = function () {
            $scope.getreporttwo = [];
            var data = {
                "FAUGRP_Id": $scope.FAUGRP_IdThree
            }
            apiService.create("FAUser_Group/Userchange", data).
                then(function (promise) {

                    if (promise.getreporttwo != null && promise.getreporttwo.length > 0) {
                        $scope.getreporttwo = promise.getreporttwo;
                    }
                    else if (promise.returnval == "admin") {
                        swal('Please Contact  Administrator  !');
                    }

                })

        };
    }
    //FAMGRP_Idtwo
})(); 