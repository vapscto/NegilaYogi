
(function () {
    'use strict';
    angular
.module('app')
.controller('DriverChartController', DriverChartController)

    DriverChartController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache','$filter']
    function DriverChartController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.masterlist = false;
        $scope.sortKey = 'trdC_Id';
        $scope.sortReverse = true;

        $scope.feeorder = false;
        $scope.feetext = true;
        $scope.searchValue = "";

        $scope.loaddata = function () {

            $scope.masterdistancerate = [];
            var pageid = 2;
            apiService.getURI("DriverChart/getdata/", pageid).then(function (promise) {
                $scope.masterdistancerate = promise.getloaddata;
                $scope.fillvahicleno = promise.fillvahicleno;
                $scope.filldrivrname = promise.filldrivrname;

                if ($scope.masterdistancerate.length > 0) {

                    angular.forEach($scope.masterdistancerate, function (xx) {

                        xx.trdC_Date = xx.trdC_Date == null ? "" : $filter('date')(xx.trdC_Date, "dd/MM/yyyy");

                    })

                    //
                    //$scope.presentCountgrid = promise.getloaddata.length;
                    $scope.masterlist = true;
                }
                else {
                    swal("No Records Found");
                    $scope.masterlist = false;
                }
            })
        }

        $scope.onvahiclechange = function ()
        {
            var data = {
                "TRMV_Id": $scope.trmV_Id,
            }
            apiService.create("DriverChart/Onvahiclechange/", data).then(function (promise) {

                if (promise.trdC_FromKM != null) {
                    $scope.TRDC_FromKM = promise.trdC_FromKM;
                }
                //$scope.filldrivrname = promise.filldrivrname;
               
            })

        }

        $scope.checkvalid = function () {
            ////if (parseInt($scope.TRDC_FromKM) >= parseInt($scope.TRDC_ToKM)) {
            ////    swal("To KM Must Be Grather Than From KM");
            ////    $scope.TRDC_ToKM = parseInt($scope.TRDC_FromKM) + 1;
            ////} else {
            ////    $scope.TRDC_ToKM = $scope.TRDC_ToKM;
            ////}

            if ($scope.TRDC_ToKM != 0) {
                if (parseInt($scope.TRDC_ToKM) >= parseInt($scope.TRDC_FromKM)) {
                    $scope.TRDC_TotalKM = $scope.TRDC_ToKM - $scope.TRDC_FromKM;
                    if ($scope.TRDC_TotalKM != 0 && $scope.TRDC_NoofLtr != 0) {

                        $scope.TRDC_TotalMileage = $scope.TRDC_TotalKM / $scope.TRDC_NoofLtr;
                    }
                }
                else {
                    swal("Closing K.M is Must be Greater than Opening K.M")
                    $scope.TRDC_ToKM = '';
                }
            }

           
           
        }

        $scope.ratechange = function () {
            debugger;
            $scope.TRDC_TotalAmount = $scope.TRDC_NoofLtr * $scope.TRDC_RateKm;

            var num = parseFloat($scope.TRDC_TotalAmount);
            $scope.TRDC_TotalAmount = num.toFixed(3);
            $scope.TRDC_TotalAmount = Number($scope.TRDC_TotalAmount);

            if ($scope.TRDC_AddtAmt == undefined || $scope.TRDC_AddtAmt == null) {
                $scope.TRDC_AddtAmt = 0;
            }
            if ($scope.TRDC_Emission == undefined || $scope.TRDC_Emission == null) {
                $scope.TRDC_Emission = 0;
            }

            $scope.TRDC_GrossAmount = $scope.TRDC_AddtAmt + $scope.TRDC_Emission + $scope.TRDC_TotalAmount;
        }

        $scope.TRDC_TotalKM = 0;
      
        $scope.onliterchange = function () {
            debugger;
            if ($scope.TRDC_TotalKM != 0 && $scope.TRDC_NoofLtr != 0) {

                $scope.TRDC_TotalMileage = $scope.TRDC_TotalKM / $scope.TRDC_NoofLtr;
            }
            if ($scope.TRDC_RateKm == undefined || $scope.TRDC_RateKm == null) {
                $scope.TRDC_RateKm = 0;
            }
            if ($scope.TRDC_AddtAmt == undefined || $scope.TRDC_AddtAmt == null) {
                $scope.TRDC_AddtAmt = 0;
            }
            if ($scope.TRDC_Emission == undefined || $scope.TRDC_Emission == null) {
                $scope.TRDC_Emission = 0;
            }
            $scope.TRDC_TotalAmount = Number($scope.TRDC_NoofLtr) * Number($scope.TRDC_RateKm);
            var num = parseFloat($scope.TRDC_TotalAmount);
            $scope.TRDC_TotalAmount = num.toFixed(3);
            $scope.TRDC_TotalAmount = Number($scope.TRDC_TotalAmount)

            $scope.TRDC_GrossAmount = $scope.TRDC_AddtAmt + $scope.TRDC_Emission + $scope.TRDC_TotalAmount;
        }

        $scope.calcgross = function () {

            $scope.TRDC_GrossAmount = $scope.TRDC_AddtAmt + $scope.TRDC_Emission + $scope.TRDC_TotalAmount;
        }


        $scope.submitted = false;
        $scope.savedata = function () {
            if ($scope.myForm.$valid) {

                var fromdate1 = $scope.TRDC_Date == null ? "" : $filter('date')($scope.TRDC_Date, "yyyy-MM-dd");
                var data = {
                    "TRDC_Id":$scope.TRDC_Id,
                    "TRMV_Id": $scope.trmV_Id,
                    "TRMD_Id": $scope.trmD_Id,
                    "TRDC_FromKM": $scope.TRDC_FromKM,
                    "TRDC_ToKM": $scope.TRDC_ToKM,
                    "TRDC_TotalKM": $scope.TRDC_TotalKM,
                    "TRDC_NoofLtr": $scope.TRDC_NoofLtr,

                    "TRDC_TotalMileage": $scope.TRDC_TotalMileage,
                    "TRDC_RateKm": $scope.TRDC_RateKm,
                    "TRDC_TotalAmount": $scope.TRDC_TotalAmount,
                    "TRDC_Emission": $scope.TRDC_Emission,
                    "TRDC_AddtAmt": $scope.TRDC_AddtAmt,
                    "TRDC_Remarks": $scope.TRDC_Remarks,
                    "TRDC_GrossAmount": $scope.TRDC_GrossAmount,
                    "TRDC_Date": fromdate1,
                    "TRDC_Indent_BillNo": $scope.TRDC_Indent_BillNo
                }
                apiService.create("DriverChart/savedata/", data).then(function (promise) {
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
                            swal("Record Updated Successfully");
                        }
                        else {
                            swal("Failed To Update Record");
                        }
                    }
                    else if (promise.message == "Duplicate") {
                        swal("Record Already Exists");
                    }
                    else {

                    }
                    $state.reload();
                })
            }
            else {
                $scope.submitted = true;
            }
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.edit = function (user) {
            var data = {
                "TRDC_Id": user.trdC_Id,
            }
            apiService.create("DriverChart/edit/", data).then(function (promise) {
                if (promise.geteditdata.length > 0) {
                    $scope.TRDC_Id = promise.geteditdata[0].trdC_Id;
                    $scope.TRDC_FromKM = promise.geteditdata[0].trdC_FromKM;
                    $scope.TRDC_ToKM = promise.geteditdata[0].trdC_ToKM;
                    $scope.TRDC_RateKm = promise.geteditdata[0].trdC_RateKm;
                    $scope.trmV_Id = promise.geteditdata[0].trmV_Id;
                    $scope.trmD_Id = promise.geteditdata[0].trmD_Id;
                    $scope.TRDC_FromKM = promise.geteditdata[0].trdC_FromKM;
                    $scope.TRDC_ToKM = promise.geteditdata[0].trdC_ToKM;
                    $scope.TRDC_TotalKM = promise.geteditdata[0].trdC_TotalKM;
                    $scope.TRDC_NoofLtr = promise.geteditdata[0].trdC_NoofLtr;
                    
                    $scope.TRDC_TotalMileage = promise.geteditdata[0].trdC_TotalMileage;
                    $scope.TRDC_RateKm = promise.geteditdata[0].trdC_RateKm;
                    $scope.TRDC_TotalAmount = promise.geteditdata[0].trdC_TotalAmount;
                    $scope.TRDC_Emission = promise.geteditdata[0].trdC_Emission;
                    $scope.TRDC_AddtAmt = promise.geteditdata[0].trdC_AddtAmt;
                    $scope.TRDC_Remarks = promise.geteditdata[0].trdC_Remarks;
                    $scope.TRDC_GrossAmount = promise.geteditdata[0].trdC_GrossAmount;
                    $scope.TRDC_Date = new Date(promise.geteditdata[0].trdC_Date);
                    $scope.TRDC_Indent_BillNo = promise.geteditdata[0].trdC_Indent_BillNo;
                  //  $scope.onvahiclechange();
                    angular.forEach($scope.filldrivrname, function (zz) {
                        if ($scope.trmD_Id == zz.trmD_Id) {
                            $scope.trmD_Id = zz.trmD_Id
                        }
                    })
                  
                }
                else {
                }
            })
        }

        $scope.checkvalid1 = function () {
            $scope.TRDC_ToKM = parseInt($scope.TRDC_FromKM) + 1;
        }

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }


        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            
            return (JSON.stringify(obj.trdC_FromKM)).indexOf($scope.searchValue) >= 0 ||
                (JSON.stringify(obj.trdC_ToKM)).indexOf($scope.searchValue) >= 0 ||
                (JSON.stringify(obj.trdC_RateKm)).indexOf($scope.searchValue) >= 0
        }

        $scope.clear = function () {
            $state.reload();
        }


        $scope.deactive = function (employee) {
            var flag = true;
            var mgs = "";
            var confirmmgs = "";

            var data = {
                "TRDC_Id": employee.trdC_Id
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (flag === true) {
                //  mgs = "Deactive";
                mgs = "Delete";
                confirmmgs = "Deleted";
            }
            else {
                //mgs = "Active";
                mgs = "Activate";
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

                        apiService.create("DriverChart/deleterecord", data).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + confirmmgs + " " + "successfully");
                                }
                                else {
                                    swal("Record " + mgs + " Failed");
                                }
                                $state.reload();
                                //$scope.clear();
                                //$scope.BindData();
                            })

                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        }
    };
})();


