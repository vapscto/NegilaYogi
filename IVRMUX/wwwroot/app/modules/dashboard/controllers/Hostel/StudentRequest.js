
(function () {
    'use strict';
    angular
        .module('app')
        .controller('StudentRequest', StudentRequest)
    StudentRequest.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter']
    function StudentRequest($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter) {

        $scope.obj = {};
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
        $scope.save = function () {

            var RequestDate = $scope.HLHSREQ_RequestDate === null ? "" : $filter('date')($scope.HLHSREQ_RequestDate, "yyyy-MM-dd");

            if ($scope.myForm.$valid) {
                var data = {
                    "HLHSREQ_Id": $scope.HLHSREQ_Id,
                    "HLHSREQ_RequestDate": RequestDate,
                    //"HLHSREQ_RequestDate": new Date($scope.HLHSREQ_RequestDate).toDateString(),
                    "HLMH_Id": $scope.HLMH_Id,
                    "HLMRCA_Id": $scope.HLMRCA_Id,
                    "AMST_Id": $scope.AMST_Id,
                    "HLHSREQ_ACRoomFlg": $scope.HLHSREQ_ACRoomFlg,
                    "HLHSREQ_SingleRoomFlg": $scope.HLHSREQ_SingleRoomFlg,
                    "HLHSREQ_VegMessFlg": $scope.HLHSREQ_VegMessFlg,
                    "HLHSREQ_NonVegMessFlg": $scope.HLHSREQ_NonVegMessFlg,
                    "HLHSREQ_Remarks": $scope.HLHSREQ_Remarks,
                };

                apiService.create("StudentRequest/save", data).then(function (promise) {
                    if (promise.msg === 'Saved') {
                        swal("Data Saved Successfully....!!!!");
                        $state.reload();
                    } else if (promise.msg === 'Failed') {
                        swal("Data  Not Saved Successfully....!!!!")                      
                    }
                    else if (promise.msg === 'updated') {
                        swal("Data Updated Successfully.!");
                        $state.reload();
                    }
                    else if (promise.msg === 'failed') {
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
            apiService.create("StudentRequest/loaddata", data).then(function (promise) {
                $scope.hostel_list = promise.hostel_list;
                $scope.room_list = promise.room_list;
                $scope.student_wisedata = promise.student_wisedata;
                // if ($scope.student_wisedata.length > 0) {
                $scope.studentName = $scope.student_wisedata[0].studentName;
                $scope.AMST_Id = $scope.student_wisedata[0].AMST_Id;
                $scope.ASMCL_ClassName = $scope.student_wisedata[0].ASMCL_ClassName;
                $scope.AMST_RegistrationNo = $scope.student_wisedata[0].AMST_RegistrationNo;
                $scope.AMST_AdmNo = $scope.student_wisedata[0].AMST_AdmNo;
                // }

                $scope.all_requestdata = promise.all_requestdata;

            });
        };
        $scope.edittab1 = function (user) {

            var data = {
                "HLHSREQ_Id": user.HLHSREQ_Id
            };
            apiService.create("StudentRequest/edittab1", data).then(function (promise) {
                $scope.HLHSREQ_Id = promise.editlist[0].hlhsreQ_Id;
                $scope.HLHSREQ_RequestDate = new Date(promise.editlist[0].hlhsreQ_RequestDate);
                $scope.HLMH_Id = promise.editlist[0].hlmH_Id;
                $scope.HLMRCA_Id = promise.editlist[0].hlmrcA_Id;
                $scope.AMST_Id = promise.editlist[0].amsT_Id;
                if (promise.editlist[0].hlhsreQ_ACRoomFlg === true) {
                    $scope.HLHSREQ_ACRoomFlg = 1;
                }
                if (promise.editlist[0].hlhsreQ_SingleRoomFlg === true) {
                    $scope.HLHSREQ_SingleRoomFlg = 1;
                }
                if (promise.editlist[0].hlhsreQ_VegMessFlg === true) {
                    $scope.HLHSREQ_VegMessFlg = 1;
                }
                if (promise.editlist[0].hlhsreQ_NonVegMessFlg === true) {
                    $scope.HLHSREQ_NonVegMessFlg = 1;
                }
                $scope.HLHSREQ_Remarks = promise.editlist[0].hlhsreQ_Remarks;

            });
        };
        $scope.deactivYTab1 = function (usersem, SweerAlert) {
            debugger;

            var dystring = "";
            if (usersem.HLHSREQ_ActiveFlag === true) {
                dystring = "Deactivate";
            }
            else if (usersem.HLHSREQ_ActiveFlag === false) {
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
                        apiService.create("StudentRequest/deactive", usersem).
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





