
(function () {
    'use strict';
    angular
.module('app')
        .controller('TRServiceBillController', TRServiceBillController)

    TRServiceBillController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter']
    function TRServiceBillController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter) {



        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }
        $scope.masterlist = false;
        $scope.alldd = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;
        $scope.searchValue = "";
        $scope.TRSE_Id = "";
        $scope.TRMV_Id = '';
        $scope.TRMSST_Id = 0;
        $scope.totalballance1 = 0;

        $scope.search = '';
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

        $scope.tdssamt = 0;
        $scope.tdschange = function () {

            if ($scope.TRSE_ItemsCost == "" || $scope.TRSE_ItemsCost == null || $scope.TRSE_ItemsCost == undefined) {
                $scope.TRSE_ItemsCost = 0;
            }

            var lbrcharge = 0;
            lbrcharge = parseFloat($scope.TRSE_LabourCharges) - ((parseFloat($scope.TRSE_TDSValue) / 100) * parseFloat($scope.TRSE_LabourCharges));
            $scope.tdssamt = ((parseFloat($scope.TRSE_TDSValue) / 100) * parseFloat($scope.TRSE_LabourCharges));
            $scope.TRSE_TotalBillValue = parseFloat($scope.TRSE_ItemsCost) + lbrcharge;

            if ($scope.TRSE_TotalDiscount == null || $scope.TRSE_TotalDiscount == undefined || $scope.TRSE_TotalDiscount == '') {
                $scope.TRSE_TotalDiscount = 0;
            }

            $scope.GrandTRSE_TotalBillValue = (parseFloat($scope.TRSE_TotalBillValue)) - (parseFloat($scope.TRSE_TotalDiscount));
            
        }
        $scope.mainamtchange = function () {

            if ($scope.TRSE_ItemsCost == "" || $scope.TRSE_ItemsCost == null || $scope.TRSE_ItemsCost == undefined) {
                $scope.TRSE_ItemsCost = 0;
            }

            if ($scope.TRSE_LabourCharges == "" || $scope.TRSE_LabourCharges == null || $scope.TRSE_LabourCharges == undefined) {
                $scope.TRSE_LabourCharges = 0;
            }
            if ($scope.TRSE_TDSValue == "" || $scope.TRSE_TDSValue == null || $scope.TRSE_TDSValue == undefined) {
                $scope.TRSE_TDSValue = 0;
            }
            var lbrcharge = 0;
            lbrcharge = parseFloat($scope.TRSE_LabourCharges) - ((parseFloat($scope.TRSE_TDSValue) / 100) * parseFloat($scope.TRSE_LabourCharges));
            $scope.TRSE_TotalBillValue = parseFloat($scope.TRSE_ItemsCost) + lbrcharge;
            $scope.tdssamt = ((parseFloat($scope.TRSE_TDSValue) / 100) * parseFloat($scope.TRSE_LabourCharges));
            if ($scope.TRSE_TotalDiscount == null || $scope.TRSE_TotalDiscount == undefined || $scope.TRSE_TotalDiscount == '') {
                $scope.TRSE_TotalDiscount = 0;
            }

            $scope.GrandTRSE_TotalBillValue = (parseFloat($scope.TRSE_TotalBillValue)) - (parseFloat($scope.TRSE_TotalDiscount));

            
        }


        $scope.typechange = function (rr) {
            debugger;
            var cnt = 0;
            angular.forEach($scope.itemlist, function (ww) {
                if (rr.trseD_Id == ww.trseD_Id) {
                    cnt += 1;
                    rr.trseD_TotalAmount = parseFloat(rr.trseD_Amount);
                    rr.TRTP_DiscountAmountpe = 0;
                    rr.trseD_TotalDiscount = 0;
                }

            })
            if (cnt==0) {
                rr.trseD_TotalAmount = parseFloat(rr.trseD_Amount);
                rr.TRTP_DiscountAmountpe = 0;
                rr.trseD_TotalDiscount = 0;
            }

        }
        $scope.changeper = function (rr) {
            debugger;
            var cnt = 0;
            angular.forEach($scope.itemlist, function (ww) {
              
                if (rr.trseD_Id == ww.trseD_Id) {
                    cnt += 1;
                    rr.trseD_TotalDiscount = (parseFloat(rr.TRTP_DiscountAmountpe) / 100) * parseFloat(rr.trseD_Amount);
                    rr.trseD_TotalDiscount = parseFloat(rr.trseD_TotalDiscount).toFixed(2);;

                    if (rr.trseD_TotalDiscount > parseFloat(rr.trseD_Amount)) {
                        swal('discount amount is Greater than the item amount');
                    }
                    else {
                        rr.trseD_TotalAmount = parseFloat(rr.trseD_Amount) - (parseFloat(rr.trseD_TotalDiscount))
                    }
                   
               
                }

            })
            if (cnt==0) {
                rr.trseD_TotalDiscount = (parseFloat(rr.TRTP_DiscountAmountpe) / 100) * parseFloat(rr.trseD_Amount);
                rr.trseD_TotalDiscount = parseFloat(rr.trseD_TotalDiscount).toFixed(2);;

                if (rr.trseD_TotalDiscount > parseFloat(rr.trseD_Amount)) {
                    swal('discount amount is Greater than the item amount');
                }
                else {
                    rr.trseD_TotalAmount = parseFloat(rr.trseD_Amount) - (parseFloat(rr.trseD_TotalDiscount))
                }
            }
        }
        $scope.trippercchange = function () {
            debugger;

            if ($scope.TRTP_Discounprc == null || $scope.TRTP_Discounprc == undefined || $scope.TRTP_Discounprc == "") {
                $scope.TRTP_Discounprc = 0;
            }

            if ($scope.TRSE_TotalDiscount == null || $scope.TRSE_TotalDiscount == undefined || $scope.TRSE_TotalDiscount == "") {
                $scope.TRSE_TotalDiscount = 0;
            }


            if ($scope.TRSE_TotalBillValue == null || $scope.TRSE_TotalBillValue == undefined || $scope.TRSE_TotalBillValue == "") {
                $scope.TRSE_TotalBillValue = 0;
            }
            var disc = 0;
            disc = (parseFloat($scope.TRTP_Discounprc) / 100) * parseFloat($scope.TRSE_TotalBillValue);

           
            //disc = (parseFloat($scope.TRTP_Discounprc) / 100) * parseFloat($scope.ttlbillamount);

            //$scope.TRSEP_Amount = $scope.ttlbillamount - disc;
         
            $scope.TRSE_TotalDiscount = disc;
            $scope.GrandTRSE_TotalBillValue = parseFloat($scope.TRSE_TotalBillValue) - disc;
          
           

        }
        $scope.tripdiscchange = function () {
            debugger;

            if ($scope.TRTP_Discounprc == null || $scope.TRTP_Discounprc == undefined || $scope.TRTP_Discounprc == "") {
                $scope.TRTP_Discounprc = 0;
            }

            if ($scope.TRSE_TotalDiscount == null || $scope.TRSE_TotalDiscount == undefined || $scope.TRSE_TotalDiscount == "") {
                $scope.TRSE_TotalDiscount = 0;
            }
        
            $scope.GrandTRSE_TotalBillValue = parseFloat($scope.TRSE_TotalBillValue) - parseFloat($scope.TRSE_TotalDiscount);
          //  $scope.TRSEP_Amount = parseFloat($scope.ttlbillamount) - parseFloat($scope.TRSE_TotalDiscount);
         
           
           

        }

        $scope.changedisc = function (rr) {
            debugger;
            var cnt = 0;
            angular.forEach($scope.itemlist, function (ww) {
                if (rr.trseD_Id == ww.trseD_Id) {
                    cnt += 1;
                    if (parseFloat(rr.trseD_Amount) > parseFloat(ww.trseD_TotalDiscount)) {
                        rr.trseD_TotalAmount = parseFloat(rr.trseD_Amount) - (parseFloat(ww.trseD_TotalDiscount))
                    }
                    else {
                        swal('discount amount is Greater than the item amount');
                        rr.trseD_TotalAmount = parseFloat(rr.trseD_Amount);
                        rr.trseD_TotalDiscount = 0;
                    }
                    
                }

            })
            if (cnt==0) {
                if (parseFloat(rr.trseD_Amount) > parseFloat(rr.trseD_TotalDiscount)) {
                    rr.trseD_TotalAmount = parseFloat(rr.trseD_Amount) - (parseFloat(rr.trseD_TotalDiscount))
                }
                else {
                    swal('discount amount is Greater than the item amount');
                    rr.trseD_TotalAmount = parseFloat(rr.trseD_Amount);
                    rr.trseD_TotalDiscount = 0;
                }
            }

        }

        $scope.additems = false;
        $scope.serivelist = [];
        $scope.itemlist = [];
        $scope.paymentlist = [];
        $scope.get_srvdetails = function () {
            debugger;
            $scope.vehicles = [{ id: 'vehicle1' }];
            $scope.serivelist = [];
            $scope.itemlist = [];
            $scope.paymentlist = [];
            var servid = $scope.TRSE_Id.trsE_Id;

            var data = {
                "TRSE_Id":servid
            }
            apiService.create("MasterServiceStation/get_srvdetails", data).then(function (promise) {
                $scope.alldd = true;
                $scope.serivelist = promise.serivelist;
                $scope.itemlist = promise.itemlist;
                $scope.paymentlist = promise.paymentlist;
                debugger;
                //add service data
                $scope.TRSE_ServiceDate1 = new Date($scope.serivelist[0].trsE_ServiceDate);
                $scope.Station1 = $scope.serivelist[0].trmssT_ServiceStationName;
                $scope.vehicleno1 = $scope.serivelist[0].trmV_VehicleNo;
                $scope.TRSE_ProblemsListed1 = $scope.serivelist[0].trsE_ProblemsListed;
                $scope.TRSE_ServiceDetails1 = $scope.serivelist[0].trsE_ServiceDetails;

                if ($scope.serivelist[0].trsE_BillDate != null && $scope.serivelist[0].trsE_BillDate != '') {
                    $scope.TRSE_BillDate = new Date($scope.serivelist[0].trsE_BillDate)
                }
                else {
                    $scope.TRSE_BillDate = new Date;
                }

                $scope.TRSE_BillNo = $scope.serivelist[0].trsE_BillNo;
                $scope.TRSE_ItemsCost = $scope.serivelist[0].trsE_ItemsCost;
                $scope.TRSE_LabourCharges = $scope.serivelist[0].trsE_LabourCharges;
                $scope.TRSE_TDSValue = $scope.serivelist[0].trsE_TDSValue;
                $scope.TRSE_TotalBillValue = $scope.serivelist[0].trsE_TotalBillValue;
                $scope.TRSE_TotalDiscount = $scope.serivelist[0].trsE_TotalDiscount;
                $scope.tdssamt = ((parseFloat($scope.TRSE_TDSValue) / 100) * parseFloat($scope.TRSE_LabourCharges));
                if ($scope.TRSE_TotalBillValue == null || $scope.TRSE_TotalBillValue == undefined || $scope.TRSE_TotalBillValue == '') {
                    $scope.TRSE_TotalBillValue = 0;
                }
                if ($scope.TRSE_TotalDiscount == null || $scope.TRSE_TotalDiscount == undefined || $scope.TRSE_TotalDiscount == '') {
                    $scope.TRSE_TotalDiscount = 0;
                }

                $scope.GrandTRSE_TotalBillValue = (parseFloat($scope.TRSE_TotalBillValue)) - (parseFloat($scope.TRSE_TotalDiscount));

                $scope.ttlbillamount1 = $scope.serivelist[0].trsE_TotalBillValue;
                $scope.totalpdamt1 = $scope.serivelist[0].trsE_TotalPaid;

                $scope.ttldiscamount1 = $scope.serivelist[0].trsE_TotalDiscount;
                debugger;
                $scope.totalballance1 = parseFloat($scope.ttlbillamount1) - (parseFloat($scope.totalpdamt1) + parseFloat($scope.ttldiscamount1));

                if (promise.itemlist.length > 0) {
                    $scope.additems = true;
                    $scope.vehicles = promise.itemlist;
                }
                else {
                    $scope.additems = false;
                }
               
            })
        }
        $scope.findservice = function () {
            debugger;
            $scope.TRSE_Id = '';
            $scope.alldd = false;
            $scope.paymentlist = [];
            $scope.servicenolist=[];
            var data = {
                "TRMV_Id": $scope.TRMV_Id.trmV_Id,
                "TRMSST_Id": $scope.TRMSST_Id,
            }
            apiService.create("MasterServiceStation/findservice", data).then(function (promise) {
                if (promise.servicenolist.length>0) {
                    $scope.servicenolist = promise.servicenolist;
                }
                else {
                    swal("Service No. Not Exist");
                }

            })
        }
        $scope.duprecpcheck = function () {
            debugger;
       
            var data = {
                "TRSE_Id": $scope.TRSE_Id.trsE_Id,
                "TRSE_BillNo": $scope.TRSE_BillNo,
            }
            apiService.create("MasterServiceStation/duprecpcheck", data).then(function (promise) {
                if (promise.returnVal=="dup") {
                    swal("Duplicate Bill No");
                    $scope.TRSE_BillNo = '';
                }
               
            })
        }


        $scope.PayBill = function () {
            debugger;
            var servid = $scope.TRSE_Id.trsE_Id;

            var data = {
                "TRSE_Id":servid
            }
            apiService.create("MasterServiceStation/PayBill", data).then(function (promise) {
                $scope.modeOfPaymentList2 = promise.modeOfPaymentList;
                $scope.payingdatalist = promise.payingdatalist;

                $scope.itemlist = promise.itemlist;
                $scope.paymentlist = promise.paymentlist;
                debugger;
              
                $scope.payservdate = new Date($scope.serivelist[0].trsE_ServiceDate);
                $scope.paybilldate = new Date($scope.serivelist[0].trsE_BillDate);
                
                $scope.payservno = $scope.serivelist[0].trsE_ServiceRefNo;
                $scope.paybillno = $scope.serivelist[0].trsE_BillNo;
                $scope.ttlbillamount = $scope.serivelist[0].trsE_TotalBillValue;
                $scope.totalpdamt = $scope.serivelist[0].trsE_TotalPaid;
               
                $scope.ttldiscamount = $scope.serivelist[0].trsE_TotalDiscount;
                debugger;
                $scope.totalballance = parseFloat($scope.ttlbillamount) - (parseFloat($scope.totalpdamt) + parseFloat($scope.ttldiscamount));

                $scope.TRSEP_Amount = parseFloat($scope.totalballance);

            })
        }
        $scope.reqSelection = true;
        $scope.FinalPayBill = function () {
            debugger;
            if ($scope.myForm123.$valid) {
            var servid = $scope.TRSE_Id.trsE_Id;
            //if ($scope.TRTPP_ChequeDDDate != undefined || $scope.TRTPP_ChequeDDDate != null) {
            //    $scope.TRTPP_ChequeDDDate = new Date($scope.TRTPP_ChequeDDDate).toDateString();
            //}
            //else {
            //    $scope.TRTPP_ChequeDDDate = null;
            //}
            var fromdate1 = $scope.TRTPP_ChequeDDDate == null ? "" : $filter('date')($scope.TRTPP_ChequeDDDate, "yyyy-MM-dd");
            var fromdate2 = $scope.TRSEP_PaymentDate == null ? "" : $filter('date')($scope.TRSEP_PaymentDate, "yyyy-MM-dd");
            var data = {
                "TRSE_Id":servid,
                "TRSEP_TransactionRefNo": $scope.TRSEP_TransactionRefNo,
                "TRSEP_ChequeDDNo": $scope.TRTPP_ChequeDDNo,
                "TRSEP_ChequeDDDate": fromdate1,
                "TRSEP_PaymentDate": fromdate2,
                "TRSEP_Amount": $scope.TRSEP_Amount,
                "TRSE_TotalDiscount": $scope.TRSE_TotalDiscount,
                "TRSEP_BankName": $scope.TRTPP_BankName,
                "TRSEP_ModeOfPayment": $scope.PaymentMode,
            }
                apiService.create("MasterServiceStation/FinalPayBill", data).then(function (promise) {
                    if (promise.returnVal == "Add") {
                        if (promise.retval == true) {
                            swal("Record Saved Successfully");
                            $scope.get_srvdetails();
                            $('#myModal31').hide();
                        }
                        else {
                            swal("Record Not Saved");
                        }
                    }

            })
        }
            else {
                $scope.submitted1 = true;
        }
        }

        $scope.modeofPayment1 = function (val) {
            $scope.reqSelection = false;
            $scope.PaymentMode = val;
            if (val == 'Cheque' || val == 'DD' || val == 'Bank' || val == 'Card' || val == 'RTGS/NEFT') {
                $scope.Cheque = true;
            }
            else {
                $scope.Cheque = false;
            }
        }
        $scope.typemainchange1 = function () {
            $scope.TRTP_Discounprc = 0;
            $scope.TRSE_TotalDiscount = 0;
            $scope.TRSEP_Amount = $scope.totalballance;
            $scope.GrandTRSE_TotalBillValue = $scope.TRSE_TotalBillValue;

        }

        $scope.vehicaldetails = [];
        $scope.BindData = function () {
            
            var pageid = 2;
            apiService.getURI("MasterServiceStation/Servicebillload", pageid).then(function (promise) {
                if (promise != null) {
                    $scope.vehicaldetails = promise.vehicaldata;

                    $scope.vehicaldetails.push({ trmV_Id: 0, trmV_VehicleNo: 'ALL' })

                    //if ($scope.vehicaldetails.length > 0) {
                    //    $scope.TRMV_Id.trmV_Id = 0;
                    //}
                 //   $scope.driverdata = promise.driverdata;
                    $scope.servnamelist = promise.servnamelist;
                    $scope.servicenolist = promise.servicenolist;
                }
                else {
                    swal("No Records Found")
                }
            })


        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };
        $scope.TRSE_BillDate = new Date;
        //---Save--//
        $scope.savedata = function () {
            debugger;
            if ($scope.myForm.$valid) {
                var fromdate1 = $scope.TRSE_ServiceDate1 == null ? "" : $filter('date')($scope.TRSE_ServiceDate1, "yyyy-MM-dd");
                var fromdate2 = $scope.TRSE_BillDate == null ? "" : $filter('date')($scope.TRSE_BillDate, "yyyy-MM-dd");


                var assignedVehicle = $scope.vehicles;
                var newassgn = [];
                angular.forEach(assignedVehicle, function (qqq) {
                    newassgn.push({ id: qqq.id, trseD_ItemsName: qqq.trseD_ItemsName, trseD_Qty: qqq.trseD_Qty, trseD_Remarks: qqq.trseD_Remarks, trseD_ProblemsListed: qqq.trseD_ProblemsListed, trseD_ServiceDetails: qqq.trseD_ServiceDetails, trseD_Amount: qqq.trseD_Amount, trseD_TotalDiscount: qqq.trseD_TotalDiscount, trseD_TotalAmount: qqq.trseD_TotalAmount})
                })
                if ($scope.additems == true) {
                    var data = {
                        "TRSE_Id": $scope.TRSE_Id.trsE_Id,
                        //"TRMV_Id": $scope.TRMV_Id,
                        //"TRMD_Id": $scope.TRMD_Id,
                        //"TRMSST_Id": $scope.TRMSST_Id,
                        "TRSE_ServiceDate": fromdate2,
                        "TRSE_BillDate": fromdate1,
                        "TRSE_ProblemsListed": $scope.TRSE_ProblemsListed1,
                        "TRSE_ServiceDetails": $scope.TRSE_ServiceDetails1,
                        "TRSE_BillNo": $scope.TRSE_BillNo,
                        "TRSE_ItemsCost": $scope.TRSE_ItemsCost,
                        "TRSE_LabourCharges": $scope.TRSE_LabourCharges,
                        "TRSE_TDSValue": $scope.TRSE_TDSValue,
                        "TRSE_TotalBillValue": $scope.TRSE_TotalBillValue,
                        "TRSE_TotalDiscount": $scope.TRSE_TotalDiscount,
                        "allotteditems": newassgn
                    }
                }
                else {
                    var data = {
                        "TRSE_Id": $scope.TRSE_Id.trsE_Id,
                        //"TRMV_Id": $scope.TRMV_Id,
                        //"TRMD_Id": $scope.TRMD_Id,
                        //"TRMSST_Id": $scope.TRMSST_Id,
                        "TRSE_ServiceDate": fromdate2,
                        "TRSE_BillDate": fromdate1,
                        "TRSE_ProblemsListed": $scope.TRSE_ProblemsListed1,
                        "TRSE_ServiceDetails": $scope.TRSE_ServiceDetails1,
                        "TRSE_BillNo": $scope.TRSE_BillNo,
                        "TRSE_ItemsCost": $scope.TRSE_ItemsCost,
                        "TRSE_LabourCharges": $scope.TRSE_LabourCharges,
                        "TRSE_TDSValue": $scope.TRSE_TDSValue,
                        "TRSE_TotalBillValue": $scope.TRSE_TotalBillValue,
                        "TRSE_TotalDiscount": $scope.TRSE_TotalDiscount,
                        //"allotteditems": newassgn
                        //"allotteditems": newassgn
                    }
                }

              
                apiService.create("MasterServiceStation/saveBilldata", data).then(function (promise) {
                    if (promise.returnVal == "Add") {
                        if (promise.retval == true) {
                            swal("Record Saved Successfully");
                            $scope.get_srvdetails();


                        }
                        else {
                            swal("Record Not Saved");
                        }
                    }
                    else if (promise.returnVal == "update") {
                        if (promise.retval == true) {
                            swal("Record Updated Successfully");
                            $scope.get_srvdetails();
                        }
                        else {
                            swal("Record Not Updated");
                        }
                    }
                   
                    else if (promise.returnVal == "duplicate") {
                        swal("Record Already Exists");
                    }
                   // $state.reload();
                })
            }
            else {
                $scope.submitted = true;
            }
        }


        $scope.TRSEP_PaymentDate = new Date;
        //--Sorting--//     
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
       

        //---Edit Data--//
        $scope.viewitems = function (user) {
            //var data = {
            //    "TRPAP_Id": user.trpaP_Id
            //}
            $scope.itemlist = [];
            $scope.srno = user.trsE_ServiceRefNo;
            apiService.getURI("MasterServiceStation/viewitems", user.trsE_Id).then(function (promise) {
                if (promise != null) {

                    if (promise.itemlist.length > 0) {
                        $scope.itemlist = promise.itemlist;

                    }
                    else {

                    }
                }

                //$scope.vehicaldetails = promise.vehicaldata;
                //$scope.driverdata = promise.driverdata;
                //$scope.sessiondetils = promise.sessiondata;


            })
        }
     

        //---Edit Data--//
        $scope.edit = function (user) {
            //var data = {
            //    "TRPAP_Id": user.trpaP_Id
            //}
            $scope.vehicles = [{ id: 'vehicle1' }];
            apiService.getURI("MasterServiceStation/editpartsdata", user.trsE_Id).then(function (promise) {
                if (promise != null) {

                    $scope.TRMV_Id = promise.editparts[0];
                    $scope.TRMV_Id.trmV_Id = promise.editparts[0].trmV_Id;
                    $scope.TRMD_Id = promise.editparts[0].trmD_Id;
                    $scope.TRMSST_Id = promise.editparts[0].trmssT_Id;
                    $scope.TRSE_ServiceDate = new Date(promise.editparts[0].trsE_ServiceDate);
                    $scope.TRSE_ServiceDetails = promise.editparts[0].trsE_ServiceDetails;
                    $scope.TRSE_ProblemsListed = promise.editparts[0].trsE_ProblemsListed;
                    $scope.TRMSES_Parts = promise.editparts[0].trmseS_Parts;
                    $scope.TRSE_Id = promise.editparts[0].trsE_Id;
                    // $scope.TRPAPT_Id = Promise.editparts[0].trpapT_Id;

                    if (promise.itemlist.length > 0) {
                        $scope.additems = true;
                        $scope.vehicles = promise.itemlist;
                    }
                    else {
                        $scope.additems = false;
                    }
                   

                }
             
                //$scope.vehicaldetails = promise.vehicaldata;
                //$scope.driverdata = promise.driverdata;
                //$scope.sessiondetils = promise.sessiondata;
               
               
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
            if (user.trsE_ActiveFlag === true) {
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
                apiService.create("MasterServiceStation/activedeactiveparts/", user).
                then(function (promise) {
                    
                        if (promise.retval == true) {
                            swal(confirmmgs + " " + "Successfully");
                            $state.reload();
                        }
                        else if (promise.retval == false) 
                        {
                            swal(confirmmgs + " " + " Successfully");
                            $state.reload();
                    }
                        else {
                            swal("Error");
                            $state.reload();
                        }
                    
                })
            }
            else {
                swal("Record " + mgs + " Cancelled");
            }
            $state.reload();
        });
        }

        $scope.delete_rec = function (employee) {

            var mgs = "";
            var confirmmgs = "";



            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            if (employee.trseP_ActiveFlag === true) {
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

                        apiService.create("MasterServiceStation/delete_rec", employee).
                            then(function (promise) {
                                if (promise.retval == true) {
                                    swal("Record " + confirmmgs + " " + "successfully");
                                    $scope.get_srvdetails();

                                }
                                else {
                                    swal("Record " + mgs + " Failed");
                                }
                                //   $state.reload();
                                //$scope.clear();
                                //$scope.BindData();
                            })

                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        }


        //-Clear-//
        $scope.clearid = function () {
            //$scope.trvD_Id = 0;
            //$scope.trmV_VehicleName = "";
            //$scope.submitted = false;
            //$scope.myForm1.$setPristine();
            //$scope.myForm1.$setUntouched();
            $state.reload();
        };
        //--Sorting--//     
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }


        $scope.searchValue = '';

    };

   

})();


