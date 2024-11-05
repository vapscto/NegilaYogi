
(function () {
    'use strict';
    angular
.module('app')
.controller('MasterDriverController', MasterDriverController)

    MasterDriverController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter']
    function MasterDriverController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.sortKey = 'trmD_Id';
        $scope.sortReverse = true;
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.searchValue = "";
        $scope.list = false;

        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("MasterDriver/getdata", pageid).then(function (promise) {
                if (promise != null) {
                    if (promise.getdatamaster.length > 0) {
                        $scope.driverlist = promise.getdatamaster;
                        $scope.presentCountgrid = $scope.driverlist.length;
                        $scope.list = true;
                    }
                    else {
                        swal("No Records Found")
                        $scope.list = true;
                    }
                }
                else {
                    swal("No Records Found")
                    $scope.list = true;
                }
            })
        }

        $scope.submitted = false;
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //---Save Data--//
        $scope.savedata = function () {
            debugger;
            if ($scope.myForm.$valid) {
                if ($scope.checkoruncheck == true) {
                    $scope.checkoruncheck = true;
                }
                else {
                    $scope.checkoruncheck = false;
                }
                if ($scope.rtoname == null || $scope.rtoname == "") {
                    $scope.rtoname = "";
                }


                var dldate = $scope.dlexpdate == null ? "" : $filter('date')($scope.dlexpdate, "yyyy-MM-dd");
                var mddate = $scope.dlmtestdate == null ? "" : $filter('date')($scope.dlmtestdate, "yyyy-MM-dd");
                var safdate = $scope.dlsafetestdate == null ? "" : $filter('date')($scope.dlsafetestdate, "yyyy-MM-dd");



                var data = {
                    "TRMD_Id": $scope.TRMD_Id,
                    "TRMD_DriverName": $scope.drivername,
                    "TRMD_DriverCode": $scope.drivercode,
                    "TRMD_DLNo": $scope.driverdlno,
                    "TRMD_RTOName": $scope.rtoname,
                    "TRMD_DLExpiryDate": dldate,
                    "TRMD_MTExpiryDate": mddate,
                    "TRMD_SDExpiryDate": safdate,
                    "TRMD_DriverBadgeNo": $scope.driverbno,
                    "TRMD_LicenseType": $scope.driverdltype,
                    "TRMD_SpareDriverFlg": $scope.checkoruncheck,
                    "TRMD_MobileNo": $scope.TRMD_MobileNo,
                    "TRMD_EmailId": $scope.TRMD_EmailId
                }
                apiService.create("MasterDriver/savedata", data).then(function (promise) {
                    if (promise.message == "Add") {
                        if (promise.returnval == true) {
                            swal("Record Saved Successfully");
                        }
                        else {
                            swal("Failed To Save Record");
                        }
                    }
                    else if (promise.message == "Update") {
                        if (promise.returnval == true) {
                            swal("Record Update Successfully");
                        }
                        else {
                            swal("Failed To Update Record");
                        }
                    }
                    else if (promise.message == "Duplicate") {
                        swal("Record Already Exists")
                    }
                    $state.reload();
                })
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.clearid = function () {
            $scope.TRMD_Id = 0;
            $state.reload();
        }

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }

      

        //---Check Driver code duplicate--//
        $scope.checkdrivercode = function () {
            var data = {
                "TRMD_Id": $scope.TRMD_Id,
                "TRMD_DriverCode": $scope.drivercode,
            }
            apiService.create("MasterDriver/checkdrivercode", data).then(function (promise) {
                if (promise != null) {
                    if (promise.message == "Duplicate") {
                        swal("Driver Code Already Exists");
                        $scope.drivercode = "";
                    }
                    else {

                    }
                }
            })
        }

        //--Check drive dl duplicate --//
        $scope.checkdriverdl = function () {
            var data = {
                "TRMD_Id": $scope.TRMD_Id,
                "TRMD_DLNo": $scope.driverdlno,
            }
            apiService.create("MasterDriver/checkdriverdl", data).then(function (promise) {
                if (promise != null) {
                    if (promise.message == "Duplicate") {
                        swal("DL Number Already Exists");
                        $scope.driverdlno = "";
                    }
                    else {

                    }
                }
            })
        }

        //--Check drive badgeno duplicate --//
        $scope.checkdriverbno = function () {
            var data = {
                "TRMD_Id": $scope.TRMD_Id,
                "TRMD_DriverBadgeNo": $scope.driverbno,
            }
            apiService.create("MasterDriver/checkdriverbno", data).then(function (promise) {
                if (promise != null) {
                    if (promise.message == "Duplicate") {
                        swal("Driver Badge Number Already Exists");
                        $scope.driverbno = "";
                    }
                    else {

                    }
                }
            })
        }

        //--Edit data--//
        $scope.edit = function (user) {
            var data = {
                "TRMD_Id": user.trmD_Id,
            }
            apiService.create("MasterDriver/editdata", data).then(function (promise) {
                if (promise != null) {
                    if (promise.getdatamasteredit.length > 0) {
                        $scope.TRMD_Id = promise.getdatamasteredit[0].trmD_Id;
                        $scope.drivername = promise.getdatamasteredit[0].trmD_DriverName;
                        $scope.drivercode = promise.getdatamasteredit[0].trmD_DriverCode;
                        $scope.driverdlno = promise.getdatamasteredit[0].trmD_DLNo;
                        $scope.rtoname = promise.getdatamasteredit[0].trmD_RTOName;

                        $scope.dlexpdate = new Date(promise.getdatamasteredit[0].trmD_DLExpiryDate);
                        if (promise.getdatamasteredit[0].trmD_MTExpiryDate != null && promise.getdatamasteredit[0].trmD_MTExpiryDate != undefined) {
                            $scope.dlmtestdate = new Date(promise.getdatamasteredit[0].trmD_MTExpiryDate);
                        }


                        if (promise.getdatamasteredit[0].trmD_SDExpiryDate != null && promise.getdatamasteredit[0].trmD_SDExpiryDate != undefined) {
                            $scope.dlsafetestdate = new Date(promise.getdatamasteredit[0].trmD_SDExpiryDate);
                        }
                       
                      

                        $scope.driverbno = promise.getdatamasteredit[0].trmD_DriverBadgeNo;
                        $scope.driverdltype = promise.getdatamasteredit[0].trmD_LicenseType;
                        $scope.checkoruncheck = promise.getdatamasteredit[0].trmD_SpareDriverFlg;

                        $scope.TRMD_MobileNo = promise.getdatamasteredit[0].trmD_MobileNo;
                        $scope.TRMD_EmailId = promise.getdatamasteredit[0].trmD_EmailId;
                        
                    }
                    else {
                        swal("Something Went Wrong Please Contact Administrator");
                    }
                }
            })
        }

        //--Active Deactive Driver Master--//
        $scope.deactive = function (user, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (user.trmD_ActiveFlg === true) {
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
                apiService.create("MasterDriver/activedeactive/", user).
                then(function (promise) {
                    if (promise.message != null) {
                        swal(promise.message);
                    }
                    else {
                        if (promise.returnval == true) {
                            swal(confirmmgs + " " + "Successfully.");

                        }
                        else {
                            swal("Failed To " + confirmmgs + "Record");

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
    }
})();


