
(function () {
    'use strict';
    angular
        .module('app')
        .controller('HostelAllotForStudent', HostelAllotForStudent)
    HostelAllotForStudent.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$q', '$stateParams', '$filter']
    function HostelAllotForStudent($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $q, $stateParams, $filter) {


        $scope.sortKey = 'HLHSALT_Id';
        $scope.sortReverse = true;
        $scope.submitted = false;
        $scope.HLHSALT_AllotmentDate = new Date();

        $scope.currentPage2 = 1;
        $scope.itemsPerPage2 = 10;
        $scope.search2 = "";

        $scope.showflag = false;
        $scope.showflag_stud = true;

        $scope.loadData = function () {

            var id = 2;

            apiService.getURI("HostelAllotForStudent/loaddata", id).
                then(function (promise) {

                    $scope.academicYear = promise.yearlist;
                    $scope.ASMAY_Id = promise.asmaY_Id;
                    $scope.hostel_list = promise.hostel_list;
                    $scope.student_allotlist = promise.student_allotlist;

                    $scope.classlist = promise.classlist;
                    $scope.sectionlist = promise.sectionlist;

                    $scope.roomcatgry_list = promise.roomcatgry_list;
                    $scope.room_list = promise.room_list;

                })
        }



        //============================== Approve
        $scope.savedata = function () {

            var AllotmentDate = $scope.HLHSALT_AllotmentDate == null ? "" : $filter('date')($scope.HLHSALT_AllotmentDate, "yyyy-MM-dd");
            var VacatedDate = $scope.HLHSALT_VacatedDate == null ? "" : $filter('date')($scope.HLHSALT_VacatedDate, "yyyy-MM-dd");

            if ($scope.myForm.$valid) {
                var data = {
                    "HLHSALT_Id": $scope.HLHSALT_Id,
                    "HLHSALT_AllotmentDate": AllotmentDate,
                    "ASMAY_Id": $scope.ASMAY_Id,
                    "HLMH_Id": $scope.HLMH_Id,
                    "HLMRCA_Id": $scope.HLMRCA_Id,
                    "AMST_Id": $scope.AMST_Id,
                    "ASMCL_Id": $scope.asmcL_Id,
                    "ASMS_Id": $scope.asmS_Id,
                    "HRMRM_Id": $scope.HRMRM_Id,
                    "HLHSALT_NoOfBeds": $scope.HRMRM_BedCapacity,
                    "HLHSALT_AllotRemarks": $scope.HLHSALT_AllotRemarks,
                    "HLHSALT_VacateFlg": $scope.HLHSALT_VacateFlg,
                    "HLHSALT_VacatedDate": VacatedDate,
                    "HLHSALT_VacateRemarks": $scope.HLHSALT_VacateRemarks,
                }
                apiService.create("HostelAllotForStudent/savedata", data)
                    .then(function (promise) {
                        if (promise.duplicate == false) {
                            if (promise.returnval == true) {

                                if ($scope.HLHSALT_Id > 0) {
                                    swal('Record Updated Successfully!!!');
                                }
                                else {
                                    swal('Record Saved Successfully!!!');
                                }
                            }
                            else {
                                if (promise.returnval == false) {
                                    if ($scope.HLHSALT_Id > 0) {
                                        swal('Record Not Update Successfully!!!');
                                    }
                                    else {
                                        swal('Record Not Saved Successfully!!!');
                                    }
                                }
                            }
                        }
                        else {
                            swal('Record Already Exist!');
                        }

                        $state.reload();
                    });
            }
            else {
                $scope.submitted = true;
            }
        }
        //=======================================End



        //============= Get Student Information
        $scope.get_studInfo = function () {
            $scope.housewise_studentList = [];
            var data = {
                "HLMH_Id": $scope.HLMH_Id,
            }
            apiService.create("HostelAllotForStudent/get_studInfo", data)
                .then(function (promise) {
                    if (promise.housewise_studentList.length > 0) {
                        $scope.housewise_studentList = promise.housewise_studentList;
                    }
                    else {
                        swal('Record Not Available!');
                        $scope.housewise_studentList = [];
                    }
                });
        }
        //===============================End


        $scope.Clearid = function () {
            $state.reload();
        }


        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        $scope.sort = function (keyname) {
            $scope.sortKey = keyname;   //set the sortKey to the param passed
            $scope.reverse = !$scope.reverse; //if true make it false and vice versa
        }

        //======================================
        $scope.get_studetdetails = function (studdata) {
            debugger;
            $scope.asmcL_Id = studdata[0].ASMCL_Id;
            $scope.asmS_Id = studdata[0].ASMS_Id;
            $scope.HLMRCA_Id = studdata[0].HLMRCA_Id;

            if (studdata[0].HLHSREQC_VegMessFlg == true) {
                $scope.HLHSREQC_VegMessFlg = 1;
            }
            if (studdata[0].HLHSREQC_NonVegMessFlg == true) {
                $scope.HLHSREQC_NonVegMessFlg = 1;
            }
            if (studdata[0].HLHSREQC_ACRoomFlg == true) {
                $scope.HLHSREQC_ACRoomFlg = 1;
            }
            if (studdata[0].HLHSREQC_SingleRoomFlg == true) {
                $scope.HLHSREQC_SingleRoomFlg = 1;
            }
        }


        $scope.get_roomdetails = function () {

            var data = {
                "HRMRM_Id": $scope.HRMRM_Id,
            }
            apiService.create("HostelAllotForStudent/get_roomdetails", data).then(function (promise) {

                $scope.HRMRM_BedCapacity = promise.hrmrM_BedCapacity;

            })
        }

        //======================================Edit 
        $scope.editstudetLV = function (user) {

            var data = {
                "HLHSALT_Id": user.HLHSALT_Id,
                "ASMAY_Id": user.ASMAY_Id,
                "HLMH_Id": user.HLMH_Id,
            }
            apiService.create("HostelAllotForStudent/editdata", data).then(function (promise) {

                $scope.editlist = promise.editlist;

                $scope.HLHSALT_Id = promise.editlist[0].hlhsalT_Id;
                $scope.ASMAY_Id = promise.editlist[0].asmaY_Id;
                $scope.HLHSALT_AllotmentDate = new Date(promise.editlist[0].hlhsalT_AllotmentDate);
                $scope.HLMH_Id = promise.editlist[0].hlmH_Id;
                $scope.AMST_Id = promise.editlist[0].amsT_Id;
                $scope.asmcL_Id = promise.editlist[0].asmcL_Id;
                $scope.asmS_Id = promise.editlist[0].asmS_Id;
                $scope.HLMRCA_Id = promise.editlist[0].hlmrcA_Id;
                $scope.HRMRM_Id = promise.editlist[0].hrmrM_Id;
           
                if ($scope.HRMRM_Id != undefined && $scope.HRMRM_Id != null && $scope.HRMRM_Id != '') {
                    $scope.get_roomdetails();
                }

                $scope.HRMRM_BedCapacity = promise.hrmrM_BedCapacity;

                $scope.HLHSALT_AllotRemarks = promise.editlist[0].hlhsalT_AllotRemarks;

                if (promise.editlist[0].hlhsalT_VacateFlg == true) {

                    $scope.hlhsalT_VacateFlg = 1;
                    $scope.HLHSALT_VacatedDate = new Date(promise.editlist[0].hlhsalT_VacatedDate);
                    $scope.HLHSALT_VacateRemarks = promise.editlist[0].hlhsalT_VacateRemarks;

                }
                if ($scope.HLMH_Id != undefined && $scope.HLMH_Id != null && $scope.HLMH_Id != '') {
                    //$scope.get_studInfo();
                      $scope.housewise_studentList = promise.housewise_studentList;
                    if (promise.housewise_studentList[0].HLHSREQC_VegMessFlg == true) {
                        $scope.HLHSREQC_VegMessFlg = 1;
                    }
                    if (promise.housewise_studentList[0].HLHSREQC_NonVegMessFlg == true) {
                        $scope.HLHSREQC_NonVegMessFlg = 1;
                    }
                    if (promise.housewise_studentList[0].HLHSREQC_ACRoomFlg == true) {
                        $scope.HLHSREQC_ACRoomFlg = 1;
                    }
                    if (promise.housewise_studentList[0].HLHSREQC_SingleRoomFlg == true) {
                        $scope.HLHSREQC_SingleRoomFlg = 1;
                    }

                    $scope.AMST_Id = promise.housewise_studentList[0].AMST_Id;
                }                
              
               

                


            });


        }
        //=======================================End


    }
})();