(function () {
    'use strict';
    angular.module('app').controller('SA_Exam_Room_DateController', SA_Exam_Room_DateController)

    SA_Exam_Room_DateController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'uiCalendarConfig', 'superCache', '$window', '$http']

    function SA_Exam_Room_DateController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, uiCalendarConfig, superCache, $window, $http) {

        $scope.tempcldrlst = [];
        $scope.searchbtn = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.search = "";

        $scope.ESAABSTU_MaxExamDate = new Date();
        $scope.ESAEDATE_ExamDate = new Date();

        $scope.Getroomdetails = [];
        $scope.GetExamDateloaddata = function () {
            var id = 1;
            apiService.getURI("Exam_Room_Date/GetExamDateloaddata", id).then(function (promise) {
                $scope.yearlst = promise.getyearlist;
                $scope.examlist = promise.getexamlisst;
                $scope.university_examlist = promise.getuniversityexamlist;
                $scope.coursedetails = promise.getcourselist;
                $scope.slotlist = promise.getslotlist;
                $scope.ASMAY_Id = promise.asmaY_Id;
                $scope.GetSavedDetails = promise.getSavedDetails;
            });
        };

        $scope.OnChangeyear = function () {
            $scope.Getroomdetails = [];
            $scope.EME_Id = "";
            $scope.ESAUE_Id = "";
            $scope.ESAESLOT_Id = "";
            $scope.AMCO_Id = "";
            $scope.submitted1 = false;
            $scope.submitted = false;
        };

        $scope.OnChangeexam = function () {
            $scope.Getroomdetails = [];             
            $scope.ESAUE_Id = "";
            $scope.ESAESLOT_Id = "";
            $scope.AMCO_Id = "";
            $scope.submitted1 = false;
            $scope.submitted = false;
        };

        $scope.OnChangeuniversityexam = function () {
            $scope.Getroomdetails = [];            
            $scope.ESAESLOT_Id = "";
            $scope.AMCO_Id = "";
            $scope.submitted1 = false;
            $scope.submitted = false;
        };

        $scope.OnChangeslot = function () {
            $scope.Getroomdetails = [];            
            $scope.AMCO_Id = "";
            $scope.submitted1 = false;
            $scope.submitted = false;
        };

        $scope.Onchangecourse = function () {
            $scope.Getroomdetails = [];           
            $scope.submitted1 = false;
            $scope.submitted = false;
        };

        $scope.GetSearchExamDateData = function () {
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "EME_Id": $scope.EME_Id,
                    //"AMCO_Id": $scope.AMCO_Id,
                    "ESAUE_Id": $scope.ESAUE_Id,
                    "ESAESLOT_Id": $scope.ESAESLOT_Id,
                    "ESAEDATE_ExamDate": new Date($scope.ESAEDATE_ExamDate).toDateString()
                };
                apiService.create("Exam_Room_Date/GetSearchExamDateData", data).then(function (promise) {
                    if (promise !== null) {
                        $scope.Getroomdetails = promise.getroomdetails;
                        $scope.Getsavedroomdetails = promise.getsavedroomdetails;

                        angular.forEach($scope.Getroomdetails, function (d) {
                            d.esaedateD_Id = 0;
                            d.checked = false;
                        });

                        if ($scope.Getsavedroomdetails !== null && $scope.Getsavedroomdetails.length > 0) {
                            angular.forEach($scope.Getroomdetails, function (dd) {
                                angular.forEach($scope.Getsavedroomdetails, function (d) {
                                    if (dd.esarooM_Id === d.esarooM_Id) {
                                        dd.esaedateD_Id = d.esaedateD_Id;
                                        dd.checked = true;
                                        dd.maxcount = d.esaedateD_AllotedCapacity;
                                    }
                                });
                            });
                        }

                        if ($scope.Getroomdetails === null || $scope.Getroomdetails.length === 0) {
                            swal("No Room Details Found Kindly Enter The Room Details");
                        }
                    }
                });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.CheckCount = function (dd) {

            var maxcount = parseInt(dd.esarooM_RoomMaxCapacity);
            var allotedcount = parseInt(dd.maxcount);
            if (maxcount >= 0) {
                if (allotedcount > maxcount) {
                    dd.maxcount = "";
                    swal("Alloted Count Should Be Less Than Equal To Max Count");
                } else {

                    var data = {
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "EME_Id": $scope.EME_Id,
                        //"AMCO_Id": $scope.AMCO_Id,
                        "ESAUE_Id": $scope.ESAUE_Id,
                        "ESAESLOT_Id": $scope.ESAESLOT_Id,
                        "ESAEDATE_ExamDate": new Date($scope.ESAEDATE_ExamDate).toDateString(),
                        "ESAEDATED_AllotedCapacity": dd.maxcount,
                        "ESAROOM_RoomMaxCapacity": dd.esarooM_RoomMaxCapacity,
                        "ESAEDATED_Id": dd.esaedateD_Id,
                        "ESAROOM_Id": dd.esarooM_Id
                    };
                    apiService.create("Exam_Room_Date/CheckCount", data).then(function (promise) {
                        if (promise !== null) {
                            if (promise.message === "Room Capactity") {
                                dd.maxcount = "";
                                swal("Alloted Count Should Be Less Than Equal To Max Count");
                            }
                        }
                    });
                }
            }
        };

        $scope.SaveExamDateData = function (objform) {
            if (objform.$valid) {
                $scope.Room_Temp_Details = [];

                angular.forEach($scope.Getroomdetails, function (dd) {
                    if (dd.checked === true) {
                        $scope.Room_Temp_Details.push({ ESAEDATED_Id: dd.esaedateD_Id, ESAROOM_Id: dd.esarooM_Id, ESAEDATED_AllotedCapacity: dd.maxcount });
                    }
                });

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "EME_Id": $scope.EME_Id,
                    //"AMCO_Id": $scope.AMCO_Id,
                    "ESAUE_Id": $scope.ESAUE_Id,
                    "ESAESLOT_Id": $scope.ESAESLOT_Id,
                    "ESAEDATE_ExamDate": new Date($scope.ESAEDATE_ExamDate).toDateString(),
                    "Room_Temp_Details": $scope.Room_Temp_Details
                };
                apiService.create("Exam_Room_Date/SaveExamDateData", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.returnval === true) {
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
                "ESAEDATE_Id": dd.esaedatE_Id
            };

            apiService.create("Exam_Room_Date/EditExamDateData", data).then(function (promise) {

                if (promise !== null) {
                    $scope.GetEditedDateDetails = promise.getEditedDateDetails;
                    $scope.GetEditedRoomDetails = promise.getEditedRoomDetails;
                    $scope.Getroomdetails = promise.getroomdetails;

                    angular.forEach($scope.Getroomdetails, function (d) {
                        d.esaedateD_Id = 0;
                        d.checked = false;
                    });

                    if ($scope.GetEditedRoomDetails !== null && $scope.GetEditedRoomDetails.length > 0) {
                        angular.forEach($scope.Getroomdetails, function (dd) {
                            angular.forEach($scope.GetEditedRoomDetails, function (d) {
                                if (dd.esarooM_Id === d.esarooM_Id) {
                                    dd.esaedateD_Id = d.esaedateD_Id;
                                    dd.checked = true;
                                    dd.maxcount = d.esaedateD_AllotedCapacity;
                                }
                            });
                        });
                    }

                    $scope.ASMAY_Id = $scope.GetEditedDateDetails[0].asmaY_Id;
                    //$scope.AMCO_Id = $scope.GetEditedDateDetails[0].amcO_Id;
                    $scope.EME_Id = $scope.GetEditedDateDetails[0].emE_Id;
                    $scope.ESAUE_Id = $scope.GetEditedDateDetails[0].esauE_Id;
                    $scope.ESAESLOT_Id = $scope.GetEditedDateDetails[0].esaesloT_Id;
                    $scope.searchbtn = true;
                    if ($scope.Getroomdetails === null || $scope.Getroomdetails.length === 0) {
                        swal("No Room Details Found Kindly Enter The Room Details");
                    }
                }
            });
        };

        $scope.ViewRoomDetails = function (dd) {
            var data = {
                "ESAEDATE_Id": dd.esaedatE_Id
            };

            apiService.create("Exam_Room_Date/ViewRoomDetails", data).then(function (promise) {
                if (promise !== null) {
                    $scope.GetViewRoomDetails = promise.getViewRoomDetails;

                    if ($scope.GetViewRoomDetails !== null && $scope.GetViewRoomDetails.length > 0) {
                        $('#mymodalviewroom').modal('show');
                    }
                }
            });
        };

        $scope.ActiveDeactiveExamDate = function (dd) {
            var mgs = "";
            var confirmmgs = "";

            if (dd.esaedatE_ActiveFlg === true) {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";
            } else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }

            var data = {
                "ESAEDATE_Id": dd.esaedatE_Id
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
                        apiService.create("Exam_Room_Date/ActiveDeactiveExamDate", data).then(function (promise) {
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

            if (dd.esaedateD_ActiveFlg === true) {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";
            } else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }

            var data = {
                "ESAEDATE_Id": dd.esaedatE_Id,
                "ESAEDATED_Id": dd.esaedateD_Id                 
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
                        apiService.create("Exam_Room_Date/ActiveDeactiveRoomDetails", data).then(function (promise) {
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

                                $scope.GetViewRoomDetails = promise.getViewRoomDetails;                               
                            }
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.cleardata = function () {
            $scope.ASMAY_Id = "";
            $scope.AMCO_Id = "";
            $scope.EME_Id = "";
            $scope.ESAESLOT_Id = "";
            $scope.ESAUE_Id = "";
            $scope.Room_Temp_Details = [];
            $scope.ESAABSTU_ExamDate = null;
            $scope.Getroomdetails = [];
            $scope.Getsavedroomdetails = [];
            $scope.yearlst = [];
            $scope.examlist = [];
            $scope.university_examlist = [];
            $scope.coursedetails = [];
            $scope.searchbtn = false;
            $scope.slotlist = [];
            $scope.GetExamDateloaddata();
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.isOptionsRequired1 = function () {
            return !$scope.Getroomdetails.some(function (options) {
                return options.checked;
            });
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };
    }
})();