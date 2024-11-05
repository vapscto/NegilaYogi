(function () {
    'use strict';
    angular
        .module('app')
        .controller('Hostel_Vacate', Hostel_Vacate);
    Hostel_Vacate.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter'];
    function Hostel_Vacate($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter) {

        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;
        $scope.page1 = "page1";
        $scope.search = " ";
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;
            $scope.reverse = !$scope.reverse;
        };


        //===================load data
        $scope.submitted = false;
        $scope.search = '';
        $scope.type = "student";
        $scope.loaddata = function () {
            var data = {
                "type": "student"
            };

            apiService.create("StudentVacant/loaddata", data).then(function (promise) {

                $scope.studentdata = promise.studentdata;
                $scope.staffdata = promise.staffdata;
                $scope.guestdata = promise.guestdata;

                $scope.gridlistdata = promise.gridlistdata;

            });
        };

        $scope.get_studentDetail = function () {

            $scope.ASMCL_ClassName = "";
            $scope.ASMC_SectionName = "";
            $scope.HLMH_Name = "";
            $scope.HRMRM_RoomNo = "";
            $scope.HLHSALT_AllotmentDate = "";

            var data = {
                "AMST_Id": $scope.AMST_Id
            };
            apiService.create("StudentVacant/get_studentDetail", data).then(function (promise) {

                $scope.ASMCL_ClassName = promise.studentdata[0].ASMCL_ClassName;
                $scope.ASMC_SectionName = promise.studentdata[0].ASMC_SectionName;
                $scope.HLMH_Name = promise.studentdata[0].HLMH_Name;
                $scope.HRMRM_RoomNo = promise.studentdata[0].HRMRM_RoomNo;
                $scope.HLHSALT_AllotmentDate = new Date(promise.studentdata[0].HLHSALT_AllotmentDate);


            });
        };
        $scope.get_staffDetail = function () {
            var data = {
                "HRME_Id": $scope.HRME_Id
            };
            apiService.create("StudentVacant/get_staffDetail", data).then(function (promise) {

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
            apiService.create("StudentVacant/get_guestDetail", data).then(function (promise) {

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
                $scope.type = "student";
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
            apiService.create("StudentVacant/getalldetailsOnselectiontype", data).
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
                        "AMST_Id": $scope.AMST_Id,
                        "HLHSALT_VacatedDate": studentvacatdate,
                        "HLHSALT_VacateRemarks": $scope.HLHSALT_VacateRemarks,
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
                apiService.create("StudentVacant/edittab1", data).then(function (promise) {
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