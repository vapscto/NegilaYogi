
(function () {
    'use strict';
    angular
.module('app')
        .controller('KMLogBookController', KMLogBookController)

    KMLogBookController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache','$filter']
    function KMLogBookController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.masterlist = false;
       // $scope.sortKey = 'trdC_Id';
        $scope.sortReverse = true;
        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.options = {
            hstep: [1, 2, 3],
            mstep: [1, 5, 10, 15, 25, 30]
        };
        $scope.feeorder = false;
        $scope.feetext = true;
        $scope.searchValue = "";

        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("KMLogBook/getdata/", pageid).then(function (promise) {
                $scope.masterdistancerate = promise.getloaddata;
                $scope.fillvahicleno = promise.fillvahicleno;
                $scope.filldrivrname = promise.filldrivrname;
                if ($scope.masterdistancerate.length > 0) {


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
            apiService.create("KMLogBook/Onvahiclechange/", data).then(function (promise) {

                if (promise.trkmlB_ClosingReading != null && promise.trkmlB_ClosingReading !='') {
                    $scope.TRDC_FromKM = Number(promise.trkmlB_ClosingReading);
                }
                else {
                    $scope.TRDC_FromKM = 0;
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
            debugger;
            if (parseInt($scope.TRDC_ToKM) >= parseInt($scope.TRDC_FromKM)) 
                {
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

        $scope.ratechange = function () {
            debugger;
            $scope.TRDC_TotalAmount = $scope.TRDC_NoofLtr * $scope.TRDC_RateKm;

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
            $scope.TRDC_TotalAmount = $scope.TRDC_NoofLtr * $scope.TRDC_RateKm;
            $scope.TRDC_GrossAmount = $scope.TRDC_AddtAmt + $scope.TRDC_Emission + $scope.TRDC_TotalAmount;
        }

        $scope.calcgross = function () {

            $scope.TRDC_GrossAmount = $scope.TRDC_AddtAmt + $scope.TRDC_Emission + $scope.TRDC_TotalAmount;
        }


        $scope.submitted = false;
        $scope.savedata = function () {
            debugger;
            if ($scope.myForm.$valid) {
                var startTime = $filter('date')($scope.TRKMLB_FromTime, "h:mm a");
                var endTime = $filter('date')($scope.TRKMLB_ToTime, "h:mm a");
                var entrydate = $scope.TRKMLB_EntryDate == null ? "" : $filter('date')($scope.TRKMLB_EntryDate, "yyyy-MM-dd");
                var fromdate1 = $scope.TRKMLB_FromDate == null ? "" : $filter('date')($scope.TRKMLB_FromDate, "yyyy-MM-dd");
                var todate = $scope.TRKMLB_ToDate == null ? "" : $filter('date')($scope.TRKMLB_ToDate, "yyyy-MM-dd");
                var data = {
                    "TRKMLB_Id": $scope.TRKMLB_Id,
                    "TRMV_Id": $scope.trmV_Id,
                    "TRKMLB_EntryDate": entrydate,
                    "TRKMLB_FromDate": fromdate1,
                    "TRKMLB_FromTime": startTime,
                    "TRKMLB_ToDate": todate,
                    "TRKMLB_ToTime": endTime,
                    "TRKMLB_OpeningReading": $scope.TRDC_FromKM,
                    "TRKMLB_ClosingReading": $scope.TRDC_ToKM,
                    "TRKMLB_NoOfKM": $scope.TRDC_TotalKM,
                    "TRKMLB_Remarks": $scope.TRKMLB_Remarks,
                
                }
                apiService.create("KMLogBook/savedata/", data).then(function (promise) {
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
            $scope.TRKMLB_FromTime = null;
            $scope.TRKMLB_ToTime = null;
            debugger;
            var data = {
                "TRKMLB_Id": user.trkmlB_Id,
            }
            apiService.create("KMLogBook/edit/", data).then(function (promise) {
                if (promise.geteditdata.length > 0) {
                    $scope.TRKMLB_Id = promise.geteditdata[0].trkmlB_Id;
                    //$scope.TRDC_ToKM = promise.geteditdata[0].trdC_ToKM;
                    $scope.TRKMLB_EntryDate = new Date(promise.geteditdata[0].trkmlB_EntryDate);
                    $scope.trmV_Id = promise.geteditdata[0].trmV_Id;
                    $scope.TRDC_FromKM = promise.geteditdata[0].trkmlB_OpeningReading;
                    $scope.TRDC_ToKM = promise.geteditdata[0].trkmlB_ClosingReading;
                    $scope.TRDC_TotalKM = promise.geteditdata[0].trkmlB_NoOfKM;
                    $scope.TRKMLB_FromDate = new Date(promise.geteditdata[0].trkmlB_FromDate);
                    $scope.TRKMLB_ToDate = new Date(promise.geteditdata[0].trkmlB_ToDate);

                   // $scope.TRKMLB_FromTime = promise.geteditdata[0].trkmlB_FromTime;
                   // $scope.TRKMLB_ToTime = promise.geteditdata[0].trkmlB_ToTime;
                    $scope.TRKMLB_Remarks = promise.geteditdata[0].trkmlB_Remarks;
                    $scope.TRDC_FromKM = Number($scope.TRDC_FromKM);
                    $scope.TRDC_ToKM = Number($scope.TRDC_ToKM);

                   // $scope.TRDC_ToKM = promise.geteditdata[0].trkmlB_ClosingReading;

                    if (promise.geteditdata[0].trkmlB_FromTime != null) {
                        $scope.TRKMLB_FromTime = moment(promise.geteditdata[0].trkmlB_FromTime, 'h:mm a').format();
                    }
                    if (promise.geteditdata[0].trkmlB_ToTime != null) {
                        $scope.TRKMLB_ToTime = moment(promise.geteditdata[0].trkmlB_ToTime, 'h:mm a').format();
                    }
                  //  $scope.onvahiclechange();
                   
                  
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
            var flag = employee.trkmlB_ActiveFlag;
            var mgs = "";
            var confirmmgs = "";

            var data = {
                "TRKMLB_Id": employee.trkmlB_Id,
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (flag === true) {
                  mgs = "Deactive";
               // mgs = "Delete";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                //mgs = "Activate";
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

                        apiService.create("KMLogBook/deleterecord", data).
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


