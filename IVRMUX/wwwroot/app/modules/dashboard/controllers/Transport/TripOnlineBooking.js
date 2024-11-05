(function () {
    'use strict';

    angular
        .module('app')
        .controller('TripOnlineBooking', TripOnlineBooking);

    TripOnlineBooking.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache'];

    function TripOnlineBooking($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache) {
        
        $scope.reqSelection = true;
        $scope.TRTOB_BookingDate = new Date();
        $scope.TRTOB_Date = new Date();
        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.options = {
            hstep: [1, 2, 3],
            mstep: [1, 5, 10, 15, 25, 30]
        };

        $scope.searchValue = '';
        $scope.validateTomintime_24 = function (timedata) {
            
            $scope.TRTOB_ToTime = "";
            $scope.totime = timedata;
            var hh = $scope.totime.getHours();
            var mm = $scope.totime.getMinutes();
            $scope.min = timedata;

            $scope.min.setMinutes(hh);
            $scope.min.setMinutes(mm);
        }
        $scope.validateTomintime_12 = function (timedata) {
            
            //$scope.TRTOB_ToTime = "";
            //$scope.totime = timedata;
            //var hh = $scope.totime.getHours();
            //var mm = $scope.totime.getMinutes();
            //$scope.min = timedata;

            //$scope.min.setMinutes(hh);
            //$scope.min.setMinutes(mm);
        }
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.loaddata = function () {
            
            apiService.getURI("TripOnlineBooking/loadData", 4).then(function (promise) {
                if (promise.returnVal != null && promise.returnVal != '') {
                    swal(promise.returnVal);
                }
                else {
                    $scope.TRTOB_BookingId = promise.trtoB_BookingId;
                }
                $scope.hirerGroupList = promise.hirerGroupList;
                $scope.hirerDrpDwn = promise.hirerDrpDwn;
                $scope.modeOfPaymentList = promise.modeOfPaymentList;
               
                if (promise.count > 0) {
                    $scope.tripOnlineBookingList = promise.tripOnlineBookingList;
                    $scope.presentCountgrid = $scope.tripOnlineBookingList.length;
                }
                else {
                    if (promise.returnVal != null && promise.returnVal != '') {
                        swal(promise.returnVal);
                    }
                    else {
                        swal("No Records Found");
                    }
                }
            });
        }
        $scope.getHirer = function (groupId) {
            if (groupId > 0) {
                apiService.getURI("TripOnlineBooking/getHirer/", groupId).then(function (promise) {
                    if (promise.hirerDrpDwn != null) {
                        $scope.hirerDrpDwn = promise.hirerDrpDwn;
                    }
                    else {
                        swal("No Hirer Name is mapped to selected group name");
                    }
                  
                });
            }
        }
        $scope.getHirerDetails = function (hirerId) {
            if (hirerId > 0) {
                var data = {
                    "TRTOB_BookingDate":new Date($scope.TRTOB_BookingDate).toDateString(),
                    "TRMH_Id": hirerId
                }
                apiService.create("TripOnlineBooking/getHirerDetails/", data).then(function (promise) {
                    if (promise.hirerDetails != null) {
                        $scope.TRTOB_ConatctPerName = promise.hirerDetails[0].trmH_ConatctPerName;
                        $scope.TRTOB_ContactPersonDesg = promise.hirerDetails[0].trmH_ContactPersonDesg;
                        $scope.TRTOB_ContactNo = promise.hirerDetails[0].trmH_ContactNo;
                        $scope.TRTOB_MobileNo = promise.hirerDetails[0].trmH_MobileNo;
                        $scope.TRTOB_EmailId = promise.hirerDetails[0].trmH_EmailId;
                        $scope.TRTOB_Address = promise.hirerDetails[0].trmH_Address;
                        $scope.TRTOB_BookingId = promise.trtoB_BookingId;
                    }
                });
            }
        }
        $scope.modeofPayment = function (val) {
            $scope.reqSelection = false;
            $scope.PaymentMode = val;
            if (val == 'Cheque' || val == 'DD') {
                $scope.Cheque = true;
            }
            else {
                $scope.Cheque = false;
            }
        }
        $scope.submitted = false;
        $scope.savedata = function () {
            debugger;
            if ($scope.myForm.$valid) {
                
                var startTime = $filter('date')($scope.TRTOB_FromTime, "h:mm a");
                var endTime = $filter('date')($scope.TRTOB_ToTime, "h:mm a");
               
                var obj = {
                    "TRTOB_Id":$scope.TRTOB_Id,
                    "TRTOB_BookingDate": new Date($scope.TRTOB_BookingDate).toDateString(),
                    "TRTOB_Date":new Date($scope.TRTOB_Date).toDateString(),
                    "TRTOB_BookingId": $scope.TRTOB_BookingId,
                    "TRHG_Id": $scope.TRHG_Id,
                    "TRTOB_HirerName": $scope.TRTOB_HirerName,
                    "TRTOB_ConatctPerName": $scope.TRTOB_ConatctPerName,
                    "TRTOB_ContactPersonDesg": $scope.TRTOB_ContactPersonDesg,
                    "TRTOB_ContactNo": $scope.TRTOB_ContactNo,
                    "TRTOB_MobileNo": $scope.TRTOB_MobileNo,
                    "TRTOB_EmailId": $scope.TRTOB_EmailId,
                    "TRTOB_Address": $scope.TRTOB_Address,
                    "TRTOB_TripAddress": $scope.TRTOB_TripAddress,
                    "TRTOB_PickUpLocation": $scope.TRTOB_PickUpLocation,
                    "TRTOB_TripFromDate": new Date($scope.TRTOB_TripFromDate).toDateString(),
                    "TRTOB_TripToDate":new Date($scope.TRTOB_TripToDate).toDateString(),
                    "TRTOB_FromTime": startTime,
                    "TRTOB_ToTime": endTime,
                    "TRTOB_BookingAmount": $scope.TRTOB_BookingAmount,
                    "TRTOB_ModeOfPayment": $scope.PaymentMode,
                    "TRTPP_ChequeDDNo": $scope.TRTPP_ChequeDDNo,
                    "TRTOB_TripPurpose": $scope.TRTOB_TripPurpose,
                    "TRTOB_NoOfTravellers": $scope.TRTOB_NoOfTravellers,
                    "TRTOB_DropLocation": $scope.TRTOB_DropLocation,
                   // "TRTPP_ChequeDDDate":new Date($scope.TRTPP_ChequeDDDate).toDateString()
                }
                apiService.create("TripOnlineBooking/save", obj).then(function (promise) {
                    if (promise.returnVal == 'saved') {
                        swal("Record Saved Successfully");
                        $scope.clear();
                        $state.reload();
                    }
                    else if (promise.returnVal == 'updated') {
                        swal("Record Updated Successfully");
                       $state.reload();
                    }
                    else if (promise.returnVal == 'duplicate') {
                        swal("Record already exist");
                    }
                    else if (promise.returnVal == "savingFailed") {
                        swal("Failed to save record");
                    }
                    else if (promise.returnVal == "failedUpdate") {
                        swal("Failed to update record");
                    }
                    else {
                        swal("Sorry...something went wrong");
                    }
                });
            }
            else {
                $scope.submitted = true;
            }
        }
        $scope.edit = function (data) {
            $scope.TRTOB_Id = data.trtoB_Id;
            apiService.getURI("TripOnlineBooking/edit/", $scope.TRTOB_Id).then(function (promise) {
                $scope.editDataList = promise.editDataList;
                if ($scope.editDataList[0].trtoB_FromTime != null && $scope.editDataList[0].trtoB_FromTime!= undefined) {
                    $scope.TRTOB_FromTime = moment($scope.editDataList[0].trtoB_FromTime, 'h:mm a').format();
                }
                if ($scope.editDataList[0].trtoB_ToTime != null && $scope.editDataList[0].trtoB_ToTime != undefined) {
                    $scope.TRTOB_ToTime = moment($scope.editDataList[0].trtoB_ToTime, 'h:mm a').format();
                }

             
               
                $scope.TRTOB_BookingDate = new Date($scope.editDataList[0].trtoB_BookingDate);
                $scope.TRTOB_BookingId = $scope.editDataList[0].trtoB_BookingId;
                $scope.TRHG_Id = $scope.editDataList[0].trhG_Id;
                $scope.TRTOB_HirerName = $scope.editDataList[0].trtoB_HirerName;
                for (var i = 0; i < $scope.hirerDrpDwn.length; i++) {
                    if ($scope.hirerDrpDwn[i].trmH_HirerName == $scope.editDataList[0].trtoB_HirerName) {
                        $scope.TRTOB_HirerName = $scope.hirerDrpDwn[i].trmH_Id;
                    }
                }
                $scope.TRTOB_ConatctPerName = $scope.editDataList[0].trtoB_ConatctPerName;
                $scope.TRTOB_ContactPersonDesg = $scope.editDataList[0].trtoB_ContactPersonDesg;
                $scope.TRTOB_ContactNo = $scope.editDataList[0].trtoB_ContactNo;
                $scope.TRTOB_MobileNo = $scope.editDataList[0].trtoB_MobileNo;
                $scope.TRTOB_EmailId = $scope.editDataList[0].trtoB_EmailId;
                $scope.TRTOB_Address = $scope.editDataList[0].trtoB_Address;
                $scope.TRTOB_TripAddress = $scope.editDataList[0].trtoB_TripAddress;
                $scope.TRTOB_PickUpLocation = $scope.editDataList[0].trtoB_PickUpLocation;
                $scope.TRTOB_TripFromDate = new Date($scope.editDataList[0].trtoB_TripFromDate);
                $scope.TRTOB_TripToDate = new Date($scope.editDataList[0].trtoB_TripToDate);
                $scope.TRTOB_TripPurpose = $scope.editDataList[0].trtoB_TripPurpose;
                $scope.TRTOB_NoOfTravellers = $scope.editDataList[0].trtoB_NoOfTravellers;
                $scope.TRTOB_DropLocation = $scope.editDataList[0].trtoB_DropLocation;
            });
        }
        $scope.deactive = function (data) {
            debugger;
            var obj = {
                "TRTOB_Id": data.trtoB_Id,
                "TRTOB_ActiveFlg": true
            }

            var mgs = "Delete";
            var confirmmgs = "Deleted";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + mgs + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {

                        apiService.create("TripOnlineBooking/deactivate/", obj).then(function (promise) {
                            if (promise.returnVal != '' && promise != null) {
                                swal(promise.returnVal);
                                $state.reload();
                            }
                            else {
                                swal("Something went wrong");
                            }
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                })


        }
        $scope.cancel = function () {
            $state.reload();
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.clear = function () {
            $scope.TRTOB_BookingDate = "";
            $scope.TRTOB_Date = "";
            $scope.TRTOB_BookingId = "";
            $scope.TRHG_Id = "";
            $scope.TRTOB_HirerName = "";
            $scope.TRTOB_ConatctPerName = "";
            $scope.TRTOB_ContactPersonDesg = "";
            $scope.TRTOB_ContactNo = "";
            $scope.TRTOB_MobileNo = "";
            $scope.TRTOB_EmailId = "";
            $scope.TRTOB_Address = "";
            $scope.TRTOB_TripAddress = "";
            $scope.TRTOB_PickUpLocation = "";
            $scope.TRTOB_TripFromDate = "";
            $scope.TRTOB_TripToDate = "";
            $scope.TRTOB_FromTime = "";
            $scope.TRTOB_ToTime = "";
            $scope.TRTOB_BookingAmount = "";
            $scope.TRTOB_ModeOfPayment = "";
            $scope.TRTPP_ChequeDDNo = "";
            $scope.TRTPP_ChequeDDDate = "";
            $scope.TRTOB_TripPurpose = "";
            $scope.TRTOB_NoOfTravellers = 0;
            $scope.TRTOB_DropLocation = '';
        }
       
    }
})();
