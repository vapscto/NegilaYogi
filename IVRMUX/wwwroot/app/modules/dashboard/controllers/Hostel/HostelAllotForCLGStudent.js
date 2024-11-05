
(function () {
    'use strict';
    angular
        .module('app')
        .controller('HostelAllotForCLGStudentController', HostelAllotForCLGStudentController)
    HostelAllotForCLGStudentController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter', '$timeout']
    function HostelAllotForCLGStudentController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter, $timeout) {


        $scope.sortKey = 'HLHSALTC_Id';
        $scope.sortReverse = true;
        $scope.submitted = false;
        $scope.HLHSALT_AllotmentDate = new Date();

        $scope.currentPage2 = 1;
        $scope.itemsPerPage2 = 10;
        $scope.search2 = "";

        $scope.showflag = false;
        $scope.showflag_stud = true;
        $scope.obj = {};
        $scope.HLMRCAC_To_HRMRM_Id = "";
        $scope.obj.allotmenttype = 'Request';
        $scope.HLHSALTC_TransferDate = new Date();
        $scope.obj.Graphicalselect = false;
        $scope.loadData = function () {
            
            var id = 2;

            apiService.getURI("HostelAllotForCLGStudent/loaddata", id).
                then(function (promise) {

                    $scope.academicYear = promise.yearlist;
                    $scope.ASMAY_Id = promise.asmaY_Id;
                    $scope.hostel_list = promise.hostel_list;
                    $scope.student_allotlist = promise.student_allotlist;

                    $scope.semister = promise.semisterlist;
                    $scope.course = promise.courselist;
                    $scope.branch = promise.branchlist;
                    $scope.sectionlist = promise.sectionlist;


                    $scope.student_Requestlist = promise.student_Requestlist;
                    //$scope.room_list = promise.room_list;


                })
        }



        $scope.HLHSALTC_VacateFlg = false;
        $scope.HLHSALTC_VacateRemarks = "";
        $scope.HLHSALTC_AllotRemarks = "";
        //============================== Save
        $scope.savedata = function () {
            var AllotmentDate = $scope.HLHSALT_AllotmentDate == null ? "" : $filter('date')($scope.HLHSALT_AllotmentDate, "yyyy-MM-dd");
            var VacatedDate = $scope.HLHSALT_VacatedDate == null ? "" : $filter('date')($scope.HLHSALT_VacatedDate, "yyyy-MM-dd");

            var HRMRM_Id = 0;

            if ($scope.obj.Graphicalselect == true) {
                angular.forEach($scope.Temp_Floor_List, function (dd) {
                    angular.forEach(dd.room_details, function (ddd) {
                        if (ddd.HRMRM_IdCheck == true) {
                            HRMRM_Id = ddd.HRMRM_Id;
                        }
                    })
                });
            }
            else {
                HRMRM_Id = $scope.obj.HRMRM_Id;
            }      

                var data = {
                    //"HLHSALTC_Id": $scope.hlhsaltC_Id,
                    "HLHSALTC_AllotmentDate": AllotmentDate,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "HLMH_Id": $scope.obj.HLMH_Id,
                    "HLMRCA_Id": $scope.obj.HLMRCA_Id,
                    "AMCST_Id": $scope.AMCST_Id,
                    "AMCO_Id": $scope.amcO_Id,
                    "AMSE_Id": $scope.amsE_Id,
                    "ACMS_Id": $scope.acmS_Id,
                    "AMB_Id": $scope.amB_Id,
                    "HRMRM_Id": HRMRM_Id,
                    "HLHSALTC_VacateFlg": $scope.HLHSALTC_VacateFlg,
                    "HLHSALTC_VacatedDate": VacatedDate,
                    "HLHSALTC_VacateRemarks": $scope.HLHSALTC_VacateRemarks,
                    "HLHSALTC_AllotRemarks": $scope.obj.HLHSALTC_AllotRemarks,
                    "HLHSALTC_EntireRoomReqdFlg": $scope.obj.HLHSREQC_EntireRoomReqdFlg,
                    "HLHSREQC_EntireRoomReqdFlg": $scope.obj.HLHSREQC_EntireRoomReqdFlg,
                }
                apiService.create("HostelAllotForCLGStudent/savedata", data).then(function (promise) {
                    if (promise.duplicate == false) {
                        if (promise.returnval == true) {
                            if ($scope.HLHSALTC_Id > 0)
                            {
                                $scope.Clearid();
                                swal('Record Updated Successfully!!!');
                            }
                            else {
                                $scope.Clearid();
                                swal('Record Saved Successfully!!!');
                            }
                        }
                        else {
                            if (promise.returnval == false) {
                                if ($scope.HLHSALTC_Id > 0) {
                                    swal('Record Not Update Successfully!!!');
                                }
                                else {
                                    swal('Record Not Saved Successfully!!!');
                                }
                            }
                        }
                    }
                    else {
                        swal('Record Already Exist!');
                    }
                });
        }
        //=======================================End

        $scope.obj.HLHSREQC_ACRoomReqdFlg = false;
        $scope.obj.HLHSREQC_EntireRoomReqdFlg = false
        $scope.obj.HLHSREQC_VegMessReqdFlg = false;
        $scope.obj.HLHSREQC_NonVegMessReqdFlg = false;

        //============================== Approve
        $scope.requestApproved = function () {
           // $scope.HLHSALTC_AllotRemarks = "";
            var HLMRCA = 0;
            var HRMRM_Id = 0;
            if ($scope.obj.Graphicalselect == true) {
                angular.forEach($scope.Temp_Floor_List, function (dd) {
                    angular.forEach(dd.room_details, function (ddd) {
                        if (ddd.HRMRM_IdCheck == true) {
                            HRMRM_Id = ddd.HRMRM_Id;
                        }
                    })
                });
            }
            else {
                HRMRM_Id = $scope.obj.HRMRM_Id;
            }
           
           
            if ($scope.roomcategory > 0) {
                HLMRCA = $scope.roomcategory;
            }
            else {
                HLMRCA = $scope.obj.HLMRCA_Id;
            }
            var HLHSREQC = 0;
            if ($scope.HLHSREQC_Id > 0) {
                HLHSREQC = $scope.HLHSREQC_Id;
            }           
            var data = {
                "HLHSREQC_Id": HLHSREQC,
                "HLHSREQCC_Remarks": $scope.HLHSREQC_Remarks,
                "HLHSALTC_AllotRemarks": $scope.obj.HLHSALTC_AllotRemarks,
                "HLHSREQC_Remarks": $scope.HLHSREQC_Remarks,
                "HRMRM_Id": HRMRM_Id,
                "HLMH_Id": $scope.obj.HLMH_Id,
                "HLMRCA_Id": HLMRCA,
                "HLHSREQC_ACRoomReqdFlg": $scope.obj.HLHSREQC_ACRoomReqdFlg,
                "HLHSREQC_EntireRoomReqdFlg": $scope.obj.HLHSREQC_EntireRoomReqdFlg,
                "HLHSREQC_VegMessReqdFlg": $scope.obj.HLHSREQC_VegMessReqdFlg,
                "HLHSREQC_NonVegMessReqdFlg": $scope.obj.HLHSREQC_NonVegMessReqdFlg,
                "allottype": $scope.obj.allotmenttype,
                "AMCST_Id": $scope.obj.AMCST_Id,
                "HLHSALTC_AllotmentDate": $scope.HLHSALTC_AllotmentDate,
            }
            apiService.create("HostelAllotForCLGStudent/requestApproved", data)
                .then(function (promise) {
                    if (promise.returnval == true) {
                        swal('Student Request Approved Successfully!');
                        $scope.savedata();
                    }
                    else {
                        swal('Student Request Not Approved!');
                    }

                });                       
        }
        //=======================================End



        //==========================Transfer
        $scope.transfer = function () {
            if ($scope.myForm.$valid) {
                var HLHSTRSC_Id = 0;
                if ($scope.HLHSTRSC_Id > 0) {
                    HLHSTRSC_Id = $scope.hlhstrsC_Id;
                }
                var data = {
                    "HLHSTRSC_Id": HLHSTRSC_Id,
                    "HLHSALTC_TransferDate": new Date($scope.HLHSALTC_TransferDate).toDateString(),
                    "HLMH_Id": $scope.obj.HLMH_Id,
                    "HLMRCA_Id": $scope.roomcategory,
                    "ACMST_Id": $scope.obj.AMCST_Id,
                    "HRMRM_Id": $scope.HRMRM_Id_Old,
                    // "HLHSTRSC_RoomFee": $scope.obj.HLHSTRSC_RoomFee,
                    "HLHSTRSC_To_HLMRCA_Id": $scope.HLMRCAC_To_HRMRM_Id,
                    "HLMRCAC_To_HRMRM_Id": $scope.HRMRM_Id,
                    "HLHSTRSC_EntireRoomReqdFlg": $scope.obj.HLHSREQC_EntireRoomReqdFlg,
                    "HLHSTRSC_NewRoomFee": $scope.obj.HLHSTRSC_NewRoomFee,
                    "HLHSTRSC_AllotRemarks": $scope.obj.HLHSTRSC_AllotRemarks,
                    "HLHSTRSC_VacateRemarks": $scope.obj.HLHSTRSC_VacateRemarks,


                }
                apiService.create("HostelAllotForCLGStudent/HostelT", data).then(function (promise) {

                    if (promise.returnval == "save") {
                        swal("Record Save / Update Successfully !");
                    }
                    else if (promise.returnval == "notsave") {
                        swal("Record Not Saved!");
                    }
                    else if (promise.returnval == "admin") {
                        swal("Please Contact Administrator !");
                    }
                    else if (promise.returnval == "duplicate") {
                        swal("Record Duplicate  !");
                    }
                    $state.reload();
                });
            }
            else {
                $scope.submitted = true;
            }
        }
        //==========================Transfer End


        //============================== Reject
        $scope.requestRejected = function () {
            var HLMRCA = "";
            if ($scope.roomcategory > 0) {
                HLMRCA = $scope.roomcategory;
            }
            else {
                HLMRCA = $scope.obj.HLMRCA_Id;
            }
            var HLHSREQC = 0;
            if ($scope.HLHSREQC_Id > 0) {
                HLHSREQC = $scope.HLHSREQC_Id;
            }
            if ($scope.myForm.$valid) {
                var data = {
                    "HLHSREQC_Id": HLHSREQC,
                    "HLHSALTC_AllotRemarks": $scope.HLHSALTC_AllotRemarks,
                    "HRMRM_Id": $scope.HRMRM_Id,
                    "HLMH_Id": $scope.obj.HLMH_Id,
                    "HLMRCA_Id": HLMRCA,
                    "HLHSREQC_ACRoomReqdFlg": $scope.obj.HLHSREQC_ACRoomReqdFlg,
                    "HLHSREQC_EntireRoomReqdFlg": $scope.obj.HLHSREQC_EntireRoomReqdFlg,
                    "HLHSREQC_VegMessReqdFlg": $scope.obj.HLHSREQC_VegMessReqdFlg,
                    "HLHSREQC_NonVegMessReqdFlg": $scope.obj.HLHSREQC_NonVegMessReqdFlg,
                }
                apiService.create("HostelAllotForCLGStudent/requestRejected", data)
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



        $scope.roomcatgry_list = ""; $scope.HRMRM_Id = ""; $scope.HRMRM_BedCapacity = "";
        $scope.amcO_Id = ""; $scope.amB_Id = ""; $scope.amsE_Id = ""; $scope.acmS_Id = "";
        $scope.obj.allotmenttype = "";



        //============= Get Student Information
        $scope.get_studInfo = function (objtype,obj) {
            $scope.HLMH_Id = 0; $scope.hlmF_Id = ""; $scope.HLHSREQC_Remarks = ""; $scope.HRMRM_Id = 0;
            $scope.AMCST_Id_Temp_Details = [];
            $scope.housewise_studentList = [];
            var data = {
                "HLMH_Id": $scope.HLMH_Id,
                "Type": objtype,
              
            }

            apiService.create("HostelAllotForCLGStudent/get_studInfo", data).then(function (promise) {


                if (promise.housewise_studentList.length > 0) {
                    $scope.housewise_studentList = promise.housewise_studentList;
                }
                else {
                    $scope.AMCST_Id_Temp_Details = [];
                    $scope.display = false;
                    swal('Requested Student Not Available!');
                    // $scope.housewise_studentList = [];
                }


            });
        }
        //===============================End

        //===============================Std Transfer
        //$scope.stdtrnsfer = function (objtype) {
        //    $scope.hlmF_Id = ""; $scope.HLHSREQC_Remarks = "";
        //    $scope.AMCST_Id_Temp_Details = [];
        //    $scope.housewise_studentList = [];
        //    var data = {
        //        "HLMH_Id": $scope.HLMH_Id,
        //        "Type": objtype,
        //        "HRMRM_Id": $scope.HRMRM_Id
        //    }

        //    apiService.create("HostelAllotForCLGStudent/get_studInfo", data).then(function (promise) {


        //        if (promise.housewise_studentList.length > 0) {
        //            $scope.housewise_studentList = promise.housewise_studentList;
        //        }
        //        else {
        //            $scope.AMCST_Id_Temp_Details = [];
        //            swal('Requested Student Not Available!');
        //            // $scope.housewise_studentList = [];
        //        }


        //    });
        //}
        //===============================End

        //============= floor wise room
        $scope.floor = function () {
            $scope.floor_list = [];
            var data = {
                "HLMH_Id": $scope.obj.HLMH_Id,
            }
            apiService.create("HostelAllotForCLGStudent/floor", data).then(function (promise) {
                if (promise.floor_list != null && promise.floor_list.length > 0) {
                    $scope.floor_list = promise.floor_list;
                } else {
                    swal('Record Not Available!');
                }
                if (promise.roomcatgry_list != null && promise.roomcatgry_list.length > 0) {
                    $scope.roomcatgry_list = promise.roomcatgry_list;
                } else {
                    swal('Record Not Available!');
                }
            });
        };
        //===============================End
         //============= floor wise room
        $scope.room = function () {
            $scope.room_Details = [];
            $scope.HRMRM_RoomNoONE = "";
            $scope.HRMRM_BedCapacity = "";
            $scope.AllotedCount = "";
            $scope.AvailableBedCapacity = "";
            $scope.obj.HRMRM_Id = "";
            var data = {
                "HLMF_Id": $scope.obj.hlmF_Id,
                "HLMRCA_Id": $scope.obj.HLMRCA_Id,
            }
            apiService.create("HostelAllotForCLGStudent/room", data).then(function (promise) {

                if (promise.room_list != null && promise.room_list.length > 0) {
                    $scope.room_list = promise.room_list;
                }
            });
        }
        //============= Room  details
        $scope.roomdetails = function () {
            $scope.room_Details = [];
            $scope.HRMRM_RoomNoONE = "";
            $scope.HRMRM_BedCapacity = "";
            $scope.AllotedCount = "";
            $scope.AvailableBedCapacity = "";

            var data = {
                "HLMH_Id": $scope.obj.HLMH_Id,
                "HLMF_Id": $scope.obj.hlmF_Id,
                "HLMRCA_Id": $scope.obj.HLMRCA_Id,
                "HRMRM_Id": $scope.obj.HRMRM_Id,
            }
            apiService.create("HostelAllotForCLGStudent/roomdetails", data).then(function (promise) {
                   if (promise.room_Details != null && promise.room_Details.length > 0) {
                    $scope.room_Details = promise.room_Details;
                       $scope.HRMRM_RoomNoONE = $scope.room_Details[0].HRMRM_RoomNo;
                    $scope.HRMRM_BedCapacity = $scope.room_Details[0].HRMRM_BedCapacity;
                    $scope.AllotedCount = $scope.room_Details[0].AllotedCount;
                    $scope.AvailableBedCapacity = $scope.room_Details[0].AvailableBedCapacity;
                }
            });
        }
        //======================================
        $scope.get_studetdetails = function (studdata) {
            $scope.obj.HLHSREQC_VegMessReqdFlg = false;
            $scope.obj.HLHSREQC_NonVegMessReqdFlg = false;
            $scope.obj.HLHSREQC_ACRoomReqdFlg = false;
            $scope.obj.HLHSREQC_EntireRoomReqdFlg = false;
            $scope.AMCST_Id_Temp_Details = [];
            angular.forEach(studdata, function (dd) {
                if (dd.AMCST_Id == $scope.obj.AMCST_Id) {
                    $scope.AMCST_Id_Temp_Details.push(dd);
                }
            });
            if ($scope.AMCST_Id_Temp_Details.length > 0) {
                $scope.amcO_Id = $scope.AMCST_Id_Temp_Details[0].AMCO_Id;
                $scope.amsE_Id = $scope.AMCST_Id_Temp_Details[0].AMSE_Id;
                $scope.amB_Id = $scope.AMCST_Id_Temp_Details[0].AMB_Id;
                $scope.acmS_Id = $scope.AMCST_Id_Temp_Details[0].ACMS_Id;
                $scope.roomcategory = $scope.AMCST_Id_Temp_Details[0].HLMRCA_Id;
                $scope.studentName = $scope.AMCST_Id_Temp_Details[0].studentName;
                $scope.amcsT_RegistrationNo = $scope.AMCST_Id_Temp_Details[0].AMCST_RegistrationNo;
                $scope.amcO_CourseName = $scope.AMCST_Id_Temp_Details[0].AMCO_CourseName;
                $scope.amB_BranchName = $scope.AMCST_Id_Temp_Details[0].AMB_BranchName;
                $scope.amsE_SEMName = $scope.AMCST_Id_Temp_Details[0].AMSE_SEMName;
                $scope.amcsT_MobileNo = $scope.AMCST_Id_Temp_Details[0].AMCST_MobileNo;
                $scope.amcsT_emailId = $scope.AMCST_Id_Temp_Details[0].AMCST_emailId;
                $scope.HLHSREQC_Id = $scope.AMCST_Id_Temp_Details[0].HLHSREQC_Id;
                $scope.AMCST_Id = $scope.AMCST_Id_Temp_Details[0].AMCST_Id;
                $scope.HLHSREQC_Remarks = $scope.AMCST_Id_Temp_Details[0].HLHSREQC_Remarks;
                $scope.HLMRCA_RoomCategory = $scope.AMCST_Id_Temp_Details[0].HLMRCA_RoomCategory;
                $scope.HRMRM_SharingFlg = $scope.AMCST_Id_Temp_Details[0].HRMRM_SharingFlg;
                $scope.HRMRM_RoomNo = $scope.AMCST_Id_Temp_Details[0].HRMRM_RoomNo;
                $scope.HRMRM_Id_Old = $scope.AMCST_Id_Temp_Details[0].HRMRM_Id;
                $scope.HLHSALTC_AllotmentDate = $scope.AMCST_Id_Temp_Details[0].HLHSALTC_AllotmentDate;

            }

            if ($scope.AMCST_Id_Temp_Details[0].HLHSREQC_VegMessReqdFlg == true) {
                $scope.obj.HLHSREQC_VegMessReqdFlg = 1;
            }
            if ($scope.AMCST_Id_Temp_Details[0].HLHSREQC_NonVegMessReqdFlg == true) {
                $scope.obj.HLHSREQC_NonVegMessReqdFlg = 1;
            }
            if ($scope.AMCST_Id_Temp_Details[0].HLHSREQC_ACRoomReqdFlg == true) {
                $scope.obj.HLHSREQC_ACRoomReqdFlg = 1;
            }
            if ($scope.AMCST_Id_Temp_Details[0].HLHSREQC_EntireRoomReqdFlg == true) {
                $scope.obj.HLHSREQC_EntireRoomReqdFlg = 1;
            }


        };


        $scope.desable = false;
        $scope.get_roomdetails = function () {

            var data = {
                "HRMRM_Id": $scope.HRMRM_Id,
            }
            apiService.create("HostelAllotForCLGStudent/get_roomdetails", data).then(function (promise) {
                $scope.bedcounts = promise.bedcount;
                if ($scope.bedcounts == true) {
                    swal("Bed is Filled");
                    $scope.Clearid();
                }
                else if ($scope.bedcounts == false) {
                    $scope.HRMRM_BedCapacity = promise.hrmrM_BedCapacity;
                }
            });
        }
        $scope.vacateflag = false;
        //======================================Edit 
        $scope.editstudetLV = function (user) {
            var data = {
                "HLHSALTC_Id": user.HLHSALTC_Id,
                "ASMAY_Id": user.ASMAY_Id,
                "HLMH_Id": user.HLMH_Id,
            }
            apiService.create("HostelAllotForCLGStudent/editdata", data).then(function (promise) {
                $scope.editlist = promise.editlist;
                $scope.HLHSALTC_Id = promise.editlist[0].hlhsaltC_Id;
                $scope.ASMAY_Id = promise.editlist[0].asmaY_Id;
                $scope.HLHSALT_AllotmentDate = new Date(promise.editlist[0].hlhsalT_AllotmentDate);
                $scope.HLMH_Id = promise.editlist[0].hlmH_Id;
                $scope.AMCST_Id = promise.editlist[0].amcsT_Id;
                $scope.HLMRCA_Id = promise.editlist[0].hlmrcA_Id;
                $scope.amcO_Id = promise.editlist[0].amcO_Id;
                $scope.amB_Id = promise.editlist[0].amB_Id;
                $scope.amsE_Id = promise.editlist[0].amsE_Id;
                $scope.acmS_Id = promise.editlist[0].acmS_Id;
                $scope.HRMRM_BedCapacity = promise.hrmrM_BedCapacity;
                $scope.HLHSALTC_AllotRemarks = promise.editlist[0].hlhsalTC_AllotRemarks;
                $scope.vacateflag = true;
                $scope.hlmF_Id = promise.floor_details[0].hlmF_Id;
                $scope.HRMRM_Id = promise.floor_details[0].hrmrM_Id;
                if ($scope.hlmF_Id != undefined && $scope.hlmF_Id != null && $scope.hlmF_Id != '') {
                    $scope.floor_list = promise.floor_list;
                    //$scope.hlmF_Id = promise.floor_list[0].hlmF_Id;
                    angular.forEach($scope.floor_list, function (dd) {
                        if (dd.hlmF_Id === parseInt($scope.hlmF_Id)) {
                            $scope.hlmF_Id = dd.hlmF_Id;
                        }
                    });
                    $scope.room_list = promise.room_list;
                    angular.forEach($scope.room_list, function (dd) {
                        if (dd.hrmrM_Id === parseInt($scope.HRMRM_Id)) {
                            $scope.HRMRM_Id = dd.hrmrM_Id;
                        }
                    });
                }
               

                if (promise.editlist[0].hlhsalT_VacateFlg == true) {
                    $scope.HLHSALTC_VacateFlg = 1;
                    $scope.HLHSALTC_VacatedDate = new Date(promise.editlist[0].hlhsalT_VacatedDate);
                    $scope.HLHSALTC_VacateRemarks = promise.editlist[0].hlhsalT_VacateRemarks;
                }
                if ($scope.HLMH_Id != undefined && $scope.HLMH_Id != null && $scope.HLMH_Id != '') {
                    //$scope.get_studInfo();
                    $scope.housewise_studentList = promise.housewise_studentList;
                    if (promise.housewise_studentList[0].HLHSREQC_VegMessFlg == true) {
                        $scope.HLHSREQC_VegMessFlg = 1;
                    }
                    if (promise.housewise_studentList[0].HLHSREQC_NonVegMessFlg == true) {
                        $scope.HLHSREQC_NonVegMessFlg = 1;
                    }
                    if (promise.housewise_studentList[0].HLHSREQC_ACRoomFlg == true) {
                        $scope.HLHSREQC_ACRoomFlg = 1;
                    }
                    if (promise.housewise_studentList[0].HLHSREQC_SingleRoomFlg == true) {
                        $scope.HLHSREQC_SingleRoomFlg = 1;
                    }
                    $scope.AMCST_Id = promise.housewise_studentList[0].AMCST_Id;
                    $scope.get_studetdetails(housewise_studentList);
                }
            });
        }
        //=======================================End
       //-===================== for view details in gridview============================///////////////
        $scope.display = false;
        $scope.fillsudentdata = function (user) {
          
            //$('#requestedstddetails').modal('show');          
            $scope.obj.HLMH_Id = user.HLMH_Id;
            $scope.floor();
            $scope.HLMRCA_Id = user.HLMRCA_Id;
            $scope.amcO_Id = user.AMCO_Id;
            $scope.amsE_Id = user.AMSE_Id;
            $scope.amB_Id = user.AMB_Id;
            $scope.acmS_Id = user.ACMS_Id;
            $scope.roomcategory = user.HLMRCA_Id;
            $scope.studentName = user.studentName;
            $scope.amcsT_RegistrationNo = user.AMCST_RegistrationNo;
            $scope.amcO_CourseName = user.AMCO_CourseName;
            $scope.amB_BranchName = user.AMB_BranchName;
            $scope.amsE_SEMName = user.AMSE_SEMName;
            $scope.amcsT_MobileNo = user.AMCST_MobileNo;
            $scope.amcsT_emailId = user.AMCST_emailId;
            $scope.HLHSREQC_Id = user.HLHSREQC_Id;
            $scope.AMCST_Id = user.AMCST_Id;
            $scope.HLHSREQC_Remarks = user.HLHSREQC_Remarks;
            $scope.HLHSALTC_AllotRemarks = user.HLHSALTC_AllotRemarks;
            $scope.HLMRCA_RoomCategory = user.HLMRCA_RoomCategory;
            $scope.HRMRM_SharingFlg = user.HRMRM_SharingFlg;
            $scope.HRMRM_RoomNo = user.HRMRM_RoomNo;
            $scope.obj.HRMRM_Id = user.HRMRM_Id;
            $scope.HLHSALTC_AllotmentDate = user.HLHSALTC_AllotmentDate;
            $scope.HLHSREQCC_ACRoomFlg = user.HLHSREQCC_ACRoomFlg;
            $scope.HLHSREQCC_NonVegMessFlg = user.HLHSREQCC_NonVegMessFlg;
            $scope.HLHSREQCC_VegMessFlg = user.HLHSREQCC_VegMessFlg;
            $scope.HLHSALTC_EntireRoomReqdFlg = user.HLHSALTC_EntireRoomReqdFlg;
            
            $scope.scroll();
        }

        $scope.fillsudentdataedit = function (user) {           
            $scope.display = true;
            $scope.obj.HLMH_Id = user.HLMH_Id;
            $scope.floor();
            $scope.HLMRCA_Id = user.HLMRCA_Id;
            $scope.amcO_Id = user.AMCO_Id;
            $scope.amsE_Id = user.AMSE_Id;
            $scope.amB_Id = user.AMB_Id;
            $scope.acmS_Id = user.ACMS_Id;
            $scope.roomcategory = user.HLMRCA_Id;
            $scope.studentName = user.studentName;
            $scope.amcsT_RegistrationNo = user.AMCST_RegistrationNo;
            $scope.amcO_CourseName = user.AMCO_CourseName;
            $scope.amB_BranchName = user.AMB_BranchName;
            $scope.amsE_SEMName = user.AMSE_SEMName;
            $scope.amcsT_MobileNo = user.AMCST_MobileNo;
            $scope.amcsT_emailId = user.AMCST_emailId;
            $scope.HLHSREQC_Id = user.HLHSREQC_Id;
            $scope.AMCST_Id = user.AMCST_Id;
            $scope.HLHSREQC_Remarks = user.HLHSREQC_Remarks;
            $scope.HLHSALTC_AllotRemarks = user.HLHSALTC_AllotRemarks;
            $scope.HLMRCA_RoomCategory = user.HLMRCA_RoomCategory;
            $scope.HRMRM_SharingFlg = user.HRMRM_SharingFlg;
            $scope.HRMRM_RoomNo = user.HRMRM_RoomNo;
            $scope.obj.HRMRM_Id = user.HRMRM_Id;
            $scope.HLHSALTC_AllotmentDate = user.HLHSALTC_AllotmentDate;

            if (user.HLHSREQC_VegMessReqdFlg == true) {
                $scope.obj.HLHSREQC_VegMessReqdFlg = 1;
            }
            if (user.HLHSREQC_NonVegMessReqdFlg == true) {
                $scope.obj.HLHSREQC_NonVegMessReqdFlg = 1;
            }
            if (user.HLHSREQC_ACRoomReqdFlg == true) {
                $scope.obj.HLHSREQC_ACRoomReqdFlg = 1;
            }
            if (user.HLHSREQC_EntireRoomReqdFlg == true) {
                $scope.obj.HLHSREQC_EntireRoomReqdFlg = 1;
            }
            $scope.scroll();
        }

        $scope.getreport = function () {
            $scope.submitted = false;
            $scope.Temp_Floor_List = [];
            $scope.Temp_RoomCategory_List = [];
            $scope.griddata = [];
            angular.forEach($scope.floor_list, function (dd) {
                if (dd.hlmF_Id == $scope.obj.hlmF_Id) {
                    $scope.Temp_Floor_List.push({ HLMF_Id: dd.hlmF_Id, HRMF_FloorName: dd.hrmF_FloorName });
                }
            });

            angular.forEach($scope.roomcatgry_list, function (dd) {
                if (dd.hlmrcA_Id == $scope.obj.HLMRCA_Id) {
                    $scope.Temp_RoomCategory_List.push({ HLMRCA_Id: dd.hlmrcA_Id, HLMRCA_RoomCategory: dd.hlmrcA_RoomCategory });
                }
            });

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "HLMH_Id": $scope.obj.HLMH_Id,
                "Floor_DTO_Temp": $scope.Temp_Floor_List,
                "Room_Category_DTO_Temp": $scope.Temp_RoomCategory_List
            };
            apiService.create("Hostel_Allotment_Report/Get_GP_Report", data).then(function (promise) {
                if (promise !== null) {

                    if (promise.griddata !== null && promise.griddata.length > 0) {
                        $scope.griddata = promise.griddata;

                        $scope.Floor_Room_List = [];

                        angular.forEach($scope.Temp_Floor_List, function (flr) {
                            $scope.Floor_Room_List = [];
                            angular.forEach($scope.griddata, function (dd) {
                                if (flr.HLMF_Id === dd.HLMF_Id) {
                                    var status = "";
                                    var status_color = "";
                                    if (dd.AllotedBedsCount === 0) {
                                        status = "Available";
                                        status_color = "#52b7e9";
                                    }
                                    else {
                                        if (dd.AllotedBedsCount === dd.HRMRM_BedCapacity) {
                                            status = "Fully Occupied";
                                            status_color = "#242444";
                                        }
                                        else if (dd.AllotedBedsCount < dd.HRMRM_BedCapacity) {
                                            status = "Partially Occupied";
                                            status_color = "#46648c";
                                        }
                                    }

                                    $scope.Floor_Room_List.push({
                                        HLMF_Id: dd.HLMF_Id, HRMF_FloorName: dd.HRMF_FloorName,
                                        HRMRM_Id: dd.HRMRM_Id, HRMRM_RoomNo: dd.HRMRM_RoomNo,
                                        HLMRCA_Id: dd.HLMRCA_Id, HLMRCA_RoomCategory: dd.HLMRCA_RoomCategory,
                                        HLMH_Id: dd.HLMH_Id, HLMH_Name: dd.HLMH_Name,
                                        HRMRM_BedCapacity: dd.HRMRM_BedCapacity, AllotedBedsCount: dd.AllotedBedsCount,
                                        AvailableBedsCount: dd.AvailableBedsCount, RoomStatus: status, RoomBGColor: status_color
                                    })
                                }
                            });

                            flr.room_details = $scope.Floor_Room_List;
                        });
                    }
                }
            });
        };
        $scope.newallocation = true;
        $scope.allocatedstd = true;
        $scope.ViewAllotedStudentDetails = function (room_obj) {
            $scope.getstudentalloteddata = [];
            $scope.hostelname = room_obj.HLMH_Name;
            $scope.floorname = room_obj.HRMF_FloorName;
            $scope.roomcategoryname = room_obj.HLMRCA_RoomCategory;
            $scope.roonno = room_obj.HRMRM_RoomNo;
            $scope.HRMRM_Id = room_obj.HRMRM_Id;
            $scope.HLMRCAC_To_HRMRM_Id = room_obj.HRMRM_Id;
            $scope.AllotedBedsCount_Temp = room_obj.AllotedBedsCount;
            $scope.HRMRM_BedCapacity = room_obj.HRMRM_BedCapacity;
            $scope.AllotedBedsCount = room_obj.AllotedBedsCount;
            $scope.AvailableBedsCount = room_obj.AvailableBedsCount;

            if (room_obj.RoomStatus == 'Available') {
                $scope.newallocation = true;
                $scope.allocatedstd = false;
                $scope.allotment = 'newallocation';
            }
            else if (room_obj.RoomStatus == 'Fully Occupied') {
                $scope.allocatedstd = true;
                $scope.newallocation = false;
                $scope.allotment = 'alltedstd';
            }
            else if (room_obj.RoomStatus == 'Partially Occupied') {
                $scope.allocatedstd = true;
                $scope.newallocation = true;
                $scope.allotment = 'alltedstd';
            }           
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "HLMH_Id": room_obj.HLMH_Id,
                "HLMF_Id": room_obj.HLMF_Id,
                "HLMRCA_Id": room_obj.HLMRCA_Id,
                "HRMRM_Id": room_obj.HRMRM_Id
            };

            apiService.create("Hostel_Allotment_Report/Get_GP_RoomWise_StudentAlloted_Details", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.getstudentalloteddata !== null && promise.getstudentalloteddata.length > 0) {
                        $scope.getstudentalloteddata = promise.getstudentalloteddata;
                        $scope.institution_flag = promise.institution_flag;
                        $scope.allotment = 'alltedstd'
                        $('#mymodalviewstudentdetails').modal('show');
                    }
                    else {
                        $('#mymodalviewstudentdetails').modal('show');
                        //swal("Still Allotment Not Done For This Room");
                    }
                    $timeout(function () { $scope.get_studInfo('Request'); }, 500);
                }
            });
            
        };

        $scope.scroll = function () {
            $("html, body").animate({ scrollDown: 0 }, 1000);
        };

        $scope.Clearid = function () {
            $state.reload();
        }


        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.Refresh = function (field) {
            $scope.display = false;
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }
        //two
        $scope.fillsudentdatatwo = function (user) {
            if ($scope.housewise_studentList != null && $scope.housewise_studentList.length > 0) {
                $scope.display = true;
                for (var i = 0; i < $scope.housewise_studentList.length; i++) {
                    if ($scope.housewise_studentList[i].AMCST_Id == user) {
                        
                       // $scope.HLMH_Id = $scope.housewise_studentList[i].HLMH_Id;
                        $scope.obj.HLMH_Id = "";
                        $scope.HLMRCA_Id = $scope.housewise_studentList[i].HLMRCA_Id;

                        $scope.amcO_Id = $scope.housewise_studentList[i].AMCO_Id;
                        $scope.amsE_Id = $scope.housewise_studentList[i].AMSE_Id;
                        $scope.amB_Id = $scope.housewise_studentList[i].AMB_Id;
                        $scope.acmS_Id = $scope.housewise_studentList[i].ACMS_Id;
                        $scope.roomcategory = $scope.housewise_studentList[i].HLMRCA_Id;
                        $scope.studentName = $scope.housewise_studentList[i].studentName;
                        $scope.amcsT_RegistrationNo = $scope.housewise_studentList[i].AMCST_RegistrationNo;
                        $scope.amcO_CourseName = $scope.housewise_studentList[i].AMCO_CourseName;
                        $scope.amB_BranchName = $scope.housewise_studentList[i].AMB_BranchName;
                        $scope.amsE_SEMName = $scope.housewise_studentList[i].AMSE_SEMName;
                        $scope.amcsT_MobileNo = $scope.housewise_studentList[i].AMCST_MobileNo;
                        $scope.amcsT_emailId = $scope.housewise_studentList[i].AMCST_emailId;
                        $scope.HLHSREQC_Id = $scope.housewise_studentList[i].HLHSREQC_Id;
                        $scope.AMCST_Id = $scope.housewise_studentList[i].AMCST_Id;
                        $scope.HLHSREQC_Remarks = $scope.housewise_studentList[i].HLHSREQC_Remarks;
                        $scope.HLHSALTC_AllotRemarks = $scope.housewise_studentList[i].HLHSALTC_AllotRemarks;
                        $scope.HLMRCA_RoomCategory = $scope.housewise_studentList[i].HLMRCA_RoomCategory;
                        $scope.HRMRM_SharingFlg = $scope.housewise_studentList[i].HRMRM_SharingFlg;
                        $scope.HRMRM_RoomNo = $scope.housewise_studentList[i].HRMRM_RoomNo;
                        $scope.HRMRM_Id_Old = $scope.housewise_studentList[i].HRMRM_Id;
                        $scope.HLHSALTC_AllotmentDate = $scope.housewise_studentList[i].HLHSALTC_AllotmentDate;

                        if (i.HLHSREQC_VegMessReqdFlg == true) {
                            $scope.obj.HLHSREQC_VegMessReqdFlg = 1;
                        }
                        if (i.HLHSREQC_NonVegMessReqdFlg == true) {
                            $scope.obj.HLHSREQC_NonVegMessReqdFlg = 1;
                        }
                        if (i.HLHSREQC_ACRoomReqdFlg == true) {
                            $scope.obj.HLHSREQC_ACRoomReqdFlg = 1;
                        }
                        if (i.HLHSREQC_EntireRoomReqdFlg == true) {
                            $scope.obj.HLHSREQC_EntireRoomReqdFlg = 1;
                        }
                        $scope.scroll();
                        return;
                    }
                }
            }
            
            
        }
    }
})();