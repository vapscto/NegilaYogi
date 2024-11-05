
(function () {
    'use strict';
    angular
        .module('app')
        .controller('StaffRequestConfirm', StaffRequestConfirm)
    StaffRequestConfirm.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter']
    function StaffRequestConfirm($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter) {


        $scope.sortKey = 'HLHSTREQ_Id';
        $scope.sortReverse = true;      
        $scope.submitted = false;        
        $scope.F2todates = new Date();       

        $scope.currentPage2 = 1;
        $scope.itemsPerPage2 = 10;
        $scope.search2 = "";

        $scope.showflag = false;
        $scope.showflag_stud = true;

        $scope.loadData = function () {

            var id = 2;

            apiService.getURI("StaffRequestConfirm/loaddata", id).
                then(function (promise) {
                    $scope.roletype = promise.roletype;    
                    $scope.staff_RequestList = promise.staff_RequestList;
                    $scope.staff_ApprovalList = promise.staff_ApprovalList;
                    $scope.room_list = promise.room_list;    
                    

                })
        }

        
    

        //======================================Edit for Approve or Reject
      
        $scope.editstudetLV = function (user) {

            $scope.showflag = true;
            $scope.showflag_stud = true;

            $scope.HLHSTREQ_Id = user.HLHSTREQ_Id;
            $scope.HLHSTREQC_Id = user.HLHSTREQC_Id;
            
            $scope.HLMH_Name = user.HLMH_Name;
            $scope.HLMRCA_RoomCategory = user.HLMRCA_RoomCategory;
            $scope.HLHSTREQ_RequestDate = new Date(user.HLHSTREQ_RequestDate);
            $scope.HLHSTREQ_Remarks = user.HLHSTREQ_Remarks;
            $scope.HLHSTREQ_BookingStatus = user.HLHSTREQ_BookingStatus;

            $scope.empName = user.empName
            $scope.HRMD_DepartmentName = user.HRMD_DepartmentName;
            $scope.HRMDES_DesignationName = user.HRMDES_DesignationName;
            $scope.HRME_EmployeeCode = user.HRME_EmployeeCode;

            if (user.HLHSTREQ_VegMessFlg == true) {
                $scope.HLHSTREQ_VegMessFlg = 1;
            }
            if (user.HLHSTREQ_NonVegMessFlg == true) {
                $scope.HLHSTREQ_NonVegMessFlg = 1; 
            }
            if (user.HLHSTREQ_ACRoomFlg == true) {
                $scope.HLHSTREQ_ACRoomFlg = 1;
            }
            if (user.HLHSTREQ_SingleRoomFlg == true) {
                $scope.HLHSTREQ_SingleRoomFlg = 1;
            }
            
        }
        //=======================================End


        //============================== Approve
        $scope.requestApproved = function () {

            if ($scope.myForm.$valid) {
                var data = {
                    "HLHSTREQ_Id": $scope.HLHSTREQ_Id,
                    "HLHSTREQC_Id": $scope.HLHSTREQC_Id,
                    "HLHSTREQC_Remarks": $scope.HLHSTREQC_Remarks,
                    "HRMRM_Id": $scope.HRMRM_Id,
                }
                apiService.create("StaffRequestConfirm/requestApproved", data)
                    .then(function (promise) {
                        if (promise.returnval == true) {
                            swal('Staff Request Approved Successfully!');
                        }
                        else {
                            swal('Staff Request Not Approved Successfully!');
                        }
                        $state.reload();
                    });
            }
            else {
                $scope.submitted = true;
            }
        }
        //=======================================End



        //============================== Reject
        $scope.requestRejected = function () {

            if ($scope.myForm.$valid) {
                var data = {
                    "HLHSTREQ_Id": $scope.HLHSTREQ_Id,
                    "HLHSTREQC_Id": $scope.HLHSTREQC_Id,
                    "HLHSTREQC_Remarks": $scope.HLHSTREQC_Remarks,
                    "HRMRM_Id": $scope.HRMRM_Id,
                }
                apiService.create("StaffRequestConfirm/requestRejected", data)
                    .then(function (promise) {
                        if (promise.returnval == true) {
                            swal('Staff Request Rejected Successfully!');
                        }
                        else {
                            swal('Staff Request Not Rejected Successfully!');
                        }
                        $state.reload();
                    });
            }
            else {
                $scope.submitted = true;
            }
        }
        //=======================================End
        

        $scope.Clearid = function () {
            $state.reload();
        }

      
        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }




    }
})();