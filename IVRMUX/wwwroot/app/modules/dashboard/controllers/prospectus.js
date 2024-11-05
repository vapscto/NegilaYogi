
(function () {
    'use strict';
    angular
.module('app')
        .controller('ProspectusController', ProspectusController)

    ProspectusController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$window', 'FormSubmitter','$filter','superCache']
    function ProspectusController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $window, FormSubmitter, $filter, superCache) {
        var HostName = location.host;
        //var vm = this;

        //vm.redirectMe = function () {
        //    var url = 'http://requestb.in/z4lvjoz4';
        //    var method = 'POST';
        //    var params = {
        //        param1: 'value1',
        //        param2: 'value2'
        //    };

        //$scope.search = '';
        //$scope.filterOnLocation = function (user) {
        //    return angular.lowercase(user.pasP_First_Name + ' ' + user.pasP_Middle_Name + ' ' + user.pasP_Last_Name).indexOf(angular.lowercase($scope.search)) >= 0 || angular.lowercase(user.pasP_ProspectusNo).indexOf(angular.lowercase($scope.search)) >= 0 || angular.lowercase(user.pasP_Enquiry).indexOf(angular.lowercase($scope.search)) >= 0 || angular.lowercase(user.pasP_EmailId).indexOf(angular.lowercase($scope.search)) >= 0 || JSON.stringify(user.pasP_MobileNo).indexOf($scope.search) >= 0 || ($filter('date')(user.pasP_Date, 'dd-MM-yyyy').indexOf($scope.search) >= 0);
        //};

        var searchObject = $location.search();
        $scope.prospectusno = false;
        $scope.presentCountgrid = 0;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings!=null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.sortKey = "pasP_Id";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa

        $scope.fillpay = function (pasR_Id) {

            $window.location.href = 'http://' + HostName + '/#/app/FeePreadmissionTransaction/';

        }
        $scope.submitted = false;
        $scope.fillpay1 = function (pasR_Id) {

            $window.location.href = 'http://' + HostName + '/#/app/srkvsnew/';

        }
       // swal(searchObject.status);
        if (searchObject.status == "failure")
        {
            swal("Payment Unsuccessfull");
          //  Request.QueryString.Remove("status");
            //$location.url($location.path)
            if ($location.$$search.status)
            {
                delete $location.$$search.status;
                $location.$$compose();
            }
        }
        else if (searchObject.status == "success")
        {
            swal("Payment Successfull");
            //Request.QueryString.Remove("status");
            if ($location.$$search.status) {
                delete $location.$$search.status;
                $location.$$compose();
            }
        }
        else if (searchObject.status == "Networkfailure") {
            swal('Network failure..!!','Try again after some time');
            //Request.QueryString.Remove("status");
            if ($location.$$search.status) {
                delete $location.$$search.status;
                $location.$$compose();
            }
        }

        //var myParam = GetQueryStringByParameter('status');
        //if (myParam != "" && myParam != null)
        //{
        //    swal(myParam);
        //}

        //    FormSubmitter.submit(url, method, params);
        //}

        var configsettings = JSON.parse(localStorage.getItem("configsettings"));
        for (var i = 0; i < configsettings.length; i++) {
            if (configsettings.length > 0) {
                $scope.configurationsettings = configsettings[i];
            }
        }

        $scope.transnumbconfigurationsettings = [];
        $scope.transnumbconfigurationsettingsassign = {};
        var transnumconfigsettings = JSON.parse(localStorage.getItem("transnumconfigsettings"));
        for (var i = 0; i < transnumconfigsettings.length; i++) {
            if (transnumconfigsettings.length > 0) {
                if (transnumconfigsettings[i].imN_Flag == "Prospectus") {
                    $scope.transnumbconfigurationsettingsassign = transnumconfigsettings[i];
                }
            }
        }


        $scope.onclasschange = function (classid, yr, flg) {

          
                var data = {
                    "ASMCL_Id": classid,
                    "ASMAY_Id": yr,
                    "admflag": "adm"
                };

                apiService.create("StudentAdmission/classchangemaxminage", data).then(function (promise) {
                    $scope.electivelist_Groups = promise.electivelist_Groups;
                    $scope.electivelist = promise.electivelist;
                    //if (promise.electivelist_Groups != null && promise.electivelist_Groups.length) {
                    //    $scope.electivelist_Groups = promise.electivelist_Groups;
                    //}
                    //if (promise.electivelist != null && promise.electivelist.length) {
                    //    $scope.electivelist = promise.electivelist;
                    //}
                });
            
        };











        //Date:23-12-2016 for displaying privileges.
        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.userPrivileges = privlgs[i];
            }
        }

        //$scope.PASP_Date = new Date();

        $scope.onlyWeekendsPredicate = function (date) {
            var day = date.getDay();
            return day === 0 || day === 6;
        };

        $scope.getState = function (arrlist) {
            var countryId = $scope.IVRMMC_Id;
            apiService.getURI("Prospectus/getorganisationcontroller", countryId).
        then(function (promise) {
            $scope.arrliststate = promise.stateDrpDown;
        })
        }

        $scope.getCity = function (arrliststate) {
            var StateId = $scope.IVRMMS_Id;
            apiService.getURI("Prospectus/getorganisationstatecontroller", StateId).
        then(function (promise) {
            $scope.arrlistCity = promise.cityDrpDown;
        })
        }

        $scope.Back = function () {
            $scope.PaymentMode = false;
            $scope.ProspectuseScreen = true;
            $state.reload();
        };

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
        $scope.IsHidden = true;
        $scope.ShowHide = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden = $scope.IsHidden ? false : true;
        }

        //$scope.NEXT = function () {
        //    $scope.PaymentMode = false;
        //    $scope.ProspectuseScreen = true;
        //};

        $scope.searchByColumn = function (search, searchColumn) {

            if (search != null || search != undefined && searchColumn != null || searchColumn != undefined) {
                var data = {
                    "EnteredData": search,
                    "SearchColumn": searchColumn,
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }

                apiService.create("Prospectus/SearchByColumn", data)

                .then(function (promise) {

                    if (promise.count == 0) {
                        swal("No Records Found.....!!");
                    }
                    if (promise.message != "" && promise.message != null) {
                        swal(promise.message);
                        $scope.students = promise.prospectuslist;

                        $scope.presentCountgrid = promise.prospectuslist.length;
                    }
                    else {
                        $scope.search = "";
                        $scope.students = promise.prospectuslist;

                        $scope.presentCountgrid = promise.prospectuslist.length;
                    }

                })
            }
            else {

            }
        }

        $scope.prospctusDetls = function () {
            
            $scope.PaymentMode = false;
            $scope.ProspectuseScreen = true;

            var pageid = 2;
            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;

            var inputs = {
                transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign
            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            apiService.create("Prospectus/getalldetails", inputs).
            then(function (promise) {
                
                $scope.prospectusdwnldpath = promise.prospectusfilePath;
                $scope.arrlist = promise.countryDrpDown;
                if ($scope.arrlist != null && $scope.arrlist.length > 0) {
                    angular.forEach($scope.arrlist, function (itm) {
                        
                        if (itm.ivrmmC_Default == 1) {
                            $scope.IVRMMC_Id = itm.ivrmmC_Id;
                        }
                    });
                }
                

                $scope.arrliststate = promise.stateDrpDown;
                if ($scope.arrliststate != null && $scope.arrliststate.length > 0) {
                    angular.forEach($scope.arrliststate, function (itm) {

                        if (itm.ivrmmS_Default == 1) {
                            $scope.IVRMMS_Id = itm.ivrmmS_Id;
                        }
                    });
                }
                // $scope.arrlistCity = promise.cityDrpDown;
                // $scope.arrlist2 = promise.yeardropDown;
                $scope.arrlist4 = promise.classdropDown;
                
                $scope.arrlist5 = promise.referencedropDown;
                $scope.arrlist6 = promise.sourcedropDown;
                $scope.students = promise.prospectuslist;
                $scope.presentCountgrid = promise.prospectuslist.length;
                $scope.prospectusPaymentlist = promise.prospectusPaymentlist;


                if (promise.classdropDown != null && promise.classdropDown.length == 1) {
                    $scope.ASMCL_Id = promise.classdropDown[0].asmcL_Id;
                }
                if (promise.mI_ID === 10) {
                    //$scope.ASMCL_Id =72;
                    $scope.IVRMMC_Id =101;
                    $scope.IVRMMS_Id =17;
                }

                if ($scope.configurationsettings.ispaC_ProspectusFlag === 1) {

                    $scope.ispaC_ProsptFeeApp = $scope.configurationsettings.ispaC_ProsptFeeApp;
                    $scope.prosH = true;
                    $scope.prosL = true;
                }
                else {
                    $scope.prosH = false;
                    $scope.prosL = false;
                }
                if ($scope.ASMAY_Id !== promise.yeardropDown[0].asmaY_Id) {
                    $scope.arrlist2 = promise.yeardropDown;
                    $scope.ASMAY_Id = promise.yeardropDown[0].asmaY_Id;
                }
                

                if (promise.ispaC_EnquiryApplFlag === 1) {
                    $scope.EnquiryScreen = true;
                }
                else if (promise.ispaC_EnquiryApplFlag === 0) {
                    $scope.EnquiryScreen = false;
                }


                $scope.Enquirenorequire = false;
                // Enquiry number autogeneration
                if ($scope.transnumbconfigurationsettingsassign.imN_AutoManualFlag === "Manual") {
                    $scope.EnquiryNo = true;
                    $scope.Enquirenorequire = true;

                }
                else {
                    $scope.EnquiryNo = false;
                }

                swal("Kindly fill both Prospectus and Application form to complete your registration process!!!","Thank You!!");

            })
            $scope.order = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }


          
        }


        $scope.delete = function (employee, SweetAlert) {
            $scope.editEmployee = employee.pasP_Id;
            var orgaid = $scope.editEmployee
            swal({
                title: "Are you sure?",
                text: "Do you want to delete record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it!",
                cancelButtonText: "Cancel!!!!!!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
            function (isConfirm) {
                if (isConfirm) {

                    var inputs = {
                        "PASP_Id": orgaid,
                        transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }

                    apiService.create("Prospectus/deletedetails", inputs).
                    then(function (promise) {
                        $scope.students = promise.prospectuslist;
                        $scope.presentCountgrid = promise.prospectuslist.length;
                        if (promise.returnval === true) {
                            swal('Record Deleted Successfully');
                            $state.reload();
                        }
                        else {
                            swal('Record Deleted Successfully');
                            $state.reload();
                        }
                    });
                }
                else {
                    swal("Record Deletion Cancelled");
                }
            });
        }
        $scope.edit = function (employee) {
            $scope.editEmployee = employee.pasP_Id;
            var propsId = $scope.editEmployee;
            //orgid = 12;
            apiService.getURI("Prospectus/getdetails", propsId).
            then(function (promise) {
                $scope.IVRMMC_Id = promise.prospectuslist[0].IVRMMC_Id;
                // getStateByCountry(promise.prospectuslist[0].ivrmmC_Id, promise.prospectuslist[0].ivrmmS_Id);
                $scope.IVRMMS_Id = promise.prospectuslist[0].IVRMMS_Id;
                //getCityByState(promise.prospectuslist[0].ivrmmS_Id, promise.prospectuslist[0].ivrmmcT_Id);
                $scope.IVRMMCT_Id = promise.prospectuslist[0].IVRMMCT_Id;
                // $scope.ASMAY_Id = promise.prospectuslist[0].asmaY_Id;
                $scope.ASMCL_Id = promise.prospectuslist[0].ASMCL_Id;
                $scope.PAMR_Id = promise.prospectuslist[0].PAMR_Id;
                $scope.PAMS_Id = promise.prospectuslist[0].PAMS_Id;
                $scope.PASP_First_Name = promise.prospectuslist[0].PASP_First_Name;
                $scope.PASP_Middle_Name = promise.prospectuslist[0].PASP_Middle_Name;
                $scope.PASP_Last_Name = promise.prospectuslist[0].PASP_Last_Name;
                $scope.PASP_Pincode = promise.prospectuslist[0].PASP_Pincode;
                // $scope.PASP_Date = new Date(promise.prospectuslist[0].pasP_Date);
                $scope.PASP_MobileNo = promise.prospectuslist[0].PASP_MobileNo;

                $scope.PASP_PhoneNo = promise.prospectuslist[0].PASP_PhoneNo;
                $scope.PASP_EmailId = promise.prospectuslist[0].PASP_EmailId;
                $scope.PASP_HouseNo = promise.prospectuslist[0].PASP_HouseNo;
                $scope.PASP_Street = promise.prospectuslist[0].PASP_Street;
                $scope.PASP_Area = promise.prospectuslist[0].PASP_Area;
                $scope.PASP_Enquiry = promise.prospectuslist[0].PASP_Enquiry;
                $scope.PASP_ProspectusNo = promise.prospectuslist[0].PASP_ProspectusNo;
                $scope.PASP_Id = promise.prospectuslist[0].PASP_Id;
                
                $scope.prospectusno = true;
                if (promise.activeflag === 0) {
                    //$scope.activeflag = function () {
                    //    $scope.activeflag.checked = true;
                    //};
                    $scope.activeflag = true;
                }
                else {
                    //$scope.activeflag = function () {
                    //    $scope.activeflag.checked = false;
                    //};
                    $scope.activeflag = false;
                }
                $scope.PASP_DateOfBirth = new Date(promise.prospectuslist[0].PASP_DateOfBirth);
                $scope.PASP_FatherName = promise.prospectuslist[0].PASP_FatherName;;
            })
        }
        $scope.clearid = function () {
            //$scope.submitted = false;
            //$scope.IVRMMCT_Id = "";
            //$scope.IVRMMC_Id = "";
            //$scope.IVRMMS_Id = "";
            //$scope.ASMCL_Id = "";
            //$scope.PAMR_Id = "";
            //$scope.PAMS_Id = "";
            //$scope.PASP_First_Name = "";
            //$scope.PASP_Middle_Name = "";
            //$scope.PASP_Last_Name = "";
            //$scope.PASP_Pincode = "";
            ////$scope.PASP_Date = "";
            //$scope.PASP_MobileNo = "";
            //$scope.PASP_PhoneNo = "";
            //$scope.PASP_EmailId = "";
            //$scope.PASP_HouseNo = "";
            //$scope.PASP_Street = "";
            //$scope.PASP_Area = "";
            //$scope.PASP_Enquiry = "";
            //$scope.PASP_Id = 0;

            $state.reload();
        }

        $scope.submitted = false;
        $scope.searchEnq = function () {
            var inputs = {
                "id": $scope.result,
                "enquiryChoice": $scope.searchinput

            }
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }
            apiService.create("Prospectus/getEnquiry", inputs).
             then(function (promise) {

                 //$scope.arrlist2 = promise.yearDrpDwn;

                 if (promise.enquiryList != null) {
                     $scope.IVRMMC_Id = promise.enquiryList[0].ivrmmC_Id;
                     $scope.IVRMMS_Id = promise.enquiryList[0].pasE_State;
                     $scope.IVRMMCT_Id = promise.enquiryList[0].pasE_City;

                     $scope.ASMCL_Id = promise.enquiryList[0].asmcL_Id;
                     // $scope.PAMR_Id = promise.enquiryList[0].pamR_Id;
                     // $scope.PAMS_Id = promise.enquiryList[0].pamS_Id;
                     $scope.PASP_First_Name = promise.enquiryList[0].pasE_FirstName;
                     $scope.PASP_Middle_Name = promise.enquiryList[0].pasE_MiddleName;
                     $scope.PASP_Last_Name = promise.enquiryList[0].pasE_LastName;
                     $scope.PASP_Pincode = promise.enquiryList[0].pasE_Pincode;
                     //$scope.PASP_Date = new Date(promise.enquiryList[0].pasE_Date);
                     $scope.PASP_MobileNo = promise.enquiryList[0].pasE_MobileNo;
                     $scope.PASP_PhoneNo = promise.enquiryList[0].pasE_Phone;
                     $scope.PASP_EmailId = promise.enquiryList[0].pasE_emailid;
                     $scope.PASP_HouseNo = promise.enquiryList[0].pasE_Address1;
                     $scope.PASP_Street = promise.enquiryList[0].pasE_Address2;
                     $scope.PASP_Area = promise.enquiryList[0].pasE_Address3;
                     //  $scope.PASP_Enquiry = promise.enquiryList[0].pasP_Enquiry;
                     $scope.PASP_Id = 0;
                 }
                 else {
                     $scope.PASP_Id = 0;

                     $state.reload();

                 }
             });
        }

        function getCityByState(stateId, cityId) {

            apiService.getURI("Prospectus/getorganisationstatecontroller", stateId).
            then(function (promise) {
                $scope.arrlistCity = [{ "ivrmmcT_Id": 0, "ivrmmcT_Name": "--Select--" }];
                var ct = Number(cityId);
                $scope.IVRMMCT_Id = ct;
                $scope.citydata = promise.cityDrpDown;
                $scope.arrlistCity.push.apply($scope.arrlistCity, $scope.citydata);
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
                "surl": "http://localhost:57606/api/Prospectus/paymentresponse/",
                "furl": "http://localhost:57606/api/Prospectus/paymentresponse/"
            }
            FormSubmitter.submit(url, method, params);
        }

        $scope.submitted = false;
        $scope.savestudentdata = function () {
            
            if ($scope.PASP_PhoneNo == null || $scope.PASP_PhoneNo == "")
            {
                $scope.PASP_PhoneNo = 0;
            }
            if ($scope.PAMR_Id == null || $scope.PAMR_Id == "") {
                $scope.PAMR_Id = 2;
            }

            if ($scope.PAMS_Id == null || $scope.PAMS_Id == "") {
                $scope.PAMS_Id = 1;
            }
            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var data = {
                    "IVRMMCT_Id": $scope.IVRMMCT_Id,
                    "IVRMMC_Id": $scope.IVRMMC_Id,
                    "IVRMMS_Id": $scope.IVRMMS_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ASMCL_Id": $scope.ASMCL_Id,
                    "PAMR_Id": $scope.PAMR_Id,
                    "PAMS_Id": $scope.PAMS_Id,
                    "PASP_First_Name": $scope.PASP_First_Name,
                    "PASP_Middle_Name": $scope.PASP_Middle_Name,
                    "PASP_Last_Name": $scope.PASP_Last_Name,
                    "PASP_Pincode": $scope.PASP_Pincode,
                    "PASP_ProspectusNo": $scope.PASP_ProspectusNo,
                    // "PASP_Date": $scope.PASP_Date,
                    "PASP_MobileNo": $scope.PASP_MobileNo,
                    "PASP_PhoneNo": $scope.PASP_PhoneNo,
                    "PASP_EmailId": $scope.PASP_EmailId,
                    "PASP_HouseNo": $scope.PASP_HouseNo,
                    "PASP_Street": $scope.PASP_Street,
                    "PASP_Area": $scope.PASP_Area,
                    "PASP_Enquiry": $scope.PASP_Enquiry,
                    "PASP_Id": $scope.PASP_Id,
                    "returnval": $scope.PASP_FatherName, //Ftaher name (returnval)
                    "PASP_Date": new Date($scope.PASP_DateOfBirth).toDateString(), // date Of Birth (PASP_Date)
                    configurationsettings: $scope.configurationsettings,
                    transnumbconfigurationsettingsss: $scope.transnumbconfigurationsettingsassign,
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("Prospectus/", data)
                .then(function (promise) {
                    if (promise.returnval === "mobileDuplicate") {
                        swal('Mobile number already Exist..!');
                        return;
                    } else if (promise.returnval === "phoneDuplicate") {
                        swal('Phone number already Exist..!');
                        return;
                    } else
                        if (promise.returnval === "emailDuplicate") {
                            swal('Email Id already Exist..!');
                            return;
                        }
                        else
                            if (promise.returnval === "ProspectusNoDuplicate") {
                                swal('Prospectus No already Exist..!');
                                return;
                            }
                        else if (promise.returnval === "true") {
                                swal("Record saved/updated Successfully", "Kindly fill Application form to complete your registration process!!");
                                //$state.reload();
                                $window.location.href = 'http://' + HostName + '/#/app/srkvsnew/';
                            }
                            // $window.location.href = 'http://test.payu.in/_payment';

                            // document.myForm.action = 'http://test.payu.in/_payment';

                            //$scope.pay_data = [];
                            //$scope.pay_data.push(promise.payment_details)
                            //  $scope.payment_details = promise.payment_details;
                            // $state.reload();
                
                        else if (promise.returnval === "false") {
                            swal('Record Not Saved/Updated Successfully', 'Failed');
                            $state.reload();
                        }
                })


            };

        }

        $scope.download = function () {
            $http.get('api/Prospectus/download/1', { responseType: 'arraybuffer' })
        .success(function (data) {
            var file = new Blob([data], { type: 'application/pdf', filename: 'Prospectors.pdf' });
            var fileURL = URL.createObjectURL(file);
            window.open(fileURL);
        });

        }
        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
    }
})();

