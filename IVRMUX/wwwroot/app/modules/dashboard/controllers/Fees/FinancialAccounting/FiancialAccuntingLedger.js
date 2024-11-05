(function () {
    'use strict';
    angular
        .module('app')
        .controller('FiancialAccuntingLedgerController', FiancialAccuntingLedgerController)

    FiancialAccuntingLedgerController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$stateParams', '$filter', 'superCache', '$q']
    function FiancialAccuntingLedgerController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $stateParams, $filter, superCache, $q) {
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.obj = {};
        $scope.FAMLED_LedgerCreatedDate = new Date();
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.transrows = [{ itrS_Id: 'trans1' }];
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("FiancialAccuntingLedger/getalldetails", pageid).then(function (promise) {
                $scope.getreport = promise.getreport;
                $scope.getgroupname = promise.getgroupname;
                $scope.companyname = promise.companyname;
                $scope.fyear = promise.fyear;
            })
        };
        $scope.submitted = false;
        $scope.savepages = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                // var FAMLED_Id = 0;
                var FAMLED_BillwiseFlg = 0;
                if ($scope.obj.FAMLED_BillwiseFlg == true) {
                    FAMLED_BillwiseFlg = 1;
                }
                //if ($scope.FAMLED_Id > 0) {
                //    FAMLED_Id = $scope.FAMLED_Id;
                //}
                var data = {
                    "FAMLED_Id": $scope.FAMLED_Id,
                    "FAMCOMP_Id": $scope.FAMCOMP_Id.famcomP_Id,
                    "IMFY_Id": $scope.IMFY_Id,
                    "FAMGRP_Id": $scope.FAMGRP_Id.FAMGRP_Id,
                    // "FAUGRP_Id": $scope.FAUGRP_Id.faugrP_Id,
                    "FAMLED_LedgerName": $scope.FAMLED_LedgerName,
                    "FAMLED_LedgerAliasName": $scope.FAMLED_LedgerAliasName,
                    "FAMLED_LedgerCreatedDate": new Date($scope.FAMLED_LedgerCreatedDate).toDateString(),
                    "FAMLED_Remarks": $scope.FAMLED_Remarks,
                    "FAMLED_Type": $scope.FAMLED_Type,
                    "FAMLED_PostalAddress": $scope.FAMLED_PostalAddress,
                    "FAMLED_EmailAddress": $scope.FAMLED_EmailAddress,
                    "FAMLED_Under": $scope.FAMLED_Under,
                    "FAMLED_BillwiseFlg": FAMLED_BillwiseFlg,
                    "FAMLEDD_OpeningBalance": $scope.FAMLEDD_OpeningBalance,
                    "FAMLEDD_OBCRDRFlg": $scope.FAMLEDD_OBCRDRFlg,

                     // "ledgerdetails": $scope.transrows

                }
                apiService.create("FiancialAccuntingLedger/savedata", data).
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
        //
        //Deactive
        $scope.Deletedata = function (item, SweetAlert) {
            var data = {
                "FAMLED_Id": item.FAMLED_Id
            }
            var dystring = "";
            if (item.FAMLED_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (item.FAMLED_ActiveFlg == false) {
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
                        apiService.create("FiancialAccuntingLedger/Deletedetails", data).
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
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        }

        $scope.edit = function (user) {
           //editledger
            var data = {
                "FAMLED_Id": user.FAMLED_Id
            }
            apiService.create("FiancialAccuntingLedger/edit", data).
                then(function (promise) {
                    $scope.FAMCOMP_Id = promise.editledger[0].famcomP_Id;
                    $scope.IMFY_Id = promise.editledger[0].imfY_Id;
                    $scope.FAMGRP_Id = promise.editledger[0].famgrP_Id;                    
                    $scope.FAMLED_Id = promise.editledger[0].famleD_Id;
                    $scope.FAMLED_LedgerName = promise.editledger[0].famleD_LedgerName;
                    $scope.FAMLED_LedgerAliasName = promise.editledger[0].famleD_LedgerAliasName;
                    $scope.FAMLED_LedgerCreatedDate = new Date(promise.editledger[0].famleD_LedgerCreatedDate);
                    $scope.FAMLED_Remarks = promise.editledger[0].famleD_Remarks;
                    $scope.FAMLED_PostalAddress = promise.editledger[0].famleD_PostalAddress;
                    $scope.FAMLED_EmailAddress = promise.editledger[0].famleD_EmailAddress;
                    $scope.FAMLED_Type = promise.editledger[0].famleD_Type;
                    $scope.obj.FAMLED_BillwiseFlg = promise.editledger[0].famleD_BillwiseFlg;
                    $scope.FAMLED_Under = promise.editledger[0].famleD_Under;                    
                    if (promise.editledger[0].FAMLED_BillwiseFlg == 1) {
                        $scope.FAMLED_BillwiseFlg = true;
                    }
                    else {
                        $scope.FAMLED_BillwiseFlg = false;
                    }

                    if ($scope.companyname != null && $scope.companyname.length > 0) {
                        for (var i = 0; i < $scope.companyname.length; i++) {
                            if (promise.editledger[0].famcomP_Id == $scope.companyname[i].famcomP_Id) {
                                $scope.companyname[i].Selected = true;
                                $scope.FAMCOMP_Id = $scope.companyname[i];
                            }
                        }
                    }

                    if ($scope.getgroupname != null && $scope.getgroupname.length > 0) {
                        for (var i = 0; i < $scope.getgroupname.length; i++) {
                            if (promise.editledger[0].famgrP_Id == $scope.getgroupname[i].FAMGRP_Id) {
                                $scope.getgroupname[i].Selected = true;
                                $scope.FAMGRP_Id = $scope.getgroupname[i];
                            }
                        }
                    }


                    if ($scope.usergroupname != null && $scope.usergroupname.length > 0) {
                        for (var i = 0; i < $scope.usergroupname.length; i++) {
                            if (promise.editledger[0].faugrP_Id == $scope.getgroupname[i].faugrP_Id) {
                                $scope.getgroupname[i].Selected = true;
                                $scope.FAUGRP_Id = $scope.getgroupname[i];
                            }
                        }
                    }

                    if (promise.editledgerdetail != null && promise.editledgerdetail.length > 0) {
                        $scope.FAMLEDD_OpeningBalance = promise.editledgerdetail[0].famledD_OpeningBalance;
                        $scope.FAMLEDD_OBCRDRFlg = promise.editledgerdetail[0].famledD_OBCRDRFlg;

                    }
                   
                   // $scope.FAMGRPChange();
                   // $scope.FAUGRP_Id = promise.editledger[0].faugrP_Id;
                })

        };
        $scope.clear = function () {
            $state.reload();
        };
        $scope.transrows = [{ itrS_Id: 'trans1' }];
        if ($scope.transrows.length === 1) {
            $scope.cnt = 1;
        }
        $scope.addgrnrows = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                if ($scope.transrows.length > 1) {
                    for (var i = 0; i === $scope.transrows.length; i++) {
                        var id = $scope.transrows[i].itrS_Id;
                        var lastChar = id.substr(id.length - 1);
                        $scope.cnt = parseInt(lastChar);
                    }
                }
                $scope.cnt = $scope.cnt + 1;
                $scope.tet = 'trans' + $scope.cnt;
                var newItemNo = $scope.cnt;
                $scope.transrows.push({ 'itrS_Id': 'trans' + newItemNo });
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.FAMGRPChange = function () {
            $scope.getreporttwo = [];
            $scope.usergroupname = [];
            var data = {
                "FAMGRP_Id": $scope.FAMGRP_Id.FAMGRP_Id,
            }
            apiService.create("FiancialAccuntingLedger/savedatatwo", data).
                then(function (promise) {

                    if (promise.usergroupname != null && promise.usergroupname.length > 0) {
                        $scope.usergroupname = promise.usergroupname;
                    }
                    else if (promise.returnval == "admin") {
                        swal('Please Contact  Administrator  !');
                    }
                    else  {
                        swal('Record Not Found  !');
                    }
                    

                })

        };

        $scope.removegrnrows = function (index, data) {
            var newItemNo = $scope.transrows.length - 1;
            $scope.transrows.splice(index, 1);
            
        };
    }

})();