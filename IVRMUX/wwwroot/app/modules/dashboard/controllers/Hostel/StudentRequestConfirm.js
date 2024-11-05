
(function () {
    'use strict';
    angular
        .module('app')
        .controller('StudentRequestConfirm', StudentRequestConfirm)
    StudentRequestConfirm.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter']
    function StudentRequestConfirm($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter) {


        $scope.sortKey = 'HLHSREQ_Id';
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

            apiService.getURI("StudentRequestConfirm/loaddata", id).
                then(function (promise) {
                  
                    $scope.student_RequestList = promise.student_RequestList;
                    $scope.student_RequestApp = promise.student_RequestApp;
                    $scope.room_list = promise.room_list;                                       
                 
                })
        }

        
    

        //======================================Edit for Approve or Reject
        $scope.editstudetLV = function (user) {

            $scope.showflag = true;
            $scope.showflag_stud = true;

            $scope.HLHSREQ_Id = user.HLHSREQ_Id;
            $scope.HLHSREQC_Id = user.HLHSREQC_Id;
            $scope.studentName = user.studentName
            $scope.ASMCL_ClassName = user.ASMCL_ClassName;           
            $scope.AMST_AdmNo = user.AMST_AdmNo;
            $scope.AMST_RegistrationNo = user.AMST_RegistrationNo;
            $scope.HLMH_Name = user.HLMH_Name;
            $scope.HLMRCA_RoomCategory = user.HLMRCA_RoomCategory;
            $scope.HLHSREQ_RequestDate = new Date(user.HLHSREQ_RequestDate);
            $scope.HLHSREQ_Remarks = user.HLHSREQ_Remarks;
            $scope.HLHSREQ_BookingStatus = user.HLHSREQ_BookingStatus;

            if (user.HLHSREQ_VegMessFlg == true) {
                $scope.HLHSREQ_VegMessFlg = 1;
            }
            if (user.HLHSREQ_NonVegMessFlg == true) {
                $scope.HLHSREQ_NonVegMessFlg = 1; 
            }
            if (user.HLHSREQ_ACRoomFlg == true) {
                $scope.HLHSREQ_ACRoomFlg = 1;
            }
            if (user.HLHSREQ_SingleRoomFlg == true) {
                $scope.HLHSREQ_SingleRoomFlg = 1;
            }
            
        }
        //=======================================End


        //============================== Approve
        $scope.requestApproved = function () {

            if ($scope.myForm.$valid) {
                var data = {
                    "HLHSREQ_Id": $scope.HLHSREQ_Id,
                    "HLHSREQC_Id": $scope.HLHSREQC_Id,
                    "HLHSREQC_Remarks": $scope.HLHSREQC_Remarks,
                    "HRMRM_Id": $scope.HRMRM_Id,
                }
                apiService.create("StudentRequestConfirm/requestApproved", data)
                    .then(function (promise) {
                        if (promise.returnval == true) {
                            swal('Student Request Approved Successfully!');
                        }
                        else {
                            swal('Student Request Not Approved Successfully!');
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
                    "HLHSREQ_Id": $scope.HLHSREQ_Id,
                    "HLHSREQC_Id": $scope.HLHSREQC_Id,
                    "HLHSREQC_Remarks": $scope.HLHSREQC_Remarks,
                    "HRMRM_Id": $scope.HRMRM_Id,
                }
                apiService.create("StudentRequestConfirm/requestRejected", data)
                    .then(function (promise) {
                        if (promise.returnval == true) {
                            swal('Student Request Rejected Successfully!');
                        }
                        else {
                            swal('Student Request Not Rejected Successfully!');
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