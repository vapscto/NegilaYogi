(function () {
    'use strict';
    angular.module('app').controller('CollegeAttendanceEntryNewController', CollegeAttendanceEntryNewController)
    CollegeAttendanceEntryNewController.$inject = ['$rootScope', '$scope', '$state', '$location', 'Flash', 'appSettings', 'apiService', '$http', 'superCache', '$stateParams', '$filter']
    function CollegeAttendanceEntryNewController($rootScope, $scope, $state, $location, Flash, appSettings, apiService, $http, superCache, $stateParams, $filter) {
        $scope.obj = {};

        $scope.searc_button = true;
        $scope.sortReverse = true;
        $scope.searchValue = "";
        $scope.itemsPerPage = 10;
        $scope.currentPage = 1;
        $scope.getBatch = [];

        $scope.userPrivileges = "";
        var pageid = $stateParams.pageId;
        var privlgs = JSON.parse(localStorage.getItem("privileges"));
        if (privlgs !== null && privlgs.length > 0) {
            for (var i = 0; i < privlgs.length; i++) {
                if (privlgs[i].pageId == pageid) {
                    $scope.userPrivileges = privlgs[i];
                }
            }
        }

        var paginationformasters;
        var ivrmcofigsettings = JSON.parse(localStorage.getItem("ivrmgeneralconfiglist"));
        if (ivrmcofigsettings != null) {
            if (ivrmcofigsettings.length > 0) {
                paginationformasters = ivrmcofigsettings[0].ivrmgC_PagePagination;
            } else {
                paginationformasters = 10;
            }
        } else {
            paginationformasters = 10;
        }


        $scope.ddate = {};
        $scope.ddate = new Date();
        $scope.amsT_Date = $scope.ddate;

        var logopath = "";
        $scope.usrname = localStorage.getItem('username');
        var admfigsettings = JSON.parse(localStorage.getItem("admfigsettings"));
        if (admfigsettings !== null) {
            if (admfigsettings.length > 0) {
                logopath = admfigsettings[0].asC_Logo_Path;
            }
        } else {
            logopath = "";
        }

        $scope.imgname = logopath;
        var configsettings = JSON.parse(localStorage.getItem("feeconfigsettings"));

        $scope.loaddata = function () {
            var pageid = 2;
            var dataf, datat;
            apiService.getURI("CollegeAttendanceEntryNew/getalldetails", pageid).then(function (promise) {
                $scope.getYear = promise.getYear;

                for (var i = 0; i < $scope.getYear.length; i++) {
                    name = $scope.getYear[i].asmaY_Id;
                    for (var j = 0; j < $scope.getYear.length; j++) {
                        if (name == $scope.getYear[j].asmaY_Id) {
                            $scope.getYear[0].Selected = true;
                            //$scope.asmaY_Id = $scope.getYear[0].asmaY_Id;

                            dataf = new Date($scope.getYear[j].asmaY_From_Date);
                            datat = new Date($scope.getYear[j].asmaY_To_Date);
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

                            $scope.minDatedtf = new Date(
                                $scope.fDate.getFullYear(),
                                $scope.fDate.getMonth(),
                                $scope.fDate.getDate());

                            $scope.maxDatedtf = new Date(
                                $scope.dd.getFullYear(),
                                $scope.dd.getMonth(),
                                $scope.dd.getDate());

                            $scope.minDatemf = new Date(
                                $scope.fDate.getFullYear(),
                                $scope.fDate.getMonth(),
                                $scope.fDate.getDate());

                            $scope.maxDatemf = new Date(
                                $scope.dd.getFullYear(),
                                $scope.dd.getMonth(),
                                $scope.dd.getDate());

                            $scope.minDatedf = new Date(
                                $scope.fDate.getFullYear(),
                                $scope.fDate.getMonth(),
                                $scope.fDate.getDate());

                            $scope.maxDatedf = new Date(
                                $scope.dd.getFullYear(),
                                $scope.dd.getMonth(),
                                $scope.dd.getDate());
                        }
                    }
                }

                $scope.getBranch = promise.getBranch;

                if (promise.message !== null && promise.message !== "") {
                    swal(promise.message);
                } else {
                    $scope.subjectlist = promise.subjectlist;
                    $scope.getPeriod = promise.getPeriod;
                }
            });
        };

        $scope.searchchkbx = "";
        $scope.filterchkbx = function (obj) {
            return angular.lowercase(obj.amB_BranchName).indexOf(angular.lowercase($scope.searchchkbx)) >= 0;
        };

        //------- On click Check box
        $scope.chnagecheckbox = function () {

            $scope.amcO_Id = "";
            $scope.amB_Id = "";
            $scope.amsE_Id = "";
            $scope.acmS_Id = "";
            $scope.ismS_Id = "";
            $scope.acaB_Id = "";
            $scope.period = 'Regular';
            $scope.getStudentdetails = [];
            $scope.albumNameArraycolumn = [];

        };

        // ON CLICK BRANCH //
        $scope.addbranch = function () {
            $scope.selectedbranchlist = [];
            $scope.subjectlist = [];
            angular.forEach($scope.getBranch, function (dd) {
                if (dd.selected === true) {
                    $scope.selectedbranchlist.push({ AMB_Id: dd.amB_Id });
                }
            });
            if ($scope.selectedbranchlist.length > 0) {
                $scope.getsubjects();

            }
        };

        // GET SUBJECTS LIST
        $scope.getsubjects = function () {
            var data = {
                "selectedbranchlist": $scope.selectedbranchlist,
                "ASMAY_Id": $scope.asmaY_Id
            };

            apiService.create("CollegeAttendanceEntryNew/getsubjectslist", data).then(function (promise) {
                if (promise !== null) {
                    $scope.subjectlist = promise.subjectlist;
                }
            });
        };

        //--------Subject Change
        $scope.onSubjectchange = function () {
            $scope.acaB_Id = "";
            $scope.getBatch = [];

            $scope.getStudentdetails = [];

            angular.forEach($scope.getPeriod, function (chk) {
                if (chk.selected) {
                    chk.selected = false;
                }
            });
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "ISMS_Id": $scope.ismS_Id,
                "selectedbranchlist": $scope.selectedbranchlist
            };
            apiService.create("CollegeAttendanceEntryNew/getBatchdata", data).then(function (promise) {
                $scope.getBatch = promise.getBatch;
            });
        };

        //----------------------- category Grid   
        $scope.get_Studentdetails = function () {
            $scope.submitted = true;
            if ($scope.acaB_Id == undefined || $scope.acaB_Id == null || $scope.acaB_Id == "") {
                $scope.acaB_Id = 0;
            }

            if ($scope.myForm.$valid) {

                $scope.albumNameArraycolumn = [];
                angular.forEach($scope.getPeriod, function (role) {
                    if (!!role.selected) $scope.albumNameArraycolumn.push(role);
                });

                var pageid = 2;

                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ISMS_Id": $scope.ismS_Id,
                    "ACSA_Regular_Extra": $scope.period,
                    "ACSA_AttendanceDate": new Date($scope.amsT_Date).toDateString(),
                    "ClgAttendanceEntryTTPeriodTempDTO": $scope.albumNameArraycolumn,
                    "ACAB_Id": $scope.acaB_Id,
                    "selectedbranchlist": $scope.selectedbranchlist
                };

                apiService.create("CollegeAttendanceEntryNew/getStudentdata", data).then(function (promise) {

                    if (promise.message === null) {

                        $scope.tempattary = [];
                        $scope.tempattary1 = [];

                        $scope.getStudentdetails = promise.getStudentdetails;

                        var temp_list = [];
                        var sub_list = [];
                        for (var x = 0; x < promise.getStudentdetails.length; x++) {
                            var stu_id = promise.getStudentdetails[x].amcsT_Id;
                            var stu_subj_list = [];
                            angular.forEach(promise.getStudentdetails, function (opq) {
                                if (opq.amcsT_Id == stu_id) {
                                    stu_subj_list.push({
                                        amcsT_Id: opq.amcsT_Id, amcsT_FirstName: opq.amcsT_FirstName, amcsT_AdmNo: opq.amcsT_AdmNo,
                                        acysT_RollNo: opq.acysT_RollNo, ttmP_Id: opq.ttmP_Id, amcO_Id: opq.amcO_Id, amB_Id: opq.amB_Id, amsE_Id: opq.amsE_Id,
                                        acmS_Id: opq.acmS_Id, acsA_Id: opq.acsA_Id, acsaS_Id: opq.acsaS_Id,
                                        pdays: opq.pdays
                                    });
                                }
                            })
                            if (temp_list.length == 0) {
                                temp_list.push({
                                    amcsT_Id: promise.getStudentdetails[x].amcsT_Id, amcsT_FirstName: promise.getStudentdetails[x].amcsT_FirstName,
                                    amcsT_AdmNo: promise.getStudentdetails[x].amcsT_AdmNo, acysT_RollNo: promise.getStudentdetails[x].acysT_RollNo,
                                    amcO_Id: promise.getStudentdetails[x].amcO_Id, amB_Id: promise.getStudentdetails[x].amB_Id, amsE_Id: promise.getStudentdetails[x].amsE_Id,
                                    acmS_Id: promise.getStudentdetails[x].acmS_Id, sub_list: stu_subj_list
                                });
                            }
                            else if (temp_list.length > 0) {
                                var already_cnt = 0;
                                angular.forEach(temp_list, function (opq1) {
                                    if (opq1.amcsT_Id == stu_id) {
                                        already_cnt += 1;
                                    }
                                })
                                if (already_cnt == 0) {
                                    temp_list.push({
                                        amcsT_Id: promise.getStudentdetails[x].amcsT_Id, amcsT_FirstName: promise.getStudentdetails[x].amcsT_FirstName,
                                        amcsT_AdmNo: promise.getStudentdetails[x].amcsT_AdmNo, acysT_RollNo: promise.getStudentdetails[x].acysT_RollNo,
                                        amcO_Id: promise.getStudentdetails[x].amcO_Id, amB_Id: promise.getStudentdetails[x].amB_Id,
                                        amsE_Id: promise.getStudentdetails[x].amsE_Id, acmS_Id: promise.getStudentdetails[x].acmS_Id,
                                        sub_list: stu_subj_list
                                    });
                                }
                            }
                        }
                        var savecount = 0;
                        var updatecount = 0;
                        var deletecount = 0;

                        console.log(temp_list);
                        if ($scope.getStudentdetails.length > 0) {
                            angular.forEach(temp_list, function (lis) {
                                angular.forEach(lis.sub_list, function (ee) {
                                    if (promise.check_attendance_entrytype === "A") {
                                        if (ee.pdays == 0) {
                                            ee.Selected = true;
                                            savecount = ee.acsA_Id;
                                        } else {
                                            ee.Selected = false;
                                            savecount = ee.acsA_Id;
                                        }
                                    } else if (promise.check_attendance_entrytype === "P") {

                                        if (ee.pdays > 0) {
                                            ee.Selected = true;
                                            savecount = ee.acsA_Id;
                                        } else {
                                            ee.Selected = false;
                                            savecount = ee.acsA_Id;
                                        }
                                    } else {
                                        ee.Selected = false;
                                    }
                                });
                            });
                            $scope.update = savecount;

                            //if (promise.check_attendance_entrytype == "A") {
                            //    angular.forEach($scope.getStudentdetails, function (dd) {
                            //        if (dd.pdays == 0) {
                            //            dd.Selected = true;
                            //        } else {
                            //            dd.Selected = false;
                            //        }
                            //    })
                            //} else if (promise.check_attendance_entrytype == "P") {
                            //    angular.forEach($scope.getStudentdetails, function (dd) {
                            //        if (dd.pdays == 0) {
                            //            dd.Selected = false;
                            //        } else {
                            //            dd.Selected = true;
                            //        }
                            //    })
                            //} else {
                            //    angular.forEach($scope.getStudentdetails, function (dd) {
                            //        dd.Selected = false;
                            //    })
                            //}
                            $scope.getStudentdetails = temp_list;
                            console.log($scope.getStudentdetails);

                        }
                        else {
                            swal("No Records Found");
                        }
                    } else {
                        swal(promise.message);
                        $state.reload();
                    }
                });
            } else {
                $scope.submitted = true;
            }
        };

        $scope.toggleAll = function () {
            angular.forEach($scope.getStudentdetails, function (subj) {
                subj.xyz = $scope.all;
            });
        };

        $scope.optionToggled = function () {
            $scope.all = $scope.getStudentdetails.every(function (itm) { return itm.xyz; });
        };

        //------------------------------Save
        $scope.saveatt = function (objas) {
            $scope.submitted = true;
            if ($scope.myForm.$valid) {
                $scope.albumNameArraycolumn1 = [];
                angular.forEach($scope.getPeriod, function (role) {
                    if (!!role.selected) $scope.albumNameArraycolumn1.push(role);
                });

                if ($scope.acaB_Id != undefined) {
                    $scope.acaB_Id = $scope.acaB_Id;
                } else {
                    $scope.acaB_Id = 0;
                }

                var data = {
                    "ASMAY_Id": $scope.asmaY_Id,
                    "ISMS_Id": $scope.ismS_Id,
                    "ClgAttendanceEntryTempDTO": $scope.getStudentdetails,
                    "ACSA_Regular_Extra": $scope.period,
                    "ACSA_AttendanceDate": new Date($scope.amsT_Date).toDateString(),
                    "ClgAttendanceEntryTTPeriodTempDTO": $scope.albumNameArraycolumn1,
                    "ACAB_Id": $scope.acaB_Id,
                    "selectedbranchlist": $scope.selectedbranchlist
                };

                apiService.create("CollegeAttendanceEntryNew/saveatt", data).then(function (promise) {

                    if (promise.returnval === true) {
                        if (promise.message === "Add") {
                            swal("Record Saved / Update Successfully");
                        } else {
                            swal("Record Saved / Update Successfully");
                        }
                    }
                    else if (promise.returnval === false) {
                        if (promise.message === "Add") {
                            swal("Failed To Save / Update Record");
                        } else {
                            swal("Failed To Save / Update Record");
                        }
                    }
                    else {
                        swal("Something Went Wrong.. Contact With Administrator");
                    }
                    $scope.getStudentdetails = [];
                    $scope.albumNameArraycolumn = [];
                    $scope.sub_list = [];
                    //$state.reload();
                });
            }
            else {
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
            $scope.sortReverse = ($scope.sortKey == key) ? !$scope.sortReverse : $scope.sortReverse;
            $scope.sortKey = key;
        };

        $scope.IsHidden = true;
        $scope.ShowHide = function () {
            $scope.IsHidden = $scope.IsHidden ? false : true;
        };

        $scope.search_box = function () {
            if ($scope.searchValue !== "" || $scope.searchValue !== null) {
                $scope.searc_button = false;
            }
            else {
                $scope.searc_button = true;
            }
        };

        $scope.albumNameArraycolumn = [];
        //disable the sundays in calender
        $scope.onlyWeekendsPredicate = function (date) {
            var day = date.getDay();
            return day === 1 || day === 2 || day === 3 || day === 4 || day === 5 || day === 6;

        };

        $scope.addColumn2 = function (role1) {
            $scope.all2 = $scope.getPeriod.every(function (itm) { return itm.selected; });
            if (role1.selected === true) {
                $scope.albumNameArraycolumn.push(role1);
                $scope.columnsTest.push(role1);
                //return $filter('filter')($scope.newuser1, { checked: true });
            }
            else {
                var som = $scope.albumNameArraycolumn.indexOf(role1);
                $scope.columnsTest.splice($scope.albumNameArraycolumn.indexOf(role1), 1);
                $scope.albumNameArraycolumn.splice($scope.albumNameArraycolumn.indexOf(role1), 1);
                //return $filter('filter')($scope.newuser1, { checked: true });
            }
        };

        $scope.isOptionsRequired1 = function () {
            return !$scope.getPeriod.some(function (options1) {
                return options1.selected;
            });
        };

        $scope.isOptionsRequired11 = function () {
            return !$scope.getBranch.some(function (options1) {
                return options1.selected;
            });
        };

        $scope.delete = function (objas, SweetAlert) {

            var mgs = "";
            var confirmmgs = "";
            var config = {
                headers: {
                    'Content-Type': 'application/json;'
                }
            }

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

                        $scope.albumNameArraycolumn1 = [];
                        angular.forEach($scope.getPeriod, function (role) {
                            if (!!role.selected) $scope.albumNameArraycolumn1.push(role);
                        });

                        if ($scope.acaB_Id != undefined) {
                            $scope.acaB_Id = $scope.acaB_Id;
                        } else {
                            $scope.acaB_Id = 0;
                        }
                        var data = {
                            "ASMAY_Id": $scope.asmaY_Id,
                            "AMCO_Id": objas.amcO_Id,
                            "AMB_Id": objas.amB_Id,
                            "AMSE_Id": objas.amsE_Id,
                            "ACMS_Id": objas.acmS_Id,
                            "ACSA_Id": objas.acsA_Id,
                            "ACSA_AttendanceDate": new Date(objas.acsA_AttendanceDate).toDateString(),
                            "Multibranch": $scope.multibranch
                        };

                        apiService.create("CollegeAttendanceEntryNew/delete", data).
                            then(function (promise) {

                                if (promise.returnval === true) {
                                    swal("Record Delected Successfully");
                                } else {
                                    swal("Failed To Delect Record");
                                }
                                if (promise.getpreviewdata.length > 0) {
                                    $scope.details = promise.getpreviewdata;
                                    $('#myModal').modal('show');
                                } else {
                                    swal("No Records Found");
                                }
                            });
                    }
                    else {
                        swal("Record Delection Cancelled");
                    }
                    //$state.reload();
                });
        };

        $scope.getsaveddatepreview = function () {
            $scope.search = '';
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "selectedbranchlist": $scope.selectedbranchlist
            };

            apiService.create("CollegeAttendanceEntryNew/getsaveddatepreview", data).then(function (promise) {
                if (promise !== null) {
                    if (promise.getpreviewdata !== null) {
                        if (promise.getpreviewdata.length > 0) {
                            $scope.details = promise.getpreviewdata;
                            $('#myModal').modal('show');
                        } else {
                            swal("No Records Found");
                        }
                    } else {
                        swal("No Records Found");
                    }
                } else {
                    swal("No Records Found");
                }
            });
        };

        $scope.search = '';
        $scope.searchvalue = function (obj) {
            return ($filter('date')(obj.acsA_AttendanceDate, 'dd/MM/yyyy').indexOf($scope.search) >= 0)
                || (angular.lowercase(obj.ismS_SubjectName)).indexOf(angular.lowercase($scope.search)) >= 0
                || (angular.lowercase(obj.ttmP_PeriodName)).indexOf(angular.lowercase($scope.search)) >= 0
                || (angular.lowercase(obj.acsA_Regular_Extra)).indexOf(angular.lowercase($scope.search)) >= 0
                || (angular.lowercase(obj.employeename)).indexOf(angular.lowercase($scope.search)) >= 0
                || (angular.lowercase(obj.amcO_CourseName)).indexOf(angular.lowercase($scope.search)) >= 0
                || (angular.lowercase(obj.amB_BranchName)).indexOf(angular.lowercase($scope.search)) >= 0
                || (angular.lowercase(obj.amsE_SEMName)).indexOf(angular.lowercase($scope.search)) >= 0
                || (angular.lowercase(obj.acmS_SectionName)).indexOf(angular.lowercase($scope.search)) >= 0
                || (JSON.stringify(obj.totalPresent)).indexOf($scope.search) >= 0
                || (JSON.stringify(obj.totalabsent)).indexOf($scope.search) >= 0
                || (JSON.stringify(obj.totalcount)).indexOf($scope.search) >= 0;
        };

        //-----------Course Change
        $scope.onCoursechange = function () {

            $scope.acmS_Id = "";
            $scope.amsE_Id = "";
            $scope.amB_Id = "";
            $scope.period = "Regular";
            $scope.ismS_Id = "";
            $scope.acaB_Id = "";
            $scope.getBatch = [];


            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "AMCO_Id": $scope.amcO_Id,
                "Multibranch": $scope.multibranch
            };
            apiService.create("CollegeAttendanceEntryNew/getBranchdata", data).
                then(function (promise) {
                    if ($scope.multibranch == 1) {
                        $scope.getSemester = promise.getSemester;
                    } else {
                        $scope.getBranch = promise.getBranch;
                    }
                });
        };

        //-----------Branch Change
        $scope.onBranchchange = function () {

            $scope.acmS_Id = "";
            $scope.amsE_Id = "";
            $scope.period = "Regular";
            $scope.ismS_Id = "";
            $scope.acaB_Id = "";
            $scope.getBatch = [];

            var ambid = 0;
            if ($scope.multibranch == 1) {
                ambid = 0;
            } else {
                ambid = $scope.amB_Id;
            }

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "AMCO_Id": $scope.amcO_Id,
                "AMB_Id": ambid,
                "Multibranch": $scope.multibranch
            };

            apiService.create("CollegeAttendanceEntryNew/getSemesterdata", data).
                then(function (promise) {
                    if ($scope.multibranch == 1) {
                        $scope.getSection = promise.getSection;
                    } else {
                        $scope.getSemester = promise.getSemester;
                    }
                });
        };

        //---- Semester Change
        $scope.onSemchange = function () {

            $scope.acmS_Id = "";
            $scope.period = "Regular";
            $scope.ismS_Id = "";
            $scope.acaB_Id = "";
            $scope.getBatch = [];


            var ambid = 0;
            if ($scope.multibranch == 1) {
                ambid = 0;
            } else {
                ambid = $scope.amB_Id;
            }
            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "AMCO_Id": $scope.amcO_Id,
                "AMB_Id": ambid,
                "AMSE_Id": $scope.amsE_Id,
                "Multibranch": $scope.multibranch
            };
            apiService.create("CollegeAttendanceEntryNew/getSectiondata", data).
                then(function (promise) {
                    $scope.getSection = promise.getSection;
                });
        };

        //--------Section Change
        $scope.onSectionchange = function () {

            $scope.period = "Regular";
            $scope.ismS_Id = "";
            $scope.acaB_Id = "";
            $scope.getBatch = [];

            var ambid = 0;
            if ($scope.multibranch == 1) {
                ambid = 0;
            } else {
                ambid = $scope.amB_Id;
            }

            var data = {
                "ASMAY_Id": $scope.asmaY_Id,
                "AMCO_Id": $scope.amcO_Id,
                "AMB_Id": ambid,
                "AMSE_Id": $scope.amsE_Id,
                "ACMS_Id": $scope.acmS_Id,
                "Multibranch": $scope.multibranch
            };
            apiService.create("CollegeAttendanceEntryNew/getSubjdata", data).
                then(function (promise) {
                    $scope.ismS_Id = "";
                    $scope.acaB_Id = "";
                    $scope.getSubject = promise.getSubject;
                });
        };
    }
})();