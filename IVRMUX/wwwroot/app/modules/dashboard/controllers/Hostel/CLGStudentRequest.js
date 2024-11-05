
(function () {
    'use strict';
    angular
        .module('app')
        .controller('CLGStudentRequest', CLGStudentRequest)
    CLGStudentRequest.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter']
    function CLGStudentRequest($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter) {


        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;
        $scope.page1 = "page1";
        $scope.search = " ";
        $scope.hlhsreqC_ACRoomFlg = 0;
        $scope.hlhsreqC_SingleRoomFlg = 0;
        $scope.hlhsreqC_VegMessFlg = 0;
        $scope.hlhsreqC_NonVegMessFlg = 0;
        $scope.hlhsreqC_Id = 0;
        $scope.amcsT_Id = 0;

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;
            $scope.reverse = !$scope.reverse;
        };
        $scope.submitted = false;
        $scope.preferredpartnar = false;
        $scope.HLHSREQ_RequestDate = new Date();
        $scope.save = function () {

            var RequestDate = $scope.HLHSREQ_RequestDate === null ? "" : $filter('date')($scope.HLHSREQ_RequestDate, "yyyy-MM-dd");
            var HLHSREQC_Id = 0;
            if ($scope.hlhsreqC_Id > 0) {
                HLHSREQC_Id = $scope.hlhsreqC_Id;
            }
            if ($scope.myForm.$valid) {
                var data = {
                    "HLHSREQC_Id": HLHSREQC_Id,
                    "HLHSREQC_RequestDate": RequestDate,
                    "HLMH_Id": $scope.HLMH_Id,
                    "HLMRCA_Id": $scope.HLMRCA_Id,
                    "AMCST_Id": $scope.AMCST_Id,
                    "HLHSREQC_ACRoomReqdFlg": $scope.hlhsreqC_ACRoomReqdFlg,
                    "HLHSREQC_EntireRoomReqdFlg": $scope.hlhsreqC_EntireRoomReqdFlg,
                    "HLHSREQC_VegMessReqdFlg": $scope.hlhsreqC_VegMessReqdFlg,
                    "HLHSREQC_NonVegMessReqdFlg": $scope.hlhsreqC_NonVegMessReqdFlg,
                    "HLHSREQC_Remarks": $scope.HLHSREQC_Remarks,
                    "HRMRM_Id": $scope.HRMRM_Id
                   
                  
                };

                apiService.create("CLGStudentRequest/save", data).then(function (promise) {
                    if (promise.returnval === true) {
                        swal("Data Saved Successfully....!!!!");
                        $state.reload();
                    } 
                    else if (promise.duplicate === true) {
                        swal("Record Already Exist!");
                    }
                   
                    else if (promise.returnupdate === 'updated') {
                        swal("Data Updated Successfully.!");
                        $state.reload();
                    }
                    else if (promise.returnupdate === 'failed') {
                        swal("Data Not Updated Successfully...!!!!");
                    } 
                    else if (promise.returnval === false) {
                        swal(" Data Not Saved Successfully...!!!!");
                    }
                   
                });
            }
            else {
                $scope.submitted = true;
            }
        };

       $scope.search = '';
        $scope.filtervalue1 = function (user) {
        };
        $scope.loaddata = function () {
            var data = {
            };
            apiService.create("CLGStudentRequest/loaddata", data).then(function (promise) {
                $scope.hostel_list = promise.hostel_list;
                
                $scope.student_wisedata = promise.student_wisedata;
                // if ($scope.student_wisedata.length > 0) {

                $scope.stdName = $scope.student_wisedata[0].studentName;
                $scope.AMCST_Id = $scope.student_wisedata[0].AMCST_Id;
                $scope.AMCO_CourseName = $scope.student_wisedata[0].AMCO_CourseName;
                $scope.AMCO_CourseCode = $scope.student_wisedata[0].AMCO_CourseCode;
                $scope.AMB_BranchName = $scope.student_wisedata[0].AMB_BranchName;
                $scope.AMB_BranchCode = $scope.student_wisedata[0].AMB_BranchCode;
                $scope.AMCST_RegistrationNo = $scope.student_wisedata[0].AMCST_RegistrationNo;
                $scope.AMCST_AdmNo = $scope.student_wisedata[0].AMCST_AdmNo;
                $scope.ACYST_RollNo = $scope.student_wisedata[0].ACYST_RollNo;
                $scope.ASMAY_Id = $scope.student_wisedata[0].ASMAY_Id;
                $scope.AMSE_SEMName = $scope.student_wisedata[0].AMSE_SEMName;
                $scope.AMSE_SEMCode = $scope.student_wisedata[0].AMSE_SEMCode;
                $scope.ACMS_SectionName = $scope.student_wisedata[0].ACMS_SectionName;
                $scope.ACMS_SectionCode = $scope.student_wisedata[0].ACMS_SectionCode;

                // }

                $scope.all_requestdata = promise.all_requestdata;

               
              

               // $scope.HRMRM_RoomNo = $scope.all_requestdata[0].HRMRM_RoomNo;

            });
        };

        $scope.vacateDetails = function () {
            $('#vacateddetails').modal('show');
            $scope.vacateremarks = $scope.all_requestdata[0].HLHSALTC_VacateRemarks;
            $scope.vacateredate = $scope.all_requestdata[0].HLHSALTC_VacatedDate;
        };

        $scope.edittab1 = function (user) {

            var data = {
                "HLHSREQC_Id": user.HLHSREQC_Id
            };
            apiService.create("CLGStudentRequest/edittab1", data).then(function (promise) {
                $scope.hlhsreqC_Id = promise.editlist[0].hlhsreqC_Id;
                $scope.HLHSREQC_RequestDate = new Date(promise.editlist[0].hlhsreQC_RequestDate);
                $scope.HLMH_Id = promise.editlist[0].hlmH_Id;
                $scope.HLMRCA_Id = promise.editlist[0].hlmrcA_Id;
                $scope.AMCST_Id = promise.editlist[0].amcsT_Id;
                $scope.roomdetails();
                $scope.Catgory();
               
                
              
                if (promise.editlist[0].hlhsreqC_ACRoomReqdFlg === true) {
                    $scope.hlhsreqC_ACRoomReqdFlg = true;
                }
                else {
                    $scope.hlhsreqC_ACRoomReqdFlg = false;
                }
                if (promise.editlist[0].hlhsreqC_EntireRoomReqdFlg === true) {
                    $scope.hlhsreqC_EntireRoomReqdFlg = true;
                }
                else {
                    $scope.hlhsreqC_EntireRoomReqdFlg = false;
                }
                if (promise.editlist[0].hlhsreqC_VegMessReqdFlg === true) {
                    $scope.hlhsreqC_VegMessReqdFlg = true;
                }
                else {
                    $scope.hlhsreqC_VegMessReqdFlg = false;
                }
                if (promise.editlist[0].hlhsreqC_NonVegMessReqdFlg === true) {
                    $scope.hlhsreqC_NonVegMessReqdFlg = true;
                }
                else {
                    $scope.hlhsreqC_NonVegMessReqdFlg = false;
                }
                $scope.HLHSREQC_Remarks = promise.editlist[0].hlhsreqC_Remarks;

            });
        };

        $scope.roomdetails = function () {
            var data = {
                "HLMH_Id": $scope.HLMH_Id,
                "HLMRCA_Id": $scope.HLMRCA_Id
            };
            apiService.create("CLGStudentRequest/roomdetails", data).then(function (promise) {
                if (promise.roomdetails != null && promise.roomdetails.length>0)
                {
                    $scope.singlebedrate = promise.roomdetails[0].hlmrcA_SORate;
                    $scope.multibedrate = promise.roomdetails[0].hlmrcA_RoomRate;                     
                }

                if (promise.studentList.length > 0) {
                    $scope.housewise_studentList = promise.studentList;
                }

            });
        };


        $scope.Catgory = function () {
            var data = {
                "HLMH_Id": $scope.HLMH_Id,                
            };
            apiService.create("CLGStudentRequest/Catgory", data).then(function (promise) {
                if (promise.room_list != null && promise.room_list.length > 0) {
                    $scope.room_list = promise.room_list;
                }
                
                if (promise.facility_list != null && promise.facility_list.length > 0) {
                    $scope.facility_list = promise.facility_list;
                }               
            });
        };
        $scope.getPdetails = function () {
            var data = {
                "HLMH_Id": $scope.HLMH_Id,
                "HLMRCA_Id": $scope.HLMRCA_Id,
                "HRMRM_Id": $scope.HRMRM_Id,
               //  "AMCST_Id": $scope.obj.AMCST_Id
            };
            apiService.create("CLGStudentRequest/getPdetails", data).then(function (promise) {
                if (promise.room_Details != null && promise.room_Details.length > 0) {
                    $scope.room_Details = promise.room_Details;

                    $scope.AvailableBedCapacity = $scope.room_Details[0].AvailableBedCapacity;
                    $scope.AllotedCount = $scope.room_Details[0].AllotedCount;
                    $scope.HRMRM_BedCapacity = $scope.room_Details[0].HRMRM_BedCapacity;
                    $scope.HRMRM_RoomNo = $scope.room_Details[0].HRMRM_RoomNo;


                    if ($scope.AvailableBedCapacity <= 0) {
                        $scope.desabled = true;
                    }
                    else {
                        $scope.desabled = false;

                    }
                }

                
            });
        };


        $scope.fillsudentdatatwo = function (user) {
            if ($scope.housewise_studentList != null && $scope.housewise_studentList.length > 0) {
                $scope.display = true;
                for (var i = 0; i < $scope.housewise_studentList.length; i++) {
                    if ($scope.housewise_studentList[i].AMCST_Id == user.AMCST_Id) {
                        $scope.HLMH_IdNow = $scope.housewise_studentList[i].HLMH_Id;
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
                        $scope.HRMRM_Id = $scope.housewise_studentList[i].HRMRM_Id;
                        $scope.HLHSALTC_AllotmentDate = $scope.housewise_studentList[i].HLHSALTC_AllotmentDate;
                        $scope.getPdetails()
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

        $scope.deactivYTab1 = function (usersem, SweerAlert) {
            debugger;

            var dystring = "";
            if (usersem.HLHSREQC_ActiveFlag === true) {
                dystring = "Deactivate";
            }
            else if (usersem.HLHSREQC_ActiveFlag === false) {
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
                        apiService.create("CLGStudentRequest/deactive", usersem).
                            then(function (promise) {
                                if (promise.ret === true) {
                                    swal("Record " + dystring + "d" + " Successfully!!!");
                                    $state.reload();
                                }
                                else {

                                    swal("Record Not " + dystring + "d " + "Successfully!!!");
                                    $state.reload();
                                }
                            });
                    }
                    else {
                        swal("Record Deactivation Cancelled!!!");
                    }

                });
        };
        $scope.Clearid = function () {
            $state.reload();
        };
    }
})();





