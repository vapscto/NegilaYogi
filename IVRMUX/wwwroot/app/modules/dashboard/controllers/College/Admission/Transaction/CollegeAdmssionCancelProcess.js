(function () {
    'use strict';
    angular
        .module('app')
        .controller('CollegeAdmssionCancelProcessController', CollegeAdmssionCancelProcessController)
    CollegeAdmssionCancelProcessController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache']
    function CollegeAdmssionCancelProcessController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
        $scope.obj = {};

        $scope.searc_button = true;
        $scope.sortReverse = true;
        $scope.searchValue = "";
        $scope.student = false;

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings !== null) {
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            } else {
                paginationformasters = 10;
            }
        } else {
            paginationformasters = 10;
        }

        $scope.itemsPerPage = paginationformasters;
        $scope.currentPage = 1;
        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.amsT_Date = $scope.ddate;

        //$scope.usrname = localStorage.getItem('username');
        //var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        //if (admfigsettings.length > 0) {
        //    var logopath = admfigsettings[0].asC_Logo_Path;
        //}
        //$scope.imgname = logopath;
        //var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));
        //----------------------------------Page Load------------------------------------------------


        $scope.loaddata = function () {
            var pageid = 2;
            apiService.getURI("CollegeAdmssionCancelProcess/getalldetails", pageid).
                then(function (promise) {
                    $scope.getYear = promise.yearlist;
                });
        };

        // on year change 

        $scope.onyearchange = function () {
            $scope.students = [];
            $scope.amcO_Id = "";
            $scope.amB_Id = "";
            $scope.amsE_Id = "";
            $scope.acmS_Id = "";
            var data = {
                "ASMAY_Id": $scope.asmaY_Id
            };
            apiService.create("CollegeAdmssionCancelProcess/onyearchange", data).then(function (promise) {

                if (promise !== null) {
                    $scope.getCourse = promise.courselist;
                    if ($scope.getCourse.length === 0) {
                        swal("No Course Details For This Year");
                    }
                } else {
                    swal("No Records Found");
                }

            });
        };


        //-----------Course Change
        $scope.onCoursechange = function () {

            $scope.students = [];
            $scope.amB_Id = "";
            $scope.amsE_Id = "";
            $scope.acmS_Id = "";
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "AMCO_Id": $scope.amcO_Id
            };

            apiService.create("CollegeAdmssionCancelProcess/onCoursechange", data).
                then(function (promise) {
                    if (promise !== null) {
                        $scope.getBranch = promise.branchlist;
                        if ($scope.getBatch.length === 0) {
                            swal("No Records Found");
                        }
                    } else {
                        swal("No Records Found");
                    }
                });
        };

        //-----------Branch Change
        $scope.onBranchchange = function () {

            $scope.students = [];
            $scope.amsE_Id = "";
            $scope.acmS_Id = "";

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "AMCO_Id": $scope.amcO_Id,
                "AMB_Id": $scope.amB_Id
            };

            apiService.create("CollegeAdmssionCancelProcess/onBranchchange", data).
                then(function (promise) {
                    if (promise !== null) {
                        $scope.getSemester = promise.semesterlist;
                        if ($scope.getSemester.length === 0) {
                            swal("No Records Found");
                        }
                    } else {
                        swal("No Records Found");
                    }
                });
        };

        //---- Semester Change
        $scope.onSemchange = function () {

            $scope.students = [];
            $scope.acmS_Id = "";

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "AMCO_Id": $scope.amcO_Id,
                "AMB_Id": $scope.amB_Id,
                "AMSE_Id": $scope.amsE_Id
            };

            apiService.create("CollegeAdmssionCancelProcess/get_Studentdetails", data).
                then(function (promise) {

                    if (promise != null) {
                        $scope.students = promise.studentlist;
                        if ($scope.students.length == 0) {
                            swal("No Records Found");
                        }
                    } else {
                        swal("No Records Found");
                    }
                });
        };


        //--------Section Change
        $scope.onSectionchange = function () {
            $scope.students = [];
        };

        $scope.obj123 = {};

        $scope.getStudentdetails = function (obj123) {
            $scope.submitted = true;

            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "AMCO_Id": $scope.amcO_Id,
                    "AMB_Id": $scope.amB_Id,
                    "AMSE_Id": $scope.amsE_Id,
                    "AMCST_Id": obj123.AMCST_Id.amcsT_Id
                };

                apiService.create("CollegeAdmssionCancelProcess/getStudentdetails", data).then(function (promise) {

                    if (promise !== null) {

                        $scope.studentdetails1 = promise.studentdetails;

                        if ($scope.studentdetails1 !== null || $scope.studentdetails1.length > 0) {

                            $scope.student = true;

                            if (promise.count > 0) {

                                swal({
                                    title: "Section Allotment Is Done For This Student Are you sure?",
                                    text: "Do you want to Cancel Admission For This Student?",
                                    type: "warning",
                                    showCancelButton: true,
                                    confirmButtonColor: "#DD6B55", confirmButtonText: "Yes,Cancel it!",
                                    cancelButtonText: "Cancel..!",
                                    closeOnConfirm: false,
                                    closeOnCancel: false
                                },
                                    function (isConfirm) {
                                        if (isConfirm) {

                                            $scope.canceltypebased = promise.cancellationtype;

                                            $scope.todays = promise.todays;
                                            if ($scope.studentdetails1.length > 0) {
                                                $scope.studentdetails = promise.studentdetails;
                                                $scope.studentname = $scope.studentdetails[0].studentname;
                                                $scope.admno = $scope.studentdetails[0].admno;
                                                $scope.regno = $scope.studentdetails[0].regno;
                                                $scope.doa = $scope.studentdetails[0].doa;
                                                $scope.totalfeepaid = $scope.studentdetails[0].totalfeepaid;
                                                $scope.balancefee = $scope.studentdetails[0].balancefee;
                                                $scope.cancel = $scope.studentdetails[0].cancel + '%';
                                                $scope.refund = $scope.studentdetails[0].refund + '%';
                                                $scope.totalfee = $scope.studentdetails[0].totalfee;

                                                var refundamt = 0;
                                                var cancelamt = 0;

                                                $scope.cancel4 = $scope.studentdetails[0].cancel;
                                                $scope.refund4 = $scope.studentdetails[0].refund;

                                                refundamt = ($scope.totalfee / 100) * $scope.refund4;
                                                cancelamt = ($scope.totalfee / 100) * $scope.cancel4;

                                                $scope.refundamtper = refundamt;
                                                $scope.cancelamtper = cancelamt;

                                                swal("Ok");
                                            }
                                        }
                                        else {
                                            swal("Cancelled");
                                        }
                                    });
                            } else {
                                $scope.canceltypebased = promise.cancellationtype;
                                $scope.studentdetails1 = promise.studentdetails;
                                $scope.todays = promise.todays;

                                if ($scope.studentdetails1.length > 0) {
                                    $scope.studentdetails = promise.studentdetails;
                                    $scope.studentname = $scope.studentdetails[0].studentname;
                                    $scope.admno = $scope.studentdetails[0].admno;
                                    $scope.regno = $scope.studentdetails[0].regno;
                                    $scope.doa = $scope.studentdetails[0].doa;
                                    $scope.totalfeepaid = $scope.studentdetails[0].totalfeepaid;
                                    $scope.balancefee = $scope.studentdetails[0].balancefee;
                                    $scope.cancel = $scope.studentdetails[0].cancel + '%';
                                    $scope.refund = $scope.studentdetails[0].refund + '%';
                                    $scope.totalfee = $scope.studentdetails[0].totalfee;

                                    $scope.cancel4 = $scope.studentdetails[0].cancel;
                                    $scope.refund4 = $scope.studentdetails[0].refund;

                                    var refundamt = 0;
                                    var cancelamt = 0;

                                    refundamt = ($scope.totalfee / 100) * $scope.refund4;
                                    cancelamt = ($scope.totalfee / 100) * $scope.cancel4;

                                    $scope.refundamtper = refundamt;
                                    $scope.cancelamtper = cancelamt;


                                }
                            }
                        } else {
                            swal("Something Went Wrong Kindly Contact Administrator");
                            $scope.student = false;
                        }

                    } else {
                        swal("Something Went Wrong Kindly Contact Administrator");
                        $scope.student = false;
                    }
                });
            } else {
                $scope.submitted = true;
            }
        };


        $scope.toggleAll = function () {
            var toggleStatus = $scope.all;
            $scope.printstudents = [];
            angular.forEach($scope.students, function (itm) {
                itm.selected = toggleStatus;
                if (itm.selected === true) {
                    if ($scope.printstudents.indexOf(itm) === -1) {
                        $scope.printstudents.push(itm);
                    }
                }
            });
        };


        $scope.optionToggled = function (SelectedStudentRecord, index) {
            $scope.all = $scope.students.every(function (itm) { return itm.selected; });
            if ($scope.printstudents.indexOf(SelectedStudentRecord) === -1) {
                $scope.printstudents.push(SelectedStudentRecord);
            }
            else {
                $scope.printstudents.splice($scope.printstudents.indexOf(SelectedStudentRecord), 1);
            }
        };



        //------------------------------Save
        $scope.saveatt = function (obj123) {
            $scope.submitted = false;

            if ($scope.myForm.$valid) {

                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "AMCO_Id": $scope.amcO_Id,
                    "AMB_Id": $scope.amB_Id,
                    "AMSE_Id": $scope.amsE_Id,
                    "ACMS_Id": $scope.acmS_Id,
                    "AMCST_Id": obj123.AMCST_Id.amcsT_Id,
                    "reason": $scope.reason,
                    "refundper": $scope.refund4,
                    "cancelper": $scope.cancel4
                };

                apiService.create("CollegeUsernameCreation/saveatt", data).then(function (promise) {
                    if (promise !== null) {
                        if (promise.returnval === true) {

                            swal("Record Saved Successfully");
                        } else {
                            swal("Failed To Save Record");
                        }
                    } else {
                        swal("Something Went Wrong Please Contact Administrator");
                    }

                    $state.reload();
                });

            } else {
                $scope.submitted = true;
            }
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.clear = function () {
            $state.reload();
        };
        //--------------------------Grid
        $scope.sort = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };

        $scope.IsHidden = true;
        $scope.ShowHide = function () {
            $scope.IsHidden = $scope.IsHidden ? false : true;
        };





    }
})();