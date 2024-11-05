(function () {
    'use strict';
    angular.module('app').controller('CollegeAdmissionStandardController', CollegeAdmissionStandardController)

    CollegeAdmissionStandardController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function CollegeAdmissionStandardController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.pagesrecord = {};       

        var studclear = [];

        var ascid;

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        ///////////////////////////////////////Saving Functionlity///////////////////////////////////////////////////////////////////////////
        $scope.savedata = function (ascid) {
            if ($scope.myForm.$valid) {
                if ($scope.idvalue != null) {
                    ascid = $scope.idvalue;
                }
                else {
                    ascid = 0;
                }

                if ($scope.ASC_Adm_AddFieldsFlag == true) {
                    $scope.ASC_Adm_AddFieldsFlag = 1;
                }
                else {
                    $scope.ASC_Adm_AddFieldsFlag = 0;
                }
                if ($scope.ASC_TC_AddFieldsFlag == true) {
                    $scope.ASC_TC_AddFieldsFlag = 1;
                }
                else {
                    $scope.ASC_TC_AddFieldsFlag = 0;
                }
                if ($scope.ASC_MaxAgeApl_Flag == true) {
                    $scope.ASC_MaxAgeApl_Flag = 1;
                }
                else {
                    $scope.ASC_MaxAgeApl_Flag = 0;
                }
                if ($scope.ASC_MinAgeApl_Flag == true) {
                    $scope.ASC_MinAgeApl_Flag = 1;
                }
                else {
                    $scope.ASC_MinAgeApl_Flag = 0;
                }

                if ($scope.ASC_TC_Payment == true) {
                    $scope.ASC_TC_Payment = 1;
                }
                else {
                    $scope.ASC_TC_Payment = 0;
                }

                var ASC_ECS_Flag_New = 0;
                if ($scope.ASC_ECS_Flag == true) {
                    ASC_ECS_Flag_New = 1;
                }
                else {
                    ASC_ECS_Flag_New = 0;
                }

                if ($scope.ASC_Att_DefaultEntry_Type == 'A' || $scope.ASC_Att_DefaultEntry_Type == 'P') {
                    //dd
                }
                else {
                    $scope.ASC_Att_DefaultEntry_Type = 0;
                }

                if ($scope.ASC_Default_Gender == 'M' || $scope.ASC_Default_Gender == 'F' || $scope.ASC_Default_Gender == 'Ot') {
                    //dd
                }
                else {
                    $scope.ASC_Default_Gender = 0;
                }
                if ($scope.ASC_DefaultSMS_Flag == 'F') {
                    $scope.ASC_DefaultSMS_Flag = 'F';
                }
                else
                    if ($scope.ASC_DefaultSMS_Flag == 'M') {
                        $scope.ASC_DefaultSMS_Flag = 'M';
                    }
                    else
                        if ($scope.ASC_DefaultSMS_Flag == 'S') {
                            $scope.ASC_DefaultSMS_Flag = 'S';
                        }
                        else {
                            $scope.ASC_DefaultSMS_Flag = 0;
                        }

                if ($scope.ASC_DefaultPhotoUpload == 1 || $scope.ASC_DefaultPhotoUpload == 2) {
                    //dd
                }
                else {
                    $scope.ASC_DefaultPhotoUpload = 0;
                }



                if ($scope.ASC_ParentsMonthlyIncome_Flag == true) {
                    $scope.ASC_ParentsMonthlyIncome_Flag = 1;
                }
                else {
                    $scope.ASC_ParentsMonthlyIncome_Flag = 0;
                }


                if ($scope.ASC_ParentsAnnualIncome_Flag == true) {
                    $scope.ASC_ParentsAnnualIncome_Flag = 1;
                }
                else {
                    $scope.ASC_ParentsAnnualIncome_Flag = 0;
                }



                if ($scope.ASC_School_Address == true) {
                    $scope.ASC_School_Address = 1;
                }
                else {
                    $scope.ASC_School_Address = 0;
                }



                if ($scope.ASC_Category_Address == true) {
                    $scope.ASC_Category_Address = 1;
                }
                else {
                    $scope.ASC_Category_Address = 0;
                }

                if ($scope.ASC_Att_Default_OrderFlag != 0 || $scope.ASC_Att_Default_OrderFlag != null) {
                    //dd
                }
                else {
                    $scope.ASC_Att_Default_OrderFlag = 0;
                }

                var data = {
                    "ASC_Adm_AddFieldsFlag": $scope.ASC_Adm_AddFieldsFlag,
                    "ASC_TC_AddFieldsFlag": $scope.ASC_TC_AddFieldsFlag,
                    "ASC_MaxAgeApl_Flag": $scope.ASC_MaxAgeApl_Flag,
                    "ASC_MinAgeApl_Flag": $scope.ASC_MinAgeApl_Flag,
                    "ASC_Att_DefaultEntry_Type": $scope.ASC_Att_DefaultEntry_Type,
                    "ASC_Default_Gender": $scope.ASC_Default_Gender,
                    "ASC_ParentsMonthlyIncome_Flag": $scope.ASC_ParentsMonthlyIncome_Flag,
                    "ASC_ParentsAnnualIncome_Flag": $scope.ASC_ParentsAnnualIncome_Flag,
                    "ASC_School_Address": $scope.ASC_School_Address,
                    "ASC_Category_Address": $scope.ASC_Category_Address,
                    "ASC_DefaultPhotoUpload": $scope.ASC_DefaultPhotoUpload,
                    "ASC_Id": $scope.ascid,
                    "ASC_Att_Default_OrderFlag": $scope.ASC_Att_Default_OrderFlag,
                    "ASC_Stu_Photo_Path": $scope.ASC_Stu_Photo_Path,
                    "ASC_Staff_Photo_Path": $scope.ASC_Staff_Photo_Path,
                    "ASC_Logo_Path": $scope.ASC_Logo_Path,
                    "ASC_Doc_Path": $scope.ASC_Doc_Path,
                    "ASC_DefaultSMS_Flag": $scope.ASC_DefaultSMS_Flag,
                    "ASC_TC_Payment": $scope.ASC_TC_Payment,
                    "ASC_ECS_Flag": ASC_ECS_Flag_New,
                    "ASC_Att_Scheduler_Flag": $scope.ASC_Att_Scheduler_Flag
                };

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };

                apiService.create("CollegeAdmissionStandard/savedata/", data).then(function (promise) {
                    if (promise.returnval == true) {
                        swal('Record Saved Successfully', 'success');
                        $state.reload();
                    }
                    else {
                        //swal('Record Successfully Updated');
                    }
                    // $scope.loaddata();
                });

            }
            else {
                $scope.submitted = true;
            }
        };

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        $scope.loaddata = function () {

            var data = 2;

            apiService.getURI("CollegeAdmissionStandard/loaddata/", data).then(function (promise) {

                if (promise.fillconfig.length > 0) {
                    $scope.cancelDis = true;
                    $scope.reset = false;
                    $scope.idvalue = promise.fillconfig[0].asC_Id;
                    if (promise.fillconfig[0].asC_Adm_AddFieldsFlag == 1) {
                        $scope.ASC_Adm_AddFieldsFlag = 1;
                    }
                    else {
                        $scope.ASC_Adm_AddFieldsFlag = 0;
                    }

                    if (promise.fillconfig[0].asC_TC_AddFieldsFlag == 1) {
                        $scope.ASC_TC_AddFieldsFlag = 1;
                    }
                    else {
                        $scope.ASC_TC_AddFieldsFlag = 0;
                    }
                    if (promise.fillconfig[0].asC_MaxAgeApl_Flag == 1) {
                        $scope.ASC_MaxAgeApl_Flag = 1;
                    }
                    else {
                        $scope.ASC_MaxAgeApl_Flag = 0;
                    }
                    if (promise.fillconfig[0].asC_MinAgeApl_Flag == 1) {
                        $scope.ASC_MinAgeApl_Flag = 1;
                    }
                    else {
                        $scope.ASC_MinAgeApl_Flag = 0;
                    }

                    if (promise.fillconfig[0].admC_TCAllowBalanceFlg == 1) {
                        $scope.ASC_TC_Payment = 1;
                    }
                    else {
                        $scope.ASC_TC_Payment = 0;
                    }

                    if (promise.fillconfig[0].asC_ECS_Flag == 1) {
                        $scope.ASC_ECS_Flag = 1;
                    }
                    else {
                        $scope.ASC_ECS_Flag = 0;
                    }

                    if (promise.fillconfig[0].asC_Att_DefaultEntry_Type == 'P') {
                        $scope.ASC_Att_DefaultEntry_Type = 'P';
                    }
                    else
                        if (promise.fillconfig[0].asC_Att_DefaultEntry_Type == 'A') {
                            $scope.ASC_Att_DefaultEntry_Type = 'A';
                        }

                    if (promise.fillconfig[0].asC_Default_Gender == 'M') {
                        $scope.ASC_Default_Gender = 'M';
                    }
                    else
                        if (promise.fillconfig[0].asC_Default_Gender == 'F') {
                            $scope.ASC_Default_Gender = 'F';
                        }
                        else
                            if (promise.fillconfig[0].asC_Default_Gender == 'Ot') {
                                $scope.ASC_Default_Gender = 'Ot';
                            }
                    if (promise.fillconfig[0].asC_DefaultSMS_Flag == 'F') {
                        $scope.ASC_DefaultSMS_Flag = 'F';
                    }
                    else
                        if (promise.fillconfig[0].asC_DefaultSMS_Flag == 'M') {
                            $scope.ASC_DefaultSMS_Flag = 'M';
                        }
                        else
                            if (promise.fillconfig[0].asC_DefaultSMS_Flag == 'S') {
                                $scope.ASC_DefaultSMS_Flag = 'S';
                            }

                    if (promise.fillconfig[0].asC_ParentsMonthlyIncome_Flag == 1) {
                        $scope.ASC_ParentsMonthlyIncome_Flag = 1;
                    }
                    else {
                        $scope.ASC_ParentsMonthlyIncome_Flag = 0;
                    }

                    if (promise.fillconfig[0].asC_ParentsAnnualIncome_Flag == 1) {
                        $scope.ASC_ParentsAnnualIncome_Flag = 1;
                    }
                    else {
                        $scope.ASC_ParentsAnnualIncome_Flag = 0;
                    }
                    if (promise.fillconfig[0].asC_School_Address == 1) {
                        $scope.ASC_School_Address = 1;
                    }
                    else {
                        $scope.ASC_School_Address = 0;
                    }
                    if (promise.fillconfig[0].asC_Category_Address == 1) {
                        $scope.ASC_Category_Address = 1;
                    }
                    else {
                        $scope.ASC_Category_Address = 0;
                    }

                    if (promise.fillconfig[0].asC_DefaultPhotoUpload == "1" || promise.fillconfig[0].asC_DefaultPhotoUpload == "2") {
                        $scope.ASC_DefaultPhotoUpload = promise.fillconfig[0].asC_DefaultPhotoUpload;
                    }
                    else {
                        $scope.ASC_DefaultPhotoUpload = "0";
                    }
                    if (promise.fillconfig[0].asC_Att_Default_OrderFlag != 0) {
                        $scope.ASC_Att_Default_OrderFlag = promise.fillconfig[0].asC_Att_Default_OrderFlag;
                    }
                    else {
                        $scope.ASC_Att_Default_OrderFlag = 0;
                    }

                    $scope.ASC_Stu_Photo_Path = promise.fillconfig[0].asC_Stu_Photo_Path;
                    $scope.ASC_Staff_Photo_Path = promise.fillconfig[0].asC_Staff_Photo_Path;
                    $scope.ASC_Logo_Path = promise.fillconfig[0].asC_Logo_Path;
                    $scope.ASC_Doc_Path = promise.fillconfig[0].asC_Doc_Path;
                    $scope.ASC_Att_Scheduler_Flag = promise.fillconfig[0].asC_Att_Scheduler_Flag;
                    $scope.ascid = promise.fillconfig[0].asC_Id;
                }
                else {
                    $scope.cancelDis = false;
                    $scope.reset = true;
                }

            });
        };


        $scope.cancel = function () {
            $state.reload();
        };

        $scope.resetData = function () {
            $scope.loaddata();
        };
    }
})();

