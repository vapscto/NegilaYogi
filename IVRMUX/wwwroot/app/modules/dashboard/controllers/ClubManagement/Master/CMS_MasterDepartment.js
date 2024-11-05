(function () {
    'use strict';
    angular
        .module('app')
        .controller('CMS_MasterDepartmentController', CMS_MasterDepartmentController)
    CMS_MasterDepartmentController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$window', '$stateParams', '$filter', 'superCache', '$q']
    function CMS_MasterDepartmentController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $window, $stateParams, $filter, superCache, $q) {                
        var paginationformasters;
        var copty;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !=null &&  ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        }
        $scope.itemsPerPage = paginationformasters;
        $scope.coptyright = copty;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.loaddata = function () {            
            var pageid = 2;
            apiService.getURI("CMS_MasterDepartment/loaddata", pageid).then(function (promise) {
                $scope.getreport = promise.pages;
                if ($scope.getreport != null && $scope.getreport.length > 0) {
                    $scope.presentCountgrid = $scope.getreport.length;
                }
                
            })           
        };                         
        $scope.submitted = false;
        $scope.savepages = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var CMSMDEPT_Id = 0;
                if ($scope.CMSMDEPT_Id > 0) {
                    CMSMDEPT_Id = $scope.CMSMDEPT_Id;
                }
                var data = {
                    "CMSMDEPT_DepartmentName": $scope.CMSMDEPT_DepartmentName,
                    "CMSMDEPT_DeptCode": $scope.CMSMDEPT_DeptCode,
                    "CMSMDEPT_Id": CMSMDEPT_Id
                }
                apiService.create("CMS_MasterDepartment/savedata", data).
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

                "CMSMDEPT_Id": item.cmsmdepT_Id
            }
            var dystring = "";
            if (item.cmsmdepT_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (item.cmsmdepT_ActiveFlag == false) {
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
                        apiService.create("CMS_MasterDepartment/deactive", data).
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
            $scope.CMSMDEPT_DepartmentName = user.cmsmdepT_DepartmentName;
            $scope.CMSMDEPT_DeptCode = user.cmsmdepT_DeptCode;
            $scope.CMSMDEPT_Id = user.cmsmdepT_Id;
        };
        $scope.clear = function () {
            $state.reload();
        }
    }

})();