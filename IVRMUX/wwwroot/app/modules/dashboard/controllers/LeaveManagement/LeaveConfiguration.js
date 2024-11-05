(function () {
    'use strict';
    angular
.module('app')
.controller('LeaveConfiguration', LeaveConfiguration)
    LeaveConfiguration.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter', 'uiGridConstants']
    function LeaveConfiguration($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter, $uiGridConstants) {

        $scope.loadData = function () {

            
           var id = 2;

            apiService.getURI("LeaveConfig/getSPName", id).
       then(function (promise) {
           


           if (promise.config_data != null) {
               $scope.policy_name = promise.config_data[0].hrlpC_LeavePolicyName;
               $scope.proc_name = promise.config_data[0].hrlpC_SPName;
               $scope.servic_name = promise.config_data[0].hrlpC_ServiceName;
               $scope.late_flag = promise.config_data[0].hrlpC_LateInFlag.toString();
              // $scope.Late_Time = moment(promise.config_data[0].hrlpC_LateInTime, 'h:mm a').format();
               $scope.Late_Time = promise.config_data[0].hrlpC_LateInTime;
               $scope.Early = promise.config_data[0].hrlpC_EarlyOutFlag.toString();
               //$scope.Early_Time = moment(promise.config_data[0].hrlpC_EarlyOutTime, 'h:mm a').format();
               $scope.numlate_flag = promise.config_data[0].hrlpC_NoOfLatesFag;
               $scope.Late_num = promise.config_data[0].hrlpC_NoOfLates;
               $scope.Early_Time = promise.config_data[0].hrlpC_EarlyOutTime;
               $scope.Time_flag = promise.config_data[0].hrlpC_CummulativeTimeFlag.toString();
               // $scope.cum_time = moment(promise.config_data[0].hrlpC_CummulativeTime, 'h:mm a').format();
               $scope.cum_time = promise.config_data[0].hrlpC_CummulativeTime;
               // $scope.Aftr_cum_time = moment(promise.config_data[0].hrlpC_AfterCummulativeTime, 'h:mm a').format();
               $scope.Aftr_cum_time = promise.config_data[0].hrlpC_AfterCummulativeTime;
               $scope.carry_flag = promise.config_data[0].hrlpC_NoOfLatesCFFlag.toString();
               $scope.sufix_flag = promise.config_data[0].hrlpC_LeavePrefixSuffixFlag.toString();
               $scope.holiday_inc_flag = promise.config_data[0].hrlpC_IncludeHolidayFlag.toString();

               if (promise.config_data[0].hrlpC_LateLeaveFlag == true) {
                   $scope.HRLPC_LateLeaveFlag = "leave";
               }
               else if (promise.config_data[0].hrlpC_LateLOPFlag == true) {
                   $scope.HRLPC_LateLOPFlag = "lop";
               }
               $scope.policy_id = promise.HRLPC_Id;

               $scope.sporgen_flag = promise.config_data[0].hrlpC_SpOrGen;

               if (promise.config_data[0].hrlpC_AbsentLeaveFlag == true) {
                   $scope.HRLPC_AbsentLeaveFlag = "leave";                   
               }
               else if (promise.config_data[0].hrlpC_AbsentLOPFlag == true) {                 
                   $scope.HRLPC_AbsentLeaveFlag = "lop";
               }
           }
           else {
               swal("Settings not Done !!");
           }

       })
        };

        $scope.radio_click = function (radioval) {
            $scope.selectedval = radioval;
        }

        $scope.clear1 = function () {
            $scope.policy_id = 0;
            $scope.policy_name = "";
            $scope.proc_name = "";
            $scope.servic_name = "";
            $scope.late_flag = "";
            $scope.Late_Time = "";
            $scope.Early = "";
            $scope.Early_Time = "";
            $scope.Time_flag = "";
            $scope.cum_time = "";
            $scope.Aftr_cum_time = "";
            $scope.carry_flag="";
            $scope.sufix_flag="";
            $scope.holiday_inc_flag = "";
            $scope.HRLPC_LateLeaveFlag = "";
            $scope.HRLPC_LateLOPFlag = "";
            $scope.Late_num = "";
        };
        $scope.submitted = false;
        $scope.interacted = function (field) {

            return $scope.submitted || field.$dirty;
        };
        $scope.submit = function () {
            
            if ($scope.myForm2.$valid) {
                if ($scope.selectedval == "lop") {
                    $scope.HRLPC_LateLOPFlag = true;
                    $scope.HRLPC_LateLeaveFlag = false;
                }
                else if ($scope.selectedval == "leave") {
                    $scope.HRLPC_LateLeaveFlag = true;
                    $scope.HRLPC_LateLOPFlag = false;
                }

                if ($scope.HRLPC_AbsentLeaveFlag == "lop") {
                    $scope.HRLPC_AbsentLOPFlag = true;
                    $scope.HRLPC_AbsentLeaveFlag = false;
                } else if ($scope.HRLPC_AbsentLeaveFlag == "leave") {
                    $scope.HRLPC_AbsentLOPFlag = false;
                    $scope.HRLPC_AbsentLeaveFlag = true;
                }
                
                var data = {
                    "HRLPC_LeavePolicyName": $scope.policy_name,
                    "HRLPC_SPName": $scope.proc_name,
                    "HRLPC_ServiceName": $scope.servic_name,
                    "HRLPC_LateInFlag": $scope.late_flag,
                    //"HRLPC_LateInTime": $filter('date')($scope.Late_Time, "HH:mm"),
                    "HRLPC_LateInTime": $scope.Late_Time,
                    "HRLPC_EarlyOutFlag": $scope.Early,
                    //"HRLPC_EarlyOutTime": $filter('date')($scope.Early_Time, "HH:mm"),
                    "HRLPC_EarlyOutTime": $scope.Early_Time,
                    "HRLPC_CummulativeTimeFlag": $scope.Time_flag,
                    //"HRLPC_CummulativeTime": $filter('date')($scope.cum_time, "HH:mm"),
                    "HRLPC_CummulativeTime": $scope.cum_time, 
                   // "HRLPC_AfterCummulativeTime": $filter('date')($scope.Aftr_cum_time, "HH:mm"),
                    "HRLPC_AfterCummulativeTime":$scope.Aftr_cum_time,
                    "HRLPC_NoOfLatesCFFlag": $scope.carry_flag,
                    "HRLPC_LeavePrefixSuffixFlag": $scope.sufix_flag,
                    "HRLPC_IncludeHolidayFlag": $scope.holiday_inc_flag,
                    "HRLPC_LateLeaveFlag": $scope.HRLPC_LateLeaveFlag,
                    //"HRLPC_LateLOPFlag": $scope.HRLPC_LateLOPFlag,
                    "HRLPC_NoOfLatesFag": $scope.numlate_flag,
                    "HRLPC_NoOfLates": $scope.Late_num,

                    "HRLPC_SpOrGen": $scope.sporgen_flag,
                    "HRLPC_AbsentLOPFlag": $scope.HRLPC_AbsentLOPFlag,
                    "HRLPC_AbsentLeaveFlag": $scope.HRLPC_AbsentLeaveFlag,
                }
                debugger;
                apiService.create("LeaveConfig/SaveData", data).then(function (promise) {

                    
                    if (promise.returnval == true) {
                        swal("Record Saved/Updated Successfully", "Success");
                        $state.reload();
                    }
                    if (promise.returnval == false) {
                        swal("Failed to Save/Update. Check The Entries.");
                        // $state.reload();
                    }



                })
            }
            else {
                $scope.submitted = true;

            }

        };
    }
    })();
