
(function () {
    'use strict';
    angular
.module('app')
.controller('DriverEmployeeMappingController', DriverEmployeeMappingController)

    DriverEmployeeMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter']
    function DriverEmployeeMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, superCache) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        $scope.masterlist = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.searchValue = "";

        $scope.BindData = function () {
            
            var pageid = 2;
            apiService.getURI("DriverEmployeeMapping/getdata", pageid).then(function (promise) {
                if (promise != null) {
                  
                    if (promise.savedata.length > 0) {
                        $scope.mastervehicle = promise.savedata;
                        $scope.presentCountgrid = $scope.mastervehicle.length;
                        $scope.masterlist = true;
                    }
                    else {
                        swal("No Records Found")
                        $scope.masterlist = false;
                    }
                    $scope.employeedetails = promise.employeedata;
                    $scope.driverdata = promise.driverdata;
                }
                else {
                    swal("No Records Found")
                }
            })


        }



        $scope.clearid = function () {
            $state.reload();
        }
        //---Save--//
        $scope.savedata = function () {

            if ($scope.myForm.$valid) {
                var data = {
                    "TRDE_Id": $scope.TRDE_Id,
                    "HRME_Id": $scope.HRME_Id,
                    "TRMD_Id": $scope.TRMD_Id
                }
                apiService.create("DriverEmployeeMapping/savedata", data).then(function (promise) {
                    if (promise.message == "Add") {
                        if (promise.retrunval == true) {
                            swal("Record Saved Successfully");
                        }
                        else {
                            swal("Record Not Saved");
                        }
                    }
                    else if (promise.message == "Update") {
                        if (promise.retrunval == true) {
                            swal("Record Updated Successfully");
                        }
                        else {
                            swal("Record Not Updated");
                        }
                    }
                    else if (promise.message == "Mapped") {
                        swal("You Can Not Edit This Record It Already Mapped");
                    }
                    else if (promise.message == "Duplicate") {
                        swal("Record Already Exists");
                    }
                    $state.reload();
                })
            }
            else {
                $scope.submitted = true;
            }
        }

        //--Sorting--//     
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
       


        //---Edit Data--//
        $scope.edit = function (user) {
            var data = {
                "TRDE_Id": user.trdE_Id
            }
            
            apiService.create("DriverEmployeeMapping/edit", data).then(function (Promise) {
                if (Promise != null) {
                    $scope.HRME_Id = Promise.edit[0].hrmE_Id;
                    $scope.TRMD_Id = Promise.edit[0].trmD_Id;
                    $scope.TRDE_Id = Promise.edit[0].trdE_Id;
                   
                }
                //$scope.edit = promise.edit;
                ////$scope.employeedetails = promise.employeedata;
                ////$scope.driverdata = promise.driverdata;
            })
        }
        //--Active Deactive--//
        $scope.deactive = function (user, SweetAlert) {
            var mgs = "Delete";
            var confirmmgs = "Deleted";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            //if (user.trmL_ActiveFlg === true) {
            //    mgs = "Deactive";
            //    confirmmgs = "Deactivated";
            //}
            //else {
            //    mgs = "Active";
            //    confirmmgs = "Activated";
            //}
            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record??????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
        function (isConfirm) {
            if (isConfirm) {
                apiService.create("DriverEmployeeMapping/deletedata/", user).
                then(function (promise) {
                    if (promise.message != null) {
                        swal(promise.message);
                    }
                    else {
                        if (promise.returnval == true) {
                            swal(confirmmgs + " " + "Successfully");
                            $state.reload();
                        }
                        else {
                            swal(confirmmgs + " " + " Successfully");
                            $state.reload();
                        }
                    }
                })
            }
            else {
                swal("Record " + mgs + " Cancelled");
            }
            $state.reload();
        });
        }

        //--Sorting--//     
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.searchValue = '';

    };



})();


