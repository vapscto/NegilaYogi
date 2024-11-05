(function () {
    'use strict';
    angular
        .module('app')
        .controller('TC_ClassTeacher_ApprovalController', TC_ClassTeacher_ApprovalController)

    TC_ClassTeacher_ApprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function TC_ClassTeacher_ApprovalController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

        $scope.sortKey = 'ATCCTAPP_Id';
        $scope.sortReverse = true;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        }



        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings.length > 0) {
            var logopath = admfigsettings[0].asC_Logo_Path;
        }

        $scope.imgname = logopath;

        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));



        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        }
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {

            return (angular.lowercase(obj.mob)).indexOf(angular.lowercase($scope.searchValue)) >= 0
        }

        //get data
        $scope.loaddata = function () {

            $scope.currentPage = 1;
            $scope.itemsPerPage = paginationformasters;
            var pageid = 2;
            apiService.getURI("EmployeeLeaveApply/getdata_CTA", pageid)
                .then(function (promise) {
                    $scope.student_dd = promise.student_dd;
                    $scope.tc_ct_list = promise.tc_ct_list;
                    $scope.presentCountgrid = $scope.tc_ct_list.length;
                    $scope.editopn = false;
                })
        }

        //save
        $scope.submitted = false;
        $scope.savedata = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "AMST_Id": $scope.AMST_Id,
                    "ATCCTAPP_ApprovedDate": $scope.ATCCTAPP_ApprovedDate,
                    "ATCCTAPP_AttendanceApprovalFlg": true,
                    "ATCCTAPP_ExamApprovalFlg": true,
                    "ATCCTAPP_Remarks": $scope.ATCCTAPP_Remarks,
                    "ATCCTAPP_Id": $scope.Id
                }
                apiService.create("EmployeeLeaveApply/SaveEdit_CTA", data).
                    then(function (promise) {


                        if (promise.returndata === "Update") {
                            swal('Record Updated Successfully.');
                            $scope.AMST_Id = "";
                            $scope.studentname = "";
                            $scope.ATCCTAPP_ApprovedDate = "";
                            $scope.ATCCTAPP_AttendanceApprovalFlg = false;
                            $scope.ATCCTAPP_ExamApprovalFlg = false;
                            $scope.ATCCTAPP_Remarks = "";
                            $scope.libstudetails = [];
                            $scope.exmdetails = [];
                            $scope.loaddata();
                            return;
                        }
                        else if (promise.returndata === "Add") {
                            swal('Record Saved Successfully.');
                            $scope.AMST_Id = "";
                            $scope.studentname = "";
                            $scope.ATCCTAPP_ApprovedDate = "";
                            $scope.ATCCTAPP_AttendanceApprovalFlg = false;
                            $scope.ATCCTAPP_ExamApprovalFlg = false;
                            $scope.ATCCTAPP_Remarks = "";
                            $scope.libstudetails = [];
                            $scope.exmdetails = [];
                            $scope.loaddata();
                            return;
                        }
                        else if (promise.returndata === "Error") {
                            swal('Record Not Saved/Updated successfully.');
                            $scope.AMST_Id = "";
                            $scope.studentname = "";
                            $scope.ATCCTAPP_ApprovedDate = "";
                            $scope.ATCCTAPP_AttendanceApprovalFlg = false;
                            $scope.ATCCTAPP_ExamApprovalFlg = false;
                            $scope.ATCCTAPP_Remarks = "";
                            $scope.libstudetails = [];
                            $scope.exmdetails = [];
                            $scope.loaddata();
                            return;
                        }

                    })

            }
            else {

                $scope.submitted = true;
            }

        };

        //edit
        $scope.edibl = {}
            $scope.edit = function (bil) {
            $scope.edibl = bil.ATCCTAPP_Id;
            var pageid = $scope.edibl;
            apiService.getURI("EmployeeLeaveApply/details_CTA", pageid).
                then(function (promise) {
                    $scope.editopn =true;
                    $scope.Id = promise.tc_ct_details[0].ATCCTAPP_Id;
                    $scope.AMST_Id = promise.tc_ct_details[0].AMST_Id;
                    $scope.studentname = promise.tc_ct_details[0].studentname;
                    $scope.ATCCTAPP_ApprovedDate = new Date(promise.tc_ct_details[0].ATCCTAPP_ApprovedDate);
                    $scope.ATCCTAPP_AttendanceApprovalFlg = promise.tc_ct_details[0].ATCCTAPP_AttendanceApprovalFlg;
                    $scope.ATCCTAPP_ExamApprovalFlg = promise.tc_ct_details[0].ATCCTAPP_ExamApprovalFlg;
                    $scope.ATCCTAPP_Remarks = promise.tc_ct_details[0].ATCCTAPP_Remarks;

                })
        };

        $scope.getstudetails = function (ww) {
            var data = {
                "AMST_Id":ww
            }
            apiService.create("EmployeeLeaveApply/getstudetails_CTA", data).then(function (promise) {
                $scope.libstudetails = promise.libstudetails;
                $scope.exmdetails = promise.exmdetails;
            });
        }

        $scope.cancel = function () {
            $scope.AMST_Id = "";
            $scope.studentname = "";
            $scope.ATCCTAPP_ApprovedDate = "";
            $scope.ATCCTAPP_AttendanceApprovalFlg = false;
            $scope.ATCCTAPP_ExamApprovalFlg = false;
            $scope.ATCCTAPP_Remarks = "";
            $scope.libstudetails = [];
            $scope.exmdetails = [];
            $scope.editopn = false;
            
        }

        //deactive
        $scope.deactive = function (flr, SweetAlertt) {

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            var mgs = "";
            if (flr.ISMCLTPRBOM_ActiveFlag === false) {
                mgs = "Activate";
            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " the Academic Year?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55",
                confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("EmployeeLeaveApply/deactivate_CTA", flr).
                            then(function (promise) {


                                if (promise.returndata === 'false') {
                                    swal('Client Project Bill Of Material Deactivated Successfully.');
                                }

                                else if (promise.returndata === 'true') {
                                    swal('Client Project Bill Of Material Activated Successfully.');
                                }

                                else if (promise.returndata === 'Error') {
                                    swal('Operation Failed!!!');
                                }

                              
                                $scope.loaddata();
                              
                            });
                    } else {
                        swal("Cancelled");
                        $scope.loaddata();
                    }

                });
        };


        $scope.interacted = function (field) {
            return $scope.submitted || field.$dirty;
        };


    }

})();



