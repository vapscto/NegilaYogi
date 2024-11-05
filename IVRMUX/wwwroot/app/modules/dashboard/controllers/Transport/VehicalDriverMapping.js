﻿
(function () {
    'use strict';
    angular
.module('app')
.controller('VehicalDriverMappingController', VehicalDriverMappingController)

    VehicalDriverMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter']
    function VehicalDriverMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter) {

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
            apiService.getURI("VehicalDriverMapping/getdata", pageid).then(function (promise) {
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
                   
                    $scope.vehicaldetails = promise.vehicaldata;
                    $scope.driverdata = promise.driverdata;
                    $scope.sessiondetils = promise.sessiondata;
                }
                else {
                    swal("No Records Found")
                }
            })


        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //---Save--//
        $scope.savedata = function () {

            if ($scope.myForm.$valid) {
                var data = {
                    "TRVD_Id":$scope.trvD_Id,
                    "TRMV_Id": $scope.trmV_Id,
                    "TRMD_Id": $scope.trmD_Id,
                    "TRMS_Id": $scope.trmS_Id,
                    "TRMS_Flag": $scope.trmS_Flag,
                    "TRVD_Date":new Date($scope.trvD_Date).toDateString()
                }
                apiService.create("VehicalDriverMapping/savedata", data).then(function (promise) {
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
        $scope.filterValue = function (obj) {
            
            return (angular.lowercase(obj.trmV_VehicleName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.trmD_DriverName)).indexOf(angular.lowercase($scope.searchValue)) >= 0     ||
                (angular.lowercase(obj.trmS_SessionName)).indexOf(angular.lowercase($scope.searchValue)) >= 0    ||
                (angular.lowercase(obj.trmS_Flag)).indexOf(angular.lowercase($scope.searchValue)) >= 0           ||
                ($filter('date')(obj.trvD_Date, 'dd/MM/yyyy').indexOf($scope.searchValue) >= 0)
        }


        //---Edit Data--//
        $scope.edit = function (user) {
            var data = {
                "TRVD_Id": user.trvD_Id
            }
            
            apiService.create("VehicalDriverMapping/editdata", data).then(function (Promise) {
                if (Promise != null) {
                    $scope.trvD_Id = Promise.editdata[0].trvD_Id;
                    $scope.trmV_Id = Promise.editdata[0].trmV_Id;
                    $scope.trmD_Id = Promise.editdata[0].trmD_Id;
                    $scope.trvD_Date = new Date(Promise.editdata[0].trvD_Date);
                    $scope.trmS_Flag = Promise.editdata[0].trmS_Flag;
                    $scope.trmS_Id = Promise.editdata[0].trmS_Id;
                    

                }
             
                $scope.vehicaldetails = promise.vehicaldata;
                $scope.driverdata = promise.driverdata;
                $scope.sessiondetils = promise.sessiondata;
               
               
            })
        }
        //--Active Deactive--//
        $scope.deactive = function (user, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (user.trvD_ActiveFlg === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";
            }
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
                apiService.create("VehicalDriverMapping/activedeactive/", user).
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

        //-Clear-//
        $scope.clearid = function () {
            $state.reload();
            //$scope.trvD_Id = 0;
            //$scope.trmV_VehicleName = "";
            //$scope.submitted = false;
            //$scope.myForm.$setPristine();
            //$scope.myForm.$setUntouched();
        };
        //--Sorting--//     
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }


        $scope.searchValue = '';

    };



})();


