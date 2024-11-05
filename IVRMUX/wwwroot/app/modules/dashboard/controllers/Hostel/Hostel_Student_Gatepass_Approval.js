(function () {
    'use strict';
    angular
        .module('app')
        .controller('Hostel_Student_Gatepass_ApprovalController', Hostel_Student_Gatepass_ApprovalController)
    Hostel_Student_Gatepass_ApprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function Hostel_Student_Gatepass_ApprovalController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {
              
        $scope.obj = {};
        $scope.obj.HLHSTGP_ComingBackDate = new Date();
        $scope.hstep = 1;
        $scope.mstep = 1;
        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };


        $scope.onLoadGetData = function () {
            var pageid = 2;
            apiService.getURI("Hostel_Student_Gatepass_Process/getalldetails", pageid).then(function (promise) {
                $scope.gridOptions = promise.gridlistdata;   
                $scope.getloaddata = promise.approved; 
            })
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.Clear = function () {
            $state.reload();
        }

        $scope.GetAllRelData = function (user) { 

            var data = {
                "HLHSTGP_Id": user.HLHSTGP_Id,
                "AMCST_Id": user.AMCST_Id,
            };
            apiService.create("Hostel_Student_Gatepass_Process/empdetails", data).then(function (promise) {
                $scope.AMCST_Id = user.AMCST_Id;
                $scope.HLHSTGP_Id = user.HLHSTGP_Id;            
                $scope.getdata = promise.griddisplay; 
            })
        }
        $scope.approvedreject = function (status) {
            if ($scope.myForm.$valid) {
                var data = {
                    "HLHSTGPAPP_Id": $scope.HLHSTGPAPP_Id,
                    "HLHSTGP_Id": $scope.HLHSTGP_Id,
                    "HLHSTGPAPP_Remarks": $scope.obj.HLHSTGPAPP_Remarks,                 
                    "AMCST_Id": $scope.AMCST_Id,
                    "HLHSTGP_ComingBackDate": $scope.obj.HLHSTGP_ComingBackDate,
                    "HLHSTGP_ComingBackTime": $filter('date')($scope.obj.HLHSTGP_ComingBackTime, "HH:mm"),
                    "HLHSTGPAPP_Status": status, 
                }
                apiService.create("Hostel_Student_Gatepass_Process/approvedrecord", data).then(function (promise) {
                    if (promise.retrunMsg == "Update") {
                        swal('Record saved successfully');
                        $state.reload();
                    }
                    else if (promise.retrunMsg == "false") {
                        swal('Record  Not saved successfully');
                    }
                })
            }
            else {
                $scope.submitted = true;
            }

        };
    }

})();