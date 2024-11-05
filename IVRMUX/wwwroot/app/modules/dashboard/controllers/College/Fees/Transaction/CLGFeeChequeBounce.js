(function () {
    'use strict';

    angular
        .module('app')
        .controller('CLGFeeChequeBounceController', CLGFeeChequeBounceController);

    CLGFeeChequeBounceController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];

    function CLGFeeChequeBounceController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'CLGFeeChequeBounce';
        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));
        $scope.cfg = {};

        activate();

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
            $scope.searchValue = "";
            apiService.getURI("CLGFeeChequeBounce/getalldetails", 2).
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
                    $scope.saveddata = promise.alldata;
                })
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

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
                $scope.getdates();
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

        $scope.get_students = function () {
            if ($scope.AMSE_Id != undefined && $scope.AMSE_Id != "") {

                var data = {
                    "ASMAY_Id": $scope.cfg.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "AMSE_Id": $scope.AMSE_Id
                }
                apiService.create("CLGFeeChequeBounce/get_students", data).then(function (promise) {
                    $scope.student_list = promise.studentlist;
                    $scope.AMCST_Id = "";
                })
            }
            else {
                $scope.student_list = [];
                $scope.AMCST_Id = "";
            }

        };
        $scope.get_receipts = function () {
            if ($scope.AMCST_Id != undefined && $scope.AMCST_Id != "") {

                var data = {
                    "ASMAY_Id": $scope.cfg.ASMAY_Id,
                    //"AMCO_Id": $scope.AMCO_Id,
                    //"AMB_Id": $scope.AMB_Id,
                    //"AMSE_Id": $scope.AMSE_Id,
                    "AMCST_Id": $scope.AMCST_Id
                }
                apiService.create("CLGFeeChequeBounce/get_receipts", data).then(function (promise) {
                    $scope.receipt_list = promise.receiptlist;
                    $scope.FYP_Id = "";
                })
            }
            else {
                $scope.receipt_list = [];
                $scope.FYP_Id = "";
            }

        };
        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "FCCB_Id": $scope.FCCB_Id,
                    "ASMAY_Id": $scope.cfg.ASMAY_Id,
                    //"AMCO_Id": $scope.AMCO_Id,
                    //"AMB_Id": $scope.AMB_Id,
                    //"AMSE_Id": $scope.AMSE_Id,
                    "AMCST_Id": $scope.AMCST_Id,
                    "FYP_Id": $scope.FYP_Id,
                    "FCCB_Date": new Date($scope.FCCB_Date).toDateString(),
                    "FCCB_Remarks": $scope.FCCB_Remarks
                }
                apiService.create("CLGFeeChequeBounce/savedata", data).then(function (promise) {
                    if (promise.returnval) {
                        if ($scope.FCCB_Id > 0) {
                            swal("Record Updated Successfully");
                        } else {
                            swal("Record Saved Successfully");
                        }
                    }
                    else if (!promise.returnval) {
                        if ($scope.FCCB_Id > 0) {
                            swal("Failed To Update Record");
                        } else {
                            swal("Failed To Save Record");
                        }
                    }
                    else {
                        swal("Something Went Wrong Please Contact Administrator");
                    }
                    $scope.clear();
                    $scope.saveddata = promise.alldata;
                })

            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.clear = function () {
            $scope.FCCB_Id = 0;            $scope.cfg.ASMAY_Id = "";            $scope.course_list = [];
            $scope.branch_list = [];
            $scope.semister_list = [];            $scope.student_list = [];
            $scope.receipt_list = [];            $scope.AMCO_Id = "";            $scope.AMB_Id = "";            $scope.AMSE_Id = "";            $scope.AMCST_Id = "";            $scope.FYP_Id = "";            $scope.FCCB_Date = null;            $scope.FCCB_Remarks = "";            $scope.all = false;            $scope.student_list = [];
            $scope.submitted = false;
            $scope.myForm.$setPristine();
            $scope.myForm.$setUntouched();
            $scope.searchValue = "";
        };
        $scope.getdates = function () {
            var data = null;
            angular.forEach($scope.year_list, function (yr) {
                if (yr.asmaY_Id == $scope.cfg.ASMAY_Id) {
                    data = yr.asmaY_Year;
                }
            })
            if (data != null) {
                debugger;
                console.log(data);
                var name, name1;
                for (var i = 0; i < data.length; i++) {
                    if (i < 4) {
                        if (i == 0) {
                            name = data[i];
                        } else {
                            name += data[i];
                        }
                    }
                    if (i > 4) {
                        if (i == 5) {
                            name1 = data[5];
                        } else {
                            name1 += data[i];
                        }
                    }
                }
                $scope.fromDate = name;
                $scope.toDate = name1;
                $scope.frommon = "";
                $scope.tomon = "";
                $scope.fromDay = "";
                $scope.toDay = "";
                $scope.minDatemf = new Date(
                    $scope.fromDate,
                    $scope.frommon,
                    $scope.fromDay + 1);

                $scope.maxDatemf = new Date(
                    $scope.toDate,
                    $scope.tomon,
                    $scope.toDay + 365);
                $scope.today = new Date();
            }
        }

        $scope.DeletRecord = function (employee, SweetAlert) {
            $scope.editEmployee = employee.fccB_Id;
            var feechequebounceid = $scope.editEmployee
            swal({
                title: "Are you sure?",
                text: "Do You Want To Delete Record?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.DeleteURI("CLGFeeChequeBounce/DeletRecord", feechequebounceid).
                            then(function (promise) {
                                if (promise.returnval == true) {
                                    swal('Record Deleted Successfully');
                                }
                                else {
                                    swal('Record Not Deleted Successfully');
                                }
                                $scope.clear();
                                $scope.saveddata = promise.alldata;
                            })
                    }
                    else {
                        swal("Record Deletion Cancelled");
                    }
                });
        }
    }
})();
