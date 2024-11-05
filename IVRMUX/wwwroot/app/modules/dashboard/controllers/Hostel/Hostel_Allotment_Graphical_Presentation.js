(function () {
    'use strict';
    angular.module('app').controller('Hostel_Allotment_Graphical_PresentationController', Hostel_Allotment_Graphical_PresentationController)

    Hostel_Allotment_Graphical_PresentationController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$filter', 'Excel', '$timeout']
    function Hostel_Allotment_Graphical_PresentationController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $filter, Excel, $timeout) {
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;
        $scope.searchValue = "";
        $scope.searchchkbx_RoomCategory = "";
        $scope.searchchkbx_FloorName = "";
        $scope.showflag = false;
        $scope.sortReverse = true;
        $scope.submitted = false;

        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("Hostel_Allotment_Report/Get_GP_OnLoad_Report", pageid).then(function (promise) {
                $scope.yearlist = promise.yearlist;
                $scope.hostellist = promise.hostellist;
                $scope.roomcategorylist = promise.roomcategorylist;
                $scope.ASMAY_Id = promise.asmaY_Id;

                $scope.all_RoomCategory = true;
                $timeout(function () { $scope.OnClickAll_RoomCategory(); }, 500);

            });
        };

        $scope.OnChangeYear = function () {
            $scope.griddata = [];
        };

        $scope.OnChangeHostel = function () {
            $scope.floorlist = [];
            $scope.griddata = [];
            $scope.searchchkbx_FloorName = "";
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "HLMH_Id": $scope.HLMH_Id,
            };

            apiService.create("Hostel_Allotment_Report/OnChangeHostel", data).then(function (promise) {
                $scope.floorlist = promise.floorlist;

                $scope.all_FloorName = true;
                $timeout(function () { $scope.OnClickAll_FloorName(); }, 500);
            });
        };

        $scope.getreport = function () {
            $scope.submitted = false;
            if ($scope.myForm.$valid) {
                $scope.Temp_Floor_List = [];
                $scope.Temp_RoomCategory_List = [];
                $scope.griddata = [];
                angular.forEach($scope.floorlist, function (dd) {
                    if (dd.checked_FloorName) {
                        $scope.Temp_Floor_List.push({ HLMF_Id: dd.hlmF_Id, HRMF_FloorName: dd.hrmF_FloorName });
                    }
                });

                angular.forEach($scope.roomcategorylist, function (dd) {
                    if (dd.checked_RoomCategory) {
                        $scope.Temp_RoomCategory_List.push({ HLMRCA_Id: dd.hlmrcA_Id, HLMRCA_RoomCategory: dd.hlmrcA_RoomCategory });
                    }
                });

                var data = {
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "HLMH_Id": $scope.HLMH_Id,
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
                                            status_color = "Green";
                                        }
                                        else {
                                            if (dd.AllotedBedsCount === dd.HRMRM_BedCapacity) {
                                                status = "Fully Occupied";
                                                status_color = "#cbcbcb";
                                            }
                                            else if (dd.AllotedBedsCount < dd.HRMRM_BedCapacity) {
                                                status = "Partially Occupied";
                                                status_color = "#b97e7e";
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
            } else {
                $scope.submitted = true;
            }
        };

        $scope.ViewAllotedStudentDetails = function (room_obj) {

            $scope.hostelname = room_obj.HLMH_Name;
            $scope.floorname = room_obj.HRMF_FloorName;
            $scope.roomcategoryname = room_obj.HLMRCA_RoomCategory;
            $scope.roonno = room_obj.HRMRM_RoomNo;
            $scope.AllotedBedsCount_Temp = room_obj.AllotedBedsCount;

            if ($scope.AllotedBedsCount_Temp > 0) {
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
                            $('#mymodalviewstudentdetails').modal('show');
                        } else {
                            swal("Still Allotment Not Done For This Room");
                        }
                    }
                });
            } else {
                swal("Still Allotment Not Done For This Room");
            }           
        };


        //Room Category
        $scope.OnClickAll_RoomCategory = function () {
            $scope.griddata = [];
            angular.forEach($scope.roomcategorylist, function (dd) {
                dd.checked_RoomCategory = $scope.all_RoomCategory;
            });
        };

        $scope.individual_RoomCategory = function () {
            $scope.griddata = [];
            $scope.all_RoomCategory = $scope.roomcategorylist.every(function (itm) { return itm.checked_RoomCategory; });
        };

        $scope.isOptionsRequired_RoomCategory = function () {
            return !$scope.roomcategorylist.some(function (options) {
                return options.checked_RoomCategory;
            });
        };

        $scope.filterchkbx_RoomCategory = function (obj) {
            return angular.lowercase(obj.hlmrcA_RoomCategory).indexOf(angular.lowercase($scope.searchchkbx_RoomCategory)) >= 0;
        };

        //Floor
        $scope.OnClickAll_FloorName = function () {
            $scope.griddata = [];
            angular.forEach($scope.floorlist, function (dd) {
                dd.checked_FloorName = $scope.all_FloorName;
            });
        };

        $scope.individual_FloorName = function () {
            $scope.griddata = [];
            $scope.all_FloorName = $scope.floorlist.every(function (itm) { return itm.checked_FloorName; });
        };

        $scope.isOptionsRequired_FloorName = function () {
            return !$scope.floorlist.some(function (options) {
                return options.checked_FloorName;
            });
        };

        $scope.filterchkbx_FloorName = function (obj) {
            return angular.lowercase(obj.hrmF_FloorName).indexOf(angular.lowercase($scope.searchchkbx_FloorName)) >= 0;
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };

        $scope.cancel = function () {
            $state.reload();
        };
    }
})();

