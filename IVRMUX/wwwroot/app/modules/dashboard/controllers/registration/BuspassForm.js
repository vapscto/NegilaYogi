(function () {
    'use strict';
    angular
.module('app')
.controller('BuspassFormController', BuspassFormController)

    //BuspassFormController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'superCache', '$window']
    //function BuspassFormController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, superCache, $window) {
    BuspassFormController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache']
    function BuspassFormController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache) {
   
        var HostName = location.host;

        $scope.obj = {};
        $scope.pdlocation = false;

        var searchObject = $location.search();
        //*****MAXMINAGE****
        //$scope.classdesable = true;

        // alert(searchObject.status);
        if (searchObject.status == "failure") {
            swal("Payment Unsuccessfull");
            //  Request.QueryString.Remove("status");
            //$location.url($location.path)
            if ($location.$$search.status) {
                delete $location.$$search.status;
                $location.$$compose();
            }
        }
        else if (searchObject.status == "success") {
            swal("Payment successful and Please submit the filled application to the School office");
            //Request.QueryString.Remove("status");
            if ($location.$$search.status) {
                delete $location.$$search.status;
                $location.$$compose();
            }
        }
        else if (searchObject.status == "Networkfailure") {
            swal('Network failure..!!', 'Try again after some time');
            //Request.QueryString.Remove("status");
            if ($location.$$search.status) {
                delete $location.$$search.status;
                $location.$$compose();
            }
        }

      

        $scope.loaddata = function () {
            $scope.currentPage = 1;
            $scope.itemsPerPage = 10;
            
            apiService.getDATA("BuspassForm/getloaddata").
                then(function (promise) {
                    
                    $scope.studentnamelst = promise.studentDetails;
                    $scope.routeDetls = promise.routeDetails;
                    $scope.prospectusPaymentlist = promise.prospectusPaymentlist;
                    if ($scope.prospectusPaymentlist.length > 0) {
                        for (var i = 0; i < $scope.routeDetls.length; i++) {
                            for (var j = 0; j < $scope.prospectusPaymentlist.length; j++) {
                                if ($scope.routeDetls[i].pasR_Id == $scope.prospectusPaymentlist[j].pasA_Id) {
                                    $scope.routeDetls[i].viewflag = true;
                                    $scope.routeDetls[i].download = false;
                                    break;
                                }
                                else if ($scope.routeDetls[i].pasR_Id != $scope.prospectusPaymentlist[j].pasA_Id) {
                                    $scope.routeDetls[i].viewflag = false;
                                    $scope.routeDetls[i].download = true;

                                }
                            }
                        }
                        console.log($scope.pages);
                    }
                    else {
                        for (var j = 0; j < $scope.routeDetls.length; j++) {
                            $scope.routeDetls[j].viewflag = false;
                            $scope.routeDetls[j].download = true;
                        }
                    }

                    $scope.areaLst = promise.areaList;
                   
                })
        };
        $scope.ProspectuseScreen = true;
        var configsettings = JSON.parse(localStorage.getItem("configsettings"));

        $scope.configurationsettings = configsettings[i];

        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        for (var i = 0; i < transnumconfigsettings.length; i++) {
            if (transnumconfigsettings.length > 0) {
                if (transnumconfigsettings[i].imN_Flag == "Online") {
                    $scope.transnumbconfigurationsettingsassign = transnumconfigsettings[i];
                }
            }
        }

       
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        //-----student name selection change
        $scope.onchange = function (pasR_Id) {
            
            var data = {
                "pasr_id": pasR_Id,
            }
            apiService.create("BuspassForm/getstudata", data).
               then(function (promise) {
                   
                   $scope.studetailslst = promise.studetailslist;
                 

                 
                   //  $scope.obj.pasR_PerStreet = $scope.studetailslst[0].PASR_PerStreet;
                   $scope.obj.pasR_FirstName = $scope.studetailslst[0].PASR_FirstName;
                   $scope.obj.pasR_PerArea = $scope.studetailslst[0].PASR_ConArea;
                   $scope.obj.pasR_PerStreet = $scope.studetailslst[0].PASR_ConStreet;
                   $scope.obj.pasR_Bloodgroup = $scope.studetailslst[0].PASR_BloodGroup;
                   $scope.obj.amsT_PerCity = $scope.studetailslst[0].PASR_ConCity;
                   $scope.obj.pasR_PerPincode = $scope.studetailslst[0].PASR_ConPincode;
                   $scope.obj.pasR_FatherName = $scope.studetailslst[0].PASR_FatherName;
                   $scope.obj.pasR_FatherMobleNo = $scope.studetailslst[0].PASR_FatherMobleNo;
                   $scope.obj.pasR_MotherMobleNo = $scope.studetailslst[0].PASR_MotherMobleNo;
                   $scope.obj.pasR_emailId = $scope.studetailslst[0].PASR_emailId;
                   $scope.obj.pasR_FatherHomePhNo = $scope.studetailslst[0].PASR_FatherHomePhNo;
                   $scope.obj.pasR_FatherOfficePhNo = $scope.studetailslst[0].PASR_FatherOfficePhNo;
                   $scope.obj.IVRMMC_CountryName = $scope.studetailslst[0].IVRMMC_CountryName;
                   $scope.obj.IVRMMS_Name = $scope.studetailslst[0].IVRMMS_Name;
                   $scope.obj.asmcL_ClassName = $scope.studetailslst[0].ASMCL_ClassName;

                   $scope.studentpercountryl = promise.studentpercountry
                   for (var t = 0; t < $scope.studetailslst.length; t++) {
                       if ($scope.studetailslst[t].IVRMMC_Id == $scope.studentpercountryl[0].IVRMMC_Id) {
                           $scope.IVRMMC_Id = $scope.studentpercountryl[t].IVRMMC_Id;
                       }
                   }

                   $scope.studentconstatel = promise.studentconstate
                   for (var t = 0; t < $scope.studetailslst.length; t++) {
                       if ($scope.studetailslst[t].IVRMMS_Id == $scope.studentconstatel[0].IVRMMS_Id) {
                           $scope.IVRMMS_Id = $scope.studentconstatel[t].IVRMMS_Id;
                       }
                   }

               })
        }

        //payment for aggregtor
        //
        $scope.pamentsave = function () {
            
            var payu_URL = $scope.payu_URL;
            var url = payu_URL;
            var method = 'POST';
            var params = {
                "key": $scope.key,
                "txnid": $scope.txnid,
                "amount": $scope.amount,
                "productinfo": $scope.productinfo,
                "firstname": $scope.firstname,
                "email": $scope.email,
                "phone": $scope.phone,
                "udf1": $scope.udf1,
                "udf2": $scope.udf2,
                "udf3": $scope.udf3,
                "udf4": $scope.udf4,
                "udf5": $scope.udf5,
                "udf6": $scope.udf6,
                "service_provider": $scope.service_provider,
                "hash": $scope.hash,
                "surl": "http://localhost:57606/api/Buspassform/paymentresponse/",
                "furl": "http://localhost:57606/api/Buspassform/paymentresponse/"

            //    "surl": "http://localhost:57606/api/StudentApplication/paymentresponse/",
            //"furl": "http://localhost:57606/api/StudentApplication/paymentresponse/"
            }
            FormSubmitter.submit(url, method, params);
        }


        //----- on onform click
        $scope.onformclick = function (pasR_Id) {
            
            var data = {
                "pasr_id": pasR_Id,
            }
            apiService.create("BuspassForm/getbuspassdata", data).
               then(function (promise) {
                   
                   $scope.buspassdatalst = promise.buspassdatalist;
                   $scope.obj.pasR_FirstName = $scope.buspassdatalst[0].PASR_FirstName;
                   $scope.obj.pasR_FatherName = $scope.buspassdatalst[0].PASR_FatherName;
                   $scope.obj.asmcL_ClassName = $scope.buspassdatalst[0].ASMCL_ClassName;
                   $scope.obj.trmR_RouteName = $scope.buspassdatalst[0].TRMR_RouteName;
                   $scope.obj.pasr_BloodGroup = $scope.buspassdatalst[0].PASR_BloodGroup;
                   $scope.obj.createdDate = $scope.buspassdatalst[0].CreatedDate;
                   $scope.obj.trmL_LocationName = $scope.buspassdatalst[0].TRML_LocationName;
                   $scope.obj.pasR_FatherOfficePhNo = $scope.buspassdatalst[0].PASR_FatherOfficePhNo;
                   $scope.obj.pasR_FatherHomePhNo = $scope.buspassdatalst[0].PASR_FatherHomePhNo;
                   $scope.obj.pasR_FatherMobleNo = $scope.buspassdatalst[0].PASR_FatherMobleNo;
                   $scope.obj.pasR_MotherMobleNo = $scope.buspassdatalst[0].PASR_MotherMobleNo;
                   $scope.obj.pasR_emailId = $scope.buspassdatalst[0].PASR_emailId;
                   //------------Address
                   $scope.obj.pasR_PerStreet = $scope.buspassdatalst[0].PASR_PerStreet;
                   $scope.obj.pasR_PerArea = $scope.buspassdatalst[0].PASR_PerArea;
                   $scope.obj.pasR_PerCity = $scope.buspassdatalst[0].PASR_PerCity;
                   $scope.obj.ivrmmS_Name = $scope.buspassdatalst[0].IVRMMS_Name;

                   $scope.obj.ivrmmC_CountryName = $scope.buspassdatalst[0].IVRMMC_CountryName;
                   $scope.obj.pasR_PerPincode = $scope.buspassdatalst[0].PASR_PerPincode;

               })
        }

        //--Area selection change 
        $scope.onareachange = function (trmA_Id) {
            
            var data = {
                "TRMA_Id": $scope.trmA_Id,
            }
            apiService.create("BuspassForm/getroutedata", data).
               then(function (promise) {
                   
                   $scope.routelst = promise.routelist;

               })
        }

        //--Route selection change 
        $scope.onroutechange = function (trmR_Id) {
            
            var data = {
                "TRMR_Id": $scope.trmR_Id,
                "TRMA_Id": $scope.trmA_Id
            }
            apiService.create("BuspassForm/getlocationdata", data).
               then(function (promise) {
                   
                   $scope.pdlocation = true;
                   $scope.locationlst = promise.locationlist;
               })
        }

        //---------SAVE         
        $scope.submitted = false;
        $scope.savedata = function () {
            
            if ($scope.myForm.$valid) {
                var data = {
                    "PASR_Id": $scope.pasR_Id,
                    "TRMA_Id": $scope.trmA_Id,
                    "TRMR_Id": $scope.trmR_Id,
                    "TRML_Idp": $scope.trmL_Idp,
                    "TRML_Idd": $scope.trmL_Idd,
                    configurationsettings: $scope.configurationsettings,
                    transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    },
                }
                apiService.create("BuspassForm/savedata", data).then(function (promise) {

                    
                    if (promise.returnval == true) {
                        swal("Record Saved Successfully");
                        $state.reload();
                    }
                    else if (promise.message == "Duplicate") {
                        swal("Record Updated Successfully");
                        $state.reload();
                    }
                    else if (promise.returnval == false) {
                        swal("Failed to Save/Update");
                    }
                })
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.Back = function () {

            $scope.PaymentMode = false;
            $scope.ProspectuseScreen = true;

            // $scope.cancel();
            $state.reload();


        }

        //payment
        $scope.paynow = function (pasR_Id) {



            // $scope.submitted = true;
            var data = {
                "pasr_Id": pasR_Id,
                configurationsettings: $scope.configurationsettings,
                transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign
            }
            apiService.create("BuspassForm/paynow", data).then(function (promise) {

                if (promise.paydet != null) {

                    $scope.reg.PASR_FirstName = promise.paydet[0].firstname;
                    $scope.reg.PASR_emailId = promise.paydet[0].email;
                    $scope.reg.PASR_MobileNo = promise.paydet[0].phone;
                    $scope.reg.Pasr_amount = promise.paydet[0].amount;

                    $scope.key = promise.paydet[0].marchanT_ID;
                    $scope.txnid = promise.paydet[0].trans_id;
                    $scope.amount = promise.paydet[0].amount;
                    $scope.productinfo = promise.paydet[0].productinfo;
                    $scope.firstname = promise.paydet[0].firstname;
                    $scope.email = promise.paydet[0].email;
                    $scope.phone = promise.paydet[0].phone;
                    $scope.surl = promise.paydet[0].surl;
                    $scope.furl = promise.paydet[0].furl;
                    $scope.hash = promise.paydet[0].hash;
                    $scope.udf1 = promise.paydet[0].udf1;
                    $scope.udf2 = promise.paydet[0].udf2;
                    $scope.udf3 = promise.paydet[0].udf3;
                    $scope.udf4 = promise.paydet[0].udf4;
                    $scope.udf5 = promise.paydet[0].udf5;
                    $scope.udf6 = promise.paydet[0].udf6;
                    $scope.service_provider = promise.paydet[0].service_provider;

                    $scope.hash_string = promise.paydet[0].hash_string;
                    $scope.payu_URL = promise.paydet[0].payu_URL;





                    $scope.PaymentMode = true;
                    $scope.ProspectuseScreen = false;
                    if ($scope.myTabIndex == 1) {
                        $scope.myTabIndex = $scope.myTabIndex - 1;
                    }
                    swal.close();
                    showConfirmButton: false
                }
                else {
                    swal('Registered Successfully,But Payment gateway details are not mapped to institute', 'Contact Administrator..!!');
                    $state.reload();
                }

            });
        }


        //Clear
        $scope.clear = function () {
            $scope.submitted = false;
            $scope.pasR_Id = "";
            $scope.obj.amsT_BloodGroup = "";
            $scope.obj.pasR_FatherName = "";
            $scope.obj.asmcL_ClassName = "";
            $scope.obj.pasR_emailId = "";
            $scope.obj.pasR_FatherMobleNo = "";
            $scope.obj.pasR_MotherMobleNo = "";
            $scope.trmA_Id = "";
            $scope.trmR_Id = "";
            $scope.trmL_Idp = "";
            $scope.trmL_Idd = "";
            $scope.plocation = false;
            $scope.dlocation = false;
            $scope.pdlocation = false;
            $scope.obj.pasR_PerStreet = "";
            $scope.obj.pasR_PerArea = "";
            $scope.ivrmmC_Id = "";
            $scope.ivrmmS_Id = "";
            $scope.obj.amsT_PerCity = "";
            $scope.obj.pasR_PerPincode = "";
            $scope.obj.pasR_FatherOfficePhNo = "";
            $scope.obj.pasR_FatherHomePhNo = "";
        }

        //Cancel
        $scope.cancel = function () {
            $state.reload();
        }

    };
})();