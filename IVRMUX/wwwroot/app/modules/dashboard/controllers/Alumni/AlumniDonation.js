
(function () {
    'use strict';
    angular
        .module('app')
        .controller('AlumniDonation', AlumniDonation)

    AlumniDonation.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function AlumniDonation($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }
        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        for (var i = 0; i < transnumconfigsettings.length; i++) {
            if (transnumconfigsettings.length > 0) {
                if (transnumconfigsettings[i].imN_Flag == "AlumniDonation") {
                    $scope.transnumbconfigurationsettingsassign = transnumconfigsettings[i];
                }
            }
        }
        $scope.imgname = logopath;
        $scope.todatedate = new Date();

        $scope.loaddata = function () {

            var id = 1;

            apiService.getURI("AlumniDonation/Pageload", id).then(function (promise) {

                $scope.ALSREG_Id = "";
                $scope.almsT_FirstName = promise.almdetails[0].almsT_FirstName;
                $scope.imagenew = promise.imagenew;
                $scope.leftyear = promise.almdetails[0].leftyear;
                $scope.classleft = promise.almdetails[0].classleft;
                $scope.almsT_RegistrationNo = promise.almdetails[0].almsT_RegistrationNo;
                $scope.almsT_AdmNo = promise.almdetails[0].almsT_AdmNo;
                $scope.almsT_MobileNo = promise.almdetails[0].almsT_MobileNo;
                $scope.almsT_emailId = promise.almdetails[0].almsT_emailId;
                $('#blah').attr('src', promise.almdetails[0].almsT_StudentPhoto);
                $scope.ALMST_Id = promise.almdetails[0].almsT_Id;
                $scope.ALSREG_Id = promise.almdetails[0].alsreG_Id;

                $scope.ALMST_ConStreet = promise.almdetails[0].almsT_ConStreet;
                $scope.ALMST_ConArea = promise.almdetails[0].almsT_ConArea;
                $scope.ALMST_ConCity = promise.almdetails[0].almsT_ConCity;
                $scope.ALMST_District = promise.almdetails[0].almsT_District;
                $scope.IVRMMS_Name = promise.almdetails[0].ivrmmS_Name;
                $scope.IVRMMC_CountryName = promise.almdetails[0].ivrmmC_CountryName;
                $scope.ALMST_ConPincode = promise.almdetails[0].almsT_ConPincode;
                $scope.ALMST_StudentPANCard = promise.almdetails[0].almsT_StudentPANCard;
                $scope.ALMST_FatherName = promise.almdetails[0].almsT_FatherName;
                if ($scope.ALMST_StudentPANCard != null) {
                    $scope.ngstatus1 = false;
                    $scope.ngstatus = true;
                }
                else {
                    $scope.ngstatus1 = true;
                }

                $scope.arrlist7 = promise.fillalumnidonationdetails;
                $scope.paymenttest = promise.fillpaymentgateway;
                $scope.paymenttestnew = promise.fillpaymentgateway;


            })
        };

        //============ radio ngchange
        $scope.nricheck = function () {
            if ($scope.NRI_status == 1) {
                $scope.ngstatus = true;
            }
            else {
                $scope.ngstatus = false;
            }
        }

        //--------------------Get amount----------
        $scope.getamount = function (ALMDON_Id, gg) {
            angular.forEach($scope.arrlist7, function (qq) {
                if (qq.almdoN_Id == ALMDON_Id) {
                    $scope.Donationname = qq.almdoN_DonationName;
                }
            })
            var data = {
                "ALMDON_Id": ALMDON_Id
            }
            apiService.create("AlumniDonation/getamount", data).then(function (promise) {
                if (promise.getamountlist != null || promise.getamountlist.length > 0) {
                    if ($scope.ALMDON_Id > 0) {
                        $scope.ALMDON_Amount = promise.getamountlist[0].almdoN_Amount;
                        $scope.Amount = promise.getamountlist[0].almdoN_Amount;
                        $scope.ALMDON_Id = promise.getamountlist[0].almdoN_Id;
                        $scope.dname = promise.getamountlist[0].almdoN_DonationName;
                        $scope.regfee = promise.getamountlist[0].almdoN_RegistrationFeeFlag;


                    }
                    else {

                        $scope.ALMDON_Amount = "";
                        $scope.Amount = "";
                        $scope.ALMDON_Id = "";
                    }

                }
                else {
                    swal('No Data Found');
                }
            });
        }


        $scope.cleardata = function () {
            $state.reload();
        };

        $scope.checkamount = function () {
            $scope.paymenttest1 = [];
            $scope.paymenttest1 = $scope.paymenttestnew;
            if ($scope.ALMDON_Amount < $scope.Amount) {
                swal('Donation Amount should be Greater Than ' + $scope.Amount + '')
                //$state.reload();
                $scope.paymenttest = [];

            }
            else {
                $scope.paymenttest = $scope.paymenttest1;
            }
        }
        $scope.Clearid = function () {
            $state.reload();
        }
        //=================payment radio button===========
        $scope.onclickloaddata = function (payobj) {

            var data = {
                "ALMDON_Amount": $scope.ALMDON_Amount,
                "ALMST_FirstName": $scope.almsT_FirstName,
                "FPGD_PGName": payobj,
                "ALMST_Id": $scope.ALMST_Id,


            }
            apiService.create("AlumniDonation/getpayment_details", data).then(function (promise) {
                if (promise.paymentgateway.length > 0 || promise.paymentgateway != null) {
                    $scope.paymentgateway = promise.paymentgateway
                    $scope.ALMDON_Amount = $scope.ALMDON_Amount;
                    $scope.SaltKey = $scope.paymentgateway[0].fpgD_SaltKey;
                    $scope.institutioname = "vaps";
                    $scope.orderid = promise.orderId;
                    $scope.paygtw = payobj;
                    $scope.institutioname = promise.institution[0].mI_Name;
                    $scope.panmi = promise.institution[0].mI_PAN;
                    $scope.logo = promise.institution[0].mI_Logo;
                    $scope.wordamount = toWordsconver($scope.ALMDON_Amount);

                }
            })
        }



        $scope.razorpay = function () {
            if ($scope.ALMDON_Amount < $scope.Amount) {
                swal('Donation Amount should be Greater Than ' + $scope.Amount + '')
                //$state.reload();


            }
            else {
                $scope.StudentPANCard = "";
                if ($scope.ALMST_StudentPANCard != null || $scope.ALMST_StudentPANCard != undefined) {
                    $scope.StudentPANCard = $scope.ALMST_StudentPANCard;
                }
                else {
                    $scope.StudentPANCard = undefined;
                }

                var options = {
                    "key": $scope.SaltKey,
                    "amount": $scope.ALMDON_Amount * 100,
                    //"amount": 200,
                     "name": $scope.institutioname,
                    "order_id": $scope.orderid,
                    "currency": "INR",
                    "description": "ONLINE PAYMENT",
                    "image": $scope.imgname,
                    "handler": function (response) {
                        $scope.paymentid = response.razorpay_payment_id;


                      

                        if ($scope.NRI_status == 1) {
                            $scope.ALDON_NRIFlg = true;
                        }
                        else {
                            $scope.ALDON_NRIFlg = false;
                        }
                        var Template = document.getElementById("alumnidonation").innerHTML;
                        if (Template != null || Template != undefined) {
                            $scope.Template = Template;
                        }
                        else {
                            $scope.Template = undefined
                        }
                        var data = {

                            "ReceiptNo": response.razorpay_payment_id,
                            "ALDON_Date": new Date(),
                            "ALDON_Amount": $scope.ALMDON_Amount,
                            "ALDON_DonorName": $scope.almsT_FirstName,
                            "OrderId": $scope.orderid,
                            "ALSREG_Id": $scope.ALSREG_Id,
                            "ALMDON_Id": $scope.ALMDON_Id,
                            "ALMST_StudentPANCard": $scope.StudentPANCard,
                            "ALMST_Id": $scope.ALMST_Id,
                            "Template": $scope.Template,
                            "ALDON_NRIFlg": $scope.ALDON_NRIFlg,
                            "ALMDON_RegistrationFeeFlag": $scope.regfee, 
                            transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign,
                           


                        };
                        apiService.create("AlumniDonation/paymentsave", data).then(function (promise) {
                            if (promise.returnval == "true") {
                                if ($scope.regfee == true) {
                                    swal({
                                        title: "Confirmed!",
                                        text: "Your Registration Payment Confirmed",
                                        type: "success"
                                    })
                                }
                                else {
                                    swal({
                                        title: "Confirmed!",
                                        text: "Your Donation Payment Confirmed",
                                        type: "success"
                                    })
                                }
                               
                            }
                            else {
                                swal("sorry!", "Your Donation Payment is not Confirmed.", "Error");
                            }
                            $state.reload();
                        })

                    },


                    "prefill": {
                        "name": $scope.almsT_FirstName,
                        "email": $scope.almsT_emailId,
                        "contact": $scope.almsT_MobileNo
                    },
                    //"notes": {
                    //    "notes_1": $scope.almsT_FirstName,
                    //    "notes_2": "a",
                    //    "notes_3": $scope.almsT_emailId,
                    //    "notes_4": "Mysore",
                    //    "notes_5": "123",
                    //    "notes_6": "123",
                    //    "notes_7": "4",
                    //    "notes_8": "2",
                    //    "notes_9": "123"
                    //},
                    "theme": {
                        "color": "#F37254"
                    },

                };

                var rzp1 = new Razorpay(options);
                rzp1.open();
                e.preventDefault();

            }
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };



        var th_val = ['', 'Thousand', 'Million', 'Billion', 'Trillion'];
        var dg_val = ['Zero', 'One', 'Two', 'Three', 'Four', 'Five', 'Six', 'Seven', 'Eight', 'Nine'];
        var tn_val = ['Ten', 'Eleven', 'Twelve', 'Thirteen', 'Fourteen', 'Fifteen', 'Sixteen', 'Seventeen', 'Eighteen', 'Nineteen'];
        var tw_val = ['Twenty', 'Thirty', 'Forty', 'Fifty', 'Sixty', 'Seventy', 'Eighty', 'Ninety'];
        function toWordsconver(s) {
            s = s.toString();
            s = s.replace(/[\, ]/g, '');
            if (s != parseFloat(s))
                return 'not a number ';
            var x_val = s.indexOf('.');
            if (x_val == -1)
                x_val = s.length;
            if (x_val > 15)
                return 'too big';
            var n_val = s.split('');
            var str_val = '';
            var sk_val = 0;
            for (var i = 0; i < x_val; i++) {
                if ((x_val - i) % 3 == 2) {
                    if (n_val[i] == '1') {
                        str_val += tn_val[Number(n_val[i + 1])] + ' ';
                        i++;
                        sk_val = 1;
                    } else if (n_val[i] != 0) {
                        str_val += tw_val[n_val[i] - 2] + ' ';
                        sk_val = 1;
                    }
                } else if (n_val[i] != 0) {
                    str_val += dg_val[n_val[i]] + ' ';
                    if ((x_val - i) % 3 == 0)
                        str_val += 'hundred ';
                    sk_val = 1;
                }
                if ((x_val - i) % 3 == 1) {
                    if (sk_val)
                        str_val += th_val[(x_val - i - 1) / 3] + ' ';
                    sk_val = 0;
                }
            }
            if (x_val != s.length) {
                var y_val = s.length;
                str_val += 'point ';
                for (var i = x_val + 1; i < y_val; i++)
                    str_val += dg_val[n_val[i]] + ' ';
            }
            return str_val.replace(/\s+/g, ' ');
        }
        //==================== end=============

    }
})();





