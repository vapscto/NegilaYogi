(function () {
    'use strict';
    angular
        .module('app')
        .controller('generalConfig', generalConfig)

    generalConfig.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache']
    function generalConfig($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache) {
        //Date:23-12-2016 for displaying privileges.
        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.userPrivileges = privlgs[i];
            }
        }

        $scope.IVRMGC_EnableSTSUBTIntFlg = false;
        $scope.IVRMGC_EnableSTCTIntFlg = false;
        $scope.IVRMGC_EnableSTHODIntFlg = false;
        $scope.IVRMGC_EnableSTPrincipalIntFlg = false;
        $scope.IVRMGC_EnableSTASIntFlg = false;
        $scope.IVRMGC_EnableSTECIntFlg = false;
        $scope.IVRMGC_GMRDTOALLFlg = false;
        $scope.IVRMGC_EnableStaffwiseIntFlg = false;
        $scope.IVRMGC_EnableCTSTIntFlg = false;
        $scope.IVRMGC_EnableHODSTIntFlg = false;
        $scope.IVRMGC_EnablePrincipalSTIntFlg = false;
        $scope.IVRMGC_EnableASSTIntFlg = false;
        $scope.IVRMGC_EnableECSTIntFlg = false;
        $scope.IVRMGC_EnableSUBTSTUIntFlg = false;


        //student update request popup
        $scope.IVRMGC_StudentDataChangeAlertFlg = false;
        $scope.IVRMGC_StudentDataChangeAlertDays = 0;

        $scope.IVRMGC_AttendanceShortageAlertFlg = false;

        $scope.IVRMGC_emailValOTPFlag = false;


        //username creation 
        $scope.IVRMGC_StudentLoginCred = false;
        $scope.IVRMGC_FatherLoginCred = false;
        $scope.IVRMGC_MotherLoginCred = false;
        $scope.IVRMGC_GuardianLoginCred = false;
        $scope.IVRMGC_AutoCreateStudentCredFlg = false;

        $scope.onQtselect = function (obj) {
            debugger;
            var data = {
                "IVRMP_Id": obj.ivrmP_Id
            };
            apiService.create("GeneralConfig/getcontent", data).then(function (promise) {

                if (promise.pagelist !== null && promise.pagelist.length > 0) {
                    $scope.IVRMIMP_DisplayContent = promise.pagelist[0].ivrmimP_DisplayContent;
                }

            });
        };


        $scope.scroll = function () {
            $("html, body").animate({ scrollTop: 0 }, 1000);
        }
        $scope.editEmployee = {};
        $scope.gridOptions = {

            paginationPageSizes: [5, 10, 15],
            paginationPageSize: 5,

            columnDefs: [
                { name: 'SlNo', field: 'name', cellTemplate: '<div class="ui-grid-cell-contents">{{grid.options.paginationPageSize *(grid.options.paginationCurrentPage-1)+grid.renderContainers.body.visibleRowCache.indexOf(row)+1}}</div>' },
                { name: 'instuitename', displayName: 'Institute Name' },
                {
                    field: 'id', name: '',
                    displayName: 'Actions', cellTemplate:
                        '<div class="grid-action-cell">  ' +

                        '<a  href="javascript:void(0)"  ng-click="grid.appScope.getorgvalue(row.entity);"> <i class="fa fa-pencil-square-o" ></i></a>  &nbsp; &nbsp;' +

                        '</div>'
                }
            ]
        };
        $scope.cfg = {};
        $scope.loadInitialDropdowns = function () {
            apiService.getURI("GeneralConfig/Configurationget", 2).then(function (promise) {

                //if ($scope.cfg.ASMAY_Id === promise.academicList[0].asmaY_Id) {
                //}
                //else {
                //    $scope.academic = promise.academicList;
                //    $scope.cfg.ASMAY_Id = promise.academicList[0].asmaY_Id;
                //}
                // $scope.UIConfig = promise.mstConfigList;
                $scope.gridOptions.data = promise.mstConfigList;
                $scope.pagelist = promise.pagelist;

            })
        }

        $scope.getorgvalue = function (employee) {
            $scope.editEmployee = employee.ivrmgC_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("GeneralConfig/geteditdata", pageid).then(function (promise) {

                if (promise.mstConfigList.length > 0) {
                    $scope.ID = promise.mstConfigList[0].ivrmgC_Id;
                    if (promise.mstConfigList[0].ivrmgC_MobileValOTPFlag === true)
                        $scope.IVRMGC_MobileValOTPFlag = 1;
                    else
                        $scope.IVRMGC_MobileValOTPFlag = 0;
                    $scope.IVRMGC_emailValOTPFlag = promise.mstConfigList[0].ivrmgC_emailValOTPFlag;
                    //if (promise.mstConfigList[0].ivrmgC_emailValOTPFlag === true)
                    //    $scope.IVRMGC_emailValOTPFlag = promise.mstConfigList[0].ivrmgC_emailValOTPFlag;
                    //else
                    //    $scope.IVRMGC_emailValOTPFlag = promise.mstConfigList[0].ivrmgC_emailValOTPFlag;

                    $scope.IVRMGC_StudentPhotoPath = promise.mstConfigList[0].ivrmgC_StudentPhotoPath;
                    $scope.IVRMGC_StaffPhotoPath = promise.mstConfigList[0].ivrmgC_StaffPhotoPath;
                    if (promise.mstConfigList[0].ivrmgC_ComTrasaNoFlag === true)
                        $scope.IVRMGC_ComTrasaNoFlag = 1;
                    else
                        $scope.IVRMGC_ComTrasaNoFlag = 0;


                    $scope.IVRMGC_SMSDomain = promise.mstConfigList[0].ivrmgC_SMSDomain;
                    $scope.IVRMGC_SMSURL = promise.mstConfigList[0].ivrmgC_SMSURL;
                    $scope.IVRMGC_SMSUserName = promise.mstConfigList[0].ivrmgC_SMSUserName;
                    $scope.IVRMGC_SMSPassword = promise.mstConfigList[0].ivrmgC_SMSPassword;
                    $scope.IVRMGC_SMSSenderId = promise.mstConfigList[0].ivrmgC_SMSSenderId;
                    $scope.IVRMGC_SendGrid_Key = promise.mstConfigList[0].ivrmgC_SendGrid_Key;
                    $scope.IVRMGC_SMSWorkingKey = promise.mstConfigList[0].ivrmgC_SMSWorkingKey;
                    $scope.IVRMGC_SMSFooter = promise.mstConfigList[0].ivrmgC_SMSFooter;
                    if (promise.mstConfigList[0].ivrmgC_SMSActiveFlag === true)
                        $scope.IVRMGC_SMSActiveFlag = 1;
                    else
                        $scope.IVRMGC_SMSActiveFlag = 0;
                    $scope.IVRMGC_OTPMobileNo = promise.mstConfigList[0].ivrmgC_OTPMobileNo;
                    $scope.IVRMGC_OTPMailId = promise.mstConfigList[0].ivrmgC_OTPMailId;
                    $scope.IVRMGC_emailUserName = promise.mstConfigList[0].ivrmgC_emailUserName;
                    $scope.IVRMGC_emailPassword = promise.mstConfigList[0].ivrmgC_emailPassword;
                    $scope.IVRMGC_HostName = promise.mstConfigList[0].ivrmgC_HostName;
                    $scope.IVRMGC_PortNo = promise.mstConfigList[0].ivrmgC_PortNo;
                    $scope.IVRMGC_MailGenralDesc = promise.mstConfigList[0].ivrmgC_MailGenralDesc;
                    $scope.IVRMGC_Webiste = promise.mstConfigList[0].ivrmgC_Webiste;
                    $scope.IVRMGC_emailid = promise.mstConfigList[0].ivrmgC_emailid;
                    $scope.IVRMGC_emailFooter = promise.mstConfigList[0].ivrmgC_emailFooter;
                    $scope.IVRMGC_CCMail = promise.mstConfigList[0].ivrmgC_CCMail;
                    $scope.IVRMGC_BCCMail = promise.mstConfigList[0].ivrmgC_BCCMail;
                    $scope.IVRMGC_ToMail = promise.mstConfigList[0].ivrmgC_ToMail;
                    if (promise.mstConfigList[0].ivrmgC_EmailActiveFlag === true)
                        $scope.IVRMGC_EmailActiveFlag = 1;
                    else
                        $scope.IVRMGC_EmailActiveFlag = 0;

                    $scope.IVRMGC_Pagination = promise.mstConfigList[0].ivrmgC_Pagination;
                    $scope.IVRMGC_PagePagination = promise.mstConfigList[0].ivrmgC_PagePagination;
                    $scope.IVRMGC_ReminderDays = promise.mstConfigList[0].ivrmgC_ReminderDays;
                    $scope.IVRMGC_ClassCapacity = promise.mstConfigList[0].ivrmgC_ClassCapacity;
                    $scope.IVRMGC_SectionCapacity = promise.mstConfigList[0].ivrmgC_SectionCapacity;
                    $scope.IVRMGC_SCLockingPeriod = promise.mstConfigList[0].ivrmgC_SCLockingPeriod;


                    if (promise.mstConfigList[0].ivrmgC_SCActive === true)
                        $scope.IVRMGC_SCActive = 1;
                    else
                        $scope.IVRMGC_SCActive = 0;

                    if (promise.mstConfigList[0].ivrmgC_FPActive === true)
                        $scope.IVRMGC_FPActive = 1;
                    else
                        $scope.IVRMGC_FPActive = 0;

                    if (promise.mstConfigList[0].ivrmgC_OnlineFPActive === true)
                        $scope.IVRMGC_OnlineFPActive = 1;
                    else
                        $scope.IVRMGC_OnlineFPActive = 0;

                    if (promise.mstConfigList[0].ivrmgC_FaceReaderActive === true)
                        $scope.IVRMGC_FaceReaderActive = 1;
                    else
                        $scope.IVRMGC_FaceReaderActive = 0;

                    if (promise.mstConfigList[0].ivrmgC_DefaultStudentSelection === true)
                        $scope.IVRMGC_DefaultStudentSelection = 1;
                    else
                        $scope.IVRMGC_DefaultStudentSelection = 0;

                    if (promise.mstConfigList[0].ivrmgC_CatLogoFlg === true)
                        $scope.IVRMGC_CatLogoFlg = 1;
                    else
                        $scope.IVRMGC_CatLogoFlg = 0;


                    $scope.IVRMGC_ReportPagination = promise.mstConfigList[0].ivrmgC_ReportPagination;

                    $scope.IVRMGC_TransportRequired = promise.mstConfigList[0].ivrmgC_TransportRequired;

                    if (promise.mstConfigList[0].ivrmgC_AdmnoColumnDisplay === true)
                        $scope.IVRMGC_AdmNoColumnDisplay = 1;
                    else
                        $scope.IVRMGC_AdmNoColumnDisplay = 0;

                    if (promise.mstConfigList[0].ivrmgC_RegnoColumnDisplay === true)
                        $scope.regno = 1;
                    else
                        $scope.regno = 0;

                    if (promise.mstConfigList[0].ivrmgC_RollnoColumnDisplay === true)
                        $scope.rolno = 1;
                    else
                        $scope.rolno = 0;
                    if (promise.mstConfigList[0].ivrmgC_SportsPointsDropdownFlg === true)
                        $scope.IVRMGC_SportsPointsDropdownFlg = 1;
                    else
                        $scope.IVRMGC_SportsPointsDropdownFlg = 0;

                    $scope.IVRMGC_Classwise_Payment = promise.mstConfigList[0].ivrmgC_Classwise_Payment;
                    $scope.IVRMGC_APIOrSMTPFlg = promise.mstConfigList[0].ivrmgC_APIOrSMTPFlg;


                    $scope.IVRMGC_AdmNo_RegNo_RollNo_DefaultFlag = promise.mstConfigList[0].ivrmgC_AdmNo_RegNo_RollNo_DefaultFlag;
                    $scope.IVRMGC_FPLockingPeriod = promise.mstConfigList[0].ivrmgC_FPLockingPeriod;

                    $scope.IVRMGC_PasswordExpiryDuration = promise.mstConfigList[0].ivrmgC_PasswordExpiryDuration;


                    $scope.IVRMGC_AttendanceShortagePercent = promise.mstConfigList[0].ivrmgC_AttendanceShortagePercent;
                    $scope.IVRMGC_AttShortageAlertDays = promise.mstConfigList[0].ivrmgC_AttShortageAlertDays;
                    $scope.IVRMGC_AttendanceShortageAlertFlg = promise.mstConfigList[0].ivrmgC_AttendanceShortageAlertFlg;


                    $scope.IVRMGC_Disclaimer = promise.mstConfigList[0].ivrmgC_Disclaimer;
                    if (promise.mstConfigList[0].ivrmgC_PrincipalSign != null) {
                        $scope.IVRMGC_PrincipalSign = promise.mstConfigList[0].ivrmgC_PrincipalSign;
                    }
                    if (promise.mstConfigList[0].ivrmgC_ManagerSign != null) {
                        $scope.IVRMGC_ManagerSign = promise.mstConfigList[0].ivrmgC_ManagerSign;
                    }

                    //Interaction 
                    $scope.IVRMGC_EnableSTSUBTIntFlg = promise.mstConfigList[0].ivrmgC_EnableSTSUBTIntFlg;
                    $scope.IVRMGC_EnableSTCTIntFlg = promise.mstConfigList[0].ivrmgC_EnableSTCTIntFlg;
                    $scope.IVRMGC_EnableSTHODIntFlg = promise.mstConfigList[0].ivrmgC_EnableSTHODIntFlg;
                    $scope.IVRMGC_EnableSTPrincipalIntFlg = promise.mstConfigList[0].ivrmgC_EnableSTPrincipalIntFlg;
                    $scope.IVRMGC_EnableSTASIntFlg = promise.mstConfigList[0].ivrmgC_EnableSTASIntFlg;
                    $scope.IVRMGC_EnableSTECIntFlg = promise.mstConfigList[0].ivrmgC_EnableSTECIntFlg;
                    $scope.IVRMGC_GMRDTOALLFlg = promise.mstConfigList[0].ivrmgC_GMRDTOALLFlg
                    $scope.IVRMGC_EnableStaffwiseIntFlg = promise.mstConfigList[0].ivrmgC_EnableStaffwiseIntFlg;
                    $scope.IVRMGC_EnableCTSTIntFlg = promise.mstConfigList[0].ivrmgC_EnableCTSTIntFlg;
                    $scope.IVRMGC_EnableHODSTIntFlg = promise.mstConfigList[0].ivrmgC_EnableHODSTIntFlg;
                    $scope.IVRMGC_EnablePrincipalSTIntFlg = promise.mstConfigList[0].ivrmgC_EnablePrincipalSTIntFlg;
                    $scope.IVRMGC_EnableASSTIntFlg = promise.mstConfigList[0].ivrmgC_EnableASSTIntFlg;
                    $scope.IVRMGC_EnableECSTIntFlg = promise.mstConfigList[0].ivrmgC_EnableECSTIntFlg;
                    $scope.IVRMGC_EnableSUBTSTUIntFlg = promise.mstConfigList[0].ivrmgC_EnableSUBTSTUIntFlg;
                    $scope.IVRMGC_StudentDataChangeAlertFlg = promise.mstConfigList[0].ivrmgC_StudentDataChangeAlertFlg;
                    $scope.IVRMGC_StudentDataChangeAlertDays = promise.mstConfigList[0].ivrmgC_StudentDataChangeAlertDays;


                    //user name creation

                    $scope.IVRMGC_StudentLoginCred = promise.mstConfigList[0].ivrmgC_StudentLoginCred;
                    $scope.IVRMGC_FatherLoginCred = promise.mstConfigList[0].ivrmgC_FatherLoginCred;
                    $scope.IVRMGC_MotherLoginCred = promise.mstConfigList[0].ivrmgC_MotherLoginCred;
                    $scope.IVRMGC_GuardianLoginCred = promise.mstConfigList[0].ivrmgC_GuardianLoginCred;
                    $scope.IVRMGC_AutoCreateStudentCredFlg = promise.mstConfigList[0].ivrmgC_AutoCreateStudentCredFlg;
                    $scope.IVRMGC_UserNameOptionsFlg = promise.mstConfigList[0].ivrmgC_UserNameOptionsFlg;

                    if (promise.usernamearray != null && promise.usernamearray.length > 0) {
                        // $scope.username = promise.usernamearray;
                        angular.forEach(promise.usernamearray, function (zz) {
                            $scope.username.push({ IVRMCUNP_Id: zz.ivrmcunP_Id, IVRMCUNP_Length: zz.ivrmcunP_Length, IVRMCUNP_Order: zz.ivrmcunP_Order, IVRMCUNP_FromOrderFlg: zz.ivrmcunP_FromOrderFlg, IVRMCUNP_Field: zz.ivrmcunP_Field });
                        })
                    }
                    else {
                        $scope.username = [];

                        $scope.username = [{ id: 'roll1' }];
                        $scope.addNewuser = function () {
                            var newItemNo = $scope.username.length + 1;
                            if (newItemNo <= 5) {
                                $scope.username.push({ 'id': 'roll' + newItemNo });
                            }
                        };
                    }
                }
                $scope.scroll();
            })
        }

        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };
        // Save master config
        $scope.submitted = false;
        $scope.SaveMasterConfig = function (arrlist2) {
            if ($scope.ID == null || $scope.ID == "undefined") {
                $scope.ID = 0;
            }
            if ($scope.IVRMGC_OTPMobileNo == null || $scope.IVRMGC_OTPMobileNo == '' || $scope.IVRMGC_OTPMobileNo == 0) {
                $scope.IVRMGC_OTPMobileNo = undefined;
            }
            if ($scope.IVRMGC_OTPMailId == null || $scope.IVRMGC_OTPMailId == 0) {
                $scope.IVRMGC_OTPMobileNo = undefined;
            }

            var usernameConfig = $scope.username;

            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                if ($scope.IVRMGC_AdmNoColumnDisplay == true) {
                    $scope.IVRMGC_AdmNoColumnDisplay = 1;
                }
                else {
                    $scope.IVRMGC_AdmNoColumnDisplay = 0;
                }
                if ($scope.regno == true) {
                    $scope.regno = 1;
                }
                else {
                    $scope.regno = 0;
                }
                if ($scope.rolno == true) {
                    $scope.ASC_Default_Clm__Adm_Flag = 1;
                }

                else {
                    $scope.ASC_Default_Clm__Adm_Flag = 0;
                }

                //added
                if ($scope.IVRMGC_CatLogoFlg == true) {
                    $scope.catnum = 1;
                }
                else {
                    $scope.catnum = 0;
                }
                //..

                if ($scope.IVRMGC_TransportRequired == true) {
                    $scope.IVRMGC_TransportRequired = 1;
                }
                else {
                    $scope.IVRMGC_TransportRequired = 0;
                }

                if ($scope.IVRMGC_SCLockingPeriod == undefined || $scope.IVRMGC_SCLockingPeriod == "") {
                    $scope.IVRMGC_SCLockingPeriod = 0;
                }

                if ($scope.IVRMGC_FPLockingPeriod == undefined || $scope.IVRMGC_FPLockingPeriod == "") {
                    $scope.IVRMGC_FPLockingPeriod = 0;
                }

                if ($scope.IVRMGC_PasswordExpiryDuration == undefined || $scope.IVRMGC_PasswordExpiryDuration == "") {
                    $scope.IVRMGC_PasswordExpiryDuration = 0;
                }

                if ($scope.IVRMGC_AttendanceShortageAlertFlg == undefined || $scope.IVRMGC_AttendanceShortageAlertFlg == "") {
                    $scope.IVRMGC_AttendanceShortageAlertFlg = 0;
                }
                if ($scope.IVRMGC_AttShortageAlertDays == undefined || $scope.IVRMGC_AttShortageAlertDays == "") {
                    $scope.IVRMGC_AttShortageAlertDays = 0;
                }
                if ($scope.IVRMGC_AttendanceShortagePercent == undefined || $scope.IVRMGC_AttendanceShortagePercent == "") {
                    $scope.IVRMGC_AttendanceShortagePercent = 0;
                }
                if ($scope.IVRMGC_StudentLoginCred == undefined || $scope.IVRMGC_StudentLoginCred == "") {
                    $scope.IVRMGC_StudentLoginCred = 0;
                }
                if ($scope.IVRMGC_FatherLoginCred == undefined || $scope.IVRMGC_FatherLoginCred == "") {
                    $scope.IVRMGC_FatherLoginCred = 0;
                }
                if ($scope.IVRMGC_MotherLoginCred == undefined || $scope.IVRMGC_MotherLoginCred == "") {
                    $scope.IVRMGC_MotherLoginCred = 0;
                }
                if ($scope.IVRMGC_GuardianLoginCred == undefined || $scope.IVRMGC_GuardianLoginCred == "") {
                    $scope.IVRMGC_GuardianLoginCred = 0;
                }
                if ($scope.IVRMGC_AutoCreateStudentCredFlg == undefined || $scope.IVRMGC_AutoCreateStudentCredFlg == "") {
                    $scope.IVRMGC_AutoCreateStudentCredFlg = 0;
                }



                if ($scope.IVRMGC_AdmNoColumnDisplay == 0 && $scope.regno == 0 && $scope.rolno == 0 && $scope.catno == 0) {
                    return swal("Please Select Alteast One Default Column Dispaly");
                }

                $scope.usernamearray = [];

                if ($scope.username != null && $scope.username.length > 0) {
                    angular.forEach($scope.username, function (zz) {
                        $scope.usernamearray.push({ IVRMCUNP_Id: zz.IVRMCUNP_Id, IVRMCUNP_Length: zz.IVRMCUNP_Length, IVRMCUNP_Order: zz.IVRMCUNP_Order, IVRMCUNP_FromOrderFlg: zz.IVRMCUNP_FromOrderFlg, IVRMCUNP_Field: zz.IVRMCUNP_Field });
                    })

                }
                var data = {
                    "IVRMGC_Id": $scope.ID,
                    "IVRMGC_MobileValOTPFlag": $scope.IVRMGC_MobileValOTPFlag,
                    "IVRMGC_emailValOTPFlag": $scope.IVRMGC_emailValOTPFlag,
                    "IVRMGC_StudentPhotoPath": $scope.IVRMGC_StudentPhotoPath,
                    "IVRMGC_StaffPhotoPath": $scope.IVRMGC_StaffPhotoPath,
                    "IVRMGC_ComTrasaNoFlag": $scope.IVRMGC_ComTrasaNoFlag,
                    "IVRMGC_SMSDomain": $scope.IVRMGC_SMSDomain,
                    "IVRMGC_SMSURL": $scope.IVRMGC_SMSURL,
                    "IVRMGC_SMSUserName": $scope.IVRMGC_SMSUserName,
                    "IVRMGC_SMSPassword": $scope.IVRMGC_SMSPassword,
                    "IVRMGC_SMSSenderId": $scope.IVRMGC_SMSSenderId,
                    "IVRMGC_SendGrid_Key": $scope.IVRMGC_SendGrid_Key,
                    "IVRMGC_SMSWorkingKey": $scope.IVRMGC_SMSWorkingKey,
                    "IVRMGC_SMSFooter": $scope.IVRMGC_SMSFooter,
                    "IVRMGC_SMSActiveFlag": $scope.IVRMGC_SMSActiveFlag,
                    "IVRMGC_emailUserName": $scope.IVRMGC_emailUserName,
                    "IVRMGC_emailPassword": $scope.IVRMGC_emailPassword,
                    "IVRMGC_HostName": $scope.IVRMGC_HostName,
                    "IVRMGC_PortNo": $scope.IVRMGC_PortNo,
                    "IVRMGC_MailGenralDesc": $scope.IVRMGC_MailGenralDesc,
                    "IVRMGC_Webiste": $scope.IVRMGC_Webiste,
                    "IVRMGC_emailid": $scope.IVRMGC_emailid,
                    "IVRMGC_emailFooter": $scope.IVRMGC_emailFooter,
                    "IVRMGC_CCMail": $scope.IVRMGC_CCMail,
                    "IVRMGC_BCCMail": $scope.IVRMGC_BCCMail,
                    "IVRMGC_ToMail": $scope.IVRMGC_ToMail,
                    "IVRMGC_EmailActiveFlag": $scope.IVRMGC_EmailActiveFlag,
                    "IVRMGC_Pagination": $scope.IVRMGC_Pagination,
                    "IVRMGC_PagePagination": $scope.IVRMGC_PagePagination,
                    "IVRMGC_ReminderDays": $scope.IVRMGC_ReminderDays,
                    "IVRMGC_ClassCapacity": $scope.IVRMGC_ClassCapacity,
                    "IVRMGC_SectionCapacity": $scope.IVRMGC_SectionCapacity,
                    "IVRMGC_SCLockingPeriod": $scope.IVRMGC_SCLockingPeriod,
                    "IVRMGC_SCActive": $scope.IVRMGC_SCActive,
                    "IVRMGC_FPActive": $scope.IVRMGC_FPActive,
                    "IVRMGC_OnlineFPActive": $scope.IVRMGC_OnlineFPActive,
                    "IVRMGC_FaceReaderActive": $scope.IVRMGC_FaceReaderActive,
                    "IVRMGC_DefaultStudentSelection": $scope.IVRMGC_DefaultStudentSelection,
                    "IVRMGC_ReportPagination": $scope.IVRMGC_ReportPagination,
                    "IVRMGC_AdmnoColumnDisplay": $scope.IVRMGC_AdmNoColumnDisplay,
                    "IVRMGC_RegnoColumnDisplay": $scope.regno,
                    "IVRMGC_RollnoColumnDisplay": $scope.rolno,
                    "IVRMGC_FPLockingPeriod": $scope.IVRMGC_FPLockingPeriod,
                    "IVRMGC_PasswordExpiryDuration": $scope.IVRMGC_PasswordExpiryDuration,
                    "IVRMGC_AttShortageAlertDays": $scope.IVRMGC_AttShortageAlertDays,
                    "IVRMGC_AttendanceShortageAlertFlg": $scope.IVRMGC_AttendanceShortageAlertFlg,
                    "IVRMGC_AttendanceShortagePercent": $scope.IVRMGC_AttendanceShortagePercent,
                    "IVRMGC_Disclaimer": $scope.IVRMGC_Disclaimer,
                    "IVRMGC_PrincipalSign": $scope.IVRMGC_PrincipalSign,
                    "IVRMGC_ManagerSign": $scope.IVRMGC_ManagerSign,
                    "IVRMGC_AdmNo_RegNo_RollNo_DefaultFlag": $scope.IVRMGC_AdmNo_RegNo_RollNo_DefaultFlag,
                    "IVRMGC_TransportRequired": $scope.IVRMGC_TransportRequired,

                    "IVRMGC_Classwise_Payment": $scope.IVRMGC_Classwise_Payment,
                    "IVRMGC_APIOrSMTPFlg": $scope.IVRMGC_APIOrSMTPFlg,

                    "IVRMGC_OTPMobileNo": $scope.IVRMGC_OTPMobileNo,
                    "IVRMGC_OTPMailId": $scope.IVRMGC_OTPMailId,
                    "IVRMGC_SportsPointsDropdownFlg": $scope.IVRMGC_SportsPointsDropdownFlg,
                    "IVRMGC_EnableSTSUBTIntFlg": $scope.IVRMGC_EnableSTSUBTIntFlg,
                    "IVRMGC_EnableSTCTIntFlg": $scope.IVRMGC_EnableSTCTIntFlg,
                    "IVRMGC_EnableSTHODIntFlg": $scope.IVRMGC_EnableSTHODIntFlg,
                    "IVRMGC_EnableSTPrincipalIntFlg": $scope.IVRMGC_EnableSTPrincipalIntFlg,
                    "IVRMGC_EnableSTASIntFlg": $scope.IVRMGC_EnableSTASIntFlg,
                    "IVRMGC_EnableSTECIntFlg": $scope.IVRMGC_EnableSTECIntFlg,
                    "IVRMGC_GMRDTOALLFlg": $scope.IVRMGC_GMRDTOALLFlg,
                    "IVRMGC_EnableStaffwiseIntFlg": $scope.IVRMGC_EnableStaffwiseIntFlg,
                    "IVRMGC_EnableCTSTIntFlg": $scope.IVRMGC_EnableCTSTIntFlg,
                    "IVRMGC_EnableHODSTIntFlg": $scope.IVRMGC_EnableHODSTIntFlg,
                    "IVRMGC_EnablePrincipalSTIntFlg": $scope.IVRMGC_EnablePrincipalSTIntFlg,
                    "IVRMGC_EnableASSTIntFlg": $scope.IVRMGC_EnableASSTIntFlg,
                    "IVRMGC_EnableECSTIntFlg": $scope.IVRMGC_EnableECSTIntFlg,
                    "IVRMGC_EnableSUBTSTUIntFlg": $scope.IVRMGC_EnableSUBTSTUIntFlg,
                    "IVRMGC_StudentDataChangeAlertFlg": $scope.IVRMGC_StudentDataChangeAlertFlg,
                    "IVRMGC_StudentDataChangeAlertDays": $scope.IVRMGC_StudentDataChangeAlertDays,

                    "IVRMGC_StudentLoginCred": $scope.IVRMGC_StudentLoginCred,
                    "IVRMGC_FatherLoginCred": $scope.IVRMGC_FatherLoginCred,
                    "IVRMGC_MotherLoginCred": $scope.IVRMGC_MotherLoginCred,
                    "IVRMGC_GuardianLoginCred": $scope.IVRMGC_GuardianLoginCred,
                    "IVRMGC_AutoCreateStudentCredFlg": $scope.IVRMGC_AutoCreateStudentCredFlg,
                    "IVRMGC_UserNameOptionsFlg": $scope.IVRMGC_UserNameOptionsFlg,
                    "pageids": $scope.pagecontentlist,
                    "IVRMGC_CatLogoFlg": $scope.catnum,
                    "usernameConfig": $scope.usernamearray

                }
                apiService.create("GeneralConfig/savegenConfigData", data).then(function (promise) {

                    swal(promise.returnval);
                    $scope.loadInitialDropdowns();
                    $state.reload();

                });
            }
        }

        $scope.delete_rec = function (option) {

            for (var i = $scope.pagecontentlist.length - 1; i >= 0; i--) {
                if ($scope.pagecontentlist[i].IVRMP_Id === option.IVRMP_Id) {
                    $scope.pagecontentlist.splice(i, 1);
                }
            }
        };

        $scope.pagecontentlist = [];
        $scope.addtogrid = function () {
            var cnt = 0;
            var pagn = '';
            angular.forEach($scope.pagecontentlist, function (zz) {
                if (zz.IVRMP_Id === $scope.IVRMP_Id.ivrmP_Id) {
                    cnt += 1;
                }
            })
            if (cnt === 0) {
                debugger;
                angular.forEach($scope.pagelist, function (xx) {
                    if (xx.ivrmP_Id === $scope.IVRMP_Id.ivrmP_Id) {
                        pagn = xx.ivrmmP_PageName;
                    }
                })

                $scope.pagecontentlist.push({ IVRMP_Id: $scope.IVRMP_Id.ivrmP_Id, IVRMIMP_DisplayContent: $scope.IVRMIMP_DisplayContent, IVRMMP_PageName: pagn });
            }

        }

        $scope.submitted = false;
        $scope.cleredata = function () {
            $scope.ID = "";
            $scope.IVRMGC_MobileValOTPFlag = 0;
            // $scope.IVRMGC_emailValOTPFlag = 0;
            $scope.IVRMGC_emailValOTPFlag = false;
            $scope.IVRMGC_StudentPhotoPath = "";
            $scope.IVRMGC_StaffPhotoPath = "";
            $scope.IVRMGC_ComTrasaNoFlag = 0;
            $scope.IVRMGC_SMSDomain = "";
            $scope.IVRMGC_SMSURL = "";
            $scope.IVRMGC_SMSUserName = "";
            $scope.IVRMGC_SMSPassword = "";
            $scope.IVRMGC_SMSSenderId = "";
            $scope.IVRMGC_SendGrid_Key = "";
            $scope.IVRMGC_SMSWorkingKey = "";
            $scope.IVRMGC_SMSFooter = "";
            $scope.IVRMGC_SMSActiveFlag = 0;
            $scope.IVRMGC_emailUserName = "";
            $scope.IVRMGC_emailPassword = "";
            $scope.IVRMGC_HostName = "";
            $scope.IVRMGC_PortNo = "";
            $scope.IVRMGC_MailGenralDesc = "";
            $scope.IVRMGC_Webiste = "";
            $scope.IVRMGC_emailid = "";
            $scope.IVRMGC_emailFooter = "";
            $scope.IVRMGC_CCMail = "";
            $scope.IVRMGC_BCCMail = "";
            $scope.IVRMGC_ToMail = "";
            $scope.IVRMGC_EmailActiveFlag = 0;
            $scope.IVRMGC_Pagination = "";
            $scope.IVRMGC_PagePagination = "";
            $scope.IVRMGC_ReminderDays = "";
            $scope.IVRMGC_ClassCapacity = "";
            $scope.IVRMGC_SectionCapacity = "";
            $scope.IVRMGC_SCLockingPeriod = "";
            $scope.IVRMGC_SCActive = 0;
            $scope.IVRMGC_FPActive = 0;
            $scope.IVRMGC_OnlineFPActive = 0;
            $scope.IVRMGC_FaceReaderActive = 0;
            $scope.IVRMGC_DefaultStudentSelection = 0;
            $scope.IVRMGC_ReportPagination = "";
            $scope.IVRMGC_PrincipalSign = "";
            $scope.IVRMGC_ManagerSign = "";
            $scope.IVRMGC_OTPMailId = "";
            $scope.IVRMGC_OTPMobileNo = "";
            $scope.IVRMGC_AdmNoColumnDisplay = 0;
            $scope.regno = 0;
            $scope.rolno = 0;
            $scope.IVRMGC_OnlinePaymentCompany = "";
            $scope.IVRMGC_FPLockingPeriod = "";
            $scope.IVRMGC_PasswordExpiryDuration = "";
            $scope.IVRMGC_AttShortageAlertDays = "";
            $scope.IVRMGC_AttendanceShortagePercent = "";


            $scope.IVRMGC_Disclaimer = "";
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $state.reload();
        }

        $scope.showprincipalsign = function (IVRMGC_PrincipalSign) {
            $('#preview').attr('src', IVRMGC_PrincipalSign);
        }
        $scope.showmangaersign = function (IVRMGC_ManagerSign) {
            $('#preview').attr('src', IVRMGC_ManagerSign);
        }

        $scope.uploadPrincipalSignature = [];

        $scope.uploadPrincipalSignature = function (input) {

            $scope.uploadPrincipalSignature = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    UploadPrincipalsign();
                }
                else if (input.files[0].type != "image/jpeg") {
                    swal("Please Upload the JPEG file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }
            }
        }

        function UploadPrincipalsign() {
            var formData = new FormData();

            for (var i = 0; i <= $scope.uploadPrincipalSignature.length; i++) {
                formData.append("File", $scope.uploadPrincipalSignature[i]);
            }

            //We can send more data to server using append         
            // formData.append("Id", 0);

            var defer = $q.defer();
            $http.post("/api/ImageUpload/Upload_Principal_sign", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);
                    // swal(d);
                    $scope.IVRMGC_PrincipalSign = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
            // Uploads1(miid);
        }


        //manager signature
        $scope.uploadManagerSignature = [];

        $scope.uploadManagerSignature = function (input) {

            $scope.uploadManagerSignature = input.files;

            if (input.files && input.files[0]) {

                if (input.files[0].type == "image/jpeg" && input.files[0].size <= 2097152)  // 2097152 bytes = 2MB 
                {
                    UploadManagersign();
                }
                else if (input.files[0].type != "image/jpeg") {
                    swal("Please Upload the JPEG file");
                    return;
                } else if (input.files[0].size > 2097152) {
                    swal("Image size should be less than 2MB");
                    return;
                }
            }
        }

        function UploadManagersign() {
            var formData = new FormData();

            for (var i = 0; i <= $scope.uploadManagerSignature.length; i++) {
                formData.append("File", $scope.uploadManagerSignature[i]);
            }

            //We can send more data to server using append         
            // formData.append("Id", 0);

            var defer = $q.defer();
            $http.post("/api/ImageUpload/Upload_Manager_sign", formData,
                {
                    withCredentials: true,
                    headers: { 'Content-Type': undefined },
                    transformRequest: angular.identity
                })
                .success(function (d) {

                    defer.resolve(d);
                    // swal(d);
                    $scope.IVRMGC_ManagerSign = d;
                })
                .error(function () {
                    defer.reject("File Upload Failed!");
                });
            // Uploads1(miid);
        }
        $scope.currentPages = 1;
        $scope.itemsPerPages = 5;

        $scope.username = [{ id: 'user' }];
        $scope.addNewuser = function () {
            var newItemNo = $scope.username.length + 1;

            if (newItemNo <= 5) {
                $scope.username.push({ 'id': 'user' + newItemNo });
            }
        };


        $scope.removeNewuser = function (index, id) {


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
                        var IVRMCUNP_Id = 0;
                        angular.forEach($scope.username, function (opqr1) {
                            if (opqr1.IVRMCUNP_Id == id) {
                                IVRMCUNP_Id = opqr1.IVRMCUNP_Id;
                            }
                        });


                        apiService.getURI("GeneralConfig/deleteUserNameconfig", IVRMCUNP_Id).then(function (promise) {
                            if (promise.message == "Success") {
                                //$scope.sectionlist = promise.sectionlist;
                                //$scope.getclass = false;

                                var newItemNo = $scope.username.length - 1;
                                $scope.username.splice(index, 1);

                                if ($scope.username.length == 0) {
                                    // $scope.Sib = false;
                                }

                                swal('Record Deleted Successfully', 'success');
                                $state.reload();
                            }
                            else {
                                swal('Failed!!!');
                            }
                        });
                    }
                });
        };
    }
})();
