(function () {
    'use strict';
    angular
        .module('app')
        .controller('CLGStudentFeeEnablePartialPaymentController', CLGStudentFeeEnablePartialPaymentController)
    CLGStudentFeeEnablePartialPaymentController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', '$filter', 'superCache']
    function CLGStudentFeeEnablePartialPaymentController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, $filter, superCache) {

        $scope.FSEPPC_RemarksDate = new Date();
        $scope.obj = {};

        $scope.onLoadGetData = function () {
            var pageid = 2;

            apiService.getURI("CLGStudentFeeEnablePartialPayment/GetYearList", pageid).then(function (promise) {
                $scope.yearlist = promise.yearlist;
                $scope.sectionlist = promise.sectionlist;
                $scope.alldata = promise.alldata;
            })
        };
        $scope.submitted = false;
        //===========Load ALL Courses data in to the CheckboxList===============//
        $scope.get_courses = function () {
            $scope.msg = '';

            $scope.amcO_Id = "";
            $scope.courselist = [];
            $scope.amB_Id = ''
            $scope.branchlist = [];
            $scope.semesterlist = [];
            $scope.amsE_Id = '';
            $scope.show_flag = false;
            if ($scope.asmaY_Id != undefined && $scope.asmaY_Id != "") {
                var data = {
                    "ASMAY_Id": $scope.asmaY_Id
                }
                apiService.create("CLGStudentFeeEnablePartialPayment/get_courses", data).then(function (promise) {
                    $scope.courselist = promise.courselist;

                    if (promise.fillmastergroup.length > 0) {
                        $scope.grouplst = promise.fillmastergroup;
                        $scope.showgroup = true;
                    }
                    else {
                        $scope.showgroup = false;
                    }


                    $scope.headlst = promise.fillmasterhead;
                    if ($scope.grouplst.length == 0) {
                        $scope.msg = "No Groups"
                    }                  
                    if ($scope.courselist.length == 0 || $scope.courselist == null) {
                        swal('For Selected Year Courses Are Not Available!!!');

                    }
                })
            }
            else {
                $scope.courselist = [];
                $scope.amcO_Id = "";
            }
            $scope.show_btn = false;
            $scope.show_cancel = false;

            $scope.show_grid = false;
        };
        //====================End===================//
        $scope.interacted = function (field) {
            return $scope.submitted;
        };


        //================Load Branches data in to the CheckboxList=====================//
        $scope.get_branches = function () {

            $scope.amB_Id = ''
            $scope.branchlist = [];
            $scope.semesterlist = [];
            $scope.amsE_Id = '';
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "AMCO_Id": $scope.amcO_Id
            }
            apiService.create("CLGStudentFeeEnablePartialPayment/get_branches", data).then(function (promise) {
                $scope.branchlist = promise.branchlist;             
                console.log($scope.branchlist)
                if ($scope.branchlist.length == 0 || $scope.branchlist == null) {
                    swal('For Selected Course Branches Are Not Available!!!');
                    $scope.show_btn = false;
                    $scope.show_cancel = false;
                    $scope.show_grid = false;
                }
            })
        }
        //========================END==================================//

        //============Load Semester data in to the CheckboxList==================//
        $scope.get_semisters = function () {

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "AMCO_Id": $scope.amcO_Id,
                "AMB_Id": $scope.amB_Id
            }
            apiService.create("CLGStudentFeeEnablePartialPayment/get_semisters", data).
                then(function (promise) {
                    $scope.semesterlist = promise.semesterlist;
                    // $scope.amsE_Id = "";
                    if ($scope.semesterlist.length == 0 || $scope.semesterlist == null) {
                        swal('For Selected Branch Semesters Are Not Available!!!');
                        $scope.show_btn = false;
                        $scope.show_cancel = false;
                        $scope.show_grid = false;
                    }
                })
        };
        //==========================END============================//
        //==================student============//
        $scope.get_student = function () {
            debugger;
            $scope.studentlist = [];
            $scope.submitted = false;
            if ($scope.myForm.$valid) {

                if ($scope.acmS_Id == 'ALL') {
                    var data = {
                        "ASMAY_Id": $scope.asmaY_Id,
                        "AMCO_Id": $scope.amcO_Id,
                        "AMB_Id": $scope.amB_Id,
                        "AMSE_Id": $scope.amsE_Id,
                      
                    }
                }
                else {
                    var data = {
                        "ASMAY_Id": $scope.asmaY_Id,
                        "AMCO_Id": $scope.amcO_Id,
                        "AMB_Id": $scope.amB_Id,
                        "AMSE_Id": $scope.amsE_Id,
                        "ACMS_Id": $scope.acmS_Id,
                       
                    }
                }

                apiService.create("CLGStudentFeeEnablePartialPayment/get_student", data).
                    then(function (promise) {
                        $scope.studentlist = promise.studentlist;                     
                        if ($scope.studentlist.length == 0 || $scope.studentlist == null) {
                            swal('No Student!!!');
                        }
                    })
            }
            else {
                $scope.submitted = true;
            }
        }
        //=================end===================/////////////////
        // clear form data
        $scope.cancel = function () {
            $scope.search = "";
            $state.reload();
        };

        $scope.saveData = function () {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                var data = {
                    "FSEPPC_Id": $scope.FSEPPC_Id,
                    "ASMAY_Id": $scope.asmaY_Id,
                    "AMCST_Id": $scope.obj.amcsT_Id.amcsT_Id,                    
                    "FSEPPC_RemarksDate": new Date($scope.FSEPPC_RemarksDate).toDateString(),
                    "FSEPPC_Remarks": $scope.FSEPPC_Remarks
                }
                apiService.create("CLGStudentFeeEnablePartialPayment/savedata", data).then(function (promise) {
                    if (promise.retrunMsg !== "")
                    {
                        if (promise.retrunMsg == "Add") {
                            swal("Record Saved Successfully..");
                            $state.reload();
                         }
                           else if (promise.retrunMsg == "false") {
                               swal("Record Not saved / Updated..", 'Fail');

                           }
                    }                   
                    else {
                        swal("Something Went Wrong Please Contact Administrator");
                    }
                    $scope.clear();                   
                })

            }
            else {
                $scope.submitted = true;
            }
        };
        $scope.DeletRecord = function (data, SweetAlert) {
            var mgs = "";
            var confirmmgs = "";
            if (data.fseppC_ActiveFlag == false) {
                mgs = "Activate";
                confirmmgs = "Activated";
            }
            else {
                mgs = "Deactivate";
                confirmmgs = "Deactivated";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to " + mgs + " Record..!?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, " + mgs + " it!",
                cancelButtonText: "Cancel..!",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {
                        apiService.create("CLGStudentFeeEnablePartialPayment/deactivate", data).then(function (promise) {
                            if (promise.retrunMsg !== "") {
                                if (promise.retrunMsg === "Activated") {
                                    swal("Record Activated successfully");
                                    $state.reload();
                                }
                                else if (promise.retrunMsg === "Deactivated") {
                                    swal("Record Deactivated successfully");
                                    $state.reload();
                                }
                                else {
                                    swal("Record Not Activated/Deactivated", 'Fail');
                                }
                            }

                        })
                    }
                    else {
                        swal(" Cancelled", "Ok");
                    }
                }

            );
        }

    }
})();