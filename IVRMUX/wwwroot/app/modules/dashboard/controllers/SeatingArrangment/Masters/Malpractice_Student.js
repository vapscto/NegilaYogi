(function () {
    'use strict';
    angular.module('app').controller('malpractice_StudentController', malpractice_StudentController)

    function malpractice_StudentController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, uiCalendarConfig, superCache, $window, $http) {
        $scope.tempcldrlst = [];

        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.search = "";

        $scope.loaddata = function () {
            var id = 1;
            apiService.getURI("SAMasterSuperintendent/load_MPS", id).then(function (promise) {
                $scope.yearlst = promise.yearlst;
                $scope.examlist = promise.examlist;
                $scope.university_examlist = promise.university_examlist;               
               
                $scope.roomlist = promise.roomlist;
                $scope.slotlist = promise.slotlist;

                if (promise.malpracticestudentlist !== null || promise.malpracticestudentlist > 0) {
                    $scope.malpracticestudentlist = promise.malpracticestudentlist;
                }
            });
        };

        $scope.GetCourse = function () {
            $scope.branchlist = [];
            $scope.semesterlist = [];
            $scope.courselist = [];
            $scope.AMCO_Id = "";
            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.studentlist = [];
            $scope.AMCST_Id = "";
            $scope.subjectlist = [];
            $scope.ISMS_Id = "";

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };
            apiService.create("SAMasterSuperintendent/GetCourse", data).then(function (promise) {
                if (promise !== null) {
                    $scope.courselist = promise.courselist;
                }
            });
        };

        $scope.GetBranch = function () {
            $scope.branchlist = [];
            $scope.semesterlist = [];           
            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.studentlist = [];
            $scope.AMCST_Id = "";
            $scope.subjectlist = [];
            $scope.ISMS_Id = "";

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id
            };
            apiService.create("SAMasterSuperintendent/GetBranch", data).then(function (promise) {
                if (promise !== null) {
                    $scope.branchlist = promise.branchlist;
                }
            });
        };

        $scope.GetSemester = function () {
            $scope.semesterlist = [];
            $scope.AMSE_Id = "";
            $scope.studentlist = [];
            $scope.AMCST_Id = "";
            $scope.subjectlist = [];
            $scope.ISMS_Id = "";

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id
            };
            apiService.create("SAMasterSuperintendent/GetSemester", data).then(function (promise) {
                if (promise !== null) {
                    $scope.semesterlist = promise.semesterlist;
                }
            });
        };

        $scope.GetSubject = function () {
            $scope.studentlist = [];
            $scope.AMCST_Id = "";
            $scope.subjectlist = [];
            $scope.ISMS_Id = "";

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id
            };
            apiService.create("SAMasterSuperintendent/GetSubject", data).then(function (promise) {
                if (promise.subjectlist !== null && promise.subjectlist.length > 0) {
                    $scope.subjectlist = promise.subjectlist;
                }
            });

        };

        $scope.GetStudent = function () {
            $scope.studentlist = [];
            $scope.AMCST_Id = "";

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMCO_Id": $scope.AMCO_Id,
                "AMB_Id": $scope.AMB_Id,
                "AMSE_Id": $scope.AMSE_Id,
                "ISMS_Id": $scope.ISMS_Id,
                "ESAUE_Id": $scope.ESAUE_Id,
                "EME_Id": $scope.EME_Id,
                "Flag" : "Malpratice"
            };
            apiService.create("SAMasterSuperintendent/GetStudent", data).then(function (promise) {
                if (promise.studentlist !== null || promise.studentlist.length > 0) {
                    $scope.studentlist = promise.studentlist;
                }
            });
        };
        
        $scope.saveData = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "ESAMALSTU_Id": $scope.ESAMALSTU_Id,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "EME_Id": $scope.EME_Id,
                    "ESAUE_Id": $scope.ESAUE_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "AMCST_Id": $scope.AMCST_Id,
                    "ISMS_Id": $scope.ISMS_Id,
                    "ESAROOM_Id": $scope.ESAROOM_Id,
                    "ESAESLOT_Id": $scope.ESAESLOT_Id,
                    "ESAMALSTU_StudentUSN": $scope.ESAMALSTU_StudentUSN,
                    "ESAMALSTU_LABTHEORYFlg": $scope.ESAMALSTU_LABTHEORYFlg,
                    "ESAMALSTU_ExamDate": $scope.ESAMALSTU_ExamDate
                };

                apiService.create("SAMasterSuperintendent/Save_MPS", data).then(function (promise) {
                    if (promise.message === "Add") {
                        swal('Record Save Successfully.');
                    }
                    else if (promise.message === "Update") {
                        swal('Record Update Successfully.');
                    }
                    if (promise.message === "Error") {
                        swal('Record Save/Update Failed..!!.');
                    }
                    $state.reload();
                });
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.Edit = function (obj) {

            var data = {
                "ESAMALSTU_Id": obj.ESAMALSTU_Id,
                "ASMAY_Id": obj.ASMAY_Id
            };
            apiService.create("SAMasterSuperintendent/Edit_MPS", data).then(function (promise) {
                if (promise.edit_mpslist !== null && promise.edit_mpslist.length > 0) {

                    $scope.editflag = true;

                    $scope.yearlst = promise.yearlst;
                    $scope.examlist = promise.examlist;
                    $scope.university_examlist = promise.university_examlist;
                    $scope.courselist = promise.courselist;
                    $scope.branchlist = promise.branchlist;
                    $scope.semesterlist = promise.semesterlist;
                    $scope.subjectlist = promise.subjectlist;
                    $scope.roomlist = promise.roomlist;
                    $scope.slotlist = promise.slotlist;
                    $scope.studentlist = promise.studentlist;

                    $scope.ESAMALSTU_Id = promise.edit_mpslist[0].esamalstU_Id;
                    $scope.ASMAY_Id = promise.edit_mpslist[0].asmaY_Id;
                    $scope.EME_Id = promise.edit_mpslist[0].emE_Id;
                    $scope.ESAUE_Id = promise.edit_mpslist[0].esauE_Id;
                    $scope.AMCO_Id = promise.edit_mpslist[0].amcO_Id;
                    $scope.AMB_Id = promise.edit_mpslist[0].amB_Id;
                    $scope.AMSE_Id = promise.edit_mpslist[0].amsE_Id;
                    $scope.AMCST_Id = promise.edit_mpslist[0].amcsT_Id;
                    $scope.ISMS_Id = promise.edit_mpslist[0].ismS_Id;
                    $scope.ESAROOM_Id = promise.edit_mpslist[0].esarooM_Id;
                    $scope.ESAESLOT_Id = promise.edit_mpslist[0].esaesloT_Id;
                    $scope.ESAMALSTU_StudentUSN = promise.edit_mpslist[0].esamalstU_StudentUSN;
                    $scope.ESAMALSTU_LABTHEORYFlg = promise.edit_mpslist[0].esamalstU_LABTHEORYFlg;
                    $scope.ESAMALSTU_ExamDate = new Date(promise.edit_mpslist[0].esamalstU_ExamDate);                    
                }
            });
        };

        $scope.DeleteStudent = function (DeleteRecord) {           
            var data = {
                "ESAMALSTU_Id": DeleteRecord.ESAMALSTU_Id                
            };

            swal({
                title: "Are you sure?",
                text: "Do you want to delete this record...!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete It!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("SAMasterSuperintendent/DeleteMalPraticeStudent", data).then(function (promise) {
                            if (promise.message === 'true') {
                                swal("Record Deleted Successfully");
                            }
                            else {
                                swal("Failed To Delete Record");
                            }
                            $state.reload();
                        });
                    } else {
                        swal("Cancelled");
                    }

                });
        };

        $scope.cleardata = function () {
            $state.reload();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
    }
})();