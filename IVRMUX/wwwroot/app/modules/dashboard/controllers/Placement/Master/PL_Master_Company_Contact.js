(function () {
    'use strict';
    angular
        .module('app')
        .controller('PL_Master_Company_ContactController', PL_Master_Company_ContactController)
    PL_Master_Company_ContactController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$stateParams', '$filter', 'superCache', '$q']
    function PL_Master_Company_ContactController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $stateParams, $filter, superCache, $q) {
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



        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.search = "";
            var pageid = 2;
            apiService.getURI("PL_Master_Company_Contact/loaddata", pageid).
                then(function (promise) {
                    $scope.get_Company = promise.get_Company;
                    $scope.get_contact = promise.get_contact;
                    $scope.presentCountgrid = $scope.get_contact.length;
                })
        };

        $scope.submitted = false;

        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    //"PLMCOMP_CompanyName": $scope.plmcomP_CompanyName,
                    "PLMCOMPCON_EmailId": $scope.plmcompcoN_EmailId,
                    "PLMCOMPCON_ContactPersonName": $scope.plmcompcoN_ContactPersonName,
                    "PLMCOMPCON_Designation": $scope.plmcompcoN_Designation,
                    "PLMCOMPCON_ContactNo": $scope.plmcompcoN_ContactNo,
                    "PLMCOMPCON_Id": $scope.plmcompcoN_Id,
                    "PLMCOMP_Id": $scope.plmcomP_Id,

                    
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("PL_Master_Company_Contact/savedata", data).then(function (promise) {

                    if (promise.returnval == true) {
                        if (promise.plmcompcoN_Id == 0 || promise.plmcompcoN_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.plmcompcoN_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else if (promise.returnduplicatestatus == 'Duplicate') {
                        swal('Record already exist');
                    }
                    else {
                        if (promise.plmcompcoN_Id == 0 || promise.plmcompcoN_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.plmcompcoN_Id > 0) {
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

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.cancel = function () {
            $state.reload();
        }
        //
        //Deactive
        $scope.deactive = function (item, SweetAlert) {

            var data = {

                "PLMCOMPCON_Id": item.plmcompcoN_Id
            }
            var dystring = "";
            if (item.plmcompcoN_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.plmcompcoN_ActiveFlag == false) {
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
                        apiService.create("PL_Master_Company_Contact/deactive", data).
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
                        swal("Record  " + dystring + " Cancelled!!!");
                    }
                });
        }

        $scope.editdata = function (user) {
            $scope.plmcomP_CompanyName = user.plmcomP_CompanyName;
            $scope.plmcompcoN_ContactPersonName = user.plmcompcoN_ContactPersonName;
            $scope.plmcompcoN_Designation = user.plmcompcoN_Designation;
            $scope.plmcompcoN_ContactNo = user.plmcompcoN_ContactNo;
            $scope.plmcompcoN_Id = user.plmcompcoN_Id;
            $scope.plmcompcoN_EmailId = user.plmcompcoN_EmailId;
            $scope.plmcomP_Id = user.plmcomP_Id;
        };
        $scope.clear = function () {
            $state.reload();
        }
        $scope.sortBy = function (propertyName) {
            $scope.reverse = ($scope.propertyName === propertyName) ? !$scope.reverse : false;
            $scope.propertyName = propertyName;
        };
        $scope.searchValue = '';

    }

})();