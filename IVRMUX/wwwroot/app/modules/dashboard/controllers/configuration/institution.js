(function () {
    'use strict';
    angular.module('app').controller('institutionController', institutionController)

    institutionController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache']
    function institutionController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache) {

        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.userPrivileges = privlgs[i];
            }
        }

        $scope.disableinstname = false;
        var HostName = location.host;
        $scope.disinstname = false;
        $scope.Previous = function () {
            $window.location.href = 'http://' + HostName + '/#/app/organization/';
        };

        $scope.Next = function () {
            $window.location.href = 'http://' + HostName + '/#/app/MasterMainMenuINS/';
        };

        $scope.Finish = function () {
            $window.location.href = 'http://' + HostName + '/#/app/ConSettingWizardComplete/';
        };

        $scope.paginate1 = "paginate1";
        $scope.paginate2 = "paginate2";

        //options

        $scope.options = ['Default', 'Office', 'Home'];
        $scope.options1 = ['Default', 'Office', 'Home'];
        $scope.options2 = ['Default', 'Office', 'Home'];

        $scope.disabled = {};
        $scope.disabled1 = {};
        $scope.disabled2 = {};


        //colorpic
        $scope.hexPicker = {
            color: ''
        };

        $scope.rgbPicker = {
            color: ''
        };
        $scope.rgbaPicker = {
            color: ''
        };
        $scope.nonInput = {
            color: ''
        };
        $scope.resetColor = function () {
            $scope.hexPicker = {
                color: '#ff0000'
            };
        };

        $scope.resetRBGColor = function () {
            $scope.rgbPicker = {
                color: 'rgb(255,255,255)'
            };
        };

        $scope.resetRBGAColor = function () {
            $scope.rgbaPicker = {
                color: 'rgb(255,255,255, 0.25)'
            };
        };

        $scope.resetNonInputColor = function () {
            $scope.nonInput = {
                color: '#ffffff'
            };
        };


        //phones add option
        $scope.phones = [{ id: 'phone1' }];
        $scope.selphs = [{ id: 'selphone1' }];

        $scope.addNewPhone = function (curval) {
            var newItemNo = $scope.phones.length + 1;
            if (newItemNo <= 3) {
                $scope.phones.push({ 'id': 'phone' + newItemNo });
            }

            var newItemNo1 = $scope.selphs.length + 1;
            if (newItemNo1 <= 3) {
                $scope.selphs.push({ 'id': 'selphone' + newItemNo1 });
            }
        };

        $scope.defchange = function (curval) {
            var newItemNo1 = $scope.selphs.length;
            for (var i = 0; i < newItemNo1; i++) {
                if ($scope.selphs[i].curval == "Default") {
                    $scope.disabled["Default"] = true;
                    break;
                }
                else {
                    $scope.disabled["Default"] = false;
                }
            }
        }

        $scope.del = [];
        $scope.removeNewPhone = function (index, curval) {
            var newItemNo = $scope.phones.length - 1;
            if (newItemNo !== 0) {
                $scope.phones.splice(index, 1);
            }
            var newItemNo1 = $scope.selphs.length - 1;
            if (newItemNo1 !== 0) {
                $scope.del = $scope.selphs.splice(index, 1);
                if ($scope.del[0].curval == "Default") {
                    $scope.disabled["Default"] = false;
                }
                else {
                    $scope.disabled["Default"] = true;
                }
            }

        };

        //
        //mobile add option
        $scope.mobiles = [{ id: 'mobile1' }];
        $scope.selmobs = [{ id: 'selmobile1' }];

        $scope.addNewMobile = function () {
            var newItemNo = $scope.mobiles.length + 1;
            if (newItemNo <= 3) {
                $scope.mobiles.push({ 'id': 'mobile' + newItemNo });
            }
            var newItemNo1 = $scope.selmobs.length + 1;
            if (newItemNo1 <= 3) {
                $scope.selmobs.push({ 'id': 'selmobile' + newItemNo1 });
            }

        };
        $scope.delm = [];
        $scope.removeNewMobile = function (index, curval1) {
            var newItemNo = $scope.mobiles.length - 1;
            if (newItemNo !== 0) {
                $scope.mobiles.splice(index, 1);
            }
            var newItemNo1 = $scope.selmobs.length - 1;
            if (newItemNo1 !== 0) {
                $scope.delm = $scope.selmobs.splice(index, 1);
                if ($scope.delm[0].curval1 == "Default") {
                    $scope.disabled["Default"] = false;
                }
                else {
                    $scope.disabled["Default"] = true;
                }
            }
        };
        $scope.defchangemob = function (curval1) {
            var newItemNo1 = $scope.selmobs.length;
            for (var i = 0; i < newItemNo1; i++) {
                if ($scope.selmobs[i].curval1 == "Default") {
                    $scope.disabled["Default"] = true;
                    break;
                }
                else {
                    $scope.disabled["Default"] = false;
                }
            }
        }

        //email option
        $scope.emails = [{ id: 'email1' }];
        $scope.addNewEmail = function () {
            var newItemNo = $scope.emails.length + 1;
            if (newItemNo <= 3) {
                $scope.emails.push({ 'id': 'email' + newItemNo });
            }
        };
        $scope.removeNewEmail = function () {
            var newItemNo = $scope.emails.length - 1;
            if (newItemNo !== 0) {
                $scope.emails.pop();
            }
        };
        $scope.showAddEmail = function (email) {
            return email.id === $scope.emails[$scope.emails.length - 1].id;
        };



        //sms credit alert email option
        $scope.emails1 = [{ id: 'email1' }];
        $scope.addNewEmail1 = function () {
            var newItemNo = $scope.emails1.length + 1;
            if (newItemNo <= 11) {
                $scope.emails1.push({ 'id': 'email' + newItemNo });
            }
        };
        $scope.removeNewEmail1 = function () {
            var newItemNo = $scope.emails1.length - 1;
            if (newItemNo !== 0) {
                $scope.emails1.pop();
            }
        };
        $scope.showAddEmail1 = function (email) {
            return email.id === $scope.emails1[$scope.emails1.length - 1].id;
        };

        $scope.Sub = {};
        $scope.subscription = {};

        $scope.FromDate = new Date();
        $scope.minDatef = new Date(
            $scope.FromDate.getFullYear(),
            $scope.FromDate.getMonth(),
            $scope.FromDate.getDate());

        $scope.InstitutionSortPagInfo = {};
        $scope.SubscriptionSortPagInfo = {};

        $scope.todate = true;

        $scope.setTodate = function (data) {
            $scope.todate = false;
            console.log(data);
            $scope.ToDate = data;
            $scope.minDate = new Date(
                $scope.ToDate.getFullYear(),
                $scope.ToDate.getMonth(),
                $scope.ToDate.getDate());

        }


        $scope.OT = '';
        $scope.ST = '';
        $scope.diplayteams = false;
        $scope.on_pic_route_change = function () {
            debugger;
            $scope.OT = '';
            $scope.ST = '';
            $scope.diplayteams = false;
            angular.forEach($scope.vcdetails, function (xx) {
                if (xx.mvidcoN_Id == $scope.MI_VCOthersFlag) {
                    $scope.OT = xx.mvidcoN_VCCode
                }

                if (xx.mvidcoN_Id == $scope.MI_VCStudentFlag) {
                    $scope.ST = xx.mvidcoN_VCCode
                }

            })

            if ($scope.OT === 'TEAMS' || $scope.ST === 'TEAMS') {
                $scope.diplayteams = true;
            }

        }

        $scope.institutList = function () {
            $scope.reverse = true;
            $scope.reverse1 = true;

            var data = {
                "instutePagination": $scope.InstitutionSortPagInfo,
                "subscriptionPagination": $scope.SubscriptionSortPagInfo
            }

            apiService.create("Institution/getalldetails", data).then(function (promise) {
                $scope.arrlistcoun = promise.countryDrpDown;
                $scope.arrliststate = promise.stateDrpDown;
                // $scope.arrlist2 = promise.cityDrpDown;
                $scope.trustList = promise.trustDropdown;

                $scope.instutedropdown = promise.instutedropdown;
                $scope.vcdetails = promise.vcdetails;
                $scope.vcdetails1 = promise.vcdetails;

                angular.forEach($scope.vcdetails1, function (zz) {
                    zz.mvidconinT_HostedURL = '';

                })

                // for institution pagination 
                //$scope.currentPage = promise.instutePagination.currentPageIndex;
                //$scope.itemsPerPage = promise.instutePagination.pageSize;
                //$scope.totalitems = promise.instutePagination.totalItems;

                $scope.currentPage = 1;
                $scope.itemsPerPage = 10;
                // $scope.totalitems = promise.instutePagination.totalItems;


                $scope.institutes = promise.institutionname;
         //       $scope.IVRM_OTP_ADMNO = promise.ivrM_OTP_ADMNO;
                // for  institution pagination


                // for subscription pagination 
                //$scope.currentPage1 = promise.subscriptionPagination.currentPageIndex;
                //$scope.itemsPerPage1 = promise.subscriptionPagination.pageSize;
                //$scope.totalitems1 = promise.subscriptionPagination.totalItems;


                $scope.currentPage1 = 1;
                $scope.itemsPerPage1 = 10;
                //  $scope.totalitems1 = promise.subscriptionPagination.totalItems;

                $scope.subscriptionlist = promise.subscriptionlist;
                // for  subscription pagination



                $scope.mandatory = promise.mandatoryList;
                var b = document.querySelector("#countryId");
                var c = document.querySelector("#stateId");
                var ct = document.querySelector("#cityId");
                var tr = document.querySelector("#trust");
                var ins = document.querySelector("#institution");
                var ty = document.querySelector("#instype");
                var em = document.querySelector("#email");
                var address = document.querySelector("#address");
                var pincode = document.querySelector("#pincode");
                var add2 = document.querySelector("#add2");
                var add3 = document.querySelector("#add3");
                var fax = document.querySelector("#fax");
                var phone = document.querySelector("#phone");
                var mobile = document.querySelector("#mobile");
                var formcolor = document.querySelector("#formcolor");
                var fontcolor = document.querySelector("#fontcolor");
                var FontSize = document.querySelector("#FontSize");
                var WeekstartDay = document.querySelector("#WeekstartDay");
                var dateformat = document.querySelector("#dateformat");
                var dateseparator = document.querySelector("#dateseparator");
                var grade = document.querySelector("#grade");
                var img = document.querySelector("#img");
                var logo = document.querySelector("#img1");

                angular.forEach($scope.mandatory, function (value, index) {
                    if (value.ivrM_MAND_FIELDS == "State") {
                        c.setAttribute("required", "required");
                    }
                    if (value.ivrM_MAND_FIELDS == "Country") {
                        b.setAttribute("required", "required");
                    }
                    if (value.ivrM_MAND_FIELDS == "Institution Name") {
                        ins.setAttribute("required", "required");
                    }
                    if (value.ivrM_MAND_FIELDS == "Trust Name") {
                        tr.setAttribute("required", "required");
                    }
                    if (value.ivrM_MAND_FIELDS == "Instituion Type") {
                        ty.setAttribute("required", "required");
                    }
                    if (value.ivrM_MAND_FIELDS == "email") {
                        em.setAttribute("required", "required");
                    }
                    if (value.ivrM_MAND_FIELDS == "Address1") {
                        address.setAttribute("required", "required");
                    }
                    if (value.ivrM_MAND_FIELDS == "Pincode") {
                        pincode.setAttribute("required", "required");
                    }
                    if (value.ivrM_MAND_FIELDS == "City") {
                        ct.setAttribute("required", "required");
                    }
                    if (value.ivrM_MAND_FIELDS == "Address2") {
                        add2.setAttribute("required", "required");
                    }
                    if (value.ivrM_MAND_FIELDS == "Address3") {
                        add3.setAttribute("required", "required");
                    }
                    if (value.ivrM_MAND_FIELDS == "Fax") {
                        fax.setAttribute("required", "required");
                    }
                    if (value.ivrM_MAND_FIELDS == "Phone No") {
                        phone.setAttribute("required", "required");
                    }
                    if (value.ivrM_MAND_FIELDS == "Mobile Number") {
                        mobile.setAttribute("required", "required");
                    }
                    if (value.ivrM_MAND_FIELDS == "Form Color") {
                        formcolor.setAttribute("required", "required");
                    }
                    if (value.ivrM_MAND_FIELDS == "Font Color") {
                        fontcolor.setAttribute("required", "required");
                    }
                    if (value.ivrM_MAND_FIELDS == "Font Size") {
                        FontSize.setAttribute("required", "required");
                    }
                    if (value.ivrM_MAND_FIELDS == "Week start Day") {
                        WeekstartDay.setAttribute("required", "required");
                    }
                    if (value.ivrM_MAND_FIELDS == "Date Format") {
                        dateformat.setAttribute("required", "required");
                    }
                    if (value.ivrM_MAND_FIELDS == "Date Separator") {
                        dateseparator.setAttribute("required", "required");
                    }
                    if (value.ivrM_MAND_FIELDS == "Grading System") {
                        grade.setAttribute("required", "required");
                    }

                })
            })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }

        }

        //edit record
        $scope.edit = function (id) {
            $scope.disableinstname = true;
            $scope.mobiles = [{ id: 'mobile1' }];
            $scope.phones = [{ id: 'phone1' }];
            $scope.emails = [{ id: 'email1' }];
            $scope.emails1 = [];
            $('#Background').attr('src', "");
            $scope.MI_BackgroundImage = "";

            $('#Logo').attr('src', "");
            $scope.MI_Logo = "";
            // swal(id);
            apiService.getURI("Institution/getInstituteById", id).
                then(function (promise) {

                    var stateid = "";
                    var city = "";
                    $('#Background').attr('src', promise.mI_BackgroundImage);
                    $scope.MI_BackgroundImage = promise.mI_BackgroundImage;

                    $('#Logo').attr('src', promise.mI_Logo);
                    $scope.MI_Logo = promise.mI_Logo;
                    angular.forEach(promise.institutionname, function (value, key) {

                        if (value.mI_Id !== 0) {
                            $scope.MI_Id = value.mI_Id;

                            //  $scope.Sub.MI_Id = value.mI_Id;

                            $scope.MO_Id = value.mO_Id;
                            $scope.MI_Name = value.mI_Name;

                            $scope.MI_Affiliation = value.mI_Affiliation,
                                $scope.MI_Address1 = value.mI_Address1;
                            $scope.MI_Address2 = value.mI_Address2;
                            $scope.MI_Address3 = value.mI_Address3;
                            $scope.pin = value.mI_Pincode;
                            $scope.fax = value.mI_FaxNo;
                            $scope.rgbPicker.color = value.mI_FormColor;
                            $scope.rgbaPicker.color = value.mI_FontColor;
                            $scope.MI_FontSize = value.mI_FontSize;
                            $scope.MI_WeekStartDay = value.mI_WeekStartDay;
                            $scope.MI_DateFormat = value.mI_DateFormat;
                            $scope.MI_DateSeparator = value.mI_DateSeparator;
                            $scope.MI_GradingSystem = value.mI_GradingSystem;

                            $scope.MI_Subdomain = value.mI_Subdomain;

                            $scope.MI_PAN = value.mI_PAN;
                            $scope.MI_PAN = value.mI_PAN;


                            $scope.MI_SMSCountAlert = value.mI_SMSCountAlert;
                            $scope.MI_NAAC_InstitutionTypeFlg = value.mI_NAAC_InstitutionTypeFlg;
                            $scope.MI_NAAC_SubInstitutionTypeFlg = value.mI_NAAC_SubInstitutionTypeFlg;

                            $scope.MI_MSTeamsClientId = value.mI_MSTeamsClientId;
                            $scope.MI_MSTeamsTenentId = value.mI_MSTeamsTenentId;
                            $scope.MI_MSTemasClinetSecretCode = value.mI_MSTemasClinetSecretCode;
                            $scope.MI_MSTeamsAppAccessTockenURL = value.mI_MSTeamsAppAccessTockenURL;
                            $scope.MI_MSTeamsUserAceessTockenURL = value.mI_MSTeamsUserAceessTockenURL;
                            $scope.MI_MSTeamsMeetingScheduleURL = value.mI_MSTeamsMeetingScheduleURL;
                            $scope.MI_MSTeamsGrantType = value.mI_MSTeamsGrantType;
                            $scope.MI_MSTeamsScope = value.mI_MSTeamsScope;
                            $scope.MI_80GRegNo = value.mI_80GRegNo;
                            $scope.MI_MSTeamsAdminUsername = value.mI_MSTeamsAdminUsername;
                            $scope.MI_MSTeamsAdminPassword = value.mI_MSTeamsAdminPassword;
                            $scope.MI_EntityId = value.mI_EntityId;
                            $scope.IVRM_OTP_ADMNO = promise.ivrM_OTP_ADMNO;

                            $scope.OT = '';
                            $scope.ST = '';
                            $scope.diplayteams = false;
                            angular.forEach($scope.vcdetails, function (xx) {
                                if (xx.mvidcoN_VCCode == value.mI_VCOthersFlag) {

                                    $scope.MI_VCOthersFlag = xx.mvidcoN_Id;
                                    $scope.OT = xx.mvidcoN_VCCode
                                }

                                if (xx.mvidcoN_VCCode == value.mI_VCStudentFlag) {

                                    $scope.MI_VCStudentFlag = xx.mvidcoN_Id;
                                    $scope.ST = xx.mvidcoN_VCCode
                                }

                            })





                            if ($scope.OT === 'TEAMS' || $scope.ST === 'TEAMS') {
                                $scope.diplayteams = true;
                            }

                            if (value.mI_FranchiseFlag == 1) {
                                $scope.Franchiseflag = true;
                            }
                            else {
                                $scope.Franchiseflag = false;
                            }

                            if (value.mI_ActiveFlag === 0) {
                                $scope.MI_ActiveFlag = false;
                            }
                            else {
                                $scope.MI_ActiveFlag = true;
                            }
                            $scope.MI_AboutInstitute = value.mI_AboutInstitute;
                            $scope.MI_ContactDetails = value.mI_ContactDetails;
                            $scope.MI_SchoolCollegeFlag = value.mI_SchoolCollegeFlag;
                            $scope.IVRMMC_Id = value.ivrmmC_Id;
                            getSelectGetState($scope.IVRMMC_Id, value.ivrmmS_Id);
                            $scope.IVRMMS_Id = value.ivrmmS_Id;
                            //  getSelectGetCity($scope.IVRMMS_Id, value.ivrmmcT_Id);
                            $scope.IVRMMCT_Name = value.ivrmmcT_Name;
                            $scope.editTrusttype(value.mO_Id, value.mI_Type);
                            //     $scope.MI_Type = value.mI_Type;
                            //  $scope.forminstype = true;
                        }
                    });

                    angular.forEach($scope.vcdetails1, function (ee) {

                        angular.forEach(promise.vcdetails, function (ff) {

                            if (ff.mvidcoN_Id == ee.mvidcoN_Id) {
                                ee.checkedvalue = true;
                                ee.mvidconinT_HostedURL = ff.mvidconinT_HostedURL;
                            }
                        });
                    });



                    if (promise.phonearrayList.length > 0) {
                        $scope.phones = promise.phonearrayList;
                    }
                    if (promise.mobilearrayList.length > 0) {
                        $scope.mobiles = promise.mobilearrayList;
                    }
                    if (promise.emailarrayList.length > 0) {
                        $scope.emails = promise.emailarrayList;
                    }


                    if (promise.emailsalert != null && promise.emailsalert.length > 0) {


                        angular.forEach(promise.emailsalert, function (ee) {

                            $scope.emails1.push({ mI_SMSAlertToemailids: ee });

                        });


                    }
                    else {
                        $scope.emails1 = [{ id: 'email1' }];
                    }

                });

        }
        //function getSelect
        // Get state by country
        $scope.onSelectGetState = function (arrlist) {
            var countryidd = $scope.IVRMMC_Id;
            apiService.getURI("StudentApplication/getdpstate", countryidd).then(function (promise) {
                $scope.arrliststate = promise.stateDrpDown;
            })
        }

        //// Get city by state
        //$scope.onSelectGetCity = function (IVRMMS_Id) {
        //    var stateId = IVRMMS_Id;
        //    apiService.getURI("StudentApplication/getdpcities", stateId).then(function (promise) {
        //        $scope.arrlist2 = promise.cityDrpDown;
        //    })
        //}

        //function getSelectGetCity(stateid, cityId) {

        function getSelectGetState(countryidd, stateid) {
            apiService.getURI("StudentApplication/getdpstate", countryidd).then(function (promise) {
                $scope.arrliststate = [{ "ivrmmS_Id": 0, "ivrmmS_Name": "Select State" }];
                var sts = Number(stateid);
                $scope.IVRMMS_Id = sts;

                $scope.data = promise.stateDrpDown;
                $scope.arrliststate.push.apply($scope.arrliststate, $scope.data);
            })
        }

        //delete record
        $scope.delete = function (data, SweetAlert) {

            var mgs = "";
            if (data.mI_ActiveFlag == 0) {
                mgs = "Activate";

            }
            else {
                mgs = "Deactivate";
            }

            // swal(id);
            swal({
                title: "Are you sure?",
                text: "Do you want to " + mgs + " Institute ..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.DeleteURI("Institution/deletedetails", data.mI_Id).
                            then(function (promise) {
                                $scope.institutes = promise.institutionname;
                                $scope.instutedropdown = promise.institutionname;
                                if (promise.returnval === "true") {
                                    swal('Institution Deactivated Successfully..!', 'success');
                                }
                                else {
                                    swal('Institution Activated Successfully..!', 'success');
                                }
                            })
                    }
                    else {
                        swal("Cancelled", "Ok");
                    }
                });
        }

        //clear record
        $scope.cleardata = function () {

            $state.reload();
            //$scope.submitted = false;
            //$scope.MI_Id = "";
            //$scope.MO_Id = "";
            //$scope.MI_Name = "";
            //$scope.MI_Type = "";
            //$scope.MI_Affiliation="";
            //$scope.MI_Address1 = "";
            //$scope.MI_Address2 = "";
            //$scope.MI_Address3 = "";
            //$scope.IVRMMCT_Name = "";
            //$scope.IVRMMS_Id = "";
            //$scope.IVRMMC_Id = "";
            //$scope.pin = "";
            //$scope.fax = "";
            //$scope.rgbPicker.color = "";
            //$scope.rgbaPicker.color = "";
            //$scope.MI_FontSize = "";
            //$scope.MI_WeekStartDay = "";
            //$scope.MI_DateFormat = "";
            //$scope.MI_DateSeparator = "";
            //$scope.MI_GradingSystem = "";
            //$scope.MI_BackgroundImage = "";
            //$scope.MI_Logo = "";
            //$scope.mobiles = [{ id: 'mobile1' }];
            //$scope.phones = [{ id: 'phone1' }];
            //$scope.emails = [{ id: 'email1' }];

            //$('#Background').attr('src',"");
            //$('#Logo').attr('src',"");
        }

        //files upload
        // Background Image upload

        $scope.SelectedFileForUploadz = [];
        $scope.selectFileforUploadz = function (input) {

            $scope.SelectedFileForUploadz = input.files;

            if (input.files && input.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    $('#Background')
                        .attr('src', e.target.result)
                };
                reader.readAsDataURL(input.files[0]);
                Uploads();
            }
        }


        function Uploads() {

            var formData = new FormData();

            for (var i = 0; i <= $scope.selectFileforUploadz.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadz[i]);
            }

            //We can send more data to server using append         
            formData.append("Id", 0);

            var defer = $q.defer();
            $http.post("/api/InstituteImageUpload/UploadInstitutionBackgroundImage", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    //swal(d);
                    $scope.MI_BackgroundImage = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
            // Uploads1(miid);
        }


        // Logo image upload
        $scope.SelectedFileForUploadzl = [];
        $scope.selectFileforUploadzl = function (input) {

            $scope.SelectedFileForUploadzl = input.files;
            if (input.files && input.files[0]) {
                var reader = new FileReader();

                reader.onload = function (e) {
                    $('#Logo')
                        .attr('src', e.target.result)
                };
                reader.readAsDataURL(input.files[0]);
                Uploads1();
            }
        }

        function Uploads1() {
            var formData = new FormData();

            for (var i = 0; i <= $scope.selectFileforUploadzl.length; i++) {
                formData.append("File", $scope.SelectedFileForUploadzl[i]);
            }
            //We can send more data to server using append         
            formData.append("Id", 0);

            var defer = $q.defer();
            $http.post("/api/InstituteImageUpload/UploadInstitutionLogo", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {
                    defer.resolve(d);
                    //swal(d);
                    $scope.MI_Logo = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
        }



        //validation part
        $scope.submitted = false;
        $scope.InstutedataValidation = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                if ($scope.MI_Id === undefined || $scope.MI_Id === "") {
                    var phones = $scope.phones;
                    var mobiles = $scope.mobiles;
                    var emails = $scope.emails;

                    var data = {
                        "MI_Name": $scope.MI_Name,
                        "MO_Id": $scope.MO_Id,
                        "IVRMMCT_Name": $scope.IVRMMCT_Name,
                        phones: phones,
                        mobiles: mobiles,
                        emails: emails
                    }

                    apiService.create("Institution/DuplicateInstitionDataFind", data).
                        then(function (promise) {

                            $scope.institutes = promise.institutionname;
                            if (promise.returnval === "nameexist") {
                                swal('Institution Name Exist..!', 'Please Enter Different Name');
                                return;
                            }
                            else if (promise.returnval === "emailexist") {
                                swal('Email Exist..!', 'Please Enter Different Email');
                                return;
                            }
                            else if (promise.returnval === "phoneexist") {
                                swal('Phone Number Exist..!', 'Please Enter Different Phone Number');
                                return;
                            }
                            else if (promise.returnval === "mobileexist") {
                                swal('Mobile Number Exist..!', 'Please Enter Different Mobile Number');
                                return;
                            }
                            else {
                                $scope.saveInstutedata();
                            }
                        })
                    $scope.disableinstname = false;
                }
                else {
                    $scope.saveInstutedata();
                }
            }
        };

        $scope.interacted = function (field) {
            // swal(field);

            return $scope.submitted || field.$dirty;
        };

        $scope.interacted1 = function (field1) {
            // swal(field);

            return $scope.submitted || field1.$dirty;
        };
        $scope.interacted2 = function (field2) {
            // swal(field);

            return $scope.submitted || field2.$dirty;
        };

        $scope.interactedSub = function (field) {
            // swal(field);

            return $scope.submittedSub || field.$dirty;
        };

        var emailsstd = [];
        var selectedvc = [];
        // save /update data
        $scope.saveInstutedata = function () {


            emailsstd = [];
            angular.forEach($scope.emails1, function (email) {
                if (email.mI_SMSAlertToemailids != undefined && email.mI_SMSAlertToemailids != "" && email.mI_SMSAlertToemailids != null) {
                    emailsstd.push(email);
                }
            });
            selectedvc = [];
            angular.forEach($scope.vcdetails1, function (ff) {
                if (ff.checkedvalue == true) {
                    selectedvc.push(ff);
                }

            })


            debugger;
            var phones = $scope.phones;
            var mobiles = $scope.mobiles;
            var emails = $scope.emails;
            var Franchise_flag = 0;
            if ($scope.Franchiseflag) {
                Franchise_flag = 1;
            }
            var data = {
                "MI_Id": $scope.MI_Id,
                "MO_Id": $scope.MO_Id,
                "MI_Name": $scope.MI_Name,
                "MI_Type": $scope.MI_Type,
                "MI_Affiliation": $scope.MI_Affiliation,
                "MI_Address1": $scope.MI_Address1,
                "MI_Address2": $scope.MI_Address2,
                "MI_Address3": $scope.MI_Address3,
                "IVRMMCT_Name": $scope.IVRMMCT_Name,
                "IVRMMS_Id": $scope.IVRMMS_Id,
                "IVRMMC_Id": $scope.IVRMMC_Id,
                "MI_Pincode": $scope.pin,
                "MI_FaxNo": $scope.fax,
                "MI_BackgroundImage": $scope.MI_BackgroundImage,
                "MI_FormColor": $scope.rgbPicker.color,
                "MI_FontColor": $scope.rgbaPicker.color,
                "MI_FontSize": $scope.MI_FontSize,
                "MI_WeekStartDay": $scope.MI_WeekStartDay,
                "MI_DateFormat": $scope.MI_DateFormat,
                "MI_DateSeparator": $scope.MI_DateSeparator,
                "MI_GradingSystem": $scope.MI_GradingSystem,
                "MI_Logo": $scope.MI_Logo,
                "MI_Subdomain": $scope.MI_Subdomain,
                "MI_FranchiseFlag": Franchise_flag,
                "MI_AboutInstitute": $scope.MI_AboutInstitute,
                "MI_ContactDetails": $scope.MI_ContactDetails,
                "MI_SchoolCollegeFlag": $scope.MI_SchoolCollegeFlag,
                "MI_PAN": $scope.MI_PAN,
                "MI_TAN": $scope.MI_TAN,
                "MI_NAAC_InstitutionTypeFlg": $scope.MI_NAAC_InstitutionTypeFlg,
                "MI_NAAC_SubInstitutionTypeFlg": $scope.MI_NAAC_SubInstitutionTypeFlg,
                "MI_SMSCountAlert": $scope.MI_SMSCountAlert,
                "MI_MSTeamsClientId": $scope.MI_MSTeamsClientId,
                "MI_MSTeamsTenentId": $scope.MI_MSTeamsTenentId,
                "MI_MSTemasClinetSecretCode": $scope.MI_MSTemasClinetSecretCode,
                "MI_MSTeamsAppAccessTockenURL": $scope.MI_MSTeamsAppAccessTockenURL,
                "MI_MSTeamsUserAceessTockenURL": $scope.MI_MSTeamsUserAceessTockenURL,
                "MI_MSTeamsMeetingScheduleURL": $scope.MI_MSTeamsMeetingScheduleURL,
                "MI_MSTeamsGrantType": $scope.MI_MSTeamsGrantType,
                "MI_MSTeamsScope": $scope.MI_MSTeamsScope,
                "MI_80GRegNo": $scope.MI_80GRegNo,
                "MI_VCStudentFlag": $scope.ST,
                "MI_VCOthersFlag": $scope.OT,
                "MI_MSTeamsAdminPassword": $scope.MI_MSTeamsAdminPassword,
                "MI_MSTeamsAdminUsername": $scope.MI_MSTeamsAdminUsername,
                "IVRM_OTP_ADMNO": $scope.IVRM_OTP_ADMNO,
                phones: phones,
                mobiles: mobiles,
                emails: emails,
                alertemails: emailsstd,
                selectedvc: selectedvc,
                "MI_EntityId": $scope.MI_EntityId

            };
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            apiService.create("Institution/", data).
                then(function (promise) {
                   
                    $scope.submitted = true;                  
                    if (promise.returnval === "add") {                                             
                        swal('New Institution Created Successfully..!', 'success');
                    }
                    else if (promise.returnval === "update") {
                        
                      
                        swal('Institution Updated Successfully..!', 'success');
                    }
                    $scope.institutes = promise.institutionname;
                    $scope.Sub.MI_Id = promise.mI_Id;
                    $scope.cleardata();
                    $state.reload();
                });
        };
        $scope.submittedSub = false;
        $scope.saveSubscriptionDetails = function (data) {
            $scope.err = "select valid date";
            $scope.submittedSub = true;
            if ($scope.SubForm.$valid) {
                $scope.disinstname = false;
                var subfromdate = new Date($scope.Sub.MISV_FromDate).toDateString();
                var subtodate = new Date($scope.Sub.MISV_ToDate).toDateString();

                var data1 = {
                    "MISV_FromDate": subfromdate,
                    "MISV_ToDate": subtodate,
                    "MI_Id": data.MI_Id,
                    "MISV_Id": data.MISV_Id,
                    "MISV_SubscriptionType": data.MISV_SubscriptionType
                };
                apiService.create("Institution/saveSubscription/", data1).
                    then(function (promise) {
                        $scope.subscriptionlist = promise.subscriptionlist;
                        if (promise.misV_Id > 0) {
                            $scope.Sub = {};
                            $scope.submittedSub = false;
                            $scope.currentPage1 = 1;
                            if (promise.returnval === "sbsave") {
                                swal('Institution Subscription Created Successfully..!', 'success');
                                $scope.Clearsub();
                            }
                            else if (promise.returnval === "sbupdate") {
                                swal('Institution Subscription Updated Successfully..!', 'success');
                                $scope.Clearsub();
                            }
                        } else if (promise.returnval === "sbdup") {
                            swal('Institution Subscription Already Exist..!');
                            $scope.Clearsub();
                            return;
                        }
                        else {
                            swal('Institution Subscription Not Created/ Updated.');
                            $scope.Clearsub();
                        }
                    });
            }
        };

        $scope.Clearsub = function () {
            $scope.todate = true;
            $scope.Sub = {};
            $scope.searchValueSub = "";
            $scope.submittedSub = false;
            $scope.SubForm.$setPristine();
            $scope.SubForm.$setUntouched();


        }
        $scope.checkErr = function (FromDate, ToDate) {
            $scope.errMessage = '';
            var curDate = new Date();

            if (new Date(FromDate) > new Date(ToDate)) {
                $scope.errMessage = 'To Date should be greater than from date';
                return false;
            }
        };

        $scope.editsub = function (sub) {
            $scope.disinstname = true;
            $scope.Sub.MI_Id = sub.mI_Id
            $scope.Sub.MISV_Id = sub.misV_Id
            $scope.Sub.MISV_FromDate = new Date(sub.misV_FromDate);
            $scope.Sub.MISV_ToDate = new Date(sub.misV_ToDate);
            $scope.Sub.MISV_SubscriptionType = sub.misV_SubscriptionType;
            console.log(sub);
        }
        $scope.deletesub = function (data, SweetAlert) {
            console.log(data);
            $scope.Sub = {};
            var mgs = "";
            var confirmmgs = "";
            if (data.misV_ActiveFlag == false) {
                mgs = "Activate";
                confirmmgs = "Activated";

            }
            else {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";

            }

            swal({
                title: "Are you sure?",
                text: "Do you want to " + mgs + " Institute Subscription ..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.DeleteURI("Institution/deleteSubscription", data.misV_Id).
                            then(function (promise) {
                                $scope.currentPage1 = 1;
                                $scope.subscriptionlist = promise.subscriptionlist;
                                if (promise.returnval === "true") {
                                    swal('Institution Subscription Deactivated Successfully..!', 'success');
                                }
                                else {
                                    swal('Institution Subscription Activated Successfully..!', 'success');
                                }
                            })
                    }
                    else {

                        swal("Cancelled", "Ok");



                    }
                });
        }


        //pagination

        //search functionality and pagination
        //$scope.pageChanged = function (newPage) {
        //    if (newPage > 0) {
        //        $scope.newPage = newPage;
        //        $scope.searchInstitute();   //calling Search functionality
        //    }
        //};

        $scope.order = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        $scope.order1 = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse1 = !$scope.reverse1; //if true make it false and vice versa
        }


        $scope.orderInstitute = function (keyname) {

            if (keyname == "trust" && $scope.reverse == true) {

                $scope.sortOrder = "trust_desc";
                $scope.reverse = !$scope.reverse;
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.searchInstitute();  //calling Search functionality
            }
            else if (keyname == "trust" && $scope.reverse == false) {
                $scope.sortOrder = "trust";
                $scope.reverse = !$scope.reverse;
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.searchInstitute();  //calling Search functionality
            }

            if (keyname == "name" && $scope.reverse == true) {
                $scope.sortOrder = "name_desc";
                $scope.reverse = !$scope.reverse;
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.searchInstitute();  //calling Search functionality
            }
            else if (keyname == "name" && $scope.reverse == false) {
                $scope.sortOrder = "name";
                $scope.reverse = !$scope.reverse;
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.searchInstitute();  //calling Search functionality
            }
            else if (keyname == "address" && $scope.reverse == true) {
                $scope.sortOrder = "address_desc";
                $scope.reverse = !$scope.reverse;
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.searchInstitute();  //calling Search functionality


            }
            else if (keyname == "address" && $scope.reverse == false) {
                $scope.sortOrder = "address";
                $scope.reverse = !$scope.reverse;
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.searchInstitute();  //calling Search functionality


            }
            else if (keyname == "pincode" && $scope.reverse == true) {
                $scope.sortOrder = "pincode_desc";
                $scope.reverse = !$scope.reverse;
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.searchInstitute();  //calling Search functionality


            }
            else if (keyname == "pincode" && $scope.reverse == false) {
                $scope.sortOrder = "pincode";
                $scope.reverse = !$scope.reverse;
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.searchInstitute();  //calling Search functionality
            }

            //  $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        // for institute Search pagination

        //search added by hemalekha
        //institution list
        $scope.searchValue = "";
        $scope.test = {};
        //$scope.filterValue = function (obj) {
        //    if (obj.mI_ActiveFlag == 1) {
        //        $scope.test = "Activate";
        //    } else if (obj.mI_ActiveFlag == 0) {
        //        $scope.test = "Deactivate";
        //    }
        //    return angular.lowercase(obj.mI_Name).indexOf(angular.lowercase($scope.searchValue)) >= 0 || angular.lowercase(obj.organisation.mO_Name).indexOf(angular.lowercase($scope.searchValue)) >= 0 || angular.lowercase(obj.mI_Address1).indexOf(angular.lowercase($scope.searchValue)) >= 0 || angular.lowercase($scope.test).indexOf(angular.lowercase($scope.searchValue)) >= 0;
        //}

        //Subscription list
        $scope.searchValueSub = "";
        $scope.testSub = {};
        $scope.filterSubValue = function (obj1) {
            if (obj1.misV_ActiveFlag == true) {
                $scope.testSub = "Activate";
            } else if (obj1.misV_ActiveFlag == false) {
                $scope.testSub = "Deactivate";
            }
            return angular.lowercase(obj1.institution.mI_Name).indexOf(angular.lowercase($scope.searchValueSub)) >= 0 || angular.lowercase(obj1.misV_SubscriptionType).indexOf(angular.lowercase($scope.searchValueSub)) >= 0 || $filter('date')(obj1.misV_FromDate, 'dd-MM-yyyy').indexOf($scope.searchValueSub) >= 0 || $filter('date')(obj1.misV_ToDate, 'dd-MM-yyyy').indexOf($scope.searchValueSub) >= 0 || ($scope.testSub).indexOf($scope.searchValueSub) >= 0;
        }



        $scope.inst = {};
        $scope.searchInstitute = function () {

            $scope.InstitutionSortPagInfo = {
                sortOrder: $scope.sortOrder,
                CurrentPageIndex: $scope.newPage,
                searchString: $scope.inst.searchString,
                searchType: $scope.inst.searchType
            };

            apiService.create("Institution/getInstitutionSearchedDetails", $scope.InstitutionSortPagInfo).
                then(function (promise) {
                    $scope.InstitutionSortPagInfo = {};
                    $scope.inst = {};
                    // for institution pagination 

                    if (promise.institutionname != null && promise.institutionname.length > 0) {

                        $scope.currentPage = promise.instutePagination.currentPageIndex;
                        $scope.itemsPerPage = promise.instutePagination.pageSize;
                        $scope.totalitems = promise.instutePagination.totalItems;

                        $scope.institutes = promise.institutionname;
                    }
                    else {

                        //swal('No Records found to display..!', '');
                    }

                    // for  institution pagination
                });

        }




        // for Subscription Search pagination
        //search functionality and 



        $scope.pageChangedSubscription = function (newPage) {
            if (newPage > 0) {

                $scope.subscription.newPage = newPage;
                $scope.searchSubscription();   //calling Search functionality
            }
        };

        $scope.orderSubscription = function (keyname) {
            if (keyname == "Name" && $scope.reverse1 == true) {
                $scope.subscription.sortOrder = "name_desc";
                $scope.reverse1 = !$scope.reverse1;
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.searchSubscription();  //calling Search functionality


            }
            else if (keyname == "Name" && $scope.reverse1 == false) {
                $scope.subscription.sortOrder = "Name";
                $scope.reverse1 = !$scope.reverse1;
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.searchSubscription();  //calling Search functionality


            }
            if (keyname == "type" && $scope.reverse1 == true) {
                $scope.subscription.sortOrder = "type_desc";
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse1 = !$scope.reverse1;
                $scope.searchSubscription();  //calling Search functionality

            }
            else if (keyname == "type" && $scope.reverse1 == false) {
                $scope.subscription.sortOrder = "type";
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse1 = !$scope.reverse1;
                $scope.searchSubscription();  //calling Search functionality

            }

            else if (keyname == "fromdate" && $scope.reverse1 == true) {
                $scope.subscription.sortOrder = "fromdate_desc";
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse1 = !$scope.reverse1;
                $scope.searchSubscription();  //calling Search functionality

            }
            else if (keyname == "fromdate" && $scope.reverse1 == false) {
                $scope.subscription.sortOrder = "fromdate";
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse1 = !$scope.reverse1;
                $scope.searchSubscription();  //calling Search functionality

            }
            else if (keyname == "todate" && $scope.reverse1 == true) {
                $scope.subscription.sortOrder = "todate_desc";
                $scope.reverse1 = !$scope.reverse1;
                $scope.searchSubscription();  //calling Search functionality

            }
            else if (keyname == "todate" && $scope.reverse1 == false) {
                $scope.subscription.sortOrder = "todate";
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse1 = !$scope.reverse1;
                $scope.searchSubscription();  //calling Search functionality
            }

            // $scope.reverse1 = !$scope.reverse1; //if true make it false and vice versa
        }

        //$scope.searchSubscription = function () {

        //    $scope.SubscriptionSortPagInfo = {
        //        sortOrder: $scope.subscription.sortOrder,
        //        CurrentPageIndex: $scope.subscription.newPage,
        //        searchString: $scope.subscription.searchString,
        //        searchType: $scope.subscription.searchType
        //    };

        //    apiService.create("Institution/getSubscriptionSearchedDetails", $scope.SubscriptionSortPagInfo).
        //       then(function (promise) {
        //           $scope.SubscriptionSortPagInfo = {};
        //           $scope.subscription = {};
        //           // for institution pagination 

        //           if (promise.subscriptionlist != null && promise.subscriptionlist.length > 0) {
        //               // for subscription pagination 
        //               $scope.subscription.currentPage = promise.subscriptionPagination.currentPageIndex;
        //               $scope.subscription.itemsPerPage = promise.subscriptionPagination.pageSize;
        //               $scope.subscription.totalitems = promise.subscriptionPagination.totalItems;

        //               $scope.subscriptionlist = promise.subscriptionlist;
        //               // for  subscription pagination
        //           }
        //           else {

        //               //swal('No Records found to display..!', '');
        //           }

        //           // for  institution pagination
        //       });

        //}

        //
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
        $scope.forminstype = false;
        $scope.onSelectGetTrustdetails = function (trustid) {

            //swal(trustid);

            apiService.getURI("Organisation/getdetails", trustid).then(function (promise) {
                if (promise.organisationname[0].mO_OrganisationType == "Group") {
                    $scope.forminstype = false;
                    // $scope.MI_Typearrlist = [{ "MI_Type": "Yearly", "MI_Type_Name": "Yearly" }, { "MI_Type": "Semester", "MI_Type_Name": "Semester" }];
                }
                else {
                    //  var type = promise.organisationname[0].mO_OrganisationType;
                    //  $scope.MI_Typearrlist = [{ "MI_Type": type, "MI_Type_Name": type }];
                    $scope.MI_Type = promise.organisationname[0].mO_OrganisationType;
                    $scope.forminstype = true;
                }
            });
        };

        $scope.editTrusttype = function (trustid, instituteype) {
            //swal(trustid);

            apiService.getURI("Organisation/getdetails", trustid).then(function (promise) {
                if (promise.organisationname[0].mO_OrganisationType == "Group") {
                    $scope.MI_Type = instituteype;
                    $scope.forminstype = false;
                    // $scope.MI_Typearrlist = [{ "MI_Type": "Yearly", "MI_Type_Name": "Yearly" }, { "MI_Type": "Semester", "MI_Type_Name": "Semester" }];
                }
                else {
                    //  var type = promise.organisationname[0].mO_OrganisationType;
                    //  $scope.MI_Typearrlist = [{ "MI_Type": type, "MI_Type_Name": type }];
                    $scope.MI_Type = instituteype;
                    $scope.forminstype = true;
                }
            });
        };


        //Auto Mapping

        $scope.OnClickAutoMapping = function () {

            if ($scope.SubForm_Auto.$valid) {

                if ($scope.Pervious_MI_Id === $scope.Current_MI_Id) {
                    swal("Reference Institute & Current Institute Should Not Be Same");
                }
                else {
                    var data = {
                        "Pervious_MI_Id": $scope.Pervious_MI_Id,
                        "Current_MI_Id": $scope.Current_MI_Id,
                        "User_Name": $scope.User_Name,
                    };

                    apiService.create("Institution/OnClickSaveAutoMapping", data).then(function (promise) {
                        if (promise !== null) {

                            if (promise.returnval === "true") {
                                swal("Record Saved Successfully");
                            } else {
                                swal("Failed To Save Records");
                            }                            
                        } else {
                            swal("Failed To Save Records");
                        }
                        $state.reload();
                    });
                }

            } else {
                $scope.submitted_Auto = true;
            }
        };

        $scope.interacted_Auto = function (field) {
            return $scope.submitted_Auto;
        };
    }
})();