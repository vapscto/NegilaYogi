
(function () {
    'use strict';
    angular
        .module('app')
        .controller('HostelAllotForStaff', HostelAllotForStaff)
    HostelAllotForStaff.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter']
    function HostelAllotForStaff($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter) {


        $scope.sortKey = 'HLHSTALT_Id';
        $scope.sortReverse = true;
        $scope.submitted = false;
        $scope.HLHSTALT_AllotmentDate = new Date();

        $scope.currentPage2 = 1;
        $scope.itemsPerPage2 = 10;
        $scope.search2 = "";

        $scope.showflag = false;
        $scope.showflag_stud = true;

        $scope.loadData = function () {
            var id = 2;
            $scope.currentPage2 = 1;
            $scope.itemsPerPage2 = 10;
            apiService.getURI("HostelAllotForStaff/loaddata", id).
                then(function (promise) {
                    $scope.desglist = promise.desglist;
                    $scope.hostel_list = promise.hostel_list;
                    $scope.student_allotlist = promise.student_allotlist;
                    $scope.deptlist = promise.deptlist;
                    $scope.roomcatgry_list = promise.roomcatgry_list;
                    $scope.room_list = promise.room_list;
                })
        }




        //======================================Edit for Approve or Reject
        //$scope.editstudetLV = function (user) {

        //    $scope.showflag = true;
        //    $scope.showflag_stud = true;

        //    $scope.HLHSREQ_Id = user.HLHSREQ_Id;
        //    $scope.HLHSREQC_Id = user.HLHSREQC_Id;


        //    $scope.staffName = user.staffName
        //    $scope.HRMD_DepartmentName = user.HRMD_DepartmentName;
        //    $scope.HRMDES_DesignationName = user.HRMDES_DesignationName;
        //    //$scope.AMST_AdmNo = user.AMST_AdmNo;
        //    //$scope.AMST_RegistrationNo = user.AMST_RegistrationNo;
        //    $scope.HLMH_Name = user.HLMH_Name;
        //    $scope.HLMRCA_RoomCategory = user.HLMRCA_RoomCategory;
        //    $scope.HLHSTREQ_RequestDate = new Date(user.HLHSTREQ_RequestDate);
        //    $scope.HLHSTREQ_Remarks = user.HLHSTREQ_Remarks;
        //    $scope.HLHSTREQ_BookingStatus = user.HLHSTREQ_BookingStatus;

        //    if (user.HLHSREQ_VegMessFlg == true) {
        //        $scope.HLHSREQ_VegMessFlg = 1;
        //    }
        //    if (user.HLHSREQ_NonVegMessFlg == true) {
        //        $scope.HLHSREQ_NonVegMessFlg = 1;
        //    }
        //    if (user.HLHSREQ_ACRoomFlg == true) {
        //        $scope.HLHSREQ_ACRoomFlg = 1;
        //    }
        //    if (user.HLHSREQ_SingleRoomFlg == true) {
        //        $scope.HLHSREQ_SingleRoomFlg = 1;
        //    }

        //}
        //=======================================End


        //============================== Approve
        $scope.savedata = function () {

            var AllotmentDate = $scope.HLHSTALT_AllotmentDate == null ? "" : $filter('date')($scope.HLHSTALT_AllotmentDate, "yyyy-MM-dd");
            //var VacatedDate = $scope.HLHSTALT_VacatedDate == null ? "" : $filter('date')($scope.HLHSTALT_VacatedDate, "yyyy-MM-dd");




            if ($scope.myForm.$valid) {
                var data = {
                    "HLHSTALT_Id": $scope.HLHSTALT_Id,
                    "HLHSTALT_AllotmentDate": AllotmentDate,
                    "HLMH_Id": $scope.HLMH_Id,
                    "HLMRCA_Id": $scope.hlmrcA_Id,
                    "HRME_Id": $scope.hrmE_Id,
                    "HRMD_Id": $scope.hrmD_Id,
                    "HRMDES_Id": $scope.hrmdeS_Id,
                    "HRMRM_Id": $scope.HRMRM_Id,
                    "HLHSTALT_NoOfBeds": $scope.HRMRM_BedCapacity,
                    "HLHSTALT_AllotRemarks": $scope.HLHSTALT_AllotRemarks,
                    //"HLHSTALT_VacateFlg": $scope.HLHSTALT_VacateFlg,
                    //"HLHSTALT_VacatedDate": VacatedDate,
                    // "HLHSTALT_VacateRemarks": $scope.HLHSTALT_VacateRemarks,
                }
                apiService.create("HostelAllotForStaff/savedata", data)
                    .then(function (promise) {
                        if (promise.duplicate == false) {
                            if (promise.returnval == true) {

                                if ($scope.HLHSTALT_Id > 0) {
                                    swal('Record Updated Successfully!');
                                }
                                else {
                                    swal('Record Saved Successfully!');
                                }
                                $state.reload();
                            }
                            else if (promise.returnval == false) {
                                if ($scope.HLHSTALT_Id > 0) {
                                    swal('Record Not Updated Successfully!');
                                }
                                else {
                                    swal('Record Not Saved Successfully!');
                                }
                            }
                        }
                        else {
                            swal('Record Already Exist!');
                        }


                    });
            }
            else {
                $scope.submitted = true;
            }
        }
        //=======================================End

        // ====== deactivate ==========


        $scope.deactivYTab1 = function (usersem, SweetAlert) {

            var dystring = "";
            if (usersem.HLHSTALT_ActiveFlag == true) {
                dystring = "Deactivate";
            }
            else if (usersem.HLHSTALT_ActiveFlag == false) {
                dystring = "Activate"
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
                        apiService.create("HostelAllotForStaff/deactivYTab1", usersem).
                            then(function (promise) {
                                if (promise.ret == true) {
                                    swal("Record " + dystring + "d" + " Successfully!!!");
                                    $state.reload();
                                }
                                else {

                                    swal("Record Not " + dystring + "d " + "Successfully!!!");
                                    $state.reload();
                                }
                            })
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }

                });
        }


        //============= Get Student Information
        $scope.get_studInfo = function () {

            $scope.housewise_studentList = [];
            var data = {
                "HLMH_Id": $scope.HLMH_Id,
            }
            apiService.create("HostelAllotForStaff/get_studInfo", data)
                .then(function (promise) {

                    if (promise.housewise_studentList.length > 0) {
                        $scope.housewise_studentList = promise.housewise_studentList;
                       // $scope.hrmE_Id = promise.housewise_studentList[0].hrmE_Id;
                        $scope.hrmD_Id = promise.housewise_studentList[0].HRMD_Id;
                        $scope.hrmdeS_Id = promise.housewise_studentList[0].HRMDES_Id;
                        $scope.hlmrcA_Id = promise.housewise_studentList[0].HLMRCA_Id;
                        $scope.HRMRM_Id = promise.housewise_studentList[0].hrmrM_Id;
                    }
                    else {
                        swal('Record Not Available!');
                        $scope.housewise_studentList = [];
                    }
                });
        }
        //===============================End

        //============= Edit data

        $scope.editstudetLV = function (user) {

            var data = {
                "HLHSTALT_Id": user.HLHSTALT_Id,
                "HLMH_Id": user.HLMH_Id,
            }
            apiService.create("HostelAllotForStaff/editdata", data).then(function (promise) {

                $scope.editlist = promise.editlist;

                $scope.HLHSTALT_Id = promise.editlist[0].hlhstalT_Id;

                //$scope.HLHSALT_AllotmentDate = new Date(promise.editlist[0].hlhsalT_AllotmentDate);
                $scope.HLHSTALT_AllotmentDate = new Date(promise.editlist[0].hlhstalT_AllotmentDate);
                $scope.HLMH_Id = promise.editlist[0].hlmH_Id;
                //$scope.hrmE_Id = promise.editlist[0].hrmE_Id;
                //$scope.hrmD_Id = promise.editlist[0].hrmD_Id;
                //$scope.hrmdeS_Id = promise.editlist[0].hrmdeS_Id;
                $scope.hlmrcA_Id = promise.housewise_studentList[0].HLMRCA_Id;
                $scope.HRMRM_Id = promise.editlist[0].hrmrM_Id;

                if ($scope.HRMRM_Id != undefined && $scope.HRMRM_Id != null && $scope.HRMRM_Id != '') {
                    $scope.get_roomdetails();
                }

                $scope.HRMRM_BedCapacity = promise.editlist[0].hlhstalT_NoOfBeds;

                $scope.HLHSTALT_AllotRemarks = promise.editlist[0].hlhstalT_AllotRemarks;

                if ($scope.HLMH_Id != undefined && $scope.HLMH_Id != null && $scope.HLMH_Id != '') {

                    $scope.housewise_studentList = promise.housewise_studentList;
                    if (promise.housewise_studentList[0].HLHSTREQC_VegMessFlg == true) {
                        $scope.HLHSTREQC_VegMessFlg = 1;
                    }
                    if (promise.housewise_studentList[0].HLHSTREQC_NonVegMessFlg == true) {
                        $scope.HLHSTREQC_NonVegMessFlg = 1;
                    }
                    if (promise.housewise_studentList[0].HLHSTREQC_ACRoomFlg == true) {
                        $scope.HLHSTREQC_ACRoomFlg = 1;
                    }
                    if (promise.housewise_studentList[0].HLHSTREQC_SingleRoomFlg == true) {
                        $scope.HLHSTREQC_SingleRoomFlg = 1;
                    }

                    $scope.hrmE_Id = promise.housewise_studentList[0].HRME_Id;
                    $scope.staffname = promise.housewise_studentList[0].staffname;
                    $scope.HRME_EmployeeCode = promise.housewise_studentList[0].HRME_EmployeeCode;

                    $scope.hrmD_Id = promise.housewise_studentList[0].HRMD_Id;
                    $scope.hrmD_DepartmentName = promise.housewise_studentList[0].HRMD_DepartmentName;

                    $scope.hrmdeS_Id = promise.housewise_studentList[0].HRMDES_Id;
                    $scope.hrmdeS_DesignationName = promise.housewise_studentList[0].HRMDES_DesignationName;


                    $scope.hlmrcA_RoomCategory = promise.housewise_studentList[0].HLMRCA_RoomCategory;
                }






            });


        }




        //$scope.editdata = function () {
        //    var data = {
        //        "HLHSALT_Id": $scope.HLHSALT_Id,
        //    }
        //    apiService.create("HostelAllotForStaff/editdata", data).then(function (promise) {

        //        $scope.editlist = promise.editlist;

        //        $scope.HLHSALT_Id = $scope.editlist[0].hlhsalT_Id;
        //        $scope.HLHSTALT_AllotmentDate = new Date($scope.editlist[0].hlhsalT_AllotmentDate);
        //        $scope.ASMAY_Id = $scope.editlist[0].asmaY_Id;
        //        $scope.HLMH_Id = $scope.editlist[0].hlmH_Id;
        //        $scope.HLMRCA_Id = $scope.editlist[0].hlmrcA_Id;
        //        $scope.amsT_Id = $scope.editlist[0].amsT_Id;
        //        $scope.asmcL_Id = $scope.editlist[0].asmcL_Id;
        //        $scope.asmS_Id = $scope.editlist[0].asmS_Id;
        //        $scope.HRMRM_Id = $scope.editlist[0].hrmrM_Id;
        //        $scope.HRMRM_BedCapacity = $scope.editlist[0].hrmrM_BedCapacity;

        //        $scope.HLHSTALT_VacatedDate = new Date($scope.editlist[0].hlhsalT_VacatedDate);
        //        $scope.HLHSTALT_VacateRemarks = $scope.editlist[0].hlhsalT_VacateRemarks;
        //        $scope.HLHSTALT_AllotRemarks = $scope.editlist[0].hlhsalT_AllotRemarks;
        //        if ($scope.editlist[0].hlhsalT_VacateFlg == true) {
        //            $scope.HLHSTALT_VacateFlg = 1;
        //        }


        //    });
        //}
        //===============================End


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

        //======================================
        $scope.get_studetdetails = function (studdata) {

            $scope.hrmD_Id = studdata[0].HRMD_Id;
            $scope.hrmdeS_Id = studdata[0].HRMDES_Id;
            $scope.hlmrcA_Id = studdata[0].HLMRCA_Id;

            // $scope.HLMRCA_Id = studdata[0].HLMRCA_Id;

            if (studdata[0].HLHSTREQC_VegMessFlg == true) {
                $scope.HLHSTREQC_VegMessFlg = 1;
            }
            if (studdata[0].HLHSTREQC_NonVegMessFlg == true) {
                $scope.HLHSTREQC_NonVegMessFlg = 1;
            }
            if (studdata[0].HLHSTREQC_ACRoomFlg == true) {
                $scope.HLHSTREQC_ACRoomFlg = 1;
            }
            if (studdata[0].HLHSTREQC_SingleRoomFlg == true) {
                $scope.HLHSTREQC_SingleRoomFlg = 1;
            }
        }


        $scope.get_roomdetails = function () {

            var data = {
                "HRMRM_Id": $scope.HRMRM_Id,
            }
            apiService.create("HostelAllotForStaff/get_roomdetails", data).then(function (promise) {

                $scope.HRMRM_BedCapacity = promise.hrmrM_BedCapacity;

            })
        }


    }
})();