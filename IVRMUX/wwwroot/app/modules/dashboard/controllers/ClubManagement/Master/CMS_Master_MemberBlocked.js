(function () {
    'use strict';
    angular
        .module('app')
        .controller('CMS_Master_MemberBlockedController', CMS_Master_MemberBlockedController)
    CMS_Master_MemberBlockedController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$stateParams', '$filter', 'superCache', '$q']
    function CMS_Master_MemberBlockedController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $stateParams, $filter, superCache, $q) {
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
        $scope.obj = {};
        $scope.search = "";
        $scope.plMaxdate = new Date();
        $scope.editflag = false;
        $scope.plMaxdate.setDate($scope.plMaxdate.getDate());
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("CMS_Master_MemberBlocked/loaddata", pageid).then(function (promise) {
                $scope.getreport = promise.getreport;
                if ($scope.getreport != null && $scope.getreport.length > 0) {
                    $scope.presentCountgrid = $scope.getreport.length;
                }
                else if (promise.returnval == "admin") {
                    swal('Please Contact  Administrator  !');
                }
                $scope.allCaste = promise.getname;

            })
        };
        $scope.submitted = false;
        $scope.savepages = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var CMSMMEMBLK_Id = 0;
                if ($scope.obj.CMSMMEMBLK_Id > 0) {
                    CMSMMEMBLK_Id = $scope.obj.CMSMMEMBLK_Id;
                }
                var data = {
                    "CMSMMEMBLK_Id": CMSMMEMBLK_Id,
                    "CMSMMEM_Id": $scope.obj.cmsmmeM_Id.cmsmmeM_Id,
                    "CMSMMEMBLK_BlockedDate": new Date($scope.obj.CMSMMEMBLK_BlockedDate).toDateString(),
                    "CMSMMEMBLK_RenewalDate": new Date($scope.obj.CMSMMEMBLK_RenewalDate).toDateString(),
                    "CMSMMEMBLK_ReasonForBlock": $scope.obj.CMSMMEMBLK_ReasonForBlock
                }
                apiService.create("CMS_Master_MemberBlocked/savedetail1", data).
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
                "CMSMMEMBLK_Id": item.cmsmmemblK_Id
            }
            var dystring = "";
            if (item.cmsmmemblK_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (item.cmsmmemblK_ActiveFlg == false) {
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
                        apiService.create("CMS_Master_MemberBlocked/deactive", data).
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
           
            $scope.obj.CMSMMEMBLK_Id = user.cmsmmemblK_Id;
            $scope.obj.CMSMMEMBLK_BlockedDate = new Date(user.cmsmmemblK_BlockedDate);
            $scope.obj.CMSMMEMBLK_RenewalDate = new Date(user.cmsmmemblK_RenewalDate);
            $scope.obj.CMSMMEMBLK_ReasonForBlock = user.cmsmmemblK_ReasonForBlock;
            if ($scope.allCaste.length > 0) {
                for (var i = 0; i < $scope.allCaste.length; i++) {
                    if (user.cmsmmeM_Id == $scope.allCaste[i].cmsmmeM_Id) {
                        $scope.allCaste[i].Selected = true;
                        $scope.obj.cmsmmeM_Id = $scope.allCaste[i];
                        $scope.newcaste = user.cmsmmeM_Id;
                        $scope.editflag = true;
                    }
                }
            }

        };
        $scope.clear = function () {
            $state.reload();
        }
    }

})();