(function () {
    'use strict';

    angular.module('app').controller('configController', configController);

    configController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$cookieStore', '$stateParams', '$filter', 'FormSubmitter', '$window', 'superCache'];

    function configController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $cookieStore, $stateParams, $filter, FormSubmitter, $window, superCache) {

        $scope.disonedit = false;
        var HostName = location.host;
       
        $scope.Previous = function () {
            $window.location.href = 'http://' + HostName + '/#/app/masterschooltype/';
        };

        $scope.Next = function () {
            $window.location.href = 'http://' + HostName + '/#/app/MasterMenuPageMapping/';
        };

        $scope.Finish = function () {
            $window.location.href = 'http://' + HostName + '/#/app/generalSettingWizardComplete/';
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

        $scope.scroll = function () {
            $("html, body").animate({ scrollTop: 0 }, 1000);
        }  

        $scope.cfg = {};
        // load initial dropdowns
        $scope.loadInitialDropdowns = function () {
            apiService.getURI("StudentMasterConfiguration/masterConfiguration", 2).then(function (promise) {
                //$scope.academic = promise.academicList;

                if ($scope.cfg.ASMAY_Id === promise.academicList[0].asmaY_Id) {
                }
                else {
                    $scope.academic = promise.academicList;
                    $scope.cfg.ASMAY_Id = promise.academicList[0].asmaY_Id;

                }

                // $scope.ASMAY_Id = $scope.academicList[0].asmaY_Id;

                $scope.trust = promise.trustList;
                $scope.institution = promise.instituteList;
                $scope.arrlist2 = promise.studentlicategorylist;
                $scope.schoolorClg = promise.mI_SchoolCollegeFlag;
             
                $scope.masterConfigList = promise.masterConfigList;

                $scope.presentCountgrid = promise.masterConfigList.length;
                // for pagination 
                $scope.currentPage = 1;
                $scope.itemsPerPage = 5;
                // for pagination
            })
            $scope.sort = function (keyname) {
                $scope.sortKey = keyname;   //set the sortKey to the param passed
                $scope.reverse = !$scope.reverse; //if true make it false and vice versa
            }

            //$scope.search = '';
            //$scope.searchconfig = function (user) {
            //    return (trst.mO_Name + inst.mI_Name + ay.asmaY_Year).indexOf($scope.search) >= 0;
            //};
        }
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }

        $scope.sortKey = "ispaC_Id";   //set the sortKey to the param passed
        $scope.reverse = true; //if true make it false and vice versa

        $scope.presentCountgrid = 0;
        //

        $scope.clearfun = function () {
            $state.reload();
        }

        $scope.binddata = function (position, arrlist2) {
            if ($scope.schoolorClg == 'S') {
                angular.forEach(arrlist2, function (option, index) {
                    if (position != index)
                        option.AMC_Id = false;
                });
            }
            else {
                angular.forEach(arrlist2, function (option, index) {
                    if (position != index)
                        option.AMCOC_Id = false;
                });
            }
           
        }

        $scope.catlist = [];
        $scope.edit = function (id, arrlist2) {
            $scope.disonedit = true;
            apiService.getURI("StudentMasterConfiguration/geteditdata", id).then(function (promise) {

                for (var i = 0; i < arrlist2.length; i++) {
                    name = arrlist2[i].amC_Id
                    if (name == promise.mstConfigList[0].amC_Id) {
                        arrlist2[i].AMC_Id = true
                    }
                }


                $scope.academic = promise.academicList;
                $scope.cfg.ASMAY_Id = promise.academicList[0].asmaY_Id;
                $scope.cfg.ispaC_Id = promise.mstConfigList[0].ispaC_Id;
                $scope.cfg.mI_Id = promise.mstConfigList[0].mI_Id;
                // $scope.cfg.asmaY_Id = promise.mstConfigList[0].asmaY_Id;
                $scope.cfg.mO_Id = promise.mstConfigList[0].mO_Id;

                $scope.cfg.ISPAC_EnquiryApplFlag = promise.mstConfigList[0].ispaC_EnquiryApplFlag;
                $scope.cfg.ISPAC_ApplnDownladFlag = promise.mstConfigList[0].ispaC_ApplnDownladFlag;
                $scope.cfg.ISPAC_ApplFeeFlag = promise.mstConfigList[0].ispaC_ApplFeeFlag;
                $scope.cfg.ISPAC_ApplIssueFlag = promise.mstConfigList[0].ispaC_ApplIssueFlag;
                $scope.cfg.ISPAC_EnqSMSFlag = promise.mstConfigList[0].ispaC_EnqSMSFlag;
                $scope.cfg.ISPAC_EnqMailFlag = promise.mstConfigList[0].ispaC_EnqMailFlag;
                $scope.cfg.ISPAC_EnqMailBackground = promise.mstConfigList[0].ispaC_EnqMailBackground;
                $scope.cfg.ISPAC_NoofApplications = promise.mstConfigList[0].ispaC_NoofApplications;

                if (promise.mstConfigList[0].ispaC_ApplNoIncrementBy == "" || promise.mstConfigList[0].ispaC_ApplNoIncrementBy == 0 || promise.mstConfigList[0].ispaC_ApplNoIncrementBy == null) {
                    $scope.cfg.ISPAC_ApplNoIncrementBy = 0;
                    $scope.cfg.ISPAC_ApplNoIncrementFlg = 0;
                }
                else {
                    $scope.cfg.ISPAC_ApplNoIncrementBy = promise.mstConfigList[0].ispaC_ApplNoIncrementBy;
                    $scope.cfg.ISPAC_ApplNoIncrementFlg = 1;
                }

                $scope.cfg.ISPAC_RegSMSFlag = promise.mstConfigList[0].ispaC_RegSMSFlag;
                $scope.cfg.ISPAC_RegMailFlag = promise.mstConfigList[0].ispaC_RegMailFlag;
                $scope.cfg.ISPAC_SeatBlockFlag = promise.mstConfigList[0].ispaC_SeatBlockFlag;

                $scope.cfg.ISPAC_RegFeeFlag = promise.mstConfigList[0].ispaC_RegFeeFlag;
                $scope.cfg.ISPAC_ApplSMSFlag = promise.mstConfigList[0].ispaC_ApplSMSFlag;
                $scope.cfg.ISPAC_ApplMailFlag = promise.mstConfigList[0].ispaC_ApplMailFlag;
                $scope.cfg.ISPAC_Healthapp = promise.mstConfigList[0].ispaC_Healthapp;
                $scope.cfg.ISPAC_ApplMailBackground = promise.mstConfigList[0].ispaC_ApplMailBackground;
                $scope.cfg.ISPAC_OralMarksFlag = promise.mstConfigList[0].ispaC_OralMarksFlag;
                $scope.cfg.ISPAC_DOBMinAgeFlag = promise.mstConfigList[0].ispaC_DOBMinAgeFlag;
                $scope.cfg.ISPAC_ApplCutOffDateFlag = promise.mstConfigList[0].ispaC_ApplCutOffDateFlag;
                $scope.cfg.ISPAC_DOBMaxAgeFlag = promise.mstConfigList[0].ispaC_DOBMaxAgeFlag;
                $scope.cfg.ISPAC_ApplNoGenFlag = promise.mstConfigList[0].ispaC_ApplNoGenFlag;
                $scope.cfg.ISPAC_EnquiryNoGenFlag = promise.mstConfigList[0].ispaC_EnquiryNoGenFlag;
                $scope.cfg.ISPAC_RgNoGenFlag = promise.mstConfigList[0].ispaC_RgNoGenFlag;
                $scope.cfg.ISPAC_HostelFlag = promise.mstConfigList[0].ispaC_HostelFlag;
                $scope.cfg.ISPAC_TransaportFlag = promise.mstConfigList[0].ispaC_TransaportFlag;
                $scope.cfg.ISPAC_SibblingConcessionFlag = promise.mstConfigList[0].ispaC_SibblingConcessionFlag;
                $scope.cfg.ISPAC_ParentConcessionFlag = promise.mstConfigList[0].ispaC_ParentConcessionFlag;
                $scope.cfg.ISPAC_ECSFlag = promise.mstConfigList[0].ispaC_ECSFlag;
                $scope.cfg.ISPAC_GymFlag = promise.mstConfigList[0].ispaC_GymFlag;
                $scope.cfg.ISPAC_FatherAliveFlag = promise.mstConfigList[0].ispaC_FatherAliveFlag;
                $scope.cfg.ISPAC_MotherAliveFlag = promise.mstConfigList[0].ispaC_MotherAliveFlag;
                $scope.cfg.ISPAC_MaritalStatusFlag = promise.mstConfigList[0].ispaC_MaritalStatusFlag;
                $scope.cfg.ISPAC_CommonScheduleFlag = promise.mstConfigList[0].ispaC_CommonScheduleFlag;

                $scope.cfg.ISPAC_WrittenTestSchApplFlag = promise.mstConfigList[0].ispaC_WrittenTestSchApplFlag;
                $scope.cfg.ISPAC_OralTestSchApplFlag = promise.mstConfigList[0].ispaC_OralTestSchApplFlag;
                $scope.cfg.ispaC_AdmCategoryFlag = promise.mstConfigList[0].ispaC_AdmCategoryFlag;
                $scope.cfg.ispaC_WrittenTestApplFlag = promise.mstConfigList[0].ispaC_WrittenTestApplFlag;
                $scope.cfg.ispaC_OralTestApplFlag = promise.mstConfigList[0].ispaC_OralTestApplFlag;
                $scope.cfg.ispaC_MarksEntry = promise.mstConfigList[0].ispaC_MarksEntry;
                $scope.cfg.ISPAC_AdmissionTransfer = promise.mstConfigList[0].ispaC_AdmissionTransfer;
                $scope.cfg.ISPAC_FeePaymentFlag = promise.mstConfigList[0].ispaC_FeePaymentFlag;
                $scope.cfg.ispaC_SeatBlockFlag = promise.mstConfigList[0].ispaC_SeatBlockFlag;
                $scope.cfg.ISPAC_OralByMax_Marks = promise.mstConfigList[0].ispaC_OralByMax_Marks;
                $scope.cfg.ISPAC_DefaultStatusFlag = promise.mstConfigList[0].ispaC_DefaultStatusFlag;
                $scope.cfg.ISPAC_OralTestBy = promise.mstConfigList[0].ispaC_OralTestBy;
                $scope.cfg.ISPAC_Transfer_Settings_Flag = promise.mstConfigList[0].ispaC_Transfer_Settings_Flag;
                $scope.cfg.ISPAC_OfflineFee_Flag = promise.mstConfigList[0].ispaC_OfflineFee_Flag;
                $scope.cfg.ISPAC_Transfer_Settings_after_payment_Flag = promise.mstConfigList[0].ispaC_Transfer_Settings_after_payment_Flag;
                $scope.cfg.ISPAC_ProspectusFlag = promise.mstConfigList[0].ispaC_ProspectusFlag;
                $scope.cfg.ISPAC_ProsptFeeApp = promise.mstConfigList[0].ispaC_ProsptFeeApp;

                $scope.scroll();

            })
        }

        $scope.delete = function (id, SweetAlert) {
            // swal(id);
            swal({
                title: "Are you sure?",
                text: "Do you want to delete record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, delete it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.DeleteURI("StudentMasterConfiguration/deletedetails", id).
                            then(function (promise) {
                                swal('Record Deleted Successfully..!', 'success');
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record Deletion Cancelled", "Failed");
                    }
                });
        }

        $scope.interacted = function (field) {
            // swal(field);

            return $scope.submitted || field.$dirty;
        };
        // Save master config
        $scope.submitted = false;
        $scope.SaveMasterConfig = function (arrlist2) {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {

                var catid;
                if ($scope.schoolorClg == 'S') {
                    for (var i = 0; i < arrlist2.length; i++) {
                        name = arrlist2[i].AMC_Id
                        if (name == "true") {
                            catid = arrlist2[i].amC_Id
                        }
                    }
                }
                else {
                    for (var i = 0; i < arrlist2.length; i++) {
                        name = arrlist2[i].AMCOC_Id
                        if (name == "true") {
                            catid = arrlist2[i].amcoC_Id
                        }
                    }
                }
             

              

                if ($scope.cfg.ISPAC_EnquiryApplFlag == false) {
                    $scope.cfg.ISPAC_EnqSMSFlag = 0;
                    $scope.cfg.ISPAC_EnqMailFlag = 0;
                }






                if ($scope.cfg.ISPAC_OralTestSchApplFlag == false) {
                    $scope.cfg.ISPAC_OralMarksFlag = 0;
                    // $scope.cfg.ISPAC_EnqMailFlag == 0;
                }

                if ($scope.cfg.ISPAC_NoofApplications == "") {
                    $scope.cfg.ISPAC_NoofApplications = 1;
                }

                if ($scope.cfg.ISPAC_ApplNoIncrementBy == "" || $scope.cfg.ISPAC_ApplNoIncrementBy == 0 || $scope.cfg.ISPAC_ApplNoIncrementBy == null) {
                    $scope.cfg.ISPAC_ApplNoIncrementBy = 0;
                    $scope.cfg.ISPAC_ApplNoIncrementFlg = 0;
                }
                else {
                    $scope.cfg.ISPAC_ApplNoIncrementFlg = 1;
                }
                
                var ddd = $scope.yearid;
                var data = {
                    "AMC_Id": catid,
                    "ISPAC_Id": $scope.cfg.ispaC_Id,
                    "MI_Id": $scope.cfg.mI_Id,
                    "ASMAY_Id": $scope.cfg.ASMAY_Id,
                    "mO_Id": $scope.cfg.mO_Id,
                    "ISPAC_ApplnDownladFlag": $scope.cfg.ISPAC_ApplnDownladFlag,
                    "ISPAC_EnquiryApplFlag": $scope.cfg.ISPAC_EnquiryApplFlag,
                    "ISPAC_EnqSMSFlag": $scope.cfg.ISPAC_EnqSMSFlag,
                    "ISPAC_EnqMailFlag": $scope.cfg.ISPAC_EnqMailFlag,
                    "ISPAC_EnqMailBackground": $scope.cfg.ISPAC_EnqMailBackground,
                    "ISPAC_DOBMinAgeFlag": $scope.cfg.ISPAC_DOBMinAgeFlag,
                    "ISPAC_ApplCutOffDateFlag": $scope.cfg.ISPAC_ApplCutOffDateFlag,
                    "ISPAC_DOBMaxAgeFlag": $scope.cfg.ISPAC_DOBMaxAgeFlag,
                    "ISPAC_OralMarksFlag": $scope.cfg.ISPAC_OralMarksFlag,
                    "ISPAC_NoofApplications": $scope.cfg.ISPAC_NoofApplications,

                    "ISPAC_ApplSMSFlag": $scope.cfg.ISPAC_ApplSMSFlag,
                    "ISPAC_ApplMailFlag": $scope.cfg.ISPAC_ApplMailFlag,
                    "ISPAC_Healthapp": $scope.cfg.ISPAC_Healthapp,
                    "ISPAC_ApplMailBackground": $scope.cfg.ISPAC_ApplMailBackground,
                    "ISPAC_WrittenTestSchApplFlag": $scope.cfg.ISPAC_WrittenTestSchApplFlag,
                    "ISPAC_CommonScheduleFlag": $scope.cfg.ISPAC_CommonScheduleFlag,
                    "ISPAC_RegSMSFlag": $scope.cfg.ISPAC_RegSMSFlag,
                    "ISPAC_RegMailFlag": $scope.cfg.ISPAC_RegMailFlag,
                    "ISPAC_RegMailBackground": $scope.cfg.ISPAC_EnqMailBackground,
                    "ISPAC_ApplNoGenFlag": $scope.cfg.ISPAC_ApplNoGenFlag,
                    "ISPAC_EnquiryNoGenFlag": $scope.cfg.ISPAC_EnquiryNoGenFlag,
                    "ISPAC_RgNoGenFlag": $scope.cfg.ISPAC_RgNoGenFlag,
                    "ISPAC_HostelFlag": $scope.cfg.ISPAC_HostelFlag,
                    "ISPAC_TransaportFlag": $scope.cfg.ISPAC_TransaportFlag, // Added on 8-11-2016
                    "ISPAC_SibblingConcessionFlag": $scope.cfg.ISPAC_SibblingConcessionFlag,
                    "ISPAC_ParentConcessionFlag": $scope.cfg.ISPAC_ParentConcessionFlag,
                    "ISPAC_ECSFlag": $scope.cfg.ISPAC_ECSFlag,
                    "ISPAC_GymFlag": $scope.cfg.ISPAC_GymFlag,
                    "ISPAC_FatherAliveFlag": $scope.cfg.ISPAC_FatherAliveFlag,
                    "ISPAC_MotherAliveFlag": $scope.cfg.ISPAC_MotherAliveFlag,
                    "ISPAC_MaritalStatusFlag": $scope.cfg.ISPAC_MaritalStatusFlag,
                    "ISPAC_OralTestSchApplFlag": $scope.cfg.ISPAC_OralTestSchApplFlag,
                    "ISPAC_AdmCategoryFlag": $scope.cfg.ispaC_AdmCategoryFlag,
                    "ISPAC_RegFeeFlag": $scope.cfg.ISPAC_RegFeeFlag,
                    //"ISPAC_WrittenTestApplFlag": $scope.cfg.ispaC_WrittenTestApplFlag,
                    // "ISPAC_OralTestApplFlag": $scope.cfg.ispaC_OralTestApplFlag,
                    "ISPAC_MarksEntry": $scope.cfg.ispaC_MarksEntry,
                    "ISPAC_AdmissionTransfer": $scope.cfg.ISPAC_AdmissionTransfer,
                    "ISPAC_FeePaymentFlag": $scope.cfg.ISPAC_FeePaymentFlag,

                    "ISPAC_OralByMax_Marks": $scope.cfg.ISPAC_OralByMax_Marks,
                    "ISPAC_DefaultStatusFlag": $scope.cfg.ISPAC_DefaultStatusFlag,
                    "ISPAC_OralTestBy": $scope.cfg.ISPAC_OralTestBy,
                    "ISPAC_ApplIssueFlag": $scope.cfg.ISPAC_ApplIssueFlag,
                    "ISPAC_ApplFeeFlag": $scope.cfg.ISPAC_ApplFeeFlag,
                    "ISPAC_Transfer_Settings_Flag": $scope.cfg.ISPAC_Transfer_Settings_Flag,
                    "ISPAC_OfflineFee_Flag": $scope.cfg.ISPAC_OfflineFee_Flag,
                    "ISPAC_Transfer_Settings_after_payment_Flag": $scope.cfg.ISPAC_Transfer_Settings_after_payment_Flag,
                    "ISPAC_ProspectusFlag": $scope.cfg.ISPAC_ProspectusFlag,
                    "ISPAC_ProsptFeeApp": $scope.cfg.ISPAC_ProsptFeeApp,
                    "ISPAC_SeatBlockFlag": $scope.cfg.ISPAC_SeatBlockFlag,
                    "ISPAC_OralTestApplFlag": 1,
                    "ISPAC_WrittenTestApplFlag": 1,
                    "ISPAC_ApplNoIncrementFlg": $scope.cfg.ISPAC_ApplNoIncrementFlg,
                    "ISPAC_ApplNoIncrementBy": $scope.cfg.ISPAC_ApplNoIncrementBy
                }
                apiService.create("StudentMasterConfiguration/", data).then(function (promise) {
                    swal(promise.message);
                    // $scope.cfg = {};
                    //$scope.submitted = false;
                    //$scope.loadInitialDropdowns();
                    $scope.disonedit = false;
                    $state.reload();
                });
            }
        }


        $scope.enqchange = function (enq) {
            if (enq == false) {
                $scope.cfg.ISPAC_EnqSMSFlag = false;
                $scope.cfg.ISPAC_EnqMailFlag = false;
            }
        }


        $scope.regchange = function (enq1) {
            if (enq1 == false) {
                $scope.cfg.ISPAC_RegSMSFlag = false;
                $scope.cfg.ISPAC_RegMailFlag = false;
            }
        }

        $scope.oralchange = function (oral) {
            if (oral == false) {
                $scope.cfg.ISPAC_OralMarksFlag = false;

            }
        }
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

        $scope.IsHidden5 = true;
        $scope.ShowHide5 = function () {
            //If DIV is hidden it will be visible and vice versa.
            $scope.IsHidden5 = $scope.IsHidden5 ? false : true;
        }
    }
})();
