(function () {
    'use strict';

    angular
        .module('app')
        .controller('LateInStudentController', LateInStudentController);

    LateInStudentController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$stateParams', 'Excel', '$timeout', 'superCache', '$filter'];

    function LateInStudentController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $stateParams, Excel, $timeout, superCache, $filter) {

        $scope.searchValue = "";

        $scope.loaddata = function () {
            apiService.getURI("LateInStudent/loaddata/", 5).then(function (promise) {
                $scope.academicYear = promise.academicYear;

                $scope.getstudentlist = promise.getstudentlist;

            });
        }

        $scope.cancel = function () {

            $state.reload();
        }
        $scope.interacted = function (field) {
            return $scope.submitted;
        };
        $scope.currentPage = 1;
        $scope.itemsPerPage = 10;



        //==================================Change Asmay_Id
        $scope.get_class = function () {

            
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,

            }
            apiService.create("LateInStudent/get_class", data).then(function (promise) {

                $scope.classList = promise.classList;
            })
        }
        //==================================end 

        //==================================Change 
        $scope.get_section = function () {

           
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,

            }
            apiService.create("LateInStudent/get_section", data).then(function (promise) {

                $scope.sectionList = promise.sectionList;
            })
        }
        //==================================end  


        //==================================Change 
        $scope.get_student = function () {

            
            var data = {
                "ASMAY_Id": $scope.ASMAY_Id,
                "ASMCL_Id": $scope.ASMCL_Id,
                "ASMS_Id": $scope.ASMS_Id,

            }
            apiService.create("LateInStudent/get_student", data).then(function (promise) {

                $scope.studentlist = promise.studentlist;
                console.log($scope.studentlist);
            })
        }
        //==================================end

        $scope.toggleAll = function () {
            var toggleStatus = $scope.checkall;
            angular.forEach($scope.yearEndReport, function (itm) {
                itm.checked = toggleStatus;
                if ($scope.checkall == true) {
                    $scope.printdatatable.push(itm);
                }
                else {
                    $scope.printdatatable.splice(itm);
                }
            });
        }
        $scope.optionToggled = function (SelectedStudentRecord, index) {

            $scope.checkall = $scope.yearEndReport.every(function (itm) { return itm.checked; });
            if ($scope.printdatatable.indexOf(SelectedStudentRecord) === -1) {
                $scope.printdatatable.push(SelectedStudentRecord);
            }
            else {
                $scope.printdatatable.splice($scope.printdatatable.indexOf(SelectedStudentRecord), 1);
            }


        }






        //////////=========================================================For House
        $scope.searchchkbx = "";
        $scope.all_check = function () {
            var checkStatus = $scope.usercheck;
            angular.forEach($scope.studentlist, function (itm) {
                itm.stud = checkStatus;
            });
        }

        $scope.togchkbx = function () {
            $scope.usercheck = $scope.studentlist.every(function (options) {
                return options.stud;
            });
        }

        $scope.isOptionsRequired = function () {
            return !$scope.studentlist.some(function (options) {
                return options.stud;
            });
        }

        $scope.filterchkbx = function (obj) {
            return (angular.lowercase(obj.studentName)).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        }

        //=============================================================== For Section

        $scope.searchchkbx23 = "";
        $scope.all_check23 = function () {
            var checkStatus = $scope.usercheck23;
            angular.forEach($scope.sectionList, function (itm) {
                itm.select = checkStatus;
            });
        }

        $scope.togchkbx23 = function () {
            $scope.usercheck23 = $scope.sectionList.every(function (options) {
                return options.select;
            });
        }

        $scope.isOptionsRequired23 = function () {
            return !$scope.sectionList.some(function (options) {
                return options.select;
            });
        }

        $scope.filterchkbx23 = function (obj) {
            return (angular.lowercase(obj.sectionName)).indexOf(angular.lowercase($scope.searchchkbx23)) >= 0;
        }

        $scope.saverecord = function () {
            
            $scope.studetdata = [];
            $scope.studetdatalist = [];
            if ($scope.myForm.$valid) {

                $scope.PunchTime = $filter('date')($scope.ALIEOS_PunchTime, "HH:mm");

                var AttendanceDate = $scope.ALIEOS_AttendanceDate == null ? "" : $filter('date')($scope.ALIEOS_AttendanceDate, "yyyy-MM-dd");
                var PunchDate = $scope.ALIEOS_PunchDate == null ? "" : $filter('date')($scope.ALIEOS_PunchDate, "yyyy-MM-dd");

                angular.forEach($scope.studentlist, function (cls) {
                    if (cls.stud == true) {
                        $scope.studetdata.push(cls);
                    }
                });
                if ($scope.studetdata.length > 0) {


                    var data = {
                        "ALIEOS_Id": $scope.ALIEOS_Id,
                        "ASMAY_Id": $scope.ASMAY_Id,
                        "ASMCL_Id": $scope.ASMCL_Id,
                        "ASMS_Id": $scope.ASMS_Id,
                        "ALIEOS_AttendanceDate": AttendanceDate,
                        "ALIEOS_PunchDate": PunchDate,
                        "ALIEOS_PunchTime": $scope.PunchTime,
                        "ALIEOS_Reason": $scope.ALIEOS_Reason,
                        studetdatalist: $scope.studetdata,

                    }
                    apiService.create("LateInStudent/savedata", data).
                        then(function (promise) {

                            if (promise.returnval != null) {

                                if (promise.returnval == true) {
                                    if ($scope.ALIEOS_Id > 0) {
                                        swal('Record Updated Successfully!!!');
                                    }
                                    else {
                                        swal('Record Saved Successfully!!!');
                                    }

                                }
                                else {
                                    if (promise.returnval == false) {
                                        if ($scope.ALIEOS_Id > 0) {
                                            swal('Record Not Update Successfully!!!');
                                        }
                                        else {
                                            swal('Record Not Saved Successfully!!!');
                                        }
                                    }
                                }

                                $state.reload();
                            }
                            else {
                                swal("Kindly Contact Administrator!!!");
                            }

                        })

                }
                else {
                    swal('Select Student!');
                }
            }
            else {
                $scope.submitted = true;
            }

        };


        //==============================================Active And Deactivate Row data
        $scope.deactivate = function (newuser1, SweetAlertt) {
         
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

            var mgs = "";
            if (newuser1.ALIEOS_AactiveFlag == false) {

                mgs = "Activate";

            } else {
                mgs = "Deactivate";
            }
            swal({
                title: "Are you sure?",
                text: "Do you want to  " + mgs + " Record?",
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
                        var data = {
                            "ALIEOS_Id":newuser1.ALIEOS_Id
                        }
                        apiService.create("LateInStudent/deactive", data).then(function (promise) {

                                if (promise.returnval == true) {
                                    swal("Record" + mgs + "d Successfully!!");

                                }
                                else {
                                    swal("Record Not " + mgs + "d Successfully!!!");
                                }
                                $state.reload();

                            })
                    } else {
                        swal("Record " + mgs + " Cancelled");
                    }
                })
        }



        $scope.edit = function (data) {
           
            $scope.student_falg = true;
            apiService.create("LateInStudent/editdata", data).
                then(function (promise) {
                  
                    $scope.editlist = promise.editlist;
                    $scope.ALIEOS_Id = promise.editlist[0].alieoS_Id;
                    $scope.ALIEOS_AttendanceDate = new Date(promise.editlist[0].alieoS_AttendanceDate);
                    $scope.ALIEOS_PunchDate = new Date(promise.editlist[0].alieoS_PunchDate);
                    $scope.ALIEOS_PunchTime = moment(promise.editlist[0].alieoS_PunchTime, 'h:mm a').format();
                    $scope.ALIEOS_Reason = promise.editlist[0].alieoS_Reason;

                    $scope.ASMAY_Id = promise.editlist[0].asmaY_Id;

                    $scope.classList = promise.classList;
                    $scope.sectionList = promise.sectionList;
                    $scope.ASMCL_Id = promise.editlist[0].asmcL_Id;

                    $scope.ASMS_Id = promise.editlist[0].asmS_Id;

                    $scope.studentlist = promise.studentlist;
                    $scope.amsT_Id = promise.editlist[0].amsT_Id;

                    $scope.studentlist = promise.studentlist;

                    angular.forEach($scope.studentlist, function (tt) {
                        if (tt.amsT_Id == promise.editlist[0].amsT_Id) {
                            tt.stud = true;
                        }
                    })


                });


        }



    }
})();
