(function () {
    'use strict';

    angular
        .module('app')
        .controller('Trip', Trip);

    Trip.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache'];

    function Trip($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache) {
        $scope.myTabIndex = 0;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.reqSelection = true;
        $scope.disableDriverallottment = false;
        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.options = {
            hstep: [1, 2, 3],
            mstep: [1, 5, 10, 15, 25, 30]
        };
        var editvalue = '';
        $scope.edit = function (user) {
            $scope.tdshow = true;
            $scope.disableDriverallottment = false;
            editvalue = 'edit';
        }

        $scope.deletetrip = function (user) {
            var mgs = "Delete";
            var confirmmgs = "Deleted";
           
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

                        var config = {
                            headers: {
                                'Content-Type': 'application/json;'
                            }
                        }

                        apiService.create("Trip/deletetrip", user).
                            then(function (promise) {
                              
                                if (promise.returnVal =='success') {
                                        swal("Record " + confirmmgs + " " + "successfully");
                                    }
                                    else {
                                        
                                        swal("Record " + mgs + " Failed");
                                    }
                              
                              
                        
                                $state.reload();
                               
                            })
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };


        $scope.TRTP_BookingDate1 = new Date();
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.sort1 = function (key1) {
            $scope.sortReverse1 = ($scope.sortKey1 == key1) ? !$scope.sortReverse1 : $scope.sortReverse1;
            $scope.sortKey1 = key1;
        }

        $scope.vehicletypelist = [];
        //Add more Vehicle and Driver.
        $scope.vehicles = [{ id: 'vehicle1' }];
        $scope.addNewVehicle = function () {
            debugger;
            var newItemNo = $scope.vehicles.length + 1;
            if (newItemNo <= 500) {
                $scope.vehicles.push({ 'id': 'vehicle' + newItemNo });
            }
        };
        //removing Vehicle and Driver.
        $scope.delm = [];
        $scope.removeVehicle = function (index) {
            var newItemNo = $scope.vehicles.length - 1;
            if (newItemNo !== 0) {
                $scope.delm = $scope.vehicles.splice(index, 1);
            }
        };
        $scope.vehicleListtemp = [];
        $scope.getvahicle = function (typeid,abc) {
            debugger;
            var assignedVehicle = $scope.vehicles;
            var newassgn = [];
            angular.forEach(assignedVehicle, function (qqq) {
                newassgn.push({ id: qqq.id, trmV_Id: qqq.trmV_Id, trvD_Id: qqq.trvD_Id })
            })
            if (newassgn.length > 0) {
                var search = {
                    "TRMVT_Id": typeid,
                    "allottedVehicleDriver": newassgn
                }
            } else {
                var search = {
                    "TRMVT_Id": typeid,
                }
            }
            
            apiService.create("Trip/getvahicle/", search).then(function (promise) {
                $scope.vehicleListtemp = promise.vehicleList;
                //if (promise.driverList.length>0) {
                //    $scope.driverList = promise.driverList;
                //}
               

                angular.forEach($scope.vehicles, function (qwe) {


                    if (qwe.id == abc.id) {
                        angular.forEach($scope.vehicleListtemp, function () {

                        })

                        qwe.vehicleList = $scope.vehicleListtemp;
                    }
                  
                })

                //$scope.disablecheck();
            })

        }


        $scope.loaddata1 = function () {
            debugger;
            apiService.getURI("Trip/loadData1/", 1).then(function (promise) {
                $scope.hirerGroupList1 = promise.hirerGroupList;
                $scope.vehicleList = promise.vehicleList;
                console.log($scope.vehicleList);
                $scope.driverList = promise.driverList;
                $scope.tripDrpwn = promise.tripDrpwn;
                $scope.hirerDrpDwn = promise.hirerDrpDwn;
                $scope.modeOfPaymentList = promise.modeOfPaymentList;
                $scope.bookingIdDrpDwn = promise.bookingIdDrpDwn;
                $scope.vehicletypelist = promise.vehicletypelist;
            });
        }
        $scope.modeofPayment1 = function (val) {
            $scope.reqSelection = false;
            $scope.PaymentMode = val;
            if (val == 'Cheque' || val == 'DD' || val == 'Bank') {
                $scope.Cheque = true;
            }
            else {
                $scope.Cheque = false;
            }
        }

        $scope.tdshow = true;

        $scope.disablecheck = function () {
            debugger;
            angular.forEach($scope.vehicles, function (ss) {

                angular.forEach(ss.vehicleList, function (mm) {

                    if (mm.trmV_Id == ss.trmV_Id) {
                        mm.dis = 'dis';
                    }
                    //else {
                    //    mm.dis = '';
                    //}

                })

            })

        }
        $scope.checkdriverdis = function (id,xyz) {
            debugger;
            if (id != undefined && xyz !=undefined) {
                angular.forEach($scope.vehicles, function (qwe) {


                    if (qwe.id == xyz.id) {
                        angular.forEach($scope.driverList, function (ff) {
                            if (id == ff.trmD_Id) {
                                qwe.trmD_MobileNo = ff.trmD_MobileNo;
                            }

                        })


                    }

                })
            }
        

            angular.forEach($scope.vehicles, function (ss) {

                angular.forEach($scope.driverList, function (mm) {

                    if (ss.trvD_Id == mm.trmD_Id || mm.drdis == 'dis') {
                        mm.drdis = 'dis';
                    }
                    else  {
                        mm.drdis = '';
                    }

                })

            })

        }



        $scope.Search = function () {


            angular.forEach($scope.driverList, function (mm) {

                mm.drdis = '';

            })

            $scope.TRTP_Id = 0;
            $scope.vehicles = [{ id: 'vehicle1' }];
            editvalue = '';
            $scope.tripList = [];
            debugger;
            var search = {
                "TRTOB_BookingId": $scope.TRTOB_BookingId1
            }
            apiService.create("Trip/Search/", search).then(function (promise) {
                if (promise.count > 0) {
                    debugger;

                 

                    $scope.tripOnlineBookingDetails=promise.tripOnlineBookingDetails;
                    $scope.TRTP_BookingDate1 =new Date(promise.tripOnlineBookingDetails[0].trtoB_BookingDate);
                    $scope.TRTP_TripDate1 =new Date(promise.tripOnlineBookingDetails[0].trtoB_Date);
                    $scope.TRHG_Id1 = promise.tripOnlineBookingDetails[0].trhG_Id;
                    $scope.TRTP_HirerName1 = promise.tripOnlineBookingDetails[0].trtoB_HirerName;
                    $scope.TRTP_HirerContactPerson1 = promise.tripOnlineBookingDetails[0].trtoB_ConatctPerName;
                    $scope.TRTP_HirerContactNo1 = promise.tripOnlineBookingDetails[0].trtoB_ContactNo;
                    $scope.TRTP_HirerAddress1 = promise.tripOnlineBookingDetails[0].trtoB_Address;
                    $scope.TRTP_TripAddress1 = promise.tripOnlineBookingDetails[0].trtoB_TripAddress;
                    $scope.TRTP_PickUpLocation1 = promise.tripOnlineBookingDetails[0].trtoB_PickUpLocation;
                    $scope.TRTP_DropLocation1 = promise.tripOnlineBookingDetails[0].trtoB_DropLocation;
                    
                    $scope.TRTP_TripFromDate1 =new Date(promise.tripOnlineBookingDetails[0].trtoB_TripFromDate);
                    $scope.TRTP_TripToDate1 = new Date(promise.tripOnlineBookingDetails[0].trtoB_TripToDate);
                    $scope.TRTP_NoOfTravellers = promise.tripOnlineBookingDetails[0].trtoB_NoOfTravellers;

                    if (promise.tripOnlineBookingDetails[0].trtoB_FromTime != null) {
                        $scope.TRTP_FromTime1 = moment(promise.tripOnlineBookingDetails[0].trtoB_FromTime, 'h:mm a').format();
                    }
                    if (promise.tripOnlineBookingDetails[0].trtoB_ToTime != null) {
                        $scope.TRTP_ToTime1 = moment(promise.tripOnlineBookingDetails[0].trtoB_ToTime, 'h:mm a').format();
                    }
                   // $scope.TRTP_FromTime1=moment(promise.tripOnlineBookingDetails[0].trtoB_FromTime, 'h:mm a').format();
                   // $scope.TRTP_ToTime1 = moment(promise.tripOnlineBookingDetails[0].trtoB_ToTime, 'h:mm a').format();
                    $scope.TRTOB_Id= promise.tripOnlineBookingDetails[0].trtoB_Id;
                    $scope.trtoB_BookingAmount = $scope.tripOnlineBookingDetails[0].trtoB_BookingAmount;
                    $scope.TRTOB_MobileNo1=promise.tripOnlineBookingDetails[0].trtoB_MobileNo;
                    $scope.TRTOB_EmailId1 = promise.tripOnlineBookingDetails[0].trtoB_EmailId;
                    if (promise.vehicleDriverAllottmentList != null) {
                        debugger;
                        $scope.vehicles = promise.vehicleDriverAllottmentList;

                      // $scope.disablecheck();
                        $scope.checkdriverdis();
                        console.log($scope.vehicles)
                        $scope.disableDriverallottment = true;
                        $scope.TRTP_Id = promise.trtP_Id;
                        $scope.tdshow = false;

                        angular.forEach($scope.vehicles, function
                            (gg)
                        {
                            

                            gg.vehicleList = $scope.vehicleList;
                        })

                        angular.forEach($scope.vehicles, function
                            (gg) {
                            angular.forEach($scope.vehicleList, function
                                (xx) {

                                if (gg.trmV_Id == xx.trmV_Id) {
                                    gg.trmvT_Id = xx.trmvT_Id
                                }

                            })
                            
                        })
                        console.log($scope.vehicles)
                    }
                    else {
                        $scope.disableDriverallottment = false;
                        $scope.tdshow = true;
                    }

                    if(promise.tripList!=null){
                        $scope.tripList = promise.tripList;
                        $scope.TRTP_BillGeneratedFlag = false;
                        if ($scope.tripList.length > 0) {
                            $scope.TRTP_BillGeneratedFlag = promise.tripList[0].trtP_BillGeneratedFlag;
                        }
                       
                       
                    } else {
                        $scope.TRTP_BillGeneratedFlag = false;
                    }
                  
                    //alert($scope.TRTP_BillGeneratedFlag);
                }
                else {
                    $scope.tripOnlineBookingDetails = "";
                    $scope.vehicles = "";
                    swal("No Records Found");
                }
            });
        }


        $scope.printbill = function (billdata) {
           
            $scope.printbilldata = [];

            var data = {
                "TRTP_Id": billdata.trtP_Id,
                "TRTOB_Id": billdata.trtoB_Id
            }
            
            apiService.create("Trip/printbill/", data).then(function (promise) {
                $scope.printbilldata = promise.printbilldata;
                $scope.instname = promise.instname;
                if (promise.instname != null) {
                    $scope.inst_name = $scope.instname[0].mI_Name;
                    $scope.add = $scope.instname[0].mI_Address1;
                    $scope.add1 = $scope.instname[0].mI_Address2;
                    $scope.city = $scope.instname[0].ivrmmcT_Name;
                    $scope.pin = $scope.instname[0].mI_Pincode;
                }

                if (promise.printbilldata != null) {
                    console.log($scope.printbilldata)
                    $scope.recno = $scope.printbilldata[0].TRTP_BillNo;
                    $scope.recnodate = $scope.printbilldata[0].TRTP_BillDate;
                    $scope.hirename = $scope.printbilldata[0].TRTP_HirerName;
                    $scope.openingtime = $scope.printbilldata[0].TRTP_FromTime;
                    $scope.bookeddate = $scope.printbilldata[0].TRTP_BookingDate;
                    $scope.reqdate = $scope.printbilldata[0].TRTOB_TripFromDate;
                    $scope.triploc = $scope.printbilldata[0].TRTP_TripAddress;
                    $scope.trippurpose = $scope.printbilldata[0].TRTOB_TripPurpose;
                    $scope.reqdateto = $scope.printbilldata[0].TRTOB_TripToDate;
                    

                    $scope.temparray = [];
                    $scope.ttarray = [];
                    var amt = 0;
                    var opentotal = 0;
                    var closetotal = 0;
                    var nettotal = 0;
                    var ratetotal = 0;
                    var amttotal = 0;
                    var ttlbillamt = 0;
                    angular.forEach($scope.printbilldata, function (bb) {
                        if (amt == 0) {
                            $scope.temparray.push({ TRMV_VehicleNo: bb.TRMV_VehicleNo, TRTP_OpeningKM: bb.TRTP_OpeningKM, TRTP_ClosingKM: bb.TRTP_ClosingKM, NET_KM: bb.NET_KM, TRHR_RatePerKM: bb.TRHR_RatePerKM, TRTPP_PaidAmount: bb.TRTP_BillAmount, billamount: (bb.NET_KM * bb.TRHR_RatePerKM) })
                        }
                        else {
                            $scope.temparray.push({ TRMV_VehicleNo: bb.TRMV_VehicleNo, TRTP_OpeningKM: bb.TRTP_OpeningKM, TRTP_ClosingKM: bb.TRTP_ClosingKM, NET_KM: bb.NET_KM, TRHR_RatePerKM: bb.TRHR_RatePerKM, TRTPP_PaidAmount: '', billamount: (bb.NET_KM * bb.TRHR_RatePerKM)  })
                        }

                        amt += 1;
                        opentotal += bb.TRTP_OpeningKM
                        closetotal += bb.TRTP_ClosingKM
                        nettotal += bb.NET_KM
                        ratetotal += bb.TRHR_RatePerKM
                        amttotal = bb.TRTP_BillAmount
                        ttlbillamt = bb.TRTP_BillAmount

                    })
                    $scope.ttarray.push({ opentotal: opentotal, closetotal: closetotal, nettotal: nettotal, ratetotal: ratetotal, amttotal: amttotal, ttlbillamt: ttlbillamt })
                    console.log($scope.array)
                    $scope.amtinwords = convertNumberToWords(amttotal)
                }

            });
        }

        $scope.rprint1 = function () {

            var innerContents = document.getElementById("printareaId1").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
                '<link type="text/css" media="print" href="css/print/trnRecieptprint.css" rel="stylesheet" />' +
                '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
            );
            popupWinindow.document.close();

        }

        $scope.SearchByTripId = function () {
            editvalue = '';
            $scope.databyTripId = [];
            var search = {
                "TRVD_TripId": $scope.TRVD_TripId,
                "SearchBy": "Trip"
            }
            apiService.create("Trip/SearchByTripId/", search).then(function (promise) {
                if (promise.tripList != null) {
                    if ($scope.searchradio == 'trip') {
                        $scope.genrecSearchResult = promise.tripList;
                       
                    }
                    else {
                        $scope.databyTripId = promise.tripList;
                    }
                    $scope.TRTP_BookingDate2 = new Date(promise.tripList[0].trtP_BookingDate);
                    $scope.TRTP_TripDate2 = new Date(promise.tripList[0].trtP_TripDate);
                    $scope.TRTP_HirerName2 = promise.tripList[0].trtP_HirerName;
                    $scope.TRTP_HirerContactNo2 = promise.tripList[0].trtP_HirerContactNo;
                    $scope.TRTP_TripAddress2 = promise.tripList[0].trtP_TripAddress;
                    $scope.TRTP_TripFromDate2 = new Date(promise.tripList[0].trtP_TripFromDate);
                    $scope.TRTP_TripToDate2 = new Date(promise.tripList[0].trtP_TripToDate);
                    $scope.TRTP_Id = promise.trtP_Id;
                    if (promise.vehicleDriverAllottmentList != null) {
                        $scope.vehicles = promise.vehicleDriverAllottmentList;
                        console.log($scope.vehicles)
                        $scope.disableDriverallottment = true;
                    }
                    
                       // $scope.TRTP_Id = promise.tripList[0].trtP_Id;
                      //  $scope.TRMV_Id2 = promise.tripList[0].trmV_Id;
                       // $scope.TRVD_Id2 = promise.tripList[0].trvD_Id;
                       // $scope.TRTP_OpeningKM2 = promise.tripList[0].trtP_OpeningKM;
                       
                        $scope.TRTP_BillGeneratedFlag = promise.tripList[0].trtP_BillGeneratedFlag;
                      //  $scope.id = promise.tripList[0].trvD_TripId;
                }
                else {
                    swal("No Records Found");
                }
            });
        }
      
        $scope.submitted = false;
        $scope.savedata = function () {
            debugger;
            if ($scope.myForm.$valid) {
                debugger;
                var startTime = $filter('date')($scope.TRTP_FromTime1, "h:mm a");
                var endTime = $filter('date')($scope.TRTP_ToTime1, "h:mm a");
               
                var assignedVehicle = $scope.vehicles;
                var newassgn = [];
                angular.forEach(assignedVehicle, function (qqq) {
                    newassgn.push({ id: qqq.id, trmV_Id: qqq.trmV_Id, trvD_Id: qqq.trvD_Id, trmD_MobileNo: qqq.trmD_MobileNo })
                })

                var obj = {
                    "TRTP_BookingDate": new Date($scope.TRTP_BookingDate1).toDateString(),
                    "TRTP_TripDate":new Date($scope.TRTP_TripDate1).toDateString(),
                    "TRHG_Id": $scope.TRHG_Id1,
                    "TRTP_HirerName": $scope.TRTP_HirerName1,
                    "TRTP_HirerContactPerson": $scope.TRTP_HirerContactPerson1,
                    "TRTP_HirerContactNo": $scope.TRTP_HirerContactNo1,
                    "TRTP_HirerAddress": $scope.TRTP_HirerAddress1,
                    "TRTP_TripAddress": $scope.TRTP_TripAddress1,
                    "TRTP_PickUpLocation": $scope.TRTP_PickUpLocation1,
                    "TRTP_TripFromDate":new Date($scope.TRTP_TripFromDate1).toDateString(),
                    "TRTP_TripToDate":new Date($scope.TRTP_TripToDate1).toDateString(),
                    "TRTP_FromTime": startTime,
                    "TRTP_ToTime": endTime,
                    "TRMV_Id": $scope.TRMV_Id,
                    "TRVD_Id": $scope.TRVD_Id,
                    "TRTP_OpeningKM": $scope.TRTP_OpeningKM,
                    "TRTOB_Id": $scope.tripOnlineBookingDetails[0].trtoB_Id,
                    "TRTP_Id": $scope.TRTP_Id,
                    "TRTP_AdvancePaid": $scope.tripOnlineBookingDetails[0].trtoB_BookingAmount,
                    "TRTP_AdvanceReceived": $scope.TRTP_AdvanceReceived,
                    "TRTP_ModeOfPayment": $scope.PaymentMode,
                    "TRTPP_ChequeDDNo":$scope.TRTPP_ChequeDDNo,
                    "TRTPP_ChequeDDDate": $scope.TRTPP_ChequeDDDate,
                    "TRMH_MobileNo": $scope.TRTOB_MobileNo1,
                    "TRMH_EmailId": $scope.TRTOB_EmailId1,
                    "allottedVehicleDriver": newassgn,
                    "BtnClickVal": editvalue,
                    "TRTP_NoOfTravellers": $scope.TRTP_NoOfTravellers,
                    "TRTP_DropLocation": $scope.TRTP_DropLocation1,
                }
                apiService.create("Trip/save", obj).then(function (promise) {


                    if (promise.returnVal == 'saved') {
                        swal("Record Saved Successfully");
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
                    else if (promise.returnVal == "Nomapping") {
                        swal("Please Create Trip Mapping in Transaction Numbering Page");
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
        $scope.cancel = function () {
            $state.reload();
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.cancelupdate = function () {
            $state.reload();
        }
        $scope.submitted1 = false;
        $scope.updateTrip = function () {
            if ($scope.myForm1.$valid) {
                debugger;
                var assignedVehicle = $scope.vehicles;
                var cnt = 0;
                angular.forEach($scope.vehicles, function (mm) {
                    if (Number(mm.trtP_OpeningKM) > Number(mm.trtP_ClosingKM)) {
                        cnt += 1;
                    }
                })
                if (cnt >0) {
                    swal('Closing K.M is Must Be Equal Or Greater Than Opening  K.M')
                } else {
                    var data = {
                        "TRTP_ClosingKM": $scope.TRTP_ClosingKM,
                        "TRTP_Id": $scope.TRTP_Id,
                        "BtnClickVal": "Update",
                        "allottedVehicleDriver": assignedVehicle,
                        "TRTP_BookingDate": new Date($scope.TRTP_BookingDate2).toDateString(),
                        "TRTP_TripFromDate": new Date($scope.TRTP_TripFromDate2).toDateString(),
                        "TRTP_TripToDate": new Date($scope.TRTP_TripToDate2).toDateString(),
                    }
                    apiService.create("Trip/save", data).then(function (promise) {

                        if (promise.returnVal == 'updated') {
                            swal("Record Updated Successfully");
                            $state.reload();
                        }
                        else if (promise.returnVal == 'duplicate') {
                            swal("Record already exist");
                        }
                        else if (promise.returnVal == "failedUpdate") {
                            swal("Failed to update record");
                        }
                        else {
                            swal("Sorry...something went wrong");
                        }
                        $scope.cancel();
                    });
                }
               
                
            }
            else {
                $scope.submitted1 = true;
            }
        }
        $scope.submitted2 = false;
        $scope.update = function () {
            if ($scope.myForm2.$valid) {
                $('#myModal').modal('hide');
                $scope.TRTP_DiscountAmount = 0;
                var assignedVehicle = $scope.vehicles;
                var obj = {
                    "TRTP_ClosingKM": $scope.TRTP_ClosingKM,
                    "TRTP_Id": $scope.TRTP_Id,
                    "TRTP_BillNo": $scope.TRTP_BillNo,
                    "TRTP_BillDate":new Date($scope.TRTP_BillDate).toDateString(),
                    "TRTP_BillAmount": $scope.TRTP_BillAmount,
                    "TRTP_DiscountAmount": $scope.TRTP_DiscountAmount,
                    "BtnClickVal": "GenerateBill",
                    "allottedVehicleDriver": assignedVehicle,
                    "TRTP_BookingDate": new Date($scope.TRTP_BookingDate2).toDateString(),
                    "TRTP_TripFromDate": new Date($scope.TRTP_TripFromDate2).toDateString(),
                    "TRTP_TripToDate": new Date($scope.TRTP_TripToDate2).toDateString(),
                }
                apiService.create("Trip/save", obj).then(function (promise) {

                  
                   // $('#myModal').modal('toggle');
                    if (promise.returnVal == 'saved') {
                        swal("Record Saved Successfully");
                        $state.reload();
                    }
                    else if (promise.returnVal == 'updated') {
                       
                        //$('#myModal').modal('hide');
                        swal("Record Updated Successfully");
                     
                        //$('#myModal').modal('hide');
                      
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
                      //  $('#myModal').modal('hide');
                       $('#myModal').modal('toggle');
                    }
                    else {
                        swal("Sorry...something went wrong");
                       // $('#myModal').modal('hide');
                        $('#myModal').modal('toggle');
                    }
                    //$state.reload();
                    //$scope.cancel();
                });
            }
            else {
                $scope.submitted2 = true;
            }
        }
        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };
        $scope.interacted2 = function (field) {
            return $scope.submitted2;
        };
        $scope.interacted3 = function (field) {
            return $scope.submitted3;
        };
        $scope.submitted1 = false;
        $scope.bill = function () {
            debugger;
            if ($scope.myForm1.$valid) {
                var data = {
                    "TRTP_Id": $scope.TRTP_Id,
                    "TRTP_ClosingKM": $scope.TRTP_ClosingKM,
                    "allottedVehicleDriver": $scope.vehicles,
                }
                apiService.create("Trip/getbillNo", data).then(function (promise) {
                    if (promise.returnVal == 'Nomapping') {
                        swal("Please Do Transaction Numbering Mapping For TripBill");
                    }
                    else if (promise.returnVal =='rate') {
                        swal("Enter Hirer Rate for vahicle type and Group");
                    } 
                    else {
                        debugger;
                        $scope.TRTP_BillAmount = promise.trtP_BillAmount;
                        $scope.TRTP_BillNo = promise.trtP_BillNo;
                        $scope.id = promise.trvD_TripId;
                        $('#myModal').modal('show');
                    }
                   
                });
            }
            else {
                $scope.submitted1 = true;
                $('#myModal').modal('hide');
            }
        }
        $scope.removeall = function () {
            $('#myModal').modal('hide');
        }
        $scope.SearchByHirer = function () {
            var search = {
                "TRMH_Id": $scope.Receipt.TRMH_Id,
                "SearchBy": "Hirer"
            }
            apiService.create("Trip/SearchByTripId/", search).then(function (promise) {
                $scope.hirerList = promise.hirerList;
            })
        }
        //praveen
        $scope.rprint = function () {

            var innerContents = document.getElementById("printareaId").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
           '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
       '<link type="text/css" media="print" rel="stylesheet" href="css/print/printpdf.css" />' +
        '<link type="text/css" media="print" href="css/print/trntripsheetprint.css" rel="stylesheet" />' +
       '</head> <body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>'
       );
            popupWinindow.document.close();

        }


        function convertNumberToWords(amount) {
            debugger;
            if (amount == 0) {
                words_string = 'ZERO';
            } else {

                var words = new Array();
                words[0] = '';
                words[1] = 'One';
                words[2] = 'Two';
                words[3] = 'Three';
                words[4] = 'Four';
                words[5] = 'Five';
                words[6] = 'Six';
                words[7] = 'Seven';
                words[8] = 'Eight';
                words[9] = 'Nine';
                words[10] = 'Ten';
                words[11] = 'Eleven';
                words[12] = 'Twelve';
                words[13] = 'Thirteen';
                words[14] = 'Fourteen';
                words[15] = 'Fifteen';
                words[16] = 'Sixteen';
                words[17] = 'Seventeen';
                words[18] = 'Eighteen';
                words[19] = 'Nineteen';
                words[20] = 'Twenty';
                words[30] = 'Thirty';
                words[40] = 'Forty';
                words[50] = 'Fifty';
                words[60] = 'Sixty';
                words[70] = 'Seventy';
                words[80] = 'Eighty';
                words[90] = 'Ninety';
                amount = amount.toString();
                var atemp = amount.split(".");
                var number = atemp[0].split(",").join("");
                var n_length = number.length;
                var words_string = "";
                if (n_length <= 9) {
                    var n_array = new Array(0, 0, 0, 0, 0, 0, 0, 0, 0);
                    var received_n_array = new Array();
                    for (var i = 0; i < n_length; i++) {
                        received_n_array[i] = number.substr(i, 1);
                    }
                    for (var i = 9 - n_length, j = 0; i < 9; i++ , j++) {
                        n_array[i] = received_n_array[j];
                    }
                    for (var i = 0, j = 1; i < 9; i++ , j++) {
                        if (i == 0 || i == 2 || i == 4 || i == 7) {
                            if (n_array[i] == 1) {
                                n_array[j] = 10 + parseInt(n_array[j]);
                                n_array[i] = 0;
                            }
                        }
                    }
                    var value = "";
                    for (var i = 0; i < 9; i++) {
                        if (i == 0 || i == 2 || i == 4 || i == 7) {
                            value = n_array[i] * 10;
                        } else {
                            value = n_array[i];
                        }
                        if (value != 0) {
                            words_string += words[value] + " ";
                        }
                        if ((i == 1 && value != 0) || (i == 0 && value != 0 && n_array[i + 1] == 0)) {
                            words_string += "Crores ";
                        }
                        if ((i == 3 && value != 0) || (i == 2 && value != 0 && n_array[i + 1] == 0)) {
                            words_string += "Lakhs ";
                        }
                        if ((i == 5 && value != 0) || (i == 4 && value != 0 && n_array[i + 1] == 0)) {
                            words_string += "Thousand ";
                        }
                        if (i == 6 && value != 0 && (n_array[i + 1] != 0 && n_array[i + 2] != 0)) {
                            words_string += "Hundred and ";
                        } else if (i == 6 && value != 0) {
                            words_string += "Hundred ";
                        }
                    }
                    words_string = words_string.split("  ").join(" ");
                }

            }

            return words_string;
        }
        $scope.viewtripsheetprint = function (user) {

            var data = {
                "TRTP_Id": user.trtP_Id
            }
            console.log(data)
            apiService.create("Trip/printtripsheet/", data).then(function (promise) {
                $scope.tripsheetprint = promise.tripsheetprint;

                console.log($scope.tripsheetprint)
                $scope.instname = promise.instname;
                if (promise.instname != null) {
                    $scope.inst_name = $scope.instname[0].mI_Name;
                    $scope.add = $scope.instname[0].mI_Address1;
                    $scope.add1 = $scope.instname[0].mI_Address2;
                    $scope.city = $scope.instname[0].ivrmmcT_Name;
                    $scope.pin = $scope.instname[0].mI_Pincode;
                }

            });
        }
    }
})();
