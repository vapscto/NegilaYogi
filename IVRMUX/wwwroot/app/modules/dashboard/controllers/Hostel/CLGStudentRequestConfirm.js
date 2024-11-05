
(function () {
    'use strict';
    angular
        .module('app')
        .controller('CLGStudentRequestConfirmController', CLGStudentRequestConfirmController)
    CLGStudentRequestConfirmController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter']
    function CLGStudentRequestConfirmController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter) {


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

            apiService.getURI("CLGStudentRequestConfirm/loaddata", id).
                then(function (promise) {

                    $scope.student_RequestList = promise.student_RequestList;
                    $scope.room_list = promise.room_list;

                })
        }




        //======================================Edit for Approve or Reject
        $scope.editstudetLV = function (user) {

            $scope.showflag = true;
            $scope.showflag_stud = true;

            $scope.HLHSREQC_Id = user.HLHSREQC_Id;
            $scope.HLHSREQC_Id = user.HLHSREQC_Id;
            $scope.studentName = user.studentName
            $scope.AMCO_CourseName = user.AMCO_CourseName;
            $scope.AMCST_AdmNo = user.AMCST_AdmNo;
            $scope.AMCST_RegistrationNo = user.AMCST_RegistrationNo;
            $scope.HLMH_Name = user.HLMH_Name;
            $scope.HLMRCA_RoomCategory = user.HLMRCA_RoomCategory;
            $scope.HLHSREQC_RequestDate = new Date(user.HLHSREQC_RequestDate);
            $scope.HLHSREQC_Remarks = user.HLHSREQC_Remarks;
            $scope.HLHSREQC_BookingStatus = user.HLHSREQC_BookingStatus;

            if (user.HLHSREQC_VegMessFlg == true) {
                $scope.HLHSREQC_VegMessFlg = 1;
            }
            if (user.HLHSREQC_NonVegMessFlg == true) {
                $scope.HLHSREQC_NonVegMessFlg = 1;
            }
            if (user.HLHSREQC_ACRoomFlg == true) {
                $scope.HLHSREQC_ACRoomFlg = 1;
            }
            if (user.HLHSREQC_SingleRoomFlg == true) {
                $scope.HLHSREQC_SingleRoomFlg = 1;
            }

        }
        //=======================================End


        //============================== Approve
        $scope.requestApproved = function () {

            if ($scope.myForm.$valid) {
                var data = {
                    "HLHSREQC_Id": $scope.HLHSREQC_Id,
                    "HLHSREQC_Id": $scope.HLHSREQC_Id,
                    "HLHSREQC_Remarks": $scope.HLHSREQC_Remarks,
                    "HRMRM_Id": $scope.HRMRM_Id,
                }
                apiService.create("CLGStudentRequestConfirm/requestApproved", data)
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
                    "HLHSREQC_Id": $scope.HLHSREQC_Id,
                    "HLHSREQC_Id": $scope.HLHSREQC_Id,
                    "HLHSREQC_Remarks": $scope.HLHSREQC_Remarks,
                    "HRMRM_Id": $scope.HRMRM_Id,
                }
                apiService.create("CLGStudentRequestConfirm/requestRejected", data)
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


        $scope.bedcapacity = function () {

            var data = {
                "HRMRM_Id": $scope.HRMRM_Id
            };
            apiService.create("CLGStudentRequestConfirm/bedcapacity", data).then(function (promise) {
                if (promise.roomdetails != null && promise.roomdetails.length > 0) {
                    if (promise.bedcount == true) {
                        swal("Bed is Filled");
                        $scope.Clearid();
                    }
                    else
                    {
                        $scope.maxcapacity = promise.roomdetails[0].hrmrM_BedCapacity;
                        $scope.acavailable = promise.roomdetails[0].hrmrM_ACFlg;
                    }
                   
                }
               

            });
        };



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