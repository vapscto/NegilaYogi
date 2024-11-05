(function () {
    'use strict';

    angular
        .module('app')
        .controller('CLGFeeOpeningBalance', CLGFeeOpeningBalance);

    CLGFeeOpeningBalance.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];

    function CLGFeeOpeningBalance($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'CLGFeeOpeningBalance';

        activate();
        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));
        $scope.cfg = {};
        function activate() { }


        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null) {
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            }
        }
        else {
            paginationformasters = 10;
        }

        $scope.BindData = function () {
            debugger;
            apiService.getURI("CLGFeeOpeningBalance/getalldetails", 2).
                then(function (promise) {
                    $scope.currentPage = 1;
                    $scope.itemsPerPage = paginationformasters;
                    $scope.year_list = promise.yearlist;
                    $scope.cfg.ASMAY_Id = academicyrlst[0].asmaY_Id;
                    $scope.get_courses();
                    //  $scope.course_list = promise.courselist;
                    //  $scope.branch_list = promise.branchlist;
                    //  $scope.semister_list = promise.semisterlist;
                    // $scope.section_list = promise.sectionlist;
                    $scope.student_list = [];
                })
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.get_courses = function () {
            $scope.student_list = [];
            if ($scope.cfg.ASMAY_Id != undefined && $scope.cfg.ASMAY_Id != "") {

                var data = {
                    "ASMAY_Id": $scope.cfg.ASMAY_Id,
                }
                apiService.create("CLGFeeOpeningBalance/get_courses", data).then(function (promise) {
                    $scope.course_list = promise.courselist;
                    $scope.AMCO_Id = "";
                    //$scope.group_list = promise.grouplist;
                    //$scope.FMG_Id = "";
                })
            }
            else {
                $scope.course_list = [];
                $scope.AMCO_Id = "";
                //$scope.group_list = [];
                //$scope.FMG_Id = "";
            }

        };

        $scope.get_branches = function () {
            $scope.student_list = [];
            if ($scope.AMCO_Id != undefined && $scope.AMCO_Id != "") {
                var data = {
                    "ASMAY_Id": $scope.cfg.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id
                }
                apiService.create("CLGFeeOpeningBalance/get_branches", data).then(function (promise) {
                    $scope.branch_list = promise.branchlist;
                    $scope.AMB_Id = "";
                })
            }
            else {
                $scope.branch_list = [];
                $scope.AMB_Id = "";
            }
        };

        $scope.get_semisters = function () {
            $scope.student_list = [];
            if ($scope.AMB_Id != undefined && $scope.AMB_Id != "") {
                var data = {
                    "ASMAY_Id": $scope.cfg.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id
                }
                apiService.create("CLGFeeOpeningBalance/get_semisters", data).then(function (promise) {
                    $scope.semister_list = promise.semisterlist;
                    $scope.AMSE_Id = "";
                })
            }
            else {
                $scope.semister_list = [];
                $scope.AMSE_Id = "";
            }
        };

        $scope.get_groups = function () {
            $scope.student_list = [];
            if ($scope.AMSE_Id != undefined && $scope.AMSE_Id != "") {

                var data = {
                    "ASMAY_Id": $scope.cfg.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "AMSE_Id": $scope.AMSE_Id
                }
                apiService.create("CLGFeeOpeningBalance/get_groups", data).then(function (promise) {
                    $scope.group_list = promise.grouplist;
                    $scope.FMG_Id = "";
                })
            }
            else {
                $scope.group_list = [];
                $scope.FMG_Id = "";
            }

        };

        $scope.get_heads = function () {
            $scope.student_list = [];
            if ($scope.cfg.ASMAY_Id != undefined && $scope.cfg.ASMAY_Id != "" && $scope.FMG_Id != undefined && $scope.FMG_Id != "") {

                var data = {
                    "ASMAY_Id": $scope.cfg.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "FMG_Id": $scope.FMG_Id
                }
                apiService.create("CLGFeeOpeningBalance/get_heads", data).then(function (promise) {
                    $scope.head_list = promise.headlist;
                    $scope.FMH_Id = "";
                })
            }
            else {
                $scope.course_list = [];
                $scope.AMCO_Id = "";
            }

        };
        $scope.get_installments = function () {
            $scope.student_list = [];
            if ($scope.cfg.ASMAY_Id != undefined && $scope.cfg.ASMAY_Id != "" && $scope.FMG_Id != undefined && $scope.FMG_Id != "" && $scope.FMH_Id != undefined && $scope.FMH_Id != "") {

                var data = {
                    "ASMAY_Id": $scope.cfg.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "FMG_Id": $scope.FMG_Id,
                    "FMH_Id": $scope.FMH_Id
                }
                apiService.create("CLGFeeOpeningBalance/get_installments", data).then(function (promise) {
                    $scope.installment_list = promise.installmentlist;
                    $scope.FTI_Id = "";
                })
            }
            else {
                $scope.installment_list = [];
                $scope.FTI_Id = "";
            }

        };

        $scope.get_students = function () {
            $scope.student_list = [];
            if ($scope.myForm.$valid) {
                var data = {
                    "ASMAY_Id": $scope.cfg.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "AMSE_Id": $scope.AMSE_Id,
                    "FMG_Id": $scope.FMG_Id,
                    "FMH_Id": $scope.FMH_Id,
                    "FTI_Id": $scope.FTI_Id,
                    //"ACMS_Id": $scope.ACMS_Id
                }
                apiService.create("CLGFeeOpeningBalance/get_students", data).then(function (promise) {
                    $scope.student_list = promise.studentlist;
                    $scope.All_S = false;
                    angular.forEach($scope.student_list, function (stu) {
                        angular.forEach(promise.saveddata, function (stu_S) {
                            if (stu_S.amcsT_Id == stu.amcsT_Id) {
                                stu.xyz = true;
                                stu.FCMOB_Institution_Due = stu_S.fcmoB_Institution_Due;
                                stu.FCMOB_Student_Due = stu_S.fcmoB_Student_Due;
                            }
                        })
                    })
                    $scope.optionToggled();
                    $scope.saveddata = promise.saveddata;
                    if (promise.studentlist.length == 0 || promise.studentlist == "" || promise.studentlist == null) {
                        swal("For Selected Details Students Are Not Mapped!!!");
                    }
                })
            }
            else {
                $scope.submitted = true;
            }
        };

        $scope.toggleAll = function () {
            angular.forEach($scope.student_list, function (stu) {
                stu.xyz = $scope.all;
            })
        };

        $scope.optionToggled = function () {
            $scope.all = $scope.student_list.every(function (itm) { return itm.xyz; });
        };
        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        $scope.submitted = false;
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var ids = [];
                var nonvalid_cnt = 0;
                angular.forEach($scope.student_list, function (stu) {
                    if (stu.xyz) {
                        if (stu.FCMOB_Student_Due == 0 && stu.FCMOB_Institution_Due == 0) {
                            nonvalid_cnt += 1;
                        }
                        ids.push({ AMCST_Id: stu.amcsT_Id, FCMOB_Student_Due: stu.FCMOB_Student_Due, FCMOB_Institution_Due: stu.FCMOB_Institution_Due });
                    }
                })
                if (ids.length > 0) {
                    //if (nonvalid_cnt = 0) {
                        var data = {
                            // "ACASMP_Id": $scope.ACASMP_Id,
                            "ASMAY_Id": $scope.cfg.ASMAY_Id,
                            //"AMCO_Id": $scope.AMCO_Id,
                            //"AMB_Id": $scope.AMB_Id,
                            //"AMSE_Id": $scope.AMSE_Id,   
                            "FMG_Id": $scope.FMG_Id,
                            "FMH_Id": $scope.FMH_Id,
                            "FTI_Id": $scope.FTI_Id,
                            "sub_data": ids
                        }
                        apiService.create("CLGFeeOpeningBalance/savedata", data).then(function (promise) {
                            if (promise.returnval) {
                                if ($scope.saveddata.length > 0) {
                                    swal("Record Updated Successfully");
                                } else {
                                    swal("Record Saved Successfully");
                                }
                            }
                            else if (!promise.returnval) {
                                if ($scope.saveddata.length > 0) {
                                    swal("Failed To Update Record");
                                } else {
                                    swal("Failed To Save Record");
                                }
                            }
                            else {
                                swal("Something Went Wrong Please Contact Administrator");
                            }
                            $scope.clear();
                        })
                    //}
                    //else {
                    //    swal("Institution Or Student Due Must Be More Than Zero Of Selected Student!!!");
                    //}

                }
                else {
                    swal("Select Atleast Student To Save Data!!!");
                }

            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.isOptionsRequired1 = function () {
            return !$scope.student_list.some(function (options) {
                return options.checked;
            });
        };
        $scope.clear = function () {
            $scope.ACASMP_Id = 0;            $scope.cfg.ASMAY_Id = "";            $scope.course_list = [];
            $scope.branch_list = [];
            $scope.semister_list = [];            $scope.group_list = [];
            $scope.head_list = [];
            $scope.installment_list = [];            $scope.AMCO_Id = "";            $scope.AMB_Id = "";            $scope.AMSE_Id = "";            // $scope.ACMS_Id = "";            $scope.FMG_Id = "";            $scope.FMH_Id = "";            $scope.FTI_Id = "";            $scope.all = false;            $scope.student_list = [];            $scope.saveddata = [];
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.search = "";
        };
    }
})();
