
(function () {
    'use strict';
    angular
.module('app')
.controller('RTODetailsController', RTODetailsController)

    RTODetailsController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function RTODetailsController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

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
            var pageid = 2;
            apiService.getURI("RTODetails/getdata/", pageid).then(function (promise) {
                $scope.masterdistancerate = promise.getloaddata;
                $scope.fillvahicleno = promise.fillvahicleno;
                if (promise.getloaddata.length > 0) {


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
            apiService.create("RTODetails/Onvahiclechange/", data).then(function (promise) {
             

                $scope.filldrivrname = promise.filldrivrname;
               
            })

        }

        $scope.checkvalid = function () {
            ////if (parseInt($scope.TRDC_FromKM) >= parseInt($scope.TRDC_ToKM)) {
            ////    swal("To KM Must Be Grather Than From KM");
            ////    $scope.TRDC_ToKM = parseInt($scope.TRDC_FromKM) + 1;
            ////} else {
            ////    $scope.TRDC_ToKM = $scope.TRDC_ToKM;
            ////}

            if (parseInt($scope.TRDC_ToKM) >= parseInt($scope.TRDC_FromKM)) 
                {
                $scope.TRDC_TotalKM = $scope.TRDC_ToKM - $scope.TRDC_FromKM;
                if ($scope.TRDC_TotalKM != 0 && $scope.TRDC_NoofLtr != 0) {

                    $scope.TRDC_TotalMileage = $scope.TRDC_TotalKM / $scope.TRDC_NoofLtr;
                }
                }
           
        }

        $scope.ratechange = function () {
            
            $scope.TRDC_TotalAmount = $scope.TRDC_NoofLtr * $scope.TRDC_RateKm;
            $scope.TRDC_GrossAmount = $scope.TRDC_AddtAmt + $scope.TRDC_Emission + $scope.TRDC_TotalAmount;
        }

        $scope.TRDC_TotalKM = 0;
      
        $scope.onliterchange = function () {

            if ($scope.TRDC_TotalKM != 0 && $scope.TRDC_NoofLtr != 0) {

                $scope.TRDC_TotalMileage = $scope.TRDC_TotalKM / $scope.TRDC_NoofLtr;
            }
            $scope.TRDC_TotalAmount = $scope.TRDC_NoofLtr * $scope.TRDC_RateKm;
            $scope.TRDC_GrossAmount = $scope.TRDC_AddtAmt + $scope.TRDC_Emission + $scope.TRDC_TotalAmount;
        }

        $scope.calcgross = function () {

            $scope.TRDC_GrossAmount = $scope.TRDC_AddtAmt + $scope.TRDC_Emission + $scope.TRDC_TotalAmount;
        }



        $scope.delete = function (DeleteRecord, SweetAlert) {
            $scope.deleteId = DeleteRecord.trrtO_Id;
            var MdeleteId = $scope.deleteId;
            // swal(id);
            swal({
                title: "Are you sure?",
                text: "Do you want to delete this item?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes,delete it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {
                    apiService.DeleteURI("RTODetails/deleterecord", MdeleteId).
                    then(function (promise) {
                        if (promise.returnval==true) {
                            swal("Record Deleted successfully");
                          
                        }
                        else {
                            swal("Error");
                          
                        }
                        
                        $state.reload();
                    })
                }
                else {
                    swal(" Cancelled", "Ok");
                }
            });
        }














        $scope.submitted = false;
        $scope.savedata = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "TRRTO_Id": $scope.TRRTO_Id,
                    "TRMV_Id": $scope.trmV_Id,
                    "TRRTO_Insurance_FromDate": $scope.TRRTO_Insurance_FromDate,
                    "TRRTO_Insurance_Todate": $scope.TRRTO_Insurance_Todate,
                    "TRRTO_Company_Name": $scope.TRRTO_Company_Name,
                    "TRRTO_Tax_FromDate": $scope.TRRTO_Tax_FromDate,
                    "TRRTO_Tax_ToDate": $scope.TRRTO_Tax_ToDate,

                    "TRRTO_FC_FromDate": $scope.TRRTO_FC_FromDate,
                    "TRRTO_FC_ToDate": $scope.TRRTO_FC_ToDate,
                    "TRRTO_Permit_FromDate": $scope.TRRTO_Permit_FromDate,
                    "TRRTO_Permit_ToDate": $scope.TRRTO_Permit_ToDate,
                    "TRRTO_Emission_FromDate": $scope.TRRTO_Emission_FromDate,
                    "TRRTO_Emission_ToDate": $scope.TRRTO_Emission_ToDate,
                    "TRRTO_Ceasefire_FromDate": $scope.TRRTO_Ceasefire_FromDate,
                    "TRRTO_Ceasefire_ToDate": $scope.TRRTO_Ceasefire_ToDate,
                    "TRRTO_GPS_GPRS_Fitted_date": $scope.TRRTO_GPS_GPRS_Fitted_date
                }
                apiService.create("RTODetails/savedata/", data).then(function (promise) {
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
                "TRRTO_Id": user.trrtO_Id,
            }
            apiService.create("RTODetails/edit/", data).then(function (promise) {
                if (promise.geteditdata.length > 0) {
                    $scope.TRRTO_Id = promise.geteditdata[0].trrtO_Id;
                    $scope.trmV_Id = promise.geteditdata[0].trmV_Id;
                    $scope.TRRTO_Insurance_FromDate = new Date(promise.geteditdata[0].trrtO_Insurance_FromDate);
                    $scope.TRRTO_Insurance_Todate = new Date(promise.geteditdata[0].trrtO_Insurance_Todate);
                    $scope.TRRTO_Company_Name =promise.geteditdata[0].trrtO_Company_Name;
                  
                    $scope.TRRTO_Tax_FromDate = new Date(promise.geteditdata[0].trrtO_Tax_FromDate);
                    $scope.TRRTO_Tax_ToDate = new Date(promise.geteditdata[0].trrtO_Tax_ToDate);
                    $scope.TRRTO_FC_ToDate = new Date(promise.geteditdata[0].trrtO_FC_ToDate);
                    $scope.TRRTO_FC_FromDate = new Date(promise.geteditdata[0].trrtO_FC_FromDate);
                    $scope.TRRTO_Permit_FromDate =new Date(promise.geteditdata[0].trrtO_Permit_FromDate);
                    
                    $scope.TRRTO_Permit_ToDate = new Date(promise.geteditdata[0].trrtO_Permit_ToDate);
                    $scope.TRRTO_Emission_FromDate = new Date(promise.geteditdata[0].trrtO_Emission_FromDate);
                    $scope.TRRTO_Emission_ToDate = new Date(promise.geteditdata[0].trrtO_Emission_ToDate);
                    $scope.TRRTO_Ceasefire_FromDate = new Date(promise.geteditdata[0].trrtO_Ceasefire_FromDate);
                    $scope.TRRTO_Ceasefire_ToDate = new Date(promise.geteditdata[0].trrtO_Ceasefire_ToDate);
                    $scope.TRRTO_GPS_GPRS_Fitted_date = new Date(promise.geteditdata[0].trrtO_GPS_GPRS_Fitted_date);
                  
                  
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
        //$scope.filterValue = function (obj) {
        //    
        //    return (JSON.stringify(obj.trdC_FromKM)).indexOf($scope.searchValue) >= 0 ||
        //        (JSON.stringify(obj.trdC_ToKM)).indexOf($scope.searchValue) >= 0 ||
        //        (JSON.stringify(obj.trdC_RateKm)).indexOf($scope.searchValue) >= 0
        //}

        $scope.clear = function () {
            $state.reload();
        }
    };
})();


