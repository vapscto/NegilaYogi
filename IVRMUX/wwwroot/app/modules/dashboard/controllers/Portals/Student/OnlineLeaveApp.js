
(function () {
    'use strict';
    angular
        .module('app')
        .controller('OnlineLeaveAppController', OnlineLeaveAppController)
    OnlineLeaveAppController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter']
    function OnlineLeaveAppController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter) {
        $scope.aslA_Id = 0;
        $scope.flag = "";

        $scope.sortKey = 'aslA_Id';
        $scope.sortReverse = true;

        $scope.submitted = false;
        $scope.submitted2 = false;

        $scope.aslA_ApplyDate = new Date();
        $scope.F2todates = new Date();

        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.search = "";

        $scope.currentPage2 = 1;
        $scope.itemsPerPage2 = 10;
        $scope.search2 = "";

        $scope.showflag = false;
        $scope.showflag_stud = true;

        $scope.loadData = function () {

            var id = 2;

            apiService.getURI("OnlineLeaveApp/getdetails", id).
                then(function (promise) {
                    $scope.roletype = promise.roletype;

                    if ($scope.roletype != undefined || $scope.roletype != null || $scope.roletype != "") {
                        if ($scope.roletype == "Student") {

                            $scope.studentdetails = promise.studentdetails;
                            if (promise.studentdetails.length > 0) {
                                $scope.asmcL_ClassName = promise.studentdetails[0].asmcL_ClassName;
                                $scope.asmS_Id = promise.studentdetails[0].asmS_Id;
                                $scope.asmC_SectionName = promise.studentdetails[0].asmC_SectionName;
                                $scope.amsT_RegistrationNo = promise.studentdetails[0].amsT_RegistrationNo;
                                $scope.amsT_MobileNo = promise.studentdetails[0].amsT_MobileNo;
                                $scope.amsT_emailId = promise.studentdetails[0].amsT_emailId;
                                $scope.asmcL_Id = promise.studentdetails[0].asmcL_Id;
                                $scope.amsT_FirstName = promise.studentdetails[0].amsT_FirstName;
                                $scope.amsT_Id = promise.studentdetails[0].amsT_Id;
                            }

                            $scope.allstuddata = promise.allstuddata;

                        }
                        else if ($scope.roletype == "Staff") {

                            $scope.pendingleave = promise.pendingleave;

                            if ($scope.pendingleave.length > 0) {
                                $scope.amsT_FirstName = promise.pendingleave[0].amsT_FirstName
                                $scope.asmcL_ClassName = promise.pendingleave[0].asmcL_ClassName;
                                $scope.asmC_SectionName = promise.pendingleave[0].asmC_SectionName;
                                $scope.amsT_RegistrationNo = promise.pendingleave[0].amsT_RegistrationNo;
                                $scope.amsT_MobileNo = promise.pendingleave[0].amsT_MobileNo;
                                $scope.amsT_emailId = promise.pendingleave[0].amsT_emailId;
                                $scope.asmcL_Id = promise.pendingleave[0].asmcL_Id;
                                $scope.amsT_Id = promise.pendingleave[0].amsT_Id;
                                $scope.aslA_Id = promise.pendingleave[0].aslA_Id;
                                $scope.asmS_Id = promise.pendingleave[0].asmS_Id;
                                $scope.aslA_Flag = promise.pendingleave[0].aslA_Flag;
                                $scope.aslA_Reason = promise.pendingleave[0].aslA_Reason;
                                $scope.aslA_FromDate = new Date(promise.pendingleave[0].aslA_FromDate);
                                $scope.aslA_ToDate = new Date(promise.pendingleave[0].aslA_ToDate);

                            }
                        }
                        else if ($scope.roletype == "Principal" || $scope.roletype == "Manager" ) {

                            $scope.pendingleave = promise.pendingleave;

                            if ($scope.pendingleave.length > 0) {
                                $scope.amsT_FirstName = promise.pendingleave[0].amsT_FirstName
                                $scope.asmcL_ClassName = promise.pendingleave[0].asmcL_ClassName;
                                $scope.asmC_SectionName = promise.pendingleave[0].asmC_SectionName;
                                $scope.amsT_RegistrationNo = promise.pendingleave[0].amsT_RegistrationNo;
                                $scope.amsT_MobileNo = promise.pendingleave[0].amsT_MobileNo;
                                $scope.amsT_emailId = promise.pendingleave[0].amsT_emailId;
                                $scope.asmcL_Id = promise.pendingleave[0].asmcL_Id;
                                $scope.amsT_Id = promise.pendingleave[0].amsT_Id;
                                $scope.aslA_Id = promise.pendingleave[0].aslA_Id;
                                $scope.asmS_Id = promise.pendingleave[0].asmS_Id;
                                $scope.aslA_Flag = promise.pendingleave[0].aslA_Flag;
                                $scope.aslA_Reason = promise.pendingleave[0].aslA_Reason;
                                $scope.aslA_FromDate = new Date(promise.pendingleave[0].aslA_FromDate);
                                $scope.aslA_ToDate = new Date(promise.pendingleave[0].aslA_ToDate);

                            }
                        }



                    }
                    else {

                    }
                })
        }


        //===========================Leave Apply By Student
        $scope.leaveapply = function () {

            if ($scope.myForm.$valid) {
                var data = {
                    "ASLA_Id": $scope.aslA_Id,
                    "ASLA_ApplyDate": new Date($scope.aslA_ApplyDate).toDateString(),
                    "ASLA_FromDate": new Date($scope.aslA_FromDate).toDateString(),
                    "ASLA_ToDate": new Date($scope.aslA_ToDate).toDateString(),
                    "ASLA_Flag": $scope.aslA_Flag,
                    "ASLA_LeaveId": $scope.aslA_LeaveId,
                    "ASLA_Reason": $scope.aslA_Reason,
                   
                }
                apiService.create("OnlineLeaveApp/leaveapply", data).
                    then(function (promise) {

                        if (promise.returnval != "" && promise.duplicate != null) {
                            if (promise.duplicate == false) {
                                if (promise.returnval == true) {
                                    if ($scope.aslA_Id > 0) {
                                        swal("Leave Apply Upadated Successfully!");
                                    }
                                    else {
                                        swal("Leave Apply Successfully!");
                                    }
                                }
                                else {
                                    if (promise.returnval == false) {
                                        if ($scope.aslA_Id > 0) {
                                            swal("Leave Apply Not Upadated Successfully!");
                                        }
                                        else {
                                            swal("Leave Not Apply Successfully!");
                                        }
                                    }
                                }
                            }
                            else {
                                swal("Record already exist");
                            }
                            $state.reload();
                        }
                        else {
                            swal("Kindly Contact Administrator!");
                        }
                    });
            }
            else {
                $scope.submitted = true;
            }
        }



        //================================edit data        
        $scope.editdata = function (user) {

            var data = {
                "ASLA_Id": user.aslA_Id,
            }
            apiService.create("OnlineLeaveApp/editdata", data)
                .then(function (promise) {
                    $scope.editlist = promise.editlist;
                    if ($scope.editlist.length > 0) {
                        $scope.aslA_Flag = promise.editlist[0].aslA_Flag;
                        $scope.aslA_LeaveId = promise.editlist[0].aslA_LeaveId;
                        $scope.aslA_Reason = promise.editlist[0].aslA_Reason;
                        $scope.aslA_FromDate = new Date(promise.editlist[0].aslA_FromDate);
                        $scope.aslA_ToDate = new Date(promise.editlist[0].aslA_ToDate);
                        $scope.aslA_Id = promise.editlist[0].aslA_Id;
                    }
                })
        }
        //==================End ==================//

        //======================================Edit for Approve or Reject
        $scope.editstudetLV = function (user) {

            $scope.showflag = true;
            $scope.showflag_stud = true;

            $scope.amsT_FirstName = user.amsT_FirstName
            $scope.asmcL_ClassName = user.asmcL_ClassName;
            $scope.asmC_SectionName = user.asmC_SectionName;
            $scope.amsT_RegistrationNo = user.amsT_RegistrationNo;
            $scope.amsT_MobileNo = user.amsT_MobileNo;
            $scope.amsT_emailId = user.amsT_emailId;
            $scope.asmcL_Id = user.asmcL_Id;
            $scope.amsT_Id = user.amsT_Id;
            $scope.aslA_Id = user.aslA_Id;
            $scope.asmS_Id = user.asmS_Id;
            $scope.aslA_Flag = user.aslA_Flag;
            $scope.aslA_Reason = user.aslA_Reason;
            $scope.aslA_FromDate1 = new Date(user.aslA_FromDate);
            $scope.aslA_ToDate1 = new Date(user.aslA_ToDate);
            $scope.aslA_LeaveId = user.aslA_LeaveId;
        }
        //=======================================End


        //==============================Leave Approve
        $scope.leaveApproved = function () {

            if ($scope.myForm2.$valid) {
                var data = {
                    "ASLA_Id": $scope.aslA_Id,
                    "ASMS_Id": $scope.asmS_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASLA_ApprovedFromDate": $scope.aslA_FromDate1,
                    "ASLA_ApprovedToDate": $scope.aslA_ToDate1,
                    "ASAP_ApprovalDate": $scope.aslA_ApplyDate,
                }
                apiService.create("OnlineLeaveApp/leaveApproved", data)
                    .then(function (promise) {
                        if (promise.returnval == true) {
                            swal('Leave Approved Successfully!');
                        }
                        else {
                            swal('Leave Not Approved Successfully!');
                        }
                        $state.reload();
                    });
            }
            else {
                $scope.submitted2 = true;
            }
        }
        //=======================================End



        //==============================Leave Reject
        $scope.leaveRejected = function () {

            if ($scope.myForm2.$valid) {
                var data = {
                    "ASLA_Id": $scope.aslA_Id,
                    "ASMS_Id": $scope.asmS_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASLAP_AppFromDate": $scope.aslA_FromDate,
                    "ASLAP_AppToDate": $scope.aslA_ToDate,
                    "ASLAP_AppRejDate": $scope.aslA_ApplyDate,
                }
                apiService.create("OnlineLeaveApp/leaveRejected", data)
                    .then(function (promise) {
                        if (promise.returnval == true) {
                            swal('Leave Rejected Successfully!');
                        }
                        else {
                            swal('Leave Not Rejected Successfully!');
                        }
                        $state.reload();
                    });
            }
            else {
                $scope.submitted2 = true;
            }
        }
        //=======================================End


        //=================Activation/Deactivation--Record.........
        $scope.deactiveY = function (user, SweetAlert) {

            $scope.aslA_Id = user.aslA_Id;

            var dystring = "";
            if (user.aslA_ActiveFlag == 1) {
                dystring = "Deactivate";
            }
            else if (user.aslA_ActiveFlag == 0) {
                dystring = "Activate";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("OnlineLeaveApp/deactiveY", user).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal("Record " + dystring + "d Successfully!!!");
                                }
                                else {
                                    swal("Record Not " + dystring + "d Successfully!!!");
                                }
                                $state.reload();
                            })
                    }
                    else {
                        swal("Record " + dystring + " Cancelled");
                    }

                });
        }
        //================End--Activation/Deactivation--Record.........



        $scope.Clearid = function () {
            $state.reload();
        }

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted2;
        };


        $scope.cancellationRecord = function (user, SweetAlert) {

            debugger;
            $scope.aslA_Id = user.aslA_Id;            
            var dystring = "";
            var dystring12 = "Cancellation";
            var dystring34 = "Cancelled";
            if (user.aslA_Status == 'Pending') {
                dystring = "Cancel";
            }
            else {
                dystring = "Not Cancel";
            }
            swal({
                title: "Are you sure?",
                text: "Do You Want To " + dystring + " Leave?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + dystring + " it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("OnlineLeaveApp/cancellationRecord", user).then(function (promise) {
                            if (promise.returnval == true) {
                                swal("Leave Application " + dystring34 + " Successfully!!!");
                            }
                            else {
                                swal("Leave Application Not " + dystring34 + " Successfully!!!");
                            }
                            $state.reload();
                        })
                    }
                    else {
                        swal("Leave " + dystring12 + " Cancelled");
                    }

                });
        }

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }




    }
})();