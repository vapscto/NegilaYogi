(function () {
    'use strict';
    angular
        .module('app')
        .controller('StaffRequest', StaffRequest);
    StaffRequest.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter'];
    function StaffRequest($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter) {

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
        $scope.submitted = false;
        $scope.HLHSREQ_RequestDate = new Date();

        //====================Save 
        $scope.HLHSTREQ_RequestDate = new Date();
        $scope.save = function () {


            if ($scope.myForm.$valid) {
                var data = {
                    "HLHSTREQ_Id": $scope.HLHSTREQ_Id,
                    "HLHSTREQ_RequestDate": new Date($scope.HLHSTREQ_RequestDate).toDateString(),
                    "HLHSREQ_RequestDate": new Date($scope.HLHSREQ_RequestDate).toDateString(),
                    "HLMH_Id": $scope.HLMH_Id,
                    "HLMRCA_Id": $scope.HLMRCA_Id,
                    "HLHSTREQ_ACRoomFlg": $scope.HLHSTREQ_ACRoomFlg,
                    "HLHSTREQ_SingleRoomFlg": $scope.HLHSTREQ_SingleRoomFlg,
                    "HLHSTREQ_VegMessFlg": $scope.HLHSTREQ_VegMessFlg,
                    "HLHSTREQ_NonVegMessFlg": $scope.HLHSTREQ_NonVegMessFlg,
                    "HLHSTREQ_Remarks": $scope.HLHSTREQ_Remarks,
                    studentlistdata: $scope.studentlistdata
                };
                apiService.create("StaffRequest/save", data).then(function (promise) {
                    if (promise.msg === 'Saved') {
                        swal("Data Saved Successfully....!!!!");
                        $state.reload();
                    }
                    else if (promise.msg === 'Not Saved') {
                        swal("Data  Not Saved Successfully....!!!!");
                        $state.reload();
                    }
                    else if (promise.msg === 'Updated') {
                        swal("Data Updated Successfully.!");
                        $state.reload();
                    }
                    else if (promise.msg === 'Not Updated') {
                        swal("Data Not Updated Successfully...!!!!");
                    } else if (promise.msg === true) {
                        swal(" Data Already Exists....!!!!");
                    }
                    else if (promise.duplicate === true) {
                        swal("Record Already Exist!");
                    }
                    else {
                        swal("Something is wrong.....");
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
            apiService.create("StaffRequest/loaddata", data).then(function (promise) {

                $scope.hostel_list = promise.hostel_list;
                $scope.room_list = promise.room_list;
                $scope.HRME_EmployeeFirstName = promise.staff_wisedata[0].empName;
                $scope.HRMD_DepartmentName = promise.staff_wisedata[0].HRMD_DepartmentName;
                $scope.HRMDES_DesignationName = promise.staff_wisedata[0].HRMDES_DesignationName;
                $scope.HRME_EmployeeCode = promise.staff_wisedata[0].HRME_EmployeeCode;
                $scope.all_requestdata = promise.all_requestdata;

            });
        };

        $scope.edittab1 = function (user) {

            var data = {
                "HLHSTREQ_Id": user.HLHSTREQ_Id
            };
            apiService.create("StaffRequest/edittab1", data).then(function (promise) {
                $scope.HLHSTREQ_Id = promise.editlist[0].hlhstreQ_Id;
                $scope.HLHSTREQ_RequestDate = new Date(promise.editlist[0].hlhstreQ_RequestDate);
                $scope.HLMH_Id = promise.editlist[0].hlmH_Id;
                $scope.HLMRCA_Id = promise.editlist[0].hlmrcA_Id;


                if (promise.editlist[0].hlhstreQ_ACRoomFlg === true) {
                    $scope.HLHSTREQ_ACRoomFlg = 1;
                }
                if (promise.editlist[0].hlhstreQ_SingleRoomFlg === true) {
                    $scope.HLHSTREQ_SingleRoomFlg = 1;
                }
                if (promise.editlist[0].hlhstreQ_VegMessFlg === true) {
                    $scope.HLHSTREQ_VegMessFlg = 1;
                }
                if (promise.editlist[0].hlhstreQ_NonVegMessFlg === true) {
                    $scope.HLHSTREQ_NonVegMessFlg = 1;
                }
                $scope.HLHSTREQ_Remarks = promise.editlist[0].hlhstreQ_Remarks;


            });
        };

        $scope.deactivYTab1 = function (usersem, SweerAlert) {

            var dystring = "";
            if (usersem.HLHSTREQ_ActiveFlag === true) {
                dystring = "Deactivate";
            }
            else if (usersem.HLHSTREQ_ActiveFlag === false) {
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
                        apiService.create("StaffRequest/deactive", usersem).
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