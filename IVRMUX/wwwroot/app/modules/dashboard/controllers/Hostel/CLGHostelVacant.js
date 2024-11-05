(function () {
    'use strict';
    angular
        .module('app')
        .controller('CLGHostelVacantController', CLGHostelVacantController);
    CLGHostelVacantController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter'];
    function CLGHostelVacantController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter) {

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;
        $scope.page1 = "page1";
        $scope.search = " ";
        $scope.obj = {};
      
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;
            $scope.reverse = !$scope.reverse;
        };
      

        $scope.HLHSALTC_TransferDate = new Date();
        //===================load data
        $scope.submitted = false;
        $scope.search = '';
        $scope.type = "ClgStudent";
        $scope.loaddata = function () {
            $scope.studentdata = [];
            $scope.roomcatgry_list = [];
            //$scope.obj.HLMRCA_Id = "";
            var data = {
                "type": "ClgStudent"
            };

            apiService.create("CLGHostelVacant/loaddata", data).then(function (promise) {

                $scope.studentdata = promise.studentdata;
                $scope.staffdata = promise.staffdata;
                $scope.guestdata = promise.guestdata;
                $scope.gridlistdata = promise.gridlistdata;
            });
        };

        $scope.get_studentDetail = function () {
            $scope.roomcatgry_list = [];
            $scope.obj.HLMRCA_Id = "";

            var data = {
                "AMCST_Id": $scope.AMCST_Id
            };
            apiService.create("CLGHostelVacant/get_studentDetail", data).then(function (promise) {
                $scope.hostel_list = promise.hostel_list;
                $scope.AllotmentDate = new Date(promise.studentdata[0].HLHSALTC_AllotmentDate);
                $scope.AMCO_CourseName = promise.studentdata[0].AMCO_CourseName;
                $scope.AMB_BranchName = promise.studentdata[0].AMB_BranchName;
                $scope.HLMH_Name = promise.studentdata[0].HLMH_Name;
                $scope.HRMRM_RoomNo = promise.studentdata[0].HRMRM_RoomNo;
                $scope.HLMRCA_RoomCategory = promise.studentdata[0].HLMRCA_RoomCategory;
                $scope.roomcategory = promise.studentdata[0].HLMRCA_Id;
                $scope.HRMRM_Id_Old = promise.studentdata[0].HRMRM_Id;
                $scope.obj.ToHLMH_Id = promise.studentdata[0].HLMH_Id;
                $scope.obj.HLHSALTC_AllotRemarks = promise.studentdata[0].HLHSALTC_AllotRemarks;
                $scope.floor();

                


            });
        };

        
        //============= floor wise room
        $scope.floor = function () {
            $scope.floor_list = [];
            $scope.roomcatgry_list = [];
            $scope.obj.HLMRCA_Id = "";


            var data = {
                "HLMH_Id": $scope.obj.ToHLMH_Id,
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
                "HRMRM_Id_Old": $scope.HRMRM_Id_Old,
            }
            apiService.create("HostelAllotForCLGStudent/roomForVacateReport", data).then(function (promise) {

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
                "HLMH_Id": $scope.obj.ToHLMH_Id,
                "HLMF_Id": $scope.obj.hlmF_Id,
                "HLMRCA_Id": $scope.obj.HLMRCA_Id,
                "HRMRM_Id": $scope.obj.HRMRM_Id,
                "HRMRM_Id_Old": $scope.HRMRM_Id_Old,
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
        //==========================Transfer
       
        
        $scope.transfer = function () {

            if ($scope.myForm1.$valid) {
                var HLHSTRSC_Id = 0;
                if ($scope.HLHSTRSC_Id > 0) {
                    HLHSTRSC_Id = $scope.hlhstrsC_Id;
                }
                var data = {
                    "HLHSTRSC_Id": HLHSTRSC_Id,
                   "HLHSALTC_TransferDate": new Date($scope.HLHSALTC_TransferDate).toDateString(),
                    "HLMH_Id": $scope.obj.ToHLMH_Id,
                    "HLMRCA_Id": $scope.roomcategory,
                    "ACMST_Id": $scope.AMCST_Id,
                    "HRMRM_Id": $scope.HRMRM_Id_Old,
                    // "HLHSTRSC_RoomFee": $scope.obj.HLHSTRSC_RoomFee,
                    "HLHSTRSC_To_HLMRCA_Id": $scope.obj.HLMRCA_Id,
                    "HLMRCAC_To_HRMRM_Id": $scope.obj.HRMRM_Id,
                    "HLHSTRSC_EntireRoomReqdFlg": $scope.HLHSREQC_EntireRoomReqdFlg,
                    "HLHSTRSC_NewRoomFee": $scope.obj.HLHSTRSC_NewRoomFee,
                    "HLHSTRSC_AllotRemarks": $scope.obj.HLHSALTC_AllotRemarks,
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
        };
        //==========================Transfer End



        $scope.get_staffDetail = function () {
            var data = {
                "HRME_Id": $scope.HRME_Id
            };
            apiService.create("CLGHostelVacant/get_staffDetail", data).then(function (promise) {

                $scope.HRMD_DepartmentName = promise.staffdata[0].HRMD_DepartmentName;
                $scope.HRMDES_DesignationName = promise.staffdata[0].HRMDES_DesignationName;
                $scope.HLMH_Name = promise.staffdata[0].HLMH_Name;

                $scope.HLHSTALT_AllotmentDate = promise.staffdata[0].HLHSTALT_AllotmentDate;

                $scope.HRMRM_RoomNo = promise.staffdata[0].HRMRM_RoomNo;


            });
        };
        $scope.get_guestDetail = function () {

            var data = {
                "HLHGSTALT_Id": $scope.HLHGSTALT_Id
            };
            apiService.create("CLGHostelVacant/get_guestDetail", data).then(function (promise) {

                $scope.HLHGSTALT_Id = promise.guest_details[0].hlhgstalT_Id;
                $scope.HLHGSTALT_GuestName = promise.guest_details[0].HLHGSTALT_GuestName;
                $scope.HLMH_Name = promise.guest_details[0].hlmH_Name;
                $scope.HLHGSTALT_AllotmentDate = new Date(promise.guest_details[0].hlhgstalT_AllotmentDate);
                $scope.HRMRM_RoomNo = promise.guest_details[0].hrmrM_RoomNo;

            });
        };


        $scope.changeradio = function (abcc) {

            $scope.HRMD_DepartmentName = "";
            $scope.HRMDES_DesignationName = "";
            $scope.HLMH_Name = "";
            $scope.HLHSALT_AllotmentDate = "";
            $scope.HRMRM_RoomNo = "";
            $scope.ASMCL_ClassName = "";
            $scope.ASMC_SectionName = "";
            $scope.HLHSTALT_AllotmentDate = "";
            $scope.gridlistdata = [];
            $scope.HLHGSTALT_AllotmentDate = "";

            if (abcc === 'stud') {
                $scope.type = "ClgStudent";
            }
            else if (abcc === 'staff') {
                $scope.type = "staff";
            }
            else if (abcc === 'guest') {
                $scope.type = "guest";
            }
            var data = {
                "type": $scope.type
            };
            //var data = {
            //    "changeradio": $scope.changeradio
            //}
            apiService.create("CLGHostelVacant/getalldetailsOnselectiontype", data).
                then(function (promise) {

                    $scope.alldata = promise.list;

                    $scope.gridlistdata = promise.gridlistdata;

                });
        };


        $scope.edittab1 = function () {
            var data = {};

            var studentvacatdate = $scope.HLHSALT_VacatedDate === null ? "" : $filter('date')($scope.HLHSALT_VacatedDate, "yyyy-MM-dd");
            var staffvacatdate = $scope.HLHSTALT_VacatedDate === null ? "" : $filter('date')($scope.HLHSTALT_VacatedDate, "yyyy-MM-dd");
            var guestvacatdate = $scope.HLHGSTALT_VacatedDate === null ? "" : $filter('date')($scope.HLHGSTALT_VacatedDate, "yyyy-MM-dd");

            if ($scope.myForm.$valid) {
                if ($scope.stuchk === 'stud') {
                    $scope.type = "student";
                    data = {
                        "AMCST_Id": $scope.AMCST_Id,
                        "HLHSALTC_VacatedDate": studentvacatdate,
                        "HLHSALTC_VacateRemarks": $scope.HLHSALTC_VacateRemarks,
                        "type": $scope.type

                    };
                }
                else if ($scope.stuchk === 'staff') {
                    $scope.type = "staff";
                    data = {
                        "HRME_Id": $scope.HRME_Id,
                        "HLHSTALT_VacatedDate": staffvacatdate,
                        "HLHSTALT_VacateRemarks": $scope.HLHSTALT_VacateRemarks,
                        "type": $scope.type

                    };
                }
                else if ($scope.stuchk === 'guest') {
                    $scope.type = 'guest';
                    data = {
                        "HLHGSTALT_Id": $scope.HLHGSTALT_Id,
                        "HLHGSTALT_VacatedDate": guestvacatdate,
                        "HLHGSTALT_VacateRemarks": $scope.HLHGSTALT_VacateRemarks,
                        "type": $scope.type
                    };
                }
                apiService.create("CLGHostelVacant/edittab1", data).then(function (promise) {
                    if (promise.msg === "updated") {
                        swal("Record Updated!");
                        $state.reload();
                    }
                    else {
                        swal("Record Not Updated!");
                        $state.reload();
                    }
                });
            }
            else {
                $scope.submitted = true;
            }

        };


        $scope.Clearid = function () {
            $state.reload();
        };



    }
})();