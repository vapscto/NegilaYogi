(function () {
    'use strict';
    angular
        .module('app')
        .controller('MC_342_HRIncentivesController', MC_342_HRIncentivesController);
    MC_342_HRIncentivesController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'Excel', '$timeout', '$q', '$filter', '$sce', 'myFactorynaac'];

    function MC_342_HRIncentivesController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, Excel, $timeout, $q, $filter, $sce, myFactorynaac) {
        $scope.searc_button = true;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }
        var miid = myFactorynaac.get();
        if (miid > 0) {
            $scope.mI_Id = miid;
        } else {
            $scope.mI_Id = 0;
        }
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.search = "";
        $scope.institute_flag = false;
        //=======================Page Load
        $scope.NCMCHRI342_Id = 0;
        $scope.instit = false;
        $scope.loaddata = function () {           
            $scope.NCMCHRI342_Id = 0;
            $scope.change_institution();
            if ($scope.mI_Id == undefined || $scope.mI_Id == null || $scope.mI_Id == '') {
                $scope.mI_Id = 0;
            }
            apiService.getURI("MC_342_HRIncentives/loaddata", $scope.mI_Id).then(function (promise) {             
                $scope.institutionlist = promise.institutionlist;
                $scope.mI_Id = promise.mI_Id;
                $scope.yearlist = promise.yearlist;
                $scope.alldata1 = promise.alldata1;
            })
        }
        $scope.sort = function (keyname) {
            $scope.propertyName = keyname;   //set the propertyName to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        //========================
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.submitted = false;
        $scope.savedata = function () {
            debugger;
            if ($scope.myForm.$valid) {
                if ($scope.check1 == '1') {
                    $scope.NCMCHRI342_CareerAdvancement = true;
                }
                if ($scope.check2 == '1') {
                    $scope.NCMCHRI342_IncrementInSalary = true;
                }
                if ($scope.check3 == '1') {
                    $scope.NCMCHRI342_RecThroughWebsiteNotification = true;
                }
                if ($scope.check4 == '1') {
                    $scope.NCMCHRI342_CommnCertAndCashaward = true;
                }                
                var data = {
                    "MI_Id": $scope.mI_Id,
                    "NCMCHRI342_Id": $scope.ncmchrI342_Id,
                    "ASMAY_Id": $scope.NCMCHRI342_Year,
                    "NCMCHRI342_CareerAdvancement": $scope.NCMCHRI342_CareerAdvancement,
                    "NCMCHRI342_IncrementInSalary": $scope.NCMCHRI342_IncrementInSalary,
                    "NCMCHRI342_RecThroughWebsiteNotification": $scope.NCMCHRI342_RecThroughWebsiteNotification,
                    "NCMCHRI342_CommnCertAndCashaward": $scope.NCMCHRI342_CommnCertAndCashaward,  
                    "MI_Id": $scope.mI_Id,                   
                }
                apiService.create("MC_342_HRIncentives/savedata", data).then(function (promise) {
                    if (promise.duplicate == false) {
                        if (promise.returnval == true) {
                            if ($scope.ncmchrI342_Id > 0) {
                                swal('Record Updated Successfully!');
                            }
                            else {
                                swal('Record Saved Successfully!');
                            }
                            $state.reload();
                        }
                        else {
                            if ($scope.ncmchrI342_Id > 0) {
                                swal('Record Nolt Updated Successfully!');
                            }
                            else {
                                swal('Record Not Saved Successfully!');
                            }
                        }
                    }
                    else {
                        swal('Record Already Exist!');
                    }
                })
            }
            else {
                $scope.submitted = true;
            }
        }
        //=========================Edit For Tab2 Mapping data
        $scope.edittab1 = function (user) {
            debugger;
            var data = {
                "NCMCHRI342_Id": user.ncmchrI342_Id,
                "MI_Id": user.mI_Id,
            }
            apiService.create("MC_342_HRIncentives/editdata", data).then(function (promise) {
                debugger;
                $scope.editdata = promise.editdata;
                $scope.institute_flag = true;
                $scope.NCMCHRI342_Id = promise.editdata[0].ncmchrI342_Id;
                $scope.mI_Id = promise.editdata[0].mI_Id;                
                $scope.ASMAY_Id = promise.editdata[0].ncmchrI342_Year;
                if (promise.editdata[0].ncmchrI342_CareerAdvancement == true) {
                    $scope.check1 = 1;
                }
                if (promise.editdata[0].ncmchrI342_IncrementInSalary == true) {
                    $scope.check2 = 1;
                }
                if (promise.editdata[0].ncmchrI342_RecThroughWebsiteNotification == true) {
                    $scope.check3 = 1;
                }
                if (promise.editdata[0].ncmchrI342_CommnCertAndCashaward == true) {
                    $scope.check4 = 1;
                }               
            })
        }
        //==========================cancel Button  for Tab2
        $scope.canceltab2 = function () {
            $state.reload();
        }
        $scope.change_institution = function () {
           // $scope.materaldocuupload = [{ id: 'Materal1' }];
            $scope.ASMAY_Id = '';
            $scope.NCMCHRI342_Year = '';
            $scope.ncmchrI342_Id = 0;           
            $scope.NCMCHRI342_CareerAdvancement = '';
            $scope.NCMCHRI342_IncrementInSalary = '';
            $scope.NCMCHRI342_RecThroughWebsiteNotification = '';
            $scope.NCMCHRI342_CommnCertAndCashaward = '';
            $scope.usercheckC = '';              
        } 
        $scope.deactive = function (usersem, SweetAlert) {
            var dystring = "";
            if (usersem.ncmchrI342_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (usersem.ncmchrI342_ActiveFlag == false) {
                dystring = "Activate"
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
                        apiService.create("MC_342_HRIncentives/deactive", usersem).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d" + " Successfully!!!");
                                    $state.reload();
                                }
                                else {
                                    swal("Record Not " + dystring + "d" + " Successfully!!!");
                                    $state.reload();
                                }
                            })
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }
                });
        }
    }
})();
