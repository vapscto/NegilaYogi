(function () {
    'use strict';
    angular
        .module('app')
        .controller('CMS_MemberCategoryController', CMS_MemberCategoryController)
    CMS_MemberCategoryController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$stateParams', '$filter', 'superCache', '$q']
    function CMS_MemberCategoryController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $stateParams, $filter, superCache, $q) {
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
        $scope.ismeridian = true;
        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("CMS_MemberCategory/loaddata", pageid).then(function (promise) {
                $scope.getreport = promise.getreport;
            })
        };
        $scope.submitted = false;
        $scope.savepages = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var CMSMCAT_Id = 0;
                if ($scope.CMSMCAT_Id > 0) {
                    CMSMCAT_Id = $scope.CMSMCAT_Id;
                }
                var data = {
                    "CMSMCAT_CategoryName": $scope.CMSMCAT_CategoryName,
                    "CMSMCAT_CategoryCode": $scope.CMSMCAT_CategoryCode,
                    "CMSMCAT_MaxNoOfGuest": $scope.CMSMCAT_MaxNoOfGuest,
                    "CMSMCAT_MembershipDuration": $scope.CMSMCAT_MembershipDuration,
                    "CMSMCAT_AllowCreditTransFlg": $scope.CMSMCAT_AllowCreditTransFlg,
                    "CMSMCAT_MaxCreditLimit": $scope.CMSMCAT_MaxCreditLimit,
                    "CMSMCAT_MinTransAmt": $scope.CMSMCAT_MinTransAmt,
                    "CMSMCAT_MaxNoOfDependents": $scope.CMSMCAT_MaxNoOfDependents,
                    "CMSMCAT_Id": CMSMCAT_Id
                }
                apiService.create("CMS_MemberCategory/savedata", data).
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
                "CMSMCAT_Id": item.cmsmcaT_Id
            }
            var dystring = "";
            if (item.cmsmcaT_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.cmsmcaT_ActiveFlag == false) {
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
                        apiService.create("CMS_MemberCategory/deactive", data).
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
            $scope.CMSMCAT_Id = user.cmsmcaT_Id;
            if ($scope.CMSMCAT_Id > 0) {
                var data = {

                    "CMSMCAT_Id": $scope.CMSMCAT_Id
                }
                apiService.create("CMS_MemberCategory/edit", data).
                    then(function (promise) {
                        //edit
                        if (promise.edit != null && promise.edit.length > 0) {
                            $scope.CMSMCAT_CategoryName = promise.edit[0].cmsmcaT_CategoryName;
                            $scope.CMSMCAT_CategoryCode = promise.edit[0].cmsmcaT_CategoryCode;
                            $scope.CMSMCAT_MaxNoOfGuest = promise.edit[0].cmsmcaT_MaxNoOfGuest;
                            $scope.CMSMCAT_MembershipDuration = promise.edit[0].cmsmcaT_MembershipDuration;
                            $scope.CMSMCAT_AllowCreditTransFlg = promise.edit[0].cmsmcaT_AllowCreditTransFlg;
                            $scope.CMSMCAT_MaxCreditLimit = promise.edit[0].cmsmcaT_MaxCreditLimit;
                            $scope.CMSMCAT_MinTransAmt = promise.edit[0].cmsmcaT_MinTransAmt;
                            $scope.CMSMCAT_MaxNoOfDependents = promise.edit[0].cmsmcaT_MaxNoOfDependents;
                        }
                        else {
                            swal("Please Contact Administrator !");
                        }

                    })
            }
           
            
        };
        $scope.clear = function () {
            $state.reload();
        }
    }

})();