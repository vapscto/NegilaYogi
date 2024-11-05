(function () {
    'use strict';
    angular
        .module('app')
        .controller('StudentApplicationController', StudentApplicationController)

    StudentApplicationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache', '$compile', '$sce']
    function StudentApplicationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache, $compile, $sce) {

        $('.modal').on('hide.bs.modal', function (e) {
            e.stopPropagation();
            $('body').css('padding-right', '');
        });
        $('body').on('hidden.bs.modal', function () {
            if ($('.modal.in').length > 0) {
                $('body').addClass('modal-open');
            }
        });
        $scope.formfilactive = false;
        $scope.nextpayment = true;
        $scope.updatedata = false;
        $scope.disbalehealth = false;
        $scope.showrepatsib = true;
        $scope.qwe = {};
        var HostName = location.host;
        $scope.xyz = " ";
        $scope.underoverage = "";
        $scope.applicationform = "hutchings";
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings != null && admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.allSources = [];
        $scope.allRefrence = [];
        $scope.prospectusshow = false;
        var org = 0;
        var intd = 0;
        $scope.showstream = false;
        $scope.concessioncon = false;
        $scope.concessionconfirmsibling = false;
        $scope.photoupload_flag = 'Default';

        $scope.WebcamGenrate = function () {
            $scope.photoupload_flag = 'Webcam';
            $scope.ImagePath = "";
            $scope.wbcamurl = "";
            Webcam.snap;
            Webcam.snap(function (data_uri) {
                $scope.wbcamurl = data_uri;
            });


            if ($scope.wbcamurl != "") {
                $scope.reg.PASR_Student_Pic_Path = $scope.wbcamurl;
            }
            $scope.selectFileforUploadz($scope.wbcamurl);
        }

        $scope.showpdf = false;

        $scope.backtoview = function () {
            $scope.showpdf = false;
        };

        //$scope.downloadview = function (pdfview) {
        //    $scope.pdfurl = pdfview;
        //    $scope.showpdf = true;
        //    $('#showpdf').modal('show');

        //};

        $scope.downloadview = function (pdfview) {

            $('#showpdf').modal('hide');
            var imagedownload1 = "";
            imagedownload1 = pdfview;

            $http.get(imagedownload1, { responseType: 'arraybuffer' })
                .success(function (response) {
                    var fileURL = "";
                    var file = "";
                    var embed = "";
                    var pdfId = "";
                    file = new Blob([(response)], { type: 'application/pdf' });
                    fileURL = URL.createObjectURL(file);

                    pdfId = document.getElementById("pdfIdzz");
                    pdfId.removeChild(pdfId.childNodes[0]);
                    embed = document.createElement('embed');
                    embed.setAttribute('src', fileURL);
                    embed.setAttribute('type', 'application/pdf');
                    embed.setAttribute('width', '100%');
                    embed.setAttribute('height', '1000');
                    pdfId.appendChild(embed);
                    $('#showpdf').modal('show');
                });
        }

        $scope.showsingle = false;
        $scope.showdouble = false;


        $scope.shogwgrid = true;

        $scope.imgnameload = logopath;

        $scope.getAge = function (birthday) {
            $scope.year_age = "";
            var mdate = $filter('date')(birthday, 'yyyy-MM-dd');
            var yearThen = parseInt(mdate.substring(0, 4), 10);
            var monthThen = parseInt(mdate.substring(5, 7), 10);
            var dayThen = parseInt(mdate.substring(8, 10), 10);

            var age = Math.floor(age);
            var today = new Date();
            birthday = new Date(yearThen, monthThen - 1, dayThen);
            var differenceInMilisecond = today.valueOf() - birthday.valueOf();
            var year_age = Math.floor(differenceInMilisecond / 31536000000);
            var day_age = Math.floor((differenceInMilisecond % 31536000000) / 86400000);
            var month_age = Math.floor(day_age / 30);
            day_age = day_age % 30;
            $scope.year_age = year_age;
            $scope.month_age = month_age;
        };

        $scope.uppercase = function (str) {
            var array1 = str.split(' ');
            var newarray1 = [];

            for (var x = 0; x < array1.length; x++) {
                newarray1.push(array1[x].charAt(0).toUpperCase() + array1[x].slice(1).toLowerCase());
            }
            $scope.camelcase = newarray1;
            return $scope.camelcase = newarray1.join(' ');
        };

        $scope.reg = {};
        $scope.obj = {};

        var monthname;
        var datename;
        var yearname;
        var adminyesno;

        var configsettings = JSON.parse(localStorage.getItem("configsettings"));

        if (configsettings != null && configsettings.length > 0) {
            var cutoffdate = configsettings[0].ispaC_ApplCutOffDateFlag;

            var maxage = configsettings[0].ispaC_DOBMaxAgeFlag;
            var minage = configsettings[0].ispaC_DOBMinAgeFlag;

            var offlinefee = configsettings[0].ispaC_OfflineFee_Flag;
            if (offlinefee == 1) {
                $scope.offlinefee = true;
            }
            else {
                $scope.result = 1;
                $scope.offlinefee = false;
            }

        }
        else {
            cutoffdate = 0;
            maxage = 0;
            minage = 0;
        }
        $scope.myDate = new Date();

        $scope.maxDatedob = new Date(
            $scope.myDate.getFullYear(),
            $scope.myDate.getMonth(),
            $scope.myDate.getDate());


        $scope.myTabIndex = 0;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        if (transnumconfigsettings != null && transnumconfigsettings != undefined && transnumconfigsettings.length > 0) {
            for (var i = 0; i < transnumconfigsettings.length; i++) {
                if (transnumconfigsettings.length > 0) {
                    if (transnumconfigsettings[i].imN_Flag == "Online") {
                        $scope.transnumbconfigurationsettingsassign = transnumconfigsettings[i];
                    }
                }
            }
        }

        $scope.clickimage = function (radioimage) {
            angular.forEach($scope.paymenttest, function (objectt) {
                if (objectt.fpgD_Image == radioimage) {
                    $scope.qwe.paygtw = objectt.fpgD_PGName;
                    $scope.onclickloaddata(objectt.fpgD_PGName, $scope.result);
                    return;
                }
            });
        };

        //Student Search Dropdown
        $scope.searchfilter = function (objj) {

            if (objj.search.length >= '1') {
                $scope.admsudentslist = $scope.prospectusDlist;
                angular.forEach($scope.admsudentslist, function (objectt) {
                    if (objectt.pasP_ProspectusNo.length > 0) {
                        var string = objectt.pasP_ProspectusNo;
                        objectt.pasP_ProspectusNo = string.replace(/  +/g, ' ');
                    }
                });
            }
            else {
                $scope.admsudentslist = [];
            }
        };

        $scope.countshow = false;

        $scope.electivesubgrouplist = {};

        $scope.sortKey = "pasr_id";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa

        $scope.presentCountgrid = 0;

        var searchObject = $location.search();
        //*****MAXMINAGE****
        //$scope.classdesable = true;
        $scope.classdesable = false;
        // alert(searchObject.status);
        if (searchObject.status == "failure" || searchObject.status == "TXN_FAILURE") {
            swal("Payment Unsuccessful");
            //  Request.QueryString.Remove("status");
            //$location.url($location.path)
            if ($location.$$search.status) {
                delete $location.$$search.status;
                $location.$$compose();
            }
        }
        else if (searchObject.status == "success" || searchObject.status == "TXN_SUCCESS") {
            //swal("Payment successful and Please submit the filled application to the School office");
            swal("Payment Successful..!!");
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


        //payment for aggregtor OLD WITOUT PAY U
        //
        //$scope.pamentsave = function () {

        //    var payu_URL = $scope.payu_URL;
        //    var url = payu_URL;
        //    var method = 'POST';
        //    var params = {
        //        "key": $scope.key,
        //        "txnid": $scope.txnid,
        //        "amount": $scope.amount,
        //        "productinfo": $scope.productinfo,
        //        "firstname": $scope.firstname,
        //        "email": $scope.email,
        //        "phone": $scope.phone,
        //        "udf1": $scope.udf1,
        //        "udf2": $scope.udf2,
        //        "udf3": $scope.udf3,
        //        "udf4": $scope.udf4,
        //        "udf5": $scope.udf5,
        //        "udf6": $scope.udf6,
        //        "service_provider": $scope.service_provider,
        //        "hash": $scope.hash,
        //        "surl": "https://vapsecampus.azurewebsites.net/api/StudentApplication/paymentresponse/",
        //        "furl": "https://vapsecampus.azurewebsites.net/api/StudentApplication/paymentresponse/"
        //    }
        //    FormSubmitter.submit(url, method, params);
        //}

        //payment for aggregtor with paytm
        //
        $scope.pamentsave = function (qwe, offon) {
            // $scope.submitted = true;
            var data = {
                "pasR_Id": $scope.pasR_Id,
                "offon": offon,
                configurationsettings: $scope.configurationsettings,
                transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign,
                "onlinepaygteway": qwe.paygtw
            };
            apiService.create("StudentApplication/paynow", data).then(function (promise) {
                if ($scope.result === 1) {
                    if (promise.paydet != null) {

                        if (qwe.paygtw === 'PAYU') {
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
                                "surl": "https://vapsecampus.azurewebsites.net/api/StudentApplication/paymentresponse/",
                                "furl": "https://vapsecampus.azurewebsites.net/api/StudentApplication/paymentresponse/"
                            }
                            FormSubmitter.submit(url, method, params);

                        }
                        else if (qwe.paygtw === 'PAYTM') {
                            $scope.MID = promise.paydet[0].mid;
                            $scope.ORDER_ID = promise.paydet[0].ordeR_ID;
                            $scope.CUST_ID = promise.paydet[0].cusT_ID;
                            $scope.TXN_AMOUNT = promise.paydet[0].txN_AMOUNT;
                            $scope.CHANNEL_ID = promise.paydet[0].channeL_ID;
                            $scope.EMAIL = promise.paydet[0].email;
                            $scope.MOBILE_NO = promise.paydet[0].mobilE_NO;
                            $scope.INDUSTRY_TYPE_ID = promise.paydet[0].industrY_TYPE_ID;
                            $scope.WEBSITE = promise.paydet[0].website;
                            $scope.CHECKSUMHASH = promise.paydet[0].checksumhash;
                            $scope.MERC_UNQ_REF = promise.paydet[0].merC_UNQ_REF;

                            $scope.payu_URL = promise.paydet[0].payu_URL;
                            payu_URL = $scope.payu_URL;
                            url = payu_URL;
                            method = 'POST';
                            params = {
                                "MID": $scope.MID,
                                "ORDER_ID": $scope.ORDER_ID,
                                "CUST_ID": $scope.CUST_ID,
                                "TXN_AMOUNT": $scope.TXN_AMOUNT,
                                "CHANNEL_ID": $scope.CHANNEL_ID,
                                "EMAIL": $scope.EMAIL,
                                "MOBILE_NO": $scope.MOBILE_NO,
                                "INDUSTRY_TYPE_ID": $scope.INDUSTRY_TYPE_ID,
                                "WEBSITE": $scope.WEBSITE,
                                "CHECKSUMHASH": $scope.CHECKSUMHASH,
                                "MERC_UNQ_REF": $scope.MERC_UNQ_REF,
                                //"CALLBACK_URL": "https://secure.paytm.in/oltp-web/invoiceResponse",
                                "CALLBACK_URL": "https://vapsecampus.azurewebsites.net/api/StudentApplication/paymentresponsepaytm/",
                            }
                            FormSubmitter.submit(url, method, params);
                        }
                        else if (qwe.paygtw == "RAZORPAY") {

                            $scope.txnamt = Number(promise.paydet[0].fmA_Amount) * 100;
                            $scope.SaltKey = promise.paydet[0].ivrmoP_MERCHANT_KEY;
                            $scope.orderid = promise.paydet[0].trans_id;

                            $scope.institutioname = promise.paydet[0].institutioname;
                            $scope.institulogo = promise.paydet[0].institulogo;

                            $scope.stuname = promise.paydet[0].firstname;
                            $scope.stuemailid = promise.paydet[0].email;
                            $scope.stuaddress = promise.paydet[0].stuaddress;
                            $scope.stumobileno = promise.paydet[0].mobile;
                            $scope.stuadmno = promise.paydet[0].pasR_RegistrationNo;
                            $scope.splitpayinfor = promise.paydet[0].splitpayinformation;

                            $scope.mI_Id = promise.mI_Id;
                            $scope.asmaY_Id = promise.asmaY_Id;
                            $scope.amst_Id = promise.paydet[0].pasR_ID;
                        }

                        else if (qwe.paygtw == "EASEBUZZ") {
                            var strdata = promise.multiplegroups;
                            $scope.htmldata = promise.multiplegroups;
                            var e1 = angular.element(document.getElementById("test"));
                            $compile(e1.html($scope.htmldata))(($scope));
                        }
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
                }
                else {
                    swal("Kindly visit school to pay the registration fees before the last date");
                    $state.reload();
                }
            });
        };

        $scope.offlinepayment = function () {
            var data = {
                "pasR_Id": $scope.pasR_Id,
                "offon": 2,
                configurationsettings: $scope.configurationsettings,
                transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign
            };
            apiService.create("StudentApplication/paynow", data).then(function (promise) {
                swal("Kindly visit school to pay the registration fees before the last date.");
                $state.reload();
            });
        };

        $scope.onlineclik = function () {
            if ($scope.online === '1') {

                $scope.pynw = false;
            }
            else {
                $scope.pynw = true;
                swal("Pay Registration Fee In school !!");
            }

        };


        $scope.submitted = false;
        $scope.fillpay1 = function (pasR_Id) {

            $window.location.href = 'https://' + HostName + '/#/app/prospectus/';

        }
        //single account
        //$scope.pamentsave = function () {

        //    var payu_URL = $scope.payu_URL;
        //    var url = payu_URL;
        //    var method = 'POST';
        //    var params = {
        //        "Key": $scope.key,
        //        "txnid": $scope.txnid,
        //        "amount": $scope.amount,
        //        "productinfo": $scope.productinfo,
        //        "firstname": $scope.firstname,
        //        "email": $scope.email,
        //        "phone": $scope.phone,
        //        "surl": $scope.surl,
        //        "furl": $scope.furl,
        //        "hash": $scope.hash,
        //        "udf1": $scope.udf1,
        //        "udf2": $scope.udf2,
        //        "udf3": $scope.udf3,
        //        "udf4": $scope.udf4,
        //        "udf5": $scope.udf5,
        //        "udf6": $scope.udf6,
        //        "service_provider": $scope.service_provider,
        //        "hash_string": $scope.hash_string

        //    }
        //    FormSubmitter.submit(url, method, params);
        //}


        $scope.appcutoffdate = false;
        $scope.ProspectuseScreen = true;
        $scope.paginate2 = "paginate2";
        $scope.currentPage2 = 1;
        $scope.itemsPerPage2 = paginationformasters;

        $scope.currentPage8 = 1;
        $scope.itemsPerPage8 = paginationformasters;

        $scope.currentPage12 = 1;
        $scope.itemsPerPage12 = paginationformasters;

        $scope.currentPage9 = 1;
        $scope.itemsPerPage9 = paginationformasters;


        $scope.itemsPerPages = paginationformasters;
        $scope.currentPages = 1;
        $scope.PreviousresultData = [];

        var maxdatepre;
        var mindatepre;
        $scope.calcage1 = function (dateString) {
            $scope.underoverage = "";
            $scope.reg.ASMST_Id = "";
            var data = {
                "dateString": new Date(dateString).toDateString()
            };
            apiService.create("StudentApplication/classchangemaxminage", data).then(function (promise) {
                maxdatepre = $filter('date')(promise.maxdate, 'dd-MM-yyyy');
                mindatepre = $filter('date')(promise.mindate, 'dd-MM-yyyy');
                //maxdatepre =new Date(promise.maxdate);
                //mindatepre = new Date(promise.mindate);
                if (promise.class_list.length > 0) {
                    $scope.arrlist5 = promise.class_list;
                    $scope.reg.ASMCL_Id = promise.class_list[0].asmcL_Id;
                    $scope.getstreams($scope.reg.ASMCL_Id);
                    //*****MAXMINAGE****
                    $scope.classdesable = false;
                }
                else {
                    $scope.arrlist5 = "";
                    var stringvar = "";
                    $scope.classdesable = true;
                    $scope.clslst2 = promise.class_list2;
                    $('#myModalswal').modal('show');

                }
            });
        };

        $scope.caloverage = function (ASMCL_Id, dateString) {
            $scope.underoverage = "";

            var data = {
                "ASMCL_Id": ASMCL_Id,
                "dateString": new Date(dateString).toDateString(),
                "ASMST_Id": 0
            }
            apiService.create("StudentApplication/classoverunderage", data).then(function (promise) {

                if (promise.message != "" || promise.message != null) {
                    $scope.underoverage = promise.message;
                }


            })
        }

        $scope.caloveragestraem = function (ASMCL_Id, dateString, sylla) {
            if (dateString == undefined || dateString == null) {
                swal("Enter Date of Birth!!");
            }
            else {
                $scope.underoverage = "";
                if ($scope.reg.ASMST_Id == undefined || $scope.reg.ASMST_Id == null) {
                    $scope.reg.ASMST_Id = "";
                }
                var data = {
                    "ASMCL_Id": ASMCL_Id,
                    "dateString": new Date(dateString).toDateString(),
                    "ASMST_Id": sylla
                }
                apiService.create("StudentApplication/classoverunderage", data).then(function (promise) {

                    if (promise.class_list.length > 0) {
                        $scope.arrlist5 = promise.class_list;
                        //*****MAXMINAGE****
                        $scope.classdesable = false;
                        $scope.reg.ASMCL_Id = "";
                    }
                    else {
                        $scope.arrlist5 = "";
                        var stringvar = "";
                        $scope.classdesable = true;
                        $scope.clslst2 = promise.class_list2;
                        $('#myModalswal').modal('show');

                    }


                })
            }

        }


        $scope.GetSubjectsofinstitute = function (ASMCL_Id) {
            if ($scope.reg.ASMST_Id == undefined || $scope.reg.ASMST_Id == null) {
                $scope.reg.ASMST_Id = 0;
            }
            var data = {
                "ASMCL_Id": ASMCL_Id,
                "ASMST_Id": $scope.reg.ASMST_Id
            }
            apiService.create("StudentApplication/GetSubjectsofinstitute", data).then(function (promise) {
                if (promise.electivegrouplist != null) {
                    $scope.electivegrouplist = promise.electivegrouplist;

                    //angular.forEach($scope.electivegrouplist, function (opqr) {
                    //    opqr.subjectid = opqr.ISMS_Id;
                    //}
                    //)


                    $scope.electivesubgrouplist = promise.electivesubgrouplist;
                    if (promise.streamexams != null && promise.streamexams.length > 0) {
                        $scope.streamexams = promise.streamexams;
                    }
                    else {
                        $scope.streamexams = [];
                    }

                    angular.forEach($scope.electivesubgrouplist, function (opqr) {
                        opqr.ismS_Id = "";
                    }
                    )
                    if ($scope.electivesubgrouplist.length >= 1) {
                        //angular.forEach($scope.electivegrouplist, function (opqr) {
                        //    angular.forEach($scope.electivesubgrouplist, function (opqr1) {
                        //        if (opqr.EMG_Id == opqr1.EMG_Id) {
                        //            opqr1.ismS_Id = opqr.ISMS_Id;
                        //        }
                        //    })
                        //})
                        $scope.grouplength = $scope.electivesubgrouplist.length;
                    }
                    else {
                        $scope.electivesubgrouplist = {};
                    }

                    if ($scope.electivesubgrouplist != null && $scope.electivesubgrouplist.length >= 0) {
                        $scope.electives = [];
                        $scope.compulsory = [];
                        angular.forEach($scope.electivesubgrouplist, function (opqr1) {
                            if (opqr1.EMG_ElectiveFlg == false) {
                                $scope.compulsory.push(opqr1);
                            }
                            else {
                                $scope.electives.push(opqr1);
                            }
                        })
                    }
                }
            })
        }

        $scope.getstreams = function (ASMCL_Id) {
            $scope.syllabuslist = {};
            $scope.streamexams = {};
            $scope.electivesubgrouplist = {};
            $scope.electivegrouplist = {};
            $scope.electives = {};
            $scope.compulsory = {};
            $scope.showstream = true;
            var data = {
                "ASMCL_Id": ASMCL_Id
            }
            apiService.create("StudentApplication/getstreams", data).then(function (promise) {
                if ((promise.syllabuslist != null && promise.syllabuslist.length > 0) || (promise.syllabuslistoth != null && promise.syllabuslistoth.length > 0)) {

                    if (promise.syllabuslist.length > 0) {
                        $scope.syllabuslist = promise.syllabuslist;
                        $scope.showstream = true;
                    }
                    else if (promise.syllabuslistoth.length > 0) {
                        $scope.syllabuslist = promise.syllabuslistoth;
                        $scope.showstream = false;
                        angular.forEach($scope.syllabuslist, function (opqr1) {
                            $scope.selectedsy = promise.syllabuslistoth[0].asmsT_Id;
                            $scope.reg.ASMST_Id = promise.syllabuslistoth[0].asmsT_Id;
                        })
                    }

                }
                else {
                    $scope.syllabuslist = {};
                    $scope.streamexams = {};
                    $scope.electivesubgrouplist = {};
                    $scope.electivegrouplist = {};
                    $scope.electives = {};
                    $scope.compulsory = {};
                    $scope.showstream = true;
                    //if (promise.message != null && promise.message != "") {
                    //    $scope.applastdatedisable = true;
                    //    swal(promise.message);                       
                    //}
                    //else {
                    //    $scope.applastdatedisable = false;
                    //}
                }
            })
        }

        $scope.valid_certificate = function (IMC_Id) {

            angular.forEach($scope.caste_doc_map_list, function (opqr) {
                angular.forEach($scope.documentList, function (opqr1) {
                    if (opqr1.amsmD_Id == opqr.amsmD_Id) {
                        opqr1.amsmD_FLAG = false;
                    }
                })

            })

            angular.forEach($scope.caste_doc_map_list, function (opqr) {
                if (opqr.imC_Id == IMC_Id) {
                    angular.forEach($scope.documentList, function (opqr1) {
                        if (opqr1.amsmD_Id == opqr.amsmD_Id) {
                            opqr1.amsmD_FLAG = true;
                        }
                    })
                }
            })


            //var imc =IMC_Id;
            //apiService.getURI("StudentApplication/getdpstatesubcatse", imc).then(function (promise) {
            //    if (promise.subcastedrp.length > 0) {
            //        $scope.arrlistsub = promise.subcastedrp;
            //    }
            //    else {
            //        $scope.arrlistsub ="";
            //    }
            //})

        }

        $scope.valid_certificatef = function (IMC_Id) {

            var imc = IMC_Id;
            apiService.getURI("StudentApplication/getdpstatesubcatsefather", imc).then(function (promise) {
                if (promise.subcastedrpf.length > 0) {
                    $scope.arrlistsubf = promise.subcastedrpf;
                }
                else {
                    $scope.arrlistsubf = "";
                }
            })

        }

        $scope.valid_certificatem = function (IMC_Id) {

            var imc = IMC_Id;
            apiService.getURI("StudentApplication/getdpstatesubcatsemother", imc).then(function (promise) {
                if (promise.subcastedrpm.length > 0) {
                    $scope.arrlistsubm = promise.subcastedrpm;
                }
                else {
                    $scope.arrlistsubm = "";
                }
            })

        }

        $scope.calcage = function (dateString) {


            var now = new Date();
            var today = new Date(now.getYear(), now.getMonth(), now.getDate());

            var yearNow = now.getYear();
            var monthNow = now.getMonth();
            var dateNow = now.getDate();
            var doob = new Date(reg.PASR_DOB);
            var dob = new Date(doob.getYear(6, 10),
                doob.getMonth(0, 2) - 1,
                doob.getDate(3, 5)
            );

            var yearDob = doob.getYear();
            var monthDob = doob.getMonth();
            var dateDob = doob.getDate();
            var age = {};
            var ageString = "";
            var yearString = "";
            var monthString = "";
            var dayString = "";

            var yearAge = "";
            yearAge = yearNow - yearDob;

            if (monthNow >= monthDob)
                var monthAge = monthNow - monthDob;
            else {
                yearAge--;
                var monthAge = 12 + monthNow - monthDob;
            }

            if (dateNow >= dateDob)
                var dateAge = dateNow - dateDob;
            else {
                monthAge--;
                var dateAge = 31 + dateNow - dateDob;

                if (monthAge < 0) {
                    monthAge = 11;
                    yearAge--;
                }
            }

            age = {
                years: yearAge,
                months: monthAge,
                days: dateAge
            };

            // swal(ageString);
            $scope.reg.PASR_Age = age.years;
            if (minage == 1) {
                if (age.years > minageeyear) {
                }
                else if (age.years == minageeyear) {
                    if (age.months > minageemonth) {

                        if (age.days >= minageedays) {

                        }
                        else {
                            swal("Less than " + minageeyear + " year", 'Sorry You are not Eligible');
                            $scope.reg.PASR_Age = "";
                            $scope.reg.PASR_DOB = "";
                            return;
                        }
                    }
                    else if (age.months == minageemonth) {
                        if (age.days >= minageedays) {

                        }
                        else {
                            swal("Less than " + minageeyear + " year", 'Sorry You are not Eligible');
                            $scope.reg.PASR_Age = "";
                            $scope.reg.PASR_DOB = "";
                            return;
                        }
                    }
                    else {
                        swal("Less than " + minageeyear + " year", 'Sorry You are not Eligible');
                        $scope.reg.PASR_Age = "";
                        $scope.reg.PASR_DOB = "";
                        return;
                    }
                }
                else {
                    swal("Less than " + minageeyear + " year", 'Sorry You are not Eligible');
                    $scope.reg.PASR_Age = "";
                    $scope.reg.PASR_DOB = "";
                    return;
                }
            }

            if (maxage == 1) {
                if (age.years < maxageeyear) {
                }
                else if (age.years == maxageeyear) {
                    if (age.months < maxageemonth) {

                        if (age.days <= maxageedays) {

                        }
                        else {
                            swal("Greater than " + maxageeyear + " year", 'Sorry You are not Eligible');
                            $scope.reg.PASR_Age = "";
                            $scope.reg.PASR_DOB = "";
                            return;
                        }
                    }
                    else if (age.months == maxageemonth) {
                        if (age.days <= maxageedays) {

                        }
                        else {
                            swal("Greater than " + maxageeyear + " year", 'Sorry You are not Eligible');
                            $scope.reg.PASR_Age = "";
                            $scope.reg.PASR_DOB = "";
                            return;
                        }
                    }
                    else {
                        swal("Greater than " + maxageeyear + " year", 'Sorry You are not Eligible');
                        $scope.reg.PASR_Age = "";
                        $scope.reg.PASR_DOB = "";
                        return;
                    }
                }
                else {
                    swal("Greater than " + maxageeyear + " year", 'Sorry You are not Eligible');
                    $scope.reg.PASR_Age = "";
                    $scope.reg.PASR_DOB = "";
                    return;
                }
            }
        }
        if (configsettings != undefined && configsettings != null) {
            for (var i = 0; i < configsettings.length; i++) {
                if (configsettings.length > 0) {

                    $scope.MasterConfigHostelApplicable = false;
                    $scope.MasterConfigTransportApplicable = false;
                    $scope.MasterConfigGymApplicable = false;
                    $scope.MasterConfigSiblingApplicable = false;
                    $scope.MasterConfigParentsApplicable = false;
                    $scope.MasterConfigECSApplicable = false;

                    $scope.configurationsettings = configsettings[i];

                    if ($scope.configurationsettings.ispaC_HostelFlag == 1) {
                        $scope.MasterConfigHostelApplicable = true;
                        $scope.MasterConfigHostelApplicable = $scope.configurationsettings.ispaC_HostelFlag;
                    }


                    if ($scope.configurationsettings.ispaC_TransaportFlag == 1) {
                        $scope.MasterConfigTransportApplicable = true;
                        $scope.MasterConfigTransportApplicable = $scope.configurationsettings.ispaC_TransaportFlag;
                    }


                    if ($scope.configurationsettings.ispaC_GymFlag == 1) {
                        $scope.MasterConfigGymApplicable = true;
                        $scope.MasterConfigGymApplicable = $scope.configurationsettings.ispaC_GymFlag;
                    }

                    //if ($scope.configurationsettings[0].ispaC_FatherAliveFlag == true) {
                    //    $scope.MasterConfigFatherAliveFlag = $scope.configurationsettings[0].ispaC_FatherAliveFlag;
                    //}

                    //if ($scope.configurationsettings[0].ispaC_MotherAliveFlag == true) {
                    //    $scope.MasterConfigMotherAliveFlag = $scope.configurationsettings[0].ispaC_MotherAliveFlag;
                    //}
                    //if ($scope.configurationsettings[0].ispaC_MaritalStatusFlag == true) {
                    //    $scope.MasterConfigParentsMarriageStatusFlag = $scope.configurationsettings[0].ispaC_MaritalStatusFlag;
                    //}

                    if ($scope.configurationsettings.ispaC_SibblingConcessionFlag == 1) {
                        $scope.MasterConfigSiblingApplicable = true;
                        $scope.MasterConfigSiblingApplicable = $scope.configurationsettings.ispaC_SibblingConcessionFlag;
                    }
                    else {
                        $scope.MasterConfigSiblingApplicable = false;
                    }


                    if ($scope.configurationsettings.ispaC_ParentConcessionFlag == 1) {
                        $scope.MasterConfigParentsApplicable = true;
                        $scope.MasterConfigParentsApplicable = $scope.configurationsettings.ispaC_ParentConcessionFlag;
                    }


                    if ($scope.configurationsettings.ispaC_ECSFlag == 1) {
                        $scope.MasterConfigECSApplicable = true;
                        $scope.MasterConfigECSApplicable = $scope.configurationsettings.ispaC_ECSFlag;
                    }
                }
            }
        }

        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        var RegistrationNumbering;
        if (transnumconfigsettings != null && transnumconfigsettings != undefined && transnumconfigsettings.length > 0) {
            for (var i = 0; i < transnumconfigsettings.length; i++) {
                if (transnumconfigsettings.length > 0) {
                    $scope.transnumbconfigurationsettings = transnumconfigsettings[i];

                    if (transnumconfigsettings[i].mI_Id == 10001 || transnumconfigsettings[i].mI_Id == 4) {
                        if (transnumconfigsettings[i].imN_Flag == "Form" && transnumconfigsettings[i].imN_SuffixParticular === "REG") {
                            RegistrationNumbering = transnumconfigsettings[i];
                        }
                        else if (transnumconfigsettings[i].imN_Flag == "Registration") {
                            RegistrationNumbering = transnumconfigsettings[i];
                        }
                    }
                    else {
                        if (transnumconfigsettings[i].imN_Flag == "Registration") {
                            RegistrationNumbering = transnumconfigsettings[i];
                        }
                    }

                }
            }
        }

        //Date:23-12-2016 for displaying privileges.
        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        if (privlgs !== null) {
            for (var i = 0; i < privlgs.length; i++) {
                if (privlgs[i].pageId == pageid) {
                    $scope.userPrivileges = privlgs[i];
                }
            }
        }


        $scope.panNovalidation = function (pan) {
            if (pan.length == 10) {
                var upperpan = pan.toUpperCase();

                var regpan = /^([A-Z]){5}([0-9]){4}([A-Z]){1}?$/; //regular expression for capital letter PAN NO.
                if (regpan.test(upperpan) == false) {
                    $scope.not_valid = "Not a valid PAN No.";
                    $scope.is_valid = true;
                }
                else {
                    $scope.is_valid = false;
                    $scope.not_valid - false;
                }
            }

        };

        $scope.panNovalidation1 = function (pan1) {
            if (pan1.length == 10) {
                var upperpan1 = pan1.toUpperCase();

                var regpan1 = /^([A-Z]){5}([0-9]){4}([A-Z]){1}?$/; //regular expression for capital letter PAN NO.
                if (regpan1.test(upperpan1) == false) {
                    $scope.not_valid1 = "Not a valid PAN No.";
                    $scope.is_valid1 = true;
                }
                else {
                    $scope.is_valid1 = false;
                    $scope.not_valid1 - false;
                }
            }

        };




        //Add dynamic siblings details
        //mobile add option
        $scope.Sib = true;
        $scope.siblings = [{ id: 'sibling1' }];
        $scope.addNewsibling = function () {
            var newItemNo = $scope.siblings.length + 1;

            if (newItemNo <= 5) {
                $scope.siblings.push({ 'id': 'sibling' + newItemNo });
            }
        };

        //$scope.removeNewsibling = function () {
        //    var newItemNo = $scope.siblings.length - 1;
        //    if (newItemNo !== 0) {
        //        $scope.siblings.pop();
        //    }
        //};

        $scope.removeNewsibling = function (index) {
            var newItemNo = $scope.siblings.length - 1;
            $scope.siblings.splice(index, 1);

            if ($scope.siblings.length == 0) {
                $scope.Sib = false;
            }
        };
        $scope.showAddsibling = function (sibling) {
            return sibling.id === $scope.siblings[$scope.siblings.length - 1].id;
        };

        //previous school details

        $scope.PreviousSchoolList = [{ id: 'PreviousSchool1' }];
        $scope.addNewPreviousSchool = function () {
            var newItemNo = $scope.PreviousSchoolList.length + 1;

            if (newItemNo <= 5) {
                $scope.PreviousSchoolList.push({ 'id': 'PreviousSchool' + newItemNo });
            }
        };

        $scope.removeNewPreviousSchool = function (index) {
            var newItemNo = $scope.PreviousSchoolList.length - 1;
            $scope.PreviousSchoolList.splice(index, 1);

            if ($scope.PreviousSchoolList.length == 0) {
            }
        };
        $scope.showAddPreviousSchool = function (PreviousSchool) {
            return PreviousSchool.id === $scope.PreviousSchoolList[$scope.PreviousSchoolList.length - 1].id;
        };

        $scope.IsHidden = true;
        $scope.ShowHide = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden = $scope.IsHidden ? false : true;
        }

        $scope.order = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.search = '';
        $scope.filterOnLocation = function (user) {
            //  (angular.lowercase(obj.User_Name)).indexOf(angular.lowercase($scope.searchValue))
            return (angular.lowercase(user.name)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(user.pasR_RegistrationNo)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(user.pasR_emailId)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                //(angular.lowercase(user.pasR_Sex)).indexOf(angular.lowercase($scope.search)) >= 0  ||
                (angular.lowercase(user.classname)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(user.username)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(user.pasR_Applicationno)).indexOf(angular.lowercase($scope.search)) >= 0;
        };
        $scope.IsHidden1 = true;
        $scope.ShowHide1 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden1 = $scope.IsHidden1 ? false : true;
        }

        $scope.IsHidden2 = true;
        $scope.ShowHide2 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden2 = $scope.IsHidden2 ? false : true;
        }

        $scope.IsHidden3 = true;
        $scope.ShowHide3 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden3 = $scope.IsHidden3 ? false : true;
        }


        $scope.IsHidden4 = true;
        $scope.ShowHide4 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden4 = $scope.IsHidden4 ? false : true;
        }

        $scope.IsHidden5 = true;
        $scope.ShowHide5 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden5 = $scope.IsHidden5 ? false : true;
        }


        $scope.IsHidden6 = true;
        $scope.ShowHide6 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden6 = $scope.IsHidden6 ? false : true;
        }

        $scope.IsHidden7 = true;
        $scope.ShowHide7 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden7 = $scope.IsHidden7 ? false : true;
        }

        $scope.IsHidden8 = true;
        $scope.ShowHide8 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden8 = $scope.IsHidden8 ? false : true;
        }

        $scope.IsHidden9 = true;
        $scope.ShowHide9 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden9 = $scope.IsHidden9 ? false : true;
        }
        //show model details

        $scope.showmodaldetails = function (data) {

            $('#preview').attr('src', data.document_Path);
        }

        //------------------//



        $scope.reg.PASR_FirstName = '';
        $scope.reg.PASR_emailId = '';
        //age start
        $scope.dateOfBirth;
        //age end

        $scope.myDate = new Date();

        $scope.showForm = false;
        $scope.showpay = false;


        $scope.fill_prospectus = function (pasP_Id) {



            var data = {

                "PASP_Id": pasP_Id
            }
            apiService.create("StudentApplication/fill_prospectus", data)
                .then(function (promise) {
                    $scope.prospectusDatalist = promise.prospectusDatalist;
                    $scope.reg.PASR_FirstName = promise.prospectusDatalist[0].pasP_First_Name;
                    $scope.reg.PASR_MiddleName = promise.prospectusDatalist[0].pasP_Middle_Name;
                    $scope.reg.PASR_LastName = promise.prospectusDatalist[0].pasP_Last_Name;
                    $scope.reg.PASR_MobileNo = promise.prospectusDatalist[0].pasP_MobileNo;
                    $scope.reg.PASR_emailId = promise.prospectusDatalist[0].pasP_EmailId;

                });
        };



        $scope.onclasschange = function (classid, yr) {

            if (parseInt(flg) === 0) {
                $scope.obj.amsT_DOB = "";
            }

            if (classid !== "") {
                var data = {
                    "ASMCL_Id": classid,
                    "ASMAY_Id": yr

                };

                apiService.create("StudentApplication/classchange", data).then(function (promise) {


                    if (promise.electivelist_Groups != null && promise.electivelist_Groups.length) {
                        $scope.electivelist_Groups = promise.electivelist_Groups;
                    }
                    if (promise.electivelist != null && promise.electivelist.length) {
                        $scope.electivelist = promise.electivelist;
                    }

                });
            }
        };

        $scope.specialsub = [];
        $scope.changeelectivesub = function (sub, index) {



            angular.forEach($scope.electives, function (ee, i) {

                if (ee.ismS_Id === sub.ismS_Id && i !== index) {
                    // count += 1;
                    angular.forEach($scope.electives, function (ee) {
                        ee.ismS_Id = "";
                    });
                    $scope.specialsub = [];
                    swal("You can't select same elective subjects!");
                }
            });


            //$scope.specialsub2 = [];
            //if ($scope.specialsub.length == 0) {
            //    angular.forEach($scope.electivegrouplist, function (opqr1) {
            //        if (opqr1.ISMS_Id == sub) {
            //            $scope.specialsub.push(opqr1);
            //        }
            //    })
            //}
            //else {
            //    angular.forEach($scope.electivegrouplist, function (opqr1) {
            //        if (opqr1.ISMS_Id == sub) {
            //            $scope.specialsub2.push(opqr1);
            //        }
            //    })

            //    angular.forEach($scope.specialsub, function (opqr) {
            //        angular.forEach($scope.specialsub2, function (opqr1) {
            //            if (opqr.ISMS_Id != opqr1.ISMS_Id) {
            //                if (opqr1.ISMS_ProgramFlag == opqr.ISMS_ProgramFlag) {
            //                    swal("You can't select Both JEE and NEET!!");
            //                }
            //            }
            //            else {
            //                $scope.electivegrouplist = $scope.electivegrouplist;
            //                swal("You can't select same elective subjects!");
            //            }
            //        })
            //    })
            //}
        }



        $scope.fillpay = function (pasR_Id) {

            $window.location.href = 'https://' + HostName + '/#/app/FeePreadmissionTransaction/';

        }

        $scope.intervalcall = function () {
            blockUI.stop();
            var data = { "Organization": org, "Institute": intd }
            apiService.create("StudentApplication/Getcountofstudents", data).then(function (promise) {
                blockUI.stop();
                if (promise.totalcountDetails != null) {
                    for (var a = 0; a < promise.totalcountDetails.length; a++) {
                        $scope.regisrertotal = promise.totalcountDetails[a].reg;
                        $scope.notappform = promise.totalcountDetails[a].notapplicationform;
                        $scope.appform = promise.totalcountDetails[a].applicationform;
                        $scope.pay = promise.totalcountDetails[a].payment;
                        $scope.notpay = promise.totalcountDetails[a].notpayment;
                        $scope.transferstu = promise.totalcountDetails[a].transferstu;
                        $scope.duplicate = promise.totalcountDetails[a].duplicate;
                    }
                }

            })
            blockUI.start();
        }

        $scope.checkemail = function () {
            var regex = /^[A-Za-z0-9]+$/

            //Validate TextBox value against the Regex.
            var isValid = regex.test($scope.reg.PASR_emailId);
            if (!isValid) {

            } else {
                alert("Does not contain Special Characters.");
            }

            return isValid;
        };

        $scope.regformfour = function (organization, institute) {

            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            $scope.underoverage = "";
            var data = { "Organization": organization, "Institute": institute }
            apiService.create("StudentApplication/getdpforreg", data).then(function (promise) {

                var rolename = promise.roleName;
                $scope.pages = promise.registrationList;
                $scope.statuslist = promise.statuslist;
                if (promise.fillpaymentgateway != null && promise.fillpaymentgateway.length > 0) {
                    $scope.paymenttest = promise.fillpaymentgateway;
                    angular.forEach($scope.paymenttest, function (i) {
                        i.hidepay = false;
                    })
                    if ($scope.paymenttest.length > 1) {
                        $scope.showsingle = false;
                        $scope.showdouble = true;
                    }
                    else if ($scope.paymenttest.length == 1) {
                        $scope.showsingle = true;
                        $scope.showdouble = false;
                    }
                }
                //if (promise.multipleapplications == true) {
                //    if (promise.classcategoryList != null && promise.classcategoryList.length > 0) {
                //        $scope.classcategoryList = promise.classcategoryList;
                //    }
                //    if (rolename != 'ADMIN' && rolename != 'COORDINATOR' && rolename != 'Admission End User') {
                //        $('#myModalswalclass').modal({ backdrop: 'static', keyboard: false });                     
                //    }
                //    else if ($scope.pages.length == 0 && rolename != 'ADMIN' && rolename != 'COORDINATOR' && rolename != 'Admission End User' && promise.classcategoryList == null) {
                //        if (promise.precutdate == "True") {
                //            $('#myModalswalclass').modal({ backdrop: 'static', keyboard: false })
                //        }
                //        else {
                //            $scope.shogwgrid = false;
                //        }
                //    }
                //}
                $scope.schoolview = promise.multipleapplications;
                $scope.collegeview = promise.multipleapplicationsc;
                $scope.specialuser = promise.specialuser;
                $scope.allRefrence = promise.studentReferenceDetails;
                $scope.allSources = promise.studentSourceDetails;

                if ($scope.allRefrence != null && $scope.allRefrence.length > 0) {

                }
                if ($scope.allSources != null && $scope.allSources.length > 0) {

                }
                if (promise.precutdate == "True") {
                    $scope.cuttoffimage = false;
                }
                else {
                    $scope.cuttoffimage = true;
                }


                var admissioncongiguration = promise.admissioncongigurationList;
                localStorage.setItem("admissionconfigsettings", JSON.stringify(admissioncongiguration));

                var admissionSettings = JSON.parse(localStorage.getItem("admissionconfigsettings"));

                if (admissionSettings !== null && admissionSettings !== "") {
                    if (admissionSettings.length > 0) {

                        $scope.photo = admissionSettings[0].asC_DefaultPhotoUpload;
                    }
                }
                else {

                    $scope.photo = 0;
                }
                if (parseInt($scope.photo) === 1) {
                    $scope.profile_photo = 1;
                }
                else {
                    $scope.profile_photo = 2;
                }


                if (rolename.toUpperCase() == 'ADMIN' || rolename == 'COORDINATOR' || rolename == 'Admission End User') {
                    $scope.admNoEntryFlag = true;

                }
                else {
                    $scope.admNoEntryFlag = false;
                }
                $scope.prospectusPaymentlist = promise.prospectusPaymentlist;
                if (promise.prospectusfilePath != null) {
                    $scope.prospectusdwnldpath = promise.prospectusfilePath;
                }
                $scope.arrlist58 = promise.admissioncatdrpall;

                if (promise.vaccines != null) {
                    $scope.vaccines = promise.vaccines;
                    $scope.vaccinesrebind = promise.vaccines;

                    if (promise.vaccines.length > 0) {
                        angular.forEach($scope.vaccines, function (opqr1) {
                            opqr1.pamvA_YesNoFlg = "";
                        })
                    }
                }
                //srkvs deepak

                $scope.prospectuslist = promise.prospectuslist;
                if (promise.prospectuslist != null) {
                    $scope.prospectusDlist = promise.prospectuslist;
                    $scope.admsudentslist = $scope.prospectusDlist;
                }


                //end srkvs deepak


                //syllabus
                $scope.syllabusarray = promise.syllabuslist
                if ($scope.configurationsettings != null && $scope.configurationsettings != undefined) {
                    if ($scope.configurationsettings.ispaC_ApplFeeFlag === 1) {

                        $scope.ispaC_ApplFeeFlag = $scope.configurationsettings.ispaC_ApplFeeFlag;
                        $scope.prosH = true;
                        $scope.prosL = true;
                    }
                    else {
                        $scope.ispaC_ApplFeeFlag = 0;
                        $scope.prosH = true;
                        $scope.prosL = true;
                    }
                }



                $scope.albumNameArray1 = [];
                if ($scope.pages != null) {
                    for (var i = 0; i < $scope.pages.length; i++) {
                        if ($scope.pages[i].pasR_FirstName != '') {
                            if ($scope.pages[i].pasR_MiddleName !== null) {
                                if ($scope.pages[i].pasR_LastName !== null) {

                                    $scope.albumNameArray1.push({ name: $scope.pages[i].pasR_FirstName + " " + $scope.pages[i].pasR_MiddleName + " " + $scope.pages[i].pasR_LastName, pasR_Id: $scope.pages[i].pasR_Id });
                                }
                                else {
                                    $scope.albumNameArray1.push({ name: $scope.pages[i].pasR_FirstName + " " + $scope.pages[i].pasR_MiddleName, pasR_Id: $scope.pages[i].pasR_Id });
                                }
                            }
                            else {
                                $scope.albumNameArray1.push({ name: $scope.pages[i].pasR_FirstName, pasR_Id: $scope.pages[i].pasR_Id });
                            }

                            $scope.pages[i].name = $scope.albumNameArray1[i].name;
                        }
                    }


                    if (promise.userdetails != null && promise.userdetails != undefined && promise.userdetails.length > 0) {
                        for (var i = 0; i < $scope.pages.length; i++) {
                            for (var j = 0; j < promise.userdetails.length; j++) {
                                if ($scope.pages[i].pasr_id == promise.userdetails[j].pasR_Id) {
                                    $scope.pages[i].username = promise.userdetails[j].stusername;
                                    break;
                                }
                            }
                        }
                    }
                    if ($scope.configurationsettings != null && $scope.configurationsettings != undefined) {
                        if ($scope.configurationsettings.ispaC_ApplFeeFlag === 1) {
                            if ($scope.prospectusPaymentlist.length > 0) {
                                for (var i = 0; i < $scope.pages.length; i++) {
                                    for (var j = 0; j < $scope.prospectusPaymentlist.length; j++) {
                                        if ($scope.pages[i].pasr_id == $scope.prospectusPaymentlist[j].pasA_Id && $scope.ispaC_ApplFeeFlag == 1) {
                                            $scope.pages[i].viewflag = true;
                                            $scope.pages[i].download = false;
                                            break;
                                        }
                                        else if ($scope.pages[i].pasr_id != $scope.prospectusPaymentlist[j].pasA_Id && $scope.ispaC_ApplFeeFlag == 1) {
                                            $scope.pages[i].viewflag = false;
                                            $scope.pages[i].download = true;

                                        }
                                    }
                                }
                                console.log($scope.pages);
                            }

                            else if ($scope.prospectusPaymentlist.length == 0) {

                                for (var i = 0; i < $scope.pages.length; i++) {
                                    if ($scope.ispaC_ApplFeeFlag == 1) {
                                        $scope.pages[i].viewflag = false;
                                        $scope.pages[i].download = true;

                                    }
                                    else if ($scope.ispaC_ApplFeeFlag == 0) {


                                        $scope.pages[i].viewflag = true;
                                        $scope.pages[i].download = false;

                                    }
                                }
                            }
                        }
                        else {
                            for (var i = 0; i < $scope.pages.length; i++) {
                                if ($scope.ispaC_ApplFeeFlag == 1) {
                                    $scope.pages[i].viewflag = false;
                                    $scope.pages[i].download = true;

                                }
                                else if ($scope.ispaC_ApplFeeFlag == 0) {


                                    $scope.pages[i].viewflag = true;
                                    $scope.pages[i].download = false;

                                }
                            }
                        }
                    }



                    $scope.totalcount = promise.totalcountDetails;
                    if (promise.countrole == true) {
                        $scope.countshow = true;
                        adminyesno = true;

                        angular.forEach($scope.pages, function (xy) {
                            xy.viewflagclass = true;
                        })
                    }
                    else {
                        adminyesno = false;
                        if ($scope.pages != null && $scope.pages != undefined && $scope.pages.length > 0) {
                            if ($scope.pages.length > 0) {
                                for (var i = 0; i < $scope.pages.length; i++) {
                                    for (var j = 0; j < promise.admissioncatdrp.length; j++) {
                                        if ($scope.pages[i].asmcL_Id == promise.admissioncatdrp[j].asmcL_Id) {
                                            $scope.pages[i].viewflagclass = true;
                                            break;
                                        }
                                        else if ($scope.pages[i].asmcL_Id != promise.admissioncatdrp[j].asmcL_Id) {
                                            $scope.pages[i].viewflagclass = false;
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if ($scope.totalcount != null) {
                        for (var a = 0; a < $scope.totalcount.length; a++) {
                            $scope.regisrertotal = $scope.totalcount[a].reg;
                            $scope.notappform = $scope.totalcount[a].notapplicationform;
                            $scope.appform = $scope.totalcount[a].applicationform;
                            $scope.pay = $scope.totalcount[a].payment;
                            $scope.notpay = $scope.totalcount[a].notpayment;
                            $scope.transferstu = $scope.totalcount[a].transferstu;
                            $scope.duplicate = promise.totalcountDetails[a].duplicate;
                        }
                    }

                    if ($scope.arrlist58.length > 0) {
                        for (var i = 0; i < $scope.pages.length; i++) {
                            for (var j = 0; j < $scope.arrlist58.length; j++) {
                                if ($scope.pages[i].asmcL_Id == $scope.arrlist58[j].asmcL_Id) {
                                    $scope.pages[i].classname = $scope.arrlist58[j].asmcL_ClassName;
                                }
                            }
                        }
                    }
                    if ($scope.statuslist.length > 0) {
                        for (var i = 0; i < $scope.pages.length; i++) {
                            for (var j = 0; j < $scope.statuslist.length; j++) {
                                if ($scope.pages[i].pamS_Id == $scope.statuslist[j].pamsT_Id) {
                                    $scope.pages[i].statusname = $scope.statuslist[j].pamsT_Status;
                                }
                            }
                        }
                    }
                }

                //if (promise.multipleapplications == true) {
                $scope.hutchingsschool = [];
                $scope.hutchingshigher = [];
                angular.forEach($scope.pages, function (opqr1) {
                    if (opqr1.amC_Id == 4) {
                        $scope.hutchingsschool.push(opqr1);
                    }
                    else if (opqr1.amC_Id == 5) {
                        $scope.hutchingshigher.push(opqr1);
                    }
                });
                if ($scope.hutchingsschool != null && $scope.hutchingsschool.length > 0) {
                    $scope.presentCountgridhs = $scope.hutchingsschool.length;
                }
                if ($scope.hutchingshigher != null && $scope.hutchingshigher.length > 0) {

                    if (promise.syllabuslist != null && promise.syllabuslist.length > 0) {
                        for (var i = 0; i < $scope.hutchingshigher.length; i++) {
                            for (var j = 0; j < promise.syllabuslist.length; j++) {
                                if ($scope.hutchingshigher[i].asmsT_Id == promise.syllabuslist[j].asmsT_Id) {
                                    $scope.hutchingshigher[i].streamname = promise.syllabuslist[j].asmsT_StreamName;
                                }
                            }
                        }
                    }
                    $scope.presentCountgridhj = $scope.hutchingshigher.length;
                }
                //}

                if (promise.precutdate == "True") {
                    $scope.showForm = true;
                    $scope.showpay = true;
                    $scope.arrlist = promise.countryDrpDown;
                    $scope.sourcedropDown = promise.sourcedropDown;

                    $scope.arrlist4 = promise.categorydrp;
                    if ($scope.arrlist4.length > 0 || $scope.arrlist4 != null) {
                        angular.forEach($scope.arrlist4, function (xy) {
                            if (xy.fmcC_ConcessionFlag == "G") {
                                $scope.fmccid = xy.fmcC_Id;
                                $scope.reg.FMCC_Id = xy.fmcC_Id;
                            }
                        })
                    }
                    $scope.arrlistofclass = promise.admissioncatdrp;

                    $scope.areaLst = promise.areaList;
                    //$scope.locationlist = promise.locationdrp;
                    if ($scope.areaLst != null && $scope.areaLst.length > 0) {
                        $scope.trmA_Id = promise.areaList[0].trmA_Id;
                        $scope.reg.trmA_Id = promise.areaList[0].trmA_Id;
                    }
                    if ($scope.locationlist != null && $scope.locationlist.length > 0) {
                        $scope.trmL_Idp = $scope.locationlist[0].trmL_Id;
                        $scope.reg.trmL_Idp = $scope.locationlist[0].trmL_Id;
                    }

                    $scope.arrlist7 = promise.castedrp;
                    $scope.arrlistsub = promise.subcastedrp;
                    $scope.arrlistsubf = promise.subcastedrp;
                    $scope.arrlistsubm = promise.subcastedrp;
                    $scope.arrlist8 = promise.religiondrp;
                    $scope.arrlist9 = promise.castecategorydrp;
                    $scope.arrlist8 = promise.religiondrp;
                    $scope.caste_doc_map_list = promise.caste_doc_maplist;

                    if (promise.registrationList != null) {
                        $scope.presentCountgrid = promise.registrationList.length;
                        // for pagination
                    }

                    //if (promise.stuusername != null) {
                    //    $scope.reg.PASR_FirstName = promise.stuusername;
                    //    $scope.reg.PASR_emailId = promise.useremail;
                    //    $scope.reg.PASR_MobileNo = promise.mobilenumber;
                    //    if (promise.useimagepath != null && promise.useimagepath != undefined) {
                    //        $('#blah').attr('src', promise.useimagepath);
                    //        $scope.reg.PASR_Student_Pic_Path = promise.useimagepath;
                    //    }
                    //}





                    if (promise.academicdrp != null && promise.academicdrp.length > 0) {

                        $scope.arrlist6 = promise.academicdrp;
                        $scope.reg.ASMAY_Id = $scope.arrlist6[0].asmaY_Id;

                        angular.forEach($scope.arrlist6, function (xy) {
                            if (xy.asmaY_Id == $scope.reg.ASMAY_Id) {
                                xy.asmaY_Id = $scope.yearid;
                            }
                        })

                        $scope.sectiondropdown = promise.sectiondropdown;
                        //document list
                        $scope.documentList = promise.documentList;

                        var lenofarray = $scope.documentList.length;


                        for (var i = 0; i < lenofarray; i++) {
                            $scope.documentList[i].document_Path = '';
                        }

                        $scope.syllabuslist = promise.syllabuslist;
                        //if (promise.syllabuslist.length > 0) {
                        //    angular.forEach($scope.syllabuslist, function (opqr1) {
                        //        opqr1.selectedsy = opqr1.asmsT_Id;
                        //        $scope.reg.ASMST_Id = opqr1.asmsT_Id;
                        //    })
                        //}


                        if (cutoffdate == 1) {
                            var cutofdate = promise.academicdrp[0].asmaY_Cut_Of_Date;
                            //console.log(cutofdate);
                            $scope.dateformats = cutofdate;
                            console.log($scope.dateformats);
                            $scope.lastdate = $scope.dateformats;
                            var date = new Date();
                            $scope.appdate = $filter('date')(new Date(), 'yyyy-MM-dd');

                            if ($scope.lastdate === $scope.appdate) {
                                $scope.appcutoffdate = true;
                                swal("Application Last date");
                                $scope.Flastdate = true;
                            }

                            else if ($scope.lastdate < $scope.appdate) {
                                $scope.appcutoffdate = true;
                                swal("Application Last date is completed");
                                $scope.applastdatedisable = true;
                            }

                        }



                    } else {
                        // swal("Preadmission Is Not Open For Current Date");
                        // $window.location.href = 'https://' + HostName + '/#/app/homepage';
                    }

                }
                else if (promise.precutdate === "false") {
                    $scope.showForm = false;
                    $scope.showpay = false;
                    swal("Online application dates are closed!");
                    if ($scope.pages == null || $scope.pages == undefined || $scope.pages.length == 0) {
                        $window.location.href = 'https://' + HostName + '/#/app/homepage';
                    }
                }
                else if (promise.precutdate === "Not") {
                    swal('Something went wrong', 'You dont have enough permission to access this page..!');
                    $window.location.href = 'https://' + HostName + '/#/app/homepage';
                }
                else {
                    swal('Something went wrong', 'try after some time..!');
                    $window.location.href = 'https://' + HostName + '/#/app/homepage';
                }
            });
        };

        $scope.nationality = function () {
            apiService.get("StudentApplication", 2).then(function (promise) {
                $scope.arrlist = promise.countryDrpDown;
                $scope.arrlist1 = promise.citydro;
                $scope.arrlist3 = promise.locationdrp;
                $scope.arrlist2 = promise.routedrp;
                $scope.arrlist4 = promise.categorydrp;
                $scope.arrlist5 = promise.admissioncatdrp;
                $scope.arrlist6 = promise.academicdrp;
            });
        }
        $scope.applastdatedisable = false;
        $scope.Flastdate = false;
        // Added on 19-9-2016
        $scope.getIndependentDrpDwns = function (organization, institute) {
            var data = { "Organization": organization, "Institute": institute }
            apiService.create("StudentApplication/getdpforreg", data).then(function (promise) {
                $scope.arrlist = promise.countryDrpDown;
                $scope.sourcedropDown = promise.sourcedropDown;
                //$scope.arrlist1 = promise.citydro;
                $scope.arrlist3 = promise.locationdrp;
                $scope.arrlist2 = promise.routedrp;
                $scope.arrlist4 = promise.categorydrp;
                $scope.arrlist5 = promise.admissioncatdrp;
                $scope.areaLst = promise.areaList;
                if ($scope.areaLst != null || $scope.areaLst.length > 0) {
                    $scope.trmA_Id = promise.areaList[0].trmA_Id;
                    $scope.reg.trmA_Id = promise.areaList[0].trmA_Id;
                }
                $scope.arrlist6 = promise.academicdrp;
                $scope.arrlist7 = promise.castedrp;
                $scope.arrlistsub = promise.subcastedrp;
                $scope.arrlistsubf = promise.subcastedrp;
                $scope.arrlistsubm = promise.subcastedrp;
                $scope.arrlist8 = promise.religiondrp;
                //$scope.MasterConfigRegister = promise.mstConfig;
                // for pagination 
                $scope.currentPage = 1;
                $scope.itemsPerPage = paginationformasters;
                $scope.pages = promise.registrationList;
                $scope.presentCountgrid = promise.registrationList.length;
                $scope.reg.ASMAY_Id = promise.academicdrp[0].asmaY_Id;
                // 30-9-2016
                if (promise.mstConfig != "" && promise.mstConfig != null) {
                    $scope.MasterConfigHostelApplicable = promise.mstConfig[0].ispaC_HostelFlag;
                    $scope.MasterConfigTransportApplicable = promise.mstConfig[0].ispaC_TransaportFlag;
                    $scope.MasterConfigGymApplicable = promise.mstConfig[0].ispaC_GymFlag;
                    $scope.MasterConfigFatherAliveFlag = promise.mstConfig[0].ispaC_FatherAliveFlag;
                    $scope.MasterConfigMotherAliveFlag = promise.mstConfig[0].ispaC_MotherAliveFlag;
                    $scope.MasterConfigParentsMarriageStatusFlag = promise.mstConfig[0].ispaC_MaritalStatusFlag;
                    $scope.MasterConfigSiblingConcessionFlag = promise.mstConfig[0].ispaC_SibblingConcessionFlag;
                    $scope.MasterConfigSiblingConcessionFlag = promise.mstConfig[0].ispaC_SibblingConcessionFlag;
                    $scope.MasterConfigSiblingConcessionFlag = promise.mstConfig[0].ispaC_SibblingConcessionFlag;
                }
                //document list
                $scope.documentList = promise.documentList;
                var lenofarray = $scope.documentList.length;
                for (var i = 0; i < lenofarray; i++) {
                    $scope.documentList[i].document_Path = '';
                }
                //else {
                //    $('#myModalswalclass').modal('hide');
                //    $('.modal-backdrop').remove();
                //    $window.location.href = 'https://' + HostName + '/#/app/PreadmissionDashboard/';
                //}
                // 30-9-2016
                if (cutoffdate == 1) {
                    var cutofdate = promise.academicdrp[0].asmaY_Cut_Of_Date;
                    //console.log(cutofdate);
                    $scope.dateformats = cutofdate;
                    console.log($scope.dateformats);
                    $scope.lastdate = $scope.dateformats;
                    var date = new Date();
                    $scope.appdate = $filter('date')(new Date(), 'yyyy-MM-dd');
                    if ($scope.lastdate === $scope.appdate) {
                        $scope.appcutoffdate = true;
                        swal("Application Last date");
                        $scope.Flastdate = true;
                    }
                    else if ($scope.lastdate < $scope.appdate) {
                        $scope.appcutoffdate = true;
                        swal("Application Last date is completed");
                        $scope.applastdatedisable = true;
                    }
                }
            })
        }
        var maxageeyear;
        var maxageemonth;
        var maxageedays;
        var minageeyear;
        var minageemonth;
        var minageedays;
        // $scope.DOB = true;
        this.minDate = new Date();
        $scope.onclasschange = function (classid) {
            $scope.reg.PASR_DOB = "";
            if (classid != "") {
                var data = {
                    "ASMCL_Id": classid
                }
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("StudentApplication/classchangemaxminage", data).then(function (promise) {
                    if (promise.message == "MaxCapacity") {
                        swal("Sorry,Class Capacity is Full");
                        $scope.applastdatedisable = true;
                    }
                    else {
                        $scope.applastdatedisable = false;
                    }
                    maxageeyear = promise.fillclass[0].asmcL_MaxAgeYear;
                    maxageemonth = promise.fillclass[0].asmcL_MaxAgeMonth;
                    maxageedays = promise.fillclass[0].asmcL_MaxAgeDays;
                    minageeyear = promise.fillclass[0].asmcL_MinAgeYear;
                    minageemonth = promise.fillclass[0].asmcL_MinAgeMonth;
                    minageedays = promise.fillclass[0].asmcL_MinAgeDays;
                    // $scope.DOB = false;
                    if (maxage == 0 && minage == 0) {
                        $scope.maxDate1 = new Date(
                            $scope.myDate.getFullYear(),
                            $scope.myDate.getMonth(),
                            $scope.myDate.getDate());
                    }
                    else if (maxage == 1 && minage == 0) {
                        $scope.maxDate1 = new Date(
                            $scope.myDate.getFullYear(),
                            $scope.myDate.getMonth(),
                            $scope.myDate.getDate());
                    }
                    else if (maxage == 0 && minage == 1) {
                        $scope.maxDate1 = new Date(
                            $scope.myDate.getFullYear(),
                            $scope.myDate.getMonth(),
                            $scope.myDate.getDate());
                    }
                    else if (maxage == 1 && minage == 1) {
                        $scope.maxDate1 = new Date(
                            $scope.myDate.getFullYear(),
                            $scope.myDate.getMonth(),
                            $scope.myDate.getDate());
                    }
                })
            }
        }


        $scope.onclasschangeforapp = function (classid) {


            if (classid != "") {
                var data = {
                    "ASMCL_Id": classid
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("StudentApplication/getapplicationhtml", data).then(function (promise) {

                    if (promise.classcategoryList.length > 0) {
                        $scope.applicationform = promise.classcategoryList[0].applicationhtml;
                        if ($scope.applicationform == 'hutchings') {
                            $('#myModalswalclass').modal('hide');
                        }
                        else {
                            $('#myModalswalclass').modal('hide');
                            $('.modal-backdrop').remove();
                            $window.location.href = 'https://' + HostName + '/#/app/hutchingshigher/';
                        }
                    }
                    else {
                        $('#myModalswalclass').modal('hide');
                        $('.modal-backdrop').remove();
                        $window.location.href = 'https://' + HostName + '/#/app/PreadmissionDashboard/';
                    }
                })

            }
        }

        $scope.closemodal = function () {

            $('#myModalswalclass').modal('hide');
            $('.modal-backdrop').remove();
            $window.location.href = 'https://' + HostName + '/#/app/PreadmissionDashboard/';
        }

        $scope.closefilled = function () {
            if ($scope.pages != null && $scope.pages.length > 0) {
                $('#myModalswalclass').modal('hide');
                $('.modal-backdrop').remove();
                $scope.applicationform = $scope.classcategoryList[0].applicationhtml;
                $window.location.href = 'https://' + HostName + '/#/app/' + $scope.applicationform + '/';
            }
            else {
                $('.modal-backdrop').remove();
                swal("Not filled any application!!");
                $window.location.href = 'https://' + HostName + '/#/app/PreadmissionDashboard/';
            }
        }

        $scope.onprospectuschanges = function (studentlst) {

            $scope.studentid = 0;
            if (studentlst == undefined) {
                $scope.studentid = 0;
            }
            else {
                $scope.studentid = studentlst.pasP_Id;
            }
            var countryidd = $scope.studentid;
            apiService.getURI("StudentApplication/getdprospectusdetails", countryidd).then(function (promise) {
                if (promise.prospectusdetails.length > 0) {
                    $scope.prospectusdetails = promise.prospectusdetails;
                    $scope.reg.PASR_FirstName = $scope.prospectusdetails[0].pasP_First_Name;
                    $scope.reg.PASR_MiddleName = $scope.prospectusdetails[0].pasP_Middle_Name;
                    $scope.reg.PASR_LastName = $scope.prospectusdetails[0].pasP_Last_Name;
                    $scope.reg.PASR_MobileNo = $scope.prospectusdetails[0].pasP_MobileNo;
                    $scope.reg.PASR_emailId = $scope.prospectusdetails[0].pasP_EmailId;
                    $scope.reg.ASMCL_Id = $scope.prospectusdetails[0].asmcL_Id;
                    $scope.reg.PASR_Nationality = $scope.prospectusdetails[0].ivrmmC_Id;
                    $scope.reg.PASR_BirthPlace = $scope.prospectusdetails[0].ivrmmcT_Id;
                    $scope.arrlist5 = promise.admissioncatdrp;
                    $scope.reg.PASR_Sex = 'Male';
                    $scope.reg.Religion_Id = 13;
                    $scope.reg.Caste_Id = 4133;
                    $scope.getstreams($scope.reg.ASMCL_Id);
                    //$scope.reg.PASR_Nationality = $scope.prospectusdetails[0].ivrmmS_Id;
                }

            })
        }

        //function getSelect
        // Get state by country
        $scope.onSelectGetState = function (countryidd) {
            apiService.getURI("StudentApplication/getdpstate", countryidd).then(function (promise) {
                $scope.arrStatelist = promise.stateDrpDown;

            })
        }
        // Get state by country
        $scope.onSelectGetState2 = function (countryidd) {
            countryidd = $scope.reg.PASR_ConCountry;
            apiService.getURI("StudentApplication/getdpstate", countryidd).then(function (promise) {
                $scope.arrStatelist2 = promise.stateDrpDown;
            });
        };
        $scope.FonSelectGetState2 = function (countryidd) {
            countryidd = $scope.reg.PASR_FatherNationality;
            apiService.getURI("StudentApplication/getdpstate", countryidd).then(function (promise) {
                $scope.farrStatelist2 = promise.stateDrpDown;
            });
        };
        //district
        $scope.onSelectGetDistrict2 = function (stateidd) {
            var stateidd = $scope.reg.PASR_ConState;
            apiService.getURI("StudentApplication/getdpdistrict", stateidd).then(function (promise) {
                $scope.arrDistrictlist2 = promise.districtDrpDown;
            });
        };

        $scope.onSelectGetDistrict = function (stateidd) {
            apiService.getURI("StudentApplication/getdpdistrict", stateidd).then(function (promise) {
                $scope.arrDistrictlist = promise.districtDrpDown;

            })
        }
        //


        $scope.MonSelectGetState2 = function (countryidd) {
            countryidd = $scope.reg.PASR_MotherNationality;
            apiService.getURI("StudentApplication/getdpstate", countryidd).then(function (promise) {
                $scope.marrStatelist2 = promise.stateDrpDown;
            });
        };

        //get permenent address state while editing
        function getSelectGetState(countryidd, stateid) {
            apiService.getURI("StudentApplication/getdpstate", countryidd).then(function (promise) {
                $scope.arrStatelist = [{ "ivrmmS_Id": 0, "ivrmmS_Name": "Select State" }];
                var sts = Number(stateid);
                $scope.reg.PASR_PerState = sts;

                $scope.data = promise.stateDrpDown;
                $scope.arrStatelist.push.apply($scope.arrStatelist, $scope.data);
            });
        }

        //get permenent address district while editing
        function getSelectGetDistrict(stateid, districtid) {
            apiService.getURI("StudentApplication/getdpdistrict", stateid).then(function (promise) {
                $scope.arrDistrictlist = [{ "ivrmmD_Id": 0, "ivrmmD_Name": "Select District" }];
                var sts = Number(districtid);
                $scope.reg.PASR_PerDistrict = sts;

                $scope.data = promise.districtDrpDown;
                $scope.arrDistrictlist.push.apply($scope.arrDistrictlist, $scope.data);
            });
        }
        $scope.farrStatelist2 = [];
        $scope.marrStatelist2 = [];
        function getSelectGetStatefps(countryidd, stateid) {
            apiService.getURI("StudentApplication/getdpstate", countryidd).then(function (promise) {
                //$scope.arrStatelist = [{ "ivrmmS_Id": 0, "ivrmmS_Name": "Select State" }];
                var sts = Number(stateid);
                $scope.reg.PASR_FatherPresentState = sts;

                $scope.farrStatelist2 = promise.stateDrpDown;
                //$scope.farrStatelist2.push.apply($scope.arrStatelist, $scope.data);
            });
        }

        function getSelectGetStatemps(countryidd, stateid) {
            apiService.getURI("StudentApplication/getdpstate", countryidd).then(function (promise) {
                $scope.arrStatelist = [{ "ivrmmS_Id": 0, "ivrmmS_Name": "Select State" }];
                var sts = Number(stateid);

                $scope.marrStatelist2 = promise.stateDrpDown;
                //$scope.marrStatelist2.push.apply($scope.arrStatelist, $scope.data);
            });
        }

        //get Residential address state while editing
        function getSelectGetState2(countryidd, stateid) {
            apiService.getURI("StudentApplication/getdpstate", countryidd).then(function (promise) {
                $scope.arrStatelist2 = [{ "ivrmmS_Id": 0, "ivrmmS_Name": "Select State" }];
                var sts = Number(stateid);
                $scope.reg.PASR_ConState = sts;

                $scope.data2 = promise.stateDrpDown;
                $scope.arrStatelist2.push.apply($scope.arrStatelist2, $scope.data2);
            })
        }

        //get Residential address district while editing
        function getSelectGetDistrict2(stateid, districtid) {
            apiService.getURI("StudentApplication/getdpdistrict", stateid).then(function (promise) {
                $scope.arrDistrictlist2 = [{ "ivrmmD_Id": 0, "ivrmmD_Name": "Select District" }];
                var sts = Number(districtid);
                $scope.reg.PASR_ConDistrict = sts;

                $scope.data2 = promise.districtDrpDown;
                $scope.arrDistrictlist2.push.apply($scope.arrDistrictlist2, $scope.data2);
            })
        }

        $scope.SelectedFileForUpload = [];
        $scope.selectFileforUpload = function (files) {
            //$scope.SelectedFileForUpload = file[0];
            $scope.SelectedFileForUpload = files;
        }
        $scope.defaultUpload = [];
        $scope.defaultUpload = function (input) {
            $scope.photoupload_flag = 'Default'
            $scope.selectFileforUploadz(input);
        }

        $scope.SelectedFileForUploadz = [];


        $scope.selectFileforUploadz = function (input) {
            $scope.wbcamurl = "";
            //$scope.photoupload_flag = 'Default';

            $scope.wbcamurl = input;
            if ($scope.photoupload_flag == 'Default') {

                $scope.SelectedFileForUploadz = input.files;
                if (input.files && input.files[0]) {
                    if ((input.files[0].type == "image/jpeg" || input.files[0].type == "image/png" || input.files[0].type == "image/jpg"))
                    //&& input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                    {
                        var reader = new FileReader();

                        reader.onload = function (e) {
                            $('#blah')
                                .attr('src', e.target.result)
                        };
                        reader.readAsDataURL(input.files[0]);
                        Uploadprofile();

                    }
                    else if (input.files[0].type != "image/jpeg" && input.files[0].type != "image/png" && input.files[0].type != "image/jpg") {
                        swal("Please Upload the JPEG file");
                        return;
                    }
                    //else if (input.files[0].size > 2097152) {
                    //    swal("Image size should be less than 2MB");
                    //    return;
                    //}
                }
            }
            else {
                // Uploadprofile($scope.wbcamurl);
                $scope.reg.PASR_Student_Pic_Path = $scope.wbcamurl;
            }
            // 


        }

        function Uploadprofile() {

            var formData = new FormData();

            //if ($scope.photoupload_flag == 'Webcam' && $scope.wbcamurl !="" ) {


            //    var data = {
            //        "PASMD_Path": $scope.wbcamurl
            //    }



            //    apiService.create("StudentApplication/Capture", data).then(function (promise) {
            //        if (promise.message == "MaxCapacity") {

            //        }


            //    })

            //}
            //else {
            for (var i = 0; i <= $scope.selectFileforUploadz.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadz[i]);
            }


            formData.append("Id", 0);

            var defer = $q.defer();
            $http.post("/api/ImageUpload/Uploadprofilepic", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);
                    // swal(d);
                    $scope.reg.PASR_Student_Pic_Path = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
            // }






            //We can send more data to server using append         

            // Uploads1(miid);
        }

        //Uplaod Father 

        $scope.SelectedFileForUploadF = [];
        $scope.selectFileforUploadF = function (files) {
            //$scope.SelectedFileForUpload = file[0];
            $scope.SelectedFileForUploadF = files;
        }


        $scope.SelectedFileForUploadzF = [];
        $scope.selectFileforUploadzF = function (input) {

            $scope.SelectedFileForUploadzF = input.files;

            if (input.files && input.files[0]) {
                if ((input.files[0].type == "image/jpeg" || input.files[0].type == "image/png" || input.files[0].type == "image/jpg"))
                //&& input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();

                    reader.onload = function (e) {
                        $('#blahf')
                            .attr('src', e.target.result)
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadprofileF();

                } else if (input.files[0].type != "image/jpeg" && input.files[0].type != "image/png" && input.files[0].type != "image/jpg") {
                    swal("Please Upload the JPEG file");
                    return;
                }
                //else if (input.files[0].size > 2097152) {
                //    swal("Image size should be less than 2MB");
                //    return;
                //}
            }
        };

        function UploadprofileF() {

            var formData = new FormData();

            for (var i = 0; i <= $scope.selectFileforUploadzF.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzF[i]);
            }

            //We can send more data to server using append         
            formData.append("Id", 0);

            var defer = $q.defer();
            $http.post("/api/ImageUpload/ParentsProfilePics", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);
                    // swal(d);
                    $scope.reg.PASR_FatherPhoto = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
            // Uploads1(miid);
        }

        //Upload Mother

        $scope.SelectedFileForUploadm = [];
        $scope.selectFileforUploadF = function (files) {
            //$scope.SelectedFileForUpload = file[0];
            $scope.SelectedFileForUploadF = files;
        }


        $scope.SelectedFileForUploadzm = [];
        $scope.selectFileforUploadzm = function (input) {

            $scope.SelectedFileForUploadzm = input.files;

            if (input.files && input.files[0]) {
                if ((input.files[0].type == "image/jpeg" || input.files[0].type == "image/png" || input.files[0].type == "image/jpg"))
                //&& input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();

                    reader.onload = function (e) {
                        $('#blahm')
                            .attr('src', e.target.result)
                    };
                    reader.readAsDataURL(input.files[0]);
                    Uploadprofilem();

                } else if (input.files[0].type != "image/jpeg" && input.files[0].type != "image/png" && input.files[0].type != "image/jpg") {
                    swal("Please Upload the JPEG file");
                    return;
                }
                //else if (input.files[0].size > 2097152) {
                //    swal("Image size should be less than 2MB");
                //    return;
                //}
            }
        }

        function Uploadprofilem() {

            var formData = new FormData();

            for (var i = 0; i <= $scope.selectFileforUploadzm.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzm[i]);
            }

            //We can send more data to server using append         
            formData.append("Id", 0);

            var defer = $q.defer();
            $http.post("/api/ImageUpload/ParentsProfilePics", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);
                    // swal(d);
                    $scope.reg.PASR_MotherPhoto = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
            // Uploads1(miid);
        }


        //indil


        //Uplaod JOINT PHOTO 

        $scope.SelectedFileForUploadJ = [];
        $scope.selectFileforUploadJ = function (files) {
            //$scope.SelectedFileForUpload = file[0];
            $scope.SelectedFileForUploadJ = files;
        };


        $scope.SelectedFileForUploadzJ = [];
        $scope.selectFileforUploadzJ = function (input) {

            $scope.SelectedFileForUploadzJ = input.files;

            if (input.files && input.files[0]) {
                if ((input.files[0].type == "image/jpeg" || input.files[0].type == "image/png" || input.files[0].type == "image/jpg"))
                //&& input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();

                    reader.onload = function (e) {
                        $('#blahj')
                            .attr('src', e.target.result);
                    };
                    reader.readAsDataURL(input.files[0]);
                    UploadprofileJ();

                }
                else if (input.files[0].type != "image/jpeg" && input.files[0].type != "image/png" && input.files[0].type != "image/jpg") {
                    swal("Please Upload the JPEG file");
                    return;
                }
                //else if (input.files[0].size > 2097152) {
                //    swal("Image size should be less than 2MB");
                //    return;
                //}
            }
        };

        function UploadprofileJ() {

            var formData = new FormData();

            for (var i = 0; i <= $scope.selectFileforUploadzJ.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzJ[i]);
            }

            //We can send more data to server using append         
            formData.append("Id", 0);

            var defer = $q.defer();
            $http.post("/api/ImageUpload/ParentsProfilePics", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);
                    // swal(d);
                    $scope.reg.PASR_JointPhoto = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
            // Uploads1(miid);
        };

        // joint close

        $scope.SelectedFileForUploadzd = [];

        $scope.selectFileforUploadzd = function (input, document) {
            $scope.SelectedFileForUploadzd = input.files;

            if (input.files && input.files[0]) {

                var filename = input.files[0].name.toString();

                var nameArray = filename.split('.');

                var extention = nameArray[nameArray.length - 1];


                if ((angular.lowercase(extention) == "jpeg" || angular.lowercase(extention) == "png" || angular.lowercase(extention) == "jpg" || angular.lowercase(extention) == "pdf" || angular.lowercase(extention) == 'docx' || angular.lowercase(extention) == 'doc'))
                //&& input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    var reader = new FileReader();

                    reader.onload = function (e) {

                        $('#' + document.amsmD_Id)
                            .attr('src', e.target.result)
                    };
                    reader.readAsDataURL(input.files[0]);
                    Uploadprofiled(document);

                }
                else if (angular.lowercase(extention) != "jpg" && angular.lowercase(extention) != "jpeg" && angular.lowercase(extention) != "png" && angular.lowercase(extention) != "pdf") {

                    $('#' + document.id).removeAttr('src');
                    swal("Please Upload the valid file");
                    return;
                }
                //else if (input.files[0].size > 2097152) {
                //    $('#' + document.id).removeAttr('src');
                //    swal("Document size should be less than 2MB");
                //    return;
                //}

            }
        }

        $scope.viewdox = function (data) {
            var docpath = "https://view.officeapps.live.com/op/view.aspx?src=" + data;

            $scope.detailFrame = $sce.trustAsResourceUrl(docpath);

            $('#myModaldocx').modal('show');
            $('.modal').on('hide.bs.modal', function (e) {
                e.stopPropagation();
                $('body').css('padding-right', '');
            });
        };

        function Uploadprofiled(data) {
            console.log(data);
            var formData = new FormData();

            for (var i = 0; i <= $scope.selectFileforUploadzd.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzd[i]);
            }
            //We can send more data to server using append         
            formData.append("Id", data.amsmD_Id);
            var defer = $q.defer();
            $http.post("/api/ImageUpload/UploadStudentDocuments", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);
                    data.document_Path = d;


                    var img = data.document_Path;
                    var imagarr = img.split('.');
                    var lastelement = imagarr[imagarr.length - 1];

                    data.filetype = lastelement;
                    // swal(d);
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });

            // Uploads1(miid);
        }
        //$scope.previewPdf = function (pdfurl) {
        //    


        //}

        //delete record
        $scope.delete = function (id, SweetAlert) {
            // swal(id);
            swal({
                title: "Are you sure?",
                text: "Do you want to delete record ..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.DeleteURI("StudentApplication/deletedetails", id).
                            then(function (promise) {
                                swal('Record Deleted Successfully..!', 'success');
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }

                });
        };




        //Active Deactivate----
        $scope.editEmployee = {}
        $scope.deletedata = function (employee, SweetAlert) {
            $scope.editEmployee = employee.pasr_id;
            var pageid = $scope.editEmployee;

            var msg;
            if (employee.pasR_ActiveFlag == 1) {
                msg = "Activate";
            }
            else {
                msg = "Deactivate";
            }
            swal({
                title: "Are you sure",
                text: "Do you want to " + msg + " record????",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + msg + " it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        var data = { "pasr_id": pageid }
                        apiService.create("StudentApplication/ActivateDactivate", data).then(function (promise) {
                            //apiService.DeleteURI("StudentApplication/ActivateDactivate", pageid).
                            //    then(function (promise) {
                            if (promise != null && promise != "") {
                                if (promise.returnval === "true") {
                                    swal('Record Deactivated Successfully', 'success');
                                    $state.reload();
                                }
                                else if (promise.returnval === "false") {
                                    swal('Record Activated Successfully!', 'success');
                                    $state.reload();
                                }
                                else {
                                    swal('Record not  Activated/Deactivated.!', 'Failed');
                                    return;
                                }

                            }
                            else {
                                swal('Record not Activated/Deactivated.!', 'Failed');
                                return;
                            }

                        })
                    }
                    else {
                        swal("Record Activation/Deactivation Cancelled");
                    }
                });
        }




        $scope.Back = function () {

            $scope.PaymentMode = false;
            $scope.ProspectuseScreen = true;

            // $scope.cancel();
            $state.reload();


        }

        $scope.paynow = function (pasR_Id) {

            if ($scope.offlinefee == false) {
                $scope.result = 1;
            }
            //else {
            //    $scope.result = 1;
            //}


            var data = {
                "SearchData": $scope.search,
                "searchType": $scope.searchType
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("StudentApplication/SearchData", data).then(function (promise) {
                if ($scope.pages != null && $scope.pages != undefined && $scope.pages.length > 0) {
                    angular.forEach($scope.pages, function (item) {
                        if (item.pasr_id == pasR_Id) {
                            $scope.reg.PASR_FirstName = item.name;
                            $scope.reg.PASR_emailId = item.pasR_emailId;
                            $scope.reg.PASR_MobileNo = item.pasR_MobileNo;
                            var regex = /^[A-Za-z0-9@.]+$/
                            var isValid = regex.test($scope.reg.PASR_emailId);
                            if (!isValid) {
                                angular.forEach($scope.paymenttest, function (i) {
                                    i.hidepay = false;
                                    if (i.fpgD_PGName === "PAYTM") {
                                        i.hidepay = true;
                                    }
                                })
                            }
                        }
                    })
                    $scope.pasR_Id = pasR_Id;
                }

                if ($scope.offlinefee == false) {
                    $scope.result = 1;
                }
                else {
                    $scope.result = "";
                }

                $scope.PaymentMode = true;
                $scope.ProspectuseScreen = false;
                $scope.disbalehealth = true;

                if ($scope.myTabIndex == 1) {
                    $scope.myTabIndex = $scope.myTabIndex - 1;
                }
                swal.close();
                showConfirmButton: false
            });




            //}
            //else {
            //    swal('Registered Successfully,But Payment gateway details are not mapped to institute', 'Contact Administrator..!!');
            //    $state.reload();
            //}

            //});
        }
        // ADDING AND REMOVING REFRENCE DETAILS
        $scope.Refrencecheckboxchcked = [];
        $scope.CheckedRefrenceName = function (data) {
            if ($scope.Refrencecheckboxchcked.indexOf(data) === -1) {
                for (var i = 0; i < $scope.allRefrence.length; i++) {
                    if ($scope.allRefrence[i].Selected == true) {
                        $scope.Refrencecheckboxchcked.push($scope.allRefrence[i]);
                    }
                }
            }
            else {
                $scope.Refrencecheckboxchcked.splice($scope.Refrencecheckboxchcked.indexOf(data), 1);
            }
        };

        // ADDING AND REMOVING SOURCE DETAILS
        $scope.Sourcescheckboxchcked = [];
        $scope.CheckedSourcesName = function (data) {
            if ($scope.Sourcescheckboxchcked.indexOf(data) === -1) {
                for (var i = 0; i < $scope.allSources.length; i++) {
                    if ($scope.allSources[i].Selected == true) {
                        $scope.Sourcescheckboxchcked.push($scope.allSources[i]);
                    }
                }
            }
            else {
                $scope.Sourcescheckboxchcked.splice($scope.Sourcescheckboxchcked.indexOf(data), 1);
            }
        };

        $scope.edit = function (pasR_Id) {
            angular.forEach($scope.documentList, function (value, key) {
                $('#' + value.amsmD_Id).removeAttr('src');
                $scope.documentList[key].document_Path = '';
            });
            $scope.updatedata = true;
            apiService.getURI("StudentApplication/getEditData", pasR_Id).then(function (promise) {
                $scope.reg.pasr_id = promise.studentReg_DTObj[0].pasr_id;
                if (promise.editflag == "True" && promise.roletypefind == false) {
                    $scope.editflaggg = true;
                }
                else {
                    $scope.editflaggg = false;
                }
                if (adminyesno == true) {
                    $scope.classdesable = false;
                }
                else {
                    $scope.classdesable = true;
                }
                //$scope.ASMAY_Id = promise.studentReg_DTObj[0].asmaY_Id;

                //if ($scope.reg.ASMAY_Id === promise.asmaY_Id) {
                //}
                //else {
                //    $scope.arrlist6 = promise.academicdrp;
                //    $scope.reg.ASMAY_Id = promise.asmaY_Id;
                //}
                $scope.prospectusshow = true;
                $scope.arrlist5 = promise.admissioncatdrp;
                $scope.arrlist6 = promise.academicdrp;
                //$scope.reg.ASMAY_Id = $scope.arrlist6[0].asmaY_Id;

                $scope.yearid = $scope.arrlist6[0].asmaY_Id;

                $('#blah').attr('src', promise.studentReg_DTObj[0].pasR_Student_Pic_Path);
                $scope.reg.PASR_Student_Pic_Path = promise.studentReg_DTObj[0].pasR_Student_Pic_Path;

                $('#blahj').attr('src', promise.studentReg_DTObj[0].pasR_JointPhoto);
                $scope.reg.PASR_JointPhoto = promise.studentReg_DTObj[0].pasR_JointPhoto;

                $('#blahf').attr('src', promise.studentReg_DTObj[0].pasR_FatherPhoto);
                $scope.reg.PASR_FatherPhoto = promise.studentReg_DTObj[0].pasR_FatherPhoto;

                $('#blahm').attr('src', promise.studentReg_DTObj[0].pasR_MotherPhoto);
                $scope.reg.PASR_MotherPhoto = promise.studentReg_DTObj[0].pasR_MotherPhoto;

                $scope.reg.PASR_FirstName = promise.studentReg_DTObj[0].pasR_FirstName;
                $scope.reg.PASR_MiddleName = promise.studentReg_DTObj[0].pasR_MiddleName;
                $scope.reg.PASR_LastName = promise.studentReg_DTObj[0].pasR_LastName;
                $scope.reg.PASR_Date = new Date(promise.studentReg_DTObj[0].pasR_Date);
                $scope.reg.PASR_RegistrationNo = promise.studentReg_DTObj[0].pasR_RegistrationNo;
                $scope.reg.PASRProspectusnNo = promise.studentReg_DTObj[0].pasR_RegistrationNo;
                if (promise.prospectuslist != null) {
                    $scope.prospectusDlist = promise.prospectuslist;
                    $scope.admsudentslist = $scope.prospectusDlist;
                }


                $scope.reg.appNo = promise.studentReg_DTObj[0].pasR_Applicationno;
                $scope.PASP_Id = promise.pasP_Id;
                if ($scope.PASP_Id > 0) {
                    $scope.drop_prospectous = true;
                }

                if ($scope.reg.appNo != null || $scope.reg.appNo != undefined) {
                    $scope.reg.admcheck = true;
                }
                else {
                    $scope.reg.admcheck = false;
                }



                $scope.reg.PASR_Sex = promise.studentReg_DTObj[0].pasR_Sex;
                $scope.reg.PASA_TransferrableJobFlg = promise.studentReg_DTObj[0].pasA_TransferrableJobFlg;
                if ($scope.reg.PASA_TransferrableJobFlg == true) {
                    $scope.reg.PASA_TransferrableJobFlg = "1";
                }
                else {
                    $scope.reg.PASA_TransferrableJobFlg = "0";
                }
                $scope.reg.PASR_DOB = new Date(promise.studentReg_DTObj[0].pasR_DOB);
                $scope.reg.PASR_ChurchBaptisedDate = new Date(promise.studentReg_DTObj[0].pasR_ChurchBaptisedDate);
                $scope.reg.PASR_Age = promise.studentReg_DTObj[0].pasR_Age;
                $scope.reg.ASMCL_Id = promise.studentReg_DTObj[0].asmcL_Id;
                $scope.ASMCL_Id = promise.studentReg_DTObj[0].asmcL_Id;
                $scope.reg.PASR_BloodGroup = promise.studentReg_DTObj[0].pasR_BloodGroup;
                $scope.reg.medium = promise.studentReg_DTObj[0].pasR_Medium;
                $scope.reg.PASR_Emisno = promise.studentReg_DTObj[0].pasR_Emisno;
                $scope.reg.PASR_Boarding = promise.studentReg_DTObj[0].pasR_Boarding;
                $scope.reg.PASR_MotherTongue = promise.studentReg_DTObj[0].pasR_MotherTongue;
                if (promise.studentReg_DTObj[0].religion_Id > 0) {
                    $scope.reg.Religion_Id = promise.studentReg_DTObj[0].religion_Id;
                }

                $scope.reg.CasteCategory_Id = promise.studentReg_DTObj[0].casteCategory_Id;
                if (promise.studentReg_DTObj[0].caste_Id > 0) {
                    $scope.reg.Caste_Id = promise.studentReg_DTObj[0].caste_Id;
                }

                $scope.reg.PASR_BirthCertificateNo = promise.studentReg_DTObj[0].pasR_BirthCertificateNo;

                //Mald Changes
                $scope.reg.PASR_CatseCertificateNo = promise.studentReg_DTObj[0].pasR_CatseCertificateNo;
                $scope.reg.PASR_Illnessdetails = promise.studentReg_DTObj[0].pasR_Illnessdetails;
                $scope.reg.PASR_AdmissionReason = promise.studentReg_DTObj[0].pasR_AdmissionReason;
                //

                $scope.reg.PASR_LastPlayGrndAttnd = promise.studentReg_DTObj[0].pasR_LastPlayGrndAttnd;
                $scope.reg.PASR_ExtraActivity = promise.studentReg_DTObj[0].pasR_ExtraActivity;
                $scope.reg.PASR_OtherResidential_Addr = promise.studentReg_DTObj[0].pasR_OtherResidential_Addr;
                $scope.reg.PASR_OtherPermanentAddr = promise.studentReg_DTObj[0].pasR_OtherPermanentAddr;

                $scope.reg.PASR_PerStreet = promise.studentReg_DTObj[0].pasR_PerStreet;
                $scope.reg.PASR_PerArea = promise.studentReg_DTObj[0].pasR_PerArea;
                $scope.reg.PASR_PerCountry = promise.studentReg_DTObj[0].pasR_PerCountry;
                $scope.reg.PASR_FatherNationality = promise.studentReg_DTObj[0].pasR_FatherNationality;
                $scope.reg.PASR_MotherNationality = promise.studentReg_DTObj[0].pasR_MotherNationality;
                //$scope.reg.PASR_PerState = promise.studentReg_DTObj[0].pasR_PerState;

                getSelectGetState($scope.reg.PASR_PerCountry, promise.studentReg_DTObj[0].pasR_PerState);
                $scope.PASR_PerState = promise.studentReg_DTObj[0].pasR_PerState;
                //district 
                getSelectGetDistrict($scope.reg.PASR_PerDistrict, promise.studentReg_DTObj[0].pasR_PerDistrict);
                $scope.PASR_PerDistrict = promise.studentReg_DTObj[0].pasR_PerDistrict;
                //
                $scope.reg.PASR_FatherPresentState = promise.studentReg_DTObj[0].pasR_FatherPresentState;
                getSelectGetStatefps($scope.reg.PASR_FatherNationality, promise.studentReg_DTObj[0].pasR_FatherPresentState);

                $scope.reg.PASR_FatherPermanentState = promise.studentReg_DTObj[0].pasR_FatherPermanentState;

                $scope.reg.PASR_MotherPresentState = promise.studentReg_DTObj[0].pasR_MotherPresentState;
                getSelectGetStatemps($scope.reg.PASR_MotherNationality, promise.studentReg_DTObj[0].pasR_MotherPresentState);

                $scope.reg.PASR_MotherPermanentState = promise.studentReg_DTObj[0].pasR_MotherPermanentState;

                $scope.reg.PASR_PerCity = promise.studentReg_DTObj[0].pasR_PerCity;

                if (promise.studentReg_DTObj[0].pasR_PerPincode != 0) {
                    $scope.reg.PASR_PerPincode = promise.studentReg_DTObj[0].pasR_PerPincode;
                }
                $scope.reg.PASR_ConStreet = promise.studentReg_DTObj[0].pasR_ConStreet;
                $scope.reg.PASR_ConArea = promise.studentReg_DTObj[0].pasR_ConArea;
                $scope.reg.PASR_ConCountry = promise.studentReg_DTObj[0].pasR_ConCountry;
                getSelectGetState2($scope.reg.PASR_ConCountry, promise.studentReg_DTObj[0].pasR_ConState);
                //
                getSelectGetDistrict2($scope.reg.PASR_ConDistrict, promise.studentReg_DTObj[0].pasR_ConDistrict);


                // $scope.PASR_ConState = promise.studentReg_DTObj[0].pasR_ConState;

                $scope.reg.PASR_ConCity = promise.studentReg_DTObj[0].pasR_ConCity;

                if (promise.studentReg_DTObj[0].pasR_ConPincode != 0) {
                    $scope.reg.PASR_ConPincode = promise.studentReg_DTObj[0].pasR_ConPincode;
                }


                if (promise.studentReg_DTObj[0].pasR_AadharNo != 0) {
                    $scope.reg.PASR_AadharNo = promise.studentReg_DTObj[0].pasR_AadharNo;
                }
                if (promise.studentReg_DTObj[0].pasR_MotherAadharNo != 0) {
                    $scope.reg.pasR_MotherAadharNo = promise.studentReg_DTObj[0].pasR_MotherAadharNo;
                }
                if (promise.studentReg_DTObj[0].pasR_FatherAadharNo != 0) {
                    $scope.reg.pasR_FatherAadharNo = promise.studentReg_DTObj[0].pasR_FatherAadharNo;
                }
                $scope.reg.PASR_AccountNo = promise.studentReg_DTObj[0].pasR_AccountNo;
                $scope.reg.PASR_BankName = promise.studentReg_DTObj[0].pasR_BankName;
                $scope.reg.PASR_BranchName = promise.studentReg_DTObj[0].pasR_BranchName;
                $scope.reg.PASR_BranchName = promise.studentReg_DTObj[0].pasR_BranchName;
                $scope.reg.PASR_IFSCCode = promise.studentReg_DTObj[0].pasR_IFSCCode;
                $scope.reg.PASR_Domicile = promise.studentReg_DTObj[0].pasR_Domicile;
                $scope.reg.PASR_SchoolDISECode = promise.studentReg_DTObj[0].pasR_SchoolDISECode;
                $scope.reg.PASR_MedicalComplaints = promise.studentReg_DTObj[0].pasR_MedicalComplaints;
                $scope.reg.PASR_NewlyAdmisstedFlg = promise.studentReg_DTObj[0].pasR_NewlyAdmisstedFlg;
                $scope.reg.PASR_OtherInformations = promise.studentReg_DTObj[0].pasR_OtherInformations;
                $scope.reg.PASR_NoOfDependencies = promise.studentReg_DTObj[0].pasR_NoOfDependencies;
                $scope.reg.PASR_VaccinatedFlg = promise.studentReg_DTObj[0].pasR_VaccinatedFlg;
                $scope.reg.PASR_Tcflag = promise.studentReg_DTObj[0].pasR_Tcflag;


                $scope.reg.PASR_Distirct = promise.studentReg_DTObj[0].pasR_Distirct;
                $scope.reg.PASR_Taluk = promise.studentReg_DTObj[0].pasR_Taluk;
                $scope.reg.PASR_Village = promise.studentReg_DTObj[0].pasR_Village;
                $scope.reg.PASR_Languagespeaking = promise.studentReg_DTObj[0].pasR_Languagespeaking;
                $scope.reg.PASR_FatherPanno = promise.studentReg_DTObj[0].pasR_FatherPanno;
                $scope.reg.PASR_MotherPanno = promise.studentReg_DTObj[0].pasR_MotherPanno;
                $scope.reg.PASR_Stayingwith = promise.studentReg_DTObj[0].pasR_Stayingwith;
                if ($scope.reg.PASR_Stayingwith == 'Both Perents') {
                    $scope.StayType = 1;
                }
                else if ($scope.reg.PASR_Stayingwith == 'Father') {
                    $scope.StayType = 2;
                }
                else if ($scope.reg.PASR_Stayingwith == 'Mother') {
                    $scope.StayType = 3;
                }
                else if ($scope.reg.PASR_Stayingwith == 'Guardian') {
                    $scope.StayType = 4;
                }
                else {
                    $scope.reg.PASR_Alumani = '';
                }


                $scope.reg.PASR_MobileNo = promise.studentReg_DTObj[0].pasR_MobileNo;
                $scope.reg.PASR_emailId = promise.studentReg_DTObj[0].pasR_emailId;

                $scope.reg.PASR_MaritalStatus = promise.studentReg_DTObj[0].pasR_MaritalStatus;
                $scope.reg.PASR_FatherMaritalStatus = promise.studentReg_DTObj[0].pasR_FatherMaritalStatus;
                $scope.reg.PASR_MotherMaritalStatus = promise.studentReg_DTObj[0].pasR_MotherMaritalStatus;
                $scope.reg.PASR_FatherAliveFlag = promise.studentReg_DTObj[0].pasR_FatherAliveFlag;
                $scope.reg.PASR_FatherName = promise.studentReg_DTObj[0].pasR_FatherName;
                $scope.reg.PASR_FatherAadharNo = promise.studentReg_DTObj[0].pasR_FatherAadharNo;
                $scope.reg.PASR_FatherSurname = promise.studentReg_DTObj[0].pasR_FatherSurname;
                $scope.reg.PASR_FatherEducation = promise.studentReg_DTObj[0].pasR_FatherEducation;
                $scope.reg.PASR_FatherPermanentPinCode = promise.studentReg_DTObj[0].pasR_FatherPermanentPinCode;

                $scope.reg.PASR_FatherPresentAddress = promise.studentReg_DTObj[0].pasR_FatherPresentAddress;
                $scope.reg.PASR_FatherPresentCity = promise.studentReg_DTObj[0].pasR_FatherPresentCity;
                $scope.reg.PASR_FatherPresentPS = promise.studentReg_DTObj[0].pasR_FatherPresentPS;
                $scope.reg.PASR_FatherPresentPO = promise.studentReg_DTObj[0].pasR_FatherPresentPO;
                $scope.reg.PASR_FatherPresentPinCode = promise.studentReg_DTObj[0].pasR_FatherPresentPinCode;

                $scope.reg.PASR_MotherPresentAddress = promise.studentReg_DTObj[0].pasR_MotherPresentAddress;
                $scope.reg.PASR_MotherPresentCity = promise.studentReg_DTObj[0].pasR_MotherPresentCity;
                $scope.reg.PASR_MotherPresentPS = promise.studentReg_DTObj[0].pasR_MotherPresentPS;
                $scope.reg.PASR_MotherPresentPO = promise.studentReg_DTObj[0].pasR_MotherPresentPO;
                $scope.reg.PASR_MotherPresentPinCode = promise.studentReg_DTObj[0].pasR_MotherPresentPinCode;

                $scope.reg.PASR_MotherPermanentAddress = promise.studentReg_DTObj[0].pasR_MotherPermanentAddress;
                $scope.reg.PASR_MotherPermanentCity = promise.studentReg_DTObj[0].pasR_MotherPermanentCity;
                $scope.reg.PASR_MotherPermanentPS = promise.studentReg_DTObj[0].pasR_MotherPermanentPS;
                $scope.reg.PASR_MotherPermanentPO = promise.studentReg_DTObj[0].pasR_MotherPermanentPO;
                $scope.reg.PASR_MotherPermanentPinCode = promise.studentReg_DTObj[0].pasR_MotherPermanentPinCode;

                $scope.reg.PASR_FatherPermanentAddress = promise.studentReg_DTObj[0].pasR_FatherPermanentAddress;
                $scope.reg.PASR_FatherPermanentCity = promise.studentReg_DTObj[0].pasR_FatherPermanentCity;
                $scope.reg.PASR_FatherPermanentPS = promise.studentReg_DTObj[0].pasR_FatherPermanentPS;
                $scope.reg.PASR_FatherPermanentPO = promise.studentReg_DTObj[0].pasR_FatherPermanentPO;
                $scope.reg.PASR_FatherOccupation = promise.studentReg_DTObj[0].pasR_FatherOccupation;

                $scope.reg.PASR_FatherBankName = promise.studentReg_DTObj[0].pasR_FatherBankName;
                $scope.reg.PASR_FatherBankBranch = promise.studentReg_DTObj[0].pasR_FatherBankBranch;
                $scope.reg.PASR_FatherIFSC = promise.studentReg_DTObj[0].pasR_FatherIFSC;
                $scope.reg.PASR_FatherBankAccount = promise.studentReg_DTObj[0].pasR_FatherBankAccount;

                $scope.reg.PASR_MotherBankName = promise.studentReg_DTObj[0].pasR_MotherBankName;
                $scope.reg.PASR_MotherBankBranch = promise.studentReg_DTObj[0].pasR_MotherBankBranch;
                $scope.reg.PASR_MotherIFSC = promise.studentReg_DTObj[0].pasR_MotherIFSC;
                $scope.reg.PASR_MotherBankAccount = promise.studentReg_DTObj[0].pasR_MotherBankAccount;

                $scope.reg.PASR_FatherChurchAffiliation = promise.studentReg_DTObj[0].pasR_FatherChurchAffiliation;
                $scope.reg.PASR_FatherSelfEmployedFlg = promise.studentReg_DTObj[0].pasR_FatherSelfEmployedFlg;


                $scope.allRefrence = promise.studentReferenceDetails;
                $scope.allSources = promise.studentSourceDetails;

                //$scope.allRefrence = promise.edit_StudentReferenceDetails;
                //$scope.allSources = promise.edit_StudentSourceDetails;
                //$scope.referenceId = 0;
                //$scope.sourceId = 0;
                //if (promise.edit_StudentReferenceDetails != null && promise.edit_StudentReferenceDetails.length > 0) {
                //    $scope.referenceId = $scope.edit_StudentReferenceDetails[0].pamR_Id;
                //}
                //if ($scope.edit_StudentSourceDetails != null && $scope.edit_StudentSourceDetails.length > 0) {
                //    $scope.sourceId = $scope.edit_StudentSourceDetails[0].pamS_Id;
                //}

                if (promise.edit_StudentReferenceDetails != null && promise.edit_StudentReferenceDetails.length > 0) {
                    angular.forEach($scope.allRefrence, function (role) {
                        angular.forEach(promise.edit_StudentReferenceDetails, function (erole) {
                            if (role.pamR_Id == erole.pamR_Id) {
                                role.Selected = true;
                            }

                        });
                    });
                }

                if (promise.edit_StudentSourceDetails != null && promise.edit_StudentSourceDetails.length > 0) {
                    angular.forEach($scope.allSources, function (role) {
                        angular.forEach(promise.edit_StudentSourceDetails, function (erole) {
                            if (role.pamS_Id == erole.pamS_Id) {
                                role.Selected = true;
                            }

                        });
                    });
                }

                if ($scope.reg.PASR_FatherOccupation == 'Government Employee') {
                    $scope.ReportType = 1;
                }
                else if ($scope.reg.PASR_FatherOccupation == 'Private Employee') {
                    $scope.ReportType = 2;
                }
                else if ($scope.reg.PASR_FatherOccupation == 'Self Employee') {
                    $scope.ReportType = 3;
                }
                else {
                    $scope.ReportType = 4;
                }

                $scope.reg.PASR_FatherDesignation = promise.studentReg_DTObj[0].pasR_FatherDesignation;
                $scope.reg.PASR_FatherIncome = promise.studentReg_DTObj[0].pasR_FatherIncome;
                $scope.reg.PASR_FatherMonIncome = promise.studentReg_DTObj[0].pasR_FatherMonIncome;
                $scope.reg.PASR_FatherAnnIncome = promise.studentReg_DTObj[0].pasR_FatherAnnIncome;
                $scope.reg.PASR_FatherMobleNo = promise.studentReg_DTObj[0].pasR_FatherMobleNo;


                $scope.reg.PASR_Churchname = promise.studentReg_DTObj[0].pasR_ChurchName;
                $scope.reg.PASR_ChurchAddress = promise.studentReg_DTObj[0].pasR_ChurchAddress;
                $scope.reg.PASR_FatherOfficePhNo = promise.studentReg_DTObj[0].pasR_FatherOfficePhNo;
                $scope.reg.PASR_FatherHomePhNo = promise.studentReg_DTObj[0].pasR_FatherHomePhNo;
                $scope.reg.PASR_MotherOfficePhNo = promise.studentReg_DTObj[0].pasR_MotherOfficePhNo;
                $scope.reg.PASR_MotherHomePhNo = promise.studentReg_DTObj[0].pasR_MotherHomePhNo;

                $scope.reg.PASR_FatherPassingYear = promise.studentReg_DTObj[0].pasR_FatherPassingYear;
                $scope.reg.PASR_MotherPassingYear = promise.studentReg_DTObj[0].pasR_MotherPassingYear;





                //  $scope.concessionconfirmsibling = promise.concessionconfirmsibling;


                if (promise.studentReg_DTObj[0].pasR_FatherNationality > 0) {
                    $scope.reg.PASR_FatherNationality = promise.studentReg_DTObj[0].pasR_FatherNationality;
                }

                $scope.reg.PASR_FatherOfficeAddr = promise.studentReg_DTObj[0].pasR_FatherOfficeAddr;
                if (promise.studentReg_DTObj[0].pasR_AltContactNo != 0) {
                    $scope.reg.PASR_AltContactNo = promise.studentReg_DTObj[0].pasR_AltContactNo;
                }

                $scope.reg.PASR_AltContactEmail = promise.studentReg_DTObj[0].pasR_AltContactEmail;

                $scope.reg.PASR_FatheremailId = promise.studentReg_DTObj[0].pasR_FatheremailId;
                $scope.reg.PASR_MotherAliveFlag = promise.studentReg_DTObj[0].pasR_MotherAliveFlag;
                $scope.reg.PASR_MotherName = promise.studentReg_DTObj[0].pasR_MotherName;
                $scope.reg.PASR_MotherAadharNo = promise.studentReg_DTObj[0].pasR_MotherAadharNo;
                $scope.reg.PASR_MotherSurname = promise.studentReg_DTObj[0].pasR_MotherSurname;
                $scope.reg.PASR_MotherEducation = promise.studentReg_DTObj[0].pasR_MotherEducation;
                $scope.reg.PASR_MotherOccupation = promise.studentReg_DTObj[0].pasR_MotherOccupation;
                $scope.reg.PASR_MotherChurchAffiliation = promise.studentReg_DTObj[0].pasR_MotherChurchAffiliation;
                $scope.reg.PASR_MotherSelfEmployedFlg = promise.studentReg_DTObj[0].pasR_MotherSelfEmployedFlg;
                if ($scope.reg.PASR_MotherOccupation == 'Government Employee') {
                    $scope.ReportTypem = 1;
                }
                else if ($scope.reg.PASR_MotherOccupation == 'Private Employee') {
                    $scope.ReportTypem = 2;
                }
                else if ($scope.reg.PASR_MotherOccupation == 'Self Employee') {
                    $scope.ReportTypem = 3;
                }
                else {
                    $scope.ReportTypem = 4;
                }
                $scope.reg.PASR_MotherDesignation = promise.studentReg_DTObj[0].pasR_MotherDesignation;
                $scope.reg.PASR_MotherIncome = promise.studentReg_DTObj[0].pasR_MotherIncome;
                $scope.reg.PASR_MotherMonIncome = promise.studentReg_DTObj[0].pasR_MotherMonIncome;
                $scope.reg.PASR_MotherAnnIncome = promise.studentReg_DTObj[0].pasR_MotherAnnIncome;
                $scope.reg.PASR_MotherMobleNo = promise.studentReg_DTObj[0].pasR_MotherMobleNo;

                if (promise.studentReg_DTObj[0].pasR_MotherNationality > 0) {
                    $scope.reg.PASR_MotherNationality = promise.studentReg_DTObj[0].pasR_MotherNationality;
                }
                $scope.reg.PASR_MotherOfficeAddr = promise.studentReg_DTObj[0].pasR_MotherOfficeAddr;
                $scope.reg.PASR_MotheremailId = promise.studentReg_DTObj[0].pasR_MotheremailId;
                //"PASR_TotalIncome": $scope.PASR_TotalIncome,
                $scope.reg.PASR_BirthPlace = promise.studentReg_DTObj[0].pasR_BirthPlace;
                $scope.reg.PASR_Nationality = promise.studentReg_DTObj[0].pasR_Nationality;

                if (promise.studentReg_DTObj[0].pasR_HostelReqdFlag == 1) {
                    $scope.reg.PASR_HostelReqdFlag = 1;
                }
                else {
                    $scope.reg.PASR_HostelReqdFlag = 0;
                }

                if (promise.studentReg_DTObj[0].pasR_TransportReqdFlag == 1) {
                    $scope.reg.PASR_TransportReqdFlag = 1;
                }
                else {
                    $scope.reg.PASR_TransportReqdFlag = 0;
                }

                if (promise.studentReg_DTObj[0].pasR_Studentillness == true) {
                    $scope.reg.PASR_Studentillness = 1;
                }
                else {
                    $scope.reg.PASR_Studentillness = 0;
                }

                $scope.reg.PASR_PovertyLine = promise.studentReg_DTObj[0].pasR_PovertyLine;

                $scope.reg.PASR_NickName = promise.studentReg_DTObj[0].pasR_NickName;
                $scope.reg.PASR_FatherHobbies = promise.studentReg_DTObj[0].pasR_FatherHobbies;
                $scope.reg.PASR_MotherHobbies = promise.studentReg_DTObj[0].pasR_MotherHobbies;
                $scope.reg.PASR_NeighbourPhoneNo = promise.studentReg_DTObj[0].pasR_NeighbourPhoneNo;
                $scope.reg.PASR_NeighbourAddr1 = promise.studentReg_DTObj[0].pasR_NeighbourAddr1;
                $scope.reg.PASR_NeighbourAddr2 = promise.studentReg_DTObj[0].pasR_NeighbourAddr2;
                $scope.reg.PASR_NeighbourName = promise.studentReg_DTObj[0].pasR_NeighbourName;
                $scope.reg.PASR_PhysicalDisability = promise.studentReg_DTObj[0].pasR_PhysicalDisability;

                $scope.reg.PASR_GymReqdFlag = promise.studentReg_DTObj[0].pasR_GymReqdFlag;
                $scope.reg.PASR_ECSFlag = promise.studentReg_DTObj[0].pasR_ECSFlag;
                $scope.reg.PASR_SibblingConcessionFlag = promise.studentReg_DTObj[0].pasR_SibblingConcessionFlag;
                $scope.reg.PASR_ParentConcessionFlag = promise.studentReg_DTObj[0].pasR_ParentConcessionFlag;

                if (promise.syllabuslist != null) {
                    $scope.syllabuslist = promise.syllabuslist;
                    $scope.selectedsy = promise.studentReg_DTObj[0].asmsT_Id;
                    $scope.reg.ASMST_Id = promise.studentReg_DTObj[0].asmsT_Id;
                    $scope.showstream = true;
                }

                if (promise.studentReg_DTObj[0].pasR_Noofbrothers > 0) {
                    $scope.reg.PASR_Noofbrothers = promise.studentReg_DTObj[0].pasR_Noofbrothers;
                }
                if (promise.studentReg_DTObj[0].pasR_Noofsisters > 0) {
                    $scope.reg.PASR_Noofsisters = promise.studentReg_DTObj[0].pasR_Noofsisters;
                }
                $scope.reg.PASR_NoOfElderBrothers = promise.studentReg_DTObj[0].pasR_NoOfElderBrothers;
                $scope.reg.PASR_NoOfYoungerBrothers = promise.studentReg_DTObj[0].pasR_NoOfYoungerBrothers;
                $scope.reg.PASR_NoOfElderSisters = promise.studentReg_DTObj[0].pasR_NoOfElderSisters;
                $scope.reg.PASR_NoOfYoungerSisters = promise.studentReg_DTObj[0].pasR_NoOfYoungerSisters;
                //$scope.sourcedropDown = promise.sourcedropDown;
                $scope.reg.PAMS_Id = promise.pamsId;
                $scope.reg.PASR_lastclassperc = promise.studentReg_DTObj[0].pasR_lastclassperc;
                $scope.reg.PASR_FatherReligion = promise.studentReg_DTObj[0].pasR_FatherReligion;
                $scope.reg.PASR_MotherReligion = promise.studentReg_DTObj[0].pasR_MotherReligion;
                $scope.reg.PASR_MotherCaste = promise.studentReg_DTObj[0].pasR_MotherCaste;
                $scope.reg.PASR_FatherCaste = promise.studentReg_DTObj[0].pasR_FatherCaste;
                $scope.reg.fatherSubCaste_Id = promise.studentReg_DTObj[0].pasR_Fathersubcaste;
                $scope.reg.motherSubCaste_Id = promise.studentReg_DTObj[0].pasR_Mothersubcatse;
                $scope.reg.SubCaste_Id = promise.studentReg_DTObj[0].pasR_Subcaste;
                $scope.reg.PASR_Tribe = promise.studentReg_DTObj[0].pasR_Tribe;
                $scope.reg.PASR_FatherTribe = promise.studentReg_DTObj[0].pasR_FatherTribe;
                $scope.reg.PASR_MotherTribe = promise.studentReg_DTObj[0].pasR_MotherTribe;
                $scope.reg.PASR_FirstLanguage = promise.studentReg_DTObj[0].pasR_FirstLanguage;
                $scope.reg.PASR_SecondLanguage = promise.studentReg_DTObj[0].pasR_SecondLanguage;
                $scope.reg.PASR_Thirdlanguage = promise.studentReg_DTObj[0].pasR_Thirdlanguage;
                $scope.reg.PASR_FatherPhoto = promise.studentReg_DTObj[0].pasR_FatherPhoto;
                $scope.reg.PASR_MotherPhoto = promise.studentReg_DTObj[0].pasR_MotherPhoto;

                $scope.reg.PASA_MedicalFitForSportsFlg = promise.studentReg_DTObj[0].pasA_MedicalFitForSportsFlg;
                $scope.reg.PASA_HealthIssueFlg = promise.studentReg_DTObj[0].pasA_HealthIssueFlg;
                if ($scope.reg.PASA_MedicalFitForSportsFlg == true) {
                    $scope.reg.PASA_MedicalFitForSportsFlg = "true";
                }
                else {
                    $scope.reg.PASA_MedicalFitForSportsFlg = "false";
                }
                $scope.reg.PASA_JoiningReasons = promise.studentReg_DTObj[0].pasA_JoiningReasons;
                if ($scope.reg.PASA_HealthIssueFlg == true) {
                    $scope.reg.PASA_HealthIssueFlg = "true";
                }
                else {
                    $scope.reg.PASA_HealthIssueFlg = "false";
                }

                $scope.reg.PASR_UndertakingFlag = promise.studentReg_DTObj[0].pasR_UndertakingFlag;
                if ($scope.reg.PASR_UndertakingFlag == 1) {
                    $scope.reg.PASR_UndertakingFlag = true;
                }
                else {
                    $scope.reg.PASR_UndertakingFlag = false;
                }
                //// guardian
                if (promise.studentGuardian_DTObj != null && promise.studentGuardian_DTObj != "") {
                    $scope.reg.pasrG_Id = promise.studentGuardian_DTObj[0].pasrG_Id;
                    $scope.reg.PASRG_GuardianName = promise.studentGuardian_DTObj[0].pasrG_GuardianName;
                    $scope.reg.PASRG_GuardianAddress = promise.studentGuardian_DTObj[0].pasrG_GuardianAddress;
                    $scope.reg.PASRG_GuardianRelation = promise.studentGuardian_DTObj[0].pasrG_GuardianRelation;
                    $scope.reg.PASRG_emailid = promise.studentGuardian_DTObj[0].pasrG_emailid;
                    $scope.reg.PASRG_GuardianPhoneNo = promise.studentGuardian_DTObj[0].pasrG_GuardianPhoneNo;
                    $scope.reg.PASRG_PhoneOffice = promise.studentGuardian_DTObj[0].pasrG_PhoneOffice;
                    $scope.reg.PASRG_FeeUndertakeFlg = promise.studentGuardian_DTObj[0].pasrG_FeeUndertakeFlg;
                    if ($scope.reg.PASRG_PhoneOffice == 0) {
                        $scope.reg.PASRG_PhoneOffice = "";
                    }
                    if ($scope.reg.PASRG_GuardianPhoneNo == 0) {
                        $scope.reg.PASRG_GuardianPhoneNo = "";
                    }
                    if ($scope.reg.PASRG_FeeUndertakeFlg == true) {
                        $scope.reg.PASRG_FeeUndertakeFlg = "1";
                    }
                    else {
                        $scope.reg.PASRG_FeeUndertakeFlg = "0";
                    }
                    $scope.reg.PASRG_Occupation = promise.studentGuardian_DTObj[0].pasrG_Occupation;
                    if ($scope.reg.PASRG_Occupation == 'Government Employee') {
                        $scope.ReportTypeg = 1;
                    }
                    else if ($scope.reg.PASRG_Occupation == 'Private Employee') {
                        $scope.ReportTypeg = 2;
                    }
                    else if ($scope.reg.PASRG_Occupation == 'Self Employee') {
                        $scope.ReportTypeg = 3;
                    }
                    else {
                        $scope.ReportTypeg = 4;
                    }


                }
                else {
                    $scope.reg.pasrG_Id = 0;
                    $scope.reg.PASRG_GuardianName = "";
                    $scope.reg.PASRG_GuardianAddress = "";
                    $scope.reg.PASRG_FeeUndertakeFlg = "";
                    $scope.reg.PASRG_GuardianRelation = "";
                    $scope.reg.PASRG_emailid = "";
                    $scope.reg.PASRG_GuardianPhoneNo = "";
                    $scope.reg.PASRG_PhoneOffice = "";
                    $scope.reg.PASRG_Occupation = "";
                    $scope.ReportTypeg = 0;
                }
                //// Sibling
                if (promise.studentSbling_DTObj.length > 0) {
                    $scope.siblings = promise.studentSbling_DTObj;
                }
                else {
                    $scope.siblings = [];
                    $scope.Sib = true;
                    $scope.siblings = [{ id: 'sibling1' }];
                    $scope.addNewsibling = function () {
                        var newItemNo = $scope.siblings.length + 1;
                        if (newItemNo <= 5) {
                            $scope.siblings.push({ 'id': 'sibling' + newItemNo });
                        }
                    };
                }
                //// Subjects
                if (promise.studentSubjects_DTObj != null && promise.studentSubjects_DTObj.length > 0) {
                    //  $scope.electivesubgrouplist = promise.StudentSubjects_DTObj;
                    $scope.electivegrouplist = promise.electivegrouplist;
                    $scope.electivesubgrouplist = promise.electivesubgrouplist;
                    angular.forEach(promise.studentSubjects_DTObj, function (opqr) {
                        angular.forEach($scope.electivesubgrouplist, function (opqr1) {
                            if (opqr.emG_Id == opqr1.EMG_Id) {
                                opqr1.ismS_Id = opqr.ismS_Id;
                            }
                        });
                    });
                    $scope.tempaarry = $scope.electivesubgrouplist;
                    $scope.grouplength = $scope.electivesubgrouplist.length;
                }
                else {
                    $scope.electivesubgrouplist = {};
                }
                if ($scope.electivesubgrouplist != null && $scope.electivesubgrouplist.length >= 0) {
                    $scope.electives = [];
                    $scope.compulsory = [];
                    angular.forEach($scope.electivesubgrouplist, function (opqr1) {
                        if (opqr1.EMG_ElectiveFlg == false) {
                            $scope.compulsory.push(opqr1);
                        }
                        else {
                            $scope.electives.push(opqr1);
                        }
                    });
                }
                if (promise.streamexams != null && promise.streamexams.length > 0) {
                    $scope.streamexams = promise.streamexams;
                    $scope.reg.ASMCE_Id = promise.asmcE_Id;
                    $scope.ASMCE_Id = promise.asmcE_Id;
                }
                else {
                    $scope.streamexams = [];
                }
                // source and reference
                //$scope.allRefrence = promise.allRefrence;
                //$scope.allSources = promise.allSources;



                //documnets
                if (promise.studentDocuments_DTObj.length > 0) {
                    //$scope.document = {};
                    //$scope.documentList = promise.studentDocuments_DTObj;
                    angular.forEach($scope.documentList, function (value, key) {
                        $('#' + value.amsmD_Id).removeAttr('src');
                        $scope.documentList[key].document_Path = '';
                        angular.forEach(promise.studentDocuments_DTObj, function (value1, key1) {
                            //$scope.document.pasrD_Id = value.pasrD_Id;
                            //$scope.document.pasR_Id = value.pasR_Id;
                            //$scope.document.amsmD_Id = value.amsmD_Id;
                            //$scope.document.amsmD_DocumentName = value.amsmD_DocumentName;
                            //$scope.document.document_Path = value.document_Path;
                            if (value.amsmD_Id == value1.amsmD_Id) {
                                $('#' + value.amsmD_Id).attr('src', value1.document_Path);
                                $scope.documentList[key].document_Path = value1.document_Path;
                                $scope.documentList[key].pasrD_Id = value1.pasrD_Id;
                                $scope.documentList[key].pasR_Id = value1.pasR_Id;
                                var img = value1.document_Path;
                                var imagarr = img.split('.');
                                var lastelement = imagarr[imagarr.length - 1];
                                $scope.documentList[key].filetype = lastelement;
                            }
                        });
                    });
                };
                angular.forEach($scope.caste_doc_map_list, function (opqr) {
                    angular.forEach($scope.documentList, function (opqr1) {
                        if (opqr1.amsmD_Id == opqr.amsmD_Id) {
                            opqr1.amsmD_FLAG = false;
                        }
                    });
                });
                angular.forEach($scope.caste_doc_map_list, function (opqr) {
                    if (opqr.imC_Id == promise.studentReg_DTObj[0].caste_Id) {
                        angular.forEach($scope.documentList, function (opqr1) {
                            if (opqr1.amsmD_Id == opqr.amsmD_Id) {
                                opqr1.amsmD_FLAG = true;
                            }
                        });
                    };
                });
                //// Previous School
                if (promise.studentPrevSch_DTObj.length > 0) {
                    $scope.PreviousSchoolList = promise.studentPrevSch_DTObj;
                }
                if (promise.studentPrevSch_DTObj.length > 0) {
                    $scope.PreviousSchool = {};
                    $scope.PreviousSchoolList = promise.studentPrevSch_DTObj;
                    angular.forEach(promise.studentPrevSch_DTObj, function (value, key) {
                        $scope.PreviousSchool.PASRPS_Id = value.pasrpS_Id;
                        $scope.PreviousSchool.PASRPS_PrvSchoolName = value.pasrpS_PrvSchoolName;
                        $scope.PreviousSchool.PASRPS_PreviousClass = value.pasrpS_PreviousClass;
                        $scope.PreviousSchool.PASRPS_PreviousPer = value.pasrpS_PreviousPer;
                        $scope.PreviousSchool.PASRPS_PreviousGrade = value.pasrpS_PreviousGrade;
                        $scope.PreviousSchool.PASRPS_LeftYear = value.pasrpS_LeftYear;
                        $scope.PreviousSchool.PASRPS_PreviousMarks = value.pasrpS_PreviousMarks;
                        $scope.PreviousSchool.PASRPS_Board = value.pasrpS_Board;
                        $scope.PreviousSchool.PASRPS_Address = value.pasrpS_Address;
                        $scope.PreviousSchool.PASRPS_TcIssueDate = value.pasrpS_TcIssueDate;
                        $scope.PreviousSchool.PASRPS_TCProducingDate = value.pasrpS_TCProducingDate;
                        $scope.PreviousSchool.PASRPS_TcNo = value.pasrpS_TcNo;
                        $scope.PreviousSchool.PASRPS_PreviousPer = value.pasrpS_PreviousPer;
                        $scope.reg.PASRPSLeftDate = new Date(value.pasrpS_LeftDate);
                        if (value.pasrpS_PrvSchoolName != null) {
                            $scope.reg.PASRPSLeftDate = new Date(value.pasrpS_LeftDate);
                        }
                        else {
                            $scope.reg.PASRPSLeftDate = new Date();
                        }
                        if (value.pasrpS_PrvSchoolName != null) {
                            $scope.reg.PASRPSTcIssueDate = new Date(value.pasrpS_TcIssueDate);
                        }
                        else {
                            $scope.reg.PASRPSTcIssueDate = new Date();
                        }

                        if (value.pasrpS_PrvSchoolName != null) {
                            $scope.reg.PASRPSTCProducingDate = new Date(value.pasrpS_TCProducingDate);
                        }
                        else {
                            $scope.reg.PASRPSTCProducingDate = new Date();
                        }

                        $scope.PreviousSchool.PASRPS_ConcOrScholarshipFlg = value.pasapS_ConcOrScholarshipFlg;
                        if (value.pasapS_ConcOrScholarshipFlg != null) {
                            $scope.reg.PASRPSConceScholarshipDate = new Date(value.pasrpS_ConcOrScholarshipDate);
                        }
                        else {
                            $scope.reg.PASRPSConceScholarshipDate = new Date();
                        }
                        //$scope.frdate = $filter('date')(Date.parse(value.pasrpS_LeftDate), 'yyyy-MM-dd');
                        //$scope.obj.qweqw = new Date($scope.frdate);
                        $scope.PreviousSchool.PASRPS_LeftReason = value.pasrpS_LeftReason;

                    });
                }
                else {
                    $scope.PreviousSchool = {};
                    $scope.PreviousSchoolList = [{ id: 'PreviousSchool1' }];
                    $scope.addNewPreviousSchool = function () {
                        var newItemNo = $scope.PreviousSchoolList.length + 1;

                        if (newItemNo <= 5) {
                            $scope.PreviousSchoolList.push({ 'id': 'PreviousSchool' + newItemNo });
                        }
                    };
                }
                var sibname = "";
                $scope.reg.FMCC_Id = promise.studentReg_DTObj[0].fmcC_ID;
                if ($scope.arrlist4 != null && $scope.arrlist4.length > 0) {
                    angular.forEach($scope.arrlist4, function (dd) {
                        if (dd.fmcC_Id == $scope.reg.FMCC_Id) {
                            sibname = dd.fmcC_ConcessionFlag;
                        }
                    })
                }
                $scope.concessioncon = promise.concessionconfirm;

                if (sibname == "S" && promise.studentReg_DTObj[0].pasrapS_ID != "787928") {
                    $scope.concessionconfirmsibling = true;
                }
                else {
                    $scope.concessionconfirmsibling = false;
                }
                //// Transport
                if (promise.studentTrns_DTObj != null && promise.studentTrns_DTObj != "") {
                    //$scope.reg.pasrT_Id = promise.studentTrns_DTObj[0].pasrT_Id;
                    //$scope.reg.PASRT_ivrmmcT_Id = promise.studentTrns_DTObj[0].pasrT_ivrmmcT_Id;
                    //$scope.reg.PASRT_cmR_Id = promise.studentTrns_DTObj[0].pasrT_cmR_Id;
                    //$scope.reg.PASRT_cmL_Id = promise.studentTrns_DTObj[0].pasrT_cmL_Id;
                    //$scope.reg.PASRT_consession_type_Id = promise.studentTrns_DTObj[0].pasrT_consession_type_Id;
                    //$scope.reg.PASRT_Daughter = promise.studentTrns_DTObj[0].pasrT_Daughter;
                    //$scope.reg.PASRT_Son = promise.studentTrns_DTObj[0].pasrT_Son;
                    //$scope.reg.PASRT_Heared_Friend_Colleague = promise.studentTrns_DTObj[0].pasrT_Heared_Friend_Colleague;
                    //$scope.reg.PASRT_Internet = promise.studentTrns_DTObj[0].pasrT_Internet;
                    //$scope.reg.PASRT_Media = promise.studentTrns_DTObj[0].pasrT_Media;
                    //$scope.reg.PASRT_Other = promise.studentTrns_DTObj[0].pasrT_Other;
                    $scope.areaLst = promise.areaList;
                    $scope.reg.trmA_Id = promise.studentTrns_DTObj[0].trmA_Id;
                    $scope.trmA_Id = promise.studentTrns_DTObj[0].trmA_Id;
                    $scope.routelist = promise.routelist;

                    //$scope.onSelectarea($scope.trmA_Id);
                    $scope.trmR_Idp = promise.studentTrns_DTObj[0].pastA_PickUp_TRMR_Id;
                    $scope.reg.trmR_Idp = promise.studentTrns_DTObj[0].pastA_PickUp_TRMR_Id;

                    $scope.locationlist = promise.locationlist;
                    //$scope.onroutechangeload($scope.trmR_Idp);
                    $scope.trmL_Idp = promise.studentTrns_DTObj[0].pastA_PickUp_TRML_Id;
                    $scope.reg.trmL_Idp = promise.studentTrns_DTObj[0].pastA_PickUp_TRML_Id;

                }

                if (promise.studentEmploye != null && promise.studentEmploye.length > 0) {
                    $scope.fillstaff = promise.fillstaff;
                    $scope.hrmeid = promise.studentEmploye[0].hrmE_ID;
                    $scope.reg.hrmE_Id = promise.studentEmploye[0].hrmE_ID;
                }
                else {
                    $scope.fillstaff = {};
                }



                //if (($scope.reg.PASR_PerStreet == $scope.reg.PASR_ConStreet) && ($scope.reg.PASR_PerArea == $scope.reg.PASR_ConArea) && ($scope.reg.PASR_PerCountry == $scope.reg.PASR_ConCountry) && ($scope.reg.PASR_PerState == $scope.reg.PASR_ConState) && ($scope.reg.PASR_PerCity == $scope.reg.PASR_ConCity) && ($scope.reg.PASR_PerPincode == $scope.reg.PASR_PerPincode)) {

                //    $scope.chkbox_address = 1;
                //    $scope.PermanentDis = true;
                //    $scope.CommunicationAdDis = true;
                //}
                //else {
                //    $scope.chkbox_address = 0;
                //    $scope.PermanentDis = false;
                //    $scope.CommunicationAdDis = false;
                //}

                $scope.scroll();


            });
        }
        $scope.scroll = function () {
            $("html, body").animate({ scrollTop: 0 }, 1000);
        }

        // Search
        $scope.searchData = function () {
            var data = {
                "SearchData": $scope.search,
                "searchType": $scope.searchType
            }

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("StudentApplication/SearchData", data).then(function (promise) {
                // for pagination 
                $scope.currentPage = 1;
                $scope.itemsPerPage = paginationformasters;
                $scope.pages = promise.studentReg_DTObj;
                $scope.presentCountgrid = promise.registrationList.length;
                // for pagination
            });
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }
        }


        $scope.clearall = function () {
            if ($scope.formfilactive === true) {
                $state.reload();
            }

            for (var i1 = 0; i1 < $scope.allRefrence.length; i1++) {
                $scope.allRefrence[i1].Selected = false;
            }

            for (var i2 = 0; i2 < $scope.allSources.length; i2++) {
                $scope.allSources[i2].Selected = false;
            }
        }

        $scope.cancelload = function () {
            $state.reload();
        }

        $scope.cancel = function () {
            //$scope.scroll();
            //$state.reload();
            $scope.search = "";
            $('#blah').removeAttr('src');
            $scope.reg = {};
            $scope.reg.PASR_FatherSelfEmployedFlg = 0;
            $scope.reg.PASR_MotherSelfEmployedFlg = 4;
            $scope.drop_prospectous = false;
            $scope.documentList = [];
            $scope.PermanentDis = false;
            $scope.CommunicationAdDis = false;
            $scope.regformfour();
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.PreviousSchoolList = [];
            $scope.areaLst = [];
            $scope.routelist = [];
            $scope.electivesubgrouplist = [];
            $scope.reg.trmA_Id = "";
            $scope.PreviousSchoolList = [{ id: 'PreviousSchool1' }];
            $scope.addNewPreviousSchool = function () {
                var newItemNo = $scope.PreviousSchoolList.length + 1;

                if (newItemNo <= 5) {
                    $scope.PreviousSchoolList.push({ 'id': 'PreviousSchool' + newItemNo });
                }
            };
            $scope.siblings = [];
            $scope.Sib = true;
            $scope.siblings = [{ id: 'sibling1' }];
            $scope.addNewsibling = function () {
                var newItemNo = $scope.siblings.length + 1;

                if (newItemNo <= 5) {
                    $scope.siblings.push({ 'id': 'sibling' + newItemNo });
                }
            };
            $('#blah').attr('src', null);
            $scope.reg.PASR_Student_Pic_Path = null;

            $('#blahj').attr('src', null);
            $scope.reg.PASR_JointPhoto = null;

            $('#blahf').attr('src', null);
            $scope.reg.PASR_FatherPhoto = null;

            $('#blahm').attr('src', null);
            $scope.reg.PASR_MotherPhoto = null;
            $scope.ReportType = 0;
            $scope.ReportTypem = 0;
            $scope.ReportTypeg = 0;

        };

        $scope.submitted = false;

        $scope.savestudentdata = function (studentlst) {

            $scope.submitted = true;
            var flagg = false;
            //srkvs deepak
            $scope.studentid = 0;
            if (studentlst != undefined) {
                if (studentlst.Amst_Id == undefined) {
                    $scope.studentid = 0;
                }
                else {
                    $scope.studentid = studentlst.Amst_Id.pasP_Id;
                }
            }

            var prospectus_no = $scope.studentid;

            var now = new Date();
            var today = new Date(now.getYear(), now.getMonth(), now.getDate());

            var yearNow = now.getYear();
            var monthNow = now.getMonth();
            var dateNow = now.getDate();
            var doob = new Date($scope.reg.PASR_DOB);
            var dob = new Date(doob.getYear(6, 10),
                doob.getMonth(0, 2) - 1,
                doob.getDate(3, 5)
            );

            var yearDob = doob.getYear();
            var monthDob = doob.getMonth();
            var dateDob = doob.getDate();
            var age = {};
            var ageString = "";
            var yearString = "";
            var monthString = "";
            var dayString = "";

            var yearAge = "";
            yearAge = yearNow - yearDob;

            if (monthNow >= monthDob)
                var monthAge = monthNow - monthDob;
            else {
                yearAge--;
                monthAge = 12 + monthNow - monthDob;
            }

            if (dateNow >= dateDob)
                var dateAge = dateNow - dateDob;
            else {
                monthAge--;
                dateAge = 31 + dateNow - dateDob;

                if (monthAge < 0) {
                    monthAge = 11;
                    yearAge--;
                }
            }

            age = {
                years: yearAge

                // years: yearAge+"."+monthAge
                //months: monthAge,
                //days: dateAge
            };

            $scope.aggg = yearAge;

            angular.forEach($scope.myForm.$error.required, function (dd) {
                if (dd.$name == 'file1') {
                    flagg = true;
                }
            })
            if (flagg == false) {
                if ($scope.myForm.$valid) {

                    swal({
                        title: "Are you sure?",
                        text: "Do you want to Submit Details ..!?",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes!",
                        cancelButtonText: "Cancel!",
                        closeOnConfirm: false,
                        closeOnCancel: true
                    },
                        function (isConfirm) {
                            if (isConfirm) {
                                $scope.PreviousSchoolchckedIndexs = [];
                                angular.forEach($scope.PreviousSchoolList, function (itm) {
                                    if (itm.pasrpS_PreviousPer === "0.00" || itm.pasrpS_PreviousPer == null || itm.pasrpS_PreviousPer == undefined) {
                                        itm.pasrpS_PreviousPer = $scope.PreviousSchoolList.pasrpS_PreviousPer;
                                        $scope.PreviousSchoolchckedIndexs.push(itm);
                                    }
                                    else {
                                        $scope.PreviousSchoolchckedIndexs.push(itm);
                                    }
                                    if ($scope.reg.PASRPSLeftDate != null) {
                                        itm.pasrpS_LeftDate = new Date($scope.reg.PASRPSLeftDate).toDateString();
                                    }
                                    else {
                                        itm.pasrpS_LeftDate = "";
                                    }
                                    if ($scope.reg.PASRPSTcIssueDate != null) {
                                        itm.pasrpS_TcIssueDate = new Date($scope.reg.PASRPSTcIssueDate).toDateString();
                                    }
                                    else {
                                        itm.pasrpS_TcIssueDate = "";
                                    }
                                    if ($scope.reg.PASRPSTCProducingDate != null) {
                                        itm.pasrpS_TCProducingDate = new Date($scope.reg.PASRPSTCProducingDate).toDateString();
                                    }
                                    else {
                                        itm.pasrpS_TCProducingDate = "";
                                    }
                                    if ($scope.reg.PASRPSConceScholarshipDate != null) {
                                        itm.pasrpS_ConcOrScholarshipDate = new Date($scope.reg.PASRPSConceScholarshipDate).toDateString();;
                                    }
                                    else {
                                        itm.pasrpS_ConcOrScholarshipDate = "";
                                    }
                                });


                                if ($scope.allRefrence !== "" && $scope.allRefrence !== null) {
                                    if ($scope.allRefrence.length > 0) {
                                        for (var i = 0; i < $scope.allRefrence.length; i++) {
                                            if ($scope.allRefrence[i].Selected === true) {
                                                $scope.Refrencecheckboxchcked.push($scope.allRefrence[i]);
                                            }
                                        }
                                    }
                                }

                                if ($scope.allSources !== "" && $scope.allSources !== null) {
                                    if ($scope.allSources.length > 0) {
                                        for (var i1 = 0; i1 < $scope.allSources.length; i1++) {
                                            if ($scope.allSources[i1].Selected === true) {
                                                $scope.Sourcescheckboxchcked.push($scope.allSources[i1]);
                                            }
                                        }
                                    }
                                }

                                var RefrenceIds = $scope.Refrencecheckboxchcked;
                                var SourcesIds = $scope.Sourcescheckboxchcked;

                                var dobwords = new Date($scope.reg.PASR_DOB);
                                var monthid = dobwords.getMonth();
                                var dateid = dobwords.getDate();
                                var yearid = dobwords.getFullYear();
                                $scope.getmontnames(monthid);
                                $scope.getdatenames(dateid);
                                $scope.getyearname(yearid);
                                var DOBWords = datename + ' ' + monthname + ' ' + yearname;
                                var PASR_DOB = new Date($scope.reg.PASR_DOB).toDateString();
                                if ($scope.reg.PASR_ChurchBaptisedDate != undefined || $scope.reg.PASR_ChurchBaptisedDate != null) {
                                    var PASR_ChurchBaptisedDate = new Date($scope.reg.PASR_ChurchBaptisedDate).toDateString();
                                }
                                if ($scope.reg.CasteCategory_Id == undefined) {
                                    $scope.reg.CasteCategory_Id = 0;
                                }
                                if ($scope.reg.pasr_id == undefined) {
                                    $scope.reg.pasr_id = 0;
                                }
                                if ($scope.reg.pasrG_Id == undefined) {
                                    $scope.reg.pasrG_Id = 0;
                                }
                                if ($scope.ASMAY_Id == undefined) {
                                    $scope.ASMAY_Id = 0;
                                }
                                if ($scope.reg.Caste_Id == undefined || $scope.reg.Caste_Id == "") {
                                    $scope.reg.Caste_Id = 0;
                                }
                                if ($scope.reg.PASRT_cmR_Id == undefined) {
                                    $scope.reg.PASRT_cmR_Id = 0;
                                }
                                if ($scope.reg.PASRT_cmL_Id == undefined) {
                                    $scope.reg.PASRT_cmL_Id = 0;
                                }
                                if ($scope.reg.ASMST_Id == undefined || $scope.reg.ASMST_Id == null || $scope.reg.ASMST_Id == "") {
                                    $scope.reg.ASMST_Id = 0;
                                }
                                if ($scope.reg.PASRT_consession_type_Id == undefined) {
                                    $scope.reg.PASRT_consession_type_Id = 0;
                                }
                                if ($scope.reg.PASR_HostelReqdFlag == undefined || $scope.reg.PASR_HostelReqdFlag == false) {
                                    $scope.reg.PASR_HostelReqdFlag = 0;
                                }
                                else {
                                    $scope.reg.PASR_HostelReqdFlag = 1;
                                }
                                if ($scope.reg.PASR_TransportReqdFlag == undefined || $scope.reg.PASR_TransportReqdFlag == false) {
                                    $scope.reg.PASR_TransportReqdFlag = 0;
                                }
                                else {
                                    $scope.reg.PASR_TransportReqdFlag = 1;
                                }
                                if ($scope.reg.PASR_Studentillness == undefined || $scope.reg.PASR_Studentillness == false) {
                                    $scope.reg.PASR_Studentillness = 0;
                                }
                                else {
                                    $scope.reg.PASR_Studentillness = 1;
                                }

                                if ($scope.reg.PASR_GymReqdFlag == undefined || $scope.reg.PASR_GymReqdFlag == false) {
                                    $scope.reg.PASR_GymReqdFlag = 0;
                                } else {
                                    $scope.reg.PASR_GymReqdFlag = 1;
                                }
                                if ($scope.reg.PASR_ECSFlag == undefined || $scope.reg.PASR_ECSFlag == false) {
                                    $scope.reg.PASR_ECSFlag = 0;
                                } else {
                                    $scope.reg.PASR_ECSFlag = 1;
                                }
                                //if ($scope.reg.PASR_Adm_Confirm_Flag == undefined) {
                                //    $scope.reg.PASR_Adm_Confirm_Flag = false;
                                //}
                                if ($scope.reg.PASR_SibblingConcessionFlag == undefined) {
                                    $scope.reg.PASR_SibblingConcessionFlag = false;
                                }
                                if ($scope.reg.PASR_ParentConcessionFlag == undefined) {
                                    $scope.reg.PASR_ParentConcessionFlag = false;
                                }
                                if ($scope.reg.PASR_PaymentFlag == undefined) {
                                    $scope.reg.PASR_PaymentFlag = 0;
                                }
                                if ($scope.reg.PASR_FinalpaymentFlag == undefined) {
                                    $scope.reg.PASR_FinalpaymentFlag = 0;
                                }
                                if ($scope.reg.PASR_FatherAliveFlag == undefined) {
                                    $scope.reg.PASR_FatherAliveFlag = 0;
                                }
                                if ($scope.reg.PASR_MotherAliveFlag == undefined) {
                                    $scope.reg.PASR_MotherAliveFlag = 0;
                                }
                                if ($scope.reg.PASR_UndertakingFlag == undefined) {
                                    $scope.reg.PASR_UndertakingFlag = 0;
                                }
                                else if ($scope.reg.PASR_UndertakingFlag == true) {
                                    $scope.reg.PASR_UndertakingFlag = 1;
                                }
                                else if ($scope.reg.PASR_UndertakingFlag == false) {
                                    $scope.reg.PASR_UndertakingFlag = 0;
                                }
                                var PASR_AadharNo;
                                if ($scope.reg.PASR_AadharNo == undefined || $scope.reg.PASR_AadharNo == "") {
                                    PASR_AadharNo = "";
                                }
                                else {
                                    PASR_AadharNo = $scope.reg.PASR_AadharNo;
                                }
                                var PASR_AadharNofather;
                                if ($scope.reg.PASR_FatherAadharNo == "" || $scope.reg.PASR_FatherAadharNo == undefined) {
                                    PASR_AadharNofather = "";
                                }
                                else {
                                    PASR_AadharNofather = $scope.reg.PASR_FatherAadharNo;
                                }
                                var PASR_AadharNomother;
                                if ($scope.reg.PASR_MotherAadharNo == "" || $scope.reg.PASR_MotherAadharNo == undefined) {
                                    PASR_AadharNomother = "";
                                }
                                else {
                                    PASR_AadharNomother = $scope.reg.PASR_MotherAadharNo;
                                }
                                var PASR_AltContactNo;
                                if ($scope.reg.PASR_AltContactNo == undefined || $scope.reg.PASR_AltContactNo == "") {
                                    PASR_AltContactNo = 0;
                                }
                                else {
                                    PASR_AltContactNo = $scope.reg.PASR_AltContactNo;
                                }
                                if ($scope.reg.PASRT_Heared_Friend_Colleague == undefined || $scope.reg.PASRT_Heared_Friend_Colleague == "") {
                                    $scope.reg.PASRT_Heared_Friend_Colleague = 0;
                                }
                                if ($scope.reg.PASRT_Internet == undefined || $scope.reg.PASRT_Internet == "") {
                                    $scope.reg.PASRT_Internet = 0;
                                }
                                if ($scope.reg.PASRT_Media == undefined || $scope.reg.PASRT_Media == "") {
                                    $scope.reg.PASRT_Media = 0;
                                }
                                if ($scope.reg.PASRT_Other == undefined || $scope.reg.PASRT_Other == "") {
                                    $scope.reg.PASRT_Other = 0;
                                }
                                if ($scope.reg.PASR_SecondLanguage == undefined) {
                                    $scope.reg.PASR_SecondLanguage == "";
                                }
                                if ($scope.reg.PASR_Thirdlanguage == undefined) {
                                    $scope.reg.PASR_Thirdlanguage == "";
                                }
                                if ($scope.reg.PASR_Tribe == undefined) {
                                    $scope.reg.PASR_Tribe == "";
                                }
                                if ($scope.reg.PASR_FatherPhoto == undefined) {
                                    $scope.reg.PASR_FatherPhoto == "";
                                }
                                if ($scope.reg.PASR_MotherPhoto == undefined) {
                                    $scope.reg.PASR_MotherPhoto == "";
                                }
                                if ($scope.reg.PASR_FatherReligion == undefined || $scope.reg.PASR_FatherReligion == "" || $scope.reg.PASR_FatherReligion == null) {
                                    $scope.reg.PASR_FatherReligion == $scope.reg.Religion_Id;
                                }
                                if ($scope.reg.PASR_MotherReligion == undefined || $scope.reg.PASR_MotherReligion == "" || $scope.reg.PASR_MotherReligion == null) {
                                    $scope.reg.PASR_MotherReligion == $scope.reg.Religion_Id;
                                }
                                if ($scope.reg.PASR_MotherCaste == undefined || $scope.reg.PASR_MotherCaste == "" || $scope.reg.PASR_MotherCaste == null) {
                                    $scope.reg.PASR_MotherCaste == $scope.reg.Caste_Id;
                                }
                                if ($scope.reg.PASR_FatherCaste == undefined || $scope.reg.PASR_FatherCaste == "" || $scope.reg.PASR_FatherCaste == null) {
                                    $scope.reg.PASR_FatherCaste == $scope.reg.Caste_Id;
                                }
                                if ($scope.reg.PASR_MotherNationality == undefined || $scope.reg.PASR_MotherNationality == "" || $scope.reg.PASR_MotherNationality == null) {
                                    $scope.reg.PASR_MotherNationality == null;
                                }
                                if ($scope.reg.PASR_FatherNationality == undefined || $scope.reg.PASR_FatherNationality == "" || $scope.reg.PASR_FatherNationality == null) {
                                    $scope.reg.PASR_FatherNationality == null;
                                }
                                if ($scope.reg.PASR_ConCountry == undefined || $scope.reg.PASR_ConCountry == "" || $scope.reg.PASR_ConCountry == null) {
                                    $scope.reg.PASR_ConCountry == null;
                                }
                                if ($scope.reg.PASR_PerCountry == undefined || $scope.reg.PASR_PerCountry == "" || $scope.reg.PASR_PerCountry == null) {
                                    $scope.reg.PASR_PerCountry == null;
                                }
                                if ($scope.reg.PASR_ConState == undefined || $scope.reg.PASR_ConState == "" || $scope.reg.PASR_ConState == null) {
                                    $scope.reg.PASR_ConState == null;
                                }
                                if ($scope.reg.PASR_PerState == undefined || $scope.reg.PASR_PerState == "" || $scope.reg.PASR_PerState == null) {
                                    $scope.reg.PASR_PerState == null;
                                }
                                if ($scope.reg.PASR_MotherTribe == undefined || $scope.reg.PASR_MotherTribe == "") {
                                    $scope.reg.PASR_MotherTribe == $scope.reg.PASR_Tribe;
                                }
                                if ($scope.reg.PASR_FatherTribe == undefined || $scope.reg.PASR_FatherTribe == "") {
                                    $scope.reg.PASR_FatherTribe == $scope.reg.PASR_Tribe;
                                }
                                if ($scope.reg.admcheck == true) {
                                    $scope.reg.admcheck = "1";
                                }
                                else {
                                    $scope.reg.admcheck = "0";
                                }
                                if ($scope.reg.PASR_FatherSelfEmployedFlg == 1) {
                                    $scope.reg.PASR_FatherSelfEmployedFlg = true;
                                }
                                else {
                                    $scope.reg.PASR_FatherSelfEmployedFlg = false;
                                }
                                if ($scope.reg.PASR_MotherSelfEmployedFlg == 1) {
                                    $scope.reg.PASR_MotherSelfEmployedFlg = true;
                                }
                                else {
                                    $scope.reg.PASR_MotherSelfEmployedFlg = false;
                                }
                                if ($scope.reg.usercreation == true) {
                                    $scope.reg.usercreation = "1";
                                }
                                else {
                                    $scope.reg.usercreation = "0";
                                }
                                if ($scope.reg.trmA_Id == undefined || $scope.reg.trmA_Id == null) {
                                    $scope.reg.trmA_Id = 0;
                                }
                                if ($scope.reg.trmL_Idp == undefined || $scope.reg.trmL_Idp == null) {
                                    $scope.reg.trmL_Idp = 0;
                                }
                                if ($scope.reg.trmR_Idp == undefined || $scope.reg.trmR_Idp == null) {
                                    $scope.reg.trmR_Idp = 0;
                                }
                                if ($scope.reg.PAMS_Id == undefined || $scope.reg.PAMS_Id == null) {
                                    $scope.reg.PAMS_Id = 0;
                                }
                                var siblings = $scope.siblings;
                                if ($scope.reg.PASR_MiddleName == null) {
                                    $scope.reg.PASR_MiddleName = "";
                                }
                                if ($scope.reg.PASR_LastName == null) {
                                    $scope.reg.PASR_LastName = "";
                                }
                                if ($scope.reg.PASA_TransferrableJobFlg == "1") {
                                    $scope.reg.PASA_TransferrableJobFlg = true;
                                }
                                else {
                                    $scope.reg.PASA_TransferrableJobFlg = false;
                                }
                                if ($scope.reg.PASRG_FeeUndertakeFlg == "1") {
                                    $scope.reg.PASRG_FeeUndertakeFlg = true;
                                }
                                else {
                                    $scope.reg.PASRG_FeeUndertakeFlg = false;
                                }
                                var electivesub;
                                if ($scope.electivesubgrouplist == null || $scope.electivesubgrouplist.length == 0) {
                                    electivesub = [];
                                }
                                else {
                                    $scope.tempsub = [];
                                    angular.forEach($scope.electivesubgrouplist, function (opqr1) {
                                        if (opqr1.EMG_ElectiveFlg == true) {
                                            $scope.tempsub.push(opqr1);
                                        }
                                    });
                                    electivesub = $scope.tempsub;
                                }
                                if ($scope.reg.hrmE_Id == undefined && $scope.reg.hrmE_Id == null) {
                                    $scope.reg.hrmE_Id = 0;
                                }
                                if ($scope.reg.PASR_ConPincode == "" || $scope.reg.PASR_ConPincode == null || $scope.reg.PASR_ConPincode == undefined) {
                                    $scope.reg.PASR_ConPincode = 0;
                                }
                                if ($scope.reg.PASR_PerPincode == "" || $scope.reg.PASR_PerPincode == null || $scope.reg.PASR_PerPincode == undefined) {
                                    $scope.reg.PASR_PerPincode = 0;
                                }
                                if ($scope.PASR_Tcflag == undefined) {
                                    $scope.PASR_Tcflag = 0;
                                }
                                if ($scope.PASR_VaccinatedFlg == undefined) {
                                    $scope.PASR_VaccinatedFlg = 0;
                                }
                                if ($scope.reg.PASRG_GuardianPhoneNo == undefined || $scope.reg.PASRG_GuardianPhoneNo == null || $scope.reg.PASRG_GuardianPhoneNo == "") {
                                    $scope.reg.PASRG_GuardianPhoneNo = 0;
                                }
                                if ($scope.reg.PASRG_PhoneOffice == undefined || $scope.reg.PASRG_PhoneOffice == null || $scope.reg.PASRG_PhoneOffice == "") {
                                    $scope.reg.PASRG_PhoneOffice = 0;
                                }
                                if ($scope.reg.PASR_TotalIncome == undefined || $scope.reg.PASR_TotalIncome == null || $scope.reg.PASR_TotalIncome == "") {
                                    $scope.reg.PASR_TotalIncome = 0;
                                }
                                if ($scope.reg.PASR_PaymentDate == undefined || $scope.reg.PASR_PaymentDate == null || $scope.reg.PASR_PaymentDate == "") {
                                    $scope.reg.PASR_PaymentDate = null;
                                }
                                if ($scope.reg.PASR_Noofsisters == undefined || $scope.reg.PASR_Noofsisters == null || $scope.reg.PASR_Noofsisters == "") {
                                    $scope.reg.PASR_Noofsisters = null;
                                }
                                if ($scope.reg.PASR_Noofbrothers == undefined || $scope.reg.PASR_Noofbrothers == null || $scope.reg.PASR_Noofbrothers == "") {
                                    $scope.reg.PASR_Noofbrothers = null;
                                }
                                if ($scope.reg.PASR_ApplStatus == undefined || $scope.reg.PASR_ApplStatus == null || $scope.reg.PASR_ApplStatus == "") {
                                    $scope.reg.PASR_ApplStatus = null;
                                }
                                if ($scope.reg.PASR_AmountPaid == undefined || $scope.reg.PASR_AmountPaid == null || $scope.reg.PASR_AmountPaid == "") {
                                    $scope.reg.PASR_AmountPaid = 0;
                                }
                                if ($scope.reg.PASR_AmountPaid == undefined || $scope.reg.PASR_AmountPaid == null || $scope.reg.PASR_AmountPaid == "") {
                                    $scope.reg.PASR_AmountPaid = 0;
                                }
                                if ($scope.reg.PASRT_Son == undefined || $scope.reg.PASRT_Son == null || $scope.reg.PASRT_Son == "") {
                                    $scope.reg.PASRT_Son = 0;
                                }
                                if ($scope.reg.PASRT_Id == undefined || $scope.reg.PASRT_Id == null || $scope.reg.PASRT_Id == "") {
                                    $scope.reg.PASRT_Id = 0;
                                }
                                if ($scope.reg.PASRT_Daughter == undefined || $scope.reg.PASRT_Daughter == null || $scope.reg.PASRT_Daughter == "") {
                                    $scope.reg.PASRT_Daughter = 0;
                                }
                                if ($scope.reg.PASRT_Id == undefined || $scope.reg.PASRT_Id == null || $scope.reg.PASRT_Id == "") {
                                    $scope.reg.PASRT_Id = 0;
                                }
                                if ($scope.reg.PASR_ActiveFlag == undefined || $scope.reg.PASR_ActiveFlag == null || $scope.reg.PASR_ActiveFlag == "") {
                                    $scope.reg.PASR_ActiveFlag = 0;
                                }
                                if ($scope.reg.ASMCE_Id == undefined || $scope.reg.ASMCE_Id == null || $scope.reg.ASMCE_Id == "") {
                                    $scope.reg.ASMCE_Id = null;
                                }

                                $scope.electivelist_array = [];
                                angular.forEach($scope.electivelist, function (c) {
                                    if (c.selected == true) {
                                        $scope.electivelist_array.push({ ISMS_Id: c.ISMS_Id, EMG_Id: c.EMG_Id });
                                    }
                                });
                                var data = {
                                    "PASR_Id": $scope.reg.pasr_id,
                                    //srkvs deepak
                                    //"PASP_ProspectusNo": pasP_ProspectusNo,
                                    "PASP_Id": prospectus_no,
                                    "ASMCE_Id": $scope.reg.ASMCE_Id,
                                    "ASMAY_Id": $scope.reg.ASMAY_Id,
                                    "PASR_FirstName": $scope.reg.PASR_FirstName.toUpperCase(),
                                    "PASR_MiddleName": $scope.reg.PASR_MiddleName.toUpperCase(),
                                    "PASR_LastName": $scope.reg.PASR_LastName.toUpperCase(),
                                    "PASR_Date": $scope.reg.PASR_Date,
                                    "PASR_RegistrationNo": $scope.reg.PASR_RegistrationNo,
                                    "PASR_Sex": $scope.reg.PASR_Sex,
                                    "PASA_TransferrableJobFlg": $scope.reg.PASA_TransferrableJobFlg,
                                    "PASR_DOB": PASR_DOB,
                                    "PASR_ChurchBaptisedDate": PASR_ChurchBaptisedDate,
                                    "PASR_Age": $scope.aggg,
                                    "ASMCL_Id": Number($scope.reg.ASMCL_Id),
                                    "PASR_BloodGroup": $scope.reg.PASR_BloodGroup,
                                    "PASR_Medium": $scope.reg.medium,
                                    "PASR_Emisno": $scope.reg.PASR_Emisno,
                                    "PASR_Boarding": $scope.reg.PASR_Boarding,
                                    "PASR_MotherTongue": $scope.reg.PASR_MotherTongue,
                                    "Religion_Id": $scope.reg.Religion_Id,
                                    "CasteCategory_Id": $scope.reg.CasteCategory_Id,
                                    "Caste_Id": $scope.reg.Caste_Id,
                                    "PASR_BirthCertificateNo": $scope.reg.PASR_BirthCertificateNo,
                                    "PASR_CatseCertificateNo": $scope.reg.PASR_CatseCertificateNo,
                                    "PASR_AdmissionReason": $scope.reg.PASR_AdmissionReason,
                                    "PASR_Illnessdetails": $scope.reg.PASR_Illnessdetails,
                                    "PASR_LastPlayGrndAttnd": $scope.reg.PASR_LastPlayGrndAttnd,
                                    "PASR_ExtraActivity": $scope.reg.PASR_ExtraActivity,
                                    "PASR_OtherResidential_Addr": $scope.reg.PASR_OtherResidential_Addr,
                                    "PASR_OtherPermanentAddr": $scope.reg.PASR_OtherPermanentAddr,
                                    "PASR_PerStreet": $scope.reg.PASR_PerStreet,
                                    "PASR_PerArea": $scope.reg.PASR_PerArea,
                                    "PASR_PerCity": $scope.reg.PASR_PerCity,
                                    "PASR_PerState": $scope.reg.PASR_PerState,
                                    "PASR_PerDistrict": $scope.reg.PASR_PerDistrict,
                                    "PASR_FatherPresentState": $scope.reg.PASR_FatherPresentState,
                                    "PASR_FatherPermanentState": $scope.reg.PASR_FatherPermanentState,
                                    "PASR_MotherPresentState": $scope.reg.PASR_MotherPresentState,
                                    "PASR_MotherPermanentState": $scope.reg.PASR_MotherPermanentState,
                                    "PASR_PerCountry": $scope.reg.PASR_PerCountry,
                                    "PASR_PerPincode": $scope.reg.PASR_PerPincode,
                                    "PASR_ConStreet": $scope.reg.PASR_ConStreet,
                                    "PASR_ConArea": $scope.reg.PASR_ConArea,
                                    "PASR_ConCity": $scope.reg.PASR_ConCity,
                                    "PASR_ConState": $scope.reg.PASR_ConState,
                                    "PASR_ConDistrict": $scope.reg.PASR_ConDistrict,
                                    "PASR_ConCountry": $scope.reg.PASR_ConCountry,
                                    "PASR_ConPincode": $scope.reg.PASR_ConPincode,
                                    "PASR_Distirct": $scope.reg.PASR_Distirct,
                                    "PASR_Taluk": $scope.reg.PASR_Taluk,
                                    "PASR_Village": $scope.reg.PASR_Village,
                                    "PASR_Stayingwith": $scope.reg.PASR_Stayingwith,
                                    "PASR_Languagespeaking": $scope.reg.PASR_Languagespeaking,
                                    "PASR_FatherPanno": $scope.reg.PASR_FatherPanno,
                                    "PASR_MotherPanno": $scope.reg.PASR_MotherPanno,
                                    "PASR_AadharNo": PASR_AadharNo,
                                    "PASR_MobileNo": $scope.reg.PASR_MobileNo,
                                    "PASR_emailId": $scope.reg.PASR_emailId,
                                    "PASR_MaritalStatus": $scope.reg.PASR_MaritalStatus,
                                    "PASR_FatherMaritalStatus": $scope.reg.PASR_FatherMaritalStatus,
                                    "PASR_MotherMaritalStatus": $scope.reg.PASR_MotherMaritalStatus,
                                    "PASR_FatherAliveFlag": $scope.reg.PASR_FatherAliveFlag,
                                    "PASR_FatherName": $scope.reg.PASR_FatherName,
                                    "PASR_FatherAadharNo": PASR_AadharNofather,
                                    "PASR_FatherSurname": $scope.reg.PASR_FatherSurname,
                                    "PASR_FatherChurchAffiliation": $scope.reg.PASR_FatherChurchAffiliation,
                                    "PASR_FatherSelfEmployedFlg": $scope.reg.PASR_FatherSelfEmployedFlg,
                                    "PASR_FatherEducation": $scope.reg.PASR_FatherEducation,
                                    "PASR_FatherOccupation": $scope.reg.PASR_FatherOccupation,

                                    "PASR_FatherPresentAddress": $scope.reg.PASR_FatherPresentAddress,
                                    "PASR_FatherPresentCity": $scope.reg.PASR_FatherPresentCity,
                                    "PASR_FatherPresentPS": $scope.reg.PASR_FatherPresentPS,
                                    "PASR_FatherPresentPO": $scope.reg.PASR_FatherPresentPO,
                                    "PASR_FatherPresentPinCode": $scope.reg.PASR_FatherPresentPinCode,

                                    "PASR_MotherPresentAddress": $scope.reg.PASR_MotherPresentAddress,
                                    "PASR_MotherPresentCity": $scope.reg.PASR_MotherPresentCity,
                                    "PASR_MotherPresentPS": $scope.reg.PASR_MotherPresentPS,
                                    "PASR_MotherPresentPO": $scope.reg.PASR_MotherPresentPO,
                                    "PASR_MotherPresentPinCode": $scope.reg.PASR_MotherPresentPinCode,

                                    "PASR_MotherPermanentAddress": $scope.reg.PASR_MotherPermanentAddress,
                                    "PASR_MotherPermanentCity": $scope.reg.PASR_MotherPermanentCity,
                                    "PASR_MotherPermanentPS": $scope.reg.PASR_MotherPermanentPS,
                                    "PASR_MotherPermanentPO": $scope.reg.PASR_MotherPermanentPO,
                                    "PASR_MotherPermanentPinCode": $scope.reg.PASR_MotherPermanentPinCode,

                                    "PASR_FatherPermanentAddress": $scope.reg.PASR_FatherPermanentAddress,
                                    "PASR_FatherPermanentCity": $scope.reg.PASR_FatherPermanentCity,
                                    "PASR_FatherPermanentPS": $scope.reg.PASR_FatherPermanentPS,
                                    "PASR_FatherPermanentPO": $scope.reg.PASR_FatherPermanentPO,
                                    "PASR_FatherPermanentPinCode": $scope.reg.PASR_FatherPermanentPinCode,

                                    "PASR_FatherBankName": $scope.reg.PASR_FatherBankName,
                                    "PASR_FatherBankBranch": $scope.reg.PASR_FatherBankBranch,
                                    "PASR_FatherIFSC": $scope.reg.PASR_FatherIFSC,
                                    "PASR_FatherBankAccount": $scope.reg.PASR_FatherBankAccount,

                                    "PASR_MotherBankName": $scope.reg.PASR_MotherBankName,
                                    "PASR_MotherBankBranch": $scope.reg.PASR_MotherBankBranch,
                                    "PASR_MotherIFSC": $scope.reg.PASR_MotherIFSC,
                                    "PASR_MotherBankAccount": $scope.reg.PASR_MotherBankAccount,

                                    "PASR_FatherDesignation": $scope.reg.PASR_FatherDesignation,
                                    "PASR_FatherIncome": $scope.reg.PASR_FatherIncome,
                                    "PASR_FatherMonIncome": $scope.reg.PASR_FatherMonIncome,
                                    "PASR_FatherAnnIncome": $scope.reg.PASR_FatherAnnIncome,
                                    "PASR_FatherMobleNo": $scope.reg.PASR_FatherMobleNo,
                                    "PASR_FatherNationality": $scope.reg.PASR_FatherNationality,
                                    "PASR_FatherOfficeAddr": $scope.reg.PASR_FatherOfficeAddr,
                                    "PASR_AltContactNo": PASR_AltContactNo,
                                    "PASR_AltContactEmail": $scope.reg.PASR_AltContactEmail,
                                    "PASR_FatheremailId": $scope.reg.PASR_FatheremailId,
                                    "PASR_MotherAliveFlag": $scope.reg.PASR_MotherAliveFlag,
                                    "PASR_MotherName": $scope.reg.PASR_MotherName,
                                    "PASR_MotherAadharNo": PASR_AadharNomother,
                                    "PASR_AccountNo": $scope.reg.PASR_AccountNo,
                                    "PASR_BankName": $scope.reg.PASR_BankName,
                                    "PASR_BranchName": $scope.reg.PASR_BranchName,
                                    "PASR_IFSCCode": $scope.reg.PASR_IFSCCode,
                                    "PASR_Domicile": $scope.reg.PASR_Domicile,
                                    "PASR_SchoolDISECode": $scope.reg.PASR_SchoolDISECode,
                                    "PASR_MedicalComplaints": $scope.reg.PASR_MedicalComplaints,
                                    "PASR_OtherInformations": $scope.reg.PASR_OtherInformations,
                                    "PASR_NoOfDependencies": $scope.reg.PASR_NoOfDependencies,
                                    "PASR_NewlyAdmisstedFlg": $scope.reg.PASR_NewlyAdmisstedFlg,
                                    "PASR_VaccinatedFlg": $scope.reg.PASR_VaccinatedFlg,
                                    "PASR_Tcflag": $scope.reg.PASR_Tcflag,
                                    "PASR_MotherSurname": $scope.reg.PASR_MotherSurname,
                                    "PASR_MotherChurchAffiliation": $scope.reg.PASR_MotherChurchAffiliation,
                                    "PASR_MotherSelfEmployedFlg": $scope.reg.PASR_MotherSelfEmployedFlg,
                                    "PASR_MotherEducation": $scope.reg.PASR_MotherEducation,
                                    "PASR_MotherOccupation": $scope.reg.PASR_MotherOccupation,
                                    "PASR_MotherDesignation": $scope.reg.PASR_MotherDesignation,
                                    "PASR_MotherIncome": $scope.reg.PASR_MotherIncome,
                                    "PASR_MotherMonIncome": $scope.reg.PASR_MotherMonIncome,
                                    "PASR_MotherAnnIncome": $scope.reg.PASR_MotherAnnIncome,
                                    "PASR_MotherMobleNo": $scope.reg.PASR_MotherMobleNo,
                                    "PASR_MotherNationality": $scope.reg.PASR_MotherNationality,
                                    "PASR_MotherOfficeAddr": $scope.reg.PASR_MotherOfficeAddr,
                                    "PASR_MotheremailId": $scope.reg.PASR_MotheremailId,
                                    "PASR_TotalIncome": $scope.reg.PASR_TotalIncome,
                                    "PASR_BirthPlace": $scope.reg.PASR_BirthPlace,
                                    "PASR_Nationality": $scope.reg.PASR_Nationality,
                                    "Staffid": $scope.reg.hrmE_Id,
                                    //set flag
                                    "PASR_HostelReqdFlag": $scope.reg.PASR_HostelReqdFlag,
                                    "PASR_TransportReqdFlag": $scope.reg.PASR_TransportReqdFlag,
                                    "PASR_Studentillness": $scope.reg.PASR_Studentillness,
                                    "PASR_PovertyLine": $scope.reg.PASR_PovertyLine,
                                    "PASR_NickName": $scope.reg.PASR_NickName,
                                    "PASR_FatherHobbies": $scope.reg.PASR_FatherHobbies,
                                    "PASR_MotherHobbies": $scope.reg.PASR_MotherHobbies,
                                    "PASR_NeighbourPhoneNo": $scope.reg.PASR_NeighbourPhoneNo,
                                    "PASR_NeighbourAddr1": $scope.reg.PASR_NeighbourAddr1,
                                    "PASR_NeighbourAddr2": $scope.reg.PASR_NeighbourAddr2,
                                    "PASR_NeighbourName": $scope.reg.PASR_NeighbourName,
                                    "PASR_PhysicalDisability": $scope.reg.PASR_PhysicalDisability,
                                    "PASR_GymReqdFlag": $scope.reg.PASR_GymReqdFlag,
                                    "PASR_ECSFlag": $scope.reg.PASR_ECSFlag,
                                    "PASR_SibblingConcessionFlag": $scope.reg.PASR_SibblingConcessionFlag,
                                    "PASR_ParentConcessionFlag": $scope.reg.PASR_ParentConcessionFlag,
                                    "PASR_PaymentFlag": $scope.reg.PASR_PaymentFlag,
                                    "PASR_AmountPaid": $scope.reg.PASR_AmountPaid,
                                    "PASR_PaymentType": $scope.reg.PASR_PaymentType,
                                    "PASR_PaymentDate": $scope.reg.PASR_PaymentDate,
                                    "PASR_ReceiptNo": $scope.reg.PASR_ReceiptNo,
                                    "PASR_ActiveFlag": $scope.reg.PASR_ActiveFlag,
                                    "PASR_ApplStatus": $scope.reg.PASR_ApplStatus,
                                    "PASR_FinalpaymentFlag": $scope.reg.PASR_FinalpaymentFlag,
                                    "PASR_UndertakingFlag": $scope.reg.PASR_UndertakingFlag,
                                    // guardian
                                    "PASRG_Id": $scope.reg.pasrG_Id,
                                    "PASRG_GuardianName": $scope.reg.PASRG_GuardianName,
                                    "PASRG_GuardianRelation": $scope.reg.PASRG_GuardianRelation,
                                    "PASRG_GuardianAddress": $scope.reg.PASRG_GuardianAddress,
                                    "PASRG_FeeUndertakeFlg": $scope.reg.PASRG_FeeUndertakeFlg,
                                    "PASRG_emailid": $scope.reg.PASRG_emailid,
                                    "PASRG_GuardianPhoneNo": $scope.reg.PASRG_GuardianPhoneNo,
                                    "PASRG_Occupation": $scope.reg.PASRG_Occupation,
                                    "PASRG_PhoneOffice": $scope.reg.PASRG_PhoneOffice,
                                    "PASR_ChurchAddress": $scope.reg.PASR_ChurchAddress,
                                    "PASR_ChurchName": $scope.reg.PASR_Churchname,
                                    "PASR_FatherOfficePhNo": $scope.reg.PASR_FatherOfficePhNo,
                                    "PASR_FatherHomePhNo": $scope.reg.PASR_FatherHomePhNo,
                                    "PASR_MotherOfficePhNo": $scope.reg.PASR_MotherOfficePhNo,
                                    "PASR_MotherHomePhNo": $scope.reg.PASR_MotherHomePhNo,
                                    "PASR_FatherPassingYear": $scope.reg.PASR_FatherPassingYear,
                                    "PASR_MotherPassingYear": $scope.reg.PASR_MotherPassingYear,
                                    "PASRT_Id": $scope.reg.pasrT_Id,
                                    "trmA_Id": $scope.reg.trmA_Id,
                                    "trmR_Idp": $scope.reg.trmR_Idp,
                                    "trmL_Idp": $scope.reg.trmL_Idp,
                                    //"PASRT_consession_type_Id": $scope.reg.PASRT_consession_type_Id,
                                    "PASRT_Daughter": $scope.reg.PASRT_Daughter,
                                    "PASRT_Son": $scope.reg.PASRT_Son,
                                    "PASRT_Heared_Friend_Colleague": $scope.reg.PASRT_Heared_Friend_Colleague,
                                    "PASRT_Internet": $scope.reg.PASRT_Internet,
                                    "PASRT_Media": $scope.reg.PASRT_Media,
                                    "PASRT_Other": $scope.reg.PASRT_Other,
                                    "PASR_Student_Pic_Path": $scope.reg.PASR_Student_Pic_Path,
                                    "PASR_JointPhoto": $scope.reg.PASR_JointPhoto,
                                    "PASR_Noofbrothers": $scope.reg.PASR_Noofbrothers,
                                    "PASR_Noofsisters": $scope.reg.PASR_Noofsisters,
                                    "PASR_NoOfElderBrothers": $scope.reg.PASR_NoOfElderBrothers,
                                    "PASR_NoOfYoungerBrothers": $scope.reg.PASR_NoOfYoungerBrothers,
                                    "PASR_NoOfElderSisters": $scope.reg.PASR_NoOfElderSisters,
                                    "PASR_NoOfYoungerSisters": $scope.reg.PASR_NoOfYoungerSisters,
                                    "PASR_lastclassperc": $scope.reg.PASR_lastclassperc,
                                    "PASR_FatherReligion": $scope.reg.PASR_FatherReligion,
                                    "PASR_MotherReligion": $scope.reg.PASR_MotherReligion,
                                    "PASR_MotherCaste": $scope.reg.PASR_MotherCaste,
                                    "PASR_FatherCaste": $scope.reg.PASR_FatherCaste,
                                    "PASR_Fathersubcaste": $scope.reg.fatherSubCaste_Id,
                                    "PASR_Mothersubcatse": $scope.reg.motherSubCaste_Id,
                                    "PASR_Subcaste": $scope.reg.SubCaste_Id,
                                    "PASR_Tribe": $scope.reg.PASR_Tribe,
                                    "PASR_FatherTribe": $scope.reg.PASR_FatherTribe,
                                    "PASR_MotherTribe": $scope.reg.PASR_MotherTribe,
                                    "PASR_FirstLanguage": $scope.reg.PASR_FirstLanguage,
                                    "PASR_SecondLanguage": $scope.reg.PASR_SecondLanguage,
                                    "PASR_Thirdlanguage": $scope.reg.PASR_Thirdlanguage,
                                    "PASR_FatherPhoto": $scope.reg.PASR_FatherPhoto,
                                    "PASR_MotherPhoto": $scope.reg.PASR_MotherPhoto,
                                    "PAMSId": $scope.reg.PAMS_Id,
                                    "updateform": $scope.updatedata,
                                    "PASRAPS_ID": 787928,
                                    "ASMST_Id": Number($scope.reg.ASMST_Id),
                                    selectedDocuments: $scope.documentList,
                                    siblingsDetails: siblings,
                                    PreviousSchoolList: $scope.PreviousSchoolList,
                                    "FMCC_ID": $scope.reg.FMCC_Id,
                                    "transnumconfigsettings": RegistrationNumbering,
                                    configurationsettings: $scope.configurationsettings,
                                    "PASR_Adm_Confirm_Flag": false,
                                    transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign,
                                    elesubsubject: electivesub,
                                    "PASR_DOBWords": DOBWords,
                                    "manualAdmFlag": $scope.reg.admcheck,
                                    "ApplicationNo": $scope.reg.appNo,
                                    "Usercreatonflag": $scope.reg.usercreation,
                                    "electivelist_array": $scope.electivelist_array,
                                    "SelectedRefrenceDetails": RefrenceIds,
                                    "SelectedSourceDetails": SourcesIds,

                                    "PASA_MedicalFitForSportsFlg": $scope.reg.PASA_MedicalFitForSportsFlg,
                                    "PASA_HealthIssueFlg": $scope.reg.PASA_HealthIssueFlg,
                                    "PASA_JoiningReasons": $scope.reg.PASA_JoiningReasons
                                };
                                console.log(data);
                                apiService.create("StudentApplication/", data).then(function (promise) {
                                    if (promise.pasR_Id > 0) {
                                        //$scope.PaymentMode = false
                                        $scope.pasR_Id = promise.pasR_Id;
                                        //$scope.ProspectuseScreen = true;
                                        if (promise.message != null) {
                                            if ((promise.message == "Successfully Submitted the Application!!" && promise.paymentapplicable == "Pay") || (promise.message == "Successfully updated" && promise.paymentapplicable == "Pay")) {
                                                if ((promise.message == "Successfully updated" && promise.payementcheck == 0) || (promise.message == "Successfully Submitted the Application!!" && promise.payementcheck == 0)) {
                                                    $scope.submitted = true;
                                                    swal({
                                                        title: promise.message,
                                                        text: "Kindly proceed to pay registration fees",
                                                        type: "success",
                                                        showCancelButton: true,
                                                        confirmButtonColor: "#DD6B55", confirmButtonText: "Pay!",
                                                        cancelButtonText: "Cancel!",
                                                        closeOnConfirm: false,
                                                        closeOnCancel: true
                                                    },
                                                        function (isConfirm) {
                                                            if (isConfirm) {
                                                                $scope.paynow(promise.pasR_Id);
                                                                swal.close();
                                                            }
                                                            else {
                                                                $state.reload();
                                                            }
                                                        });
                                                }
                                                else if ((promise.message == "Successfully Submitted the Application!!" && promise.payementcheck == 0 && promise.extraforms == true)) {
                                                    swal("Successfully Submitted the Application!!", "Please Fill Health Certificate.");
                                                    $state.reload();
                                                }
                                                else if (promise.message == "Successfully registered") {
                                                    swal("Successfully Submitted the Application!!");
                                                    $state.reload();
                                                }
                                                else {
                                                    swal(promise.message);
                                                    $state.reload();
                                                }
                                                //  $state.reload();
                                            }
                                            else if ((promise.message == "Successfully updated" && promise.payementcheck == 0 && promise.extraforms == true)) {
                                                //$scope.clearall();
                                                swal("Successfully Updated the Application!!");
                                                //$scope.myTabIndex = $scope.myTabIndex + 1;
                                                $state.reload();
                                            }
                                            else if ((promise.message == "Successfully Submitted the Application!!" && promise.payementcheck == 0 && promise.extraforms == true)) {
                                                swal("Successfully Submitted the Application!!", "KINDLY FILL HEALTH FORM TO PROCEED FURTHER ONLINE PAYMENT.!!!");
                                                //$state.reload();
                                                $scope.formfilactive = true;
                                                $scope.myTabIndex = $scope.myTabIndex + 1;
                                                $scope.scroll();
                                            }
                                            else if ((promise.message == "Successfully Submitted the Application!!" && promise.paymentapplicable == "NoPay") || (promise.message == "Successfully updated" && promise.paymentapplicable == "NoPay")) {
                                                swal("Successfully Submitted the Application!!");
                                                $state.reload();
                                            }
                                            else if (promise.message == "Application no. already exist.Please enter different one") {
                                                swal("Application no. already exist.Please enter different one!!");
                                            }
                                            else {
                                                // swal(promise.message);
                                                $state.reload();
                                            }
                                        }
                                        else {
                                            swal("Something went wrong .. please contact Admin");
                                            $state.reload();
                                        }
                                    }
                                    //}
                                    else {
                                        if (promise.message == '') {
                                            swal("Student not registerd");
                                        }
                                        else {
                                            swal(promise.message);
                                        }
                                        return;
                                    }
                                    $scope.loadstunames("organization", "institute");
                                });
                            }
                            else {
                                //$state.reload();
                            }
                        });
                }
                else {
                    swal("Enter all Mandatory fields!!", "Which is marked in red *")
                }
            }
            else {
                swal("Upload Student Photo");
            };
        };

        $scope.getemployee = function (schelist, schid) {
            angular.forEach(schelist, function (obj) {
                if (obj.fmcC_Id == schid) {
                    $scope.subjectname = obj.fmcC_ConcessionFlag;
                }
            });
            if ($scope.subjectname == 'E') {
                var data = {
                    "MI_Id": 1,
                }
                apiService.create("StudentApplication/getemployees", data).
                    then(function (promise) {
                        if (promise.fillstaff.length > 0) {
                            $scope.fillstaff = promise.fillstaff;
                        }
                    })
            }
            else {
                $scope.fillstaff = {};
            }
        };


        $scope.onclickloaddata = function (paymentgetway, result) {

            $scope.nextpayment = false;
            if ($scope.reg.PASR_UndertakingFlag == 1 || $scope.reg.PASR_UndertakingFlag == true) {
                $scope.applastdatedisable = false;
            }
            else {
                $scope.applastdatedisable = true;
            }

            if (paymentgetway == "RAZORPAY") {
                $scope.pamentsave($scope.qwe, result);
            }
        }


        $scope.onclickloaddataa = function () {

            if ($scope.Healthunder == 1) {
                $scope.applastdatedisablee = false;
            }
            else {
                $scope.applastdatedisablee = true;
            }
        }

        $scope.interacted = function (field) {

            return $scope.submitted;
        };
        $scope.interactedhe = function (field) {

            return $scope.submitted2;
        };

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

        $scope.IDEAL = function () {

            var innerContents = document.getElementById("IDEAL").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +

                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BGHS/BGHSAPPPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +

                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.STJAMES = function () {

            var innerContents = document.getElementById("STJAMES").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +

                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/STJAMES/BGHSAPPPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +

                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.BBHSAPP = function () {

            var innerContents = document.getElementById("BBHSAPP").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +

                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BBHS/BBHSAPPPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +

                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.BBSAPP = function () {

            var innerContents = document.getElementById("BBSAPP").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +

                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/hutchings/Hutchingspdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +

                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.SSAPP = function () {

            var innerContents = document.getElementById("SSAPP").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +

                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/Snehasagar/Snehasagar.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +

                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.HHSAPP = function () {

            var innerContents = document.getElementById("HHSAPP").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +

                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/hutchings/Hutchingspdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +

                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.JSHSAPP = function () {
            var innerContents = document.getElementById("JSHSAPP").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/JSHS/JSHSdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +

                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.NDSJRAPP = function () {
            var innerContents = document.getElementById("NDSJRAPP").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/JSHS/NDSJRPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +

                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.MALDAAPP = function () {
            var innerContents = document.getElementById("MALDAAPP").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/StMaryMalda/PrintAppPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +

                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        };

        $scope.HHSHAPP = function () {

            var innerContents = document.getElementById("HHSHAPP").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +

                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/hutchings/HutchingsHigher.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +

                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.VIKASAAPP = function () {

            var innerContents = document.getElementById("VIKASAAPP").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +

                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/hutchings/Hutchingspdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +

                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.BCEHSAPP = function () {

            var innerContents = document.getElementById("BCOESAPP").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BCEHS/BCOESAPP.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BCEHS/BCOESAPPReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }
        $scope.BISAPP = function () {

            var innerContents = document.getElementById("BISAPP").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BCEHS/BCOESAPP.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BCEHS/BCOESAPPReportPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }
        $scope.TBHS = function () {

            var innerContents = document.getElementById("TBHS").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/JSHS/JSHSdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +

                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }

        $scope.cleartheapplication = function () {
            $scope.serilano = "";
            $scope.pasr_id = "";
            $scope.PASR_FirstName = "";
            $scope.PASR_MiddleName = "";
            $scope.PASR_LastName = "";
            $scope.namelength = 0;
            $scope.PASR_Date = "";
            $scope.PASR_RegistrationNo = "";
            $scope.PASR_Sex = "";
            $scope.PASA_TransferrableJobFlg = "";
            $scope.PASR_DOB = "";
            $scope.PASR_ChurchBaptisedDate = "";
            $scope.PASR_Age = "";
            $scope.PASR_DOBWords = "";
            $scope.asmcL_ClassName = "";
            $scope.PASR_BloodGroup = "";
            $scope.PASR_Emisno = "";
            $scope.medium = "";
            $scope.PASR_Boarding = "";
            $scope.PASR_MotherTongue = "";
            $scope.religionname = "";
            $scope.concessioncat = "";
            $scope.IMCC_CategoryName = "";
            $scope.IMC_CasteName = "";
            $scope.PASR_PerStreet = "";
            $scope.PASR_PerArea = "";
            $scope.PASR_PerCity = "";
            $scope.PASR_PerStaten = "";
            $scope.PASR_PerCountryn = "";
            $scope.PASR_PerPincode = "";
            $scope.PASR_ConStreet = "";
            $scope.PASR_ConArea = "";
            $scope.PASR_ConCity = "";
            $scope.PASR_ConStaten = "";
            $scope.PASR_ConCountryn = "";
            $scope.PASR_ConPincode = "";
            $scope.PASR_AadharNo = "";
            $scope.reg.PASR_AccountNo = "";
            $scope.reg.PASR_BankName = "";
            $scope.reg.PASR_BranchName = "";
            $scope.reg.PASR_IFSCCode = "";
            $scope.reg.PASR_Domicile = "";
            $scope.reg.PASR_SchoolDISECode = "";
            $scope.reg.PASR_MedicalComplaints = "";
            $scope.reg.PASR_OtherInformations = "";
            $scope.reg.PASR_NoOfDependencies = "";
            $scope.reg.PASR_NewlyAdmisstedFlg = false;
            $scope.reg.PASR_VaccinatedFlg = false;
            $scope.reg.PASR_Tcflag = false;
            $scope.PASR_Distirct = "";
            $scope.PASR_Taluk = "";
            $scope.PASR_Village = "";
            $scope.PASR_Stayingwith = "";
            $scope.PASR_Languagespeaking = "";
            $scope.PASR_FatherPanno = "";
            $scope.PASR_MotherPanno = "";
            $scope.PASR_MobileNo = "";
            $scope.PASR_emailId = "";
            $scope.PASR_MaritalStatus = "";
            $scope.PASR_FatherMaritalStatus = "";
            $scope.PASR_MotherMaritalStatus = "";
            $scope.PASR_FatherAliveFlag = "";
            $scope.PASR_FatherName = "";
            $scope.PASR_FatherAadharNo = "";
            $scope.PASR_FatherSurname = "";
            $scope.reg.PASR_FatherChurchAffiliation = "";
            $scope.reg.PASR_FatherSelfEmployedFlg = false;
            $scope.PASR_FatherEducation = "";
            $scope.PASR_FatherOccupation = "";
            $scope.PASR_FatherPresentAddress = "";
            $scope.pasrfatherresi1 = "";
            $scope.pasrfatherresi2 = "";
            $scope.PASR_FatherPresentCity = "";
            $scope.PASR_FatherPresentPS = "";
            $scope.PASR_FatherPresentPO = "";
            $scope.PASR_FatherPresentPinCode = "";
            $scope.PASR_MotherPresentAddress = "";
            $scope.PASR_MotherPresentCity = "";
            $scope.PASR_MotherPresentPS = "";
            $scope.PASR_MotherPresentPO = "";
            $scope.PASR_MotherPresentPinCode = "";
            $scope.PASR_MotherPermanentAddress = "";
            $scope.PASR_MotherPermanentCity = "";
            $scope.PASR_MotherPermanentPS = "";
            $scope.PASR_MotherPermanentPO = "";
            $scope.PASR_MotherPermanentPinCode = "";
            $scope.PASR_FatherPermanentAddress = "";
            $scope.PASR_FatherPermanentCity = "";
            $scope.PASR_FatherPermanentPS = "";
            $scope.PASR_FatherPermanentPO = "";
            $scope.PASR_FatherPermanentPinCode = "";
            $scope.PASR_FatherBankName = "";
            $scope.PASR_FatherBankBranch = "";
            $scope.PASR_FatherIFSC = "";
            $scope.PASR_FatherBankAccount = "";
            $scope.PASR_MotherBankName = "";
            $scope.PASR_MotherBankBranch = "";
            $scope.PASR_MotherIFSC = "";
            $scope.PASR_MotherBankAccount = "";
            $scope.PASR_FatherDesignation = "";
            $scope.PASR_FatherIncome = "";
            $scope.PASR_FatherMonIncome = "";
            $scope.PASR_FatherAnnIncome = "";
            $scope.PASR_FatherMobleNo = "";
            $scope.PASR_FatheremailId = "";
            $scope.syllabussname = "";
            $scope.PASR_FatherReligion = "";
            $scope.PASR_FatherCaste = "";
            $scope.fatherSubCaste_Id = "";
            $scope.SubCaste_Id = "";
            $scope.motherSubCaste_Id = "";
            $scope.PASR_FatherTribe = "";
            $scope.PASR_Tribe = "";
            $scope.PASR_MotherReligion = "";
            $scope.PASR_MotherCaste = "";
            $scope.PASR_MotherTribe = "";
            $scope.PASR_FirstLanguage = "";
            $scope.PASR_Thirdlanguage = "";
            $scope.PASR_SecondLanguage = "";
            $scope.PASR_MotherAliveFlag = "";
            $scope.PASR_MotherName = "";
            $scope.PASR_MotherAadharNo = "";
            $scope.PASR_MotherSurname = "";
            $scope.reg.PASR_MotherChurchAffiliation = "";
            $scope.reg.PASR_MotherSelfEmployedFlg = "";
            $scope.PASR_MotherEducation = "";
            $scope.PASR_MotherOccupation = "";
            $scope.PASR_MotherDesignation = "";
            $scope.PASR_MotherIncome = "";
            $scope.PASR_MotherMonIncome = "";
            $scope.PASR_MotherAnnIncome = "";
            $scope.PASR_MotherMobleNo = "";
            $scope.PASR_MotheremailId = "";
            $scope.PASR_ChurchAddress = "";
            $scope.PASR_Churchname = "";
            $scope.PASR_FatherOfficePhNo = "";
            $scope.PASR_FatherHomePhNo = "";
            $scope.PASR_MotherOfficePhNo = "";
            $scope.PASR_MotherHomePhNo = "";
            $scope.PASR_FatherPassingYear = "";
            $scope.PASR_MotherPassingYear = "";
            $scope.PASR_TotalIncome = "";
            $scope.PASR_BirthPlace = "";
            $scope.studentnationality = "";
            $scope.PASR_HostelReqdFlag = "";
            $scope.PASR_TransportReqdFlag = "";
            $scope.PASR_Studentillness = "";
            $scope.PASR_PovertyLine = "";
            $scope.PASR_NickName = "";
            $scope.PASR_FatherHobbies = "";
            $scope.PASR_MotherHobbies = "";
            $scope.PASR_NeighbourPhoneNo = "";
            $scope.PASR_NeighbourAddr1 = "";
            $scope.PASR_NeighbourAddr2 = "";
            $scope.PASR_PhysicalDisability = "";
            $scope.PASR_NeighbourName = "";
            $scope.PASR_GymReqdFlag = "";
            $scope.PASR_ECSFlag = "";
            $scope.PASR_PaymentFlag = "";
            $scope.PASR_AmountPaid = "";
            $scope.PASR_PaymentType = "";
            $scope.PASR_PaymentDate = "";
            $scope.PASR_ReceiptNo = "";
            //Activeflag
            //Applstatus
            $scope.PASR_FinalpaymentFlag = "";
            $scope.PASR_LastPlayGrndAttnd = "";
            $scope.PASR_ExtraActivity = "";
            $scope.PASR_OtherResidential_Addr = "";
            $scope.PASR_OtherPermanentAddr = "";
            $scope.PASR_MotherOfficeAddr = "";
            $scope.PASR_UndertakingFlag = "";
            $scope.fathernationality = "";
            $scope.mothernationality = "";
            $scope.PASR_BirthCertificateNo = "";
            $scope.PASR_CatseCertificateNo = "";
            $scope.PASR_AdmissionReason = "";
            $scope.PASR_Illnessdetails = "";
            $scope.PASR_AltContactNo = "";
            $scope.PASR_AltContactEmail = "";
            $scope.pasrfathrnature = "";
            $scope.pasrfathradd = "";
            $scope.pasrfathrnature1 = "";
            $scope.pasrfathradd1 = "";
            $scope.pasrmothrnature = "";
            $scope.pasrmothradd = "";
            $scope.pasrmothrnature1 = "";
            $scope.pasrmothradd1 = "";
            $scope.selfMotherOccupation = "";
            $scope.selfMotherOccupation = "";
            $('#blahnew').attr('src', "");
            $scope.studentphoto = "";
            $scope.PASR_Noofbrothers = "";
            $scope.PASR_Noofsisters = "";
            $scope.PASR_NoOfElderBrothers = "";
            $scope.PASR_NoOfYoungerBrothers = "";
            $scope.PASR_NoOfElderSisters = "";
            $scope.PASR_NoOfYoungerSisters = "";
            $scope.PASR_lastclassperc = "";
            $scope.PASR_SibblingConcessionFlag = "";
            $scope.PASR_ParentConcessionFlag = "";
            $scope.pasrG_Id = "";
            $scope.PASRG_GuardianName = "";
            $scope.PASRG_GuardianAddress = "";
            $scope.PASRG_GuardianRelation = "";
            $scope.PASRG_emailid = "";
            $scope.PASRG_GuardianPhoneNo = "";
            $scope.PASRG_Occupation = "";
            $scope.PASRG_FeeUndertakeFlg = "";
            $scope.PASRG_PhoneOffice = "";
            $scope.firstsibling = '';
            $scope.firstsiblingclass = '';
            $scope.secondsibling = '';
            $scope.secondsiblingclass = '';
            $scope.siblingsprint = "";
            $scope.firstsibling = "";
            $scope.firstsiblingclass = "";
            $scope.firstsibling = "";
            $scope.firstsiblingschoolAdmissionNo = "";
            $scope.secondsiblingschoolAdmissionNo = "";
            $scope.secondsiblingschool = "";
            $scope.firstsiblingschool = "";
            $scope.firstsiblingclass = "";
            $scope.secondsibling = "";
            $scope.secondsiblingclass = "";
            $scope.sibl = "Yes!";
            $scope.showbr = false;
            $scope.siblingshow = true;
            $scope.albumNameArray1 = [];
            $scope.albumNameArray2 = [];
            $scope.Hstuname = "";
            $scope.Hstuage = "";
            $scope.HFirstName = "";
            $scope.VaccinationDate = "";
            $scope.HepatitisB = "";
            $scope.TyphoidFever = "";
            $scope.Ailment = "";
            $scope.HealthProblem = "";
            $scope.CronicDesease = "";
            $scope.MEDetails = "";
            $scope.MEContactNo = "";
            $scope.vaccinesprint = "";
            $scope.PreviousSchool = {};
            $scope.PreviousSchoolprint = {};
            $scope.PreviousSchoolList = [{ id: 'PreviousSchool1' }];
            $scope.addNewPreviousSchool = function () {
                var newItemNo = $scope.PreviousSchoolList.length + 1;

                if (newItemNo <= 5) {
                    $scope.PreviousSchoolList.push({ 'id': 'PreviousSchool' + newItemNo });
                }
            };

            for (var i1 = 0; i1 < $scope.allRefrence.length; i1++) {
                $scope.allRefrence[i1].Selected = false;
            }

            for (var i2 = 0; i2 < $scope.allSources.length; i2++) {
                $scope.allSources[i2].Selected = false;
            }
            $scope.schoolname = "";
            $scope.schooladress = "";
            $scope.lastclass = "";
            $scope.lastsylaabus = "";
            $scope.percentage = "";
            $scope.leftdate = "";
            $scope.leftreason = "";
            $('#blahfnew').attr('src', "");
            $('#blahmnew').attr('src', "");
            $('#blahnewa').attr('src', "");
            $('#blahnewaa').attr('src', "");
            //var e1 = angular.element(document.getElementById("test"));
            //$compile(e1.html(promise.htmldata))(($scope));
            $('#blahnew').attr('src', "");
            $('#blahfnew').attr('src', "");
            $('#blahmnew').attr('src', "");
            $('#blahnewa').attr('src', "");
            $('#blahnewaa').attr('src', "");
        }

        $scope.showprintdata = function (pasR_Id) {
            $scope.cleartheapplication();

            apiService.getURI("StudentApplication/getprintdata", pasR_Id).then(function (promise) {

                $scope.serilano = promise.studentReg_DTObj[0].pasR_Id;
                $scope.pasr_id = promise.studentReg_DTObj[0].pasR_RegistrationNo;
                $scope.applicationno_view = promise.studentReg_DTObj[0].pasR_Applicationno;

                $scope.PASR_FirstName = promise.studentReg_DTObj[0].pasR_FirstName == null ? "" : promise.studentReg_DTObj[0].pasR_FirstName;
                $scope.PASR_MiddleName = promise.studentReg_DTObj[0].pasR_MiddleName == null ? "" : promise.studentReg_DTObj[0].pasR_MiddleName;
                $scope.PASR_LastName = promise.studentReg_DTObj[0].pasR_LastName == null ? "" : promise.studentReg_DTObj[0].pasR_LastName;
                $scope.namelength = Number($scope.PASR_FirstName.length) + Number($scope.PASR_MiddleName.length) + Number($scope.PASR_LastName.length);
                $scope.PASR_Date = new Date(promise.studentReg_DTObj[0].pasR_Date);
                $scope.PASR_RegistrationNo = promise.studentReg_DTObj[0].pasR_RegistrationNo == null ? "" : promise.studentReg_DTObj[0].pasR_RegistrationNo;
                //AMC ID
                $scope.PASR_Sex = promise.studentReg_DTObj[0].pasR_Sex == null ? "" : promise.studentReg_DTObj[0].pasR_Sex;
                $scope.PASA_TransferrableJobFlg = promise.studentReg_DTObj[0].pasA_TransferrableJobFlg == null ? "" : promise.studentReg_DTObj[0].pasA_TransferrableJobFlg;
                $scope.PASR_DOB = new Date(promise.studentReg_DTObj[0].pasR_DOB);
                $scope.PASR_ChurchBaptisedDate = new Date(promise.studentReg_DTObj[0].pasR_ChurchBaptisedDate);
                var doob = promise.studentReg_DTObj[0].pasR_DOB;

                var doobyr = doob.substring(0, 4);
                var doobmnth = doob.substring(5, 7);
                var doobdays = doob.substring(8, 10);

                $scope.b1 = doobdays.substring(0, 1);
                $scope.b2 = doobdays.substring(1, 2);


                $scope.b3 = doobmnth.substring(0, 1);
                $scope.b4 = doobmnth.substring(1, 2);

                $scope.b5 = doobyr.substring(0, 1);
                $scope.b6 = doobyr.substring(1, 2);
                $scope.b7 = doobyr.substring(2, 3);
                $scope.b8 = doobyr.substring(3, 4);


                $scope.PASR_Age = promise.studentReg_DTObj[0].pasR_Age;
                $scope.PASR_DOBWords = promise.studentReg_DTObj[0].pasR_DOBWords;
                $scope.asmcL_ClassName = promise.studentClass.length > 0 ? promise.studentClass[0].asmcL_ClassName : "";
                $scope.PASR_BloodGroup = promise.studentReg_DTObj[0].pasR_BloodGroup == null ? "" : promise.studentReg_DTObj[0].pasR_BloodGroup;
                $scope.PASR_Emisno = promise.studentReg_DTObj[0].pasR_Emisno == null ? "" : promise.studentReg_DTObj[0].pasR_Emisno;;
                $scope.medium = promise.studentReg_DTObj[0].pasR_Medium == null ? "" : promise.studentReg_DTObj[0].pasR_Medium;;
                $scope.PASR_Boarding = promise.studentReg_DTObj[0].pasR_Boarding == null ? "" : promise.studentReg_DTObj[0].pasR_Boarding;;
                $scope.PASR_MotherTongue = promise.studentReg_DTObj[0].pasR_MotherTongue == null ? "" : promise.studentReg_DTObj[0].pasR_MotherTongue;;
                $scope.religionname = promise.studentReligion.length > 0 ? promise.studentReligion[0].ivrmmR_Name : "";//
                if (promise.concessioncategory.length > 0) {
                    $scope.concessioncat = promise.concessioncategory.length > 0 ? promise.concessioncategory[0].concessioncats : "";
                }
                $scope.studentarea = promise.studentarea;
                $scope.studentroue = promise.studentroue;
                $scope.studentlocation = promise.studentlocation;
                $scope.studentsource = promise.studentsource;

                $scope.IMCC_CategoryName = promise.studentcastecategory.length > 0 ? promise.studentcastecategory[0].imcC_CategoryName : "";
                $scope.IMC_CasteName = promise.studentcaste.length > 0 ? promise.studentcaste[0].imC_CasteName : "";

                $scope.PASR_PerStreet = promise.studentReg_DTObj[0].pasR_PerStreet == null ? "" : promise.studentReg_DTObj[0].pasR_PerStreet;
                $scope.PASR_PerArea = promise.studentReg_DTObj[0].pasR_PerArea == null ? "" : promise.studentReg_DTObj[0].pasR_PerArea;
                $scope.PASR_PerCity = promise.studentReg_DTObj[0].pasR_PerCity == null ? "" : promise.studentReg_DTObj[0].pasR_PerCity;
                $scope.PASR_PerStaten = promise.studentperstate.length > 0 ? promise.studentperstate[0].pasR_PerStaten : "";
                $scope.PASR_PerCountryn = promise.studentpercountry.length > 0 ? promise.studentpercountry[0].pasR_PerCountryn : "";
                $scope.PASR_PerPincode = promise.studentReg_DTObj[0].pasR_PerPincode != 0 ? promise.studentReg_DTObj[0].pasR_PerPincode : "";


                $scope.PASR_ConStreet = promise.studentReg_DTObj[0].pasR_ConStreet == null ? "" : promise.studentReg_DTObj[0].pasR_ConStreet;
                $scope.PASR_ConArea = promise.studentReg_DTObj[0].pasR_ConArea == null ? "" : promise.studentReg_DTObj[0].pasR_ConArea;
                $scope.PASR_ConCity = promise.studentReg_DTObj[0].pasR_ConCity == null ? "" : promise.studentReg_DTObj[0].pasR_ConCity;
                $scope.PASR_ConStaten = promise.studentconstate.length > 0 ? promise.studentconstate[0].pasR_ConStaten : "";
                $scope.PASR_ConCountryn = promise.studentconcountry.length > 0 ? promise.studentconcountry[0].pasR_ConCountryn : "";
                $scope.PASR_ConPincode = promise.studentReg_DTObj[0].pasR_ConPincode != 0 ? promise.studentReg_DTObj[0].pasR_ConPincode : "";


                $scope.PASR_AadharNo = promise.studentReg_DTObj[0].pasR_AadharNo != 0 ? promise.studentReg_DTObj[0].pasR_AadharNo : "";

                $scope.PASR_AccountNo = promise.studentReg_DTObj[0].pasR_AccountNo != 0 ? promise.studentReg_DTObj[0].pasR_AccountNo : "";

                $scope.PASR_BankName = promise.studentReg_DTObj[0].pasR_BankName != 0 ? promise.studentReg_DTObj[0].pasR_BankName : "";

                $scope.PASR_BranchName = promise.studentReg_DTObj[0].pasR_BranchName != 0 ? promise.studentReg_DTObj[0].pasR_BranchName : "";

                $scope.PASR_IFSCCode = promise.studentReg_DTObj[0].pasR_IFSCCode != 0 ? promise.studentReg_DTObj[0].pasR_IFSCCode : "";

                $scope.PASR_Domicile = promise.studentReg_DTObj[0].pasR_Domicile != 0 ? promise.studentReg_DTObj[0].pasR_Domicile : "";

                $scope.PASR_SchoolDISECode = promise.studentReg_DTObj[0].pasR_SchoolDISECode != 0 ? promise.studentReg_DTObj[0].pasR_SchoolDISECode : "";

                $scope.PASR_MedicalComplaints = promise.studentReg_DTObj[0].pasR_MedicalComplaints != 0 ? promise.studentReg_DTObj[0].pasR_MedicalComplaints : "";

                $scope.PASR_OtherInformations = promise.studentReg_DTObj[0].pasR_OtherInformations != 0 ? promise.studentReg_DTObj[0].pasR_OtherInformations : "";
                $scope.PASR_NoOfDependencies = promise.studentReg_DTObj[0].pasR_NoOfDependencies != 0 ? promise.studentReg_DTObj[0].pasR_NoOfDependencies : "";

                $scope.PASR_NewlyAdmisstedFlg = promise.studentReg_DTObj[0].pasR_NewlyAdmisstedFlg != 0 ? promise.studentReg_DTObj[0].pasR_NewlyAdmisstedFlg : "";

                $scope.PASR_VaccinatedFlg = promise.studentReg_DTObj[0].pasR_VaccinatedFlg != 0 ? promise.studentReg_DTObj[0].pasR_VaccinatedFlg : "";

                $scope.PASR_Tcflag = promise.studentReg_DTObj[0].pasR_Tcflag != 0 ? promise.studentReg_DTObj[0].pasR_Tcflag : "";


                $scope.PASR_Distirct = promise.studentReg_DTObj[0].pasR_Distirct != 0 ? promise.studentReg_DTObj[0].pasR_Distirct : "";
                $scope.PASR_Taluk = promise.studentReg_DTObj[0].pasR_Taluk != 0 ? promise.studentReg_DTObj[0].pasR_Taluk : "";
                $scope.PASR_Village = promise.studentReg_DTObj[0].pasR_Village != 0 ? promise.studentReg_DTObj[0].pasR_Village : "";
                $scope.PASR_Stayingwith = promise.studentReg_DTObj[0].pasR_Stayingwith != 0 ? promise.studentReg_DTObj[0].pasR_Stayingwith : "";

                $scope.PASR_Languagespeaking = promise.studentReg_DTObj[0].pasR_Languagespeaking != 0 ? promise.studentReg_DTObj[0].pasR_Languagespeaking : "";
                $scope.PASR_FatherPanno = promise.studentReg_DTObj[0].pasR_FatherPanno != 0 ? promise.studentReg_DTObj[0].pasR_FatherPanno : "";
                $scope.PASR_MotherPanno = promise.studentReg_DTObj[0].pasR_MotherPanno != 0 ? promise.studentReg_DTObj[0].pasR_MotherPanno : "";

                $scope.PASR_MobileNo = promise.studentReg_DTObj[0].pasR_MobileNo != 0 ? promise.studentReg_DTObj[0].pasR_MobileNo : "";
                $scope.PASR_emailId = promise.studentReg_DTObj[0].pasR_emailId == null ? "" : promise.studentReg_DTObj[0].pasR_emailId;
                $scope.PASR_MaritalStatus = promise.studentReg_DTObj[0].pasR_MaritalStatus == null ? "" : promise.studentReg_DTObj[0].pasR_MaritalStatus;
                $scope.PASR_FatherMaritalStatus = promise.studentReg_DTObj[0].pasR_FatherMaritalStatus == null ? "" : promise.studentReg_DTObj[0].pasR_FatherMaritalStatus;
                $scope.PASR_MotherMaritalStatus = promise.studentReg_DTObj[0].pasR_MotherMaritalStatus == null ? "" : promise.studentReg_DTObj[0].pasR_MotherMaritalStatus;

                $scope.PASR_FatherAliveFlag = promise.studentReg_DTObj[0].pasR_FatherAliveFlag == 0 ? "Not Alive" : "Alive";
                $scope.PASR_FatherName = promise.studentReg_DTObj[0].pasR_FatherName == null ? "" : promise.studentReg_DTObj[0].pasR_FatherName;
                $scope.PASR_FatherAadharNo = promise.studentReg_DTObj[0].pasR_FatherAadharNo == 0 ? "" : promise.studentReg_DTObj[0].pasR_FatherAadharNo;
                $scope.PASR_FatherSurname = promise.studentReg_DTObj[0].pasR_FatherSurname == null ? "" : promise.studentReg_DTObj[0].pasR_FatherSurname;
                $scope.PASR_FatherChurchAffiliation = promise.studentReg_DTObj[0].pasR_FatherChurchAffiliation == null ? "" : promise.studentReg_DTObj[0].pasR_FatherChurchAffiliation;
                $scope.PASR_FatherSelfEmployedFlg = promise.studentReg_DTObj[0].pasR_FatherSelfEmployedFlg == null ? "" : promise.studentReg_DTObj[0].pasR_FatherSelfEmployedFlg;
                $scope.PASR_FatherEducation = promise.studentReg_DTObj[0].pasR_FatherEducation == null ? "" : promise.studentReg_DTObj[0].pasR_FatherEducation;
                $scope.PASR_FatherOccupation = promise.studentReg_DTObj[0].pasR_FatherOccupation == null ? "" : promise.studentReg_DTObj[0].pasR_FatherOccupation;

                $scope.PASR_FatherPresentAddress = promise.studentReg_DTObj[0].pasR_FatherPresentAddress == null ? "" : promise.studentReg_DTObj[0].pasR_FatherPresentAddress;
                if ($scope.PASR_FatherPresentAddress.length <= 30) {
                    $scope.pasrfatherresi1 = $scope.PASR_FatherPresentAddress.substring(0, 30);
                    $scope.pasrfatherresi1 = $scope.uppercase($scope.pasrfatherresi1);
                }
                if ($scope.PASR_FatherPresentAddress.length > 30) {
                    $scope.pasrfatherresi1 = $scope.PASR_FatherPresentAddress.substring(0, 30);
                    $scope.pasrfatherresi1 = $scope.uppercase($scope.pasrfatherresi1);
                    $scope.pasrfatherresi2 = $scope.PASR_FatherPresentAddress.substring(30, $scope.PASR_FatherPresentAddress.length);
                    $scope.pasrfatherresi2 = $scope.uppercase($scope.pasrfatherresi2);
                }
                $scope.PASR_FatherPresentCity = promise.studentReg_DTObj[0].pasR_FatherPresentCity == null ? "" : promise.studentReg_DTObj[0].pasR_FatherPresentCity;
                $scope.PASR_FatherPresentPS = promise.studentReg_DTObj[0].pasR_FatherPresentPS == null ? "" : promise.studentReg_DTObj[0].pasR_FatherPresentPS;
                $scope.PASR_FatherPresentPO = promise.studentReg_DTObj[0].pasR_FatherPresentPO == null ? "" : promise.studentReg_DTObj[0].pasR_FatherPresentPO;
                $scope.PASR_FatherPresentPinCode = promise.studentReg_DTObj[0].pasR_FatherPresentPinCode == null ? "" : promise.studentReg_DTObj[0].pasR_FatherPresentPinCode;

                $scope.PASR_MotherPresentAddress = promise.studentReg_DTObj[0].pasR_MotherPresentAddress == null ? "" : promise.studentReg_DTObj[0].pasR_MotherPresentAddress;
                $scope.PASR_MotherPresentCity = promise.studentReg_DTObj[0].pasR_MotherPresentCity == null ? "" : promise.studentReg_DTObj[0].pasR_MotherPresentCity;
                $scope.PASR_MotherPresentPS = promise.studentReg_DTObj[0].pasR_MotherPresentPS == null ? "" : promise.studentReg_DTObj[0].pasR_MotherPresentPS;
                $scope.PASR_MotherPresentPO = promise.studentReg_DTObj[0].pasR_MotherPresentPO == null ? "" : promise.studentReg_DTObj[0].pasR_MotherPresentPO;
                $scope.PASR_MotherPresentPinCode = promise.studentReg_DTObj[0].pasR_MotherPresentPinCode == null ? "" : promise.studentReg_DTObj[0].pasR_MotherPresentPinCode;

                $scope.PASR_MotherPermanentAddress = promise.studentReg_DTObj[0].pasR_MotherPermanentAddress == null ? "" : promise.studentReg_DTObj[0].pasR_MotherPermanentAddress;
                $scope.PASR_MotherPermanentCity = promise.studentReg_DTObj[0].pasR_MotherPermanentCity == null ? "" : promise.studentReg_DTObj[0].pasR_MotherPermanentCity;
                $scope.PASR_MotherPermanentPS = promise.studentReg_DTObj[0].pasR_MotherPermanentPS == null ? "" : promise.studentReg_DTObj[0].pasR_MotherPermanentPS;
                $scope.PASR_MotherPermanentPO = promise.studentReg_DTObj[0].pasR_MotherPermanentPO == null ? "" : promise.studentReg_DTObj[0].pasR_MotherPermanentPO;
                $scope.PASR_MotherPermanentPinCode = promise.studentReg_DTObj[0].pasR_MotherPermanentPinCode == null ? "" : promise.studentReg_DTObj[0].pasR_MotherPermanentPinCode;

                $scope.PASR_FatherPermanentAddress = promise.studentReg_DTObj[0].pasR_FatherPermanentAddress == null ? "" : promise.studentReg_DTObj[0].pasR_FatherPermanentAddress;
                $scope.PASR_FatherPermanentCity = promise.studentReg_DTObj[0].pasR_FatherPermanentCity == null ? "" : promise.studentReg_DTObj[0].pasR_FatherPermanentCity;
                $scope.PASR_FatherPermanentPS = promise.studentReg_DTObj[0].pasR_FatherPermanentPS == null ? "" : promise.studentReg_DTObj[0].pasR_FatherPermanentPS;
                $scope.PASR_FatherPermanentPO = promise.studentReg_DTObj[0].pasR_FatherPermanentPO == null ? "" : promise.studentReg_DTObj[0].pasR_FatherPermanentPO;
                $scope.PASR_FatherPermanentPinCode = promise.studentReg_DTObj[0].pasR_FatherPermanentPinCode == null ? "" : promise.studentReg_DTObj[0].pasR_FatherPermanentPinCode;

                $scope.PASR_FatherBankName = promise.studentReg_DTObj[0].pasR_FatherBankName == null ? "" : promise.studentReg_DTObj[0].pasR_FatherBankName;
                $scope.PASR_FatherBankBranch = promise.studentReg_DTObj[0].pasR_FatherBankBranch == null ? "" : promise.studentReg_DTObj[0].pasR_FatherBankBranch;
                $scope.PASR_FatherIFSC = promise.studentReg_DTObj[0].pasR_FatherIFSC == null ? "" : promise.studentReg_DTObj[0].pasR_FatherIFSC;
                $scope.PASR_FatherBankAccount = promise.studentReg_DTObj[0].pasR_FatherBankAccount == null ? "" : promise.studentReg_DTObj[0].pasR_FatherBankAccount;

                $scope.PASR_MotherBankName = promise.studentReg_DTObj[0].pasR_MotherBankName == null ? "" : promise.studentReg_DTObj[0].pasR_MotherBankName;
                $scope.PASR_MotherBankBranch = promise.studentReg_DTObj[0].pasR_MotherBankBranch == null ? "" : promise.studentReg_DTObj[0].pasR_MotherBankBranch;
                $scope.PASR_MotherIFSC = promise.studentReg_DTObj[0].pasR_MotherIFSC == null ? "" : promise.studentReg_DTObj[0].pasR_MotherIFSC;
                $scope.PASR_MotherBankAccount = promise.studentReg_DTObj[0].pasR_MotherBankAccount == null ? "" : promise.studentReg_DTObj[0].pasR_MotherBankAccount;

                $scope.PASR_FatherDesignation = promise.studentReg_DTObj[0].pasR_FatherDesignation == null ? "" : promise.studentReg_DTObj[0].pasR_FatherDesignation;
                $scope.PASR_FatherIncome = promise.studentReg_DTObj[0].pasR_FatherIncome;
                $scope.PASR_FatherMonIncome = promise.studentReg_DTObj[0].pasR_FatherMonIncome;
                $scope.PASR_FatherAnnIncome = promise.studentReg_DTObj[0].pasR_FatherAnnIncome;
                $scope.PASR_FatherMobleNo = promise.studentReg_DTObj[0].pasR_FatherMobleNo == 0 ? "" : promise.studentReg_DTObj[0].pasR_FatherMobleNo;
                $scope.PASR_FatheremailId = promise.studentReg_DTObj[0].pasR_FatheremailId == null ? "" : promise.studentReg_DTObj[0].pasR_FatheremailId;

                $scope.syllabussname = promise.sylabusss.length > 0 ? promise.sylabusss[0].imC_CasteName : "";
                $scope.PASR_FatherReligion = promise.fatherreligion.length > 0 ? promise.fatherreligion[0].imC_CasteName : "";
                $scope.PASR_FatherCaste = promise.fathercaste.length > 0 ? promise.fathercaste[0].imC_CasteName : "";
                $scope.overgae = promise.studentReg_DTObj[0].pasR_OverAge;
                $scope.undergae = promise.studentReg_DTObj[0].pasR_UnderAge;

                //$scope.fatherSubCaste_Id = promise.fathersubcaste.length > 0 ? promise.fathersubcaste[0].imC_CasteName : "";;
                //$scope.motherSubCaste_Id = promise.mothersubcaste.length > 0 ? promise.mothersubcaste[0].imC_CasteName : "";;
                //$scope.SubCaste_Id = promise.subcaste.length > 0 ? promise.subcaste[0].imC_CasteName : "";;
                //$scope.reg.fatherSubCaste_Id = promise.studentReg_DTObj[0].pasR_Fathersubcaste;
                //$scope.reg.motherSubCaste_Id = promise.studentReg_DTObj[0].pasR_Mothersubcatse;
                //$scope.reg.SubCaste_Id = promise.studentReg_DTObj[0].pasR_Subcaste;
                $scope.fatherSubCaste_Id = promise.studentReg_DTObj[0].pasR_Fathersubcaste == null ? "" : promise.studentReg_DTObj[0].pasR_Fathersubcaste;
                $scope.SubCaste_Id = promise.studentReg_DTObj[0].pasR_Subcaste == null ? "" : promise.studentReg_DTObj[0].pasR_Subcaste;
                $scope.motherSubCaste_Id = promise.studentReg_DTObj[0].pasR_Mothersubcatse == null ? "" : promise.studentReg_DTObj[0].pasR_Mothersubcatse;

                $scope.PASR_FatherTribe = promise.studentReg_DTObj[0].pasR_FatherTribe;
                $scope.PASR_Tribe = promise.studentReg_DTObj[0].pasR_Tribe;
                $scope.PASR_MotherReligion = promise.motherreligion.length > 0 ? promise.motherreligion[0].imC_CasteName : "";
                $scope.PASR_MotherCaste = promise.mothercaste.length > 0 ? promise.mothercaste[0].imC_CasteName : "";;
                $scope.PASR_MotherTribe = promise.studentReg_DTObj[0].pasR_MotherTribe;

                $scope.PASR_FirstLanguage = promise.studentReg_DTObj[0].pasR_FirstLanguage == null ? "" : promise.studentReg_DTObj[0].pasR_FirstLanguage;
                $scope.PASR_Thirdlanguage = promise.studentReg_DTObj[0].pasR_Thirdlanguage == null ? "" : promise.studentReg_DTObj[0].pasR_Thirdlanguage;
                $scope.PASR_SecondLanguage = promise.studentReg_DTObj[0].pasR_SecondLanguage == null ? "" : promise.studentReg_DTObj[0].pasR_SecondLanguage;
                //   $scope.PASR_Thirdlanguage = promise.studentReg_DTObj[0].pasR_Thirdlanguage == null ? "" : promise.studentReg_DTObj[0].pasR_Thirdlanguage;


                $scope.PASR_MotherAliveFlag = promise.studentReg_DTObj[0].pasR_MotherAliveFlag == 0 ? "Not Alive" : "Alive";
                $scope.PASR_MotherName = promise.studentReg_DTObj[0].pasR_MotherName == null ? "" : promise.studentReg_DTObj[0].pasR_MotherName;
                $scope.PASR_MotherAadharNo = promise.studentReg_DTObj[0].pasR_MotherAadharNo == 0 ? "" : promise.studentReg_DTObj[0].pasR_MotherAadharNo;
                $scope.PASR_MotherSurname = promise.studentReg_DTObj[0].pasR_MotherSurname == null ? "" : promise.studentReg_DTObj[0].pasR_MotherSurname;
                $scope.PASR_MotherChurchAffiliation = promise.studentReg_DTObj[0].pasR_MotherChurchAffiliation == null ? "" : promise.studentReg_DTObj[0].pasR_MotherChurchAffiliation;
                $scope.PASR_MotherSelfEmployedFlg = promise.studentReg_DTObj[0].pasR_MotherSelfEmployedFlg == null ? "" : promise.studentReg_DTObj[0].pasR_MotherSelfEmployedFlg;
                $scope.PASR_MotherEducation = promise.studentReg_DTObj[0].pasR_MotherEducation == null ? "" : promise.studentReg_DTObj[0].pasR_MotherEducation;
                $scope.PASR_MotherOccupation = promise.studentReg_DTObj[0].pasR_MotherOccupation == null ? "" : promise.studentReg_DTObj[0].pasR_MotherOccupation;
                $scope.PASR_MotherDesignation = promise.studentReg_DTObj[0].pasR_MotherDesignation == null ? "" : promise.studentReg_DTObj[0].pasR_MotherDesignation;
                $scope.PASR_MotherIncome = promise.studentReg_DTObj[0].pasR_MotherIncome;
                $scope.PASR_MotherMonIncome = promise.studentReg_DTObj[0].pasR_MotherMonIncome;
                $scope.PASR_MotherAnnIncome = promise.studentReg_DTObj[0].pasR_MotherAnnIncome;
                $scope.PASR_MotherMobleNo = promise.studentReg_DTObj[0].pasR_MotherMobleNo == 0 ? "" : promise.studentReg_DTObj[0].pasR_MotherMobleNo;
                $scope.PASR_MotheremailId = promise.studentReg_DTObj[0].pasR_MotheremailId == null ? "" : promise.studentReg_DTObj[0].pasR_MotheremailId;


                $scope.PASR_ChurchAddress = promise.studentReg_DTObj[0].pasR_ChurchAddress == 0 ? "" : promise.studentReg_DTObj[0].pasR_ChurchAddress;
                $scope.PASR_Churchname = promise.studentReg_DTObj[0].pasR_ChurchName == 0 ? "" : promise.studentReg_DTObj[0].pasR_ChurchName;

                $scope.PASR_FatherOfficePhNo = promise.studentReg_DTObj[0].pasR_FatherOfficePhNo == 0 ? "" : promise.studentReg_DTObj[0].pasR_FatherOfficePhNo; $scope.PASR_FatherHomePhNo = promise.studentReg_DTObj[0].pasR_FatherHomePhNo == 0 ? "" : promise.studentReg_DTObj[0].pasR_FatherHomePhNo; $scope.PASR_MotherOfficePhNo = promise.studentReg_DTObj[0].pasR_MotherOfficePhNo == 0 ? "" : promise.studentReg_DTObj[0].pasR_MotherOfficePhNo;
                $scope.PASR_MotherHomePhNo = promise.studentReg_DTObj[0].pasR_MotherHomePhNo == 0 ? "" : promise.studentReg_DTObj[0].pasR_MotherHomePhNo;

                $scope.PASR_FatherPassingYear = promise.studentReg_DTObj[0].pasR_FatherPassingYear == 0 ? "" : promise.studentReg_DTObj[0].pasR_FatherPassingYear; $scope.PASR_MotherPassingYear = promise.studentReg_DTObj[0].pasR_MotherPassingYear == 0 ? "" : promise.studentReg_DTObj[0].pasR_MotherPassingYear;




                $scope.PASR_TotalIncome = $scope.PASR_FatherIncome + $scope.PASR_MotherIncome;
                $scope.PASR_BirthPlace = promise.studentReg_DTObj[0].pasR_BirthPlace == null ? "" : promise.studentReg_DTObj[0].pasR_BirthPlace;
                $scope.studentnationality = promise.studentnationalitys.length > 0 ? promise.studentnationalitys[0].studentnationality : "";

                $scope.PASR_HostelReqdFlag = promise.studentReg_DTObj[0].pasR_HostelReqdFlag == 1 ? "Yes" : "No";//true : false
                $scope.PASR_TransportReqdFlag = promise.studentReg_DTObj[0].pasR_TransportReqdFlag == 1 ? "Yes" : "No";//true : false
                $scope.PASR_Studentillness = promise.studentReg_DTObj[0].pasR_Studentillness == 1 ? "Yes" : "No";//true : false
                $scope.PASR_PovertyLine = promise.studentReg_DTObj[0].pasR_PovertyLine;//true : false


                for (var i = 0; i < $scope.allRefrence.length; i++) {
                    $scope.allRefrence[i].Selected = false;
                }
                for (var i = 0; i < $scope.allRefrence.length; i++) {
                    name = $scope.allRefrence[i].pamR_Id;
                    for (var j = 0; j < promise.studentReferenceDetails.length; j++) {
                        if (name == promise.studentReferenceDetails[j].pamR_Id) {
                            $scope.allRefrence[i].Selected = true;
                            // $scope.Refrencecheckboxchcked.push(promise.studentReferenceDetails[j]);
                        }
                    }
                }

                $scope.PASR_NickName = promise.studentReg_DTObj[0].pasR_NickName;//true : false
                $scope.PASR_FatherHobbies = promise.studentReg_DTObj[0].pasR_FatherHobbies;//true : false
                $scope.PASR_MotherHobbies = promise.studentReg_DTObj[0].pasR_MotherHobbies;//true : false
                $scope.PASR_NeighbourPhoneNo = promise.studentReg_DTObj[0].pasR_NeighbourPhoneNo;//true : false
                $scope.PASR_NeighbourAddr1 = promise.studentReg_DTObj[0].pasR_NeighbourAddr1;//true : false
                $scope.PASR_NeighbourAddr2 = promise.studentReg_DTObj[0].pasR_NeighbourAddr2;//true : false
                $scope.PASR_NeighbourName = promise.studentReg_DTObj[0].pasR_NeighbourName;//true : false
                $scope.PASR_PhysicalDisability = promise.studentReg_DTObj[0].pasR_PhysicalDisability;//true : false

                $scope.PASA_MedicalFitForSportsFlg = promise.studentReg_DTObj[0].pasA_MedicalFitForSportsFlg == 1 ? "Yes" : "No";
                $scope.PASA_HealthIssueFlg = promise.studentReg_DTObj[0].pasA_HealthIssueFlg == 1 ? "Yes" : "Other";
                $scope.PASA_JoiningReasons = promise.studentReg_DTObj[0].pasA_JoiningReasons;
                

                $scope.PASR_GymReqdFlag = promise.studentReg_DTObj[0].pasR_GymReqdFlag == 1 ? "Yes" : "No";//true : false
                $scope.PASR_ECSFlag = promise.studentReg_DTObj[0].pasR_ECSFlag == 1 ? "Yes" : "No";
                $scope.PASR_PaymentFlag = promise.studentReg_DTObj[0].pasR_PaymentFlag == 1 ? "Yes" : "No";

                $scope.PASR_AmountPaid = promise.studentReg_DTObj[0].pasR_AmountPaid;
                $scope.PASR_PaymentType = promise.studentReg_DTObj[0].pasR_PaymentType == null ? "" : promise.studentReg_DTObj[0].pasR_PaymentType;
                $scope.PASR_PaymentDate = promise.studentReg_DTObj[0].pasR_PaymentDate == null ? "" : promise.studentReg_DTObj[0].pasR_PaymentDate;
                $scope.PASR_ReceiptNo = promise.studentReg_DTObj[0].pasR_ReceiptNo == null ? "" : promise.studentReg_DTObj[0].pasR_ReceiptNo;
                //Activeflag
                //Applstatus
                $scope.PASR_FinalpaymentFlag = promise.studentReg_DTObj[0].pasR_FinalpaymentFlag == 1 ? "Yes" : "No";
                $scope.PASR_LastPlayGrndAttnd = promise.studentReg_DTObj[0].pasR_LastPlayGrndAttnd == null ? "" : promise.studentReg_DTObj[0].pasR_LastPlayGrndAttnd;
                $scope.PASR_ExtraActivity = promise.studentReg_DTObj[0].pasR_ExtraActivity == null ? "" : promise.studentReg_DTObj[0].pasR_ExtraActivity;
                $scope.PASR_OtherResidential_Addr = promise.studentReg_DTObj[0].pasR_OtherResidential_Addr == null ? "" : promise.studentReg_DTObj[0].pasR_OtherPermanentAddr;
                $scope.PASR_OtherPermanentAddr = promise.studentReg_DTObj[0].pasR_OtherPermanentAddr == null ? "" : promise.studentReg_DTObj[0].pasR_OtherPermanentAddr;
                $scope.PASR_FatherOfficeAddr = promise.studentReg_DTObj[0].pasR_FatherOfficeAddr == null ? "" : promise.studentReg_DTObj[0].pasR_FatherOfficeAddr;
                $scope.PASR_MotherOfficeAddr = promise.studentReg_DTObj[0].pasR_MotherOfficeAddr == null ? "" : promise.studentReg_DTObj[0].pasR_MotherOfficeAddr;
                $scope.PASR_UndertakingFlag = promise.studentReg_DTObj[0].pasR_UndertakingFlag == 1 ? "Yes" : "No";
                $scope.fathernationality = promise.fathernationalitys.length > 0 ? promise.fathernationalitys[0].fathernationality : "";
                $scope.mothernationality = promise.mothernationalitys.length > 0 ? promise.mothernationalitys[0].mothernationality : "";
                $scope.PASR_BirthCertificateNo = promise.studentReg_DTObj[0].pasR_BirthCertificateNo == null ? "" : promise.studentReg_DTObj[0].pasR_BirthCertificateNo;
                $scope.PASR_CatseCertificateNo = promise.studentReg_DTObj[0].pasR_CatseCertificateNo == null ? "" : promise.studentReg_DTObj[0].pasR_CatseCertificateNo;
                $scope.PASR_AdmissionReason = promise.studentReg_DTObj[0].pasR_AdmissionReason == null ? "" : promise.studentReg_DTObj[0].pasR_AdmissionReason;
                $scope.PASR_Illnessdetails = promise.studentReg_DTObj[0].pasR_Illnessdetails == null ? "" : promise.studentReg_DTObj[0].pasR_Illnessdetails;
                $scope.PASR_AltContactNo = promise.studentReg_DTObj[0].pasR_AltContactNo == 0 ? "" : promise.studentReg_DTObj[0].pasR_AltContactNo;
                $scope.PASR_AltContactEmail = promise.studentReg_DTObj[0].pasR_AltContactEmail == null ? "" : promise.studentReg_DTObj[0].pasR_AltContactEmail;

                if ($scope.PASR_FatherOfficeAddr.length <= 30) {
                    $scope.pasrfathrnature = $scope.PASR_FatherOfficeAddr.substring(0, $scope.PASR_FatherOfficeAddr.length);
                    $scope.pasrfathrnature = $scope.uppercase($scope.pasrfathrnature);
                }
                if ($scope.PASR_FatherOfficeAddr.length > 30) {
                    $scope.pasrfathrnature = $scope.PASR_FatherOfficeAddr.substring(0, 30);
                    $scope.pasrfathrnature = $scope.uppercase($scope.pasrfathrnature);
                    $scope.pasrfathradd = $scope.PASR_FatherOfficeAddr.substring(30, $scope.PASR_FatherOfficeAddr.length);
                    $scope.pasrfathradd = $scope.uppercase($scope.pasrfathradd);
                }
                if ($scope.PASR_FatherOfficeAddr.length <= 65) {
                    $scope.pasrfathrnature1 = $scope.PASR_FatherOfficeAddr.substring(0, $scope.PASR_FatherOfficeAddr.length);
                    $scope.pasrfathrnature1 = $scope.uppercase($scope.pasrfathrnature1);
                }
                if ($scope.PASR_FatherOfficeAddr.length > 65) {
                    $scope.pasrfathrnature1 = $scope.PASR_FatherOfficeAddr.substring(0, 65);
                    $scope.pasrfathrnature1 = $scope.uppercase($scope.pasrfathrnature1);
                    $scope.pasrfathradd1 = $scope.PASR_FatherOfficeAddr.substring(65, $scope.PASR_FatherOfficeAddr.length);
                    $scope.pasrfathradd1 = $scope.uppercase($scope.pasrfathradd1);
                }

                if ($scope.PASR_MotherOfficeAddr.length <= 30) {
                    $scope.pasrmothrnature = $scope.PASR_MotherOfficeAddr.substring(0, $scope.PASR_MotherOfficeAddr.length);
                    $scope.pasrmothrnature = $scope.uppercase($scope.pasrmothrnature);
                }
                if ($scope.PASR_MotherOfficeAddr.length > 30) {
                    $scope.pasrmothrnature = $scope.PASR_MotherOfficeAddr.substring(0, 30);
                    $scope.pasrmothrnature = $scope.uppercase($scope.pasrmothrnature);
                    $scope.pasrmothradd = $scope.PASR_MotherOfficeAddr.substring(30, $scope.PASR_MotherOfficeAddr.length);
                    $scope.pasrmothradd = $scope.uppercase($scope.pasrmothradd);
                }
                if ($scope.PASR_MotherOfficeAddr.length <= 65) {
                    $scope.pasrmothrnature1 = $scope.PASR_MotherOfficeAddr.substring(0, $scope.PASR_MotherOfficeAddr.length);
                    $scope.pasrmothrnature1 = $scope.uppercase($scope.pasrmothrnature1);
                }
                if ($scope.PASR_MotherOfficeAddr.length > 65) {
                    $scope.pasrmothrnature1 = $scope.PASR_MotherOfficeAddr.substring(0, 65);
                    $scope.pasrmothrnature1 = $scope.uppercase($scope.pasrmothrnature1);
                    $scope.pasrmothradd1 = $scope.PASR_MotherOfficeAddr.substring(65, $scope.PASR_MotherOfficeAddr.length);
                    $scope.pasrmothradd1 = $scope.uppercase($scope.pasrmothradd1);
                }
                if ($scope.PASR_MotherOccupation.length >= 25) {
                    $scope.selfMotherOccupation = angular.lowercase($scope.PASR_MotherOccupation);
                }
                else {
                    $scope.selfMotherOccupation = $scope.PASR_MotherOccupation;
                }
                if ($scope.PASR_FatherOccupation.length >= 25) {
                    $scope.selfFatherOccupation = angular.lowercase($scope.PASR_FatherOccupation);
                }
                else {
                    $scope.selfFatherOccupation = $scope.PASR_FatherOccupation;
                }

                //PASR_Adm_Confirm_Flag
                //PAMS_Id
                //Id
                var photostd = "";
                if (promise.studentReg_DTObj[0].pasR_Student_Pic_Path != null && promise.studentReg_DTObj[0].pasR_Student_Pic_Path != '') {
                    photostd = promise.studentReg_DTObj[0].pasR_Student_Pic_Path;
                }
                else {
                    photostd = "https://jshsstorage.blob.core.windows.net/files/NoImage.jpg";
                }

                $('#blahnew').attr('src', photostd);

                $scope.studentphoto = photostd;

                $('#blahnewj').attr('src', promise.studentReg_DTObj[0].pasR_JointPhoto);
                $scope.studentphotojoint = promise.studentReg_DTObj[0].pasR_JointPhoto;
                //PASL_ID
                //PASL_ID
                //Remark
                //Repeat_Class_Id
                // $scope.FMCC_Id = promise.studentReg_DTObj[0].fmcC_ID;
                $scope.PASR_Noofbrothers = promise.studentReg_DTObj[0].pasR_Noofbrothers == null ? "" : promise.studentReg_DTObj[0].pasR_Noofbrothers;
                $scope.PASR_Noofsisters = promise.studentReg_DTObj[0].pasR_Noofsisters == null ? "" : promise.studentReg_DTObj[0].pasR_Noofsisters;

                $scope.PASR_NoOfElderBrothers = promise.studentReg_DTObj[0].pasR_NoOfElderBrothers == null ? "" : promise.studentReg_DTObj[0].pasR_NoOfElderBrothers;
                $scope.PASR_NoOfYoungerBrothers = promise.studentReg_DTObj[0].pasR_NoOfYoungerBrothers == null ? "" : promise.studentReg_DTObj[0].pasR_NoOfYoungerBrothers;

                $scope.PASR_NoOfElderSisters = promise.studentReg_DTObj[0].pasR_NoOfElderSisters == null ? "" : promise.studentReg_DTObj[0].pasR_NoOfElderSisters;
                $scope.PASR_NoOfYoungerSisters = promise.studentReg_DTObj[0].pasR_NoOfYoungerSisters == null ? "" : promise.studentReg_DTObj[0].pasR_NoOfYoungerSisters;


                $scope.PASR_lastclassperc = promise.studentReg_DTObj[0].pasR_lastclassperc == null ? "" : promise.studentReg_DTObj[0].pasR_lastclassperc;
                $scope.PASR_SibblingConcessionFlag = promise.studentReg_DTObj[0].pasR_SibblingConcessionFlag == 1 ? "Yes" : "No";
                $scope.PASR_ParentConcessionFlag = promise.studentReg_DTObj[0].pasR_ParentConcessionFlag == 1 ? "Yes" : "No";

                //// guardian
                if (promise.studentGuardian_DTObj != null && promise.studentGuardian_DTObj != "") {
                    $scope.pasrG_Id = promise.studentGuardian_DTObj[0].pasrG_Id;
                    $scope.PASRG_GuardianName = promise.studentGuardian_DTObj[0].pasrG_GuardianName;
                    $scope.PASRG_GuardianAddress = promise.studentGuardian_DTObj[0].pasrG_GuardianAddress;
                    $scope.PASRG_GuardianRelation = promise.studentGuardian_DTObj[0].pasrG_GuardianRelation;
                    $scope.PASRG_emailid = promise.studentGuardian_DTObj[0].pasrG_emailid;
                    $scope.PASRG_GuardianPhoneNo = promise.studentGuardian_DTObj[0].pasrG_GuardianPhoneNo;
                    $scope.PASRG_Occupation = promise.studentGuardian_DTObj[0].pasrG_Occupation;
                    $scope.PASRG_FeeUndertakeFlg = promise.studentGuardian_DTObj[0].pasrG_FeeUndertakeFlg;
                    $scope.PASRG_PhoneOffice = promise.studentGuardian_DTObj[0].pasrG_PhoneOffice;
                }
                else {
                    $scope.reg.pasrG_Id = 0;
                    $scope.reg.PASRG_GuardianName = "";
                    $scope.reg.PASRG_GuardianAddress = "";
                    $scope.reg.PASRG_GuardianRelation = "";
                    $scope.reg.PASRG_emailid = "";
                    $scope.reg.PASRG_GuardianPhoneNo = "";
                    $scope.reg.PASRG_PhoneOffice = "";
                    $scope.reg.PASRG_Occupation = "";
                    $scope.reg.PASRG_FeeUndertakeFlg = "";
                }
                ////

                //// Sibling
                $scope.firstsibling = '';
                $scope.firstsiblingclass = '';
                $scope.secondsibling = '';
                $scope.secondsiblingclass = '';
                if (promise.studentSbling_DTObj.length > 0) {
                    $scope.showrepatsib = true;
                    $scope.siblingsprint = promise.studentSbling_DTObj;

                    if ($scope.siblingsprint.length == 1) {
                        $scope.firstsibling = $scope.siblingsprint[0].pasrS_SiblingsName;
                        ////if ($scope.firstsibling === null || $scope.firstsibling === '') {
                        //    $scope.showrepatsib = false;
                        ////}
                        $scope.firstsiblingclass = $scope.siblingsprint[0].pasrS_SiblingsClass;
                        $scope.firstsiblingsection = $scope.siblingsprint[0].pasrS_SiblingsSection;
                        $scope.firstsiblingschool = $scope.siblingsprint[0].pasrS_SchoolName;
                        $scope.firstsiblingschoolAdmissionNo = $scope.siblingsprint[0].pasrS_SiblingsAdmissionNo;
                        $scope.firstsiblinginst = $scope.siblingsprint[0].pasrS_Institution;
                    }
                    if ($scope.siblingsprint.length == 2) {
                        $scope.firstsibling = $scope.siblingsprint[0].pasrS_SiblingsName;
                        $scope.firstsiblingclass = $scope.siblingsprint[0].pasrS_SiblingsClass;
                        $scope.firstsiblingsection = $scope.siblingsprint[0].pasrS_SiblingsSection;
                        $scope.firstsiblingschool = $scope.siblingsprint[0].pasrS_SchoolName;
                        $scope.firstsiblingschoolAdmissionNo = $scope.siblingsprint[0].pasrS_SiblingsAdmissionNo;
                        $scope.firstsiblinginst = $scope.siblingsprint[0].pasrS_Institution;
                        $scope.secondsibling = $scope.siblingsprint[1].pasrS_SiblingsName;
                        $scope.secondsiblingclass = $scope.siblingsprint[1].pasrS_SiblingsClass;
                        $scope.secondsiblingsection = $scope.siblingsprint[1].pasrS_SiblingsSection;
                        $scope.secondsiblingschool = $scope.siblingsprint[1].pasrS_SchoolName;
                        $scope.secondsiblingschoolAdmissionNo = $scope.siblingsprint[1].pasrS_SiblingsAdmissionNo;
                        $scope.secondsiblingschoolinst = $scope.siblingsprint[1].pasrS_Institution;
                    }
                    $scope.sibl = "Yes!";
                    $scope.showbr = false;
                    $scope.siblingshow = true;
                }
                else {
                    $scope.sibl = "No!";
                    $scope.showbr = true;
                    $scope.siblingshow = false;
                }
                if (promise.academicdrp.length > 0) {
                    $scope.ASMAY_Year = promise.academicdrp[0].asmaY_Year;
                }
                $scope.allRefrence = promise.studentReferenceDetails;
                $scope.allSources = promise.studentSourceDetails;
                $scope.reference = "";
                $scope.source = "";
                if ($scope.allRefrence != null && $scope.allRefrence.length > 0) {
                    //$scope.reference = $scope.allRefrence[0].
                }
                if ($scope.allSources != null && $scope.allSources.length > 0) {
                    // $scope.source = $scope.allRefrence[0].
                }
                if (promise.studenthelthDTO != null) {
                    if (promise.studenthelthDTO.length > 0) {
                        $scope.albumNameArray1 = [];
                        $scope.albumNameArray2 = [];
                        for (var i = 0; i < promise.studenthelthDTO.length; i++) {
                            if (promise.studenthelthDTO[i].pasR_FirstName != '') {
                                if (promise.studenthelthDTO[i].pasR_MiddleName !== null) {
                                    if (promise.studenthelthDTO[i].pasR_LastName !== null) {

                                        $scope.albumNameArray1.push({ name: promise.studenthelthDTO[i].pasR_FirstName + " " + promise.studenthelthDTO[i].pasR_MiddleName + " " + promise.studenthelthDTO[i].pasR_LastName });
                                    }
                                    else {
                                        $scope.albumNameArray1.push({ name: promise.studenthelthDTO[i].pasR_FirstName + " " + promise.studenthelthDTO[i].pasR_MiddleName });
                                    }
                                }
                                else {
                                    $scope.albumNameArray1.push({ name: promise.studenthelthDTO[i].pasR_FirstName });
                                }
                            }
                        }

                        $scope.Hstuname = $scope.albumNameArray1[0].name;
                        $scope.Hstuage = promise.studenthelthDTO[0].pasR_Age;
                        $scope.HFirstName = promise.studenthelthDTO[0].pasR_FatherName;
                        $scope.VaccinationDate = promise.studenthelthDTO[0].pashD_VaccinationDate;
                        if (promise.studenthelthDTO[0].pashD_FitsFlag == 1) {
                            $scope.FitsFlag = "Yes";
                            $scope.FitsDate = promise.studenthelthDTO[0].pashD_FitsDate;
                        }
                        else {
                            $scope.FitsFlag = "No";
                            $scope.FitsDate = " ";
                        }
                        if (promise.studenthelthDTO[0].pashD_AllergyFlg == true) {
                            $scope.AllergyFlag = "Yes";
                            $scope.Allergy = promise.studenthelthDTO[0].pashD_Allergy;
                        }
                        else {
                            $scope.AllergyFlag = "No";
                            $scope.Allergy = " ";
                        }

                        if (promise.studenthelthDTO[0].pashD_ChickenpoxFlag == 1) {
                            $scope.chickenFlag = "Yes";
                            $scope.cihickenDate = promise.studenthelthDTO[0].pashD_ChickenpoxDate;
                        }
                        else {
                            $scope.chickenFlag = "No";
                            $scope.cihickenDate = " ";
                        }
                        if (promise.studenthelthDTO[0].pashD_DiptheriaFlag == 1) {
                            $scope.dipFlag = "Yes";
                            $scope.dipDate = promise.studenthelthDTO[0].pashD_DiptheriaDate;
                        }
                        else {
                            $scope.dipFlag = "No";
                            $scope.dipDate = " ";
                        }
                        if (promise.studenthelthDTO[0].pashD_EpidemicFlag == 1) {
                            $scope.epideFlag = "Yes";
                            $scope.epideDate = promise.studenthelthDTO[0].pashD_EpidemicDate;
                        }
                        else {
                            $scope.epideFlag = "No";
                            $scope.epideDate = " ";
                        }
                        if (promise.studenthelthDTO[0].pashD_MeaslesFlag == 1) {
                            $scope.measleFlag = "Yes";
                            $scope.measleDate = promise.studenthelthDTO[0].pashD_MeaslesDate;
                        }
                        else {
                            $scope.measleFlag = "No";
                            $scope.measleDate = " ";
                        }
                        if (promise.studenthelthDTO[0].pashD_MumpsFlag == 1) {
                            $scope.mumFlag = "Yes";
                            $scope.mumDate = promise.studenthelthDTO[0].pashD_MumpsDate;
                        }
                        else {
                            $scope.mumFlag = "No";
                            $scope.mumDate = " ";
                        }
                        if (promise.studenthelthDTO[0].pashD_RingwormFlag == 1) {
                            $scope.ringFlag = "Yes";
                            $scope.ringDate = promise.studenthelthDTO[0].pashD_RingwormDate;
                        }
                        else {
                            $scope.ringFlag = "No";
                            $scope.ringDate = " ";
                        }
                        if (promise.studenthelthDTO[0].pashD_ScarletFlag == 1) {
                            $scope.scarletFlag = "Yes";
                            $scope.scarletDate = promise.studenthelthDTO[0].pashD_ScarletDate;
                        }
                        else {
                            $scope.scarletFlag = "No";
                            $scope.scarletDate = " ";
                        }
                        if (promise.studenthelthDTO[0].pashD_SmallPoxFlag == 1) {
                            $scope.smallFlag = "Yes";
                            $scope.smallDate = promise.studenthelthDTO[0].pashD_SmallPoxDate;
                        }
                        else {
                            $scope.smallFlag = "No";
                            $scope.smallDate = " ";
                        }
                        if (promise.studenthelthDTO[0].pashD_WhoopingFlag == 1) {
                            $scope.whoFlag = "Yes";
                            $scope.whoDate = promise.studenthelthDTO[0].pashD_WhoopingDate;
                        }
                        else {
                            $scope.whoFlag = "No";
                            $scope.whoDate = " ";
                        }

                        if (promise.studenthelthDTO[0].pashD_Illness == 1) {
                            $scope.Illness = "Yes";

                        }
                        else {
                            $scope.Illness = "No";

                        }

                        //$scope.Illness = promise.studenthelthDTO[0].pashD_Illness;
                        $scope.HepatitisB = new Date(promise.studenthelthDTO[0].pashD_HepatitisB);
                        $scope.TyphoidFever = new Date(promise.studenthelthDTO[0].pashD_TyphoidFever);
                        $scope.Ailment = promise.studenthelthDTO[0].pashD_Ailment;
                        //if (promise.studenthelthDTO[0].pashD_Allergy == 1) {
                        //    $scope.Allergy = "Yes";
                        //}
                        //else {
                        //    $scope.Allergy = "No";
                        //}

                        $scope.HealthProblem = promise.studenthelthDTO[0].pashD_HealthProblem;
                        $scope.CronicDesease = promise.studenthelthDTO[0].pashD_CronicDesease;
                        $scope.MEDetails = promise.studenthelthDTO[0].pashD_MEDetails;
                        $scope.MEContactNo = promise.studenthelthDTO[0].pashD_MEContactNo;
                        if (promise.vaccines != null) {
                            if (promise.vaccines.length > 0) {
                                $scope.vaccinesprint = promise.vaccines;
                                //if (promise.vaccines.length > 0) {
                                //    angular.forEach($scope.vaccinesprint, function (opqr1) {
                                //        if (opqr1.pamvA_YesNoFlg == true) {
                                //            opqr1.pamvA_YesNoFlg = "1";
                                //        }
                                //        else if (opqr1.pamvA_YesNoFlg == false) {
                                //            opqr1.pamvA_YesNoFlg = "0";
                                //        }
                                //    })
                                //}
                            }
                        }
                        // $scope.BloodGroup = promise.studenthelthDTO[0].pashD_BloodGroup;
                    }
                }

                //documnets
                if (promise.studentDocuments_DTObj.length > 0) {
                    $scope.document = {};
                    $scope.adharDocumentName = promise.studentDocuments_DTObj[0].amsmD_DocumentName;
                    //$scope.documentList = promise.studentDocuments_DTObj;
                    //angular.forEach(promise.studentDocuments_DTObj, function (value, key) {
                    //    //$scope.document.pasrD_Id = value.pasrD_Id;
                    //    //$scope.document.pasR_Id = value.pasR_Id;
                    //    //$scope.document.amsmD_Id = value.amsmD_Id;
                    //    //$scope.document.amsmD_DocumentName = value.amsmD_DocumentName;
                    //    //$scope.document.document_Path = value.document_Path;
                    //    $('#' + value.amsmD_Id).attr('src', value.document_Path);
                    //})
                }

                //// Subjects
                if (promise.studentSubjects_DTObj != null && promise.studentSubjects_DTObj.length > 0) {
                    //  $scope.electivesubgrouplist = promise.StudentSubjects_DTObj;
                    $scope.electivegrouplistprint = promise.electivegrouplist;
                    $scope.electivesubgrouplistprint = promise.electivesubgrouplist;

                    angular.forEach(promise.studentSubjects_DTObj, function (opqr) {
                        angular.forEach($scope.electivesubgrouplistprint, function (opqr1) {
                            if (opqr.emG_Id == opqr1.EMG_Id) {
                                opqr1.ismS_Id = opqr.ismS_Id;
                            }
                        })
                    })

                    $scope.grouplength = $scope.electivesubgrouplistprint.length;

                }
                else {
                    $scope.electivegrouplistprint = {};
                    $scope.electivesubgrouplistprint = {};
                }

                $scope.studentstream = promise.studentstream;

                if ($scope.electivesubgrouplistprint != null && $scope.electivesubgrouplistprint.length >= 0) {
                    $scope.electives = [];
                    $scope.compulsory = [];
                    angular.forEach($scope.electivesubgrouplistprint, function (opqr1) {
                        if (opqr1.EMG_ElectiveFlg == false) {
                            $scope.compulsory.push(opqr1);
                        }
                        else {
                            $scope.electives.push(opqr1);
                        }
                    })
                }
                if (promise.streamexamsprint != null && promise.streamexamsprint.length > 0) {
                    $scope.streamexamsprint = promise.streamexamsprint;
                    $scope.asmceid = promise.asmceId;
                }
                else {
                    $scope.streamexamsprint = [];
                }



                ////// Previous School

                if (promise.studentPrevSch_DTObj.length > 0) {
                    $scope.PreviousSchoolListprint = promise.studentPrevSch_DTObj;

                    $scope.schoolname = $scope.PreviousSchoolListprint[0].pasrpS_PrvSchoolName;
                    $scope.tcno = $scope.PreviousSchoolListprint[0].pasrpS_TcNo;
                    $scope.schooladress = $scope.PreviousSchoolListprint[0].pasrpS_Address;
                    $scope.lastclass = $scope.PreviousSchoolListprint[0].pasrpS_PreviousClass;
                    $scope.lastsylaabus = $scope.PreviousSchoolListprint[0].pasrpS_Board;
                    $scope.percentage = $scope.PreviousSchoolListprint[0].pasrpS_PreviousPer;
                    $scope.leftdate = $scope.PreviousSchoolListprint[0].pasrpS_LeftDate;
                    $scope.TcIssueDate = $scope.PreviousSchoolListprint[0].pasrpS_TcIssueDate;
                    $scope.TCProducingDate = $scope.PreviousSchoolListprint[0].pasrpS_TCProducingDate;
                    $scope.lefyear = $scope.PreviousSchoolListprint[0].pasrpS_LeftYear;
                    $scope.concessionscholer = $scope.PreviousSchoolListprint[0].pasrpS_ConcOrScholarshipFlg;
                    $scope.leftreason = $scope.PreviousSchoolListprint[0].pasrpS_LeftReason;
                    if ($scope.leftdate != null) {
                        $scope.leftreasondate = $scope.PreviousSchoolListprint[0].pasrpS_LeftDate;
                    }
                    else {
                        $scope.leftreasondate = "";
                    }

                    if ($scope.concessionscholer != null && $scope.concessionscholer != "" && $scope.concessionscholer != 'No' && $scope.concessionscholer != 'NAN' && $scope.concessionscholer != 'NA') {
                        $scope.concessiondate = $scope.PreviousSchoolListprint[0].pasrpS_ConcOrScholarshipDate
                    }
                    else {
                        $scope.concessiondate = "---";
                    }

                    $scope.obj.fatherimg = promise.studentReg_DTObj[0].pasR_FatherPhoto;
                    $scope.obj.motherimg = promise.studentReg_DTObj[0].pasR_MotherPhoto;

                }
                $('#blahfnewj').attr('src', promise.studentReg_DTObj[0].pasR_JointPhoto);
                $('#blahfnew').attr('src', promise.studentReg_DTObj[0].pasR_FatherPhoto);
                $('#blahfnew1').attr('src', promise.studentReg_DTObj[0].pasR_FatherPhoto);
                $('#blahmnew').attr('src', promise.studentReg_DTObj[0].pasR_MotherPhoto);
                $('#blahmnew1').attr('src', promise.studentReg_DTObj[0].pasR_MotherPhoto);
                $('#blahnewa').attr('src', promise.studentReg_DTObj[0].pasR_Student_Pic_Path);
                $('#blahnewaa').attr('src', promise.studentReg_DTObj[0].pasR_Student_Pic_Path);
                //var e1 = angular.element(document.getElementById("test"));
                //$compile(e1.html(promise.htmldata))(($scope));

                $('#blahnew1').attr('src', $scope.studentphoto);
                $('#blahnew').attr('src', $scope.studentphoto);
                $('#blahfnewj').attr('src', promise.studentReg_DTObj[0].pasR_JointPhoto);
                $('#blahfnew').attr('src', promise.studentReg_DTObj[0].pasR_FatherPhoto);
                $('#blahfnew1').attr('src', promise.studentReg_DTObj[0].pasR_FatherPhoto);
                $('#blahmnew').attr('src', promise.studentReg_DTObj[0].pasR_MotherPhoto);
                $('#blahmnew1').attr('src', promise.studentReg_DTObj[0].pasR_MotherPhoto);
                $('#blahnewa').attr('src', promise.studentReg_DTObj[0].pasR_Student_Pic_Path);
                $('#blahnewaa').attr('src', promise.studentReg_DTObj[0].pasR_Student_Pic_Path);


                var e1 = angular.element(document.getElementById("test"));
                $compile(e1.html(promise.htmldata))(($scope));
                //$('#test').modal('show');
                $('#myModal1').modal('show');

            });
        }
        $scope.ardochange = function (typeofc) {
            if (typeofc == "1") {
                $scope.reg.PASR_Alumani = 'Father';
            }
            else if (typeofc == "2") {
                $scope.reg.PASR_Alumani = 'Mother';
            }
            else if (typeofc == "3") {
                $scope.reg.PASR_Alumani = 'Other';
            }
            else {
                $scope.reg.PASR_Alumani = '';
            }
        }
        $scope.srdochange = function (typeofc) {
            if (typeofc == "1") {
                $scope.reg.PASR_Stayingwith = 'Both Perents';
            }
            else if (typeofc == "2") {
                $scope.reg.PASR_Stayingwith = 'Father';
            }
            else if (typeofc == "3") {
                $scope.reg.PASR_Stayingwith = 'Mother';
            }
            else if (typeofc == "4") {
                $scope.reg.PASR_Stayingwith = 'Guardian';
            }
            else {
                $scope.reg.PASR_Alumani = '';
            }
        }
        $scope.rdochange = function (typeofc) {
            if (typeofc == "1") {
                $scope.reg.PASR_FatherOccupation = 'Government Employee';
            }
            else if (typeofc == "2") {
                $scope.reg.PASR_FatherOccupation = 'Private Employee';
            }
            else if (typeofc == "3") {
                $scope.reg.PASR_FatherOccupation = 'Self Employee';
            }
            else if (typeofc == "4") {
                $scope.reg.PASR_FatherOccupation = '';
            }
        }
        $scope.rdochangem = function (typeofcm) {
            if (typeofcm == "1") {
                $scope.reg.PASR_MotherOccupation = 'Government Employee';
            }
            else if (typeofcm == "2") {
                $scope.reg.PASR_MotherOccupation = 'Private Employee';
            }
            else if (typeofcm == "3") {
                $scope.reg.PASR_MotherOccupation = 'Self Employee';
            }
            else if (typeofcm == "4") {
                $scope.reg.PASR_MotherOccupation = '';
            }
        }
        $scope.rdochangeg = function (typeofcg) {
            if (typeofcg == "1") {
                $scope.reg.PASRG_Occupation = 'Government Employee';
            }
            else if (typeofcg == "2") {
                $scope.reg.PASRG_Occupation = 'Private Employee';
            }
            else if (typeofcg == "3") {
                $scope.reg.PASRG_Occupation = 'Self Employee';
            }
            else if (typeofcg == "4") {
                $scope.reg.PASRG_Occupation = '';
            }
        }

        $scope.chkbox_addressDis = true;
        //$scope.checkboxValidate = function (reg) {
        //    if ((reg.PASR_ConStreet != "" && reg.PASR_ConStreet != undefined) && (reg.PASR_ConArea != "" && reg.PASR_ConArea != undefined) && (reg.PASR_ConCountry != "" && reg.PASR_ConCountry != undefined) && (reg.PASR_ConState != "" && reg.PASR_ConState != undefined) && (reg.PASR_ConCity != "" && reg.PASR_ConCity != undefined) && (reg.PASR_ConPincode != "" && reg.PASR_ConPincode != undefined) && (reg.PASR_ConDistrict != "" && reg.PASR_ConDistrict != undefined)) {
        //        $scope.chkbox_addressDis = false;
        //    }
        //    else {
        //        $scope.chkbox_addressDis = true;
        //    }
        //}
        $scope.checkboxValidate = function (reg) {
            if (((reg.PASR_ConStreet != "" && reg.PASR_ConStreet != undefined) && (reg.PASR_ConArea != "" && reg.PASR_ConArea != undefined) && (reg.PASR_ConCountry != "" && reg.PASR_ConCountry != undefined) && (reg.PASR_ConState != "" && reg.PASR_ConState != undefined) && (reg.PASR_ConCity != "" && reg.PASR_ConCity != undefined) && (reg.PASR_ConPincode != "" && reg.PASR_ConPincode != undefined)) || (reg.PASR_ConDistrict != "" && reg.PASR_ConDistrict != undefined)) {
                $scope.chkbox_addressDis = false;

            }
            else {
                $scope.chkbox_addressDis = true;

            }
        }

        //address copy 
        $scope.PermanentDis = false;
        $scope.CommunicationAdDis = false;
        $scope.address_copy = function (da) {
            if (da === 1) {
                $scope.PermanentDis = true;
                $scope.CommunicationAdDis = true;
                $scope.reg.PASR_PerStreet = $scope.reg.PASR_ConStreet;
                $scope.reg.PASR_PerArea = $scope.reg.PASR_ConArea;
                $scope.reg.PASR_PerCountry = $scope.reg.PASR_ConCountry;
                $scope.reg.PASR_PerState = $scope.reg.PASR_ConState;
                $scope.reg.PASR_PerCity = $scope.reg.PASR_ConCity;
                $scope.reg.PASR_PerPincode = $scope.reg.PASR_ConPincode;
                var PerCountryId = $scope.reg.PASR_ConCountry;
                var PerStateId = $scope.reg.PASR_ConState;
                if ((PerCountryId !== null && PerCountryId !== "") && (PerStateId !== null && PerStateId !== "")) {
                    getSelectGetState(PerCountryId, PerStateId);
                }
            }
            else {
                $scope.PermanentDis = false;
                $scope.CommunicationAdDis = false;
                $scope.reg.PASR_PerStreet = "";
                $scope.reg.PASR_PerArea = "";
                $scope.reg.PASR_PerCountry = "";
                $scope.reg.PASR_PerState = "";
                $scope.reg.PASR_PerCity = "";
                $scope.reg.PASR_PerPincode = "";
            }
        }

        function Uploads(pasR_Id) {
            var formData = new FormData();
            for (var i = 0; i <= $scope.selectFileforUpload.length; i++) {
                formData.append("File", $scope.SelectedFileForUpload[i]);
            }

            for (var i = 0; i <= $scope.selectFileforUpload2.length; i++) {
                formData.append("File", $scope.SelectedFileForUpload2[i]);
            }

            for (var i = 0; i <= $scope.selectFileforUpload3.length; i++) {
                formData.append("File", $scope.SelectedFileForUpload3[i]);
            }

            for (var i = 0; i <= $scope.selectFileforUpload4.length; i++) {
                formData.append("File", $scope.SelectedFileForUpload4[i]);
            }

            for (var i = 0; i <= $scope.selectFileforUpload5.length; i++) {
                formData.append("File", $scope.SelectedFileForUpload5[i]);
            }

            for (var i = 0; i <= $scope.selectFileforUpload6.length; i++) {
                formData.append("File", $scope.SelectedFileForUpload6[i]);
            }

            for (var i = 0; i <= $scope.selectFileforUpload7.length; i++) {
                formData.append("File", $scope.SelectedFileForUpload7[i]);
            }

            for (var i = 0; i <= $scope.selectFileforUploadz.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadz[i]);
            }
            //We can send more data to server using append         
            formData.append("Id", pasR_Id);
            //var defer = $q.defer();
            $http.post("/api/ImageUpload/", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
        }
        //added by vishnu for Date Of Birth Words Convertion starts here 
        //month names
        $scope.getmontnames = function (monthid) {
            monthname = "";
            switch (monthid) {
                case 0:
                    monthname = "JANUARY"
                    break;
                case 1:
                    monthname = "FEBRUARY"
                    break;
                case 2:
                    monthname = "MARCH"
                    break;
                case 3:
                    monthname = "APRIL"
                    break;
                case 4:
                    monthname = "MAY"
                    break;
                case 5:
                    monthname = "JUNE"
                    break;
                case 6:
                    monthname = "JULY"
                    break;
                case 7:
                    monthname = "AUGUST"
                    break;
                case 8:
                    monthname = "SEPTEMBER"
                    break;
                case 9:
                    monthname = "OCTOBER"
                    break;
                case 10:
                    monthname = "NOVEMBER"
                    break;
                case 11:
                    monthname = "DECEMBER"
                    break;
                default:
                    monthname = ""
                    break;
            }
            return monthname;
        }

        //get datename

        $scope.getdatenames = function (dateid) {
            datename = "";
            switch (dateid) {
                case 1:
                    datename = "FIRST"
                    break;
                case 2:
                    datename = "SECOND"
                    break;
                case 3:
                    datename = "THIRD"
                    break;
                case 4:
                    datename = "FOURTH"
                    break;
                case 5:
                    datename = "FIFTH"
                    break;
                case 6:
                    datename = "SIXTH"
                    break;
                case 7:
                    datename = "SEVENTH"
                    break;
                case 8:
                    datename = "EIGHTH"
                    break;
                case 9:
                    datename = "NINTH"
                    break;
                case 10:
                    datename = "TENTH"
                    break;
                case 11:
                    datename = "ELEVENTH"
                    break;
                case 12:
                    datename = "TWELTH"
                    break;
                case 13:
                    datename = "THIRTEENTH"
                    break;
                case 14:
                    datename = "FOURTEENTH"
                    break;
                case 15:
                    datename = "FIFTEENTH"
                    break;
                case 16:
                    datename = "SIXTEENTH"
                    break;
                case 17:
                    datename = "SEVENTEENTH"
                    break;
                case 18:
                    datename = "EIGHTEENTH"
                    break;
                case 19:
                    datename = "NINTEENTH"
                    break;
                case 20:
                    datename = "TWENTY"
                    break;
                case 21:
                    datename = "TWENTY FIRST"
                    break;
                case 22:
                    datename = "TWENTY SECOND"
                    break;
                case 23:
                    datename = "TWENTY THIRD"
                    break;
                case 24:
                    datename = "TWENTY FOURTH"
                    break;
                case 25:
                    datename = "TWENTY FIFTH"
                    break;
                case 26:
                    datename = "TWENTY SIXTH"
                    break;
                case 27:
                    datename = "TWENTY SEVENTH"
                    break;
                case 28:
                    datename = "TWENTY EIGHTH"
                    break;
                case 29:
                    datename = "TWENTY NINTH"
                    break;
                case 30:
                    datename = "THIRTY"
                    break;
                case 31:
                    datename = "THIRTY FIRST"
                    break;
                default:
                    datename = ""
                    break;
            }
            return datename;
        }


        //getting year 1
        $scope.getyearname = function (yearid) {
            var yearid = yearid.toString();
            var yearsplit = yearid.substring(0, 2)
            var dob = parseInt(yearsplit);
            var yearsplit1 = yearid.substring(2, 4)
            var dob1 = parseInt(yearsplit1);
            yearname = "";
            var datad = yearid;
            var yearname1 = "";
            //var dob = datad(1, 2);
            //var dob1 = datad(3, 4);
            if (dob >= 20) {
                yearname = "TWO THOUSAND";
            }
            else {
                yearname = "NINTEEN";
            }
            switch (dob1) {
                case 1:
                    yearname1 = "ONE"
                    break;
                case 2:
                    yearname1 = "TWO"
                    break;
                case 3:
                    yearname1 = "THREE"
                    break;
                case 4:
                    yearname1 = "FOUR"
                    break;
                case 5:
                    yearname1 = "FIVE"
                    break;
                case 6:
                    yearname1 = "SIX"
                    break;
                case 7:
                    yearname1 = "SEVEN"
                    break;
                case 8:
                    yearname1 = "EIGHT"
                    break;
                case 9:
                    yearname1 = "NINE"
                    break;
                case 10:
                    yearname1 = "TEN"
                    break;
                case 11:
                    yearname1 = "ELEVEN"
                    break;
                case 12:
                    yearname1 = "TWELVE"
                    break;
                case 13:
                    yearname1 = "THIRTEEN"
                    break;
                case 14:
                    yearname1 = "FOURTEEN"
                    break;
                case 15:
                    yearname1 = "FIFTEEN"
                    break;
                case 16:
                    yearname1 = "SIXTEEN"
                    break;
                case 17:
                    yearname1 = "SEVENTEEN"
                    break;
                case 18:
                    yearname1 = "EIGHTEEN"
                    break;
                case 19:
                    yearname1 = "NINTEEN"
                    break;
                case 20:
                    yearname1 = "TWENTY"
                    break;
                case 21:
                    yearname1 = "TWENTY ONE"
                    break;
                case 22:
                    yearname1 = "TWENTY TWO"
                    break;
                case 23:
                    yearname1 = "TWENTY THREE"
                    break;
                case 24:
                    yearname1 = "TWENTY FOUR"
                    break;
                case 25:
                    yearname1 = "TWENTY FIVE"
                    break;
                case 26:
                    yearname1 = "TWENTY SIX"
                    break;
                case 27:
                    yearname1 = "TWENTY SEVEN"
                    break;
                case 28:
                    yearname1 = "TWENTY EIGHT"
                    break;
                case 29:
                    yearname1 = "TWENTY NINE"
                    break;
                case 30:
                    yearname1 = "THIRTY"
                    break;
                case 31:
                    yearname1 = "THIRTY ONE"
                    break;
                case 32:
                    yearname1 = "THIRTY TWO"
                    break;
                case 33:
                    yearname1 = "THIRTY THREE"
                    break;
                case 34:
                    yearname1 = "THIRTY FOUR"
                    break;
                case 35:
                    yearname1 = "THIRTY FIVE"
                    break;
                case 36:
                    yearname1 = "THIRTY SIX"
                    break;
                case 37:
                    yearname1 = "THIRTY SEVEN"
                    break;
                case 38:
                    yearname1 = "THIRTY EIGHT"
                    break;
                case 39:
                    yearname1 = "THIRTY NINE"
                    break;
                case 40:
                    yearname1 = "FOURTY"
                    break;
                case 41:
                    yearname1 = "FOURTY ONE"
                    break;
                case 42:
                    yearname1 = "FOURTY TWO"
                    break;
                case 43:
                    yearname1 = "FOURTY THREE"
                    break;
                case 44:
                    yearname1 = "FOURTY FOUR"
                    break;
                case 45:
                    yearname1 = "FOURTY FIVE"
                    break;
                case 46:
                    yearname1 = "FOURTY SIX"
                    break;
                case 47:
                    yearname1 = "FOURTY SEVEN"
                    break;
                case 48:
                    yearname1 = "FOURTY EIGHT"
                    break;
                case 49:
                    yearname1 = "FOURTY NINE"
                    break;
                case 50:
                    yearname1 = "FIFTY"
                    break;
                case 51:
                    yearname1 = "FIFTY ONE"
                    break;
                case 52:
                    yearname1 = "FIFTY TWO"
                    break;
                case 53:
                    yearname1 = "FIFTY THREE"
                    break;
                case 54:
                    yearname1 = "FIFTY FOUR"
                    break;
                case 55:
                    yearname1 = "FIFTY FIVE"
                    break;
                case 56:
                    yearname1 = "FIFTY SIX"
                    break;
                case 57:
                    yearname1 = "FIFTY SEVEN"
                    break;
                case 58:
                    yearname1 = "FIFTY EIGHT"
                    break;
                case 59:
                    yearname1 = "FIFTY NINE"
                    break;
                case 60:
                    yearname1 = "SIXTY"
                    break;
                case 61:
                    yearname1 = "SIXTY ONE"
                    break;
                case 62:
                    yearname1 = "SIXTY TWO"
                    break;
                case 63:
                    yearname1 = "SIXTY THREE"
                    break;
                case 64:
                    yearname1 = "SIXTY FOUR"
                    break;
                case 65:
                    yearname1 = "SIXTY FIVE"
                    break;
                case 66:
                    yearname1 = "SIXTY SIX"
                    break;
                case 67:
                    yearname1 = "SIXTY SEVEN"
                    break;
                case 68:
                    yearname1 = "SIXTY EIGHT"
                    break;
                case 69:
                    yearname1 = "SIXTY NINE"
                    break;
                case 70:
                    yearname1 = "SEVENTY"
                    break;
                case 71:
                    yearname1 = "SEVENTY ONE"
                    break;
                case 72:
                    yearname1 = "SEVENTY TWO"
                    break;
                case 73:
                    yearname1 = "SEVENTY THREE"
                    break;
                case 74:
                    yearname1 = "SEVENTY FOUR"
                    break;
                case 75:
                    yearname1 = "SEVENTY FIVE"
                    break;
                case 76:
                    yearname1 = "SEVENTY SIX"
                    break;
                case 77:
                    yearname1 = "SEVENTY SEVEN"
                    break;
                case 78:
                    yearname1 = "SEVENTY EIGHT"
                    break;
                case 79:
                    yearname1 = "SEVENTY NINE"
                    break;
                case 80:
                    yearname1 = "EIGHTY"
                    break;
                case 81:
                    yearname1 = "EIGHTY ONE"
                    break;
                case 82:
                    yearname1 = "EIGHTY TWO"
                    break;
                case 83:
                    yearname1 = "EIGHTY THREE"
                    break;
                case 84:
                    yearname1 = "EIGHTY FOUR"
                    break;
                case 85:
                    yearname1 = "EIGHTY FIVE"
                    break;
                case 86:
                    yearname1 = "EIGHTY SIX"
                    break;
                case 87:
                    yearname1 = "EIGHTY SEVEN"
                    break;
                case 88:
                    yearname1 = "EIGHTY EIGHT"
                    break;
                case 89:
                    yearname1 = "EIGHTY NINE"
                    break;
                case 90:
                    yearname1 = "NINTY"
                    break;
                case 91:
                    yearname1 = "NINTY ONE"
                    break;
                case 92:
                    yearname1 = "NINTY TWO"
                    break;
                case 93:
                    yearname1 = "NINTY THREE"
                    break;
                case 94:
                    yearname1 = "NINTY FOUR"
                    break;
                case 95:
                    yearname1 = "NINTY FIVE"
                    break;
                case 96:
                    yearname1 = "NINTY SIX"
                    break;
                case 97:
                    yearname1 = "NINTY SEVEN"
                    break;
                case 98:
                    yearname1 = "NINTY EIGHT"
                    break;
                case 99:
                    yearname1 = "NINTY NINE"
                    break;
                case 0:
                    yearname1 = "ZERO"
                    break;
                default:
                    yearname1 = ""
                    break;
            }
            yearname = yearname + ' ' + yearname1;
            return yearname;
        }
        //end here 
        //**************************health certificate*********************//
        $scope.submitted2 = false;
        //$scope.PASHD_TyphoidFever = new Date();
        $scope.savehealthcertificate = function () {
            $scope.studentvaccinations = [];
            var studentpasrid = $scope.pasr_id;
            $scope.submitted2 = true;
            if ($scope.PASHD_FitsFlag == undefined || $scope.PASHD_FitsFlag == "0") {
                $scope.PASHD_FitsFlag = 0;
                $scope.PASHD_FitsDate = new Date();
            }
            if ($scope.PASHD_ChickenpoxFlag == "0" || $scope.PASHD_ChickenpoxFlag == undefined) {
                $scope.PASHD_ChickenpoxFlag = 0;
                $scope.PASHD_ChickenpoxDate = new Date();
            }
            if ($scope.PASHD_DiptheriaFlag == "0" || $scope.PASHD_DiptheriaFlag == undefined) {
                $scope.PASHD_DiptheriaFlag = 0;
                $scope.PASHD_DiptheriaDate = new Date();
            }
            if ($scope.PASHD_EpidemicFlag == "0" || $scope.PASHD_EpidemicFlag == undefined) {
                $scope.PASHD_EpidemicFlag = 0;
                $scope.PASHD_EpidemicDate = new Date();
            }
            if ($scope.PASHD_MeaslesFlag == "0" || $scope.PASHD_MeaslesFlag == undefined) {
                $scope.PASHD_MeaslesFlag = 0;
                $scope.PASHD_MeaslesDate = new Date();
            }
            if ($scope.PASHD_MumpsFlag == "0" || $scope.PASHD_MumpsFlag == undefined) {

                $scope.PASHD_MumpsFlag = 0;
                $scope.PASHD_MumpsDate = new Date();
            }
            if ($scope.PASHD_RingwormFlag == "0" || $scope.PASHD_RingwormFlag == undefined) {
                $scope.PASHD_RingwormFlag = 0;
                $scope.PASHD_RingwormDate = new Date();
            }
            if ($scope.PASHD_ScarletFlag == "0" || $scope.PASHD_ScarletFlag == undefined) {
                $scope.PASHD_ScarletFlag = 0;
                $scope.PASHD_ScarletDate = new Date();
            }
            if ($scope.PASHD_SmallPoxFlag == "0" || $scope.PASHD_SmallPoxFlag == undefined) {
                $scope.PASHD_SmallPoxFlag = 0;
                $scope.PASHD_SmallPoxDate = new Date();
            }
            if ($scope.PASHD_WhoopingFlag == "0" || $scope.PASHD_WhoopingFlag == undefined) {
                $scope.PASHD_WhoopingFlag = 0;
                $scope.PASHD_WhoopingDate = new Date();
            }
            if ($scope.pashD_Id == undefined) {
                $scope.pashD_Id = 0;
            }
            if ($scope.PASHD_Illness == undefined) {
                $scope.PASHD_Illness = 0;
            }

            if ($scope.PASHD_AllergyFlg == "0" || $scope.PASHD_AllergyFlg == undefined) {
                $scope.PASHD_AllergyFlg = false;
            }
            else {
                $scope.PASHD_AllergyFlg = true;
            }
            if ($scope.PASHD_VaccinationDate == undefined) {
                $scope.PASHD_VaccinationDate = new Date();
            }

            if ($scope.PASHD_HepatitisB == undefined) {
                $scope.PASHD_HepatitisB = new Date();
            }

            if ($scope.PASHD_Ailment == undefined) {
                $scope.PASHD_Ailment = '';
            }
            if ($scope.PASHD_Allergy == undefined) {
                $scope.PASHD_Allergy = '';
            }
            if ($scope.myFormhelth.$valid) {
                if ($scope.vaccines.length > 0) {
                    angular.forEach($scope.vaccines, function (opqr1) {
                        if (opqr1.pamvA_YesNoFlg == "1") {
                            opqr1.pamvA_YesNoFlg = true;
                        }
                        else if (opqr1.pamvA_YesNoFlg == "0") {
                            opqr1.pamvA_YesNoFlg = false;
                        }
                    })
                }
                $scope.studentvaccinations = $scope.vaccines;
                var data = {
                    "PASR_Id": $scope.pasr_id,
                    "PASHD_Id": $scope.pashD_Id,
                    "PASHD_VaccinationDate": new Date($scope.PASHD_VaccinationDate).toDateString(),
                    "PASHD_FitsFlag": $scope.PASHD_FitsFlag,
                    "PASHD_FitsDate": new Date($scope.PASHD_FitsDate).toDateString(),
                    "PASHD_AllergyFlg": $scope.PASHD_AllergyFlg,
                    "PASHD_Allergy": $scope.PASHD_Allergy,
                    "PASHD_ChickenpoxFlag": $scope.PASHD_ChickenpoxFlag,
                    "PASHD_ChickenpoxDate": new Date($scope.PASHD_ChickenpoxDate).toDateString(),
                    "PASHD_DiptheriaFlag": $scope.PASHD_DiptheriaFlag,
                    "PASHD_DiptheriaDate": new Date($scope.PASHD_DiptheriaDate).toDateString(),
                    "PASHD_EpidemicFlag": $scope.PASHD_EpidemicFlag,
                    "PASHD_EpidemicDate": new Date($scope.PASHD_EpidemicDate).toDateString(),
                    "PASHD_MeaslesFlag": $scope.PASHD_MeaslesFlag,
                    "PASHD_MeaslesDate": new Date($scope.PASHD_MeaslesDate).toDateString(),
                    "PASHD_MumpsFlag": $scope.PASHD_MumpsFlag,
                    "PASHD_MumpsDate": new Date($scope.PASHD_MumpsDate).toDateString(),
                    "PASHD_RingwormFlag": $scope.PASHD_RingwormFlag,
                    "PASHD_RingwormDate": new Date($scope.PASHD_RingwormDate).toDateString(),
                    "PASHD_ScarletFlag": $scope.PASHD_ScarletFlag,
                    "PASHD_ScarletDate": new Date($scope.PASHD_ScarletDate).toDateString(),
                    "PASHD_SmallPoxFlag": $scope.PASHD_SmallPoxFlag,
                    "PASHD_SmallPoxDate": new Date($scope.PASHD_SmallPoxDate).toDateString(),
                    "PASHD_WhoopingFlag": $scope.PASHD_WhoopingFlag,
                    "PASHD_WhoopingDate": new Date($scope.PASHD_WhoopingDate).toDateString(),
                    "PASHD_Illness": $scope.PASHD_Illness,
                    "PASHD_HepatitisB": $scope.PASHD_HepatitisB,
                    "PASHD_TyphoidFever": $scope.PASHD_TyphoidFever,
                    "PASHD_Ailment": $scope.PASHD_Ailment,
                    //"PASHD_Allergy": $scope.PASHD_Allergy,
                    "PASHD_HealthProblem": $scope.PASHD_HealthProblem,
                    "PASHD_CronicDesease": $scope.PASHD_CronicDesease,
                    "PASHD_MEContactNo": $scope.PASHD_MEContactNo,
                    "PASHD_MEDetails": $scope.PASHD_MEDetails,
                    StudentVaccines: $scope.studentvaccinations,
                    //"PASHD_BloodGroup": $scope.PASHD_BloodGroup
                }
                apiService.create("StudentApplication/savehealthcertificatedetail", data).
                    then(function (promise) {
                        if (promise.returnval === true) {
                            // swal('Data successfully Saved');
                            if (promise.updated == true) {
                                if ($scope.vaccinesrebind.length > 0) {
                                    angular.forEach($scope.vaccinesrebind, function (opqr1) {
                                        opqr1.pamvA_YesNoFlg = "";
                                    })
                                }
                                $scope.vaccines = $scope.vaccinesrebind;
                                swal('Data Updated successfully');
                            }
                            else if (promise.updated == false) {
                                swal({
                                    title: "Application Submitted successfully!!",
                                    text: "Do you want to Pay Pre-Admission Application Form Fee ..!?",
                                    type: "success",
                                    showCancelButton: true,
                                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes!",
                                    cancelButtonText: "Cancel!",
                                    closeOnConfirm: false,
                                    closeOnCancel: true
                                },
                                    function (isConfirm) {
                                        if (isConfirm) {
                                            $scope.pasR_Id = studentpasrid;
                                            $scope.paynow(studentpasrid);
                                            swal.close();
                                        }
                                        else {
                                            $state.reload();
                                        }
                                    });
                            }
                            $scope.clerehealthcertificate();
                            $scope.loadstunames("organization", "institute");
                        }
                        else if (promise.returnval === false && promise.returnduplicatestatus === 'Duplicate') {
                            swal('Recards AlReady Exist !');
                        }
                        else {
                            swal('Data Not Saved !');
                        }
                    })
            }
            else {
                $scope.studentvaccinations = $scope.vaccines;
            }
        };
        $scope.clerehealthcertificate = function () {
            $scope.pashD_Id = 0;
            $scope.PASR_Id = 0;
            $scope.PASHD_VaccinationDate = "";
            $scope.PASHD_FitsFlag = 0;
            $scope.PASHD_AllergyFlg = 0;
            $scope.PASHD_FitsDate = "";
            $scope.PASHD_Illness = 0;
            $scope.PASHD_HepatitisB = "";
            $scope.PASHD_TyphoidFever = undefined;
            $scope.PASHD_Ailment = "";
            $scope.PASHD_Allergy = 0;
            $scope.PASHD_HealthProblem = "";
            $scope.PASHD_CronicDesease = "";
            $scope.PASHD_MEDetails = "";
            $scope.PASHD_MEContactNo = "";
            // $scope.PASHD_BloodGroup = "";
            $scope.PASHD_ChickenpoxFlag = 0;
            $scope.PASHD_ChickenpoxDate = "";
            $scope.PASHD_DiptheriaFlag = 0;
            $scope.PASHD_DiptheriaDae = "";
            $scope.PASHD_EpidemicFlag = 0;
            $scope.PASHD_EpidemicDate = "";
            $scope.PASHD_MeaslesFlag = 0;
            $scope.PASHD_MeaslesDate = "";
            $scope.PASHD_MumpsFlag = 0;
            $scope.PASHD_MumpsDate = "";
            $scope.PASHD_RingwormFlag = 0;
            $scope.PASHD_RingwormDate = "";
            $scope.PASHD_ScarletFlag = 0;
            $scope.PASHD_ScarletDate = "";
            $scope.PASHD_SmallPoxFlag = 0;
            $scope.PASHD_SmallPoxDate = "";
            $scope.PASHD_WhoopingFlag = 0;
            $scope.PASHD_WhoopingDate = "";
            $scope.showhidechicken = false;
            $scope.showhidedip = false;
            $scope.showhideepi = false;
            $scope.showhidemea = false;
            $scope.showhidemum = false;
            $scope.showhiderin = false;
            $scope.showhidesca = false;
            $scope.showhidesma = false;
            $scope.showhidewho = false;
            $scope.showhide = false;
            $scope.submitted2 = false;
            $scope.myFormhelth.$setPristine();
            $scope.myFormhelth.$setUntouched();
            $scope.loadstunames("organization", "institute");
            angular.forEach($scope.vaccines, function (opqr1) {
                opqr1.pamvA_YesNoFlg = "";
            })

        };
        $scope.loadstunames = function (organization, institute) {
            var data = { "Organization": organization, "Institute": institute }
            apiService.create("StudentApplication/getstudata", data).then(function (promise) {
                $scope.currentPage1 = 1;
                $scope.itemsPerPage1 = paginationformasters;
                $scope.prospectusPaymentlist = promise.prospectusPaymentlist;
                $scope.pasr_id = "";
                //$scope.PASHD_VaccinationDate = "";
                //$scope.PASHD_TyphoidFever = "";
                //---payment check---
                //$scope.studentslist = [];
                //if ($scope.configurationsettings.ispaC_ApplFeeFlag === 1) {
                //    if ($scope.prospectusPaymentlist.length > 0) {
                //        for (var i = 0; i < promise.studentDetails.length; i++) {
                //            for (var j = 0; j < $scope.prospectusPaymentlist.length; j++) {
                //                if (promise.studentDetails[i].pasR_Id == $scope.prospectusPaymentlist[j].pasA_Id && $scope.configurationsettings.ispaC_ApplFeeFlag == 1) {
                //                    $scope.studentslist.push(promise.studentDetails[i]);
                //                }
                //            }
                //        }

                //    }
                //}
                //promise.studentDetails = $scope.studentslist;
                //added by suryan
                $scope.albumNameArray1 = [];
                if (promise.studentDetails != null && promise.studentDetails != undefined && promise.studentDetails.length > 0) {
                    for (var i = 0; i < promise.studentDetails.length; i++) {
                        if (promise.studentDetails[i].pasR_FirstName != '') {
                            if (promise.studentDetails[i].pasR_MiddleName !== null) {
                                if (promise.studentDetails[i].pasR_LastName !== null) {
                                    $scope.albumNameArray1.push({ name: promise.studentDetails[i].pasR_FirstName + " " + promise.studentDetails[i].pasR_MiddleName + " " + promise.studentDetails[i].pasR_LastName, pasR_Id: promise.studentDetails[i].pasR_Id });
                                }
                                else {
                                    $scope.albumNameArray1.push({ name: promise.studentDetails[i].pasR_FirstName + " " + promise.studentDetails[i].pasR_MiddleName, pasR_Id: promise.studentDetails[i].pasR_Id });
                                }
                            }
                            else {
                                $scope.albumNameArray1.push({ name: promise.studentDetails[i].pasR_FirstName, pasR_Id: promise.studentDetails[i].pasR_Id });
                            }
                        }
                    }
                }
                $scope.StudentName = $scope.albumNameArray1;
                $scope.edittimehide = false;
                //added by suryan
                $scope.albumNameArray = [];
                if (promise.studenthelthDetails != null && promise.studenthelthDetails != undefined && promise.studenthelthDetails.length > 0) {
                    for (var i = 0; i < promise.studenthelthDetails.length; i++) {
                        if (promise.studenthelthDetails[i].pasR_FirstName != '') {
                            if (promise.studenthelthDetails[i].pasR_MiddleName !== null) {
                                if (promise.studenthelthDetails[i].pasR_LastName !== null) {
                                    $scope.albumNameArray.push({ name: promise.studenthelthDetails[i].pasR_FirstName + " " + promise.studenthelthDetails[i].pasR_MiddleName + " " + promise.studenthelthDetails[i].pasR_LastName, emailid: promise.studenthelthDetails[i].pasR_EMAIL, pashD_Id: promise.studenthelthDetails[i].pashD_Id, pasR_Id: promise.studenthelthDetails[i].pasR_Id });
                                }
                                else {
                                    $scope.albumNameArray.push({ name: promise.studenthelthDetails[i].pasR_FirstName + " " + promise.studenthelthDetails[i].pasR_MiddleName, emailid: promise.studenthelthDetails[i].pasR_EMAIL, pashD_Id: promise.studenthelthDetails[i].pashD_Id, pasR_Id: promise.studenthelthDetails[i].pasR_Id });
                                }
                            }
                            else {
                                $scope.albumNameArray.push({ name: promise.studenthelthDetails[i].pasR_FirstName, emailid: promise.studenthelthDetails[i].pasR_EMAIL, pashD_Id: promise.studenthelthDetails[i].pashD_Id, pasR_Id: promise.studenthelthDetails[i].pasR_Id });
                            }
                        }
                    }
                }
                $scope.studenthelthDetailslist = $scope.albumNameArray;
                $scope.presentCountgrid1 = $scope.albumNameArray.length;
                if ($scope.configurationsettings != null && $scope.configurationsettings != undefined) {
                    if ($scope.configurationsettings.ispaC_ApplFeeFlag === 1) {
                        if ($scope.prospectusPaymentlist.length > 0) {
                            for (var i = 0; i < $scope.studenthelthDetailslist.length; i++) {
                                for (var j = 0; j < $scope.prospectusPaymentlist.length; j++) {
                                    if ($scope.studenthelthDetailslist[i].pasR_Id == $scope.prospectusPaymentlist[j].pasA_Id && $scope.configurationsettings.ispaC_ApplFeeFlag == 1) {
                                        $scope.studenthelthDetailslist[i].viewflag = true;
                                        $scope.studenthelthDetailslist[i].download = false;
                                        break;
                                    }
                                    else if ($scope.studenthelthDetailslist[i].pasR_Id != $scope.prospectusPaymentlist[j].pasA_Id && $scope.configurationsettings.ispaC_ApplFeeFlag == 1) {
                                        $scope.studenthelthDetailslist[i].viewflag = false;
                                        $scope.studenthelthDetailslist[i].download = true;
                                    }
                                }
                            }
                        }
                        else if ($scope.prospectusPaymentlist.length == 0) {
                            for (var i = 0; i < $scope.studenthelthDetailslist.length; i++) {
                                if ($scope.configurationsettings.ispaC_ApplFeeFlag == 1) {
                                    $scope.studenthelthDetailslist[i].viewflag = false;
                                    $scope.studenthelthDetailslist[i].download = true;
                                }
                                else if ($scope.configurationsettings.ispaC_ApplFeeFlag == 0) {
                                    $scope.studenthelthDetailslist[i].viewflag = true;
                                    $scope.studenthelthDetailslist[i].download = false;
                                }
                            }
                        }
                    }
                    else {
                        for (var i = 0; i < $scope.studenthelthDetailslist.length; i++) {
                            if ($scope.configurationsettings.ispaC_ApplFeeFlag == 1) {
                                $scope.studenthelthDetailslist[i].viewflag = false;
                                $scope.studenthelthDetailslist[i].download = true;
                            }
                            else if ($scope.configurationsettings.ispaC_ApplFeeFlag == 0) {
                                $scope.studenthelthDetailslist[i].viewflag = true;
                                $scope.studenthelthDetailslist[i].download = false;
                            }
                        }
                    }
                }
                if (promise.countrole === true) {
                    angular.forEach($scope.pages, function (xy) {
                        xy.viewflagclass = true;
                    });
                }
                else {
                    if ($scope.pages != null && $scope.pages != undefined && $scope.pages.length > 0) {
                        if ($scope.pages.length > 0) {
                            for (var i = 0; i < $scope.pages.length; i++) {
                                for (var j = 0; j < promise.studetailslist.length; j++) {
                                    if ($scope.pages[i].asmcL_Id == promise.studetailslist[j].asmcL_Id) {
                                        $scope.pages[i].viewflagclass = true;
                                        break;
                                    }
                                    else if ($scope.pages[i].asmcL_Id != promise.studetailslist[j].asmcL_Id) {
                                        $scope.pages[i].viewflagclass = false;
                                    }
                                }
                            }
                        }
                    }
                }
            });
        };

        $scope.onSelectarea = function (trmaid) {
            var countryidd = trmaid;
            apiService.getURI("StudentApplication/getroutes", countryidd).then(function (promise) {
                $scope.routelist = promise.routelist;
                $scope.locationlist = promise.locationlist;
                $scope.reg.trmL_Idp = '';
            })
        }

        $scope.onroutechangeload = function (trmaid) {
            var countryidd = trmaid;
            apiService.getURI("StudentApplication/getrouteslocation", countryidd).then(function (promise) {
                $scope.locationlist = promise.locationlist;
            })
        }


        $scope.edithealthcertificate = function (pashD_Id) {
            $scope.PASHD_TyphoidFever = undefined;
            apiService.getURI("StudentApplication/getEdithelthData", pashD_Id).
                then(function (promise) {
                    //added by suryan
                    $scope.albumNameArray1 = [];
                    $scope.albumNameArray2 = [];
                    if (promise.studentDetails != null && promise.studentDetails != undefined && promise.studentDetails.length > 0) {
                        for (var i = 0; i < promise.studentDetails.length; i++) {
                            if (promise.studentDetails[i].pasR_FirstName != '') {
                                if (promise.studentDetails[i].pasR_MiddleName !== null) {
                                    if (promise.studentDetails[i].pasR_LastName !== null) {
                                        $scope.albumNameArray1.push({ name: promise.studentDetails[i].pasR_FirstName + " " + promise.studentDetails[i].pasR_MiddleName + " " + promise.studentDetails[i].pasR_LastName, pasR_Id: promise.studentDetails[i].pasR_Id });
                                    }
                                    else {
                                        $scope.albumNameArray1.push({ name: promise.studentDetails[i].pasR_FirstName + " " + promise.studentDetails[i].pasR_MiddleName, pasR_Id: promise.studentDetails[i].pasR_Id });
                                    }
                                }
                                else {
                                    $scope.albumNameArray1.push({ name: promise.studentDetails[i].pasR_FirstName, pasR_Id: promise.studentDetails[i].pasR_Id });
                                }
                            }
                        }
                    }
                    $scope.StudentName = $scope.albumNameArray1;
                    angular.forEach($scope.StudentName, function (opqr1) {
                        if (opqr1.pasR_Id == promise.studenthelth_DTObj[0].pasR_Id) {
                            opqr1.Selected = true;
                        }
                    })
                    $scope.edittimehide = true;
                    $scope.pasr_id = promise.studenthelth_DTObj[0].pasR_Id;
                    $scope.pashD_Id = promise.studenthelth_DTObj[0].pashD_Id;
                    $scope.PASHD_VaccinationDate = new Date(promise.studenthelth_DTObj[0].pashD_VaccinationDate);
                    $scope.PASHD_FitsFlag = promise.studenthelth_DTObj[0].pashD_FitsFlag;
                    if (promise.studenthelth_DTObj[0].pashD_FitsFlag == 1) {
                        $scope.showhide = true;
                    }
                    else {
                        $scope.showhide = false;
                    }
                    if (promise.studenthelth_DTObj[0].pashD_AllergyFlg == true) {
                        $scope.PASHD_AllergyFlg = "1";
                        $scope.showhideallergy = true;
                    }
                    else {
                        $scope.PASHD_AllergyFlg = "0";
                        $scope.showhideallergy = false;
                    }
                    $scope.PASHD_FitsDate = new Date(promise.studenthelth_DTObj[0].pashD_FitsDate);
                    $scope.PASHD_ChickenpoxFlag = promise.studenthelth_DTObj[0].pashD_ChickenpoxFlag;
                    if (promise.studenthelth_DTObj[0].pashD_ChickenpoxFlag == 1) {
                        $scope.showhidechicken = true;
                    }
                    else {
                        $scope.showhidechicken = false;
                    }
                    $scope.PASHD_ChickenpoxDate = new Date(promise.studenthelth_DTObj[0].pashD_ChickenpoxDate);
                    $scope.PASHD_DiptheriaFlag = promise.studenthelth_DTObj[0].pashD_DiptheriaFlag;
                    if (promise.studenthelth_DTObj[0].pashD_DiptheriaFlag == 1) {
                        $scope.showhidedip = true;
                    }
                    else {
                        $scope.showhidedip = false;
                    }
                    $scope.PASHD_DiptheriaDate = new Date(promise.studenthelth_DTObj[0].pashD_DiptheriaDate);
                    $scope.PASHD_EpidemicFlag = promise.studenthelth_DTObj[0].pashD_EpidemicFlag;
                    if (promise.studenthelth_DTObj[0].pashD_EpidemicFlag == 1) {
                        $scope.showhideepi = true;
                    }
                    else {
                        $scope.showhideepi = false;
                    }
                    $scope.PASHD_EpidemicDate = new Date(promise.studenthelth_DTObj[0].pashD_EpidemicDate);
                    $scope.PASHD_MeaslesFlag = promise.studenthelth_DTObj[0].pashD_MeaslesFlag;
                    if (promise.studenthelth_DTObj[0].pashD_MeaslesFlag == 1) {
                        $scope.showhidemea = true;
                    }
                    else {
                        $scope.showhidemea = false;
                    }
                    $scope.PASHD_MeaslesDate = new Date(promise.studenthelth_DTObj[0].pashD_MeaslesDate);
                    $scope.PASHD_MumpsFlag = promise.studenthelth_DTObj[0].pashD_MumpsFlag;
                    if (promise.studenthelth_DTObj[0].pashD_MumpsFlag == 1) {
                        $scope.showhidemum = true;
                    }
                    else {
                        $scope.showhidemum = false;
                    }
                    $scope.PASHD_MumpsDate = new Date(promise.studenthelth_DTObj[0].pashD_MumpsDate);
                    $scope.PASHD_RingwormFlag = promise.studenthelth_DTObj[0].pashD_RingwormFlag;
                    if (promise.studenthelth_DTObj[0].pashD_RingwormFlag == 1) {
                        $scope.showhiderin = true;
                    }
                    else {
                        $scope.showhiderin = false;
                    }
                    $scope.PASHD_RingwormDate = new Date(promise.studenthelth_DTObj[0].pashD_RingwormDate);
                    $scope.PASHD_ScarletFlag = promise.studenthelth_DTObj[0].pashD_ScarletFlag;
                    if (promise.studenthelth_DTObj[0].pashD_ScarletFlag == 1) {
                        $scope.showhidesca = true;
                    }
                    else {
                        $scope.showhidesca = false;
                    }
                    $scope.PASHD_ScarletDate = new Date(promise.studenthelth_DTObj[0].pashD_ScarletDate);
                    $scope.PASHD_SmallPoxFlag = promise.studenthelth_DTObj[0].pashD_SmallPoxFlag;
                    if (promise.studenthelth_DTObj[0].pashD_SmallPoxFlag == 1) {
                        $scope.showhidesma = true;
                    }
                    else {
                        $scope.showhidesma = false;
                    }
                    $scope.PASHD_SmallPoxDate = new Date(promise.studenthelth_DTObj[0].pashD_SmallPoxDate);
                    $scope.PASHD_WhoopingDate = new Date(promise.studenthelth_DTObj[0].pashD_WhoopingDate);
                    $scope.PASHD_WhoopingFlag = promise.studenthelth_DTObj[0].pashD_WhoopingFlag;
                    if (promise.studenthelth_DTObj[0].pashD_WhoopingFlag == 1) {
                        $scope.showhidewho = true;
                    }
                    else {
                        $scope.showhidewho = false;
                    }
                    $scope.PASHD_Illness = promise.studenthelth_DTObj[0].pashD_Illness;
                    $scope.PASHD_HepatitisB = new Date(promise.studenthelth_DTObj[0].pashD_HepatitisB);
                    if (promise.studenthelth_DTObj[0].pashD_TyphoidFever != null && promise.studenthelth_DTObj[0].pashD_TyphoidFever != "") {
                        $scope.PASHD_TyphoidFever = new Date(promise.studenthelth_DTObj[0].pashD_TyphoidFever);
                    }
                    $scope.PASHD_Ailment = promise.studenthelth_DTObj[0].pashD_Ailment;
                    $scope.PASHD_Allergy = promise.studenthelth_DTObj[0].pashD_Allergy;
                    $scope.PASHD_HealthProblem = promise.studenthelth_DTObj[0].pashD_HealthProblem;
                    $scope.PASHD_CronicDesease = promise.studenthelth_DTObj[0].pashD_CronicDesease;
                    $scope.PASHD_MEContactNo = promise.studenthelth_DTObj[0].pashD_MEContactNo;
                    $scope.PASHD_MEDetails = promise.studenthelth_DTObj[0].pashD_MEDetails;
                    if (promise.vaccines != null) {
                        if (promise.vaccines.length > 0) {
                            $scope.vaccines = promise.vaccines;
                            if (promise.vaccines.length > 0) {
                                angular.forEach($scope.vaccines, function (opqr1) {
                                    if (opqr1.pamvA_YesNoFlg == true) {
                                        opqr1.pamvA_YesNoFlg = "1";
                                    }
                                    else if (opqr1.pamvA_YesNoFlg == false) {
                                        opqr1.pamvA_YesNoFlg = "0";
                                    }
                                })
                            }
                        }
                    }
                    //  $scope.PASHD_BloodGroup = promise.studenthelth_DTObj[0].pashD_BloodGroup;
                    $scope.scroll();
                });
        }

        $scope.deletehealthcertificate = function (id, SweetAlert) {
            swal({
                title: "Are you sure?",
                text: "Do you want to delete record ..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.DeleteURI("StudentApplication/deletehelthdetails", id).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal('Record Deleted Successfully..!');
                                }
                                else {
                                    swal('Record Not Deleted ..!');
                                }
                                $scope.clerehealthcertificate();
                            })
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }

                });
        }
        //$scope.onclickloaddatafits = function () {
        //    if ($scope.PASHD_FitsFlag === '1') {
        //        $scope.showhide = true;
        //    }
        //    else if ($scope.PASHD_FitsFlag === '0') {
        //        $scope.showhide = false;
        //    }

        //}
        $scope.onclickloaddatachicken = function () {
            if ($scope.PASHD_ChickenpoxFlag === '1') {
                $scope.showhidechicken = true;
            }
            else if ($scope.PASHD_ChickenpoxFlag === '0') {
                $scope.showhidechicken = false;
            }
        }

        $scope.onclickloaddataallergy = function () {
            if ($scope.PASHD_AllergyFlg == "1") {
                $scope.showhideallergy = true;
            }
            else if ($scope.PASHD_AllergyFlg === '0') {
                $scope.showhideallergy = false;
            }
        }
        $scope.onclickloaddatadip = function () {
            if ($scope.PASHD_DiptheriaFlag === '1') {
                $scope.showhidedip = true;
            }
            else if ($scope.PASHD_DiptheriaFlag === '0') {
                $scope.showhidedip = false;
            }
        }
        $scope.onclickloaddataepi = function () {
            if ($scope.PASHD_EpidemicFlag === '1') {
                $scope.showhideepi = true;
            }
            else if ($scope.PASHD_EpidemicFlag === '0') {
                $scope.showhideepi = false;
            }
        }
        $scope.onclickloaddatamea = function () {
            if ($scope.PASHD_MeaslesFlag === '1') {
                $scope.showhidemea = true;
            }
            else if ($scope.PASHD_MeaslesFlag === '0') {
                $scope.showhidemea = false;
            }
        }
        $scope.onclickloaddatamum = function () {
            if ($scope.PASHD_MumpsFlag === '1') {
                $scope.showhidemum = true;
            }
            else if ($scope.PASHD_MumpsFlag === '0') {
                $scope.showhidemum = false;
            }
        }
        $scope.onclickloaddatarin = function () {
            if ($scope.PASHD_RingwormFlag === '1') {
                $scope.showhiderin = true;
            }
            else if ($scope.PASHD_RingwormFlag === '0') {
                $scope.showhiderin = false;
            }
        }
        $scope.onclickloaddatasca = function () {
            if ($scope.PASHD_ScarletFlag === '1') {
                $scope.showhidesca = true;
            }
            else if ($scope.PASHD_ScarletFlag === '0') {
                $scope.showhidesca = false;
            }
        }
        $scope.onclickloaddatasma = function () {
            if ($scope.PASHD_SmallPoxFlag === '1') {
                $scope.showhidesma = true;
            }
            else if ($scope.PASHD_SmallPoxFlag === '0') {
                $scope.showhidesma = false;
            }
        }
        $scope.onclickloaddatawho = function () {
            if ($scope.PASHD_WhoopingFlag === '1') {
                $scope.showhidewho = true;
            }
            else if ($scope.PASHD_WhoopingFlag === '0') {
                $scope.showhidewho = false;
            }
        }
        $scope.showprinthelthdata = function (pashD_Id) {
            apiService.getURI("StudentApplication/printgethelthData", pashD_Id).
                then(function (promise) {
                    //added by suryan
                    $scope.albumNameArray1 = [];
                    $scope.albumNameArray2 = [];
                    for (var i = 0; i < promise.studenthelthDTO.length; i++) {
                        if (promise.studenthelthDTO[i].pasR_FirstName != '') {
                            if (promise.studenthelthDTO[i].pasR_MiddleName !== null) {
                                if (promise.studenthelthDTO[i].pasR_LastName !== null) {

                                    $scope.albumNameArray1.push({ name: promise.studenthelthDTO[i].pasR_FirstName + " " + promise.studenthelthDTO[i].pasR_MiddleName + " " + promise.studenthelthDTO[i].pasR_LastName });
                                }
                                else {
                                    $scope.albumNameArray1.push({ name: promise.studenthelthDTO[i].pasR_FirstName + " " + promise.studenthelthDTO[i].pasR_MiddleName });
                                }
                            }
                            else {
                                $scope.albumNameArray1.push({ name: promise.studenthelthDTO[i].pasR_FirstName });
                            }
                        }
                    }
                    $scope.Hstuname = $scope.albumNameArray1[0].name;
                    $scope.Hstuage = promise.studenthelthDTO[0].pasR_Age;
                    $scope.HFirstName = promise.studenthelthDTO[0].pasR_FatherName;
                    $scope.VaccinationDate = promise.studenthelthDTO[0].pashD_VaccinationDate;
                    if (promise.studenthelthDTO[0].pashD_FitsFlag == 1) {
                        $scope.FitsFlag = "Yes";
                        $scope.FitsDate = promise.studenthelthDTO[0].pashD_FitsDate;
                    }
                    else {
                        $scope.FitsFlag = "No";
                        $scope.FitsDate = " ";
                    }
                    if (promise.studenthelthDTO[0].pashD_AllergyFlg == 1) {
                        $scope.AllergyFlag = "Yes";
                        $scope.Allergy = promise.studenthelthDTO[0].pashD_Allergy;
                    }
                    else {
                        $scope.AllergyFlag = "No";
                        $scope.Allergy = " ";
                    }
                    if (promise.studenthelthDTO[0].pashD_ChickenpoxFlag == 1) {
                        $scope.chickenFlag = "Yes";
                        $scope.cihickenDate = promise.studenthelthDTO[0].pashD_ChickenpoxDate;
                    }
                    else {
                        $scope.chickenFlag = "No";
                        $scope.cihickenDate = " ";
                    }
                    if (promise.studenthelthDTO[0].pashD_DiptheriaFlag == 1) {
                        $scope.dipFlag = "Yes";
                        $scope.dipDate = promise.studenthelthDTO[0].pashD_DiptheriaDate;
                    }
                    else {
                        $scope.dipFlag = "No";
                        $scope.dipDate = " ";
                    }
                    if (promise.studenthelthDTO[0].pashD_EpidemicFlag == 1) {
                        $scope.epideFlag = "Yes";
                        $scope.epideDate = promise.studenthelthDTO[0].pashD_EpidemicDate;
                    }
                    else {
                        $scope.epideFlag = "No";
                        $scope.epideDate = " ";
                    }
                    if (promise.studenthelthDTO[0].pashD_MeaslesFlag == 1) {
                        $scope.measleFlag = "Yes";
                        $scope.measleDate = promise.studenthelthDTO[0].pashD_MeaslesDate;
                    }
                    else {
                        $scope.measleFlag = "No";
                        $scope.measleDate = " ";
                    }
                    if (promise.studenthelthDTO[0].pashD_MumpsFlag == 1) {
                        $scope.mumFlag = "Yes";
                        $scope.mumDate = promise.studenthelthDTO[0].pashD_MumpsDate;
                    }
                    else {
                        $scope.mumFlag = "No";
                        $scope.mumDate = " ";
                    }
                    if (promise.studenthelthDTO[0].pashD_RingwormFlag == 1) {
                        $scope.ringFlag = "Yes";
                        $scope.ringDate = promise.studenthelthDTO[0].pashD_RingwormDate;
                    }
                    else {
                        $scope.ringFlag = "No";
                        $scope.ringDate = " ";
                    }
                    if (promise.studenthelthDTO[0].pashD_ScarletFlag == 1) {
                        $scope.scarletFlag = "Yes";
                        $scope.scarletDate = promise.studenthelthDTO[0].pashD_ScarletDate;
                    }
                    else {
                        $scope.scarletFlag = "No";
                        $scope.scarletDate = " ";
                    }
                    if (promise.studenthelthDTO[0].pashD_SmallPoxFlag == 1) {
                        $scope.smallFlag = "Yes";
                        $scope.smallDate = promise.studenthelthDTO[0].pashD_SmallPoxDate;
                    }
                    else {
                        $scope.smallFlag = "No";
                        $scope.smallDate = " ";
                    }
                    if (promise.studenthelthDTO[0].pashD_WhoopingFlag == 1) {
                        $scope.whoFlag = "Yes";
                        $scope.whoDate = promise.studenthelthDTO[0].pashD_WhoopingDate;
                    }
                    else {
                        $scope.whoFlag = "No";
                        $scope.whoDate = " ";
                    }
                    $scope.Illness = promise.studenthelthDTO[0].pashD_Illness;
                    $scope.HepatitisB = new Date(promise.studenthelthDTO[0].pashD_HepatitisB);
                    $scope.TyphoidFever = new Date(promise.studenthelthDTO[0].pashD_TyphoidFever);
                    $scope.Ailment = promise.studenthelthDTO[0].pashD_Ailment;
                    if (promise.studenthelthDTO[0].pashD_Allergy == 1) {
                        $scope.Allergy = "Yes";
                    }
                    else {
                        $scope.Allergy = "No";
                    }
                    $scope.HealthProblem = promise.studenthelthDTO[0].pashD_HealthProblem;
                    $scope.CronicDesease = promise.studenthelthDTO[0].pashD_CronicDesease;
                    $scope.MEDetails = promise.studenthelthDTO[0].pashD_MEDetails;
                    $scope.MEContactNo = promise.studenthelthDTO[0].pashD_MEContactNo;
                    //  $scope.BloodGroup = promise.studenthelthDTO[0].pashD_BloodGroup;
                });
        }
        $scope.printstuhelth = function () {
            var innerContents = document.getElementById("BGHSHealthCertReport").innerHTML;
            var popupWinindow = window.open('');
            popupWinindow.document.open();
            popupWinindow.document.write('<html><head>' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf_Bootstrap.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/hutchings/Hutchingspdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/PrintPdf.css" />' +
                '<link type="text/css" media="print" rel="stylesheet" href="css/print/baldwin/BGHS/AplicationForms/BGHSHealthCertReportPdf.css" />' +
                '</head><body onload="window.print()"  onfocus = "window.setTimeout(function() { window.close(); }, 100);">' + innerContents + '</html>');
            popupWinindow.document.close();
        }
    }
})();