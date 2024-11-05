(function () {
    'use strict';
    angular
        .module('app')
        .controller('CMS_Member_StatusController', CMS_Member_StatusController)
    CMS_Member_StatusController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$stateParams', '$filter', 'superCache', '$q']
    function CMS_Member_StatusController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $stateParams, $filter, superCache, $q) {
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
        $scope.editflag = false;
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("CMS_Member_Status/loaddata", pageid).then(function (promise) {
                $scope.getreport = promise.getreport;
                $scope.allCaste = promise.getname;
                $scope.finacialyear = promise.finacial;
                if ($scope.getreport != null && $scope.getreport.length > 0) {
                    $scope.presentCountgrid = $scope.getreport.length;
                }

            })
        };
        $scope.submitted = false;
        $scope.savepages = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var CMSMEMSTS_Id = 0;
                if ($scope.CMSMEMSTS_Id > 0) {
                    CMSMEMSTS_Id = $scope.CMSMEMSTS_Id;
                }
                var data = {
                    "CMSMMEM_Id": $scope.obj.cmsmmeM_Id.cmsmmeM_Id,
                    "IMFY_Id": $scope.IMFY_Id,
                    "CMSMEMSTS_Id": CMSMEMSTS_Id,
                    "CMSMEMSTS_OpeningBalance": $scope.CMSMEMSTS_OpeningBalance,
                    "CMSMEMSTS_OBCRDRFlg": $scope.CMSMEMSTS_OBCRDRFlg,
                    "CMSMEMSTS_TotalDR": $scope.CMSMEMSTS_TotalDR,
                    "CMSMEMSTS_TotalDRTrans": $scope.CMSMEMSTS_TotalDRTrans,
                    "CMSMEMSTS_TotalCRTrans": $scope.CMSMEMSTS_TotalCRTrans,
                    "CMSMEMSTS_ClosingBalance": $scope.CMSMEMSTS_ClosingBalance,
                    "CMSMEMSTS_CBDRDRFlg": $scope.CMSMEMSTS_CBDRDRFlg
                }
                apiService.create("CMS_Member_Status/savedata", data).
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

                "CMSMEMSTS_Id": item.cmsmemstS_Id
            }
            var dystring = "";
            if (item.cmsmemstS_ActiveFlg == true) {
                dystring = "Deactivate";
            }
            else if (item.cmsmemstS_ActiveFlg == false) {
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
                        apiService.create("CMS_Member_Status/deactive", data).
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
            debugger;
            $scope.CMSMEMSTS_CBDRDRFlg = user.cmsmemstS_CBDRDRFlg;
            $scope.CMSMEMSTS_ClosingBalance = user.cmsmemstS_ClosingBalance;
            $scope.CMSMEMSTS_TotalCRTrans = user.cmsmemstS_TotalCRTrans;
            $scope.CMSMEMSTS_TotalDRTrans = user.cmsmemstS_TotalDRTrans;
            $scope.CMSMEMSTS_TotalDR = user.cmsmemstS_TotalDR;
            $scope.CMSMEMSTS_OBCRDRFlg = user.cmsmemstS_OBCRDRFlg;
            $scope.CMSMEMSTS_OpeningBalance = user.cmsmemstS_OpeningBalance;
            $scope.IMFY_Id = user.imfY_Id;
            $scope.CMSMEMSTS_Id = user.cmsmemstS_Id;
            if ($scope.allCaste.length > 0) {
                for (var i = 0; i < $scope.allCaste.length; i++) {
                    if (user.cmsmmeM_Id == $scope.allCaste[i].cmsmmeM_Id) {
                        $scope.allCaste[i].Selected = true;
                        $scope.obj.cmsmmeM_Id = $scope.allCaste[i];
                        $scope.newcaste = user.cmsmmeM_Id;
                       
                    }
                }
            }
            
            $scope.editflag = true;
        };
        $scope.clear = function () {
            $state.reload();
        }
    }

})();