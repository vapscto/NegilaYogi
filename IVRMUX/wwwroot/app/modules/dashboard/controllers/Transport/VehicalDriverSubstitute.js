
(function () {
    'use strict';
    angular
.module('app')
.controller('VehicalDriverSubstituteController', VehicalDriverSubstituteController)

    VehicalDriverSubstituteController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter']
    function VehicalDriverSubstituteController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter) {

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
            apiService.getURI("VehicalDriverSubstitute/getdata", pageid).then(function (promise) {
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
                    // $scope.S_driverdata = promise.driverdata;
                    $scope.A_driverdata = promise.driverdata;


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
                    "TRVDST_Id": $scope.trvDST_Id,
                    "TRMV_Id": $scope.trmV_Id,
                    "TRVDS_AbsentDriverId": $scope.trvDS_AbsentDriverId,
                    "TRVDS_SubstituteDriverId": $scope.trvDS_SubstituteDriverId,
                    "TRVDS_FromDate": new Date($scope.trvDS_FromDate).toDateString(),
                    "TRVDS_ToDate": new Date($scope.trvDS_ToDate).toDateString()
                }
                apiService.create("VehicalDriverSubstitute/savedata", data).then(function (promise) {
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
            (angular.lowercase(obj.absent_Driver)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.substitute_Driver)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                ($filter('date')(obj.trvdS_FromDate, 'dd/MM/yyyy').indexOf($scope.searchValue) >= 0) ||
                ($filter('date')(obj.trvdS_ToDate, 'dd/MM/yyyy').indexOf($scope.searchValue) >= 0)
        }


        //---Edit Data--//
        $scope.edit = function (user) {
            var data = {
                "TRVDST_Id": user.trvdsT_Id
            }

            apiService.create("VehicalDriverSubstitute/editdata", data).then(function (Promise) {
                if (Promise != null) {
                    $scope.trvdsT_Id = Promise.editdata[0].trvdsT_Id;
                    $scope.trmV_Id = Promise.editdata[0].trmV_Id;
                    $scope.get_Driver();
                    $scope.trvDS_AbsentDriverId = Promise.editdata[0].trvdS_AbsentDriverId;
                    $scope.trvDS_FromDate = new Date(Promise.editdata[0].trvdS_FromDate);
                    $scope.trvDS_ToDate = new Date(Promise.editdata[0].trvdS_ToDate);

                    // $scope.temp_trvDS_SubstituteDriverId = Promise.editdata[0].trvdS_SubstituteDriverId;
                    $scope.trvDS_SubstituteDriverId = Promise.editdata[0].trvdS_SubstituteDriverId;
                    $scope.get_Driver_S();
                    

                }

               // $scope.vehicaldetails = promise.vehicaldata;
               // $scope.driverdata = promise.driverdata;



            })
        }


        //-Clear-//
        $scope.clearid = function () {
            $scope.trvD_Id = 0;
            $scope.trmV_VehicleName = "";
            $scope.submitted = false;
            $scope.myForm1.$setPristine();
            $scope.myForm1.$setUntouched();
        };
        //--Sorting--//     
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }


        $scope.searchValue = '';

        $scope.get_Driver_S = function () {
            $scope.S_driverdata = [];
            angular.forEach($scope.driverdata, function (t) {
                if (t.trmD_Id != $scope.trvDS_AbsentDriverId) {
                    $scope.S_driverdata.push(t);
                }
            })
            //if($scope.trvdsT_Id!=undefined && $scope.trvdsT_Id>0 &&$scope.temp_trvDS_SubstituteDriverId!=undefined && $scope.temp_trvDS_SubstituteDriverId>0)
            //{
            //    $scope.trvDS_SubstituteDriverId = $scope.temp_trvDS_SubstituteDriverId;
            //}
        }
        $scope.get_Driver = function ()
        {
            //var data = {
            //    "TRMV_Id": $scope.trmV_Id
            //}
            apiService.getURI("VehicalDriverSubstitute/get_driver_data", $scope.trmV_Id).then(function (promise) {
                $scope.A_driverdata = promise.driverdata;
                if($scope.A_driverdata.length==0)
                {
                    swla("Selectd Vehicle Not Mapped With Any Drivers");
                    $scope.trmV_Id="";
                }
                if ($scope.trvdsT_Id == undefined || $scope.trvdsT_Id == 0)
                {
                    $scope.trvDS_AbsentDriverId = "";
                    $scope.trvDS_SubstituteDriverId = "";
                }
               
               
            })
        }

    };



})();


