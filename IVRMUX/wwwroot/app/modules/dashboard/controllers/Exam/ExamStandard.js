(function () {
    'use strict';
    angular.module('app').controller('ExamStandardController', ExamStandardController)

    ExamStandardController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http']
    function ExamStandardController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http) {

        $scope.BindData = function () {
            apiService.getDATA("ExamStandard/Getdetails").then(function (promise) {
                if (promise.exm_config.length === 0) {
                    $scope.ExmConfig_EnableFractionFlg = true;
                    $scope.ExmConfig_NoOfDecimal = 2;
                    $scope.ExmConfig_RankingMethod = 'Dense';
                    $scope.ExmConfig_Remarks = 'General';
                    $scope.ExmConfig_Recordsearchtype = "";
                    $scope.ExmConfig_PassFailRankFlag = 0;
                    $scope.ExmConfig_AdmnoColumnDisplay = true;
                    $scope.ExmConfig_RegnoColumnDisplay = true;
                    $scope.ExmConfig_RollnoColumnDisplay = true;
                    $scope.ExmConfig_EntryDateRestFlg = true;
                    $scope.ExmConfig_PromotionFlag = false;
                    $scope.ExmConfig_GraceAplFlg = false;
                    $scope.ExmConfig_BonusAplFlag = false;
                    $scope.ExmConfig_MinAttAplFlag = false;
                    $scope.ExmConfig_MarksMultiply = false;
                    $scope.ExmConfig_GroupMarksToResultFlg = false;
                    $scope.ExmConfig_Id = 0;
                    $scope.ExmConfig_RoundoffFlag = false;
                    $scope.ExmConfig_NoOfDecimalValues = "";
                    $scope.ExmConfig_FailBoldFlg = false;
                    $scope.ExmConfig_FailItalicFlg = false;
                    $scope.ExmConfig_FailUnderscoreFlg = false;
                    $scope.ExmConfig_FailColorFlg = "";
                    $scope.ExmConfig_AllSubjectAbsentFlg = false;
                    $scope.ExmConfig_FeeDefaulterDisplayFlg = false;

                    $scope.ExmConfig_ClassRankFlg = false;
                    $scope.ExmConfig_ClassPositionFlg = false;
                    $scope.ExmConfig_FailRankFlg = false;
                    $scope.ExmConfig_SecRankFlg = false;
                    $scope.ExmConfig_SectionPositionFlg = false;
                    $scope.ExmConfig_GroupwiseMarksFlg = false;
                    $scope.ExmConfig_SubjectwiseSecRankFlg = false;
                    $scope.ExmConfig_SubjectwiseClassRankFlg = false;
                    

                }
                else if (promise.exm_config.length > 0) {
                    $scope.ExmConfig_Id = promise.ExmConfig_Id;
                    $scope.ExmConfig_RankingMethod = promise.ExmConfig_RankingMethod;
                    $scope.ExmConfig_PromotionFlag = promise.ExmConfig_PromotionFlag;
                    $scope.ExmConfig_PassFailRankFlag = promise.ExmConfig_PassFailRankFlag;
                    $scope.ExmConfig_Recordsearchtype = promise.ExmConfig_Recordsearchtype;
                    $scope.ExmConfig_Remarks = promise.ExmConfig_Remarks;
                    $scope.ExmConfig_GraceAplFlg = promise.ExmConfig_GraceAplFlg;
                    $scope.ExmConfig_BonusAplFlag = promise.ExmConfig_BonusAplFlag;
                    $scope.ExmConfig_MinAttAplFlag = promise.ExmConfig_MinAttAplFlag;
                    $scope.ExmConfig_MarksMultiply = promise.ExmConfig_MarksMultiply;
                    $scope.ExmConfig_NoOfDecimal = promise.ExmConfig_NoOfDecimal;
                    $scope.ExmConfig_GroupMarksToResultFlg = promise.ExmConfig_GroupMarksToResultFlg;
                    $scope.ExmConfig_EnableFractionFlg = promise.ExmConfig_EnableFractionFlg;
                    $scope.ExmConfig_RoundoffFlag = promise.ExmConfig_RoundoffFlag;
                    $scope.ExmConfig_PerRoundoffFlag = promise.ExmConfig_PerRoundoffFlag;
                    $scope.ExmConfig_NoOfDecimalValues = promise.ExmConfig_NoOfDecimalValues;
                    if ($scope.ExmConfig_EnableFractionFlg == 0) {
                        $scope.ExmConfig_NoOfDecimal = "";
                    }
                    if ($scope.ExmConfig_RoundoffFlag == 0) {
                        $scope.ExmConfig_NoOfDecimalValues = "";
                    }
                    $scope.ExmConfig_EntryDateRestFlg = promise.ExmConfig_EntryDateRestFlg;
                    $scope.ExmConfig_AdmnoColumnDisplay = promise.ExmConfig_AdmnoColumnDisplay;
                    $scope.ExmConfig_RegnoColumnDisplay = promise.ExmConfig_RegnoColumnDisplay;
                    $scope.ExmConfig_RollnoColumnDisplay = promise.ExmConfig_RollnoColumnDisplay;
                    $scope.ExmConfig_HallTicketFlg = promise.ExmConfig_HallTicketFlg;
                    $scope.ExmConfig_FailBoldFlg = promise.ExmConfig_FailBoldFlg;
                    $scope.ExmConfig_FailItalicFlg = promise.ExmConfig_FailItalicFlg;
                    $scope.ExmConfig_FailUnderscoreFlg = promise.ExmConfig_FailUnderscoreFlg;
                    $scope.ExmConfig_FailColorFlg = promise.ExmConfig_FailColorFlg;
                    $scope.ExmConfig_AllSubjectAbsentFlg = promise.ExmConfig_AllSubjectAbsentFlg;
                    $scope.ExmConfig_FeeDefaulterDisplayFlg = promise.ExmConfig_FeeDefaulterDisplayFlg;
                    $scope.ExmConfig_AdmNoRegNoRollNo_DefaultFlag = promise.ExmConfig_AdmNoRegNoRollNo_DefaultFlag;

                    $scope.ExmConfig_ClassRankFlg = promise.ExmConfig_ClassRankFlg;
                    $scope.ExmConfig_ClassPositionFlg = promise.ExmConfig_ClassPositionFlg;
                    $scope.ExmConfig_FailRankFlg = promise.ExmConfig_FailRankFlg;
                    $scope.ExmConfig_SecRankFlg = promise.ExmConfig_SecRankFlg;
                    $scope.ExmConfig_SectionPositionFlg = promise.ExmConfig_SectionPositionFlg;
                    $scope.ExmConfig_GroupwiseMarksFlg = promise.ExmConfig_GroupwiseMarksFlg;
                    $scope.ExmConfig_SubjectwiseSecRankFlg = promise.ExmConfig_SubjectwiseSecRankFlg;
                    $scope.ExmConfig_SubjectwiseClassRankFlg = promise.ExmConfig_SubjectwiseClassRankFlg;
                }
            });
        };


        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };
        $scope.reset = function () {
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
            $state.reload();
        };

        // TO Save The Data
        $scope.submitted = false;
        $scope.saveddata = function () {
            $scope.submitted = true;
            if ($scope.ExmConfig_EnableFractionFlg == 0) {
                $scope.ExmConfig_NoOfDecimal = 0;
            }
            if ($scope.ExmConfig_RoundoffFlag == 0) {
                $scope.ExmConfig_NoOfDecimalValues = 0;
            }
            if ($scope.myForm.$valid) {
                var data = {
                    "ExmConfig_Id": $scope.ExmConfig_Id,
                    "ExmConfig_RankingMethod": $scope.ExmConfig_RankingMethod,
                    "ExmConfig_PromotionFlag": $scope.ExmConfig_PromotionFlag,
                    "ExmConfig_PassFailRankFlag": $scope.ExmConfig_PassFailRankFlag,
                    "ExmConfig_Recordsearchtype": $scope.ExmConfig_Recordsearchtype,
                    "ExmConfig_Remarks": $scope.ExmConfig_Remarks,
                    "ExmConfig_GraceAplFlg": $scope.ExmConfig_GraceAplFlg,
                    "ExmConfig_BonusAplFlag": $scope.ExmConfig_BonusAplFlag,
                    "ExmConfig_MinAttAplFlag": $scope.ExmConfig_MinAttAplFlag,
                    "ExmConfig_MarksMultiply": $scope.ExmConfig_MarksMultiply,
                    "ExmConfig_NoOfDecimal": $scope.ExmConfig_NoOfDecimal,
                    "ExmConfig_GroupMarksToResultFlg": $scope.ExmConfig_GroupMarksToResultFlg,
                    "ExmConfig_EnableFractionFlg": $scope.ExmConfig_EnableFractionFlg,
                    "ExmConfig_EntryDateRestFlg": $scope.ExmConfig_EntryDateRestFlg,
                    "ExmConfig_AdmnoColumnDisplay": $scope.ExmConfig_AdmnoColumnDisplay,
                    "ExmConfig_RegnoColumnDisplay": $scope.ExmConfig_RegnoColumnDisplay,
                    "ExmConfig_RollnoColumnDisplay": $scope.ExmConfig_RollnoColumnDisplay,
                    "ExmConfig_RoundoffFlag": $scope.ExmConfig_RoundoffFlag,
                    "ExmConfig_NoOfDecimalValues": $scope.ExmConfig_NoOfDecimalValues,
                    "ExmConfig_PerRoundoffFlag": $scope.ExmConfig_PerRoundoffFlag,
                    "ExmConfig_HallTicketFlg": $scope.ExmConfig_HallTicketFlg,

                    "ExmConfig_FailBoldFlg": $scope.ExmConfig_FailBoldFlg,
                    "ExmConfig_FailItalicFlg": $scope.ExmConfig_FailItalicFlg,
                    "ExmConfig_FailUnderscoreFlg": $scope.ExmConfig_FailUnderscoreFlg,
                    "ExmConfig_FailColorFlg": $scope.ExmConfig_FailColorFlg,
                    "ExmConfig_AllSubjectAbsentFlg": $scope.ExmConfig_AllSubjectAbsentFlg,
                    "ExmConfig_FeeDefaulterDisplayFlg": $scope.ExmConfig_FeeDefaulterDisplayFlg,
                    "ExmConfig_AdmNoRegNoRollNo_DefaultFlag": $scope.ExmConfig_AdmNoRegNoRollNo_DefaultFlag, 

                    "ExmConfig_ClassRankFlg" : $scope.ExmConfig_ClassRankFlg,
                    "ExmConfig_ClassPositionFlg":$scope.ExmConfig_ClassPositionFlg,
                    "ExmConfig_FailRankFlg":$scope.ExmConfig_FailRankFlg ,
                    "ExmConfig_SecRankFlg":$scope.ExmConfig_SecRankFlg,
                    "ExmConfig_SectionPositionFlg": $scope.ExmConfig_SectionPositionFlg,
                    "ExmConfig_GroupwiseMarksFlg": $scope.ExmConfig_GroupwiseMarksFlg,
                    "ExmConfig_SubjectwiseClassRankFlg": $scope.ExmConfig_SubjectwiseClassRankFlg,
                    "ExmConfig_SubjectwiseSecRankFlg": $scope.ExmConfig_SubjectwiseSecRankFlg
                };
                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                };
                apiService.create("ExamStandard/savedetails", data).then(function (promise) {
                    if (promise.returnval === true) {
                        if (promise.exmConfig_Id === 0 || promise.exmConfig_Id < 0) {
                            swal('Record saved successfully');
                        }
                        else if (promise.exmConfig_Id > 0) {
                            swal('Record updated successfully');
                        }
                    }
                    else {
                        if (promise.exmConfig_Id === 0 || promise.exmConfig_Id < 0) {
                            swal('Failed to save, please contact administrator');
                        }
                        else if (promise.exmConfig_Id > 0) {
                            swal('Failed to update, please contact administrator');
                        }
                    }
                    $scope.reset();
                });
            }
            else {
                $scope.submitted = true;
            }
        };
    }

})();