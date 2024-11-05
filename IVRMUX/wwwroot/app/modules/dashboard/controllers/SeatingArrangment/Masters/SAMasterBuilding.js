(function () {
    'use strict';
    angular.module('app').controller('SAMasterBuildingController', SAMasterBuildingController)
    SAMasterBuildingController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', 'superCache', '$stateParams', '$filter']
    function SAMasterBuildingController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, superCache, $stateParams, $filter) {

        $scope.search1 = "";
        $scope.search2 = "";
        $scope.search3 = "";
        $scope.search4 = "";
        $scope.search5 = "";
        $scope.search6 = "";

        $scope.editf = false;
        $scope.editfr = false;

        var paginationformasters = 0;
        var copty = "";
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            copty = ivrmcofigsettings[0].ivrmgC_Disclaimer;
        } else {
            paginationformasters = 10;
        }

        $scope.itemsPerPage1 = paginationformasters;
        $scope.itemsPerPage2 = paginationformasters;
        $scope.itemsPerPage3 = paginationformasters;
        $scope.itemsPerPage4 = paginationformasters;
        $scope.itemsPerPage5 = paginationformasters;
        $scope.itemsPerPage6 = paginationformasters;

        $scope.currentPage1 = 1;
        $scope.currentPage2 = 1;
        $scope.currentPage3 = 1;
        $scope.currentPage4 = 1;
        $scope.currentPage5 = 1;
        $scope.currentPage6 = 1;

        // Master Building
        $scope.LoadData = function () {
            $scope.ClearFirstTabLoadData();
            var data = 2;
            $scope.EditId = 0;
            apiService.getURI("SAMasterBuilding/LoadData", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getbuildingdetails = promise.getbuildingdetails;
                }
            });
        };

        $scope.SaveMasterBuilding = function () {
            if ($scope.myForm1.$valid) {
                $scope.ESABLD_BuildingDesc = $scope.ESABLD_BuildingDesc === undefined || $scope.ESABLD_BuildingDesc === null || $scope.ESABLD_BuildingDesc === "" ? "" : $scope.ESABLD_BuildingDesc;

                var data = {
                    "ESABLD_Id": $scope.ESABLD_Id,
                    "ESABLD_BuildingName": $scope.ESABLD_BuildingName,
                    "ESABLD_BuildingDesc": $scope.ESABLD_BuildingDesc
                };
                apiService.create("SAMasterBuilding/SaveMasterBuilding", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.message === "Add") {
                            if (promise.returnval === true) {
                                swal("Record Saved Successfully");
                            } else if (promise.returnval === false) {
                                swal("Failed To Save Record");
                            }
                        } else if (promise.message === "Update") {
                            if (promise.returnval === true) {
                                swal("Record Updated Successfully");
                            } else if (promise.returnval === false) {
                                swal("Failed To Update Record");
                            }
                        } else if (promise.message === "Duplicate") {
                            swal("Record Already Exists");
                        } else {
                            swal("Failed To Save/Update Record");
                        }
                        $state.reload();
                    }
                });
            } else {
                $scope.submitted1 = true;
            }
        };

        $scope.EditMasterBuilding = function (obj) {
            $scope.ESABLD_BuildingName = "";
            $scope.ESABLD_BuildingDesc = "";
            $scope.ESABLD_Id = 0;
            var data = {
                "ESABLD_Id": obj.esablD_Id
            };
            apiService.create("SAMasterBuilding/EditMasterBuilding", data).then(function (promise) {
                if (promise.geteditbuildingdetails !== null && promise.geteditbuildingdetails.length > 0) {
                    $scope.ESABLD_BuildingName = promise.geteditbuildingdetails[0].esablD_BuildingName;
                    $scope.ESABLD_BuildingDesc = promise.geteditbuildingdetails[0].esablD_BuildingDesc;
                    $scope.ESABLD_Id = promise.geteditbuildingdetails[0].esablD_Id;
                }
            });
        };

        $scope.ActiveDeactiveMasterBuilding = function (user, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";

            if (user.esablD_ActiveFlg === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";
            }

            var data = {
                "ESABLD_Id": user.esablD_Id
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
                        apiService.create("SAMasterBuilding/ActiveDeactiveMasterBuilding", data).then(function (promise) {
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
                                $scope.LoadData();
                            }
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };

        $scope.filterValue1 = function (obj) {
            return (angular.lowercase(obj.esablD_BuildingName)).indexOf(angular.lowercase($scope.search1)) >= 0 ||
                (angular.lowercase(obj.esablD_BuildingDesc)).indexOf(angular.lowercase($scope.search1)) >= 0;
        };

        $scope.ClearFirstTab = function () {
            $scope.ESABLD_BuildingName = "";
            $scope.ESABLD_BuildingDesc = "";
            $scope.submitted1 = false;
            $scope.ESABLD_Id = 0;
            $scope.LoadData();
        };

        $scope.ClearFirstTabLoadData = function () {
            $scope.ESABLD_BuildingName = "";
            $scope.ESABLD_BuildingDesc = "";
            $scope.ESABLD_Id = 0;
            $scope.submitted1 = false;
        };

        // Master Floor
        $scope.OnFloorDataLoad = function () {
            $scope.ClearSecondTabLoadData();
            var data = 2;
            apiService.getURI("SAMasterBuilding/OnFloorDataLoad", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getfloordetails = promise.getfloordetails;
                    $scope.masterbuilding = promise.getmasterbuilding;
                }
            });
        };

        $scope.SaveMasterFloor = function () {
            if ($scope.myForm2.$valid) {
                $scope.ESAFLR_FloorDesc = $scope.ESAFLR_FloorDesc === undefined || $scope.ESAFLR_FloorDesc === null || $scope.ESAFLR_FloorDesc === "" ? "" : $scope.ESAFLR_FloorDesc;
                var data = {
                    "ESABLD_Id": $scope.ESABLD_Id_New,
                    "ESAFLR_FloorName": $scope.ESAFLR_FloorName,
                    "ESAFLR_FloorDesc": $scope.ESAFLR_FloorDesc,
                    "ESAFLR_Id": $scope.ESAFLR_Id
                };

                apiService.create("SAMasterBuilding/SaveMasterFloor", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.message === "Add") {
                            if (promise.returnval === true) {
                                swal("Record Saved Successfully");
                            } else if (promise.returnval === false) {
                                swal("Failed To Save Record");
                            }
                        } else if (promise.message === "Update") {
                            if (promise.returnval === true) {
                                swal("Record Updated Successfully");
                            } else if (promise.returnval === false) {
                                swal("Failed To Update Record");
                            }
                        } else if (promise.message === "Duplicate") {
                            swal("Record Already Exists");
                        } else {
                            swal("Failed To Save/Update Record");
                        }
                        $scope.OnFloorDataLoad();
                    }
                });
            } else {
                $scope.submitted2 = true;
            }
        };

        $scope.EditMasterFloor = function (objsf) {
            var data = {
                "ESAFLR_Id": objsf.esaflR_Id
            };
            apiService.create("SAMasterBuilding/EditMasterFloor", data).then(function (promise) {
                if (promise !== null) {
                    $scope.editf = true;
                    $scope.geteditfloordetails = promise.geteditfloordetails;
                    $scope.ESABLD_Id_New = $scope.geteditfloordetails[0].esablD_Id;
                    $scope.ESAFLR_FloorName = $scope.geteditfloordetails[0].esaflR_FloorName;
                    $scope.ESAFLR_FloorDesc = $scope.geteditfloordetails[0].esaflR_FloorDesc;
                    $scope.ESAFLR_Id = $scope.geteditfloordetails[0].esaflR_Id;
                }
            });
        };

        $scope.ActiveDeactiveMasterFloor = function (userf, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";

            if (userf.esaflR_ActiveFlg === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";
            }

            var data = {
                "ESAFLR_Id": userf.esaflR_Id
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
                        apiService.create("SAMasterBuilding/ActiveDeactiveMasterFloor", data).then(function (promise) {
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
                                $scope.OnFloorDataLoad();
                            }
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.interacted2 = function (field) {
            return $scope.submitted2;
        };

        $scope.filterValue2 = function (obj) {
            return (angular.lowercase(obj.esablD_BuildingName)).indexOf(angular.lowercase($scope.search2)) >= 0 ||
                (angular.lowercase(obj.esaflR_FloorName)).indexOf(angular.lowercase($scope.search2)) >= 0 ||
                (angular.lowercase(obj.esablD_BuildingDesc)).indexOf(angular.lowercase($scope.search2)) >= 0;
        };

        $scope.ClearSecondTab = function () {
            $scope.ESABLD_Id_New = "";
            $scope.ESAFLR_FloorName = "";
            $scope.ESAFLR_FloorDesc = "";
            $scope.submitted2 = false;
            $scope.editf = false;
            $scope.ESAFLR_Id = 0;
            $scope.OnFloorDataLoad();
        };

        $scope.ClearSecondTabLoadData = function () {
            $scope.ESABLD_Id_New = "";
            $scope.ESAFLR_FloorName = "";
            $scope.ESAFLR_FloorDesc = "";
            $scope.submitted2 = false;
            $scope.editf = false;
            $scope.ESAFLR_Id = 0;
        };

        // Master Room
        $scope.OnRoomDataLoad = function () {
            $scope.ClearThirdTabLoadData();
            var data = 2;
            apiService.getURI("SAMasterBuilding/OnRoomDataLoad", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getroomdetails = promise.getroomdetails;
                    $scope.getmasterbuilding = promise.getmasterbuilding;
                }
            });

        };

        $scope.OnChangeBuilding = function () {
            $scope.ESAFLR_Id_Room = "";
            $scope.getbuildingfloordetails = [];
            var data = {
                "ESABLD_Id": $scope.ESABLD_Id_Room
            };
            apiService.create("SAMasterBuilding/OnChangeBuilding", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getbuildingfloordetails = promise.getbuildingfloordetails;
                    if ($scope.getbuildingfloordetails === null || $scope.getbuildingfloordetails.length === 0) {
                        swal("No Floor Is Mapped For Selected Building");
                    }
                }
            });
        };

        $scope.SaveMasterRoom = function () {
            if ($scope.myForm3.$valid) {
                $scope.ESAROOM_RoomDesc = $scope.ESAROOM_RoomDesc === undefined || $scope.ESAROOM_RoomDesc === null || $scope.ESAROOM_RoomDesc === "" ? "" : $scope.ESAROOM_RoomDesc;
                var data = {
                    "ESAROOM_Id": $scope.ESAROOM_Id,
                    "ESABLD_Id": $scope.ESABLD_Id_Room,
                    "ESAFLR_Id": $scope.ESAFLR_Id_Room,
                    "ESAROOM_RoomName": $scope.ESAROOM_RoomName,
                    "ESAROOM_RoomDesc": $scope.ESAROOM_RoomDesc,
                    "ESAROOM_RoomMaxCapacity": $scope.ESAROOM_RoomMaxCapacity,
                    "ESAROOM_RoomTypeFlg": $scope.ESAROOM_RoomTypeFlg,
                    "ESAROOM_MaxNoOfRows": $scope.ESAROOM_MaxNoOfRows,
                    "ESAROOM_MaxNoOfColumns": $scope.ESAROOM_MaxNoOfColumns,
                    "ESAROOM_BenchCapacity": $scope.ESAROOM_BenchCapacity
                };

                apiService.create("SAMasterBuilding/SaveMasterRoom", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.message === "Add") {
                            if (promise.returnval === true) {
                                swal("Record Saved Successfully");
                            } else if (promise.returnval === false) {
                                swal("Failed To Save Record");
                            }
                        } else if (promise.message === "Update") {
                            if (promise.returnval === true) {
                                swal("Record Updated Successfully");
                            } else if (promise.returnval === false) {
                                swal("Failed To Update Record");
                            }
                        } else if (promise.message === "Duplicate") {
                            swal("Record Already Exists");
                        } else {
                            swal("Failed To Save/Update Record");
                        }
                        $scope.OnRoomDataLoad();
                    }
                });

            } else {
                $scope.submitted3 = true;
            }
        };

        $scope.EditMasterRoom = function (objsr) {
            var data = {
                "ESAROOM_Id": objsr.esarooM_Id
            };
            apiService.create("SAMasterBuilding/EditMasterRoom", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.geteditroomdetails !== null && promise.geteditroomdetails.length > 0) {
                        $scope.editfr = true;
                        $scope.geteditroomdetails = promise.geteditroomdetails;
                        $scope.getbuildingfloordetails = promise.geteditbuildingfloordetails;

                        $scope.ESAROOM_Id = $scope.geteditroomdetails[0].esarooM_Id;
                        $scope.ESABLD_Id_Room = $scope.geteditroomdetails[0].esablD_Id;
                        $scope.ESAFLR_Id_Room = $scope.geteditroomdetails[0].esaflR_Id;
                        $scope.ESAROOM_RoomName = $scope.geteditroomdetails[0].esarooM_RoomName;
                        $scope.ESAROOM_RoomDesc = $scope.geteditroomdetails[0].esarooM_RoomDesc;
                        $scope.ESAROOM_RoomMaxCapacity = $scope.geteditroomdetails[0].esarooM_RoomMaxCapacity;
                        $scope.ESAROOM_RoomTypeFlg = $scope.geteditroomdetails[0].esarooM_RoomTypeFlg;
                        $scope.ESAROOM_MaxNoOfRows = $scope.geteditroomdetails[0].esarooM_MaxNoOfRows;
                        $scope.ESAROOM_MaxNoOfColumns = $scope.geteditroomdetails[0].esarooM_MaxNoOfColumns;
                        $scope.ESAROOM_BenchCapacity = $scope.geteditroomdetails[0].esarooM_BenchCapacity;
                    }
                }
            });
        };

        $scope.ActiveDeactiveMasterRoom = function (userr, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";

            if (userr.esarooM_ActiveFlg === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";
            }

            var data = {
                "ESAROOM_Id": userr.esarooM_Id
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
                        apiService.create("SAMasterBuilding/ActiveDeactiveMasterRoom", data).then(function (promise) {
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
                                $scope.OnRoomDataLoad();
                            }
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.interacted3 = function (field) {
            return $scope.submitted3;
        };

        $scope.filterValue3 = function (obj) {
            return (angular.lowercase(obj.esablD_BuildingName)).indexOf(angular.lowercase($scope.search3)) >= 0 ||
                (angular.lowercase(obj.esaflR_FloorName)).indexOf(angular.lowercase($scope.search3)) >= 0 ||
                (angular.lowercase(obj.esarooM_RoomName)).indexOf(angular.lowercase($scope.search3)) >= 0 ||
                (angular.lowercase(obj.esarooM_RoomDesc)).indexOf(angular.lowercase($scope.search3)) >= 0 ||
                (angular.lowercase(obj.esarooM_RoomTypeFlg)).indexOf(angular.lowercase($scope.search3)) >= 0 ||
                (angular.lowercase(obj.esarooM_RoomTypeFlg)).indexOf(angular.lowercase($scope.search3)) >= 0 ||
                (JSON.stringify(obj.esarooM_RoomMaxCapacity)).indexOf($scope.search3) >= 0 ||
                (JSON.stringify(obj.esarooM_MaxNoOfRows)).indexOf($scope.search3) >= 0 ||
                (JSON.stringify(obj.esarooM_MaxNoOfColumns)).indexOf($scope.search3) >= 0 ||
                (JSON.stringify(obj.esarooM_BenchCapacity)).indexOf($scope.search3) >= 0;
        };

        $scope.ClearThirdTab = function () {
            $scope.ESAROOM_Id = "";
            $scope.ESABLD_Id_Room = "";
            $scope.ESAFLR_Id_Room = "";
            $scope.ESAROOM_RoomName = "";
            $scope.ESAROOM_RoomDesc = "";
            $scope.ESAROOM_RoomMaxCapacity = "";
            $scope.ESAROOM_RoomTypeFlg = "";
            $scope.ESAROOM_MaxNoOfRows = "";
            $scope.ESAROOM_MaxNoOfColumns = "";
            $scope.ESAROOM_BenchCapacity = "";
            $scope.submitted3 = false;
            $scope.editfr = false;
            $scope.ESAROOM_Id = 0;
            $scope.OnRoomDataLoad();
        };

        $scope.ClearThirdTabLoadData = function () {
            $scope.ESAROOM_Id = "";
            $scope.ESABLD_Id_Room = "";
            $scope.ESAFLR_Id_Room = "";
            $scope.ESAROOM_RoomName = "";
            $scope.ESAROOM_RoomDesc = "";
            $scope.ESAROOM_RoomMaxCapacity = "";
            $scope.ESAROOM_RoomTypeFlg = "";
            $scope.ESAROOM_MaxNoOfRows = "";
            $scope.ESAROOM_MaxNoOfColumns = "";
            $scope.ESAROOM_BenchCapacity = "";
            $scope.submitted3 = false;
            $scope.editfr = false;
            $scope.ESAROOM_Id = 0;
        };

        // Master University Exam
        $scope.OnUniversityExamLoadData = function () {
            $scope.ClearFourthTabLoadData();
            var data = 2;
            apiService.getURI("SAMasterBuilding/OnUniversityExamLoadData", data).then(function (promise) {
                if (promise !== null) {
                    $scope.getuniversityexamdetails = promise.getuniversityexamdetails;
                    $scope.grouptypeListOrder = promise.getuniversityexamorderdetails;
                }
            });

        };

        $scope.SaveMasterUniversityExam = function () {
            if ($scope.myForm4.$valid) {
                var data = {
                    "ESAUE_Id": $scope.ESAUE_Id,
                    "ESAUE_ExamName": $scope.ESAUE_ExamName,
                    "ESAUE_ExamCode": $scope.ESAUE_ExamCode
                };

                apiService.create("SAMasterBuilding/SaveMasterUniversityExam", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.message === "Add") {
                            if (promise.returnval === true) {
                                swal("Record Saved Successfully");
                            } else if (promise.returnval === false) {
                                swal("Failed To Save Record");
                            }
                        } else if (promise.message === "Update") {
                            if (promise.returnval === true) {
                                swal("Record Updated Successfully");
                            } else if (promise.returnval === false) {
                                swal("Failed To Update Record");
                            }
                        } else if (promise.message === "Duplicate") {
                            swal("Record Already Exists");
                        } else {
                            swal("Failed To Save/Update Record");
                        }
                        $scope.OnUniversityExamLoadData();
                    }
                });

            } else {
                $scope.submitted4 = true;
            }
        };

        $scope.UpdateMasterUniversityExamOrder = function () {
            var data = {
                "Temp_OrderUpdate": $scope.grouptypeListOrder
            };
            apiService.create("SAMasterBuilding/UpdateMasterUniversityExamOrder", data).then(function (promoise) {
                if (promoise !== null) {
                    if (promoise.returnval === true) {
                        swal("Records Updated Sucessfully");
                    }
                    else {
                        swal("Failed to Update the Record");
                    }
                }
                else {
                    swal("Failed To Update Record");
                }
                $scope.OnUniversityExamLoadData();
            });
        };

        $scope.sortableOptions = {
            stop: function (e, ui) {
                for (var index in $scope.grouptypeListOrder) {
                    $scope.grouptypeListOrder[index].esauE_ExamOrder = Number(index) + 1;

                }
            }
        };

        $scope.EditMasterUniverstityExam = function (objse) {
            var data = {
                "ESAUE_Id": objse.esauE_Id
            };
            apiService.create("SAMasterBuilding/EditMasterUniverstityExam", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.getedituniversityexamdetails !== null && promise.getedituniversityexamdetails.length > 0) {
                        $scope.getedituniversityexamdetails = promise.getedituniversityexamdetails;

                        $scope.ESAUE_Id = $scope.getedituniversityexamdetails[0].esauE_Id;
                        $scope.ESAUE_ExamName = $scope.getedituniversityexamdetails[0].esauE_ExamName;
                        $scope.ESAUE_ExamCode = $scope.getedituniversityexamdetails[0].esauE_ExamCode;
                    }
                }
            });
        };

        $scope.ActiveDeactiveMasterUniverstityExam = function (usere, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";

            if (usere.esauE_ActiveFlag === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";
            }

            var data = {
                "ESAUE_Id": usere.esauE_Id
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
                        apiService.create("SAMasterBuilding/ActiveDeactiveMasterUniverstityExam", data).then(function (promise) {
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
                                $scope.OnUniversityExamLoadData();
                            }
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.interacted4 = function (field) {
            return $scope.submitted4;
        };

        $scope.filterValue4 = function (obj) {
            return (angular.lowercase(obj.esauE_ExamName)).indexOf(angular.lowercase($scope.search4)) >= 0 ||
                (angular.lowercase(obj.esauE_ExamCode)).indexOf(angular.lowercase($scope.search4)) >= 0 ||
                (JSON.stringify(obj.esauE_ExamOrder)).indexOf($scope.search4) >= 0;
        };

        $scope.ClearFourthTab = function () {
            $scope.ESAUE_ExamName = "";
            $scope.ESAUE_ExamCode = "";
            $scope.ESAUE_ExamOrder = "";
            $scope.submitted4 = false;
            $scope.ESAUE_Id = 0;
            $scope.OnRoomDataLoad();
        };

        $scope.ClearFourthTabLoadData = function () {
            $scope.ESAUE_ExamName = "";
            $scope.ESAUE_ExamCode = "";
            $scope.ESAUE_ExamOrder = "";
            $scope.submitted4 = false;
            $scope.ESAUE_Id = 0;
        };


        // Master Duty Type
        $scope.OnDutyTypeLoadData = function () {
            $scope.ClearFifthTabLoadData();
            var data = 2;
            apiService.getURI("SAMasterBuilding/OnDutyTypeLoadData", data).then(function (promise) {
                $scope.getdutytypedetails = promise.getdutytypedetails;
            });
        };

        $scope.SaveMasterDutyType = function () {
            if ($scope.myForm5.$valid) {
                var data = {
                    "ESAALLSTADTP_Id": $scope.ESAALLSTADTP_Id,
                    "ESAALLSTADTP_DTName": $scope.ESAALLSTADTP_DTName
                };

                apiService.create("SAMasterBuilding/SaveMasterDutyType", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.message === "Add") {
                            if (promise.returnval === true) {
                                swal("Record Saved Successfully");
                            } else if (promise.returnval === false) {
                                swal("Failed To Save Record");
                            }
                        } else if (promise.message === "Update") {
                            if (promise.returnval === true) {
                                swal("Record Updated Successfully");
                            } else if (promise.returnval === false) {
                                swal("Failed To Update Record");
                            }
                        } else if (promise.message === "Duplicate") {
                            swal("Record Already Exists");
                        } else {
                            swal("Failed To Save/Update Record");
                        }
                        $scope.OnDutyTypeLoadData();
                    }
                });

            } else {
                $scope.submitted5 = true;
            }
        };

        $scope.EditDutyType = function (objsdy) {
            var data = {
                "ESAALLSTADTP_Id": objsdy.esaallstadtP_Id
            };
            apiService.create("SAMasterBuilding/EditDutyType", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.geteditdutytypedetails !== null && promise.geteditdutytypedetails.length > 0) {
                        $scope.geteditdutytypedetails = promise.geteditdutytypedetails;
                        $scope.ESAALLSTADTP_Id = $scope.geteditdutytypedetails[0].esaallstadtP_Id;
                        $scope.ESAALLSTADTP_DTName = $scope.geteditdutytypedetails[0].esaallstadtP_DTName;
                    }
                }
            });
        };

        $scope.ActiveDeactiveDutyType = function (userdy, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";

            if (userdy.esaallstadtP_ActiveFlag === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";
            }

            var data = {
                "ESAALLSTADTP_Id": userdy.esaallstadtP_Id
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
                        apiService.create("SAMasterBuilding/ActiveDeactiveDutyType", data).then(function (promise) {
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
                                $scope.OnDutyTypeLoadData();
                            }
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.interacted5 = function (field) {
            return $scope.submitted5;
        };

        $scope.filterValue5 = function (obj) {
            return (angular.lowercase(obj.esaallstadtP_DTName)).indexOf(angular.lowercase($scope.search5)) >= 0;
        };

        $scope.ClearFifthTab = function () {
            $scope.ESAALLSTADTP_DTName = "";
            $scope.submitted5 = false;
            $scope.ESAALLSTADTP_Id = 0;
            $scope.OnDutyTypeLoadData();
        };

        $scope.ClearFifthTabLoadData = function () {
            $scope.ESAALLSTADTP_DTName = "";
            $scope.submitted5 = false;
            $scope.ESAALLSTADTP_Id = 0;
        };


        // Master Slot Time

        $scope.hstep = 1;
        $scope.mstep = 1;

        $scope.options = {
            hstep: [1, 2, 3],
            mstep: [1, 5, 10, 15, 25, 30]
        };

        $scope.ismeridian = true;
        $scope.toggleMode = function () {
            $scope.ismeridian = !$scope.ismeridian;
        };

        $scope.OnTimeSlotLoadData = function () {
            $scope.ClearSixthTabLoadData();
            var data = 2;
            apiService.getURI("SAMasterBuilding/OnTimeSlotLoadData", data).then(function (promise) {
                $scope.getslottimedetails = promise.getslottimedetails;
            });
        };

        $scope.SaveMasterTimeSlot = function () {
            if ($scope.myForm6.$valid) {

                var from_month = new Date().getMonth() + 1;
                var from_day = new Date().getDate();
                var from_year = new Date().getFullYear();

                var to_month = new Date().getMonth() + 1;
                var to_day = new Date().getDate();
                var to_year = new Date().getFullYear();

                $scope.ScheduleTime = $scope.ScheduleTime_24;
                var ScheduleTime = $filter('date')($scope.ESAESLOT_StartTime, "HH");
                var ScheduleTimem = $filter('date')($scope.ESAESLOT_StartTime, "mm");
                var ScheduleTimesec = "00";

                $scope.ScheduleTimeTo = $scope.ScheduleTimeTo_24;
                var ScheduleTimeTo = $filter('date')($scope.ESAESLOT_EndTime, "HH");
                var ScheduleTimeTom = $filter('date')($scope.ESAESLOT_EndTime, "mm");
                var ScheduleTimeTosec = "00";

                var timeStart = new Date(from_year + ',' + from_month + ',' + from_day + ' ' + ScheduleTime + ':' + ScheduleTimem + ':' + ScheduleTimesec).getTime();
                var timeEnd = new Date(to_year + ',' + to_month + ',' + to_day + ' ' + ScheduleTimeTo + ':' + ScheduleTimeTom + ':' + ScheduleTimeTosec).getTime();

                var hourDiff = timeEnd - timeStart; //in ms

                var secDiff = hourDiff / 1000; //in s
                var minDiff = hourDiff / 60 / 1000; //in minutes
                var hDiff = hourDiff / 3600 / 1000; //in hours

                var humanReadable = {};
                humanReadable.hours = Math.floor(hDiff);
                humanReadable.minutes = minDiff - 60 * humanReadable.hours;
                humanReadable.total = (humanReadable.hours * 60) + (humanReadable.minutes);
                console.log(humanReadable);

                if (humanReadable.total > 0) {
                    var data = {
                        "ESAESLOT_Id": $scope.ESAESLOT_Id,
                        "ESAESLOT_SlotName": $scope.ESAESLOT_SlotName,
                        "ESAESLOT_StartTime": $filter('date')($scope.ESAESLOT_StartTime, "h:mm a"),
                        "ESAESLOT_EndTime": $filter('date')($scope.ESAESLOT_EndTime, "h:mm a")
                    };

                    apiService.create("SAMasterBuilding/SaveMasterTimeSlot", data).then(function (promise) {
                        if (promise !== null) {
                            if (promise.message === "Add") {
                                if (promise.returnval === true) {
                                    swal("Record Saved Successfully");
                                } else if (promise.returnval === false) {
                                    swal("Failed To Save Record");
                                }
                            } else if (promise.message === "Update") {
                                if (promise.returnval === true) {
                                    swal("Record Updated Successfully");
                                } else if (promise.returnval === false) {
                                    swal("Failed To Update Record");
                                }
                            } else if (promise.message === "Duplicate") {
                                swal("Record Already Exists");
                            } else {
                                swal("Failed To Save/Update Record");
                            }
                            $scope.OnTimeSlotLoadData();
                        }
                    });
                } else {
                    swal("End Time Should Be Greather Than Start Time");
                }

            } else {
                $scope.submitted6 = true;
            }
        };

        $scope.EditTimeSlot = function (objsts) {
            var data = {
                "ESAESLOT_Id": objsts.esaesloT_Id
            };
            apiService.create("SAMasterBuilding/EditTimeSlot", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.geteditslottimedetails !== null && promise.geteditslottimedetails.length > 0) {
                        $scope.geteditslottimedetails = promise.geteditslottimedetails;
                        $scope.ESAESLOT_Id = $scope.geteditslottimedetails[0].esaesloT_Id;
                        $scope.ESAESLOT_SlotName = $scope.geteditslottimedetails[0].esaesloT_SlotName;
                        $scope.ESAESLOT_StartTime = moment($scope.geteditslottimedetails[0].esaesloT_StartTime, 'h:mm a').format();
                        $scope.ESAESLOT_EndTime = moment($scope.geteditslottimedetails[0].esaesloT_EndTime, 'h:mm a').format();
                    }
                }
            });
        };

        $scope.ActiveDeactiveTimeSlot = function (userts, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";

            if (userts.esaesloT_ActiveFlg === true) {
                mgs = "Deactive";
                confirmmgs = "Deactivated";
            }
            else {
                mgs = "Active";
                confirmmgs = "Activated";
            }

            var data = {
                "ESAESLOT_Id": userts.esaesloT_Id
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
                        apiService.create("SAMasterBuilding/ActiveDeactiveTimeSlot", data).then(function (promise) {
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
                                $scope.OnTimeSlotLoadData();
                            }
                        });
                    }
                    else {
                        swal("Record " + mgs + " Cancelled");
                    }
                });
        };

        $scope.interacted6 = function (field) {
            return $scope.submitted6;
        };

        $scope.filterValue6 = function (obj) {
            return (angular.lowercase(obj.esaesloT_SlotName)).indexOf(angular.lowercase($scope.search6)) >= 0 ||
                (angular.lowercase(obj.esaesloT_StartTime)).indexOf(angular.lowercase($scope.search6)) >= 0 ||
                (angular.lowercase(obj.esaesloT_EndTime)).indexOf(angular.lowercase($scope.search6)) >= 0;
        };

        $scope.ClearSixthTab = function () {
            $scope.ESAESLOT_SlotName = "";
            $scope.ESAESLOT_StartTime = "";
            $scope.ESAESLOT_EndTime = "";
            $scope.submitted6 = false;
            $scope.ESAESLOT_Id = 0;
            $scope.OnTimeSlotLoadData();
        };

        $scope.ClearSixthTabLoadData = function () {
            $scope.ESAESLOT_SlotName = "";
            $scope.ESAESLOT_StartTime = "";
            $scope.ESAESLOT_EndTime = "";
            $scope.submitted6 = false;
            $scope.ESAESLOT_Id = 0;
        };
    }
})();