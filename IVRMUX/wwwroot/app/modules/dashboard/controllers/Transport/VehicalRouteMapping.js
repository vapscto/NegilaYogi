
(function () {
    'use strict';
    angular
.module('app')
.controller('VehicalRouteMappingController', VehicalRouteMappingController)

    VehicalRouteMappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter']
    function VehicalRouteMappingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter) {

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
         
           
            apiService.getDATA("VehicalRouteMapping/getdata").then(function (promise) {
             
                if (promise != null) {

                    if (promise.savedata.length > 0) {
                      //  $scope.mastervehicle = promise.savedata;
                        $scope.griddetails = promise.savedata;

                        $scope.presentCountgrid = $scope.griddetails.length;
                        $scope.masterlist = true;
                    }
                    else {
                        swal("No Records Found")
                        $scope.masterlist = false;
                    }

                    $scope.vehicaldetails = promise.vehicaldata;
                    $scope.routedetails = promise.routedata;
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


        //-------Save Data-------//
        $scope.savedata = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "TRVR_Id": $scope.trvR_Id,
                    "TRMV_Id": $scope.trmV_Id,
                    "TRMR_Id": $scope.trmR_Id,
                    "TRMS_Id": $scope.trmS_Id,
                    "TRMS_Flag":$scope.trmS_Flag,
                    "TRVR_Date":new Date($scope.trvR_Date).toDateString()

                }
                apiService.create("VehicalRouteMapping/savedata", data).then(function (promise) {
                    if (promise != null) {
                        if (promise.message == "Add") {
                            if (promise.retrunval == true) {
                                swal("Record Saved Successfully");
                            }
                            else {
                                swal("Failed To Save Record");
                            }
                        }
                        else if (promise.message == "Update") {
                            if (promise.retrunval == true) {
                                swal("Record Update Successfully");
                            }
                            else {
                                swal("Failed To Update Record");
                            }
                        }
                        else if (promise.message == "Duplicate") {
                            swal("Record Already Exists");
                        }
                    }
                    else {
                        
                    }
                    $state.reload();
                })
            } else {
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
                  (angular.lowercase(obj.trmR_RouteName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.trmS_SessionName)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (angular.lowercase(obj.trmS_Flag)).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                ($filter('date')(obj.trvR_Date, 'dd/MM/yyyy').indexOf($scope.searchValue) >= 0)
        }


        //---Edit Data--//
        $scope.edit = function (user) {
            var data = {
                "TRVR_Id": user.trvR_Id
            }
         
            apiService.create("VehicalRouteMapping/editdata", data).then(function (Promise) {
                if (Promise != null) {
                    
                    $scope.trvR_Id = Promise.editdata[0].trvR_Id;
                    $scope.trmV_Id = Promise.editdata[0].trmV_Id;
                    $scope.trmR_Id = Promise.editdata[0].trmR_Id;
                    $scope.trvR_Date = new Date(Promise.editdata[0].trvR_Date);
                    $scope.trmS_Flag = Promise.editdata[0].trmS_Flag;
                    $scope.trmS_Id = Promise.editdata[0].trmS_Id;


                }
                $scope.vehicaldetails = promise.vehicaldata;
                $scope.routedetails = promise.routedata;
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
            if (user.trvR_ActiveFlg === true) {
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
                apiService.create("VehicalRouteMapping/activedeactive/", user).
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


        $scope.searchValue = '';

    };



})();


