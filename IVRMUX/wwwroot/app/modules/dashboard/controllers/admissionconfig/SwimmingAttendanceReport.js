(function () {
    'use strict';
    angular.module('app').controller('SwimmingAttendancecontroller', SwimmingAttendancecontroller);
    SwimmingAttendancecontroller.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', '$filter', 'superCache', '$stateParams'];

    function SwimmingAttendancecontroller($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, $filter, superCache, $stateParams) {
        $scope.chckedIndexs = [];
        $scope.unchckedIndexs = [];
        $scope.albumNameArraycolumn = [];
        $scope.columnsTest = [];

        $scope.SaveDis = false;
        $scope.IsHiddenup = true;
        $scope.IsHiddendown = false;

        $scope.currentPage = 1;
        //$scope.itemsPerPage = 10;

        // Load Initial Datas
        
        $scope.attcount = false;
        $scope.attcount1 = false;
        $scope.batchno = true;

        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        for (var i = 0; i < privlgs.length; i++) {
            if (privlgs[i].pageId == pageid) {
                $scope.userPrivileges = privlgs[i];
            }
        }

        //disable the sundays in calender
        $scope.onlyWeekendsPredicate = function (date) {
            var day = date.getDay();
            return day === 1 || day === 2 || day === 3 || day === 4 || day === 5 || day === 6;

        };

        $scope.att = {};

        $scope.AttendenceCheckDis = true;    

        $scope.loaddata = function () {
            var pageid = 1;
            apiService.getURI("SwimmingAttendance/loaddata", pageid).then(function (promise) {

                if (promise !== null) {
                    $scope.academicYearList = promise.getyear;
                } else {
                    swal("Something Went Wrong Contact Administrator");
                }
            });
        };

        $scope.onchnageyear = function () {
            var data = {
                "ASMAY_Id": $scope.att.asmaY_Id
            };
            apiService.create("SwimmingAttendance/onchnageyear", data).then(function (promise) {
                if (promise !== null) {
                    $scope.classList = promise.getclass;

                    angular.forEach($scope.academicYearList, function (y) {

                        if (y.asmaY_Id === parseInt($scope.att.asmaY_Id)) {
                            var dataf, datat;
                            dataf = new Date(y.asmaY_From_Date);
                            datat = new Date(y.asmaY_To_Date);

                            $scope.fDate = dataf;
                            $scope.tDate = datat;
                            $scope.dd = new Date();

                            $scope.minDatedof = new Date(
                                $scope.fDate.getFullYear(),
                                $scope.fDate.getMonth(),
                                $scope.fDate.getDate());

                            $scope.maxDatedof = new Date(
                                $scope.dd.getFullYear(),
                                $scope.dd.getMonth(),
                                $scope.dd.getDate());
                        }
                    });
                } else {
                    swal("Something Went Wrong Contact Administrator");
                }
            });
        };

        $scope.onchangeclass = function () {
            var data = {
                "ASMAY_Id": $scope.att.asmaY_Id,
                "ASMCL_Id": $scope.att.asmcL_Id
            };
            apiService.create("SwimmingAttendance/onchangeclass", data).then(function (promise) {
                if (promise !== null) {
                    $scope.sectionList = promise.getsection;
                } else {
                    swal("Something Went Wrong Contact Administrator");
                }
            });
        };

        $scope.search = function (att) {

            if ($scope.myForm.$valid) {

                var data = {
                    "ASMAY_Id": att.asmaY_Id,
                    "ASMCL_Id": att.asmcL_Id,
                    "ASMS_Id": att.asmS_Id,
                    "ASSC_AttendanceDate": new Date(att.ASSC_AttendanceDate).toDateString(),
                    "ASSC_EntryForFlg": 'Swimming'
                };

                apiService.create("SwimmingAttendance/search", data).then(function (promise) {

                    if (promise !== null) {

                        $scope.configuration = promise.admissionstandarad;

                        if ($scope.configuration.length > 0) {

                            if ($scope.configuration[0].asC_Att_Default_OrderFlag === 1) {
                                $scope.sortKey = "studentname";
                            }
                            else if ($scope.configuration[0].asC_Att_Default_OrderFlag === 2) {
                                $scope.sortKey = "studentname";
                            }
                            else if ($scope.configuration[0].asC_Att_Default_OrderFlag === 3) {
                                $scope.sortKey = "amaY_RollNo";
                            }
                            else if ($scope.configuration[0].asC_Att_Default_OrderFlag === 4) {
                                $scope.sortKey = "studentname";
                            }
                            else if ($scope.configuration[0].asC_Att_Default_OrderFlag === 5) {
                                $scope.sortKey = "studentname";
                            }
                            else if ($scope.configuration[0].asC_Att_Default_OrderFlag === 6) {
                                $scope.sortKey = "amsT_RegistrationNo";
                            }
                            else if ($scope.configuration[0].asC_Att_Default_OrderFlag === 7) {
                                $scope.sortKey = "amsT_AdmNo";
                            }
                            else {
                                $scope.sortKey = "studentname";
                            }

                        } else {
                            $scope.sortKey = "studentname";
                        }


                        var count = 0;
                        if (promise.getstandarad.length > 0) {
                            if (promise.getstandarad[0].ivrmgC_AdmnoColumnDisplay === 1) {
                                $scope.showadmno = true;
                                count = count + 1;
                            } else {
                                $scope.showadmno = false;
                            }


                            if (promise.getstandarad[0].ivrmgC_RegnoColumnDisplay === 1) {
                                $scope.showregno = true;
                                count = count + 1;
                            } else {
                                $scope.showregno = false;
                            }

                            if (promise.getstandarad[0].ivrmgC_RollnoColumnDisplay === 1) {
                                $scope.showrollno = true;
                                count = count + 1;
                            } else {
                                $scope.showrollno = false;
                            }
                        }
                        if (count === 0) {
                            $scope.showadmno = true;
                            $scope.showrollno = true;
                        }

                        $scope.getsavedsstudent_test = promise.getsavedsstudent;
                        if ($scope.getsavedsstudent_test !== null && $scope.getsavedsstudent_test.length > 0) {
                            $scope.studentgrid = true;
                            $scope.getsavedsstudent = $scope.getsavedsstudent_test;
                            $scope.studentlist = promise.getstudent;
                            angular.forEach($scope.studentlist, function (s) {
                                angular.forEach($scope.getsavedsstudent, function (d) {
                                    if (s.AMST_Id === d.AMST_Id) {
                                        s.ALSSC_AttendanceCount = d.ALSSC_AttendanceCount;
                                        s.ALSSC_Id = d.ALSSC_Id;
                                    }
                                });
                            });
                        } else {
                            $scope.studentlist = promise.getstudent;
                            $scope.studentgrid = true;
                            angular.forEach($scope.studentlist, function (s) {
                                s.ALSSC_AttendanceCount = "1.00";
                            });
                        }

                        console.log($scope.studentlist);
                        console.log($scope.getsavedsstudent);
                    }
                });
            } else {
                $scope.submitted = true;
            }
        };
       

        $scope.save = function (att) {
            if ($scope.newform.$valid) {
                var data = {
                    "ASMAY_Id": att.asmaY_Id,
                    "ASMCL_Id": att.asmcL_Id,
                    "ASMS_Id": att.asmS_Id,
                    "ASSC_AttendanceDate": new Date(att.ASSC_AttendanceDate).toDateString(),
                    "ASSC_EntryForFlg": 'Swimming',
                    "Tempstudent": $scope.studentlist
                };
                apiService.create("SwimmingAttendance/save", data).then(function (promise) {
                    if (promise.returnval === true) {
                        swal("Record Saved / Updated Successfully");
                    } else {
                        swal("Failed To Save /Update Record");
                    }
                    $state.reload();
                });


            } else {
                $scope.submitted1 = true;
            }          

        };

        $scope.interacted1 = function (field) {
            return $scope.submitted1;
        };


        $scope.isOptionsRequired = function () {
            return !$scope.studentlist.some(function (options) {
                return options.Selected;
            });
        };

        $scope.submitted = false;     
        $scope.submitted1 = false;     

        var savecount = 0;
        var updatecount = 0;
        var deletecount = 0;

        $scope.sortBy = function (key) {
            $scope.sortReverse = ($scope.sortKey === key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };

        $scope.interacted = function (field) {
            return $scope.submitted;
        };

        //search
        $scope.searchValue = '';
        $scope.filterValue = function (obj) {
            return JSON.stringify(obj.amaY_RollNo).indexOf($scope.searchValue) >= 0 ||
                angular.lowercase(obj.studentname).indexOf(angular.lowercase($scope.searchValue)) >= 0 ||
                (obj.amsT_RegistrationNo).indexOf($scope.searchValue) >= 0 ||
                (obj.amsT_AdmNo).indexOf($scope.searchValue) >= 0;
        };

        //clear data
        $scope.clearData = function () {
            $state.reload();
        };    

        $scope.batchno1 = 0;

        $scope.Deleteattendance = function (objas, SweetAlert) {

            var mgs = "";
            var confirmmgs = "";
            var todate = "";
            var fromdate = "";
            var ASA_Class_Attended;
            var asasB_Id;

            swal({
                title: "Are you sure",
                text: "Do You Want To Delete The Selected Attendace Details ?",
                type: "warning",
                showCancelButton: true,
                confirmButtonColor: "#DD6B55", confirmButtonText: "Yes, Delete it!",
                cancelButtonText: "Cancel",
                closeOnConfirm: false,
                closeOnCancel: false
            },
                function (isConfirm) {
                    if (isConfirm) {

                        var ASA_ClassHeld;
                        if ($scope.att.entry === 'Dailyonce') {

                            angular.forEach($scope.studentlist, function (user) {
                                if (user.pdays !== 0) {
                                    user.pdays = 0;
                                }
                                //$scope.studentlist.push(user);
                            });

                            fromdate = new Date($scope.att.ASA_Entry_DateTime).toDateString();
                            todate = new Date($scope.att.ASA_Entry_DateTime).toDateString();
                            ASA_ClassHeld = '1.00';
                            ASA_Class_Attended = '1.0';
                            asasB_Id = 0;
                        }

                        if ($scope.att.entry === 'Dailytwice') {

                            angular.forEach($scope.studentlist, function (user) {
                                if (user.pdays !== 0) {
                                    user.pdays = 0;
                                }
                                //$scope.studentlist.push(user);
                            });


                            if ($scope.att.ASA_ClassHeld1 === true || $scope.att.ASA_ClassHeld2 === true) {
                                ASA_ClassHeld = '0.5';
                            }
                            if ($scope.att.ASA_ClassHeld1 === true && $scope.att.ASA_ClassHeld2 === true) {
                                ASA_ClassHeld = '1.00';
                            }
                            if ($scope.att.ASA_ClassHeld1 === false && $scope.att.ASA_ClassHeld2 === false) {
                                swal("Kindy Select 1st Half/2nd Half Or both");
                                return;
                            }

                            fromdate = new Date($scope.att.ASA_Entry_DateTime).toDateString();
                            todate = new Date($scope.att.ASA_Entry_DateTime).toDateString();
                            ASA_ClassHeld = '1.00';
                            asasB_Id = 0;
                        }

                        if ($scope.att.entry === 'monthly') {
                            //var countclass = parseInt($scope.countclass);
                            var countclass = ($scope.countclass);
                            ASA_ClassHeld = countclass;
                            ASA_Class_Attended = '0.0';
                            fromdate = new Date($scope.att.ASA_FromDate).toDateString();
                            todate = new Date($scope.att.ASA_ToDate).toDateString();
                            asasB_Id = 0;
                        }
                        if ($scope.att.entry === 'period') {

                            angular.forEach($scope.studentlist, function (user) {
                                if (user.pdays !== 0) {
                                    user.pdays = 0;
                                }
                                //$scope.studentlist.push(user);
                            });
                            fromdate = new Date($scope.att.ASA_Entry_DateTime).toDateString();
                            todate = new Date($scope.att.ASA_Entry_DateTime).toDateString();
                            ASA_ClassHeld = '1.00';
                            if ($scope.att.asasB_Id === "") {
                                asasB_Id = 0;
                            } else {
                                asasB_Id = $scope.att.asasB_Id;
                            }
                        }

                        //var entrydate = new Date($scope.att.ASA_Entry_DateTime).toDateString();
                        var entrydate = new Date().toDateString();

                        var data = {
                            "ASA_Id": $scope.att.asA_Id,
                            "ismS_Id": $scope.att.ismS_Id,
                            "ASMAY_Id": $scope.att.asmaY_Id,
                            "ASA_Att_Type": $scope.att.entry,
                            "ASMCL_Id": $scope.att.asmcL_Id,
                            "ASMS_Id": $scope.att.asmC_Id,
                            "TTMP_Id": $scope.att.ttmP_Id,
                            "ASA_Entry_DateTime": entrydate,
                            "ASA_FromDate": fromdate,
                            "ASA_ToDate": todate,
                            "ASA_ClassHeld": ASA_ClassHeld,
                            "ASA_Regular_Extra": $scope.att.ASA_Regular_Extra,
                            "asasB_Id": asasB_Id,
                            stdList: $scope.studentlist
                        };

                        apiService.create("StudentAttendanceEntry/Deleteattendance", data).
                            then(function (promise) {

                                if (promise.returnval === true) {
                                    swal("Record Delected Successfully");
                                } else {
                                    swal("Failed To Delect Record");
                                }
                            });
                    }
                    else {
                        swal("Record Delection Cancelled");
                    }
                    $state.reload();
                });
        };
    }
})();
