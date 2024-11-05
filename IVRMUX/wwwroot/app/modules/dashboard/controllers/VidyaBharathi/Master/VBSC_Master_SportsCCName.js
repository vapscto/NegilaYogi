
(function () {
    'use strict';
    angular
        .module('app')
        .controller('VBSC_Master_SportsCCNameController', VBSC_Master_SportsCCNameController);
    VBSC_Master_SportsCCNameController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function VBSC_Master_SportsCCNameController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {


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
        $scope.obj = {};
        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("VBSC_Master_SportsCCName/getloaddata", pageid).
                then(function (promise) {
                    $scope.master_trust = promise.master_trust;

                    $scope.get_customer = promise.get_customer;

                    $scope.presentCountgrid = $scope.get_customer.length;

                    $scope.floordetailschange($scope.vbscmsccG_SCCFlag);
                })
        };
   


        $scope.floordetailschange = function (vbscmsccG_SCCFlag) {
            $scope.getGroupName = [];
            $scope.vbscmsccG_Id = "";
            var data = {
                "VBSCMSCCG_SCCFlag": vbscmsccG_SCCFlag
            }
            apiService.create("VBSC_Master_SportsCCName/getInstitute", data).
                then(function (promise) {
                    if (promise.getGroupName != null && promise.getGroupName.length > 0) {
                        $scope.getGroupName = promise.getGroupName;
                    }
                    else {
                        
                    }

                })
        }
        
        //----------------------------Savedata----------------------------------------------------

        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var VBSCMSCC_NoOfMembers = 0;
                if ($scope.vbscmscC_SGFlag == "Group") {
                    VBSCMSCC_NoOfMembers = $scope.obj.vbscmscC_NoOfMembers;
                }
                var data = {
                    //  "MT_Id": $scope.mT_Id,
                    "VBSCMSCC_Id": $scope.vbscmscC_Id,
                    "VBSCMSCCG_Id": $scope.vbscmsccG_Id,
                    "VBSCMSCC_SportsCCName": $scope.vbscmscC_SportsCCName,
                    "VBSCMSCC_SportsCCDesc": $scope.vbscmscC_SportsCCDesc,
                    "VBSCMSCC_GenderFlg": $scope.vbscmscC_GenderFlg,
                    "VBSCMSCC_SGFlag": $scope.vbscmscC_SGFlag,
                    "VBSCMSCC_NoOfMembers": VBSCMSCC_NoOfMembers,
                    "VBSCMSCC_RecHighLowFlag": $scope.vbscmscC_RecHighLowFlag,
                    "VBSCMSCC_RecInfo": $scope.vbscmscC_RecInfo
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("VBSC_Master_SportsCCName/savedetails", data).then(function (promise) {

                    if (promise.returnval == true) {
                        if (promise.vbscmscC_Id == 0 || promise.vbscmscC_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.vbscmscC_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus == 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.vbscmscC_Id == 0 || promise.vbscmscC_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.vbscmscC_Id > 0) {
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
            $scope.VBSCMSCC_Id = item.vbscmscC_Id;
            var dystring = "";
            if (item.VBSCMSCC_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.VBSCMSCC_ActiveFlag == false) {
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
                        apiService.create("VBSC_Master_SportsCCName/deactive", item).
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

    

        $scope.edit = function (item) {
            $scope.vbscmsccG_SCCFlag = item.VBSCMSCCG_SCCFlag;
            $scope.floordetailschange($scope.vbscmsccG_SCCFlag);
            $scope.vbscmsccG_Id = item.VBSCMSCCG_Id;
            $scope.vbscmscC_Id = item.VBSCMSCC_Id;
            
          
            $scope.vbscmscC_SportsCCName = item.VBSCMSCC_SportsCCName;
            $scope.vbscmscC_SportsCCDesc = item.VBSCMSCC_SportsCCDesc;
            $scope.vbscmscC_SGFlag = item.VBSCMSCC_SGFlag;
            $scope.vbscmscC_GenderFlg = item.VBSCMSCC_GenderFlg;
            $scope.obj.vbscmscC_NoOfMembers = item.VBSCMSCC_NoOfMembers;
            $scope.vbscmscC_RecHighLowFlag = item.VBSCMSCC_RecHighLowFlag;
            $scope.vbscmscC_RecInfo = item.VBSCMSCC_RecInfo;
          
          
            
        }

        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';

    }
})();