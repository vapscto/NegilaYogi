(function () {
    'use strict';

    angular
        .module('app')
        .controller('GenerateReceipt', GenerateReceipt);

    GenerateReceipt.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache'];

    function GenerateReceipt($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache) {
        $scope.searchBill = '';
        $scope.reqSelection = true;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.loaddata = function () {
            apiService.getURI("Trip/loadData1/", 1).then(function (promise) {
                $scope.hirerGroupList2 = promise.hirerGroupList;
                $scope.vehicleList = promise.vehicleList;
                $scope.driverList = promise.driverList;
                $scope.tripDrpwn2 = promise.tripDrpwn1;
                $scope.hirerDrpDwn2 = promise.hirerDrpDwn;
                $scope.modeOfPaymentList2 = promise.modeOfPaymentList;
            });
        }
        $scope.onchangeradiobtn = function () {
            $scope.genrecSearchResult = "";
            $scope.TRVD_TripId1 = "";
            $scope.trmH_HirerName = "";
            $scope.hirerTripList = "";
            $scope.TRTPP_PaymentMode = "";
            $scope.TRTP_DiscountAmount = 0;
            if ($scope.searchradio == 'hirer') {
                $scope.TRTPP_PaidAmount = 0;
            }
        }

        $scope.searchValue = ''; 
        $scope.modeofPayment1 = function (val) {
            $scope.reqSelection = false;
            $scope.PaymentMode = val;
            if (val == 'Cheque' || val == 'DD' || val == 'Bank' || val == 'Card' || val =='RTGS/NEFT') {
                $scope.Cheque = true;
            }
            else {
                $scope.Cheque = false;
            }
        }
        $scope.vehicles = [{ id: 'vehicle1' }];

        $scope.recdisable = true;


        $scope.duprecpcheck = function () {
            $scope.recduparray = [];
            var data = {
                "TRTPP_ReceiptNo": $scope.TRTPP_ReceiptNo,
            }
            apiService.create("Trip/duprecpcheck/", data).then(function (promise) {

                if (promise.recduparray.length > 0) {
                    swal("Duplicate Receipt Number");
                    $scope.TRTPP_ReceiptNo ='';
                }
            })
       

        }


     

        $scope.SearchByTripId1 = function () {
            $scope.recdisable = true;
            $scope.searchValue = ''; 
            $scope.TRTP_DiscountAmount = 0;
            $scope.TRTPP_PaidAmount = '';
            $scope.generatedReceiptsList = [];
           $scope.presentCountgrid = 0;
            var search = {
                "TRVD_TripId": $scope.TRVD_TripId1,
                "SearchBy": "Trip"
            }
            apiService.create("Trip/SearchByTripId/", search).then(function (promise) {

                if (promise.tripList != null) {
                    if (promise.returnVal == 'noMapping') {
                        swal("Please Mapp Receipt Numbering in Transaction Numbering Page");
                    }
                    else {
                        $scope.genrecSearchResult = promise.tripList;
                        $scope.PickUpLocation = promise.tripList[0].trtP_PickUpLocation;
                     //   $scope.vehicleName = promise.tripList[0].vehicleName;
                     //   $scope.driverName = promise.tripList[0].driverName;
                        $scope.TRTP_BookingDate = new Date(promise.tripList[0].trtP_BookingDate);
                        $scope.TRTP_TripDate = new Date(promise.tripList[0].trtP_TripDate);
                        $scope.TRTP_HirerName = promise.tripList[0].trtP_HirerName;
                        $scope.TRTP_HirerContactNo = promise.tripList[0].trtP_HirerContactNo;
                        $scope.TRTP_TripAddress = promise.tripList[0].trtP_TripAddress;
                        $scope.TRTP_TripFromDate = new Date(promise.tripList[0].trtP_TripFromDate);
                        $scope.TRTP_TripToDate = new Date(promise.tripList[0].trtP_TripToDate);
                        $scope.TRTP_Id = promise.tripList[0].trtP_Id;
                      //  $scope.TRMV_Id = promise.tripList[0].trmV_Id;
                      //  $scope.TRVD_Id = promise.tripList[0].trvD_Id;
                        if (promise.vehicleDriverAllottmentList != null) {
                            debugger;
                            $scope.vehicles = promise.vehicleDriverAllottmentList;
                            $scope.disableDriverallottment = true;
                            $scope.TRTP_Id = promise.trtP_Id;
                        }
                        else {
                            $scope.disableDriverallottment = false;
                        }

                       // $scope.TRTP_OpeningKM = promise.tripList[0].trtP_OpeningKM;
                       // $scope.TRTP_ClosingKM = promise.tripList[0].trtP_ClosingKM;
                        $scope.TRTP_BillGeneratedFlag = promise.tripList[0].trtP_BillGeneratedFlag;
                        debugger;
                        $scope.TRTP_PaidAmount = promise.tripList[0].trtP_PaidAmount;
                        $scope.TRTP_BalanceAmount = promise.tripList[0].trtP_BalanceAmount;
                        $scope.TRTP_TotalReceivable = promise.tripList[0].trtP_TotalReceivable;
                        $scope.TRTP_discAmount11 = promise.tripList[0].trtP_DiscountAmount;
                        $scope.TRTP_billamt11 = promise.tripList[0].trtP_BillAmount;
                        $scope.TRTPP_ReceiptNo = promise.trtpP_ReceiptNo;
                        //alert($scope.TRTP_BalanceAmount);
                        if ($scope.TRTPP_ReceiptNo == '') {
                            $scope.recdisable = false;
                        }
                    }
                }
                else {
                    swal("No Records Found");
                }
                if (promise.generatedReceiptsList!=null) {
                    $scope.generatedReceiptsList = promise.generatedReceiptsList;
                    $scope.presentCountgrid = $scope.generatedReceiptsList.length;
                }
                else {
                   // swal("No Reciept Found");
                }
            });
        }
        $scope.temptotal
        $scope.tripdiscchange = function () {
            debugger;
            $scope.TRTPP_PaidAmount = $scope.TRTP_TotalReceivable - Number($scope.TRTP_DiscountAmount);
            $scope.temptotal = $scope.TRTPP_PaidAmount + Number($scope.TRTP_DiscountAmount)
            if ($scope.TRTP_TotalReceivable < ($scope.TRTP_DiscountAmount)) {
                swal('Payable amount is Greater than the Bill amount');
                $scope.TRTPP_PaidAmount = 0;
                $scope.TRTP_DiscountAmount = 0;
            }
        }
        $scope.mainamtchange = function () {
            debugger;
            $scope.TRTPP_PaidAmount = $scope.TRTP_TotalReceivable - Number($scope.TRTP_DiscountAmount);
            
            if ($scope.TRTP_TotalReceivable < ($scope.TRTPP_PaidAmount)) {
                swal('Payable amount is Greater than the Bill amount');
                $scope.TRTPP_PaidAmount = 0;
                $scope.TRTP_DiscountAmount = 0;
            }
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.cancel = function () {
            $state.reload();
        }
        $scope.SearchByHirer = function () {
            $scope.TRMH_MobileNo = '';
            $scope.TRMH_EmailId = '';
            $scope.hirerTripList = [];
            var search = {
                "TRTP_HirerName": $scope.trmH_HirerName,
                "SearchBy": "Hirer"
            }
            apiService.create("Trip/SearchByTripId/", search).then(function (promise) {
                debugger;
                if (promise.hirerList != null) {
                    if (promise.returnVal == 'noMapping') {
                        swal("Please Mapp Receipt Numbering in Transaction Numbering Page");
                    }
                    else {
                        $scope.hirerTripList = promise.hirerList;
                        $scope.PickUpLocation = promise.hirerList[0].trtP_PickUpLocation;
                        $scope.vehicleName = promise.hirerList[0].vehicleName;
                        $scope.driverName = promise.hirerList[0].driverName;
                        $scope.TRTP_BookingDate = new Date(promise.hirerList[0].trtP_BookingDate);
                        $scope.TRTP_TripDate = new Date(promise.hirerList[0].trtP_TripDate);
                        $scope.TRTP_HirerName = promise.hirerList[0].trtP_HirerName;
                        $scope.TRTP_HirerContactNo = promise.hirerList[0].trtP_HirerContactNo;
                        $scope.TRTP_TripAddress = promise.hirerList[0].trtP_TripAddress;
                        $scope.TRTP_TripFromDate = new Date(promise.hirerList[0].trtP_TripFromDate);
                        $scope.TRTP_TripToDate = new Date(promise.hirerList[0].trtP_TripToDate);
                        $scope.TRTP_Id = promise.hirerList[0].trtP_Id;
                        $scope.TRTP_BillGeneratedFlag = promise.hirerList[0].trtP_BillGeneratedFlag;
                        $scope.TRTPP_ReceiptNo = promise.trtpP_ReceiptNo;
                        $scope.TRMH_MobileNo = promise.hirerList[0].trmH_MobileNo;
                        $scope.TRMH_EmailId = promise.hirerList[0].trmH_EmailId;
                        $scope.TRMH_Id = promise.hirerList[0].trmH_Id;
                        $scope.TRTP_PaidAmount = promise.hirerList[0].trtP_PaidAmount;
                        $scope.TRTP_BalanceAmount = promise.hirerList[0].trtP_BalanceAmount;
                        $scope.TRTP_TotalReceivable = promise.hirerList[0].trtP_TotalReceivable;

                        angular.forEach($scope.hirerTripList, function (ww) {
                            
                            ww.TRTP_DiscountAmount = 0;
                            ww.fsC_ConcessionType ='';
                        })

                    }
                }
                else {
                    swal("No Records Found");
                }
                if (promise.generatedReceiptsList!=null) {
                    $scope.generatedReceiptsList = promise.generatedReceiptsList;
                    $scope.presentCountgrid = $scope.generatedReceiptsList.length;
                }
                //else {
                //    swal("No Records Found");
                //}
            })
        }


        $scope.changeper = function (rr) {
            debugger;
            angular.forEach($scope.hirerTripList, function (ww) {
                if (rr.trtP_Id == ww.trtP_Id) {
                    rr.TRTP_DiscountAmount = (parseFloat(rr.TRTP_DiscountAmountpe) / 100) * parseFloat(rr.trtP_BalanceAmount);
                    rr.TRTP_DiscountAmount = parseFloat(rr.TRTP_DiscountAmount).toFixed(2);;

                    rr.TRTPP_PaidAmount = parseFloat(rr.trtP_TotalReceivable) - (parseFloat(rr.trtP_PaidAmount) + parseFloat(rr.TRTP_DiscountAmount))

                    if (parseFloat(rr.trtP_TotalReceivable) < (parseFloat(rr.trtP_PaidAmount) + parseFloat(rr.TRTP_DiscountAmount))) {
                        swal('Payable amount is Greater than the Bill amount');
                        rr.TRTPP_PaidAmount = 0;
                        rr.TRTP_DiscountAmount = 0;
                    }
                    else {
                        rr.TRTPP_PaidAmount = parseFloat(rr.TRTPP_PaidAmount).toFixed(2);;
                    }
                }

            })

        }
        $scope.typechange = function (rr) {
            debugger;
            angular.forEach($scope.hirerTripList, function (ww) {
                if (rr.trtP_Id == ww.trtP_Id) {
                    rr.TRTP_DiscountAmount = 0;
                    rr.TRTP_DiscountAmountpe = 0;
                    rr.TRTPP_PaidAmount = 0;
                }

            })

        }

        $scope.trippercchange = function () {

            $scope.TRTP_BalanceAmount = parseFloat($scope.TRTP_BalanceAmount);
            if ($scope.TRTP_BalanceAmount <= 0) {
                swal('Amount already Paid')
            } else {
                $scope.TRTP_DiscountAmount = (parseFloat($scope.TRTP_Discounprc) / 100) * parseFloat($scope.TRTP_BalanceAmount);
                $scope.TRTP_DiscountAmount = parseFloat($scope.TRTP_DiscountAmount).toFixed(2);
                $scope.TRTPP_PaidAmount = $scope.TRTP_TotalReceivable - $scope.TRTP_DiscountAmount;
                $scope.TRTPP_PaidAmount = parseFloat($scope.TRTPP_PaidAmount).toFixed(2);
            }
           
        }

        $scope.typemainchange1 = function () {
            $scope.TRTP_Discounprc = 0;
            $scope.TRTP_DiscountAmount = 0;
            $scope.TRTPP_PaidAmount = 0;

        }
        $scope.changedisc = function (rr) {
            debugger;

            angular.forEach($scope.hirerTripList, function (ww) {
                if (rr.trtP_Id == ww.trtP_Id) {
                    rr.TRTPP_PaidAmount = parseFloat(rr.trtP_TotalReceivable) - (parseFloat(rr.trtP_PaidAmount) + parseFloat(rr.TRTP_DiscountAmount))

                    rr.TRTPP_PaidAmount = parseFloat(rr.TRTPP_PaidAmount);
                    rr.TRTP_DiscountAmount = parseFloat(rr.TRTP_DiscountAmount).toFixed(2);;

                    if (parseFloat(rr.trtP_TotalReceivable) < (parseFloat(rr.trtP_PaidAmount) + parseFloat(rr.TRTP_DiscountAmount))) {
                        swal('Payable amount is Greater than the Bill amount');
                        rr.TRTPP_PaidAmount = 0;
                        rr.TRTP_DiscountAmount = 0;
                    }
                }

            })

        }
        


        $scope.changedisc1 = function (rr) {
            debugger;

            angular.forEach($scope.hirerTripList, function (ww) {
                if (rr.trtP_Id == ww.trtP_Id) {
                   /// rr.TRTPP_PaidAmount = rr.trtP_TotalReceivable - (rr.trtP_PaidAmount + rr.TRTP_DiscountAmount)

                    if (parseFloat(rr.trtP_BalanceAmount) < (parseFloat(rr.TRTP_DiscountAmount) + parseFloat(rr.TRTPP_PaidAmount))) {
                        swal('Payable amount is Greater than the Bill amount');
                        rr.TRTPP_PaidAmount = 0;
                        rr.TRTP_DiscountAmount = 0;
                    }
                }

            })

        }

        $scope.submitted = false;
        $scope.selectedBills = [];
        $scope.pay = function () {
            debugger;
            if ($scope.myForm.$valid) {
                if ($scope.searchradio == 'hirer') {
                    $scope.selectedBills.length = 0;
                    for (var i = 0; i < $scope.hirerTripList.length; i++) {
                        if ($scope.hirerTripList[i].checked == true) {
                            $scope.selectedBills.push($scope.hirerTripList[i]);
                        }
                    }
                }
               
                if ($scope.TRTPP_ChequeDDDate != undefined || $scope.TRTPP_ChequeDDDate != null) {
                    $scope.TRTPP_ChequeDDDate = new Date($scope.TRTPP_ChequeDDDate).toDateString();
                }
                else {
                    $scope.TRTPP_ChequeDDDate = null;
                }

                var obj = {
                    "TRTPP_TripAmount": $scope.TRTPP_TripAmount,
                    "TRTPP_PaidAmount": $scope.TRTPP_PaidAmount,
                    "TRTPP_ReceiptNo": $scope.TRTPP_ReceiptNo,
                    "TRTPP_PaymentMode": $scope.PaymentMode,
                    "TRTPP_ReceiptDate": new Date($scope.TRTPP_ReceiptDate).toDateString(),
                    "TRTPP_ChequeDDNo": $scope.TRTPP_ChequeDDNo,
                    "TRTPP_ChequeDDDate":$scope.TRTPP_ChequeDDDate,
                    "TRHOB_DueAmount": $scope.TRHOB_DueAmount,
                    "TRHOB_ExcessAmount": $scope.TRHOB_ExcessAmount,
                    "TRTP_Id": $scope.TRTP_Id,
                    "TRMH_Id": $scope.TRMH_Id,
                    "selectedBills": $scope.selectedBills,
                    "SelectedRadioVal": $scope.searchradio,
                    "TRTP_DiscountAmount": $scope.TRTP_DiscountAmount,
                    "TRTPP_BankName": $scope.TRTPP_BankName,
                    }
                apiService.create("Trip/pay/", obj).then(function (promise) {
                    if (promise.returnVal == 'payed') {
                        swal("Amount Paid Successfully");
                        $state.reload();
                    }
                    else if (promise.returnVal == 'AmountPaid') {
                        swal("Full amount is paid");
                    }
                    else if (promise.returnVal == 'failed') {
                        swal("failed");
                    }
                });


            } else {
                $scope.submitted = true;
            }
        }
        $scope.removeall1 = function () {
            $('#myModal1').modal('hide');
        }
        $scope.viewDetails = function (data) {
            var tripID = data.trtP_Id;
            apiService.getURI("Trip/viewDetails/", tripID).then(function (promise) {
                debugger;
                $scope.viewDet = promise.tripList;
                $scope.PickUpLocationDet = promise.tripList[0].trtP_PickUpLocation;
                $scope.TRTP_BookingDateDet = new Date(promise.tripList[0].trtP_BookingDate);
                $scope.TRTP_TripDateDet = new Date(promise.tripList[0].trtP_TripDate);
                $scope.TRTP_HirerNameDet = promise.tripList[0].trtP_HirerName;
                $scope.TRTP_HirerContactNoDet = promise.tripList[0].trtP_HirerContactNo;
                $scope.TRTP_TripAddressDet = promise.tripList[0].trtP_TripAddress;
                $scope.TRTP_TripFromDateDet = new Date(promise.tripList[0].trtP_TripFromDate);
                $scope.TRTP_TripToDateDet = new Date(promise.tripList[0].trtP_TripToDate);
                $scope.TRTP_Id = promise.tripList[0].trtP_Id;
                $scope.TRTP_BillGeneratedFlagDet = promise.tripList[0].trtP_BillGeneratedFlag;
                $scope.TRTP_PaidAmountDet = promise.tripList[0].trtP_PaidAmount;
                $scope.TRTP_BalanceAmountDet = promise.tripList[0].trtP_BalanceAmount;
                $scope.TRTP_TotalReceivableDet = promise.tripList[0].trtP_TotalReceivable;
                if (promise.vehicleDriverAllottmentList != null) {
                    debugger;
                    $scope.vehicles = promise.vehicleDriverAllottmentList;
                    $scope.disableDriverallottment = true;
                    $scope.TRTP_Id = promise.trtP_Id;
                }
                else {
                    $scope.disableDriverallottment = false;
                }

            });
            $('#myModal1').modal('show');
        }
        $scope.isOptionsRequired = function () {

            return !$scope.hirerTripList.some(function (options) {
                return options.checked;
            });
        }
        $scope.toggleAll = function () {

            if ($scope.radioption == "student") {

                var toggleStatus = $scope.details;
                angular.forEach($scope.studentlist, function (itm) {
                    itm.Selected = toggleStatus;
                    if ($scope.details == true) {
                        $scope.printdatatable.push(itm);
                    }
                    else {
                        $scope.printdatatable.splice(itm);
                    }
                });
            }
            else if ($scope.radioption == "Staff") {
                var toggleStatus = $scope.checkall;
                angular.forEach($scope.staffList, function (itm) {
                    itm.checked = toggleStatus;
                    if ($scope.checkall == true) {
                        $scope.staffPrintList.push(itm);
                    }
                    else {
                        $scope.staffPrintList.splice(itm);
                    }
                });

            }

        }




        $scope.optionToggled = function (SelectedStudentRecord, index) {
            debugger;
                $scope.details = $scope.studentlist.every(function (itm)
                { return itm.checked; });
                if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                    $scope.printdatatable.push(SelectedStudentRecord);
                }
                else {
                    $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
                }
        }

        //Praveen (05/30/2018)
       

        $scope.rprint = function () {

            var innerContents = document.getElementById("printareaId").innerHTML;
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

        $scope.viewprintbilldetails = function (user) {
            $scope.receptprint = [];
            var data = {
                "TRTPP_Id": user.trtpP_Id,
                "TRVD_TripId": $scope.TRVD_TripId1
            }
            console.log(data)
            apiService.create("Trip/printrecept/", data).then(function (promise) {
                $scope.receptprint = promise.receptprint;
                $scope.instname = promise.instname;
                if (promise.instname!=null) {
                    $scope.inst_name = $scope.instname[0].mI_Name;
                    $scope.add = $scope.instname[0].mI_Address1;
                    $scope.add1 = $scope.instname[0].mI_Address2;
                    $scope.city = $scope.instname[0].ivrmmcT_Name;
                    $scope.pin = $scope.instname[0].mI_Pincode;
                }

                if (promise.receptprint!=null) {
                    console.log($scope.receptprint)
                    $scope.recno = $scope.receptprint[0].TRTPP_ReceiptNo;
                    $scope.recnodate = $scope.receptprint[0].TRTPP_ReceiptDate;
                    $scope.hirename = $scope.receptprint[0].TRTP_HirerName;
                    $scope.openingtime = $scope.receptprint[0].TRTP_FromTime;
                    $scope.bookeddate = $scope.receptprint[0].TRTP_BookingDate;
                    $scope.reqdate = $scope.receptprint[0].TRTOB_TripFromDate;
                    $scope.triploc = $scope.receptprint[0].TRTP_TripAddress;
                    $scope.trippurpose = $scope.receptprint[0].TRTOB_TripPurpose;
                    $scope.reqdateto = $scope.receptprint[0].TRTOB_TripToDate;

                    $scope.temparray = [];
                    $scope.ttarray = [];
                    var amt = 0;
                    var opentotal = 0;
                    var closetotal = 0;
                    var nettotal = 0;
                    var ratetotal = 0;
                    var amttotal = 0;
                    var disamttotal = 0;
                    angular.forEach($scope.receptprint, function (bb) {
                        if (amt==0) {
                            $scope.temparray.push({ TRMV_VehicleNo: bb.TRMV_VehicleNo, TRTP_OpeningKM: bb.TRTP_OpeningKM, TRTP_ClosingKM: bb.TRTP_ClosingKM, NET_KM: bb.NET_KM, TRHR_RatePerKM: bb.TRHR_RatePerKM, TRTPP_PaidAmount: bb.TRTPP_PaidAmount, TRMD_DriverName: bb.TRMD_DriverName, TRTP_DiscountAmount: bb.TRTP_DiscountAmount})
                        }
                        else {
                            $scope.temparray.push({ TRMV_VehicleNo: bb.TRMV_VehicleNo, TRTP_OpeningKM: bb.TRTP_OpeningKM, TRTP_ClosingKM: bb.TRTP_ClosingKM, NET_KM: bb.NET_KM, TRHR_RatePerKM: bb.TRHR_RatePerKM, TRTPP_PaidAmount: '', TRMD_DriverName: bb.TRMD_DriverName, TRTP_DiscountAmount:'' })
                        }

                        amt += 1;
                        opentotal += bb.TRTP_OpeningKM
                        closetotal += bb.TRTP_ClosingKM
                        nettotal += bb.NET_KM
                        ratetotal += bb.TRHR_RatePerKM
                        amttotal = bb.TRTPP_PaidAmount
                        disamttotal = bb.TRTP_DiscountAmount
                       
                    })
                    debugger;
                    $scope.ttarray.push({ opentotal: opentotal, closetotal: closetotal, nettotal: nettotal, ratetotal: ratetotal, amttotal: amttotal, disamttotal: disamttotal })
                    console.log($scope.array)
                    console.log($scope.temparray)
                    $scope.amtinwords = convertNumberToWords(amttotal)
                }

            });
            
        }
      

        function convertNumberToWords(amount) {
           
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
                    for (var i = 9 - n_length, j = 0; i < 9; i++, j++) {
                        n_array[i] = received_n_array[j];
                    }
                    for (var i = 0, j = 1; i < 9; i++, j++) {
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


    }
})();
