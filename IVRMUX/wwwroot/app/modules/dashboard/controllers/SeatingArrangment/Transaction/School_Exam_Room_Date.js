(function () {
    'use strict';
    angular.module('app').controller('School_Exam_RoomDateController', School_Exam_RoomDateController)

    School_Exam_RoomDateController.$inject = ['$rootScope', '$scope', '$state', '$location', 'dashboardService', 'Flash', 'apiService', '$stateParams', '$filter', 'uiCalendarConfig', 'superCache', '$window', '$http']

    function School_Exam_RoomDateController($rootScope, $scope, $state, $location, dashboardService, Flash, apiService, $stateParams, $filter, uiCalendarConfig, superCache, $window, $http) {

        $scope.tempcldrlst = [];
        $scope.searchbtn = false;
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.search = "";
        $scope.obj = {};
        $scope.obj.searchValue = '';
        //$scope.ESAEDATESCH_ExamDate = new Date();

        $scope.GetRoomList = [];
        $scope.GetExamDateloaddata = function () {
            var id = 1;
            apiService.getURI("School_Exam_Date_Room/GetExamDateRoomloaddata", id).then(function (promise) {
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
            $scope.ESAESLOT_Id = "";           
            $scope.submitted1 = false;
            $scope.submitted = false;

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
            $scope.GetaSavedRoomDetails = [];
            $scope.submitted1 = false;
            $scope.submitted = false;
        };      

        $scope.OnChangeslot = function () {
            $scope.GetRoomList = [];
            $scope.GetaSavedRoomDetails = [];
            $scope.submitted1 = false;
            $scope.submitted = false;
        };      

        $scope.GetSearchExamDateData = function () {
            $scope.GetRoomList = [];
            $scope.GetaSavedRoomDetails = [];
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "EME_Id": $scope.EME_Id,                    
                    "ESAESLOT_Id": $scope.ESAESLOT_Id,
                    "ESAEDATESCH_ExamDate": new Date($scope.ESAEDATESCH_ExamDate).toDateString()
                };
                apiService.create("School_Exam_Date_Room/GetSearchExamDateRoomData", data).then(function (promise) {
                    if (promise !== null) {

                        $scope.GetRoomList = promise.getRoomList;
                        $scope.GetaSavedRoomDetails = promise.getaSavedRoomDetails;

                        angular.forEach($scope.GetRoomList, function (d) {
                            d.ESAEDATERSCH_Id = 0;
                            d.checked = false;
                        });

                        if ($scope.GetaSavedRoomDetails !== null && $scope.GetaSavedRoomDetails.length > 0) {
                            angular.forEach($scope.GetRoomList, function (dd) {
                                angular.forEach($scope.GetaSavedRoomDetails, function (d) {
                                    if (dd.esarooM_Id === d.esarooM_Id) {
                                        dd.ESAEDATERSCH_Id = d.esaedaterscH_Id;
                                        dd.checked = true;                                         
                                    }
                                });
                            });
                        }

                        if ($scope.GetRoomList === null || $scope.GetRoomList.length === 0) {
                            swal("No Room Details Found Kindly Enter The Room Details");
                        }
                    }
                });
            } else {
                $scope.submitted = true;
            }
        };    

        $scope.SaveExamDateData = function (objform) {
            if (objform.$valid) {
                $scope.Room_Temp_Details = [];
                angular.forEach($scope.GetRoomList, function (dd) {
                    if (dd.checked === true) {
                        $scope.Room_Temp_Details.push({ ESAEDATERSCH_Id: dd.ESAEDATERSCH_Id, ESAROOM_Id: dd.esarooM_Id });
                    }
                });

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "ESAEDATESCH_Id": $scope.ESAEDATESCH_Id,
                    "EME_Id": $scope.EME_Id,
                    "ESAESLOT_Id": $scope.ESAESLOT_Id,
                    "ESAEDATESCH_ExamDate": new Date($scope.ESAEDATESCH_ExamDate).toDateString(),
                    "School_Room_Temp_Details": $scope.Room_Temp_Details
                };
                apiService.create("School_Exam_Date_Room/SaveExamDateRoomData", data).then(function (promise) {
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
                "ESAEDATESCH_Id": dd.esaedatescH_Id
            };

            apiService.create("School_Exam_Date_Room/EditExamDateRoomData", data).then(function (promise) {

                if (promise !== null) {
                    $scope.GetEditedDateDetails = promise.getEditedDateDetails;
                    $scope.GetEditedRoomDateDetails = promise.getEditedRoomDateDetails;
                    $scope.GetRoomList = promise.getRoomList;

                    angular.forEach($scope.GetRoomList, function (d) {
                        d.ESAEDATERSCH_Id = 0;
                        d.checked = false;
                    });

                    if ($scope.GetEditedRoomDateDetails !== null && $scope.GetEditedRoomDateDetails.length > 0) {
                        angular.forEach($scope.GetRoomList, function (dd) {
                            angular.forEach($scope.GetEditedRoomDateDetails, function (d) {
                                if (dd.esarooM_Id === d.esarooM_Id) {
                                    dd.ESAEDATERSCH_Id = d.ESAEDATERSCH_Id;
                                    dd.checked = true;                                    
                                }
                            });
                        });
                    }

                    $scope.ESAEDATESCH_Id = $scope.GetEditedDateDetails[0].esaedatescH_Id;
                    $scope.ESAEDATESCH_ExamDate = new Date($scope.GetEditedDateDetails[0].esaedatescH_ExamDate);
                    $scope.ASMAY_Id = $scope.GetEditedDateDetails[0].asmaY_Id;
                    $scope.EME_Id = $scope.GetEditedDateDetails[0].emE_Id;
                    $scope.ESAUE_Id = $scope.GetEditedDateDetails[0].esauE_Id;
                    $scope.ESAESLOT_Id = $scope.GetEditedDateDetails[0].esaesloT_Id;
                    $scope.searchbtn = true;

                    angular.forEach($scope.yearlst, function (dd) {
                        if (dd.asmaY_Id === parseInt($scope.ASMAY_Id)) {
                            $scope.mindate = new Date(dd.asmaY_From_Date);
                            $scope.maxdate = new Date(dd.asmaY_To_Date);
                        }
                    });


                    if ($scope.GetRoomList === null || $scope.GetRoomList.length === 0) {
                        swal("No Room Details Found Kindly Enter The Room Details");
                    }
                }
            });
        };

        $scope.ViewRoomDetails = function (dd) {
            var data = {
                "ESAEDATESCH_Id": dd.esaedatescH_Id
            };

            apiService.create("School_Exam_Date_Room/ViewExamDateRoomDetails", data).then(function (promise) {
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

            if (dd.esaedatescH_ActiveFlg === true) {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";
            } else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }

            var data = {
                "ESAEDATESCH_Id": dd.esaedatescH_Id
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
                        apiService.create("School_Exam_Date_Room/ActiveDeactiveExamRoomDate", data).then(function (promise) {
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

            if (dd.esaedaterscH_ActiveFlg === true) {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";
            } else {
                mgs = "Activate";
                confirmmgs = "Activated";
            }

            var data = {
                "ESAEDATERSCH_Id": dd.esaedaterscH_Id,
                "ESAEDATESCH_Id": dd.esaedatescH_Id                 
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
                        apiService.create("School_Exam_Date_Room/ActiveDeactiveExamDateRoomDetails", data).then(function (promise) {
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
            $scope.EME_Id = "";
            $scope.ESAESLOT_Id = "";            
            $scope.Room_Temp_Details = [];
            $scope.ESAEDATESCH_ExamDate = null;
            $scope.GetRoomList = [];
            $scope.GetaSavedRoomDetails = [];
            $scope.yearlst = [];
            $scope.examlist = [];
            $scope.searchbtn = false;
            $scope.submitted = false;
            $scope.submitted1 = false;
            $scope.slotlist = [];
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
            return ($filter('date')(obj.esaedatescH_ExamDate, 'dd-MM-yyyy').indexOf($scope.obj.searchValue) >= 0) ||
                (angular.lowercase(obj.asmaY_Year)).indexOf(angular.lowercase($scope.obj.searchValue)) >= 0 ||
                (angular.lowercase(obj.emE_ExamName)).indexOf(angular.lowercase($scope.obj.searchValue)) >= 0 ||                
                (angular.lowercase(obj.esaesloT_SlotName)).indexOf(angular.lowercase($scope.obj.searchValue)) >= 0;
        };
    }
})();