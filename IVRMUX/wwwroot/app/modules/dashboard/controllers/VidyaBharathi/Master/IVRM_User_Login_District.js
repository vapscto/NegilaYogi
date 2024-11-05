(function () {
    'use strict';
    angular
        .module('app')
        .controller('IVRM_User_Login_DistrictController', IVRM_User_Login_DistrictController)
    IVRM_User_Login_DistrictController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$stateParams', '$filter', 'superCache', '$q']
    function IVRM_User_Login_DistrictController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $stateParams, $filter, superCache, $q) {
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.currentPage1 = 1;
        $scope.itemsPerPage1 = 10;
        $scope.obj = {};
        $scope.paidAmount = 0;
        $scope.obj.allpayment = false;
        $scope.statelistoneyyy = [];
        $scope.statelistoneyyyy = [];
        $scope.districtlisttemp = [];
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("IVRM_User_Login_District/loaddata", pageid).then(function (promise) {
                
                $scope.getusermap = promise.getusermap;
                $scope.presentCountgrid = $scope.getusermap.length;

                $scope.getusers = promise.getusers;

                $scope.countrylist = promise.getcountry;
                if ($scope.countrylist != null && $scope.countrylist.length > 0) {
                    for (var i = 0; i < $scope.countrylist.length; i++) {
                        if ($scope.countrylist[i].ivrmmC_Default == true) {
                            $scope.obj.IVRMMC_Id = $scope.countrylist[i].ivrmmC_Id;
                        }
                    }
                    $scope.statelistoneyyy = promise.statelist;
                    $scope.statelistV($scope.obj.IVRMMC_Id);
                    
                }

                $scope.districtlisttemp = promise.getdistrict;
              

            })
        };
        $scope.statelistV = function (IVRMMC_Id) {
            $scope.statelistone = [];
            if ($scope.statelistoneyyy != null && $scope.statelistoneyyy.length > 0) {
                for (var i = 0; i < $scope.statelistoneyyy.length; i++) {
                    if ($scope.statelistoneyyy[i].ivrmmC_Id == IVRMMC_Id) {
                        $scope.statelistone.push({
                            ivrmmS_Name: $scope.statelistoneyyy[i].ivrmmS_Name,
                            ivrmmS_Id: $scope.statelistoneyyy[i].ivrmmS_Id,
                        })

                    }

                }
            }
        }

        $scope.statelistVV = function (IVRMMS_Id) {
            $scope.statelistonee = [];
            if ($scope.districtlisttemp != null && $scope.districtlisttemp.length > 0) {
                for (var i = 0; i < $scope.districtlisttemp.length; i++) {
                    if ($scope.districtlisttemp[i].ivrmmS_Id == IVRMMS_Id) {
                        $scope.statelistonee.push({
                            ivrmmD_Name: $scope.districtlisttemp[i].ivrmmD_Name,
                            ivrmmD_Id: $scope.districtlisttemp[i].ivrmmD_Id,
                        })

                    }

                }
            }
        }


        $scope.isOptionsRequired = function () {
            return !$scope.statelistonee.some(function (options) {
                return options.selected;
            });
        };
        

        $scope.all_checkdesg = function (stateall) {
            var toggleStatus = stateall;
            angular.forEach($scope.statelistonee, function (itm) {
                itm.selected = toggleStatus;
            });
            $scope.togchkbxdesg();
        };
        //$scope.interacted = function (field) {
        //    return $scope.submitted || field.$dirty;
        //};
        

        $scope.savepages = function () {

            $scope.getreport = [];
            $scope.multiplstatelistone = [];
            $scope.submitted = true;

            if ($scope.myForm.$valid) {
                if ($scope.statelistonee != null && $scope.statelistonee.length > 0) {
                    for (var i = 0; i < $scope.statelistonee.length; i++) {
                        if ($scope.statelistonee[i].selected == true) {
                            $scope.multiplstatelistone.push({
                                IVRMMD_Id: $scope.statelistonee[i].ivrmmD_Id,
                            })
                        }
                    }
                }
                var data = {
                   
                    "multiplstatelistone": $scope.multiplstatelistone,
                    "IVRMUL_Id": $scope.IVRMUL_Id.id

                }
                apiService.create("IVRM_User_Login_District/savedata", data). then(function (promise) {
                        if (promise.returnval == true) {
                            swal("Saved Count " + promise.savecount + " , Duplicate Count " + promise.DuplicateCount+"");
                        }
                        else {
                            swal("please contact administrator");
                        }
                    $state.reload();
                })
            }

        };

        $scope.deactive = function (item, SweetAlert) {
            $scope.IVRMULDT_Id = item.ivrmuldT_Id;
            var dystring = "";
            if (item.IVRMULDT_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.IVRMULDT_ActiveFlag == false) {
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
                        apiService.create("IVRM_User_Login_District/deactive", item).
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
        
        $scope.clear = function () {
            $state.reload();
        }
        $scope.searchValue = '';

    }
})();