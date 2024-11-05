(function () {
    'use strict';
    angular
        .module('app')
        .controller('IVRM_User_Login_StateController', IVRM_User_Login_StateController)
    IVRM_User_Login_StateController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$stateParams', '$filter', 'superCache', '$q']
    function IVRM_User_Login_StateController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $stateParams, $filter, superCache, $q) {
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
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("IVRM_User_Login_State/loaddata", pageid).then(function (promise) {
                
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
        $scope.isOptionsRequired = function () {
            return !$scope.statelistone.some(function (options) {
                return options.selected;
            });
        };
        $scope.all_checkdesg = function (stateall) {
            var toggleStatus = stateall;
            angular.forEach($scope.statelistone, function (itm) {
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
                if ($scope.statelistone != null && $scope.statelistone.length > 0) {
                    for (var i = 0; i < $scope.statelistone.length; i++) {
                        if ($scope.statelistone[i].selected == true) {
                            $scope.multiplstatelistone.push({
                                IVRMMS_Id: $scope.statelistone[i].ivrmmS_Id,
                            })
                        }
                    }
                }
                var data = {
                    "IVRMMC_Id": $scope.obj.IVRMMC_Id,
                    "multiplstatelistone": $scope.multiplstatelistone,
                    "IVRMUL_Id": $scope.IVRMUL_Id.id

                }
                apiService.create("IVRM_User_Login_State/savedata", data). then(function (promise) {
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
            $scope.IVRMULST_Id = item.IVRMULST_Id;
            var dystring = "";
            if (item.IVRMULST_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.IVRMULST_ActiveFlag == false) {
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
                        apiService.create("IVRM_User_Login_State/deactive", item).
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


        $scope.EditDetails = function (data) {
            $scope.newcaste = {};
            var data = {
                "IVRMULST_Id": data.IVRMULST_Id,
            }
            apiService.create("IVRM_User_Login_State/edit/", data).then(function (promise) {
                $scope.editDetails = promise.editDetails;
                if ($scope.getusers != null && $scope.getusers.length > 0) {                    for (var i = 0; i < $scope.getusers.length; i++) {                        if (promise.editDetails[0].ivrmuL_Id == $scope.getusers[i].id) {                            $scope.getusers[i].selected = true;                            $scope.IVRMUL_Id = $scope.getusers[i];                            $scope.newcaste = $scope.getusers[0].id;                        }                    }                }
                angular.forEach($scope.statelistone, function (itm1) {
                    if (itm1.ivrmmS_Id == promise.editDetails[0].ivrmmS_Id) {
                            itm1.selected = true;
                    }      
             
                });
            });
        }
        
        $scope.clear = function () {
            $state.reload();
        }
        $scope.searchValue = '';

    }
})();