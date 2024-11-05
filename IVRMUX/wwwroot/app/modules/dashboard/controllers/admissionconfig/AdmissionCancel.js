(function () {
    'use strict';
    angular.module('app').controller('AdmissionCancelOrWDController', AdmissionCancelOrWDController)

    AdmissionCancelOrWDController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$stateParams', '$filter']
    function AdmissionCancelOrWDController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $stateParams, $filter) {

        $scope.edit = false;
        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null && ivrmcofigsettings.length > 0) {
            paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
        } else {
            paginationformasters = 10;
        }

        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        if (privlgs !== null && privlgs.length > 0) {
            for (var i = 0; i < privlgs.length; i++) {
                if (privlgs[i].pageId == pageid) {
                    $scope.userPrivileges = privlgs[i];
                    console.log($scope.userPrivileges);
                }
            }
        }
        $scope.obj = {};
        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;

        $scope.OnLoadAdmissionCancel = function () {
            var pageid = 2;
            apiService.getURI("StudentAdmission/OnLoadAdmissionCancel", pageid).then(function (promise) {
                if (promise !== null) {
                    $scope.allAcademicYear = promise.allAcademicYear;
                    $scope.academicYearOnLoad = promise.academicYearOnLoad;
                   // $scope.ASMAY_Id = promise.asmaY_Id;

                    $scope.TempYear = [];

                    $scope.CurrentYearOrder = $scope.allAcademicYear[0].asmaY_Order;
                    $scope.prevorder = $scope.CurrentYearOrder - 1;
                    $scope.TempYear = $scope.allAcademicYear;

                    angular.forEach($scope.academicYearOnLoad, function (dd) {
                        if (dd.asmaY_Order === $scope.prevorder) {
                            $scope.TempYear.push(dd);
                        }
                    });

                    if (promise.getstudentdetailslist !== undefined && promise.getstudentdetailslist !== null && promise.getstudentdetailslist.length > 0) {
                        $scope.getstudentdetailslist = promise.getstudentdetailslist;
                    } else {
                        swal("No Records Found");
                    }
                    $scope.getwdstudentdetails = promise.getwdstudentdetails;
                }
            });
        };

        $scope.OnChangeAdmissionCancelYear = function () {
            $scope.getstudentdetailslist = [];
            $scope.getstudentdetails = [];
            $scope.getwdstudentdetails = [];
            $scope.AMST_Id = "";
            $scope.edit = false;
            $scope.submitted = false;
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id
            };

            apiService.create("StudentAdmission/OnChangeAdmissionCancelYear", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.getstudentdetailslist !== undefined && promise.getstudentdetailslist !== null && promise.getstudentdetailslist.length > 0) {
                        $scope.getstudentdetailslist = promise.getstudentdetailslist;
                    } else {
                        swal("No Records Found");
                    }
                    if (promise.getwdstudentdetails !== undefined && promise.getwdstudentdetails !== null && promise.getwdstudentdetails.length > 0) {
                        $scope.getwdstudentdetails = promise.getwdstudentdetails;
                    }
                    else {
                        swal("No Records Found");
                    }
                }
            });
        };

        $scope.OnChangeAdmissionCancelStudent = function () {
            $scope.edit = false;
            $scope.submitted = false;
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "AMST_Id": $scope.AMST_Id.amsT_Id
            };

            apiService.create("StudentAdmission/OnChangeAdmissionCancelStudent", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.getstudentdetails !== undefined && promise.getstudentdetails !== null && promise.getstudentdetails.length > 0) {
                        $scope.getstudentdetails = promise.getstudentdetails;

                        $scope.FirstStudentName = $scope.getstudentdetails[0].studentname;
                        $scope.FirstStudentAdmNo = $scope.getstudentdetails[0].amsT_AdmNo;
                        $scope.FirstStudentclass = $scope.getstudentdetails[0].classname;
                        $scope.AMST_FatherName = $scope.getstudentdetails[0].amsT_FatherName;
                        $scope.AMST_MotherName = $scope.getstudentdetails[0].amsT_MotherName;
                        $scope.PhotoName = $scope.getstudentdetails[0].photoName;
                        $scope.obj.AACA_CancellationFee = "";
                        $scope.obj.AACA_ToRefundAmount = "";
                        $scope.obj.AACA_ACReason = "";
                        $scope.AACA_ACDate = new Date();
                        $scope.mindate = new Date($scope.getstudentdetails[0].AMST_Date);
                        $scope.maxdate = new Date();
                    } else {
                        swal("No Records Found");
                    }
                }
            });
        };

        $scope.SaveAdmissionCancelStudent = function () {
            if ($scope.myform.$valid) {

                swal({
                    title: "Are you sure",
                    text: "Do You Want To Cancel The Admission ?",
                    type: "warning",
                    showCancelButton: true,
                    confirmButtonColor: "#DD6B55", confirmButtonText: "Cancel The Admission",
                    cancelButtonText: "Cancel",
                    closeOnConfirm: false,
                    closeOnCancel: false
                },
                    function (isConfirm) {
                        if (isConfirm) {
                            var data = {
                                "AACA_Id": $scope.AACA_Id,
                                "ASMAY_Id": $scope.ASMAY_Id,
                                "AMST_Id": $scope.AMST_Id.amsT_Id,
                                "AACA_CancellationFee": $scope.obj.AACA_CancellationFee === null || $scope.obj.AACA_CancellationFee === "" ? null
                                    : $scope.obj.AACA_CancellationFee,
                                "AACA_ToRefundAmount": $scope.obj.AACA_ToRefundAmount === null || $scope.obj.AACA_ToRefundAmount === "" ? null : $scope.obj.AACA_ToRefundAmount,
                                "AACA_ACReason": $scope.obj.AACA_ACReason,
                                "AACA_ACDate": new Date($scope.AACA_ACDate).toDateString()
                            };
                            apiService.create("StudentAdmission/SaveAdmissionCancelStudent", data).then(function (promise) {
                                if (promise !== null) {
                                    if (promise.returnval === true) {
                                        swal("Record Saved/Updated Successfully");
                                    } else {
                                        swal("Failed To  Save/Update");
                                    }
                                    $scope.cleardata();
                                }
                            });
                        }
                        else {
                            swal("Submittion Cancelled");
                        }
                    });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.EditAdmissionCancelStudent = function (dd) {

            var data = {
                "AACA_Id": dd.aacA_Id,
                "AMST_Id": dd.amsT_Id,
                "ASMAY_Id": dd.asmaY_Id
            };

            apiService.create("StudentAdmission/EditAdmissionCancelStudent", data).then(function (promise) {

                if (promise !== null) {
                    $scope.geteditdetails = promise.geteditdetails;

                    $scope.getstudentdetailslist = promise.getstudentdetailslist;

                    $scope.getstudentdetails = promise.getstudentdetails;

                    $scope.AMST_Id = $scope.getstudentdetails[0];
                    $scope.ASMAY_Id = dd.asmaY_Id;

                    $scope.FirstStudentName = $scope.getstudentdetails[0].amsT_FirstName;
                    $scope.FirstStudentAdmNo = $scope.getstudentdetails[0].amsT_AdmNo;
                    $scope.FirstStudentclass = $scope.getstudentdetails[0].classname;
                    $scope.AMST_FatherName = $scope.getstudentdetails[0].amsT_FatherName;
                    $scope.AMST_MotherName = $scope.getstudentdetails[0].amsT_MotherName;
                    $scope.PhotoName = $scope.getstudentdetails[0].photoName;

                    $scope.obj.AACA_CancellationFee = $scope.geteditdetails[0].aacA_CancellationFee;
                    $scope.obj.AACA_ToRefundAmount = $scope.geteditdetails[0].aacA_ToRefundAmount;
                    $scope.obj.AACA_ACReason = $scope.geteditdetails[0].aacA_ACReason;
                    $scope.AACA_ACDate = new Date($scope.geteditdetails[0].aacA_ACDate);
                    $scope.AACA_Id = $scope.geteditdetails[0].aacA_Id;

                    $scope.mindate = new Date($scope.getstudentdetails[0].AMST_Date);
                    $scope.maxdate = new Date();
                    $scope.edit = true;
                }
            });
        };

        $scope.cleardata = function () {

            $scope.getstudentdetails = [];
            $scope.getwdstudentdetails = [];
            $scope.allAcademicYear = [];
            $scope.academicYearOnLoad = [];
            $scope.ASMAY_Id = "";
            $scope.AMST_Id = "";
            $scope.FirstStudentName = "";
            $scope.FirstStudentAdmNo = "";
            $scope.FirstStudentclass = "";
            $scope.AMST_FatherName = "";
            $scope.AMST_MotherName = "";
            $scope.PhotoName = "";
            $scope.AACA_CancellationFee = "";
            $scope.AACA_ToRefundAmount = "";
            $scope.obj.AACA_ACReason = "";
            $scope.AACA_ACDate = new Date();

            $scope.OnLoadAdmissionCancel();
        };

        $scope.interacted1 = function (field) {
            return $scope.submitted;
        };

        $scope.filterValue1 = function (obj) {
            return ($filter('aacA_ACDate')(obj.aacA_ACDate, 'dd-MM-yyyy').indexOf($scope.search) >= 0) ||
                (angular.lowercase(obj.studentname)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.amsT_AdmNo)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.asmaY_Year)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.asmcL_ClassName)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.asmC_SectionName)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.amsT_AdmNo)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (angular.lowercase(obj.aacA_ACReason)).indexOf(angular.lowercase($scope.search)) >= 0 ||
                (JSON.stringify(obj.aacA_CancellationFee)).indexOf($scope.search) >= 0 ||
                (JSON.stringify(obj.aacA_ToRefundAmount)).indexOf($scope.search) >= 0;
        };

    }
})();