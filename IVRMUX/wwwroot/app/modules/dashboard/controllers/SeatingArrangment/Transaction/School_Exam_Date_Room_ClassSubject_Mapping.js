(function () {
    'use strict';
    angular.module('app').controller('School_Exam_Date_Room_ClassSubject_MappingController', School_Exam_Date_Room_ClassSubject_MappingController)

    School_Exam_Date_Room_ClassSubject_MappingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'uiCalendarConfig', 'superCache', '$window', '$http']

    function School_Exam_Date_Room_ClassSubject_MappingController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, uiCalendarConfig, superCache, $window, $http) {
        $scope.obj = {};
        $scope.tempcldrlst = [];
        $scope.searchbtn = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.search = "";
        //$scope.ESAEDATESCH_ExamDate = new Date();

        $scope.GetRoomList = [];
        $scope.GetExamDateloaddata = function () {
            var id = 1;
            apiService.getURI("School_Exam_Date_Room/GetExamDateRoomClassMappingloaddata", id).then(function (promise) {
                $scope.yearlst = promise.getAcademicYearList;
                $scope.examlist = promise.getExamList;
                $scope.slotlist = promise.getExamSlotList;
                $scope.ASMAY_Id = promise.asmaY_Id;
                $scope.GetSavedDetails = promise.getSavedDetails;

                angular.forEach($scope.yearlst, function (dd) {
                    if (dd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                        $scope.mindate = new Date(dd.asmaY_From_Date);
                        $scope.maxdate = new Date(dd.asmaY_To_Date);
                    }
                });
            });
        };

        $scope.OnChangeyear = function () {
            $scope.GetRoomList = [];
            $scope.GetaSavedRoomDetails = [];
            $scope.EME_Id = "";
            $scope.ESAROOM_Id = "";
            $scope.ESAESLOT_Id = "";
            $scope.submitted1 = false;
            $scope.submitted = false;
            $scope.obj.searchValue = '';
            $scope.ESARMSCH_ExamDate = null;
            angular.forEach($scope.yearlst, function (dd) {
                if (dd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                    $scope.mindate = new Date(dd.asmaY_From_Date);
                    $scope.maxdate = new Date(dd.asmaY_To_Date);
                }
            });
        };

        $scope.OnChangeexam = function () {
            $scope.GetRoomList = [];
            $scope.ESAUE_Id = "";
            $scope.ESAESLOT_Id = "";
            $scope.ESAROOM_Id = "";
            $scope.GetaSavedRoomDetails = [];
            $scope.submitted1 = false;
            $scope.submitted = false;
            $scope.obj.searchValue = '';
            $scope.ESARMSCH_ExamDate = null;
        };

        $scope.OnChangeslot = function () {
            $scope.GetRoomList = [];
            $scope.ESAROOM_Id = "";
            $scope.GetaSavedRoomDetails = [];
            $scope.submitted1 = false;
            $scope.submitted = false;
            $scope.obj.searchValue = '';
            $scope.ESARMSCH_ExamDate = null;
        };

        $scope.OnChangeDate = function () {
            $scope.GetRoomList = [];
            $scope.ESAROOM_Id = "";           
            $scope.submitted1 = false;
            $scope.submitted = false;
            $scope.obj.searchValue = '';

            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "EME_Id": $scope.EME_Id,
                "ESAUE_Id": $scope.ESAUE_Id,
                "ESAESLOT_Id": $scope.ESAESLOT_Id,
                "ESARMSCH_ExamDate": new Date($scope.ESARMSCH_ExamDate).toDateString()
            };

            apiService.create("School_Exam_Date_Room/GetSubjectListRoomClassMapping", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.getRoomList !== null && promise.getRoomList.length > 0) {
                        $scope.GetRoomList = promise.getRoomList;
                    } else {
                        $scope.ESARMSCH_ExamDate = null;
                        swal("Availability Room Not Mapped For This Selection");
                    }
                }
            });
        };

        $scope.GetSearchExamDateData = function () {           
            $scope.GetaSavedRoomDetails = [];
            $scope.obj.searchValue = '';
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "EME_Id": $scope.EME_Id,                     
                    "ESAESLOT_Id": $scope.ESAESLOT_Id,
                    "ESAROOM_Id": $scope.ESAROOM_Id,
                    "ESARMSCH_ExamDate": new Date($scope.ESARMSCH_ExamDate).toDateString()
                };
                apiService.create("School_Exam_Date_Room/GetSearchExamDateRoomClassMappingData", data).then(function (promise) {
                    if (promise !== null) {

                        $scope.GetRoomDetails = promise.getRoomDetails;
                        $scope.GetClassList = promise.getClassList;
                        $scope.GetSubjectList = promise.getSubjectList;
                        $scope.GetSavedClassSubjectList = promise.getSavedClassSubjectList;

                        $scope.BenchSize = $scope.GetRoomDetails[0].esarooM_BenchCapacity;

                        $scope.TempClassSubjectList = [];
                        for (var i = 0; i < $scope.BenchSize; i++) {
                            $scope.TempClassSubjectList.push({ Id: i, ESARMCSSCH_Id: 0 });
                        }

                        if ($scope.GetSavedClassSubjectList !== null && $scope.GetSavedClassSubjectList.length > 0) {
                            angular.forEach($scope.GetSavedClassSubjectList, function (dd, index) {
                                $scope.TempClassSubjectList[index].ISMS_Id = dd;
                                $scope.TempClassSubjectList[index].ASMCL_Id = dd.asmcL_Id;
                                $scope.TempClassSubjectList[index].ESARMCSSCH_Id = dd.esarmcsscH_Id;
                            });
                        }
                    }
                });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.OnChangeClass = function (d,i) {
            angular.forEach($scope.TempClassSubjectList, function (dd, index) {
                if (i !== index) {
                    if (dd.ASMCL_Id === d.ASMCL_Id) {
                        swal("Class Already Selected");
                        d.ASMCL_Id = "";
                    }
                }
            });
        };

        $scope.SaveExamDateData = function (objform) {
            if (objform.$valid) {
                $scope.School_Room_ClassSubject_Temp_Details = [];

                angular.forEach($scope.TempClassSubjectList, function (dd) {
                    $scope.School_Room_ClassSubject_Temp_Details.push({ ESARMCSSCH_Id: dd.ESARMCSSCH_Id, ASMCL_Id: dd.ASMCL_Id, ISMS_Id: dd.ISMS_Id.ismS_Id });
                });

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ESARMSCH_Id": $scope.ESARMSCH_Id,
                    "EME_Id": $scope.EME_Id,
                    "ESAESLOT_Id": $scope.ESAESLOT_Id,
                    "ESAROOM_Id": $scope.ESAROOM_Id,
                    "ESARMSCH_ExamDate": new Date($scope.ESARMSCH_ExamDate).toDateString(),
                    "School_Room_ClassSubject_Temp_Details": $scope.School_Room_ClassSubject_Temp_Details
                };
                apiService.create("School_Exam_Date_Room/SaveExamDateRoomClassMappingData", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.returnValue === true) {
                            swal("Record Saved/Updated Successfully");
                        } else {
                            swal("Failed To Save/Update Record");
                        }
                        $scope.cleardata();
                    }

                });
            } else {
                $scope.submitted1 = true;
            }
        };

        $scope.EditExamDateData = function (dd) {
            $scope.GetEditedDateDetails = [];
            $scope.GetEditedRoomDetails = [];
            $scope.Getroomdetails = [];

            var data = {
                "ESARMSCH_Id": dd.esarmscH_Id
            };

            apiService.create("School_Exam_Date_Room/EditExamDateRoomClassMappingData", data).then(function (promise) {

                if (promise !== null) {

                    $scope.GetRoomDetails = promise.getRoomDetails;
                    $scope.GetClassList = promise.getClassList;
                    $scope.GetSubjectList = promise.getSubjectList;
                    $scope.GetSavedClassSubjectList = promise.getSavedClassSubjectList;
                    $scope.GetEditDetails = promise.getEditDetails;
                    $scope.GetRoomList = promise.getRoomList;

                    $scope.BenchSize = $scope.GetRoomDetails[0].esarooM_BenchCapacity;
                    $scope.EME_Id = $scope.GetEditDetails[0].emE_Id;
                    $scope.ESAROOM_Id = $scope.GetEditDetails[0].esarooM_Id;
                    $scope.ESAESLOT_Id = $scope.GetEditDetails[0].esaesloT_Id;
                    $scope.ESARMSCH_Id = $scope.GetEditDetails[0].esarmscH_Id;
                    $scope.ESARMSCH_ExamDate = new Date($scope.GetEditDetails[0].esarmscH_ExamDate);

                    $scope.TempClassSubjectList = [];
                    for (var i = 0; i < $scope.BenchSize; i++) {
                        $scope.TempClassSubjectList.push({ Id: i, ESARMCSSCH_Id: 0 });
                    }
                    $scope.searchbtn = true;
                    if ($scope.GetSavedClassSubjectList !== null && $scope.GetSavedClassSubjectList.length > 0) {
                        angular.forEach($scope.GetSavedClassSubjectList, function (dd, index) {
                            $scope.TempClassSubjectList[index].ISMS_Id = dd;
                            $scope.TempClassSubjectList[index].ASMCL_Id = dd.asmcL_Id;
                            $scope.TempClassSubjectList[index].ESARMCSSCH_Id = dd.esarmcsscH_Id;
                        });
                    }
                }
            });
        };

        $scope.ViewRoomDetails = function (dd) {
            var data = {
                "ESARMSCH_Id": dd.esarmscH_Id
            };

            apiService.create("School_Exam_Date_Room/ViewExamDateRoomClassMappingDetails", data).then(function (promise) {
                if (promise !== null) {
                    $scope.GetViewRoomClassSubjectDetails = promise.getViewRoomClassSubjectDetails;

                    if ($scope.GetViewRoomClassSubjectDetails !== null && $scope.GetViewRoomClassSubjectDetails.length > 0) {
                        $('#mymodalviewroom').modal('show');
                    }
                }
            });
        };

        $scope.ActiveDeactiveExamDate = function (dd) {
            var mgs = "";
            var confirmmgs = "";

            if (dd.esarmscH_ActiveFlg === true) {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";
            } else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }

            var data = {
                "ESARMSCH_Id": dd.esarmscH_Id
            };

            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("School_Exam_Date_Room/ActiveDeactiveExamRoomClassMappingDate", data).then(function (promise) {
                            if (promise.message === "Mapped") {
                                swal("You Can't De-activate This Record,Record Already Mapped");
                            }
                            else {
                                if (promise.returnval === true) {
                                    swal("Record" + " " + confirmmgs + " " + "Successfully");
                                }
                                else {
                                    swal("Record" + " " + confirmmgs + " " + " Successfully");
                                }
                                $scope.cleardata();
                            }
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.ActiveDeactiveRoomDetails = function (dd) {
            var mgs = "";
            var confirmmgs = "";

            if (dd.esarmcssCH_ActiveFlg === true) {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";
            } else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }

            var data = {
                "ESARMCSSCH_Id": dd.esarmcsscH_Id,
                "ESARMSCH_Id": dd.esarmscH_Id
            };

            swal({
                title: "Are you sure",
                text: "Do you want to " + mgs + " record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("School_Exam_Date_Room/ActiveDeactiveExamDateRoomClassMappingDetails", data).then(function (promise) {
                            if (promise.message === "Mapped") {
                                swal("You Can't De-activate This Record,Record Already Mapped");
                            }
                            else {
                                if (promise.returnval === true) {
                                    swal("Record" + " " + confirmmgs + " " + "Successfully");
                                }
                                else {
                                    swal("Record" + " " + confirmmgs + " " + " Successfully");
                                }

                                $scope.GetViewRoomClassSubjectDetails = promise.getViewRoomClassSubjectDetails;
                            }
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.cleardata = function () {
            $scope.ESAROOM_Id = "";
            $scope.ASMAY_Id = "";
            $scope.EME_Id = "";
            $scope.ESAESLOT_Id = "";
            $scope.Room_Temp_Details = [];
            $scope.ESARMSCH_ExamDate = null;
            $scope.GetRoomList = [];
            $scope.GetaSavedRoomDetails = [];
            $scope.yearlst = [];
            $scope.examlist = [];
            $scope.searchbtn = false;
            $scope.submitted = false;
            $scope.submitted1 = false;
            $scope.slotlist = [];
            $scope.TempClassSubjectList = [];
            $scope.obj.searchValue = '';
            $scope.GetExamDateloaddata();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.isOptionsRequired1 = function () {
            return !$scope.GetRoomList.some(function (options) {
                return options.checked;
            });
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };
        
        $scope.obj.searchValue = '';
        $scope.filterValue = function (obj) {
            return ($filter('date')(obj.esarmscH_ExamDate, 'dd-MM-yyyy').indexOf($scope.obj.searchValue) >= 0) ||
                (angular.lowercase(obj.asmaY_Year)).indexOf(angular.lowercase($scope.obj.searchValue)) >= 0 ||
                (angular.lowercase(obj.emE_ExamName)).indexOf(angular.lowercase($scope.obj.searchValue)) >= 0 ||
                (angular.lowercase(obj.esaesloT_SlotName)).indexOf(angular.lowercase($scope.obj.searchValue)) >= 0 ||
                (angular.lowercase(obj.esarooM_RoomName)).indexOf(angular.lowercase($scope.obj.searchValue)) >= 0;                 
        };
    }
})();