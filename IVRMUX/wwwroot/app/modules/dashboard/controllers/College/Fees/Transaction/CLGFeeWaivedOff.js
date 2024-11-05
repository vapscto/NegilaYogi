(function () {
    'use strict';

    angular
        .module('app')
        .controller('CLGFeeWaivedOffController', CLGFeeWaivedOffController);

    CLGFeeWaivedOffController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache'];

    function CLGFeeWaivedOffController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache) {
        /* jshint validthis:true */
        var vm = this;
        vm.title = 'CLGFeeWaivedOff';

        activate();

        var academicyrlst = JSON.parse(localStorage.getItem("academicyearlist"));
        $scope.cfg = {};

        $scope.FCSWO_Date = new Date();

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
            apiService.getURI("CLGFeeWaivedOff/getalldetails", 2).
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
            if ($scope.FCSWO_Id > 0) {} else { $scope.AMCO_Id = "";}
            if ($scope.cfg.ASMAY_Id != undefined && $scope.cfg.ASMAY_Id != "") {
                var data = {
                    "ASMAY_Id": $scope.cfg.ASMAY_Id,
                }
                apiService.create("CLGFeeOpeningBalance/get_courses", data).then(function (promise) {
                    $scope.course_list = promise.courselist;
                   // $scope.AMCO_Id = "";
                    //$scope.group_list = promise.grouplist;
                    //$scope.FMG_Id = "";
                })
                $scope.getdates();
            }
            else {
                $scope.course_list = [];
                //$scope.AMCO_Id = "";
                //$scope.group_list = [];
                //$scope.FMG_Id = "";
            }

        };

        $scope.get_branches = function () {
            if ($scope.FCSWO_Id > 0) { } else { $scope.AMB_Id = ""; }
            $scope.student_list = [];
            if ($scope.AMCO_Id != undefined && $scope.AMCO_Id != "") {
                var data = {
                    "ASMAY_Id": $scope.cfg.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id
                }
                apiService.create("CLGFeeOpeningBalance/get_branches", data).then(function (promise) {
                    $scope.branch_list = promise.branchlist;
                   // $scope.AMB_Id = "";
                })
            }
            else {
                $scope.branch_list = [];
               // $scope.AMB_Id = "";
            }
        };

        $scope.get_semisters = function () {
            $scope.student_list = [];
            if ($scope.FCSWO_Id > 0) { } else { $scope.AMSE_Id = ""; }
            if ($scope.AMB_Id != undefined && $scope.AMB_Id != "") {
                var data = {
                    "ASMAY_Id": $scope.cfg.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id
                }
                apiService.create("CLGFeeOpeningBalance/get_semisters", data).then(function (promise) {
                    $scope.semister_list = promise.semisterlist;
                   // $scope.AMSE_Id = "";
                })
            }
            else {
                $scope.semister_list = [];
               // $scope.AMSE_Id = "";
            }
        };

        $scope.get_students = function () {
            if ($scope.FCSWO_Id > 0) { } else { $scope.AMCST_Id = ""; }
            if ($scope.AMSE_Id != undefined && $scope.AMSE_Id != "") {

                var data = {
                    "ASMAY_Id": $scope.cfg.ASMAY_Id,
                    "AMCO_Id": $scope.AMCO_Id,
                    "AMB_Id": $scope.AMB_Id,
                    "AMSE_Id": $scope.AMSE_Id
                }
                apiService.create("CLGFeeWaivedOff/get_students", data).then(function (promise) {
                    $scope.student_list = promise.studentlist;
                   // $scope.AMCST_Id = "";
                })
            }
            else {
                $scope.student_list = [];
               // $scope.AMCST_Id = "";
            }

        };


        $scope.get_groups = function () {
            if ($scope.AMCST_Id != undefined && $scope.AMCST_Id != "") {

                var data = {
                    "ASMAY_Id": $scope.cfg.ASMAY_Id,
                    //"AMCO_Id": $scope.AMCO_Id,
                    //"AMB_Id": $scope.AMB_Id,
                    //"AMSE_Id": $scope.AMSE_Id,
                    "AMCST_Id": $scope.AMCST_Id,
                    "filterrefund": $scope.filterdata1,
                }
                apiService.create("CLGFeeWaivedOff/get_groups", data).then(function (promise) {
                    $scope.group_list = promise.grouplist;
                })
            }
            else {
                $scope.group_list = [];
            }

        };

        $scope.isOptionsRequired_grp = function () {
            return !$scope.group_list.some(function (options) {
                return options.selected;
            });
        }

        $scope.get_heads = function () {

            var groupidss;
            for (var i = 0; i < $scope.group_list.length; i++) {

                if ($scope.group_list[i].selected == true) {

                    if (groupidss == undefined)
                        groupidss = $scope.group_list[i].fmG_Id;
                    else
                        groupidss = groupidss + "," + $scope.group_list[i].fmG_Id;
                }


            }
            if (groupidss != undefined) {

                var data = {
                    "ASMAY_Id": $scope.cfg.ASMAY_Id,
                    "AMCST_Id": $scope.AMCST_Id,
                    "filterrefund": $scope.filterdata1,
                    "multiplegroup": groupidss
                }

                var config = {
                    headers: {
                        'Content-Type': 'application/json;'
                    }
                }
                apiService.create("CLGFeeWaivedOff/get_heads", data).
                    then(function (promise) {

                        if (promise.headlist.length > 0) {

                            $scope.head_list = promise.headlist;
                        }
                    });
            }
            else {
                $scope.head_list = [];
            }

        }

        $scope.savedata = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var checked_list = [];
                var zero_cnt = 0;
                angular.forEach($scope.head_list, function (stu) {
                    if (stu.xyz) {
                        checked_list.push(stu);
                        if (stu.FCSWO_WaivedOffAmount <= 0) {
                            zero_cnt += 1;
                        }
                    }
                })
                if (checked_list.length > 0 && zero_cnt==0) {
                    var data = {
                        "FCSWO_Id": $scope.FCSWO_Id,
                        "ASMAY_Id": $scope.cfg.ASMAY_Id,
                        //"AMCO_Id": $scope.AMCO_Id,
                        //"AMB_Id": $scope.AMB_Id,
                        //"AMSE_Id": $scope.AMSE_Id,
                        "AMCST_Id": $scope.AMCST_Id,
                        "FCSWO_Date": new Date($scope.FCSWO_Date).toDateString(),
                        checkedlist: checked_list
                    }
                    var config = {
                        headers: {
                            'Content-Type': 'application/json;'
                        }
                    }
                    if ($scope.FCSWO_Id > 0) {
                        var disfun = "Update";
                    }
                    else {
                        var disfun = "Save";
                    }

                    swal({
                        title: "Are you sure?",
                        text: "Do You Want To " + disfun + " Record? ",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55", confirmButtonText: "Yes," + disfun + " it",
                        cancelButtonText: "Cancel",
                        closeOnConfirm: false,
                        closeOnCancel: false,
                        showLoaderOnConfirm: true,

                    },
                        function (isConfirm) {
                            if (isConfirm) {
                                apiService.create("CLGFeeWaivedOff/savedata", data).
                                    then(function (promise) {
                                        if (promise.returnduplicatestatus === "Saved" || promise.returnduplicatestatus === "Updated") {
                                            swal('Record ' + promise.returnduplicatestatus + ' Successfully');
                                            $state.reload();
                                            $scope.BindData();
                                        }
                                        else if (promise.returnduplicatestatus === "adjusted") {
                                            swal('Record already adjusted/Refunded');
                                            $state.reload();
                                            $scope.BindData();
                                        }
                                        else {
                                            swal('Record ' + promise.returnduplicatestatus + ' Successfully');
                                        }
                                    })

                            }
                            else {
                                swal("Record " + disfun + " Cancelled");
                            }
                        })
                }
                else if(checked_list.length==0) {
                    swal("Select Atleast One Head For Saving!!!");
                }
                else if (zero_cnt > 0) {
                    swal("Entered Amount For Selected Heads Must Be Greater Than Zero!!!");
                }
               
            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.clear = function () {
            $scope.FCSWO_Id = 0;
            $scope.cfg.ASMAY_Id = "";
            $scope.course_list = [];
            $scope.branch_list = [];
            $scope.semister_list = [];
            $scope.student_list = [];
            $scope.group_list = [];
            $scope.head_list = [];
            $scope.AMCO_Id = "";
            $scope.AMB_Id = "";
            $scope.AMSE_Id = "";
            $scope.AMCST_Id = "";
            $scope.FCSWO_Date = null; 
            $scope.filterdata1 = 'NonRefunable';
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

        $scope.editdata = function (employee) {

            $scope.editEmployee = employee.fcswO_Id;
            var pageid = $scope.editEmployee;
            apiService.getURI("CLGFeeWaivedOff/EditDetails", pageid).
                then(function (promise) {

                    $scope.FCSWO_Id = promise.fcswO_Id;
                    $scope.cfg.ASMAY_Id = promise.asmaY_Id;
                    $scope.get_courses();
                    $scope.AMCST_Id = promise.amcsT_Id;
                    $scope.AMCO_Id = promise.amcO_Id;
                    $scope.get_branches();
                    $scope.AMB_Id = promise.amB_Id;
                    $scope.get_semisters();
                    $scope.AMSE_Id = promise.amsE_Id;
                    $scope.get_students();
                    //$scope.FSWO_Date = promise.fswO_Date;
                    $scope.FCSWO_Date = new Date(promise.fcswO_Date);
                    //  $scope.user.fswO_WaivedOffAmount = promise.fswO_WaivedOffAmount;
                    $scope.asmayiddisable = true;
                    $scope.group_list = promise.grouplist;
                    $scope.group_list[0].selected = true;
                    $scope.head_list = promise.headlist;
                    $scope.head_list[0].xyz = true;
                    $scope.filterdata1 = promise.filterrefund;
                })
        }

        $scope.DeletRecord = function (employee, SweetAlert) {
            $scope.editEmployee = employee.fcswO_Id;
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
                        apiService.DeleteURI("CLGFeeWaivedOff/DeletRecord", feechequebounceid).
                            then(function (promise) {
                                if (promise.returnduplicatestatus == "success") {
                                    swal('Record Deleted Successfully');
                                    $state.reload();
                                    $scope.BindData();
                                }
                                else if (promise.returnduplicatestatus === "adjusted") {
                                    swal('Record already adjusted/Refunded');
                                    $state.reload();
                                    $scope.BindData();
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
