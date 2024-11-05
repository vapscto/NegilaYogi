(function () {
    'use strict';
    angular
        .module('app')
        .controller('AutoLedgerCreationController', AutoLedgerCreationController)

    AutoLedgerCreationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$stateParams', '$filter', 'superCache', '$q']
    function AutoLedgerCreationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $stateParams, $filter, superCache, $q) {
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

        $scope.optionType = 'Student';
        $scope.transrows = [{ itrS_Id: 'trans1' }];

        $scope.toggleAll = function () {
            var toggleStatus = $scope.selectAll;
            angular.forEach($scope.studentlst, function (role) { role.selected = toggleStatus; });

        }

        $scope.toggleAllSales = function () {
            var toggleStatus = $scope.selectAllsales;
            angular.forEach($scope.supplierdata, function (role) { role.selected = toggleStatus; });

        }
        $scope.toggleAllItem = function () {
            var toggleStatus = $scope.selectAllitem;
            angular.forEach($scope.itemdata, function (role) { role.selected = toggleStatus; });

        }
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("AutoLedgerCreation/getalldetails", pageid).then(function (promise) {
                $scope.getreport = promise.getreport;
                $scope.getgroupname = promise.getgroupname;
                $scope.companyname = promise.companyname;
                $scope.fyear = promise.fyear;
                $scope.classarr = promise.classarr;
                $scope.sectionarr = promise.sectionarr;
                $scope.supplierdata = promise.supplierdata;
                $scope.itemdata = promise.itemdata;
            })
        };
        $scope.submitted = false;
        $scope.savepages = function () {
            $scope.submitted = true;
            $scope.albumNameArray = [];
            angular.forEach($scope.studentlst, function (role) {
                if (!!role.selected)
                    $scope.albumNameArray.push(role.amst_Id);
            })
            $scope.albumNamesales = [];
            angular.forEach($scope.supplierdata, function (role) {
                if (!!role.selected) $scope.albumNamesales.push(role.invmS_Id);
            })
            $scope.albumNameitem = [];
            angular.forEach($scope.itemdata, function (role) {
                if (!!role.selected) $scope.albumNameitem.push(role.invmI_Id);
            })

            if ($scope.myForm.$valid) {
                // var FAMLED_Id = 0;
                var FAMLED_BillwiseFlg = 0;
                if ($scope.obj.FAMLED_BillwiseFlg == true) {
                    FAMLED_BillwiseFlg = 1;
                }
             
                var data = {
                   
                    "FAMCOMP_Id": $scope.FAMCOMP_Id.famcomP_Id,
                    "IMFY_Id": $scope.IMFY_Id,
                    "FAMGRP_Id": $scope.FAMGRP_Id.FAMGRP_Id,
                    Amstid: $scope.albumNameArray,
                    salesid: $scope.albumNamesales,
                    itemid: $scope.albumNameitem,
                    "type": $scope.optionType1,
                    "crdrflg": $scope.crdrflg

                }
                apiService.create("AutoLedgerCreation/savedata", data).
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
                        apiService.create("AutoLedgerCreation/Deletedetails", data).
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
            apiService.create("AutoLedgerCreation/edit", data).
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
            apiService.create("AutoLedgerCreation/savedatatwo", data).
                then(function (promise) {

                    if (promise.usergroupname != null && promise.usergroupname.length > 0) {
                        $scope.usergroupname = promise.usergroupname;
                    }
                    else if (promise.returnval == "admin") {
                        swal('Please Contact  Administrator  !');
                    }
                    else {
                        swal('Record Not Found  !');
                    }


                })

        };

        $scope.removegrnrows = function (index, data) {
            var newItemNo = $scope.transrows.length - 1;
            $scope.transrows.splice(index, 1);

        };

        $scope.onselectclass = function () {

            var data = {
                "asmay_id": $scope.cfg.ASMAY_Id,
                "ASMCL_Id": $scope.clsdrp,
                //  "Adm_no_name": $scope.radio_button,
            }
            apiService.create("FeeOpeningBalance/getclshead/", data).then(function (promise) {

                if (promise.sectionlist != null && promise.sectionlist != "") {
                    $scope.sectiondrpre = promise.sectionlist;
                    $scope.headlst = promise.fillmasterhead;
                }
                else {
                    swal("No Section Found Kindly select Another Class/Year");
                    $scope.fee_head_flag = true;
                    $scope.fee_head = false;
                }

            });
        }

        $scope.onselectmodeof = function () {

           
                    var data = {
                       
                      
                        "ASMS_Id": $scope.ASMS_Id,
                        "ASMCL_Id": $scope.ASMCL_Id,
                       
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
            apiService.create("AutoLedgerCreation/sectionchange", data).
                        then(function (promise) {

                            if (promise.studentdata != null && promise.studentdata != "") {
                                $scope.studentlst = promise.studentdata;
                              

                            }
                            else {
                              
                            }
                        })
                
            
        }
    }

})();