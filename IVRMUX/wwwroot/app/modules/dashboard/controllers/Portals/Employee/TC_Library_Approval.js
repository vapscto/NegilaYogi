(function () {
    'use strict';
    angular
        .module('app')
        .controller('TC_Library_ApprovalController', TC_Library_ApprovalController)

    TC_Library_ApprovalController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function TC_Library_ApprovalController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {

       // $scope.sortKey = 'ATCCTAPP_Id';
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
            apiService.getURI("EmployeeLeaveApply/getdata_LIB", pageid)
                .then(function (promise) {
                    $scope.student_dd = promise.student_dd;
                    $scope.tc_library_list = promise.tc_library_list;
                    $scope.presentCountgrid = $scope.tc_library_list.length;
                    $scope.editopn = false;
                })
        }
        //============
        $scope.getstudetails = function (qq) {
            var data = {
                "AMST_Id": qq
               }
            apiService.create("EmployeeLeaveApply/getstudetails_LIB", data).then(function (promise) {
                $scope.libstudetails = promise.libstudetails;
            });
        }
        //=====================
        //save
        $scope.submitted = false;
        $scope.savedata = function () {

            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "AMST_Id": $scope.AMST_Id,
                    "ATCLIBAPP_ApprovedDate": $scope.ATCLIBAPP_ApprovedDate,
                    "ATCLIBAPP_ApprovalFlg": true,
                    "ATCLIBAPP_Remarks": $scope.ATCLIBAPP_Remarks,
                    "ATCLIBAPP_Id": $scope.Id
                }
                apiService.create("EmployeeLeaveApply/SaveEdit_LIB", data).
                    then(function (promise) {


                        if (promise.returndata === "Update") {
                            swal('Record Updated Successfully.');
                            $scope.AMST_Id = "";
                            $scope.studentname = "";
                            $scope.ATCLIBAPP_ApprovedDate = "";
                            $scope.ATCLIBAPP_ApprovalFlg = false;
                            $scope.ATCLIBAPP_Remarks = "";
                            $scope.loaddata();
                            return;
                        }
                        else if (promise.returndata === "Add") {
                            swal('Record Saved Successfully.');
                            $scope.AMST_Id = "";
                            $scope.studentname = "";
                            $scope.ATCLIBAPP_ApprovedDate = "";
                            $scope.ATCLIBAPP_ApprovalFlg = false;
                            $scope.ATCLIBAPP_Remarks = "";
                            $scope.loaddata();
                            return;
                        }
                        else if (promise.returndata === "Error") {
                            swal('Record Not Saved/Updated successfully.');
                            $scope.AMST_Id = undefined;
                            $scope.studentname = undefined;
                            $scope.ATCLIBAPP_ApprovedDate = undefined;
                            $scope.ATCLIBAPP_ApprovalFlg = false;
                            $scope.ATCLIBAPP_Remarks = undefined;
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
            $scope.edibl = bil.ATCLIBAPP_Id;
            var pageid = $scope.edibl;
            apiService.getURI("EmployeeLeaveApply/details_LIB", pageid).
                then(function (promise) {
                    $scope.editopn = true;
                    $scope.Id = promise.tc_library_details[0].ATCLIBAPP_Id;
                    $scope.AMST_Id = promise.tc_library_details[0].AMST_Id;
                    $scope.studentname = promise.tc_library_details[0].studentname;
                    $scope.ATCLIBAPP_ApprovedDate = new Date(promise.tc_library_details[0].ATCLIBAPP_ApprovedDate);
                    $scope.ATCLIBAPP_ApprovalFlg = promise.tc_library_details[0].ATCLIBAPP_ApprovalFlg;
                    $scope.ATCLIBAPP_Remarks = promise.tc_library_details[0].ATCLIBAPP_Remarks;
                    $scope.ATCLIBAPP_ActiveFlg = promise.tc_library_details[0].ATCLIBAPP_ActiveFlg;

                })
        };



        $scope.cancel = function () {
            $scope.AMST_Id = "";
            $scope.studentname = "";
            $scope.ATCLIBAPP_ApprovedDate = "";
            $scope.ATCLIBAPP_ApprovalFlg = false;
            $scope.ATCLIBAPP_Remarks = "";
            $scope.editopn = false;
            $scope.libstudetails = [];

        }

        //deactive
        $scope.deactive = function (flr, SweetAlertt) {

            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            };
            var mgs = "";
            if (flr.ATCLIBAPP_ActiveFlg === false) {
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
                        apiService.create("EmployeeLeaveApply/deactivate_LIB", flr).
                            then(function (promise) {


                                if (promise.returndata === 'false') {
                                    swal('TC Library Approval Deactivated Successfully.');
                                }

                                else if (promise.returndata === 'true') {
                                    swal('TC Library Approval Activated Successfully.');
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



