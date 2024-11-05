(function () {
    'use strict';
    angular
        .module('app')
        .controller('FAMasterGroupController', FAMasterGroupController)

    FAMasterGroupController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$stateParams', '$filter', 'superCache', '$q']
    function FAMasterGroupController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $stateParams, $filter, superCache, $q) {
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
            apiService.getURI("FAMasterGroup/getalldetails", pageid).then(function (promise) {
                $scope.getreport = promise.getreport;
                $scope.presentCountgrid = promise.getreport.length;
                $scope.getgroupname = promise.getgroupname;

            })
        };
        $scope.submittedtwo = false;
        $scope.savepagestwo = function () {
            $scope.submittedtwo = true;
            if ($scope.myFormtwo.$valid) {
                var FAMGRP_IdTwo = 0;
                if ($scope.FAMGRP_Idtwo > 0) {
                    FAMGRP_IdTwo = $scope.FAMGRP_Idtwo;
                }
                var data = {
                    "FAMGRP_GroupName": $scope.FAMGRP_GroupNametwo,
                    "FAMGRP_GroupCode": $scope.FAMGRP_GroupCodetwo,  
                    "FAMGRP_Description": $scope.FAMGRP_Descriptiontwo,
                    "FAMGRP_BSPLFlg": $scope.FAMGRP_BSPLFlgThree,
                    "FAMGRP_CRDRFlg": $scope.FAMGRP_CRDRFlgThree,
                    "FAMGRP_Id": $scope.FAMGRP_IdThree,
                    "FAMGRP_Position": $scope.FAMGRP_Position,
                    "FAMGRP_IdTwo": FAMGRP_IdTwo
                }
                apiService.create("FAMasterGroup/savedatatwo", data).
                    then(function (promise) {
                        if (promise.returnval == "save") {
                            swal('Record Save/ Update Successfully ');
                        }
                        else if (promise.returnval == "Notsave") {
                            swal('Records Not Saved !');
                        }
                        else if (promise.returnval == "update") {
                            swal('Records Save/ Update Successfully ');
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
                var FAMGRP_Id = 0;               
                var FAMGRP_BSPLFlg = "";
                var FAMGRP_CRDRFlg = "";
                if ($scope.FAMGRP_BSPLFlgBalance == true) {
                    FAMGRP_BSPLFlg = "Balance Sheet";
                }
                if ($scope.FAMGRP_CRDRFlgProfit == true) {
                    FAMGRP_BSPLFlg = "Profit & Loss";
                }
                if ($scope.FAMGRP_CRDRFlgCr == true) {
                    FAMGRP_CRDRFlg = "CR";
                }
                if ($scope.FAMGRP_CRDRFlgDr == true) {
                    FAMGRP_CRDRFlg = "DR";
                }
                if ($scope.FAMGRP_IdOne > 0) {
                    FAMGRP_Id = $scope.FAMGRP_IdOne;
                }
                var data = {
                    "FAMGRP_GroupName": $scope.FAMGRP_GroupName,
                    "FAMGRP_GroupCode": $scope.FAMGRP_GroupCode,
                    "FAMGRP_Description": $scope.FAMGRP_Description,
                    "FAMGRP_BSPLFlg": FAMGRP_BSPLFlg,
                    "FAMGRP_CRDRFlg": FAMGRP_CRDRFlg,
                    "FAMGRP_Id": FAMGRP_Id
                }
                apiService.create("FAMasterGroup/savedata", data).
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
        };       
        $scope.FAMGRPChange = function () {
            $scope.getreporttwo = [];
            var data = {               
                "FAMGRP_Id": $scope.FAMGRP_IdThree
            }
            apiService.create("FAMasterGroup/edit", data).
                then(function (promise) {
                    $scope.getreporttwo = [];
                    if (promise.getreporttwo != null && promise.getreporttwo.length > 0) {
                        $scope.getreporttwo = promise.getreporttwo;
                    }
                    else if (promise.returnval == "admin") {
                        swal('Please Contact  Administrator  !');
                    }
                 
                    })

        };
        //Deactive
        $scope.Deletedata = function (item, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";

            var data = {
                "FAMGRP_Id": item.FAMGRP_Id
            }
           
            if (item.FAMGRP_ActiveFlg == true) {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";

            }
            else if (item.FAMGRP_ActiveFlg == false) {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
          swal({
                title: "Are you sure?",
                text: "Do you want to " + mgs + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("FAMasterGroup/Deletedetails", data).
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
            $scope.FAMGRP_GroupName = user.FAMGRP_GroupName;
            $scope.FAMGRP_GroupCode = user.FAMGRP_GroupCode;
            $scope.FAMGRP_Description = user.FAMGRP_Description;
            $scope.FAMGRP_IdOne = user.FAMGRP_Id;
            if (user.FAMGRP_BSPLFlg == "Balance Sheet") {
                $scope.FAMGRP_BSPLFlgBalance = true;
            }
            else if (user.FAMGRP_BSPLFlg == "Profit & Loss") {
                $scope.FAMGRP_CRDRFlgProfit = true;
            }
            if (user.FAMGRP_CRDRFlg == "CR") {
                $scope.FAMGRP_CRDRFlgCr = true;
            }
            else if (user.FAMGRP_CRDRFlg == "DR") {
                $scope.FAMGRP_CRDRFlgDr = true;
            }
        };
        $scope.clear = function () {
            $state.reload();
        }
        $scope.cleartwo = function () {
            
            $scope.FAMGRP_Id = ""; $scope.FAMGRP_Descriptiontwo = "";
            $scope.FAMGRP_Idtwo = ""; $scope.FAMGRP_CRDRFlgThree = "";
            $scope.FAMGRP_BSPLFlgThree = ""; $scope.FAMGRP_Position = "";
            $scope.FAMGRP_GroupCodetwo = ""; $scope.FAMGRP_GroupNametwo = "";
            $scope.submittedtwo = false;
            $scope.loaddata();
        };
        $scope.balance = function () {
            if ($scope.FAMGRP_BSPLFlgBalance == true) {
                $scope.FAMGRP_CRDRFlgProfit = false;
            }
            else {
                $scope.FAMGRP_CRDRFlgProfit = true;
            }
        };
        $scope.profit = function () {
            if ($scope.FAMGRP_CRDRFlgProfit == true) {
                $scope.FAMGRP_BSPLFlgBalance = false;
            }
            else {
                $scope.FAMGRP_BSPLFlgBalance = true;
            }
        };
        $scope.CRclick = function () {
            if ($scope.FAMGRP_CRDRFlgCr == true) {
                $scope.FAMGRP_CRDRFlgDr = false;
            }
            else {
                $scope.FAMGRP_CRDRFlgDr = true;
            }
        };
        $scope.DRclick = function () {
            if ($scope.FAMGRP_CRDRFlgDr == true) {
                $scope.FAMGRP_CRDRFlgCr = false;
            }
            else {
                $scope.FAMGRP_CRDRFlgCr = true;
            }
        };
        //deactibetwo
        $scope.Deletedatatwo = function (itemtwo, SweetAlert) {
            var data = {
                "FAMGRP_IdTwo": itemtwo.famgrP_Id
            }
            var dystring = "";
            if (itemtwo.famgrP_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (itemtwo.famgrP_ActiveFlg == false) {
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
                        apiService.create("FAMasterGroup/Deletedetails", data).
                            then(function (promise) {
                                if (promise.returnval == "active") {
                                    swal("Record " + dystring + " Successfully!!!");
                                }
                                else if (promise.returnval == "notactive") {
                                    swal("Record Not  Active / Deactive  !!!");
                                }
                                else if (promise.returnval == "admin") {
                                    swal('Please Contact  Administrator  !');
                                }
                                $scope.FAMGRPChange();
                            })
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        }
        $scope.editwo = function (usertwo) {
            $scope.FAMGRP_IdThree = usertwo.famgrP_ParentId;
            $scope.FAMGRP_Position = usertwo.famgrP_Position;
            $scope.FAMGRP_Idtwo = usertwo.famgrP_Id;
            $scope.FAMGRP_GroupNametwo = usertwo.famgrP_GroupName;
            $scope.FAMGRP_GroupCodetwo = usertwo.famgrP_GroupCode;
            $scope.FAMGRP_BSPLFlgThree = usertwo.famgrP_BSPLFlg;
            $scope.FAMGRP_CRDRFlgThree = usertwo.famgrP_CRDRFlg;
            $scope.FAMGRP_Descriptiontwo = usertwo.famgrP_Description;

        };
    }
   
})(); 