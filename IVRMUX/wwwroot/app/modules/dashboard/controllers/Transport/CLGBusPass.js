﻿
(function () {
    'use strict';
    angular
.module('app')
        .controller('CLGBusPassController', CLGBusPassController)

    CLGBusPassController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$compile']
    function CLGBusPassController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $compile) {

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.sortKey = "trmL_Id";   //set the sortKey to the param passed
        $scope.sortReverse = true; //if true make it false and vice versa
        $scope.obj = {};
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));

        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgnameee = logopath;

        $scope.currentPage = 1;
        $scope.itemsPerPage = paginationformasters;


        $scope.currentPage1 = 1;
        $scope.itemsPerPage1 = paginationformasters;
        $scope.listshow = false;
        $scope.showww = false;
        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("CLGBusPass/getdata", pageid).
                then(function (promise) {
            if (promise != null) {

                if (promise.logoheader.length > 0) {
                    $scope.imgname = promise.logoheader[0].logopath;
                }
                else {
                    $scope.imgname = logopath;
                }
                $scope.getaccyear = promise.getaccyear;
                $scope.getclass = promise.getcourse;
                $scope.routename = promise.routename;
                //$scope.getdetails = promise.getdetails;
               // $scope.presentCountgrid1 = $scope.getdetails.length;
           
                //if ($scope.getdetails.length > 0) {
                //    $scope.showww = true;
                //}
                //else {
                //    $scope.showww = false;
                //}
            }
        })
        }
        $scope.getbranch_catg = function () {
            $scope.AMB_Id = '';
            $scope.AMSE_Id = '';
            $scope.ACMS_Id = '';
            $scope.semisterlist = [];
            $scope.branchlist = [];
            $scope.semisterlist = [];
            var data = {
                "AMCO_Id": $scope.AMCO_Id,
                "ASMAY_Id": $scope.ASMAY_Id,
            }
            apiService.create("CLGTRNCommon/getbranch", data).
                then(function (promise) {

                    $scope.branchlist = promise.branchlist;

                    if (promise.branchlist == "" || promise.branchlist == null) {
                        swal("No Branch Are Mapped To Selected Course");
                    }
                })

        };
        $scope.get_semister = function () {
            $scope.ACMS_Id = '';
            var data = {
                "AMCO_Id": $scope.AMCO_Id,
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMB_Id": $scope.AMB_Id,
            }
            apiService.create("CLGTRNCommon/get_semister", data).
                then(function (promise) {

                    $scope.semisterlist = promise.semisterlist;

                    if (promise.semisterlist == "" || promise.semisterlist == null) {
                        swal("No Semester Are Mapped To Selected Course/Branch");
                    }
                })
        };
        $scope.submitted = false;
        $scope.submitted1 = false;

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };

        //---Search--//
        $scope.searchdetails = function () {
            debugger;
            if ($scope.myForm.$valid) {

                var semlist = [];

                if ($scope.AMSE_Id == 0) {
                    semlist = $scope.semisterlist
                }
                else {
                    angular.forEach($scope.semisterlist, function (dd) {
                        if ($scope.AMSE_Id == dd.amsE_Id) {
                            semlist.push(dd);
                        }

                    })
                }
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    AMSE_Idss: semlist,
                    "TRMR_Id": $scope.trmR_Id,
                }
                apiService.create("CLGBusPass/searchdetails", data).then(function (promise) {
                    debugger;
                    if (promise.getalldetails.length > 0) {
                        $scope.alll2 = false;
                        $scope.printdatatablee = [];
                        $scope.locationdetails = promise.getalldetails
                     
                       
                        $scope.openingbalance = promise.openingbalance;
                        $scope.totalcharge = promise.transportcharges;
                        $scope.totalchargepaid = promise.transportchargespaid;
                        //praveen
                        $scope.axcess_op_bal = promise.axcess_op_bal;
                       
                        for (var i = 0; i < $scope.locationdetails.length; i++) {
                            for (var j = 0; j < $scope.openingbalance.length; j++) {
                                if ($scope.locationdetails[i].amcsT_Id == $scope.openingbalance[j].amcst_id) {
                                    $scope.locationdetails[i].openingbla = $scope.openingbalance[j].OpeningBalance;
                                }
                                else {
                                    $scope.locationdetails[i].openingbla = 0;
                                }
                            }
                        }
                        debugger;
                        //praveen Added
                        for (var i = 0; i < $scope.locationdetails.length; i++) {
                            
                            for (var j = 0; j < $scope.axcess_op_bal.length; j++) {
                                if ($scope.locationdetails[i].amcsT_Id == $scope.axcess_op_bal[j].amcst_id) {
                                    $scope.locationdetails[i].lastopen = $scope.axcess_op_bal[j].OBOpeningBalance;
                                    $scope.locationdetails[i].axcessbal = $scope.axcess_op_bal[j].OBExcessAmount;
                                   // $scope.locationdetails[i].axcessbal = 500;
                                }
                                else {
                                    $scope.locationdetails[i].lastopen = 0;
                                    $scope.locationdetails[i].axcessbal = 0;
                                    //$scope.locationdetails[i].axcessbal = 500;
                                }
                            }
                        }

                       



                        for (var i = 0; i < $scope.locationdetails.length; i++) {
                            for (var j = 0; j < $scope.totalcharge.length; j++) {
                                if ($scope.locationdetails[i].amcsT_Id == $scope.totalcharge[j].amcsT_Id) {
                                   
                                    if ($scope.totalcharge[j].fmT_Order == 1 && $scope.totalcharge[j].fmH_RefundFlag==false)
                                    {
                                        $scope.locationdetails[i].term1 = $scope.totalcharge[j].payableamount;
                                    }
                                    else if ($scope.totalcharge[j].fmT_Order == 2) {
                                        $scope.locationdetails[i].term2 = $scope.totalcharge[j].payableamount;
                                    }
                                    else if ($scope.totalcharge[j].fmT_Order == 3) {
                                        $scope.locationdetails[i].term3 = $scope.totalcharge[j].payableamount;
                                    }
                                    else if ($scope.totalcharge[j].fmT_Order == 1 && $scope.totalcharge[j].fmH_RefundFlag == true) {
                                        $scope.locationdetails[i].term4 = $scope.totalcharge[j].payableamount;
                                    }
                                }
                            }
                        }


                        for (var i = 0; i < $scope.locationdetails.length; i++) {
                            for (var j = 0; j < $scope.locationdetails.length; j++) {
                                if ($scope.locationdetails[i].amcsT_Id == $scope.locationdetails[j].amcsT_Id) {
                                    var totalamount = 0;
                                    //commented by Praveen
                                    //if ($scope.locationdetails[i].openingbla != null)
                                    //{
                                    //    totalamount =Number($scope.locationdetails[i].openingbla);
                                    //}
                                   

                                   if ($scope.locationdetails[i].term1!=null)
                                    {
                                        totalamount = Number(totalamount) +Number( $scope.locationdetails[i].term1);
                                    }
                                     if ($scope.locationdetails[i].term2 != null) {
                                        totalamount = Number(totalamount) + Number($scope.locationdetails[i].term2);
                                    }
                                     if ($scope.locationdetails[i].term3 != null) {
                                        totalamount =Number( totalamount) +Number( $scope.locationdetails[i].term3);
                                    }
                                     if ($scope.locationdetails[i].term4 != null) {
                                        totalamount = Number(totalamount )+ Number($scope.locationdetails[i].term4);
                                    }
                                    //Added Praveen
                                    if ($scope.locationdetails[i].lastopen != null) {
                                        totalamount = Number(totalamount) +Number($scope.locationdetails[i].lastopen);
                                    }
                                    if ($scope.locationdetails[i].axcessbal != null) {
                                        totalamount = Number(totalamount) - Number($scope.locationdetails[i].axcessbal);
                                    }
                                     //end Praveen


                                    $scope.locationdetails[i].totalamount = totalamount;

                                break;
                            }
                        else if ($scope.locationdetails[i].amcsT_Id == $scope.locationdetails[j].amcsT_Id) {
                            $scope.locationdetails[i].totalamount = 0;
                        }
                        }
                    }

                    for (var i = 0; i < $scope.locationdetails.length; i++) {
                        for (var j = 0; j < $scope.totalchargepaid.length; j++) {
                            if ($scope.locationdetails[i].amcsT_Id == $scope.totalchargepaid[j].amcsT_Id) {

                                if ($scope.totalchargepaid[j].fmT_Order == 1 && $scope.totalchargepaid[j].fmH_RefundFlag == false) {
                                    $scope.locationdetails[i].term1recipt = $scope.totalchargepaid[j].receiptno;
                                    $scope.locationdetails[i].term1date = $scope.totalchargepaid[j].paiddate;
                                }
                                else if ($scope.totalchargepaid[j].fmT_Order == 2) {
                                    $scope.locationdetails[i].term2recipt = $scope.totalchargepaid[j].receiptno;
                                    $scope.locationdetails[i].term2date = $scope.totalchargepaid[j].paiddate;
                                }
                                else if ($scope.totalchargepaid[j].fmT_Order == 3) {
                                    $scope.locationdetails[i].term3recipt = $scope.totalchargepaid[j].receiptno;
                                    $scope.locationdetails[i].term3date = $scope.totalchargepaid[j].paiddate;
                                }
                                else if ($scope.totalchargepaid[j].fmH_RefundFlag == true) {
                                    $scope.locationdetails[i].term4recipt = $scope.totalchargepaid[j].receiptno;
                                    $scope.locationdetails[i].term4date = $scope.totalchargepaid[j].paiddate;
                                }
                               
                                   

                            }

                        }
                    }

                        $scope.locationdetailsnew = [];
                       
                    angular.forEach($scope.locationdetails, function (itm) {
                       
                      //  if ((itm.term1date != undefined || itm.term2date != undefined || itm.term3date != undefined)) {
                            $scope.locationdetailsnew.push(itm);
                        //}
                    });
                      

                    angular.forEach($scope.locationdetails, function (itm) {
                        if (itm.AMCST_PerStreet != null && itm.AMCST_PerStreet.length > 2) {
                            itm.AMCST_PerStreet = itm.AMCST_PerStreet.trim();
                            if ((itm.AMCST_PerStreet.substring(itm.AMCST_PerStreet.length - 1) == ',')) {
                                itm.AMCST_PerStreet = itm.AMCST_PerStreet.substring(0, itm.AMCST_PerStreet.length - 1);
                            }
                            if ((itm.AMCST_PerStreet.substring(0, 1) == ',')) {
                                itm.AMCST_PerStreet = itm.AMCST_PerStreet.substring(1, itm.AMCST_PerStreet.length);
                            }
                        }
                        if (itm.AMCST_PerArea != null && itm.AMCST_PerArea.length > 2) {
                            itm.AMCST_PerArea = itm.AMCST_PerArea.trim();
                            if ((itm.AMCST_PerArea.substring(itm.AMCST_PerArea.length - 1) == ',')) {
                                itm.AMCST_PerArea = itm.AMCST_PerArea.substring(0, itm.AMCST_PerArea.length - 1);
                            }
                            if ((itm.AMCST_PerArea.substring(0, 1) == ',')) {
                                itm.AMCST_PerArea = itm.AMCST_PerArea.substring(1, itm.AMCST_PerArea.length);
                            }
                        }
                        if (itm.AMCST_PerCity != null && itm.AMCST_PerCity.length > 2) {
                            itm.AMCST_PerCity = itm.AMCST_PerCity.trim();
                            if ((itm.AMCST_PerCity.substring(itm.AMCST_PerCity.length - 1) == ',')) {
                                itm.AMCST_PerCity = itm.AMCST_PerCity.substring(0, itm.AMCST_PerCity.length - 1);
                            }
                            if ((itm.AMCST_PerCity.substring(0, 1) == ',')) {
                                itm.AMCST_PerCity = itm.AMCST_PerCity.substring(1, itm.AMCST_PerCity.length);
                            }
                        }

                        //if ((itm.amsT_PerPincode.substring(itm.amsT_ConPincode.length - 1) == ',')) {
                        //    itm.amsT_ConPincode = itm.amsT_ConPincode.substring(0, itm.amsT_ConPincode.length - 1);
                        //}
                    });


                 
                    $scope.presentCountgrid = $scope.locationdetailsnew.length;
                    if ($scope.locationdetailsnew.length > 0)
                    {
                        $scope.listshow = true;

                    }
                    else {
                        $scope.listshow = false;
                         swal("No Records Found")
                    }
                    var e12 = angular.element(document.getElementById("printScheduleSectionId"));
                    $compile(e12.html(promise.htmldata))(($scope));

                    } else {
                        $scope.listshow = false;
                        swal("No Records Found")
                    }
        })
    }
else {
                $scope.submitted1 = true;
}
        }



        $scope.showmodaldetails = function (astA_Id, amsT_Id) {
            
            var data = {
                "AMCST_Id": amsT_Id,
                "ASTACO_Id": astA_Id
            }
            apiService.create("CLGBusPass/showmodaldetails", data).
                then(function (promise) {

                    $scope.getdate = new Date();
                    $scope.buspassdatalst = promise.studentdetails;

                    $scope.appno = $scope.buspassdatalst[0].appno;
                    $('#blahnew').attr('src', $scope.buspassdatalst[0].AMST_Photoname);
                    $scope.AMST_AdmNo = $scope.buspassdatalst[0].AMCST_AdmNo;
                    $scope.ASTA_Landmark = $scope.buspassdatalst[0].ASTACO_Landmark;
                    $scope.amsT_FirstName = $scope.buspassdatalst[0].stuname;
                    $scope.amsT_FatherName = $scope.buspassdatalst[0].AMCST_FatherName;
                    $scope.amsE_SEMName = $scope.buspassdatalst[0].FutureSem;
                    $scope.amcO_CourseName = $scope.buspassdatalst[0].AMCO_CourseName;
                    $scope.amB_BranchName = $scope.buspassdatalst[0].AMB_BranchName;
                    $scope.amsT_BloodGroup = $scope.buspassdatalst[0].AMCST_BloodGroup;
                    $scope.trmR_RouteName = $scope.buspassdatalst[0].PickUp_Route;
                    $scope.trmR_RouteName_no = $scope.buspassdatalst[0].PickUp_Route_no;
                    $scope.PickUp_Location = $scope.buspassdatalst[0].PickUp_Location;
                    $scope.fuyear = $scope.buspassdatalst[0].fuyear;

                    $scope.Drop_Route = $scope.buspassdatalst[0].Drop_Route;
                    $scope.Drop_Route_no = $scope.buspassdatalst[0].Drop_Route_no;
                    $scope.DropUp_Location = $scope.buspassdatalst[0].DropUp_Location;

                    $scope.amsT_FatherMobleNo = $scope.buspassdatalst[0].ASTACO_PickupSMSMobileNo;

                    $scope.amsT_MotherMobileNo = $scope.buspassdatalst[0].ASTACO_DropSMSMobileNo;
                    $scope.amsT_emailId = $scope.buspassdatalst[0].AMCST_emailId;
                    //------------Address
                    $scope.amsT_PerStreet = $scope.buspassdatalst[0].AMCST_PerStreet;
                    $scope.amsT_PerArea = $scope.buspassdatalst[0].AMCST_PerArea;
                    $scope.amsT_PerCity = $scope.buspassdatalst[0].AMCST_PerCity;
                    $scope.ivrmmS_Name = $scope.buspassdatalst[0].IVRMMS_Name;
                    $scope.ivrmmC_CountryName = $scope.buspassdatalst[0].IVRMMC_CountryName;
                    $scope.amsT_PerPincode = $scope.buspassdatalst[0].AMCST_PerPincode;
                    $scope.ASTA_Regnew = $scope.buspassdatalst[0].ASTACO_Regnew;
                    $scope.amsT_Office = $scope.buspassdatalst[0].ASTACO_Phoneoff;
                    $scope.amsT_Res = $scope.buspassdatalst[0].ASTACO_PhoneRes;
                    $scope.getdate = $scope.buspassdatalst[0].ASTACO_ApplicationDate;
                    debugger;
                    $scope.MI_Name = $scope.buspassdatalst[0].MI_Name;
                    $scope.IVRMMCT_Name = $scope.buspassdatalst[0].IVRMMCT_Name;
                    $scope.MI_Pincode = $scope.buspassdatalst[0].MI_Pincode;
                    $scope.MI_Address1 = $scope.buspassdatalst[0].MI_Address1;
                    var e1 = angular.element(document.getElementById("test"));
                    $compile(e1.html(promise.htmldata))(($scope));
                    $('#blahnew').attr('src', $scope.buspassdatalst[0].AMCST_StudentPhoto);
                    $('#blahnewF').attr('src', $scope.buspassdatalst[0].AMCST_FatherPhoto);
                    $('#blahnewM').attr('src', $scope.buspassdatalst[0].AMCST_MotherPhoto);
                   ////
                   ////$scope.getdate = new Date();
                   ////$scope.buspassdatalst = promise.studentdetails;
                   ////$scope.appno = $scope.buspassdatalst[0].appno;
                   ////$scope.obj.amsT_FirstName = $scope.buspassdatalst[0].stuname;
                   ////$scope.obj.AMST_AdmNo = $scope.buspassdatalst[0].AMST_AdmNo;
                   ////$scope.obj.amsT_FatherName = $scope.buspassdatalst[0].AMST_FatherName;
                   ////$scope.obj.asmcL_ClassName = $scope.buspassdatalst[0].ASMCL_ClassName;
                   ////$scope.obj.amsT_BloodGroup = $scope.buspassdatalst[0].AMST_BloodGroup;
                   ////$scope.obj.trmR_RouteName = $scope.buspassdatalst[0].PickUp_Route;
                   ////$scope.obj.PickUp_Location = $scope.buspassdatalst[0].PickUp_Location;
                   ////$scope.obj.fuyear = $scope.buspassdatalst[0].fuyear;

                   ////$scope.obj.Drop_Route = $scope.buspassdatalst[0].Drop_Route;
                   ////$scope.obj.DropUp_Location = $scope.buspassdatalst[0].DropUp_Location;

                   ////$scope.obj.amsT_FatherMobleNo = $scope.buspassdatalst[0].AMST_FatherMobleNo;

                   ////$scope.obj.amsT_MotherMobileNo = $scope.buspassdatalst[0].AMST_MotherMobileNo;
                   ////$scope.obj.amsT_emailId = $scope.buspassdatalst[0].AMST_emailId;
                   //////------------Address
                   ////$scope.obj.amsT_PerStreet = $scope.buspassdatalst[0].AMST_PerStreet;
                   ////$scope.obj.amsT_PerArea = $scope.buspassdatalst[0].AMST_PerArea;
                   ////$scope.obj.amsT_PerCity = $scope.buspassdatalst[0].AMST_PerCity;
                   ////$scope.obj.ivrmmS_Name = $scope.buspassdatalst[0].IVRMMS_Name;
                   ////$scope.obj.ivrmmC_CountryName = $scope.buspassdatalst[0].IVRMMC_CountryName;
                   ////$scope.obj.amsT_PerPincode = $scope.buspassdatalst[0].AMST_PerPincode;

                   //$scope.getdate = new Date();
                   //$scope.buspassdatalst = promise.studentdetails;
                   //$scope.appno = $scope.buspassdatalst[0].appno;
                   //$('#blahnew').attr('src', $scope.buspassdatalst[0].AMST_Photoname);
                   //$scope.pickuprouteno = $scope.buspassdatalst[0].pickuprouteno;
                   //$scope.ASMAY_Year = $scope.buspassdatalst[0].ASMAY_Year;
                   //$scope.AMST_AdmNo = $scope.buspassdatalst[0].AMST_AdmNo;
                   //$scope.ASTA_Landmark = $scope.buspassdatalst[0].ASTA_Landmark;
                   //$scope.amsT_FirstName = $scope.buspassdatalst[0].stuname;
                   //$scope.amsT_FatherName = $scope.buspassdatalst[0].AMST_FatherName;
                   //$scope.asmcL_ClassName = $scope.buspassdatalst[0].ASMCL_ClassName;
                   //$scope.amsT_BloodGroup = $scope.buspassdatalst[0].AMST_BloodGroup;
                   //$scope.trmR_RouteName = $scope.buspassdatalst[0].PickUp_Route;
                   //$scope.trmR_RouteName_no = $scope.buspassdatalst[0].PickUp_Route_no;
                   //$scope.PickUp_Location = $scope.buspassdatalst[0].PickUp_Location;
                   //$scope.fuyear = $scope.buspassdatalst[0].fuyear;

                   //$scope.Drop_Route = $scope.buspassdatalst[0].Drop_Route;
                   //$scope.Drop_Route_no = $scope.buspassdatalst[0].Drop_Route_no;
                   //$scope.DropUp_Location = $scope.buspassdatalst[0].DropUp_Location;

                   //$scope.amsT_FatherMobleNo = $scope.buspassdatalst[0].ASTA_FatherMobileNo;

                   //$scope.amsT_MotherMobileNo = $scope.buspassdatalst[0].ASTA_MotherMobileNo;
                   //$scope.amsT_emailId = $scope.buspassdatalst[0].AMST_emailId;
                   ////------------Address
                   //$scope.amsT_PerStreet = $scope.buspassdatalst[0].AMST_PerStreet;
                   //$scope.amsT_PerArea = $scope.buspassdatalst[0].AMST_PerArea;
                   //$scope.amsT_PerCity = $scope.buspassdatalst[0].AMST_PerCity;
                   //$scope.ivrmmS_Name = $scope.buspassdatalst[0].IVRMMS_Name;
                   //$scope.ivrmmC_CountryName = $scope.buspassdatalst[0].IVRMMC_CountryName;
                   //$scope.amsT_PerPincode = $scope.buspassdatalst[0].AMST_PerPincode;
                   //$scope.ASTA_Regnew = $scope.buspassdatalst[0].ASTA_Regnew;
                   //$scope.amsT_Office = $scope.buspassdatalst[0].ASTA_Phoneoff;
                   //$scope.amsT_Res = $scope.buspassdatalst[0].ASTA_PhoneRes;
                   //$scope.getdate = $scope.buspassdatalst[0].ASTA_ApplicationDate;
                   //var e1 = angular.element(document.getElementById("test"));
                   //$compile(e1.html(promise.htmldata))(($scope));
                   //$('#blahnew').attr('src', $scope.buspassdatalst[0].AMST_Photoname);
                   //$('#blahnewF').attr('src', $scope.buspassdatalst[0].ANST_FatherPhoto);
                   //$('#blahnewM').attr('src', $scope.buspassdatalst[0].ANST_MotherPhoto);
                
               })
        }

//Save Approved List and rejected list
$scope.saveapproved = function (obj) {

    if ($scope.printdatatable.length > 0) {
        var data = {
            "Temp_Save_List": $scope.printdatatable,
            "ASMAY_Id": $scope.obj.asmaY_Id,
            "ASMCL_Id": $scope.obj.asmcL_Id,
            "Flag": "A"
        }
        apiService.create("CLGBusPass/savelist", data).then(function (promise) {
            if (promise != null) {
                swal(promise.message);
                $state.reload();
            }
        })
    } else {
        swal("Select Student List")
    }
}

//Save Approved List and rejected list
$scope.saveapprovedreject = function (obj) {
    if ($scope.printdatatable.length > 0) {
        var data = {
            "Temp_Save_List": $scope.printdatatable,
            "ASMAY_Id": $scope.obj.asmaY_Id,
            "ASMCL_Id": $scope.obj.asmcL_Id,
            "Flag": "R"
        }
        apiService.create("CLGBusPass/savelist", data).then(function (promise) {
            if (promise != null) {
                swal(promise.message);
                $state.reload();
            }
        })
    } else {
        swal("Select Student List")
    }
}



$scope.printdatatable = [];
$scope.printdatatablee = [];
$scope.toggleAll = function () {
    var toggleStatus = $scope.all2;
    angular.forEach($scope.locationdetailsnew, function (itm) {
        itm.selected = toggleStatus;
        if ($scope.all2 == true) {
            $scope.printdatatable.push(itm);
        }
        else {
            $scope.printdatatable.splice(itm);
        }
    });
}

$scope.toggleAlll = function () {
    var toggleStatuss = $scope.alll2;
    angular.forEach($scope.locationdetailsnew, function (itm) {
        itm.selected = toggleStatuss;
        if ($scope.alll2 == true) {
            $scope.printdatatablee.push(itm);
        }
        else {
            $scope.printdatatablee.splice(itm);
        }
    });
}

$scope.optionToggled = function (SelectedStudentRecord, index) {
    debugger;
    $scope.all2 = $scope.locationdetailsnew.every(function (itm)
    { return itm.selected; });
    if ($scope.printdatatablee.indexOf(SelectedStudentRecord) === -1) {
        $scope.printdatatablee.push(SelectedStudentRecord);
    }
    else {
        $scope.printdatatablee.splice($scope.printdatatablee.indexOf(SelectedStudentRecord), 1);
    }
}

//$scope.optionToggledd = function (SelectedStudentRecord, index) {
    
//    $scope.all2 = $scope.getdetails.every(function (itm)
//    { return itm.selected; });
//    if ($scope.printdatatablee.indexOf(SelectedStudentRecord) === -1) {
//        $scope.printdatatablee.push(SelectedStudentRecord);
//    }
//    else {
//        $scope.printdatatablee.splice($scope.printdatatablee.indexOf(SelectedStudentRecord), 1);
//    }
//}

$scope.searchValue = '';
$scope.filterValue = function (obj) {
    return (angular.lowercase(obj.studentname)).indexOf($scope.searchValue) >= 0 ||
        (angular.lowercase(obj.applicationno)).indexOf($scope.searchValue) >= 0 ||
          (angular.lowercase(obj.astA_ApplStatus)).indexOf($scope.searchValue) >= 0 ||
        (angular.lowercase(obj.areaname)).indexOf($scope.searchValue) >= 0 ||
         (angular.lowercase(obj.pickuproute)).indexOf($scope.searchValue) >= 0 ||
          (angular.lowercase(obj.pickuplocation)).indexOf($scope.searchValue) >= 0 ||
        (angular.lowercase(obj.drouproute)).indexOf($scope.searchValue) >= 0 ||
        (angular.lowercase(obj.drouplocation)).indexOf($scope.searchValue) >= 0 ||
        (angular.lowercase(obj.neworreguular)).indexOf($scope.searchValue) >= 0
}

$scope.searchValue1 = '';
$scope.filterValue12 = function (obj) {
    return (angular.lowercase(obj.studentname)).indexOf($scope.searchValue1) >= 0 ||
        (angular.lowercase(obj.applicationno)).indexOf($scope.searchValue1) >= 0 ||
          (angular.lowercase(obj.astA_ApplStatus)).indexOf($scope.searchValue1) >= 0 ||
        (angular.lowercase(obj.areaname)).indexOf($scope.searchValue1) >= 0
}

$scope.printScheduleData = function (printSchedule_data) {
    
    if ($scope.printdatatablee !== null && $scope.printdatatablee.length > 0) {
        var innerContents = document.getElementById("printScheduleSectionId").innerHTML;
        var popupWinindow = window.open('');
        popupWinindow.document.open();
        popupWinindow.document.write('<html><head>' +
       '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
              '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BGHS/BALDWINGIRLSTRANSPORT/BusPassReceiptPdf.css" />' +
          '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
        '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
        popupWinindow.document.close();
    }
    else {
        swal("Please Select Records to be Printed");
    }
}

$scope.printbusspassbbhs = function () {
    
    var innerContents = document.getElementById("BBHSBUSSFORM").innerHTML;
    var popupWinindow = window.open('');
    popupWinindow.document.open();
    popupWinindow.document.write('<html><head>' +

   '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
          '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BBHS/BBHSBUSSFORM/BBHSBUSSFORMPdf.css" />' +
      '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +

    '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
    popupWinindow.document.close();
}

$scope.BGHSAPP = function () {
    
    var innerContents = document.getElementById("BGHSAPP").innerHTML;
    var popupWinindow = window.open('');
    popupWinindow.document.open();
    popupWinindow.document.write('<html><head>' +

        '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
               '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BGHS/BGHSAPPPdf.css" />' +
           '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +

         '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
    popupWinindow.document.close();
}
//--Sorting--//     
$scope.sort = function (key) {
    $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
    $scope.sortKey = key;
}

$scope.cancel = function () {
    $state.reload();
}
};
})();

