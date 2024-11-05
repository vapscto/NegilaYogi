(function () {
    'use strict';

    angular
        .module('app')
        .controller('Leavetest', Leavetest);

    Leavetest.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', 'superCache']; 

    function Leavetest($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, superCache) {
      
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
               $scope.policy_id = promise.HRLPC_Id
           }
           else {
               swal("Settings not Done !!");
           }

       })
        };
    }
})();
